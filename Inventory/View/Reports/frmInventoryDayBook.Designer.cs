namespace Inventory
{
    partial class frmPartyLedger
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPartyLedger));
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnPrintPreview = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.lblReportType = new System.Windows.Forms.Label();
            this.lblAsonDate = new System.Windows.Forms.Label();
            this.lblAllSettings = new System.Windows.Forms.Label();
            this.grdInventoryDayBook = new SourceGrid.Grid();
            this.dgPartyLedger = new SourceGrid.DataGrid();
            this.lblProject = new System.Windows.Forms.Label();
            this.lblInboundQty = new System.Windows.Forms.Label();
            this.lblOutboundQty = new System.Windows.Forms.Label();
            this.lblTotalAmount = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.dgPartyLedger.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.LightBlue;
            this.panel1.Controls.Add(this.btnExport);
            this.panel1.Controls.Add(this.btnPrintPreview);
            this.panel1.Controls.Add(this.btnRefresh);
            this.panel1.Controls.Add(this.button4);
            this.panel1.Controls.Add(this.button3);
            this.panel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(986, 35);
            this.panel1.TabIndex = 22;
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
            this.btnPrintPreview.Location = new System.Drawing.Point(130, 3);
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
            this.btnRefresh.Size = new System.Drawing.Size(73, 29);
            this.btnRefresh.TabIndex = 23;
            this.btnRefresh.Text = "&Refresh";
            this.btnRefresh.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // button4
            // 
            this.button4.Image = global::Inventory.Properties.Resources.ExitButton;
            this.button4.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button4.Location = new System.Drawing.Point(285, 3);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(54, 29);
            this.button4.TabIndex = 22;
            this.button4.Text = "E&xit";
            this.button4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button3
            // 
            this.button3.Image = global::Inventory.Properties.Resources.print;
            this.button3.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button3.Location = new System.Drawing.Point(230, 3);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(54, 29);
            this.button3.TabIndex = 2;
            this.button3.Text = "&Print";
            this.button3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // lblReportType
            // 
            this.lblReportType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblReportType.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblReportType.Location = new System.Drawing.Point(238, 36);
            this.lblReportType.Name = "lblReportType";
            this.lblReportType.Size = new System.Drawing.Size(509, 31);
            this.lblReportType.TabIndex = 47;
            this.lblReportType.Text = "Inventory Party Ledger";
            this.lblReportType.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblAsonDate
            // 
            this.lblAsonDate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblAsonDate.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAsonDate.Location = new System.Drawing.Point(684, 74);
            this.lblAsonDate.Name = "lblAsonDate";
            this.lblAsonDate.Size = new System.Drawing.Size(287, 21);
            this.lblAsonDate.TabIndex = 52;
            this.lblAsonDate.Text = "As On Date:";
            this.lblAsonDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblAllSettings
            // 
            this.lblAllSettings.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblAllSettings.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAllSettings.Location = new System.Drawing.Point(0, 74);
            this.lblAllSettings.Name = "lblAllSettings";
            this.lblAllSettings.Size = new System.Drawing.Size(339, 21);
            this.lblAllSettings.TabIndex = 48;
            this.lblAllSettings.Text = "Settings";
            this.lblAllSettings.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // grdInventoryDayBook
            // 
            this.grdInventoryDayBook.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdInventoryDayBook.DefaultHeight = 23;
            this.grdInventoryDayBook.EnableSort = true;
            this.grdInventoryDayBook.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grdInventoryDayBook.Location = new System.Drawing.Point(196, 68);
            this.grdInventoryDayBook.Name = "grdInventoryDayBook";
            this.grdInventoryDayBook.OptimizeMode = SourceGrid.CellOptimizeMode.ForRows;
            this.grdInventoryDayBook.SelectionMode = SourceGrid.GridSelectionMode.Cell;
            this.grdInventoryDayBook.Size = new System.Drawing.Size(376, 325);
            this.grdInventoryDayBook.TabIndex = 53;
            this.grdInventoryDayBook.TabStop = true;
            this.grdInventoryDayBook.ToolTipText = "";
            // 
            // dgPartyLedger
            // 
            this.dgPartyLedger.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgPartyLedger.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.dgPartyLedger.Controls.Add(this.grdInventoryDayBook);
            this.dgPartyLedger.DefaultHeight = 10;
            this.dgPartyLedger.DefaultWidth = 100;
            this.dgPartyLedger.DeleteQuestionMessage = "Are you sure to delete all the selected rows?";
            this.dgPartyLedger.DeleteRowsWithDeleteKey = false;
            this.dgPartyLedger.EnableSort = false;
            this.dgPartyLedger.EndEditingRowOnValidate = false;
            this.dgPartyLedger.FixedRows = 1;
            this.dgPartyLedger.Location = new System.Drawing.Point(2, 106);
            this.dgPartyLedger.Name = "dgPartyLedger";
            this.dgPartyLedger.SelectionMode = SourceGrid.GridSelectionMode.Row;
            this.dgPartyLedger.Size = new System.Drawing.Size(979, 481);
            this.dgPartyLedger.TabIndex = 55;
            this.dgPartyLedger.TabStop = true;
            this.dgPartyLedger.ToolTipText = "";
            // 
            // lblProject
            // 
            this.lblProject.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblProject.Location = new System.Drawing.Point(319, 67);
            this.lblProject.Name = "lblProject";
            this.lblProject.Size = new System.Drawing.Size(340, 21);
            this.lblProject.TabIndex = 56;
            this.lblProject.Text = "label1";
            this.lblProject.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblInboundQty
            // 
            this.lblInboundQty.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblInboundQty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblInboundQty.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInboundQty.Location = new System.Drawing.Point(184, 599);
            this.lblInboundQty.Name = "lblInboundQty";
            this.lblInboundQty.Size = new System.Drawing.Size(274, 20);
            this.lblInboundQty.TabIndex = 57;
            this.lblInboundQty.Text = "InBound Qty";
            this.lblInboundQty.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblOutboundQty
            // 
            this.lblOutboundQty.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblOutboundQty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblOutboundQty.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOutboundQty.Location = new System.Drawing.Point(471, 599);
            this.lblOutboundQty.Name = "lblOutboundQty";
            this.lblOutboundQty.Size = new System.Drawing.Size(238, 20);
            this.lblOutboundQty.TabIndex = 57;
            this.lblOutboundQty.Text = "Outbound Qty";
            this.lblOutboundQty.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblTotalAmount
            // 
            this.lblTotalAmount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTotalAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblTotalAmount.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalAmount.Location = new System.Drawing.Point(722, 599);
            this.lblTotalAmount.Name = "lblTotalAmount";
            this.lblTotalAmount.Size = new System.Drawing.Size(240, 20);
            this.lblTotalAmount.TabIndex = 57;
            this.lblTotalAmount.Text = "TotalAmount";
            this.lblTotalAmount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // frmPartyLedger
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(982, 633);
            this.Controls.Add(this.lblTotalAmount);
            this.Controls.Add(this.lblOutboundQty);
            this.Controls.Add(this.lblInboundQty);
            this.Controls.Add(this.lblProject);
            this.Controls.Add(this.dgPartyLedger);
            this.Controls.Add(this.lblAllSettings);
            this.Controls.Add(this.lblAsonDate);
            this.Controls.Add(this.lblReportType);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "frmPartyLedger";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Party Ledger";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmInventoryDayBook_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmInventoryDayBook_KeyDown);
            this.panel1.ResumeLayout(false);
            this.dgPartyLedger.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnPrintPreview;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label lblReportType;
        private System.Windows.Forms.Label lblAsonDate;
        private System.Windows.Forms.Label lblAllSettings;
        private SourceGrid.Grid grdInventoryDayBook;
        private SourceGrid.DataGrid dgPartyLedger;
        private System.Windows.Forms.Label lblProject;
        private System.Windows.Forms.Label lblInboundQty;
        private System.Windows.Forms.Label lblOutboundQty;
        private System.Windows.Forms.Label lblTotalAmount;

    }
}