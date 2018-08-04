namespace Inventory.View.Reports
{
    partial class frmSaleRegisterPartyWise
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
            this.lblAmountInWord = new System.Windows.Forms.Label();
            this.lblProductPartyPivot = new System.Windows.Forms.Label();
            this.lbInboundQty = new System.Windows.Forms.Label();
            this.lblOutBoundQty = new System.Windows.Forms.Label();
            this.lblTotalAmt = new System.Windows.Forms.Label();
            this.lblProjectName = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dgSalesRegister = new SComponents.SDataGrid(this.components);
            this.lblClosingQty = new System.Windows.Forms.Label();
            this.lblOpeningQty = new System.Windows.Forms.Label();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnPrintPreview = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.lblAsonDate = new System.Windows.Forms.Label();
            this.lblReportType = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnPrint = new System.Windows.Forms.Button();
            this.lblAllSettings = new System.Windows.Forms.Label();
            this.lblWebsite = new System.Windows.Forms.Label();
            this.lblCompanyAddress = new System.Windows.Forms.Label();
            this.lblContact = new System.Windows.Forms.Label();
            this.lblCompanyName = new System.Windows.Forms.Label();
            this.lblPanNo = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblAmountInWord
            // 
            this.lblAmountInWord.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblAmountInWord.AutoSize = true;
            this.lblAmountInWord.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAmountInWord.Location = new System.Drawing.Point(15, 578);
            this.lblAmountInWord.Name = "lblAmountInWord";
            this.lblAmountInWord.Size = new System.Drawing.Size(60, 13);
            this.lblAmountInWord.TabIndex = 108;
            this.lblAmountInWord.Text = "AmtInWord";
            // 
            // lblProductPartyPivot
            // 
            this.lblProductPartyPivot.AutoSize = true;
            this.lblProductPartyPivot.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProductPartyPivot.Location = new System.Drawing.Point(15, 214);
            this.lblProductPartyPivot.Name = "lblProductPartyPivot";
            this.lblProductPartyPivot.Size = new System.Drawing.Size(41, 15);
            this.lblProductPartyPivot.TabIndex = 100;
            this.lblProductPartyPivot.Text = "label1";
            // 
            // lbInboundQty
            // 
            this.lbInboundQty.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lbInboundQty.AutoSize = true;
            this.lbInboundQty.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbInboundQty.Location = new System.Drawing.Point(323, 314);
            this.lbInboundQty.Name = "lbInboundQty";
            this.lbInboundQty.Size = new System.Drawing.Size(85, 15);
            this.lbInboundQty.TabIndex = 100;
            this.lbInboundQty.Text = "InBoundQty: 0";
            this.lbInboundQty.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lbInboundQty.Visible = false;
            // 
            // lblOutBoundQty
            // 
            this.lblOutBoundQty.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblOutBoundQty.AutoSize = true;
            this.lblOutBoundQty.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOutBoundQty.Location = new System.Drawing.Point(496, 314);
            this.lblOutBoundQty.Name = "lblOutBoundQty";
            this.lblOutBoundQty.Size = new System.Drawing.Size(95, 15);
            this.lblOutBoundQty.TabIndex = 99;
            this.lblOutBoundQty.Text = "OutBoundQty: 0";
            this.lblOutBoundQty.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblTotalAmt
            // 
            this.lblTotalAmt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTotalAmt.AutoSize = true;
            this.lblTotalAmt.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalAmt.Location = new System.Drawing.Point(657, 314);
            this.lblTotalAmt.Name = "lblTotalAmt";
            this.lblTotalAmt.Size = new System.Drawing.Size(64, 15);
            this.lblTotalAmt.TabIndex = 98;
            this.lblTotalAmt.Text = "Amount: 0";
            this.lblTotalAmt.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblProjectName
            // 
            this.lblProjectName.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblProjectName.AutoSize = true;
            this.lblProjectName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProjectName.Location = new System.Drawing.Point(351, 171);
            this.lblProjectName.Name = "lblProjectName";
            this.lblProjectName.Size = new System.Drawing.Size(105, 13);
            this.lblProjectName.TabIndex = 109;
            this.lblProjectName.Text = "ProjectName : All";
            this.lblProjectName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.BackColor = System.Drawing.Color.White;
            this.groupBox1.Controls.Add(this.dgSalesRegister);
            this.groupBox1.Controls.Add(this.lbInboundQty);
            this.groupBox1.Controls.Add(this.lblOutBoundQty);
            this.groupBox1.Controls.Add(this.lblTotalAmt);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(12, 232);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(826, 343);
            this.groupBox1.TabIndex = 107;
            this.groupBox1.TabStop = false;
            // 
            // dgSalesRegister
            // 
            this.dgSalesRegister.AlternateBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.dgSalesRegister.AlternateForeColor = System.Drawing.Color.Black;
            this.dgSalesRegister.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgSalesRegister.AutoSize = true;
            this.dgSalesRegister.DeleteQuestionMessage = "Are you sure to delete all the selected rows?";
            this.dgSalesRegister.EnableSort = false;
            this.dgSalesRegister.FixedRows = 1;
            this.dgSalesRegister.Location = new System.Drawing.Point(6, 18);
            this.dgSalesRegister.Name = "dgSalesRegister";
            this.dgSalesRegister.SelectionMode = SourceGrid.GridSelectionMode.Row;
            this.dgSalesRegister.Size = new System.Drawing.Size(814, 280);
            this.dgSalesRegister.TabIndex = 102;
            this.dgSalesRegister.TabStop = true;
            this.dgSalesRegister.ToolTipText = "";
            this.dgSalesRegister.DoubleClick += new System.EventHandler(this.dgSalesRegister_DoubleClick);
            // 
            // lblClosingQty
            // 
            this.lblClosingQty.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblClosingQty.AutoSize = true;
            this.lblClosingQty.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblClosingQty.Location = new System.Drawing.Point(720, 586);
            this.lblClosingQty.Name = "lblClosingQty";
            this.lblClosingQty.Size = new System.Drawing.Size(84, 15);
            this.lblClosingQty.TabIndex = 106;
            this.lblClosingQty.Text = "Closing Qty: 0";
            this.lblClosingQty.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblOpeningQty
            // 
            this.lblOpeningQty.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblOpeningQty.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOpeningQty.Location = new System.Drawing.Point(648, 206);
            this.lblOpeningQty.Name = "lblOpeningQty";
            this.lblOpeningQty.Size = new System.Drawing.Size(190, 23);
            this.lblOpeningQty.TabIndex = 105;
            this.lblOpeningQty.Text = "Opening Qty: 0";
            this.lblOpeningQty.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnExport
            // 
            this.btnExport.Image = global::Inventory.Properties.Resources.export1;
            this.btnExport.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnExport.Location = new System.Drawing.Point(78, 3);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(66, 29);
            this.btnExport.TabIndex = 25;
            this.btnExport.Text = "&Export";
            this.btnExport.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnPrintPreview
            // 
            this.btnPrintPreview.Image = global::Inventory.Properties.Resources.print_preview;
            this.btnPrintPreview.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPrintPreview.Location = new System.Drawing.Point(150, 3);
            this.btnPrintPreview.Name = "btnPrintPreview";
            this.btnPrintPreview.Size = new System.Drawing.Size(95, 29);
            this.btnPrintPreview.TabIndex = 24;
            this.btnPrintPreview.Text = "Pr&int Preview";
            this.btnPrintPreview.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnPrintPreview.UseVisualStyleBackColor = true;
            this.btnPrintPreview.Click += new System.EventHandler(this.btnPrintPreview_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Image = global::Inventory.Properties.Resources.refresh;
            this.btnRefresh.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRefresh.Location = new System.Drawing.Point(6, 3);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(66, 29);
            this.btnRefresh.TabIndex = 23;
            this.btnRefresh.Text = "&Refresh";
            this.btnRefresh.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnExit
            // 
            this.btnExit.Image = global::Inventory.Properties.Resources.ExitButton;
            this.btnExit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnExit.Location = new System.Drawing.Point(311, 3);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(46, 29);
            this.btnExit.TabIndex = 22;
            this.btnExit.Text = "E&xit";
            this.btnExit.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // lblAsonDate
            // 
            this.lblAsonDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblAsonDate.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAsonDate.Location = new System.Drawing.Point(599, 47);
            this.lblAsonDate.Name = "lblAsonDate";
            this.lblAsonDate.Size = new System.Drawing.Size(233, 21);
            this.lblAsonDate.TabIndex = 104;
            this.lblAsonDate.Text = "As On Date:";
            this.lblAsonDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblReportType
            // 
            this.lblReportType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblReportType.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblReportType.Location = new System.Drawing.Point(307, 136);
            this.lblReportType.Name = "lblReportType";
            this.lblReportType.Size = new System.Drawing.Size(221, 35);
            this.lblReportType.TabIndex = 102;
            this.lblReportType.Text = "Sales Register Party Wise";
            this.lblReportType.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.LightBlue;
            this.panel1.Controls.Add(this.btnExport);
            this.panel1.Controls.Add(this.btnPrintPreview);
            this.panel1.Controls.Add(this.btnRefresh);
            this.panel1.Controls.Add(this.btnExit);
            this.panel1.Controls.Add(this.btnPrint);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(850, 35);
            this.panel1.TabIndex = 101;
            // 
            // btnPrint
            // 
            this.btnPrint.Image = global::Inventory.Properties.Resources.print;
            this.btnPrint.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPrint.Location = new System.Drawing.Point(251, 3);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(54, 29);
            this.btnPrint.TabIndex = 2;
            this.btnPrint.Text = "&Print";
            this.btnPrint.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // lblAllSettings
            // 
            this.lblAllSettings.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAllSettings.Location = new System.Drawing.Point(15, 180);
            this.lblAllSettings.Name = "lblAllSettings";
            this.lblAllSettings.Size = new System.Drawing.Size(163, 21);
            this.lblAllSettings.TabIndex = 103;
            this.lblAllSettings.Text = "Settings";
            this.lblAllSettings.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblWebsite
            // 
            this.lblWebsite.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblWebsite.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWebsite.Location = new System.Drawing.Point(110, 111);
            this.lblWebsite.Name = "lblWebsite";
            this.lblWebsite.Size = new System.Drawing.Size(563, 25);
            this.lblWebsite.TabIndex = 113;
            this.lblWebsite.Text = "Website";
            this.lblWebsite.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblCompanyAddress
            // 
            this.lblCompanyAddress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCompanyAddress.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCompanyAddress.Location = new System.Drawing.Point(66, 76);
            this.lblCompanyAddress.Name = "lblCompanyAddress";
            this.lblCompanyAddress.Size = new System.Drawing.Size(649, 25);
            this.lblCompanyAddress.TabIndex = 112;
            this.lblCompanyAddress.Text = "Address";
            this.lblCompanyAddress.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblContact
            // 
            this.lblContact.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblContact.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblContact.Location = new System.Drawing.Point(110, 94);
            this.lblContact.Name = "lblContact";
            this.lblContact.Size = new System.Drawing.Size(563, 25);
            this.lblContact.TabIndex = 111;
            this.lblContact.Text = "Contact";
            this.lblContact.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblCompanyName
            // 
            this.lblCompanyName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCompanyName.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCompanyName.Location = new System.Drawing.Point(132, 47);
            this.lblCompanyName.Name = "lblCompanyName";
            this.lblCompanyName.Size = new System.Drawing.Size(558, 36);
            this.lblCompanyName.TabIndex = 110;
            this.lblCompanyName.Text = "Company Name";
            this.lblCompanyName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblPanNo
            // 
            this.lblPanNo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPanNo.Font = new System.Drawing.Font("Arial", 6F, System.Drawing.FontStyle.Bold);
            this.lblPanNo.Location = new System.Drawing.Point(655, 83);
            this.lblPanNo.Name = "lblPanNo";
            this.lblPanNo.Size = new System.Drawing.Size(177, 21);
            this.lblPanNo.TabIndex = 114;
            this.lblPanNo.Text = "PAN No.";
            this.lblPanNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblPanNo.Visible = false;
            // 
            // frmSaleRegisterPartyWise
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(850, 610);
            this.Controls.Add(this.lblPanNo);
            this.Controls.Add(this.lblWebsite);
            this.Controls.Add(this.lblCompanyAddress);
            this.Controls.Add(this.lblContact);
            this.Controls.Add(this.lblCompanyName);
            this.Controls.Add(this.lblAmountInWord);
            this.Controls.Add(this.lblProductPartyPivot);
            this.Controls.Add(this.lblProjectName);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lblClosingQty);
            this.Controls.Add(this.lblOpeningQty);
            this.Controls.Add(this.lblAsonDate);
            this.Controls.Add(this.lblReportType);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lblAllSettings);
            this.Name = "frmSaleRegisterPartyWise";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FrmSaleRegs";
            this.Load += new System.EventHandler(this.FrmSalesRegister_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblAmountInWord;
        private System.Windows.Forms.Label lblProductPartyPivot;
        private System.Windows.Forms.Label lbInboundQty;
        private System.Windows.Forms.Label lblOutBoundQty;
        private System.Windows.Forms.Label lblTotalAmt;
        private System.Windows.Forms.Label lblProjectName;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblClosingQty;
        private System.Windows.Forms.Label lblOpeningQty;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnPrintPreview;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label lblAsonDate;
        private System.Windows.Forms.Label lblReportType;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Label lblAllSettings;
        private SComponents.SDataGrid dgSalesRegister;
        private System.Windows.Forms.Label lblWebsite;
        private System.Windows.Forms.Label lblCompanyAddress;
        private System.Windows.Forms.Label lblContact;
        private System.Windows.Forms.Label lblCompanyName;
        private System.Windows.Forms.Label lblPanNo;

    }
}