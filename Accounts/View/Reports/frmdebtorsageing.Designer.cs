namespace Accounts.Reports
{
    partial class frmdebtorsageing
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmdebtorsageing));
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnPrintPreview = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lblAsonDate = new System.Windows.Forms.Label();
            this.grddebtorsageing = new SourceGrid.Grid();
            this.dgDebtorsAgeing = new SourceGrid.DataGrid();
            this.lblFiscalYear = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.dgDebtorsAgeing.SuspendLayout();
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
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1069, 35);
            this.panel1.TabIndex = 21;
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
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click_1);
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
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(-1, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(1068, 28);
            this.label1.TabIndex = 22;
            this.label1.Text = "Debtors Ageing Report";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblAsonDate
            // 
            this.lblAsonDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblAsonDate.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAsonDate.Location = new System.Drawing.Point(889, 65);
            this.lblAsonDate.Name = "lblAsonDate";
            this.lblAsonDate.Size = new System.Drawing.Size(167, 21);
            this.lblAsonDate.TabIndex = 30;
            this.lblAsonDate.Text = "As On Date:";
            this.lblAsonDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // grddebtorsageing
            // 
            this.grddebtorsageing.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grddebtorsageing.DefaultHeight = 23;
            this.grddebtorsageing.EnableSort = true;
            this.grddebtorsageing.Location = new System.Drawing.Point(81, 43);
            this.grddebtorsageing.Name = "grddebtorsageing";
            this.grddebtorsageing.OptimizeMode = SourceGrid.CellOptimizeMode.ForRows;
            this.grddebtorsageing.SelectionMode = SourceGrid.GridSelectionMode.Cell;
            this.grddebtorsageing.Size = new System.Drawing.Size(285, 354);
            this.grddebtorsageing.TabIndex = 31;
            this.grddebtorsageing.TabStop = true;
            this.grddebtorsageing.ToolTipText = "";
            // 
            // dgDebtorsAgeing
            // 
            this.dgDebtorsAgeing.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgDebtorsAgeing.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.dgDebtorsAgeing.Controls.Add(this.grddebtorsageing);
            this.dgDebtorsAgeing.DefaultHeight = 10;
            this.dgDebtorsAgeing.DefaultWidth = 100;
            this.dgDebtorsAgeing.DeleteQuestionMessage = "Are you sure to delete all the selected rows?";
            this.dgDebtorsAgeing.DeleteRowsWithDeleteKey = false;
            this.dgDebtorsAgeing.EnableSort = false;
            this.dgDebtorsAgeing.EndEditingRowOnValidate = false;
            this.dgDebtorsAgeing.FixedRows = 1;
            this.dgDebtorsAgeing.Location = new System.Drawing.Point(8, 89);
            this.dgDebtorsAgeing.Name = "dgDebtorsAgeing";
            this.dgDebtorsAgeing.SelectionMode = SourceGrid.GridSelectionMode.Row;
            this.dgDebtorsAgeing.Size = new System.Drawing.Size(1056, 432);
            this.dgDebtorsAgeing.TabIndex = 32;
            this.dgDebtorsAgeing.TabStop = true;
            this.dgDebtorsAgeing.ToolTipText = "";
            this.dgDebtorsAgeing.Visible = false;
            // 
            // lblFiscalYear
            // 
            this.lblFiscalYear.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFiscalYear.Location = new System.Drawing.Point(12, 65);
            this.lblFiscalYear.Name = "lblFiscalYear";
            this.lblFiscalYear.Size = new System.Drawing.Size(167, 21);
            this.lblFiscalYear.TabIndex = 33;
            this.lblFiscalYear.Text = "Fiscal Year:";
            this.lblFiscalYear.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // frmdebtorsageing
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1068, 523);
            this.Controls.Add(this.lblFiscalYear);
            this.Controls.Add(this.dgDebtorsAgeing);
            this.Controls.Add(this.lblAsonDate);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmdebtorsageing";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Debtors Ageing";
            this.Load += new System.EventHandler(this.frmdebtorsageing_Load);
            this.panel1.ResumeLayout(false);
            this.dgDebtorsAgeing.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnPrintPreview;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblAsonDate;
        private SourceGrid.Grid grddebtorsageing;
        private SourceGrid.DataGrid dgDebtorsAgeing;
        private System.Windows.Forms.Label lblFiscalYear;
    }
}