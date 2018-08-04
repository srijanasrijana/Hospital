using SComponents;
namespace HRM
{
    partial class frmAdvanceSearch
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAdvanceSearch));
            this.grdAdvanceSearch = new SourceGrid.Grid();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnForgetSearch = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtSrchParam1 = new SComponents.STextBox();
            this.cboSrchSearchIn1 = new SComponents.SComboBox();
            this.cmbtype = new SComponents.SComboBox();
            this.cmbstatus = new SComponents.SComboBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // grdAdvanceSearch
            // 
            this.grdAdvanceSearch.EnableSort = true;
            this.grdAdvanceSearch.Location = new System.Drawing.Point(1, 72);
            this.grdAdvanceSearch.Name = "grdAdvanceSearch";
            this.grdAdvanceSearch.OptimizeMode = SourceGrid.CellOptimizeMode.ForRows;
            this.grdAdvanceSearch.SelectionMode = SourceGrid.GridSelectionMode.Cell;
            this.grdAdvanceSearch.Size = new System.Drawing.Size(979, 372);
            this.grdAdvanceSearch.TabIndex = 0;
            this.grdAdvanceSearch.TabStop = true;
            this.grdAdvanceSearch.ToolTipText = "";
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Cornsilk;
            this.groupBox1.Controls.Add(this.btnForgetSearch);
            this.groupBox1.Controls.Add(this.btnSearch);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtSrchParam1);
            this.groupBox1.Controls.Add(this.cboSrchSearchIn1);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(506, 61);
            this.groupBox1.TabIndex = 58;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Search Box:";
            // 
            // btnForgetSearch
            // 
            this.btnForgetSearch.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnForgetSearch.Location = new System.Drawing.Point(415, 29);
            this.btnForgetSearch.Name = "btnForgetSearch";
            this.btnForgetSearch.Size = new System.Drawing.Size(59, 23);
            this.btnForgetSearch.TabIndex = 16;
            this.btnForgetSearch.Text = "&Forget";
            this.btnForgetSearch.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnForgetSearch.UseVisualStyleBackColor = true;
            this.btnForgetSearch.Click += new System.EventHandler(this.btnForgetSearch_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSearch.Location = new System.Drawing.Point(343, 29);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(66, 23);
            this.btnSearch.TabIndex = 15;
            this.btnSearch.Text = "&Search";
            this.btnSearch.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(192, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "Parameter";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(53, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Search In";
            // 
            // txtSrchParam1
            // 
            this.txtSrchParam1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSrchParam1.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtSrchParam1.FocusLostColor = System.Drawing.Color.White;
            this.txtSrchParam1.Location = new System.Drawing.Point(179, 33);
            this.txtSrchParam1.Name = "txtSrchParam1";
            this.txtSrchParam1.Size = new System.Drawing.Size(100, 20);
            this.txtSrchParam1.TabIndex = 4;
            // 
            // cboSrchSearchIn1
            // 
            this.cboSrchSearchIn1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.cboSrchSearchIn1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSrchSearchIn1.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.cboSrchSearchIn1.FocusLostColor = System.Drawing.Color.White;
            this.cboSrchSearchIn1.FormattingEnabled = true;
            this.cboSrchSearchIn1.Items.AddRange(new object[] {
            "Staff Code",
            "Staff Name",
            "Gender",
            "City",
            "Join Date",
            "End Date",
            "Department",
            "Designation",
            "Status",
            "Type"});
            this.cboSrchSearchIn1.Location = new System.Drawing.Point(20, 32);
            this.cboSrchSearchIn1.Name = "cboSrchSearchIn1";
            this.cboSrchSearchIn1.Size = new System.Drawing.Size(121, 21);
            this.cboSrchSearchIn1.TabIndex = 0;
            // 
            // cmbtype
            // 
            this.cmbtype.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbtype.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.cmbtype.FocusLostColor = System.Drawing.Color.White;
            this.cmbtype.FormattingEnabled = true;
            this.cmbtype.Items.AddRange(new object[] {
            "Contract",
            "Probation",
            "Permanent"});
            this.cmbtype.Location = new System.Drawing.Point(709, 11);
            this.cmbtype.Name = "cmbtype";
            this.cmbtype.Size = new System.Drawing.Size(173, 21);
            this.cmbtype.TabIndex = 57;
            this.cmbtype.Visible = false;
            // 
            // cmbstatus
            // 
            this.cmbstatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbstatus.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.cmbstatus.FocusLostColor = System.Drawing.Color.White;
            this.cmbstatus.FormattingEnabled = true;
            this.cmbstatus.Items.AddRange(new object[] {
            "Continue",
            "Leave",
            "Break",
            "Retired"});
            this.cmbstatus.Location = new System.Drawing.Point(709, 38);
            this.cmbstatus.Name = "cmbstatus";
            this.cmbstatus.Size = new System.Drawing.Size(173, 21);
            this.cmbstatus.TabIndex = 56;
            this.cmbstatus.Visible = false;
            // 
            // frmAdvanceSearch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(980, 446);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.cmbtype);
            this.Controls.Add(this.cmbstatus);
            this.Controls.Add(this.grdAdvanceSearch);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmAdvanceSearch";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Employee Search";
            this.Load += new System.EventHandler(this.frmAdvanceSearch_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private SourceGrid.Grid grdAdvanceSearch;
        private SComboBox cmbtype;
        private SComboBox cmbstatus;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnForgetSearch;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private STextBox txtSrchParam1;
        private SComboBox cboSrchSearchIn1;

    }
}