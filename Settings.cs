using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Xml.Serialization;
using System.Windows.Forms;
using System.Runtime.CompilerServices;

namespace PhotoToCSV
{
    /// <summary>
    /// Settings values.
    /// Implement INotifyPropertyChanged.
    /// Persist settings to xml file.
    /// </summary>
    /// <remarks>
    /// <para>
    /// You should create only once instance of this class. Pass this instance
    /// to any class or method that needs to access the settings.
    /// </para>
    /// </remarks>
    public class Settings : SimpleSettings.SettingsBase
    {
        // Properties that are serialized, and trigger the PropertyChanged events.

        private Point formMainLocationValue;
        public Point FormMainLocation
        {
            get { return formMainLocationValue; }
            set { SetProperty(ref formMainLocationValue, value, true); }
        }

        private Size formMainSizeValue;
        public Size FormMainSize
        {
            get { return formMainSizeValue; }
            set { SetProperty(ref formMainSizeValue, value, true); }
        }

        private string inputFolderValue = "";
        public string InputFolder
        {
            get { return inputFolderValue; }
            set { SetProperty(ref inputFolderValue, value, true); }
        }

        private string outputPhotoCSVFilenameValue = "";
        public string OutputPhotoCSVFilename
        {
            get { return outputPhotoCSVFilenameValue; }
            set { SetProperty(ref outputPhotoCSVFilenameValue, value, true); }
        }

        private string outputEncounterCSVFilenameValue = "";
        public string OutputEncounterCSVFilename
        {
            get { return outputEncounterCSVFilenameValue; }
            set { SetProperty(ref outputEncounterCSVFilenameValue, value, true); }
        }

        // Properties which are not persisted, but trigger property changed.
        // none

        // Methods
        // Load, Save, and SaveIfChanged methods must be defined.
        public static Settings Load()
        {
            return Load<Settings>();
        }
        public void Save()
        {
            base.Save(typeof(Settings));
        }
        public void SaveIfChanged()
        {
            base.SaveIfChanged(typeof(Settings));
        }
    }
}
