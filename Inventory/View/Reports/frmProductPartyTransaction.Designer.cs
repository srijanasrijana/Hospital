namespace Inventory
{
    partial class frmProductPartyTransaction
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmProductPartyTransaction));
            this.grdPurchaseProductTransaction = new SourceGrid.Grid();
            this.lblCompanyName = new System.Windows.Forms.Label();
            this.lblPanNo = new System.Windows.Forms.Label();
            this.lblCompanyAddress = new System.Windows.Forms.Label();
            this.lblContact = new System.Windows.Forms.Label();
            this.lblReportName = new System.Windows.Forms.Label();
            this.lblAllSettings = new System.Windows.Forms.Label();
            this.lblProjectName = new System.Windows.Forms.Label();
            this.lblAsonDate = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnPrintPreview = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.lblProductPartyPivot = new System.Windows.Forms.Label();
            this.dgPurchaseTransRep = new SourceGrid.DataGrid();
            this.panel1.SuspendLayout();
            this.dgPurchaseTransRep.SuspendLayout();
            this.SuspendLayout();
            // 
            // grdPurchaseProductTransaction
            // 
            this.grdPurchaseProductTransaction.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdPurchaseProductTransaction.EnableSort = true;
            this.grdPurchaseProductTransaction.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grdPurchaseProductTransaction.Location = new System.Drawing.Point(89, 25);
            this.grdPurchaseProductTransaction.Name = "grdPurchaseProductTransaction";
            this.grdPurchaseProductTransaction.OptimizeMode = SourceGrid.CellOptimizeMode.ForRows;
            this.grdPurchaseProductTransaction.SelectionMode = SourceGrid.GridSelectionMode.Cell;
            this.grdPurchaseProductTransaction.Size = new System.Drawing.Size(146, 313);
            this.grdPurchaseProductTransaction.TabIndex = 38;
            this.grdPurchaseProductTransaction.TabStop = true;
            this.grdPurchaseProductTransaction.ToolTipText = "";
            // 
            // lblCompanyName
            // 
            this.lblCompanyName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCompanyName.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCompanyName.Location = new System.Drawing.Point(167, 51);
            this.lblCompanyName.Name = "lblCompanyName";
            this.lblCompanyName.Size = new System.Drawing.Size(674, 29);
            this.lblCompanyName.TabIndex = 27;
            this.lblCompanyName.Text = "Company Name";
            this.lblCompanyName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblPanNo
            // 
            this.lblPanNo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPanNo.Font = new System.Drawing.Font("Arial", 6F, System.Drawing.FontStyle.Bold);
            this.lblPanNo.Location = new System.Drawing.Point(609, 38);
            this.lblPanNo.Name = "lblPanNo";
            this.lblPanNo.Size = new System.Drawing.Size(376, 21);
            this.lblPanNo.TabIndex = 43;
            this.lblPanNo.Text = "PAN No.";
            this.lblPanNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblPanNo.Visible = false;
            this.lblPanNo.Click += new System.EventHandler(this.lblPanNo_Click);
            // 
            // lblCompanyAddress
            // 
            this.lblCompanyAddress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCompanyAddress.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCompanyAddress.Location = new System.Drawing.Point(4, 80);
            this.lblCompanyAddress.Name = "lblCompanyAddress";
            this.lblCompanyAddress.Size = new System.Drawing.Size(1020, 18);
            this.lblCompanyAddress.TabIndex = 44;
            this.lblCompanyAddress.Text = "Address";
            this.lblCompanyAddress.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblContact
            // 
            this.lblContact.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblContact.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblContact.Location = new System.Drawing.Point(6, 96);
            this.lblContact.Name = "lblContact";
            this.lblContact.Size = new System.Drawing.Size(1018, 18);
            this.lblContact.TabIndex = 45;
            this.lblContact.Text = "Contact";
            this.lblContact.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblReportName
            // 
            this.lblReportName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblReportName.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblReportName.Location = new System.Drawing.Point(12, 115);
            this.lblReportName.Name = "lblReportName";
            this.lblReportName.Size = new System.Drawing.Size(1021, 28);
            this.lblReportName.TabIndex = 46;
            this.lblReportName.Text = "Purchase Report";
            this.lblReportName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblAllSettings
            // 
            this.lblAllSettings.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblAllSettings.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAllSettings.Location = new System.Drawing.Point(9, 137);
            this.lblAllSettings.Name = "lblAllSettings";
            this.lblAllSettings.Size = new System.Drawing.Size(251, 21);
            this.lblAllSettings.TabIndex = 44;
            this.lblAllSettings.Text = "Settings";
            this.lblAllSettings.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblProjectName
            // 
            this.lblProjectName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblProjectName.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProjectName.Location = new System.Drawing.Point(308, 152);
            this.lblProjectName.Name = "lblProjectName";
            this.lblProjectName.Size = new System.Drawing.Size(435, 21);
            this.lblProjectName.TabIndex = 47;
            this.lblProjectName.Text = "Project Name:";
            this.lblProjectName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblAsonDate
            // 
            this.lblAsonDate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblAsonDate.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAsonDate.Location = new System.Drawing.Point(625, 152);
            this.lblAsonDate.Name = "lblAsonDate";
            this.lblAsonDate.Size = new System.Drawing.Size(391, 21);
            this.lblAsonDate.TabIndex = 48;
            this.lblAsonDate.Text = "As On Date:";
            this.lblAsonDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1030, 35);
            this.panel1.TabIndex = 49;
            // 
            // btnExport
            // 
            this.btnExport.Image = global::Inventory.Properties.Resources.export1;
            this.btnExport.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnExport.Location = new System.Drawing.Point(72, 3);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(62, 29);
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
            this.btnPrintPreview.Location = new System.Drawing.Point(135, 3);
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
            this.btnRefresh.Image = global::Inventory.Properties.Resources.refresh;
            this.btnRefresh.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRefresh.Location = new System.Drawing.Point(3, 3);
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
            this.btnExit.Image = global::Inventory.Properties.Resources.ExitButton;
            this.btnExit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnExit.Location = new System.Drawing.Point(285, 3);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(50, 29);
            this.btnExit.TabIndex = 22;
            this.btnExit.Text = "E&xit";
            this.btnExit.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.Image = global::Inventory.Properties.Resources.print;
            this.btnPrint.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPrint.Location = new System.Drawing.Point(230, 3);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(54, 29);
            this.btnPrint.TabIndex = 2;
            this.btnPrint.Text = "&Print";
            this.btnPrint.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // lblProductPartyPivot
            // 
            this.lblProductPartyPivot.AutoSize = true;
            this.lblProductPartyPivot.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProductPartyPivot.Location = new System.Drawing.Point(9, 154);
            this.lblProductPartyPivot.Name = "lblProductPartyPivot";
            this.lblProductPartyPivot.Size = new System.Drawing.Size(47, 15);
            this.lblProductPartyPivot.TabIndex = 61;
            this.lblProductPartyPivot.Text = "label1";
            // 
            // dgPurchaseTransRep
            // 
            this.dgPurchaseTransRep.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgPurchaseTransRep.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.dgPurchaseTransRep.Controls.Add(this.grdPurchaseProductTransaction);
            this.dgPurchaseTransRep.DefaultHeight = 10;
            this.dgPurchaseTransRep.DefaultWidth = 100;
            this.dgPurchaseTransRep.DeleteQuestionMessage = "Are you sure to delete all the selected rows?";
            this.dgPurchaseTransRep.DeleteRowsWithDeleteKey = false;
            this.dgPurchaseTransRep.EnableSort = false;
            this.dgPurchaseTransRep.EndEditingRowOnValidate = false;
            this.dgPurchaseTransRep.FixedRows = 1;
            this.dgPurchaseTransRep.Location = new System.Drawing.Point(3, 180);
            this.dgPurchaseTransRep.Name = "dgPurchaseTransRep";
            this.dgPurchaseTransRep.SelectionMode = SourceGrid.GridSelectionMode.Row;
            this.dgPurchaseTransRep.Size = new System.Drawing.Size(1021, 379);
            this.dgPurchaseTransRep.TabIndex = 62;
            this.dgPurchaseTransRep.TabStop = true;
            this.dgPurchaseTransRep.ToolTipText = "";
            this.dgPurchaseTransRep.Visible = false;
            // 
            // frmProductPartyTransaction
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1030, 561);
            this.Controls.Add(this.dgPurchaseTransRep);
            this.Controls.Add(this.lblProductPartyPivot);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lblAsonDate);
            this.Controls.Add(this.lblProjectName);
            this.Controls.Add(this.lblAllSettings);
            this.Controls.Add(this.lblReportName);
            this.Controls.Add(this.lblContact);
            this.Controls.Add(this.lblCompanyAddress);
            this.Controls.Add(this.lblPanNo);
            this.Controls.Add(this.lblCompanyName);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmProductPartyTransaction";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Purchase-->>Product/Party Transaction";
            this.Load += new System.EventHandler(this.frmProductTransaction_Load);
            this.panel1.ResumeLayout(false);
            this.dgPurchaseTransRep.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private SourceGrid.Grid grdPurchaseProductTransaction;
        private System.Windows.Forms.Label lblCompanyName;
        private System.Windows.Forms.Label lblPanNo;
        private System.Windows.Forms.Label lblCompanyAddress;
        private System.Windows.Forms.Label lblContact;
        private System.Windows.Forms.Label lblReportName;
        private System.Windows.Forms.Label lblAllSettings;
        private System.Windows.Forms.Label lblProjectName;
        private System.Windows.Forms.Label lblAsonDate;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnPrintPreview;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Label lblProductPartyPivot;
        private SourceGrid.DataGrid dgPurchaseTransRep;
    }
}