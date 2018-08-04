namespace Inventory
{
    partial class frmStockLedger
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmStockLedger));
            this.lblAllSettings = new System.Windows.Forms.Label();
            this.grdStockLedger = new SourceGrid.Grid();
            this.lblAsonDate = new System.Windows.Forms.Label();
            this.lblProjectName = new System.Windows.Forms.Label();
            this.lblReportType = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnPrintPreview = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.lblOpeningQty = new System.Windows.Forms.Label();
            this.lblClosingQty = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.dgStockLedger = new SourceGrid.DataGrid();
            this.lblProductName = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.dgStockLedger.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblAllSettings
            // 
            this.lblAllSettings.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAllSettings.Location = new System.Drawing.Point(12, 72);
            this.lblAllSettings.Name = "lblAllSettings";
            this.lblAllSettings.Size = new System.Drawing.Size(193, 21);
            this.lblAllSettings.TabIndex = 58;
            this.lblAllSettings.Text = "Settings";
            this.lblAllSettings.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // grdStockLedger
            // 
            this.grdStockLedger.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdStockLedger.EnableSort = true;
            this.grdStockLedger.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grdStockLedger.Location = new System.Drawing.Point(196, 31);
            this.grdStockLedger.Name = "grdStockLedger";
            this.grdStockLedger.OptimizeMode = SourceGrid.CellOptimizeMode.ForRows;
            this.grdStockLedger.SelectionMode = SourceGrid.GridSelectionMode.Cell;
            this.grdStockLedger.Size = new System.Drawing.Size(41, 228);
            this.grdStockLedger.TabIndex = 64;
            this.grdStockLedger.TabStop = true;
            this.grdStockLedger.ToolTipText = "";
            // 
            // lblAsonDate
            // 
            this.lblAsonDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblAsonDate.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAsonDate.Location = new System.Drawing.Point(558, 72);
            this.lblAsonDate.Name = "lblAsonDate";
            this.lblAsonDate.Size = new System.Drawing.Size(222, 21);
            this.lblAsonDate.TabIndex = 63;
            this.lblAsonDate.Text = "As On Date:";
            this.lblAsonDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblProjectName
            // 
            this.lblProjectName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblProjectName.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProjectName.Location = new System.Drawing.Point(7, 65);
            this.lblProjectName.Name = "lblProjectName";
            this.lblProjectName.Size = new System.Drawing.Size(780, 28);
            this.lblProjectName.TabIndex = 62;
            this.lblProjectName.Text = "Project Name:";
            this.lblProjectName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblProjectName.Click += new System.EventHandler(this.lblProjectName_Click);
            // 
            // lblReportType
            // 
            this.lblReportType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblReportType.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblReportType.Location = new System.Drawing.Point(12, 37);
            this.lblReportType.Name = "lblReportType";
            this.lblReportType.Size = new System.Drawing.Size(775, 35);
            this.lblReportType.TabIndex = 57;
            this.lblReportType.Text = "Stock Ledger";
            this.lblReportType.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
            this.panel1.Location = new System.Drawing.Point(-3, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(798, 35);
            this.panel1.TabIndex = 56;
            // 
            // btnExport
            // 
            this.btnExport.Image = global::Inventory.Properties.Resources.export1;
            this.btnExport.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnExport.Location = new System.Drawing.Point(81, 3);
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
            this.btnPrintPreview.Location = new System.Drawing.Point(144, 3);
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
            this.btnRefresh.Location = new System.Drawing.Point(7, 3);
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
            this.button4.Location = new System.Drawing.Point(294, 3);
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
            this.button3.Location = new System.Drawing.Point(239, 3);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(54, 29);
            this.button3.TabIndex = 2;
            this.button3.Text = "&Print";
            this.button3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // lblOpeningQty
            // 
            this.lblOpeningQty.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblOpeningQty.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOpeningQty.Location = new System.Drawing.Point(616, 102);
            this.lblOpeningQty.Name = "lblOpeningQty";
            this.lblOpeningQty.Size = new System.Drawing.Size(163, 23);
            this.lblOpeningQty.TabIndex = 67;
            this.lblOpeningQty.Text = "Opening Qty: 0";
            this.lblOpeningQty.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblClosingQty
            // 
            this.lblClosingQty.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblClosingQty.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblClosingQty.Location = new System.Drawing.Point(638, 439);
            this.lblClosingQty.Name = "lblClosingQty";
            this.lblClosingQty.Size = new System.Drawing.Size(144, 20);
            this.lblClosingQty.TabIndex = 68;
            this.lblClosingQty.Text = "Closing Qty: 0";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(636, 418);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(144, 17);
            this.label2.TabIndex = 69;
            this.label2.Text = "________________________";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(636, 449);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(144, 17);
            this.label3.TabIndex = 70;
            this.label3.Text = "________________________";
            // 
            // dgStockLedger
            // 
            this.dgStockLedger.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgStockLedger.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.dgStockLedger.Controls.Add(this.grdStockLedger);
            this.dgStockLedger.DefaultHeight = 10;
            this.dgStockLedger.DefaultWidth = 100;
            this.dgStockLedger.DeleteQuestionMessage = "Are you sure to delete all the selected rows?";
            this.dgStockLedger.DeleteRowsWithDeleteKey = false;
            this.dgStockLedger.EnableSort = false;
            this.dgStockLedger.EndEditingRowOnValidate = false;
            this.dgStockLedger.FixedRows = 1;
            this.dgStockLedger.Location = new System.Drawing.Point(7, 128);
            this.dgStockLedger.Name = "dgStockLedger";
            this.dgStockLedger.SelectionMode = SourceGrid.GridSelectionMode.Row;
            this.dgStockLedger.Size = new System.Drawing.Size(780, 295);
            this.dgStockLedger.TabIndex = 79;
            this.dgStockLedger.TabStop = true;
            this.dgStockLedger.ToolTipText = "";
            this.dgStockLedger.Visible = false;
            // 
            // lblProductName
            // 
            this.lblProductName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblProductName.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProductName.Location = new System.Drawing.Point(190, 95);
            this.lblProductName.Name = "lblProductName";
            this.lblProductName.Size = new System.Drawing.Size(407, 20);
            this.lblProductName.TabIndex = 80;
            this.lblProductName.Text = "Product Name";
            this.lblProductName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // frmStockLedger
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(794, 475);
            this.Controls.Add(this.lblProductName);
            this.Controls.Add(this.dgStockLedger);
            this.Controls.Add(this.lblClosingQty);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblOpeningQty);
            this.Controls.Add(this.lblAllSettings);
            this.Controls.Add(this.lblAsonDate);
            this.Controls.Add(this.lblProjectName);
            this.Controls.Add(this.lblReportType);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmStockLedger";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Stock Ledger";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmStockLedger_Load);
            this.panel1.ResumeLayout(false);
            this.dgStockLedger.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblAllSettings;
        private SourceGrid.Grid grdStockLedger;
        private System.Windows.Forms.Label lblAsonDate;
        private System.Windows.Forms.Label lblProjectName;
        private System.Windows.Forms.Label lblReportType;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnPrintPreview;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label lblOpeningQty;
        private System.Windows.Forms.Label lblClosingQty;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private SourceGrid.DataGrid dgStockLedger;
        private System.Windows.Forms.Label lblProductName;
    }
}