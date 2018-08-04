using SComponents;
namespace Common
{
    partial class frmDateConverter
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDateConverter));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtNepaliDate = new SComponents.SMaskedTextBox();
            this.txtEnglishDate = new SComponents.SMaskedTextBox();
            this.btnConInsert = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblNepaliDate = new System.Windows.Forms.Label();
            this.lblEnglishDate = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 13);
            this.label1.TabIndex = 70;
            this.label1.Text = "English Date:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 13);
            this.label2.TabIndex = 71;
            this.label2.Text = "Nepali Date:";
            // 
            // txtNepaliDate
            // 
            this.txtNepaliDate.BackColor = System.Drawing.Color.White;
            this.txtNepaliDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtNepaliDate.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.txtNepaliDate.FocusLostColor = System.Drawing.Color.White;
            this.txtNepaliDate.Location = new System.Drawing.Point(82, 40);
            this.txtNepaliDate.Mask = "0000/00/00";
            this.txtNepaliDate.Name = "txtNepaliDate";
            this.txtNepaliDate.Size = new System.Drawing.Size(75, 20);
            this.txtNepaliDate.TabIndex = 69;
            this.txtNepaliDate.MaskInputRejected += new System.Windows.Forms.MaskInputRejectedEventHandler(this.txtNepaliDate_MaskInputRejected);
            this.txtNepaliDate.TextChanged += new System.EventHandler(this.txtNepaliDate_TextChanged);
            // 
            // txtEnglishDate
            // 
            this.txtEnglishDate.BackColor = System.Drawing.Color.White;
            this.txtEnglishDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtEnglishDate.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.txtEnglishDate.FocusLostColor = System.Drawing.Color.White;
            this.txtEnglishDate.Location = new System.Drawing.Point(82, 14);
            this.txtEnglishDate.Mask = "0000/00/00";
            this.txtEnglishDate.Name = "txtEnglishDate";
            this.txtEnglishDate.Size = new System.Drawing.Size(75, 20);
            this.txtEnglishDate.TabIndex = 68;
            this.txtEnglishDate.MaskInputRejected += new System.Windows.Forms.MaskInputRejectedEventHandler(this.txtEnglishDate_MaskInputRejected);
            this.txtEnglishDate.TextChanged += new System.EventHandler(this.txtEnglishDate_TextChanged);
            // 
            // btnConInsert
            // 
            this.btnConInsert.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnConInsert.Location = new System.Drawing.Point(275, 64);
            this.btnConInsert.Name = "btnConInsert";
            this.btnConInsert.Size = new System.Drawing.Size(75, 23);
            this.btnConInsert.TabIndex = 72;
            this.btnConInsert.Text = "Insert";
            this.btnConInsert.UseVisualStyleBackColor = true;
            this.btnConInsert.Click += new System.EventHandler(this.btnConInsert_Click);
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.DarkGoldenrod;
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(-2, -1);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(376, 28);
            this.label3.TabIndex = 73;
            this.label3.Text = "Please enter the valid English or Nepali date to get its equivalent date. Please " +
    "enter both dates in the format YYYY/MM/DD";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblNepaliDate);
            this.groupBox1.Controls.Add(this.lblEnglishDate);
            this.groupBox1.Controls.Add(this.txtNepaliDate);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.btnConInsert);
            this.groupBox1.Controls.Add(this.txtEnglishDate);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(6, 27);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(356, 96);
            this.groupBox1.TabIndex = 74;
            this.groupBox1.TabStop = false;
            // 
            // lblNepaliDate
            // 
            this.lblNepaliDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblNepaliDate.Location = new System.Drawing.Point(163, 41);
            this.lblNepaliDate.Name = "lblNepaliDate";
            this.lblNepaliDate.Size = new System.Drawing.Size(187, 20);
            this.lblNepaliDate.TabIndex = 74;
            this.lblNepaliDate.Text = "-";
            // 
            // lblEnglishDate
            // 
            this.lblEnglishDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblEnglishDate.Location = new System.Drawing.Point(163, 14);
            this.lblEnglishDate.Name = "lblEnglishDate";
            this.lblEnglishDate.Size = new System.Drawing.Size(187, 20);
            this.lblEnglishDate.TabIndex = 73;
            this.lblEnglishDate.Text = "-";
            // 
            // frmDateConverter
            // 
            this.AcceptButton = this.btnConInsert;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(372, 133);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label3);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "frmDateConverter";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Date Converter Utility";
            this.Load += new System.EventHandler(this.frmDateConverter_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmDateConverter_KeyDown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private SMaskedTextBox txtNepaliDate;
        private SMaskedTextBox txtEnglishDate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnConInsert;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblNepaliDate;
        private System.Windows.Forms.Label lblEnglishDate;

    }
}