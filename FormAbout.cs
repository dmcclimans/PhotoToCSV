using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PhotoToCSV
{
    public partial class FormAbout : Form
    {
        private Settings Settings { get; set; }

        public FormAbout(Settings settings)
        {
            Settings = settings;
            InitializeComponent();
        }

        private void FormAbout_Load(object sender, EventArgs e)
        {
            lblVersion.Text = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "Unknown";
            CenterToParent();
        }

        private void FormAbout_FormClosing(object sender, FormClosingEventArgs e)
        {
        }
    }
}
