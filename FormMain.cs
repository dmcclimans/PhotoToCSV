using System.Collections.Immutable;
using System.Data;
using System.Formats.Asn1;
using FileHelpers;
using CenteredMessageBox;
using AudioSplit;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace PhotoToCSV
{
    public partial class FormMain : Form
    {
        private Settings Settings { get; set; } = new Settings();

        private List<string> UnrecognizedSpecies { get; set; } = new List<string>();

        public FormMain()
        {
            InitializeComponent();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            MessageBoxEx.Caption = "PhotoToCSV";

            try
            {
                Settings = Settings.Load();
            }
            catch (Exception ex)
            {
                // Use MessageBox here, rather than MessageBoxEx, because the form isn't loaded yet.
                // MessageBoxEx will center relative to the form's position, which is 0,0, so it will
                // display the messagebox in the upper left corner. MessageBox will display in the
                // center of the screen, which is better when no form is visible.
                MessageBox.Show(ex.Message, "PhotoToCSV", MessageBoxButtons.OK);
                Settings = new Settings();
            }

            if (Settings.FormMainLocation.X != 0 ||
                  Settings.FormMainLocation.Y != 0)
            {
                this.Location = Settings.FormMainLocation;
            }
            if (Settings.FormMainSize.Height != 0 ||
                  Settings.FormMainSize.Width != 0)
            {
                this.Size = Settings.FormMainSize;
            }

            txtInputFolder.DataBindings.Add("Text", Settings, nameof(Settings.InputFolder));
            txtOutputFile1.DataBindings.Add("Text", Settings, nameof(Settings.OutputPhotoCSVFilename));
            txtOutputFile2.DataBindings.Add("Text", Settings, nameof(Settings.OutputEncounterCSVFilename));
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized || this.WindowState == FormWindowState.Maximized)
            {
                Settings.FormMainLocation = this.RestoreBounds.Location;
                Settings.FormMainSize = this.RestoreBounds.Size;
            }
            else
            {
                Settings.FormMainLocation = this.Location;
                Settings.FormMainSize = this.Size;
            }

            try
            {
                Settings.SaveIfChanged();
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(this, ex.Message, MessageBoxButtons.OK);
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormAbout dlg = new FormAbout(Settings);
            dlg.ShowDialog(this);
        }

        private void btnBrowseInputFolder_Click(object sender, EventArgs e)
        {
            string path = Settings.InputFolder;
            if (!string.IsNullOrEmpty(path))
            {
                // Sometimes the existing output folder has been deleted. Go up one level to see
                // if that is there. If so, use it for the initial directory.
                // If the initial directory doesn't exist, Windows will start from Documents.
                if (!Directory.Exists(path))
                {
                    // Remove any trailing backslash
                    path = path.TrimEnd(new[] { '/', '\\' });
                    DirectoryInfo? parentDir = Directory.GetParent(path);
                    if (parentDir != null)
                    {
                        path = parentDir.FullName;
                    }
                }
            }

            FolderBrowserDialog dlg = new FolderBrowserDialog();

            if (!string.IsNullOrEmpty(path))
            {
                dlg.InitialDirectory = path;
            }

            DialogResult result = dlg.ShowDialog();
            if (result == DialogResult.OK)
            {
                Settings.InputFolder = dlg.SelectedPath;
            }

        }

        private void btnBrowseOutputFile1_Click(object sender, EventArgs e)
        {
            Settings.OutputPhotoCSVFilename = BrowseOutputFile(Settings.OutputPhotoCSVFilename);
        }

        private void btnBrowseOutputFile2_Click(object sender, EventArgs e)
        {
            Settings.OutputEncounterCSVFilename = BrowseOutputFile(Settings.OutputEncounterCSVFilename);
        }

        private string BrowseOutputFile(string initialFilePath)
        {
            string path = Settings.OutputPhotoCSVFilename;
            string initialDirectory = "";
            if (!string.IsNullOrEmpty(initialFilePath))
            {
                initialDirectory = Path.GetDirectoryName(initialFilePath) ?? "";
                if (!Directory.Exists(initialDirectory))
                {
                    // Sometimes the existing output folder has been deleted. Go up one level to see
                    // if that is there. If so, use it for the initial directory.
                    // Remove any trailing backslash
                    initialDirectory = initialDirectory.TrimEnd(new[] { '/', '\\' });
                    DirectoryInfo? parentDir = Directory.GetParent(initialDirectory);
                    if (parentDir != null)
                    {
                        initialDirectory = parentDir.FullName;
                    }
                }
            }

            // If the initial directory doesn't exist, Windows will start from the most recent
            // folder for this application, or from Documents.

            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Title = "Output CSV file, one line per photo / species";
            dlg.Filter = "CSV Files (*.csv)|*.csv|All files (*.*)|*.*";
            dlg.FilterIndex = 1;
            dlg.InitialDirectory = initialDirectory;
            dlg.CheckFileExists = false;

            DialogResult result = dlg.ShowDialog();
            if (result == DialogResult.OK)
            {
                return dlg.FileName;
            }
            return initialFilePath;
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            try
            {
                toolStripStatusLabel1.Text = "Running";

                CheckFilesAndDirectories();

                Cursor.Current = Cursors.WaitCursor;

                // Run exiftool to produce the photo file.
                RunExiftool();

                // Read the file produced by exiftools.
                // photoRecords will contain one element per line of the file.
                FileHelperEngine<PhotoRecord> readEngine = new FileHelperEngine<PhotoRecord>();
                PhotoRecord[] photoRecords = readEngine.ReadFile(Settings.OutputPhotoCSVFilename);

                // Extract the cam number from the keywords file and populate the CamName field of photoRecords
                ProcessKeywords(photoRecords);

                // Sort the input array by CamName, then by DateTimeOriginal
                Array.Sort(photoRecords);

                // Process the photo records array into a list of encounters.
                List<EncounterRecord> encounters = MakeEncounterRecords(photoRecords);

                // Write the encounters file
                FileHelperEngine<EncounterRecord> writeEngine = new FileHelperEngine<EncounterRecord>();
                writeEngine.HeaderText = writeEngine.GetFileHeader();
                writeEngine.WriteFile(Settings.OutputEncounterCSVFilename, encounters);

                Cursor.Current = Cursors.Default;

                // If unrecognized species found, report them.
                if (UnrecognizedSpecies.Count > 0)
                {
                    StringBuilder sb = new StringBuilder("The following species were found but are not in the Species list");
                    sb.AppendLine();
                    foreach (string s in UnrecognizedSpecies)
                    {
                        sb.Append("\t").AppendLine(s);
                    }
                    MessageBoxEx.Show(this, sb.ToString());
                }

                //MessageBoxEx.Show(this, "Done", MessageBoxButtons.OK);
                toolStripStatusLabel1.Text = "Done";
            }
            catch (Exception ex)
            {
                toolStripStatusLabel1.Text = "";
                Cursor.Current = Cursors.Default;
                if (!string.IsNullOrEmpty(ex.Message))
                {
                    MessageBoxEx.Show(this, ex.Message, MessageBoxButtons.OK);
                }
            }
        }

        private void CheckFilesAndDirectories()
        {
            if (!Directory.Exists(Settings.InputFolder))
            {
                throw new Exception("The input folder does not exist.");
            }
            if (IsDirectoryEmpty(Settings.InputFolder))
            {
                throw new Exception("The input folder is empty.");
            }
            if (File.Exists(Settings.OutputPhotoCSVFilename))
            {
                if (File.Exists(Settings.OutputEncounterCSVFilename))
                {
                    DialogResult result = MessageBoxEx.Show(this,
                        "Both output files exist and will be overwritten.",
                        MessageBoxButtons.OKCancel);
                    if (result == DialogResult.Cancel)
                    {
                        throw new Exception("");
                    }
                }
                else
                {
                    DialogResult result = MessageBoxEx.Show(this,
                        String.Format("Output photo CSV file {0} exists and will be overwritten.", Settings.OutputPhotoCSVFilename),
                        MessageBoxButtons.OKCancel);
                    if (result == DialogResult.Cancel)
                    {
                        throw new Exception("");
                    }
                }
            }
            else if (File.Exists(Settings.OutputEncounterCSVFilename))
            {
                DialogResult result = MessageBoxEx.Show(this,
                    String.Format("Output encounter CSV file {0} exists and will be overwritten.", Settings.OutputEncounterCSVFilename),
                    MessageBoxButtons.OKCancel);
                if (result == DialogResult.Cancel)
                {
                    throw new Exception("");
                }
            }
        }

        private bool IsDirectoryEmpty(string folderPpath)
        {
            if (!Directory.Exists(folderPpath))
                return true;
            return !Directory.EnumerateFileSystemEntries(folderPpath).Any();
        }

        private void RunExiftool()
        {
            ExecuteCommand cmd = new ExecuteCommand();
            string cmdString =
                "exiftool" +                                        // Run exiftool
                " -q -q" +                                          // Suppress info messages and warnings
                " -csv" +                                           // Write csv file
                " -f" +                                             // Force all columns to exist even if empty.
                " -api \"missingtagvalue^=\"" +                     // Write empty string for empty fields. The quoted missingtagvalue^=
                                                                    // is an exiftool workaround to force an option to an empty string.
                " -d \"%Y/%m/%d %H:%M:%S\"" +                       // Format used for date/time fields
                " -DateTimeOriginal -Keywords -Description" +       // Fields to write
                " >\"" + Settings.OutputPhotoCSVFilename + "\"" +   // Output file
                " \"" + Settings.InputFolder + "\"";                // Input folder
            string s = cmd.ExecuteDOSCommandSync(cmdString);
            if (!string.IsNullOrWhiteSpace(s))
            {
                throw new Exception(string.Format("Error running Exiftool: {0}", s));
            }
        }

        private void ProcessKeywords(PhotoRecord[] photoRecords)
        {
            UnrecognizedSpecies = new List<string>();
            foreach (PhotoRecord rec in photoRecords)
            {
                int qty = 0;
                string[] keywords = rec.Keywords.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
                foreach (string keyword in keywords)
                {
                    if (keyword.Length == 0)
                    {
                        // Shouldn't happen, ignore it if it does.
                    }
                    else if (string.Equals(keyword, "dup", StringComparison.OrdinalIgnoreCase))
                    {
                        rec.Dup = true;
                    }
                    else if (string.Equals(keyword.SubstringWithMaxLength(0, 3), "cam", StringComparison.OrdinalIgnoreCase) &&
                        int.TryParse(keyword.AsSpan(3), out _))
                    {
                        rec.CamName = keyword.Substring(3);
                    }
                    else if (string.Equals(keyword, "DirToward", StringComparison.OrdinalIgnoreCase))
                    {
                        rec.Direction = "Toward";
                    }
                    else if (string.Equals(keyword, "DirAway", StringComparison.OrdinalIgnoreCase))
                    {
                        rec.Direction = "Away";
                    }
                    else if (string.Equals(keyword, "DirUncertain", StringComparison.OrdinalIgnoreCase))
                    {
                        rec.Direction = "Uncertain";
                    }
                    else if (string.Equals(keyword, "f", StringComparison.OrdinalIgnoreCase) ||
                             string.Equals(keyword, "l", StringComparison.OrdinalIgnoreCase))
                    {
                        // Ignore F and L keywords
                    }
                    else if (char.ToUpper(keyword[0]).Equals('M') &&
                             int.TryParse(keyword.Substring(1), out qty))
                    {
                        rec.MaleCount = qty;
                    }
                    else if (char.ToUpper(keyword[0]).Equals('F') &&
                             int.TryParse(keyword.Substring(1), out qty))
                    {
                        rec.FemaleCount = qty;
                    }
                    else if (char.ToUpper(keyword[0]).Equals('J') &&
                             int.TryParse(keyword.Substring(1), out qty))
                    {
                        rec.JuvenileCount = qty;
                    }
                    else if (char.ToUpper(keyword[0]).Equals('U') &&
                             int.TryParse(keyword.Substring(1), out qty))
                    {
                        rec.UnknownCount = qty;
                    }
                    else if (int.TryParse(keyword, out qty) && rec.UnknownCount == null)
                    {
                        // Keywords that are integers are treated as "unknown". But they won't overwrite a
                        // previous Unknown quantity.
                        if (rec.UnknownCount == null)
                        {
                            rec.UnknownCount = qty;
                        }
                    }
                    else
                    {
                        // Treat the keyword as a species
                        if (!rec.Species.Contains(keyword, StringComparer.OrdinalIgnoreCase))
                        {
                            // Add it to the species list for this photo.
                            rec.Species.Add(keyword);

                            // Check it against our species list
                            if (Settings.SpeciesList.Count > 0)
                            {
                                if (!Settings.SpeciesList.Contains(keyword, StringComparer.OrdinalIgnoreCase) && 
                                    !UnrecognizedSpecies.Contains(keyword, StringComparer.OrdinalIgnoreCase))
                                {
                                    UnrecognizedSpecies.Add(keyword);
                                }
                            }
                        }
                    }
                }
            }
        }

        private List<EncounterRecord> MakeEncounterRecords(PhotoRecord[] photoRecords)
        {
            List<EncounterRecord> encounters = new List<EncounterRecord>();

            // startIndex is the first record in an encounter. It's a non-dup record.
            // endIndex is the index one beyond the end of an encounter. It's a non-dup 
            // record, or beyond the end of the photoRecords array.
            int startIndex = 0;
            int endIndex = 1;

            while (startIndex < photoRecords.Length)
            {
                // Find the end of the encounter
                while (endIndex < photoRecords.Length && photoRecords[endIndex].Dup)
                {
                    endIndex++;
                }

                // Iterate over the photo records that are part of this encounter.
                // Get the counts of individuals. For each type (Male, Female, Juvenile,
                // Unknown), record the count the first time one is seen.
                // Similarly, the direction is the first one seen
                // And the notes is the first description seen.
                List<string> encounterSpeciesList = new List<string>();
                int? maleCount = null;
                int? femaleCount = null;
                int? juvenileCount = null;
                int? unknownCount = null;
                string? direction = null;
                string? notes = null;
                for (int i = startIndex; i < endIndex; i++)
                {
                    encounterSpeciesList.AddRange(photoRecords[i].Species);
                    if (photoRecords[i].MaleCount != null && maleCount == null)
                    {
                        maleCount = photoRecords[i].MaleCount;
                    }
                    if (photoRecords[i].FemaleCount != null && femaleCount == null)
                    {
                        femaleCount = photoRecords[i].FemaleCount;
                    }
                    if (photoRecords[i].JuvenileCount != null && juvenileCount == null)
                    {
                        juvenileCount = photoRecords[i].JuvenileCount;
                    }
                    if (photoRecords[i].UnknownCount != null && unknownCount == null)
                    {
                        unknownCount = photoRecords[i].UnknownCount;
                    }
                    if (photoRecords[i].Direction != null && direction == null)
                    {
                        direction = photoRecords[i].Direction;
                    }
                    if (!string.IsNullOrWhiteSpace(photoRecords[i].Description) &&
                        notes == null)
                    {
                        notes = photoRecords[i].Description;
                    }
                }
                encounterSpeciesList = encounterSpeciesList.Distinct().ToList();

                // Add one record for each species
                for (int speciesIndex = 0; speciesIndex < encounterSpeciesList.Count; speciesIndex++)
                {
                    EncounterRecord encounterRec = new EncounterRecord();
                    PhotoRecord startRec = photoRecords[startIndex];
                    PhotoRecord lastRec = photoRecords[endIndex - 1];
                    encounterRec.CamName = startRec.CamName;
                    encounterRec.DateTimeFirstImage = startRec.DateTimeOriginal;
                    encounterRec.DateTimeLastImage = lastRec.DateTimeOriginal;
                    encounterRec.DurationSecs = string.Format("=Round((C{0}-B{0})*60*60*24,0)", encounters.Count + 2);
                    if (speciesIndex == 0)
                    {
                        // Number of images is set only for first species
                        encounterRec.ImageCount = endIndex - startIndex;
                    }
                    encounterRec.Species = encounterSpeciesList[speciesIndex];
                    encounterRec.MaleCount = maleCount;
                    encounterRec.FemaleCount = femaleCount;
                    encounterRec.JuvenileCount = juvenileCount;
                    encounterRec.UnknownCount = unknownCount;
                    encounterRec.TotalCount = string.Format("=SUM(G{0}:J{0})", encounters.Count + 2);
                    encounterRec.Direction = direction;
                    encounterRec.Notes = notes;   

                    // Add the encounter record to the list.
                    encounters.Add(encounterRec);
                }

                // Move to next encounter
                startIndex = endIndex;
                endIndex++;
            }

            return encounters;
        }

        private void loadSpeciesListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.CheckFileExists = true;
            dlg.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                List<string> speciesList = new List<string>();
                try
                {
                    speciesList = File.ReadLines(dlg.FileName).ToList();
                }
                catch (Exception ex)
                {
                    MessageBoxEx.Show(this, ex.Message);
                    return;
                }

                // Remove empty strings.
                // Remove duplicate strings
                speciesList.RemoveAll(s => string.IsNullOrEmpty(s));
                speciesList = speciesList.Distinct().ToList();

                // Check for multi-word species and warn user.
                bool multiword = false;
                foreach (string s in speciesList)
                {
                    if (s.Trim().IndexOf(' ') >= 0)
                    {
                        multiword = true;
                        break;
                    }
                }
                if (multiword)
                {
                    DialogResult result = MessageBoxEx.Show(this,
                        "This list contains multi-word entries. Are you sure you want to use it?",
                        MessageBoxButtons.OKCancel);
                    if (result == DialogResult.Cancel)
                        return;
                }

                Settings.SpeciesList = speciesList;
            }
        }

        private void clearSpeciesListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBoxEx.Show(this,
                "Are you sure you want to clear the species list?",
                MessageBoxButtons.YesNo);
            if (result != DialogResult.Yes)
                return;

            // Do not use Clear(); Set a new value so that the settings are marked as changed and will
            // be written on program exit.
            Settings.SpeciesList = new List<string>();
        }
    }
}