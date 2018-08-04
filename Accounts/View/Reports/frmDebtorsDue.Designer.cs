namespace Accounts
{
    partial class frmDebtorsDueDisplay
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDebtorsDueDisplay));
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnPrintPreview = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.lblAsonDate = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.grddebtorsdue = new SourceGrid.Grid();
            this.dgdebtorsdue = new SourceGrid.DataGrid();
            this.label2 = new System.Windows.Forms.Label();
            this.lblTotalDebtorDue = new System.Windows.Forms.Label();
            this.lblFiscalYear = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.dgdebtorsdue.SuspendLayout();
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
            this.panel1.Controls.Add(this.btnPrint);
            this.panel1.Location = new System.Drawing.Point(0, 1);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(911, 35);
            this.panel1.TabIndex = 22;
            // 
            // btnExport
            // 
            this.btnExport.Image = global::Accounts.Properties.Resources.export1;
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
            this.btnPrintPreview.Image = global::Accounts.Properties.Resources.print_preview;
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
            this.btnRefresh.Image = global::Accounts.Properties.Resources.refresh;
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
            this.button4.Image = global::Accounts.Properties.Resources.ExitButton;
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
            // btnPrint
            // 
            this.btnPrint.Image = global::Accounts.Properties.Resources.print;
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
            // lblAsonDate
            // 
            this.lblAsonDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblAsonDate.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAsonDate.Location = new System.Drawing.Point(728, 66);
            this.lblAsonDate.Name = "lblAsonDate";
            this.lblAsonDate.Size = new System.Drawing.Size(167, 21);
            this.lblAsonDate.TabIndex = 32;
            this.lblAsonDate.Text = "As On Date:";
            this.lblAsonDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblAsonDate.Click += new System.EventHandler(this.lblAsonDate_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(-1, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(907, 28);
            this.label1.TabIndex = 31;
            this.label1.Text = "Debtors Due";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // grddebtorsdue
            // 
            this.grddebtorsdue.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grddebtorsdue.DefaultHeight = 23;
            this.grddebtorsdue.EnableSort = true;
            this.grddebtorsdue.Location = new System.Drawing.Point(102, 20);
            this.grddebtorsdue.Name = "grddebtorsdue";
            this.grddebtorsdue.OptimizeMode = SourceGrid.CellOptimizeMode.ForRows;
            this.grddebtorsdue.SelectionMode = SourceGrid.GridSelectionMode.Cell;
            this.grddebtorsdue.Size = new System.Drawing.Size(316, 271);
            this.grddebtorsdue.TabIndex = 33;
            this.grddebtorsdue.TabStop = true;
            this.grddebtorsdue.ToolTipText = "";
            // 
            // dgdebtorsdue
            // 
            this.dgdebtorsdue.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgdebtorsdue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.dgdebtorsdue.Controls.Add(this.grddebtorsdue);
            this.dgdebtorsdue.DefaultHeight = 10;
            this.dgdebtorsdue.DefaultWidth = 100;
            this.dgdebtorsdue.DeleteQuestionMessage = "Are you sure to delete all the selected rows?";
            this.dgdebtorsdue.DeleteRowsWithDeleteKey = false;
            this.dgdebtorsdue.EnableSort = false;
            this.dgdebtorsdue.EndEditingRowOnValidate = false;
            this.dgdebtorsdue.FixedRows = 1;
            this.dgdebtorsdue.Location = new System.Drawing.Point(1, 88);
            this.dgdebtorsdue.Name = "dgdebtorsdue";
            this.dgdebtorsdue.SelectionMode = SourceGrid.GridSelectionMode.Row;
            this.dgdebtorsdue.Size = new System.Drawing.Size(909, 330);
            this.dgdebtorsdue.TabIndex = 34;
            this.dgdebtorsdue.TabStop = true;
            this.dgdebtorsdue.ToolTipText = "";
            this.dgdebtorsdue.Visible = false;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(622, 428);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(111, 13);
            this.label2.TabIndex = 35;
            this.label2.Text = "Total Debtors Due";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // lblTotalDebtorDue
            // 
            this.lblTotalDebtorDue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTotalDebtorDue.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalDebtorDue.Location = new System.Drawing.Point(746, 423);
            this.lblTotalDebtorDue.Name = "lblTotalDebtorDue";
            this.lblTotalDebtorDue.Size = new System.Drawing.Size(130, 25);
            this.lblTotalDebtorDue.TabIndex = 36;
            this.lblTotalDebtorDue.Text = "0";
            this.lblTotalDebtorDue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblTotalDebtorDue.Click += new System.EventHandler(this.lblTotalDebitAmount_Click);
            // 
            // lblFiscalYear
            // 
            this.lblFiscalYear.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFiscalYear.Location = new System.Drawing.Point(12, 66);
            this.lblFiscalYear.Name = "lblFiscalYear";
            this.lblFiscalYear.Size = new System.Drawing.Size(167, 19);
            this.lblFiscalYear.TabIndex = 37;
            this.lblFiscalYear.Text = "Fiscal Year:";
            this.lblFiscalYear.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblFiscalYear.Click += new System.EventHandler(this.lblFiscalYear_Click);
            // 
            // frmDebtorsDueDisplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(911, 455);
            this.Controls.Add(this.lblFiscalYear);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblTotalDebtorDue);
            this.Controls.Add(this.dgdebtorsdue);
            this.Controls.Add(this.lblAsonDate);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmDebtorsDueDisplay";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Debtors Due Display";
            this.Load += new System.EventHandler(this.frmDebtorsDueDisplay_Load);
            this.panel1.ResumeLayout(false);
            this.dgdebtorsdue.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnPrintPreview;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Label lblAsonDate;
        private System.Windows.Forms.Label label1;
        private SourceGrid.Grid grddebtorsdue;
        private SourceGrid.DataGrid dgdebtorsdue;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblTotalDebtorDue;
        private System.Windows.Forms.Label lblFiscalYear;
    }
}