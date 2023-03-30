using System.Collections.Immutable;
using System.Data;
using System.Formats.Asn1;
using FileHelpers;
using CenteredMessageBox;

namespace PhotoToCSV
{
    public partial class FormMain : Form
    {
        private Settings Settings { get; set; } = new Settings();

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

                // TBD: Uncomment this.
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
                "exiftool" +                                    // Run exiftool
                " -q -q" +                                      // Suppress info messages and warnings
                " -csv" +                                       // Write csv file
                " -d \"%Y/%m/%d %H:%M:%S\"" +                   // Format used for date/time fields
                " -DateTimeOriginal -Keywords " +               // Fields to write
                " >\"" + Settings.OutputPhotoCSVFilename + "\"" +   // Output file
                " \"" + Settings.InputFolder + "\"";            // Input folder
            string s = cmd.ExecuteDOSCommandSync(cmdString);
            if (!string.IsNullOrWhiteSpace(s))
            {
                throw new Exception(string.Format("Error running Exiftool: {0}", s));
            }
        }

        private void ProcessKeywords(PhotoRecord[] photoRecords)
        {
            foreach (PhotoRecord rec in photoRecords)
            {
                int qty = 0;
                string[] keywords = rec.Keywords.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
                foreach (string keyword in keywords)
                {
                    if (string.Equals(keyword, "dup", StringComparison.OrdinalIgnoreCase))
                    {
                        rec.Dup = true;
                    }
                    else if (string.Equals(keyword, "f", StringComparison.OrdinalIgnoreCase) ||
                             string.Equals(keyword, "l", StringComparison.OrdinalIgnoreCase))
                    {
                        // Ignore F and L keywords
                    }
                    else if (string.Equals(keyword.Substring(0, 3), "cam", StringComparison.OrdinalIgnoreCase) &&
                        int.TryParse(keyword.AsSpan(3), out _))
                    {
                        rec.CamName = keyword.Substring(3);
                    }
                    else if (int.TryParse(keyword, out qty))
                    {
                        rec.Quantity = qty;
                    }
                    else if (!rec.Species.Contains(keyword))
                    {
                        rec.Species.Add(keyword);
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

                // Get list of species in this encounter
                List<string> encounterSpeciesList = new List<string>();
                for (int i = startIndex; i < endIndex; i++)
                {
                    encounterSpeciesList.AddRange(photoRecords[i].Species);
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
                    // Don't need to set some values because the default is correct.
                    // encounterRec.MaleCount = null;
                    // encounterRec.FemaleCount = null;
                    // encounterRec.JuvenileCount= null;
                    // encounterRec.UnknownCount = null;
                    if (photoRecords[startIndex].Quantity != 0)
                    {
                        encounterRec.UnknownCount = photoRecords[startIndex].Quantity;
                    }
                    encounterRec.TotalCount = string.Format("=SUM(G{0}:J{0})", encounters.Count + 2);
                    // encounterRec.Direction = null;
                    // encounterRec.Notes = null;

                    // Add the encounter record to the list.
                    encounters.Add(encounterRec);
                }

                // Move to next encounter
                startIndex = endIndex;
                endIndex++;
            }

            return encounters;
        }
    }
}