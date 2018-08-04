namespace Hospital.View.Report
{
    partial class frmPatientReport
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
            this.components = new System.ComponentModel.Container();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnPrintPreview = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dgPatientRegister = new SComponents.SDataGrid(this.components);
            this.lblPanNo = new System.Windows.Forms.Label();
            this.lblWebsite = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblCompanyName = new System.Windows.Forms.Label();
            this.lblCompanyAddress = new System.Windows.Forms.Label();
            this.lblReportType = new System.Windows.Forms.Label();
            this.lblContact = new System.Windows.Forms.Label();
            this.lblAsonDate = new System.Windows.Forms.Label();
            this.lblAllSettings = new System.Windows.Forms.Label();
            this.lblProjectName = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel3
            // 
            this.panel3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel3.Location = new System.Drawing.Point(2, 554);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(934, 18);
            this.panel3.TabIndex = 7;
            // 
            // btnExport
            // 
            this.btnExport.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnExport.Location = new System.Drawing.Point(71, 4);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(62, 29);
            this.btnExport.TabIndex = 25;
            this.btnExport.Text = "&Export";
            this.btnExport.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnExport.UseVisualStyleBackColor = true;
            // 
            // btnPrintPreview
            // 
            this.btnPrintPreview.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPrintPreview.Location = new System.Drawing.Point(134, 4);
            this.btnPrintPreview.Name = "btnPrintPreview";
            this.btnPrintPreview.Size = new System.Drawing.Size(94, 29);
            this.btnPrintPreview.TabIndex = 24;
            this.btnPrintPreview.Text = "Pr&int Preview";
            this.btnPrintPreview.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnPrintPreview.UseVisualStyleBackColor = true;
            this.btnPrintPreview.Click += new System.EventHandler(this.btnPrintPreview_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRefresh.Location = new System.Drawing.Point(2, 4);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(68, 29);
            this.btnRefresh.TabIndex = 23;
            this.btnRefresh.Text = "&Refresh";
            this.btnRefresh.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnExit
            // 
            this.btnExit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnExit.Location = new System.Drawing.Point(284, 4);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(50, 29);
            this.btnExit.TabIndex = 22;
            this.btnExit.Text = "E&xit";
            this.btnExit.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnExit.UseVisualStyleBackColor = true;
            // 
            // btnPrint
            // 
            this.btnPrint.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPrint.Location = new System.Drawing.Point(229, 4);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(54, 29);
            this.btnPrint.TabIndex = 2;
            this.btnPrint.Text = "&Print";
            this.btnPrint.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnPrint.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.LightBlue;
            this.panel1.Controls.Add(this.btnExport);
            this.panel1.Controls.Add(this.btnPrintPreview);
            this.panel1.Controls.Add(this.btnRefresh);
            this.panel1.Controls.Add(this.btnExit);
            this.panel1.Controls.Add(this.btnPrint);
            this.panel1.Location = new System.Drawing.Point(1, 1);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(938, 35);
            this.panel1.TabIndex = 29;
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.Controls.Add(this.dgPatientRegister);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Controls.Add(this.lblPanNo);
            this.panel2.Controls.Add(this.lblWebsite);
            this.panel2.Controls.Add(this.statusStrip1);
            this.panel2.Controls.Add(this.lblCompanyName);
            this.panel2.Controls.Add(this.lblCompanyAddress);
            this.panel2.Controls.Add(this.lblReportType);
            this.panel2.Controls.Add(this.lblContact);
            this.panel2.Controls.Add(this.lblAsonDate);
            this.panel2.Controls.Add(this.lblAllSettings);
            this.panel2.Controls.Add(this.lblProjectName);
            this.panel2.Location = new System.Drawing.Point(3, 42);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(936, 575);
            this.panel2.TabIndex = 30;
            this.panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.panel2_Paint);
            // 
            // dgPatientRegister
            // 
            this.dgPatientRegister.AlternateBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.dgPatientRegister.AlternateForeColor = System.Drawing.Color.Black;
            this.dgPatientRegister.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgPatientRegister.AutoSize = true;
            this.dgPatientRegister.DeleteQuestionMessage = "Are you sure to delete all the selected rows?";
            this.dgPatientRegister.EnableSort = false;
            this.dgPatientRegister.FixedRows = 1;
            this.dgPatientRegister.Location = new System.Drawing.Point(9, 197);
            this.dgPatientRegister.Name = "dgPatientRegister";
            this.dgPatientRegister.SelectionMode = SourceGrid.GridSelectionMode.Row;
            this.dgPatientRegister.Size = new System.Drawing.Size(916, 351);
            this.dgPatientRegister.TabIndex = 102;
            this.dgPatientRegister.TabStop = true;
            this.dgPatientRegister.ToolTipText = "";
            // 
            // lblPanNo
            // 
            this.lblPanNo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPanNo.Font = new System.Drawing.Font("Arial", 6F, System.Drawing.FontStyle.Bold);
            this.lblPanNo.Location = new System.Drawing.Point(766, 43);
            this.lblPanNo.Name = "lblPanNo";
            this.lblPanNo.Size = new System.Drawing.Size(34, 21);
            this.lblPanNo.TabIndex = 118;
            this.lblPanNo.Text = "PAN No.";
            this.lblPanNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblPanNo.Visible = false;
            // 
            // lblWebsite
            // 
            this.lblWebsite.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblWebsite.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWebsite.Location = new System.Drawing.Point(241, 78);
            this.lblWebsite.Name = "lblWebsite";
            this.lblWebsite.Size = new System.Drawing.Size(420, 25);
            this.lblWebsite.TabIndex = 117;
            this.lblWebsite.Text = "Website";
            this.lblWebsite.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 553);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1111, 22);
            this.statusStrip1.TabIndex = 6;
            this.statusStrip1.Text = "statusStrip1";
            this.statusStrip1.Visible = false;
            // 
            // lblCompanyName
            // 
            this.lblCompanyName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCompanyName.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCompanyName.Location = new System.Drawing.Point(263, 14);
            this.lblCompanyName.Name = "lblCompanyName";
            this.lblCompanyName.Size = new System.Drawing.Size(415, 36);
            this.lblCompanyName.TabIndex = 114;
            this.lblCompanyName.Text = "Company Name";
            this.lblCompanyName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblCompanyAddress
            // 
            this.lblCompanyAddress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCompanyAddress.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCompanyAddress.Location = new System.Drawing.Point(197, 43);
            this.lblCompanyAddress.Name = "lblCompanyAddress";
            this.lblCompanyAddress.Size = new System.Drawing.Size(506, 25);
            this.lblCompanyAddress.TabIndex = 116;
            this.lblCompanyAddress.Text = "Address";
            this.lblCompanyAddress.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblReportType
            // 
            this.lblReportType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblReportType.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblReportType.Location = new System.Drawing.Point(357, 103);
            this.lblReportType.Name = "lblReportType";
            this.lblReportType.Size = new System.Drawing.Size(202, 35);
            this.lblReportType.TabIndex = 109;
            this.lblReportType.Text = "Patient Register";
            this.lblReportType.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblContact
            // 
            this.lblContact.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblContact.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblContact.Location = new System.Drawing.Point(241, 61);
            this.lblContact.Name = "lblContact";
            this.lblContact.Size = new System.Drawing.Size(420, 25);
            this.lblContact.TabIndex = 115;
            this.lblContact.Text = "Contact";
            this.lblContact.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblAsonDate
            // 
            this.lblAsonDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblAsonDate.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAsonDate.Location = new System.Drawing.Point(678, 173);
            this.lblAsonDate.Name = "lblAsonDate";
            this.lblAsonDate.Size = new System.Drawing.Size(233, 21);
            this.lblAsonDate.TabIndex = 111;
            this.lblAsonDate.Text = "As On Date:";
            this.lblAsonDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblAllSettings
            // 
            this.lblAllSettings.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAllSettings.Location = new System.Drawing.Point(22, 173);
            this.lblAllSettings.Name = "lblAllSettings";
            this.lblAllSettings.Size = new System.Drawing.Size(163, 21);
            this.lblAllSettings.TabIndex = 110;
            this.lblAllSettings.Text = "Settings";
            this.lblAllSettings.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblProjectName
            // 
            this.lblProjectName.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblProjectName.AutoSize = true;
            this.lblProjectName.Location = new System.Drawing.Point(415, 138);
            this.lblProjectName.Name = "lblProjectName";
            this.lblProjectName.Size = new System.Drawing.Size(88, 13);
            this.lblProjectName.TabIndex = 113;
            this.lblProjectName.Text = "ProjectName : All";
            this.lblProjectName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // frmPatientReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(940, 619);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Name = "frmPatientReport";
            this.Text = "frmPatientReport";
            this.Load += new System.EventHandler(this.frmPatientReport_Load);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnPrintPreview;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private SComponents.SDataGrid dgPatientRegister;
        private System.Windows.Forms.Label lblPanNo;
        private System.Windows.Forms.Label lblWebsite;
        private System.Windows.Forms.Label lblCompanyName;
        private System.Windows.Forms.Label lblCompanyAddress;
        private System.Windows.Forms.Label lblReportType;
        private System.Windows.Forms.Label lblContact;
        private System.Windows.Forms.Label lblAsonDate;
        private System.Windows.Forms.Label lblAllSettings;
        private System.Windows.Forms.Label lblProjectName;
    }
}