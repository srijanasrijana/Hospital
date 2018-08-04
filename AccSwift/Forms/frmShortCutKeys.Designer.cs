namespace AccSwift
{
    partial class frmShortCutKeys
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmShortCutKeys));
            this.label7 = new System.Windows.Forms.Label();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Description = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lstShortcutKeys = new System.Windows.Forms.ListView();
            this.SuspendLayout();
            // 
            // label7
            // 
            this.label7.AutoEllipsis = true;
            this.label7.BackColor = System.Drawing.Color.PaleTurquoise;
            this.label7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(0, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(414, 35);
            this.label7.TabIndex = 58;
            this.label7.Text = "Short Cut Keys Action";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "S.No.";
            this.columnHeader1.Width = 50;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "ShortCut Keys Name";
            this.columnHeader2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader2.Width = 120;
            // 
            // Description
            // 
            this.Description.Text = "Description";
            this.Description.Width = 250;
            // 
            // lstShortcutKeys
            // 
            this.lstShortcutKeys.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.Description});
            this.lstShortcutKeys.Location = new System.Drawing.Point(0, 30);
            this.lstShortcutKeys.Name = "lstShortcutKeys";
            this.lstShortcutKeys.Size = new System.Drawing.Size(414, 379);
            this.lstShortcutKeys.TabIndex = 0;
            this.lstShortcutKeys.UseCompatibleStateImageBehavior = false;
            // 
            // frmShortCutKeys
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(419, 464);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.lstShortcutKeys);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmShortCutKeys";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Short Cut Keys";
            this.Load += new System.EventHandler(this.frmShortCutKeys_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader Description;
        private System.Windows.Forms.ListView lstShortcutKeys;
    }
}