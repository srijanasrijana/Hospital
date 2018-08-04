namespace Common
{
    partial class frmVoucherList
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmVoucherList));
            this.btnLast = new System.Windows.Forms.Button();
            this.btnFirst = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnPrev = new System.Windows.Forms.Button();
            this.txtSearchValue = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnExport = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnToDate = new System.Windows.Forms.Button();
            this.btnFromDate = new System.Windows.Forms.Button();
            this.txtToDate = new SComponents.SMaskedTextBox();
            this.btnForgetSearch = new System.Windows.Forms.Button();
            this.txtFromDate = new SComponents.SMaskedTextBox();
            this.btnPrint = new System.Windows.Forms.Button();
            this.grdVoucherList = new SourceGrid.Grid();
            this.lblCreatedBy = new System.Windows.Forms.Label();
            this.cboCreatedBy = new System.Windows.Forms.ComboBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnLast
            // 
            this.btnLast.Image = global::Common.Properties.Resources.last;
            this.btnLast.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnLast.Location = new System.Drawing.Point(186, 8);
            this.btnLast.Name = "btnLast";
            this.btnLast.Size = new System.Drawing.Size(62, 23);
            this.btnLast.TabIndex = 3;
            this.btnLast.Text = "Last";
            this.btnLast.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLast.UseVisualStyleBackColor = true;
            this.btnLast.Click += new System.EventHandler(this.btnLast_Click);
            // 
            // btnFirst
            // 
            this.btnFirst.Image = global::Common.Properties.Resources.first;
            this.btnFirst.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnFirst.Location = new System.Drawing.Point(9, 8);
            this.btnFirst.Name = "btnFirst";
            this.btnFirst.Size = new System.Drawing.Size(62, 23);
            this.btnFirst.TabIndex = 0;
            this.btnFirst.Text = "First";
            this.btnFirst.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnFirst.UseVisualStyleBackColor = true;
            this.btnFirst.Click += new System.EventHandler(this.btnFirst_Click);
            // 
            // btnNext
            // 
            this.btnNext.Image = global::Common.Properties.Resources.forward;
            this.btnNext.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnNext.Location = new System.Drawing.Point(127, 8);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(62, 23);
            this.btnNext.TabIndex = 2;
            this.btnNext.Text = "&Next";
            this.btnNext.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnPrev
            // 
            this.btnPrev.Image = global::Common.Properties.Resources.back;
            this.btnPrev.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPrev.Location = new System.Drawing.Point(68, 8);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(62, 23);
            this.btnPrev.TabIndex = 1;
            this.btnPrev.Text = "&Prev";
            this.btnPrev.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnPrev.UseVisualStyleBackColor = true;
            this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // txtSearchValue
            // 
            this.txtSearchValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSearchValue.Location = new System.Drawing.Point(907, 10);
            this.txtSearchValue.Name = "txtSearchValue";
            this.txtSearchValue.Size = new System.Drawing.Size(86, 20);
            this.txtSearchValue.TabIndex = 5;
            this.txtSearchValue.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSearchValue_KeyDown);
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.Image = global::Common.Properties.Resources._1296834456_xmag;
            this.btnSearch.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSearch.Location = new System.Drawing.Point(1008, 8);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(67, 23);
            this.btnSearch.TabIndex = 6;
            this.btnSearch.Text = "Search";
            this.btnSearch.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Silver;
            this.panel1.Controls.Add(this.lblCreatedBy);
            this.panel1.Controls.Add(this.cboCreatedBy);
            this.panel1.Controls.Add(this.btnExport);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.btnToDate);
            this.panel1.Controls.Add(this.btnFromDate);
            this.panel1.Controls.Add(this.txtToDate);
            this.panel1.Controls.Add(this.btnForgetSearch);
            this.panel1.Controls.Add(this.txtFromDate);
            this.panel1.Controls.Add(this.btnPrint);
            this.panel1.Controls.Add(this.btnNext);
            this.panel1.Controls.Add(this.btnPrev);
            this.panel1.Controls.Add(this.txtSearchValue);
            this.panel1.Controls.Add(this.btnFirst);
            this.panel1.Controls.Add(this.btnSearch);
            this.panel1.Controls.Add(this.btnLast);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1150, 39);
            this.panel1.TabIndex = 1;
            // 
            // btnExport
            // 
            this.btnExport.Image = global::Common.Properties.Resources.export1;
            this.btnExport.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnExport.Location = new System.Drawing.Point(304, 8);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(62, 23);
            this.btnExport.TabIndex = 28;
            this.btnExport.Text = "&Export";
            this.btnExport.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(746, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(26, 13);
            this.label2.TabIndex = 217;
            this.label2.Text = "To :";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(562, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 13);
            this.label1.TabIndex = 217;
            this.label1.Text = "From :";
            // 
            // btnToDate
            // 
            this.btnToDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnToDate.BackColor = System.Drawing.Color.FloralWhite;
            this.btnToDate.Image = ((System.Drawing.Image)(resources.GetObject("btnToDate.Image")));
            this.btnToDate.Location = new System.Drawing.Point(865, 8);
            this.btnToDate.Name = "btnToDate";
            this.btnToDate.Size = new System.Drawing.Size(26, 23);
            this.btnToDate.TabIndex = 216;
            this.btnToDate.UseVisualStyleBackColor = false;
            this.btnToDate.Click += new System.EventHandler(this.btnToDate_Click);
            // 
            // btnFromDate
            // 
            this.btnFromDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFromDate.BackColor = System.Drawing.Color.FloralWhite;
            this.btnFromDate.Image = ((System.Drawing.Image)(resources.GetObject("btnFromDate.Image")));
            this.btnFromDate.Location = new System.Drawing.Point(714, 8);
            this.btnFromDate.Name = "btnFromDate";
            this.btnFromDate.Size = new System.Drawing.Size(26, 23);
            this.btnFromDate.TabIndex = 216;
            this.btnFromDate.UseVisualStyleBackColor = false;
            this.btnFromDate.Click += new System.EventHandler(this.btnFromDate_Click);
            // 
            // txtToDate
            // 
            this.txtToDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtToDate.BackColor = System.Drawing.Color.FloralWhite;
            this.txtToDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtToDate.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.txtToDate.FocusLostColor = System.Drawing.Color.White;
            this.txtToDate.Location = new System.Drawing.Point(778, 10);
            this.txtToDate.Mask = "0000/00/00";
            this.txtToDate.Name = "txtToDate";
            this.txtToDate.Size = new System.Drawing.Size(81, 20);
            this.txtToDate.TabIndex = 215;
            this.txtToDate.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtToDate_KeyDown);
            // 
            // btnForgetSearch
            // 
            this.btnForgetSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnForgetSearch.Image = global::Common.Properties.Resources.gnome_window_close;
            this.btnForgetSearch.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnForgetSearch.Location = new System.Drawing.Point(1076, 8);
            this.btnForgetSearch.Name = "btnForgetSearch";
            this.btnForgetSearch.Size = new System.Drawing.Size(62, 23);
            this.btnForgetSearch.TabIndex = 7;
            this.btnForgetSearch.Text = "&Forget";
            this.btnForgetSearch.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnForgetSearch.UseVisualStyleBackColor = true;
            this.btnForgetSearch.Click += new System.EventHandler(this.btnForgetSearch_Click);
            // 
            // txtFromDate
            // 
            this.txtFromDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFromDate.BackColor = System.Drawing.Color.FloralWhite;
            this.txtFromDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtFromDate.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.txtFromDate.FocusLostColor = System.Drawing.Color.White;
            this.txtFromDate.Location = new System.Drawing.Point(604, 11);
            this.txtFromDate.Mask = "0000/00/00";
            this.txtFromDate.Name = "txtFromDate";
            this.txtFromDate.Size = new System.Drawing.Size(104, 20);
            this.txtFromDate.TabIndex = 215;
            this.txtFromDate.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtFromDate_KeyDown);
            // 
            // btnPrint
            // 
            this.btnPrint.Image = global::Common.Properties.Resources.print;
            this.btnPrint.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPrint.Location = new System.Drawing.Point(245, 8);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(62, 23);
            this.btnPrint.TabIndex = 4;
            this.btnPrint.Text = "Print";
            this.btnPrint.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // grdVoucherList
            // 
            this.grdVoucherList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.grdVoucherList.AutoStretchColumnsToFitWidth = true;
            this.grdVoucherList.EnableSort = true;
            this.grdVoucherList.Location = new System.Drawing.Point(12, 45);
            this.grdVoucherList.Name = "grdVoucherList";
            this.grdVoucherList.OptimizeMode = SourceGrid.CellOptimizeMode.ForRows;
            this.grdVoucherList.SelectionMode = SourceGrid.GridSelectionMode.Row;
            this.grdVoucherList.Size = new System.Drawing.Size(1126, 509);
            this.grdVoucherList.TabIndex = 0;
            this.grdVoucherList.TabStop = true;
            this.grdVoucherList.ToolTipText = "";
            // 
            // lblCreatedBy
            // 
            this.lblCreatedBy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCreatedBy.AutoSize = true;
            this.lblCreatedBy.Location = new System.Drawing.Point(372, 13);
            this.lblCreatedBy.Name = "lblCreatedBy";
            this.lblCreatedBy.Size = new System.Drawing.Size(68, 13);
            this.lblCreatedBy.TabIndex = 222;
            this.lblCreatedBy.Text = "Created By : ";
            // 
            // cboCreatedBy
            // 
            this.cboCreatedBy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cboCreatedBy.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboCreatedBy.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboCreatedBy.FormattingEnabled = true;
            this.cboCreatedBy.Location = new System.Drawing.Point(446, 10);
            this.cboCreatedBy.Name = "cboCreatedBy";
            this.cboCreatedBy.Size = new System.Drawing.Size(114, 21);
            this.cboCreatedBy.TabIndex = 221;
            // 
            // frmVoucherList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(1150, 568);
            this.Controls.Add(this.grdVoucherList);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmVoucherList";
            this.Text = "Voucher List";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmVoucherList_Load);
            this.Resize += new System.EventHandler(this.frmVoucherList_Resize);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnLast;
        private System.Windows.Forms.Button btnFirst;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnPrev;
        private System.Windows.Forms.TextBox txtSearchValue;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Panel panel1;
        private SourceGrid.Grid grdVoucherList;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Button btnForgetSearch;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnToDate;
        private System.Windows.Forms.Button btnFromDate;
        private SComponents.SMaskedTextBox txtToDate;
        private SComponents.SMaskedTextBox txtFromDate;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Label lblCreatedBy;
        private System.Windows.Forms.ComboBox cboCreatedBy;
      //  private SourceGrid.Grid grdVoucherList;
    }
}