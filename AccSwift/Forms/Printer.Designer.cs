namespace AccSwift.Forms
{
    partial class Printer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Printer));
            this.BtnOk = new System.Windows.Forms.Button();
            this.cboPrinterName = new System.Windows.Forms.ComboBox();
            this.lblPrinterName = new System.Windows.Forms.Label();
            this.lblselect = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // BtnOk
            // 
            this.BtnOk.Location = new System.Drawing.Point(218, 76);
            this.BtnOk.Name = "BtnOk";
            this.BtnOk.Size = new System.Drawing.Size(91, 26);
            this.BtnOk.TabIndex = 0;
            this.BtnOk.Text = "Ok";
            this.BtnOk.UseVisualStyleBackColor = true;
            this.BtnOk.Click += new System.EventHandler(this.BtnOk_Click);
            // 
            // cboPrinterName
            // 
            this.cboPrinterName.FormattingEnabled = true;
            this.cboPrinterName.Location = new System.Drawing.Point(111, 49);
            this.cboPrinterName.Name = "cboPrinterName";
            this.cboPrinterName.Size = new System.Drawing.Size(198, 21);
            this.cboPrinterName.TabIndex = 1;
            // 
            // lblPrinterName
            // 
            this.lblPrinterName.AutoSize = true;
            this.lblPrinterName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPrinterName.Location = new System.Drawing.Point(34, 52);
            this.lblPrinterName.Name = "lblPrinterName";
            this.lblPrinterName.Size = new System.Drawing.Size(80, 13);
            this.lblPrinterName.TabIndex = 2;
            this.lblPrinterName.Text = "Printer Name";
            // 
            // lblselect
            // 
            this.lblselect.AutoSize = true;
            this.lblselect.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Italic | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblselect.Location = new System.Drawing.Point(39, 17);
            this.lblselect.Name = "lblselect";
            this.lblselect.Size = new System.Drawing.Size(270, 17);
            this.lblselect.TabIndex = 3;
            this.lblselect.Text = "Select the Printer to print your documents";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.BtnOk);
            this.panel1.Controls.Add(this.lblselect);
            this.panel1.Controls.Add(this.cboPrinterName);
            this.panel1.Controls.Add(this.lblPrinterName);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(331, 120);
            this.panel1.TabIndex = 4;
            // 
            // Printer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(357, 145);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Printer";
            this.Text = "Printer";
            this.Load += new System.EventHandler(this.Printer_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button BtnOk;
        private System.Windows.Forms.ComboBox cboPrinterName;
        private System.Windows.Forms.Label lblPrinterName;
        private System.Windows.Forms.Label lblselect;
        private System.Windows.Forms.Panel panel1;
    }
}