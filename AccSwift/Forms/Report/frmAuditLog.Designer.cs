namespace AccSwift.Forms.Report
{
    partial class frmAuditLogReport
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnPrintPreview = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.dgAuditLog = new SourceGrid.DataGrid();
            this.grdAuditLog = new SourceGrid.Grid();
            this.lblFromToDate = new System.Windows.Forms.Label();
            this.lblAsOnDate = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.dgAuditLog.SuspendLayout();
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
            this.panel1.Location = new System.Drawing.Point(1, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1029, 35);
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
            // button4
            // 
            this.button4.Image = global::Inventory.Properties.Resources.ExitButton;
            this.button4.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button4.Location = new System.Drawing.Point(285, 3);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(50, 29);
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
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(328, 38);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(368, 28);
            this.label5.TabIndex = 34;
            this.label5.Text = "Audit Log";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label5.Click += new System.EventHandler(this.label5_Click);
            // 
            // dgAuditLog
            // 
            this.dgAuditLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgAuditLog.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.dgAuditLog.AutoStretchColumnsToFitWidth = true;
            this.dgAuditLog.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.dgAuditLog.Controls.Add(this.grdAuditLog);
            this.dgAuditLog.DefaultHeight = 10;
            this.dgAuditLog.DefaultWidth = 100;
            this.dgAuditLog.DeleteQuestionMessage = "Are you sure to delete all the selected rows?";
            this.dgAuditLog.DeleteRowsWithDeleteKey = false;
            this.dgAuditLog.EnableSort = false;
            this.dgAuditLog.EndEditingRowOnValidate = false;
            this.dgAuditLog.FixedRows = 1;
            this.dgAuditLog.Location = new System.Drawing.Point(1, 88);
            this.dgAuditLog.Name = "dgAuditLog";
            this.dgAuditLog.SelectionMode = SourceGrid.GridSelectionMode.Row;
            this.dgAuditLog.Size = new System.Drawing.Size(1018, 479);
            this.dgAuditLog.TabIndex = 39;
            this.dgAuditLog.TabStop = true;
            this.dgAuditLog.ToolTipText = "";
            this.dgAuditLog.Visible = false;
            this.dgAuditLog.Paint += new System.Windows.Forms.PaintEventHandler(this.dgAuditLog_Paint);
            // 
            // grdAuditLog
            // 
            this.grdAuditLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdAuditLog.DefaultHeight = 23;
            this.grdAuditLog.EnableSort = true;
            this.grdAuditLog.Location = new System.Drawing.Point(133, 55);
            this.grdAuditLog.Name = "grdAuditLog";
            this.grdAuditLog.OptimizeMode = SourceGrid.CellOptimizeMode.ForRows;
            this.grdAuditLog.SelectionMode = SourceGrid.GridSelectionMode.Cell;
            this.grdAuditLog.Size = new System.Drawing.Size(345, 258);
            this.grdAuditLog.TabIndex = 38;
            this.grdAuditLog.TabStop = true;
            this.grdAuditLog.ToolTipText = "";
            // 
            // lblFromToDate
            // 
            this.lblFromToDate.AutoSize = true;
            this.lblFromToDate.Font = new System.Drawing.Font("Arial Narrow", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFromToDate.Location = new System.Drawing.Point(30, 69);
            this.lblFromToDate.Name = "lblFromToDate";
            this.lblFromToDate.Size = new System.Drawing.Size(0, 16);
            this.lblFromToDate.TabIndex = 40;
            // 
            // lblAsOnDate
            // 
            this.lblAsOnDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblAsOnDate.AutoSize = true;
            this.lblAsOnDate.Font = new System.Drawing.Font("Arial Narrow", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAsOnDate.Location = new System.Drawing.Point(847, 69);
            this.lblAsOnDate.Name = "lblAsOnDate";
            this.lblAsOnDate.Size = new System.Drawing.Size(0, 16);
            this.lblAsOnDate.TabIndex = 41;
            // 
            // frmAuditLogReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1030, 567);
            this.Controls.Add(this.lblAsOnDate);
            this.Controls.Add(this.lblFromToDate);
            this.Controls.Add(this.dgAuditLog);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.Name = "frmAuditLogReport";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Audit Log";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmAuditLogReport_Load);
            this.panel1.ResumeLayout(false);
            this.dgAuditLog.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnPrintPreview;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label5;
        private SourceGrid.DataGrid dgAuditLog;
        private System.Windows.Forms.Label lblFromToDate;
        private System.Windows.Forms.Label lblAsOnDate;
        private SourceGrid.Grid grdAuditLog;
    }
}