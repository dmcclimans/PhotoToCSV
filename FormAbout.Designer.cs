namespace PhotoToCSV
{
    partial class FormAbout
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label1 = new Label();
            label2 = new Label();
            lblVersion = new Label();
            button1 = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point);
            label1.Location = new Point(124, 54);
            label1.Name = "label1";
            label1.Size = new Size(178, 38);
            label1.TabIndex = 0;
            label1.Text = "PhotoToCSV";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(153, 114);
            label2.Name = "label2";
            label2.Size = new Size(70, 25);
            label2.TabIndex = 1;
            label2.Text = "Version";
            // 
            // lblVersion
            // 
            lblVersion.AutoSize = true;
            lblVersion.Location = new Point(229, 114);
            lblVersion.Name = "lblVersion";
            lblVersion.Size = new Size(45, 25);
            lblVersion.TabIndex = 2;
            lblVersion.Text = "x.y.z";
            // 
            // button1
            // 
            button1.DialogResult = DialogResult.OK;
            button1.Location = new Point(157, 208);
            button1.Name = "button1";
            button1.Size = new Size(112, 34);
            button1.TabIndex = 3;
            button1.Text = "OK";
            button1.UseVisualStyleBackColor = true;
            // 
            // FormAbout
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(426, 303);
            Controls.Add(button1);
            Controls.Add(lblVersion);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "FormAbout";
            Text = "About PhotoToCSV";
            FormClosing += FormAbout_FormClosing;
            Load += FormAbout_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private Label lblVersion;
        private Button button1;
    }
}