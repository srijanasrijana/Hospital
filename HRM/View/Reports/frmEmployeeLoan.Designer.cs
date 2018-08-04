namespace HRM.View.Reports
{
    partial class frmEmployeeLoan
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmEmployeeLoan));
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnPrintPreview = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblYear = new System.Windows.Forms.Label();
            this.lblMonth = new System.Windows.Forms.Label();
            this.grdLoan = new SourceGrid.Grid();
            this.lblLoan = new System.Windows.Forms.Label();
            this.lblAsOnDate = new System.Windows.Forms.Label();
            this.dgLoan = new SourceGrid.DataGrid();
            this.label6 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
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
            this.panel1.Controls.Add(this.btnExit);
            this.panel1.Controls.Add(this.btnPrint);
            this.panel1.Location = new System.Drawing.Point(1, 1);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1039, 35);
            this.panel1.TabIndex = 28;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
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
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
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
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
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
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.Controls.Add(this.lblYear);
            this.panel2.Controls.Add(this.lblMonth);
            this.panel2.Controls.Add(this.grdLoan);
            this.panel2.Controls.Add(this.lblLoan);
            this.panel2.Controls.Add(this.lblAsOnDate);
            this.panel2.Controls.Add(this.dgLoan);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Controls.Add(this.statusStrip1);
            this.panel2.Location = new System.Drawing.Point(3, 42);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1037, 575);
            this.panel2.TabIndex = 29;
            // 
            // lblYear
            // 
            this.lblYear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblYear.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblYear.Location = new System.Drawing.Point(515, 36);
            this.lblYear.Name = "lblYear";
            this.lblYear.Size = new System.Drawing.Size(190, 15);
            this.lblYear.TabIndex = 44;
            this.lblYear.Text = "Year : ";
            this.lblYear.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblMonth
            // 
            this.lblMonth.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMonth.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMonth.Location = new System.Drawing.Point(333, 36);
            this.lblMonth.Name = "lblMonth";
            this.lblMonth.Size = new System.Drawing.Size(166, 15);
            this.lblMonth.TabIndex = 41;
            this.lblMonth.Text = "Month : ";
            this.lblMonth.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // grdLoan
            // 
            this.grdLoan.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdLoan.EnableSort = true;
            this.grdLoan.FixedRows = 1;
            this.grdLoan.Location = new System.Drawing.Point(9, 88);
            this.grdLoan.Name = "grdLoan";
            this.grdLoan.OptimizeMode = SourceGrid.CellOptimizeMode.ForRows;
            this.grdLoan.SelectionMode = SourceGrid.GridSelectionMode.Cell;
            this.grdLoan.Size = new System.Drawing.Size(1021, 441);
            this.grdLoan.TabIndex = 4;
            this.grdLoan.TabStop = true;
            this.grdLoan.ToolTipText = "";
            this.grdLoan.Paint += new System.Windows.Forms.PaintEventHandler(this.grdLoan_Paint);
            // 
            // lblLoan
            // 
            this.lblLoan.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLoan.Location = new System.Drawing.Point(12, 36);
            this.lblLoan.Name = "lblLoan";
            this.lblLoan.Size = new System.Drawing.Size(288, 15);
            this.lblLoan.TabIndex = 38;
            this.lblLoan.Text = "Loan : ";
            this.lblLoan.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblAsOnDate
            // 
            this.lblAsOnDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblAsOnDate.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAsOnDate.Location = new System.Drawing.Point(747, 36);
            this.lblAsOnDate.Name = "lblAsOnDate";
            this.lblAsOnDate.Size = new System.Drawing.Size(283, 15);
            this.lblAsOnDate.TabIndex = 37;
            this.lblAsOnDate.Text = "As On Date:";
            this.lblAsOnDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dgLoan
            // 
            this.dgLoan.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgLoan.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.dgLoan.DefaultHeight = 10;
            this.dgLoan.DefaultWidth = 100;
            this.dgLoan.DeleteQuestionMessage = "Are you sure to delete all the selected rows?";
            this.dgLoan.DeleteRowsWithDeleteKey = false;
            this.dgLoan.EnableSort = false;
            this.dgLoan.EndEditingRowOnValidate = false;
            this.dgLoan.FixedRows = 1;
            this.dgLoan.Location = new System.Drawing.Point(3, 535);
            this.dgLoan.Name = "dgLoan";
            this.dgLoan.SelectionMode = SourceGrid.GridSelectionMode.Row;
            this.dgLoan.Size = new System.Drawing.Size(530, 13);
            this.dgLoan.TabIndex = 5;
            this.dgLoan.TabStop = true;
            this.dgLoan.ToolTipText = "";
            this.dgLoan.Visible = false;
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(29, 8);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(992, 22);
            this.label6.TabIndex = 30;
            this.label6.Text = "Loan Report";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel3
            // 
            this.panel3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel3.Location = new System.Drawing.Point(2, 554);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1035, 18);
            this.panel3.TabIndex = 7;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 464);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1012, 22);
            this.statusStrip1.TabIndex = 6;
            this.statusStrip1.Text = "statusStrip1";
            this.statusStrip1.Visible = false;
            // 
            // frmEmployeeLoan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1042, 623);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmEmployeeLoan";
            this.Text = "Employee Loan";
            this.Load += new System.EventHandler(this.frmEmployeeLoan_Load);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnPrintPreview;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lblYear;
        private System.Windows.Forms.Label lblMonth;
        private System.Windows.Forms.Label lblLoan;
        private System.Windows.Forms.Label lblAsOnDate;
        private SourceGrid.DataGrid dgLoan;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private SourceGrid.Grid grdLoan;
    }
}