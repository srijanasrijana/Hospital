namespace HRM.View
{
    partial class frmLoan
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmLoan));
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.cmbLoanType = new System.Windows.Forms.ComboBox();
            this.txtRemarks = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtLoanID = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnedit = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnsave = new System.Windows.Forms.Button();
            this.btnnew = new System.Windows.Forms.Button();
            this.txtLoanName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.grdListView = new SourceGrid.Grid();
            this.tabControl2.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabPage2);
            this.tabControl2.Location = new System.Drawing.Point(221, 12);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(423, 254);
            this.tabControl2.TabIndex = 5;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.cmbLoanType);
            this.tabPage2.Controls.Add(this.txtRemarks);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.txtLoanID);
            this.tabPage2.Controls.Add(this.btnCancel);
            this.tabPage2.Controls.Add(this.btnedit);
            this.tabPage2.Controls.Add(this.btnDelete);
            this.tabPage2.Controls.Add(this.btnsave);
            this.tabPage2.Controls.Add(this.btnnew);
            this.tabPage2.Controls.Add(this.txtLoanName);
            this.tabPage2.Controls.Add(this.label1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(415, 228);
            this.tabPage2.TabIndex = 0;
            this.tabPage2.Text = "Loan";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // cmbLoanType
            // 
            this.cmbLoanType.FormattingEnabled = true;
            this.cmbLoanType.Items.AddRange(new object[] {
            "Fix Installment",
            "Different Installment"});
            this.cmbLoanType.Location = new System.Drawing.Point(143, 71);
            this.cmbLoanType.Name = "cmbLoanType";
            this.cmbLoanType.Size = new System.Drawing.Size(137, 21);
            this.cmbLoanType.TabIndex = 14;
            // 
            // txtRemarks
            // 
            this.txtRemarks.Location = new System.Drawing.Point(143, 112);
            this.txtRemarks.Name = "txtRemarks";
            this.txtRemarks.Size = new System.Drawing.Size(258, 20);
            this.txtRemarks.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(14, 115);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(49, 13);
            this.label4.TabIndex = 13;
            this.label4.Text = "Remarks";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 74);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "Loan Type";
            // 
            // txtLoanID
            // 
            this.txtLoanID.Location = new System.Drawing.Point(6, 6);
            this.txtLoanID.Name = "txtLoanID";
            this.txtLoanID.Size = new System.Drawing.Size(26, 20);
            this.txtLoanID.TabIndex = 10;
            this.txtLoanID.Visible = false;
            // 
            // btnCancel
            // 
            this.btnCancel.Image = ((System.Drawing.Image)(resources.GetObject("btnCancel.Image")));
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(330, 199);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 10;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnedit
            // 
            this.btnedit.Image = ((System.Drawing.Image)(resources.GetObject("btnedit.Image")));
            this.btnedit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnedit.Location = new System.Drawing.Point(87, 199);
            this.btnedit.Name = "btnedit";
            this.btnedit.Size = new System.Drawing.Size(75, 23);
            this.btnedit.TabIndex = 7;
            this.btnedit.Text = "&Edit";
            this.btnedit.UseVisualStyleBackColor = true;
            this.btnedit.Click += new System.EventHandler(this.btnedit_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Image = ((System.Drawing.Image)(resources.GetObject("btnDelete.Image")));
            this.btnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDelete.Location = new System.Drawing.Point(249, 199);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 9;
            this.btnDelete.Text = "&Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnsave
            // 
            this.btnsave.Image = ((System.Drawing.Image)(resources.GetObject("btnsave.Image")));
            this.btnsave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnsave.Location = new System.Drawing.Point(168, 199);
            this.btnsave.Name = "btnsave";
            this.btnsave.Size = new System.Drawing.Size(75, 23);
            this.btnsave.TabIndex = 8;
            this.btnsave.Text = "&Save";
            this.btnsave.UseVisualStyleBackColor = true;
            this.btnsave.Click += new System.EventHandler(this.btnsave_Click);
            // 
            // btnnew
            // 
            this.btnnew.Image = ((System.Drawing.Image)(resources.GetObject("btnnew.Image")));
            this.btnnew.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnnew.Location = new System.Drawing.Point(6, 199);
            this.btnnew.Name = "btnnew";
            this.btnnew.Size = new System.Drawing.Size(75, 23);
            this.btnnew.TabIndex = 6;
            this.btnnew.Text = "&New";
            this.btnnew.UseVisualStyleBackColor = true;
            this.btnnew.Click += new System.EventHandler(this.btnnew_Click);
            // 
            // txtLoanName
            // 
            this.txtLoanName.Enabled = false;
            this.txtLoanName.Location = new System.Drawing.Point(143, 32);
            this.txtLoanName.Name = "txtLoanName";
            this.txtLoanName.Size = new System.Drawing.Size(137, 20);
            this.txtLoanName.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Loan Name";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(203, 254);
            this.tabControl1.TabIndex = 6;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.grdListView);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(195, 228);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "List View";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // grdListView
            // 
            this.grdListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdListView.EnableSort = true;
            this.grdListView.Location = new System.Drawing.Point(3, 3);
            this.grdListView.Name = "grdListView";
            this.grdListView.OptimizeMode = SourceGrid.CellOptimizeMode.ForRows;
            this.grdListView.SelectionMode = SourceGrid.GridSelectionMode.Cell;
            this.grdListView.Size = new System.Drawing.Size(189, 222);
            this.grdListView.TabIndex = 1;
            this.grdListView.TabStop = true;
            this.grdListView.ToolTipText = "";
            // 
            // frmLoan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(653, 283);
            this.Controls.Add(this.tabControl2);
            this.Controls.Add(this.tabControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmLoan";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Loan";
            this.Load += new System.EventHandler(this.frmLoan_Load);
            this.tabControl2.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TextBox txtRemarks;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtLoanID;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnedit;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnsave;
        private System.Windows.Forms.Button btnnew;
        private System.Windows.Forms.TextBox txtLoanName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private SourceGrid.Grid grdListView;
        private System.Windows.Forms.ComboBox cmbLoanType;
    }
}