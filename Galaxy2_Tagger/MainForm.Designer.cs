namespace Galaxy2_Tagger
{
    partial class MainForm
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
            this.GamesList = new System.Windows.Forms.ListBox();
            this.SaveChanges = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // GamesList
            // 
            this.GamesList.FormattingEnabled = true;
            this.GamesList.Location = new System.Drawing.Point(12, 12);
            this.GamesList.Name = "GamesList";
            this.GamesList.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.GamesList.Size = new System.Drawing.Size(220, 589);
            this.GamesList.TabIndex = 0;
            this.GamesList.SelectedValueChanged += new System.EventHandler(this.GamesList_SelectedValueChanged);
            // 
            // SaveChanges
            // 
            this.SaveChanges.Location = new System.Drawing.Point(599, 580);
            this.SaveChanges.Name = "SaveChanges";
            this.SaveChanges.Size = new System.Drawing.Size(75, 23);
            this.SaveChanges.TabIndex = 1;
            this.SaveChanges.Text = "Save";
            this.SaveChanges.UseVisualStyleBackColor = true;
            this.SaveChanges.Click += new System.EventHandler(this.DoSaveChanges);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(686, 615);
            this.Controls.Add(this.SaveChanges);
            this.Controls.Add(this.GamesList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Tagger";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox GamesList;
        private System.Windows.Forms.Button SaveChanges;
    }
}

