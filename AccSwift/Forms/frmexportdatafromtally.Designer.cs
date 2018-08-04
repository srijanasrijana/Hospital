namespace AccSwift.Forms
{
    partial class frmexportdatafromtally
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmexportdatafromtally));
            this.btnexportfromtally = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnexportfromtally
            // 
            this.btnexportfromtally.Location = new System.Drawing.Point(12, 257);
            this.btnexportfromtally.Name = "btnexportfromtally";
            this.btnexportfromtally.Size = new System.Drawing.Size(130, 23);
            this.btnexportfromtally.TabIndex = 0;
            this.btnexportfromtally.Text = "Export From Tally";
            this.btnexportfromtally.UseVisualStyleBackColor = true;
            this.btnexportfromtally.Click += new System.EventHandler(this.btnexportfromtally_Click);
            // 
            // frmexportdatafromtally
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(514, 311);
            this.Controls.Add(this.btnexportfromtally);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmexportdatafromtally";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Tally To SqlServer";
            this.Load += new System.EventHandler(this.frmexportdatafromtally_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnexportfromtally;
    }
}