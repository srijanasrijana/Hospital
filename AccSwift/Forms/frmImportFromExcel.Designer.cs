using SComponents;
namespace AccSwift.Forms
{
    partial class frmImportFromExcel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmImportFromExcel));
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btnshowfile = new System.Windows.Forms.Button();
            this.txtAttachment = new System.Windows.Forms.TextBox();
            this.txtdatabasetable = new STextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtsheetname = new STextBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(301, 169);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(145, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Import From Excel";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 119);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 13);
            this.label1.TabIndex = 25;
            this.label1.Text = "Attachment:";
            // 
            // btnshowfile
            // 
            this.btnshowfile.Location = new System.Drawing.Point(426, 118);
            this.btnshowfile.Name = "btnshowfile";
            this.btnshowfile.Size = new System.Drawing.Size(24, 24);
            this.btnshowfile.TabIndex = 23;
            this.btnshowfile.Text = "...";
            this.btnshowfile.Click += new System.EventHandler(this.btnshowfile_Click);
            // 
            // txtAttachment
            // 
            this.txtAttachment.Enabled = false;
            this.txtAttachment.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAttachment.Location = new System.Drawing.Point(101, 119);
            this.txtAttachment.Name = "txtAttachment";
            this.txtAttachment.Size = new System.Drawing.Size(317, 23);
            this.txtAttachment.TabIndex = 24;
            // 
            // txtdatabasetable
            // 
            this.txtdatabasetable.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtdatabasetable.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtdatabasetable.FocusLostColor = System.Drawing.Color.White;
            this.txtdatabasetable.Location = new System.Drawing.Point(101, 42);
            this.txtdatabasetable.Name = "txtdatabasetable";
            this.txtdatabasetable.Size = new System.Drawing.Size(194, 20);
            this.txtdatabasetable.TabIndex = 26;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 13);
            this.label2.TabIndex = 27;
            this.label2.Text = "DataBaseTable:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 80);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 13);
            this.label3.TabIndex = 28;
            this.label3.Text = "SheetName:";
            // 
            // txtsheetname
            // 
            this.txtsheetname.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtsheetname.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtsheetname.FocusLostColor = System.Drawing.Color.White;
            this.txtsheetname.Location = new System.Drawing.Point(101, 78);
            this.txtsheetname.Name = "txtsheetname";
            this.txtsheetname.Size = new System.Drawing.Size(194, 20);
            this.txtsheetname.TabIndex = 29;
            // 
            // frmImportFromExcel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(474, 279);
            this.Controls.Add(this.txtsheetname);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtdatabasetable);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnshowfile);
            this.Controls.Add(this.txtAttachment);
            this.Controls.Add(this.button1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmImportFromExcel";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Insert Date to Server From Excel";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnshowfile;
        private System.Windows.Forms.TextBox txtAttachment;
        private STextBox txtdatabasetable;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private STextBox txtsheetname;
    }
}