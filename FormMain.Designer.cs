namespace PhotoToCSV
{
    partial class FormMain
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label1 = new Label();
            txtInputFolder = new TextBox();
            btnBrowseInputFolder = new Button();
            label2 = new Label();
            btnBrowseOutputFile1 = new Button();
            txtOutputFile1 = new TextBox();
            btnBrowseOutputFile2 = new Button();
            txtOutputFile2 = new TextBox();
            label3 = new Label();
            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            loadSpeciesListToolStripMenuItem = new ToolStripMenuItem();
            exitToolStripMenuItem = new ToolStripMenuItem();
            helpToolStripMenuItem = new ToolStripMenuItem();
            aboutToolStripMenuItem = new ToolStripMenuItem();
            btnRun = new Button();
            statusStrip1 = new StatusStrip();
            toolStripStatusLabel1 = new ToolStripStatusLabel();
            clearSpeciesListToolStripMenuItem = new ToolStripMenuItem();
            menuStrip1.SuspendLayout();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(13, 52);
            label1.Name = "label1";
            label1.Size = new Size(110, 25);
            label1.TabIndex = 0;
            label1.Text = "Input folder:";
            // 
            // txtInputFolder
            // 
            txtInputFolder.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtInputFolder.Location = new Point(16, 83);
            txtInputFolder.Name = "txtInputFolder";
            txtInputFolder.Size = new Size(655, 31);
            txtInputFolder.TabIndex = 1;
            // 
            // btnBrowseInputFolder
            // 
            btnBrowseInputFolder.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnBrowseInputFolder.Location = new Point(677, 81);
            btnBrowseInputFolder.Name = "btnBrowseInputFolder";
            btnBrowseInputFolder.Size = new Size(112, 34);
            btnBrowseInputFolder.TabIndex = 2;
            btnBrowseInputFolder.Text = "Browse...";
            btnBrowseInputFolder.UseVisualStyleBackColor = true;
            btnBrowseInputFolder.Click += btnBrowseInputFolder_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(13, 137);
            label2.Name = "label2";
            label2.Size = new Size(291, 25);
            label2.TabIndex = 3;
            label2.Text = "Photo CSV file (one line per photo):";
            // 
            // btnBrowseOutputFile1
            // 
            btnBrowseOutputFile1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnBrowseOutputFile1.Location = new Point(677, 163);
            btnBrowseOutputFile1.Name = "btnBrowseOutputFile1";
            btnBrowseOutputFile1.Size = new Size(112, 34);
            btnBrowseOutputFile1.TabIndex = 5;
            btnBrowseOutputFile1.Text = "Browse...";
            btnBrowseOutputFile1.UseVisualStyleBackColor = true;
            btnBrowseOutputFile1.Click += btnBrowseOutputFile1_Click;
            // 
            // txtOutputFile1
            // 
            txtOutputFile1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtOutputFile1.Location = new Point(16, 165);
            txtOutputFile1.Name = "txtOutputFile1";
            txtOutputFile1.Size = new Size(655, 31);
            txtOutputFile1.TabIndex = 4;
            // 
            // btnBrowseOutputFile2
            // 
            btnBrowseOutputFile2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnBrowseOutputFile2.Location = new Point(677, 250);
            btnBrowseOutputFile2.Name = "btnBrowseOutputFile2";
            btnBrowseOutputFile2.Size = new Size(112, 34);
            btnBrowseOutputFile2.TabIndex = 8;
            btnBrowseOutputFile2.Text = "Browse...";
            btnBrowseOutputFile2.UseVisualStyleBackColor = true;
            btnBrowseOutputFile2.Click += btnBrowseOutputFile2_Click;
            // 
            // txtOutputFile2
            // 
            txtOutputFile2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtOutputFile2.Location = new Point(16, 252);
            txtOutputFile2.Name = "txtOutputFile2";
            txtOutputFile2.Size = new Size(655, 31);
            txtOutputFile2.TabIndex = 7;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(13, 224);
            label3.Name = "label3";
            label3.Size = new Size(449, 25);
            label3.TabIndex = 6;
            label3.Text = "Encounter CSV file (one line per encounter and species):";
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(24, 24);
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, helpToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(800, 33);
            menuStrip1.TabIndex = 9;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { loadSpeciesListToolStripMenuItem, clearSpeciesListToolStripMenuItem, exitToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(54, 29);
            fileToolStripMenuItem.Text = "File";
            // 
            // loadSpeciesListToolStripMenuItem
            // 
            loadSpeciesListToolStripMenuItem.Name = "loadSpeciesListToolStripMenuItem";
            loadSpeciesListToolStripMenuItem.Size = new Size(270, 34);
            loadSpeciesListToolStripMenuItem.Text = "&Load species list...";
            loadSpeciesListToolStripMenuItem.Click += loadSpeciesListToolStripMenuItem_Click;
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new Size(270, 34);
            exitToolStripMenuItem.Text = "E&xit";
            exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;
            // 
            // helpToolStripMenuItem
            // 
            helpToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { aboutToolStripMenuItem });
            helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            helpToolStripMenuItem.Size = new Size(65, 29);
            helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            aboutToolStripMenuItem.Size = new Size(164, 34);
            aboutToolStripMenuItem.Text = "About";
            aboutToolStripMenuItem.Click += aboutToolStripMenuItem_Click;
            // 
            // btnRun
            // 
            btnRun.Anchor = AnchorStyles.Top;
            btnRun.Location = new Point(344, 317);
            btnRun.Name = "btnRun";
            btnRun.Size = new Size(112, 34);
            btnRun.TabIndex = 10;
            btnRun.Text = "&Run";
            btnRun.UseVisualStyleBackColor = true;
            btnRun.Click += btnRun_Click;
            // 
            // statusStrip1
            // 
            statusStrip1.ImageScalingSize = new Size(24, 24);
            statusStrip1.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel1 });
            statusStrip1.Location = new Point(0, 392);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(800, 22);
            statusStrip1.TabIndex = 11;
            statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            toolStripStatusLabel1.Size = new Size(0, 15);
            // 
            // clearSpeciesListToolStripMenuItem
            // 
            clearSpeciesListToolStripMenuItem.Name = "clearSpeciesListToolStripMenuItem";
            clearSpeciesListToolStripMenuItem.Size = new Size(270, 34);
            clearSpeciesListToolStripMenuItem.Text = "&Clear species list";
            clearSpeciesListToolStripMenuItem.Click += clearSpeciesListToolStripMenuItem_Click;
            // 
            // FormMain
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 414);
            Controls.Add(statusStrip1);
            Controls.Add(btnRun);
            Controls.Add(btnBrowseOutputFile2);
            Controls.Add(txtOutputFile2);
            Controls.Add(label3);
            Controls.Add(btnBrowseOutputFile1);
            Controls.Add(txtOutputFile1);
            Controls.Add(label2);
            Controls.Add(btnBrowseInputFolder);
            Controls.Add(txtInputFolder);
            Controls.Add(label1);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            MinimumSize = new Size(822, 437);
            Name = "FormMain";
            SizeGripStyle = SizeGripStyle.Show;
            Text = "Photos To CSV";
            FormClosing += FormMain_FormClosing;
            Load += FormMain_Load;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox txtInputFolder;
        private Button btnBrowseInputFolder;
        private Label label2;
        private Button btnBrowseOutputFile1;
        private TextBox txtOutputFile1;
        private Button btnBrowseOutputFile2;
        private TextBox txtOutputFile2;
        private Label label3;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem helpToolStripMenuItem;
        private Button btnRun;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem aboutToolStripMenuItem;
        private ToolStripMenuItem loadSpeciesListToolStripMenuItem;
        private ToolStripMenuItem clearSpeciesListToolStripMenuItem;
    }
}