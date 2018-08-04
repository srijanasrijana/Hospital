namespace HRM.View.Reports
{
    partial class frmEmployeeAnnualReportSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmEmployeeAnnualReportSettings));
            this.cmbEmployeeList = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnShow = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbYear = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.gbEmployeeReport = new System.Windows.Forms.GroupBox();
            this.cmbToMonth = new System.Windows.Forms.ComboBox();
            this.cmbFromMonth = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.rbtnTaxReport = new System.Windows.Forms.RadioButton();
            this.rbtnEmployeeReport = new System.Windows.Forms.RadioButton();
            this.gbTaxAdjsut = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cmbFaculty = new System.Windows.Forms.ComboBox();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.gbEmployeeReport.SuspendLayout();
            this.gbTaxAdjsut.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmbEmployeeList
            // 
            this.cmbEmployeeList.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbEmployeeList.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbEmployeeList.FormattingEnabled = true;
            this.cmbEmployeeList.Location = new System.Drawing.Point(31, 45);
            this.cmbEmployeeList.Name = "cmbEmployeeList";
            this.cmbEmployeeList.Size = new System.Drawing.Size(159, 21);
            this.cmbEmployeeList.TabIndex = 94;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(31, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 13);
            this.label1.TabIndex = 95;
            this.label1.Text = "Employee Name: ";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.LightSteelBlue;
            this.panel1.Controls.Add(this.btnShow);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Location = new System.Drawing.Point(357, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(94, 285);
            this.panel1.TabIndex = 96;
            // 
            // btnShow
            // 
            this.btnShow.Location = new System.Drawing.Point(12, 13);
            this.btnShow.Name = "btnShow";
            this.btnShow.Size = new System.Drawing.Size(75, 23);
            this.btnShow.TabIndex = 0;
            this.btnShow.Text = "Show";
            this.btnShow.UseVisualStyleBackColor = true;
            this.btnShow.Click += new System.EventHandler(this.btnShow_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Image = ((System.Drawing.Image)(resources.GetObject("btnCancel.Image")));
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(12, 42);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(76, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 114);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 13);
            this.label2.TabIndex = 95;
            this.label2.Text = "Fiscal Year:";
            // 
            // cmbYear
            // 
            this.cmbYear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbYear.FormattingEnabled = true;
            this.cmbYear.Location = new System.Drawing.Point(65, 136);
            this.cmbYear.Name = "cmbYear";
            this.cmbYear.Size = new System.Drawing.Size(151, 21);
            this.cmbYear.TabIndex = 97;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.gbTaxAdjsut);
            this.groupBox1.Controls.Add(this.rbtnEmployeeReport);
            this.groupBox1.Controls.Add(this.rbtnTaxReport);
            this.groupBox1.Controls.Add(this.gbEmployeeReport);
            this.groupBox1.Controls.Add(this.cmbToMonth);
            this.groupBox1.Controls.Add(this.cmbFromMonth);
            this.groupBox1.Controls.Add(this.cmbYear);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(7, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(344, 279);
            this.groupBox1.TabIndex = 98;
            this.groupBox1.TabStop = false;
            // 
            // gbEmployeeReport
            // 
            this.gbEmployeeReport.Controls.Add(this.cmbEmployeeList);
            this.gbEmployeeReport.Controls.Add(this.label1);
            this.gbEmployeeReport.Location = new System.Drawing.Point(134, 12);
            this.gbEmployeeReport.Name = "gbEmployeeReport";
            this.gbEmployeeReport.Size = new System.Drawing.Size(200, 91);
            this.gbEmployeeReport.TabIndex = 98;
            this.gbEmployeeReport.TabStop = false;
            // 
            // cmbToMonth
            // 
            this.cmbToMonth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbToMonth.FormattingEnabled = true;
            this.cmbToMonth.Location = new System.Drawing.Point(65, 245);
            this.cmbToMonth.Name = "cmbToMonth";
            this.cmbToMonth.Size = new System.Drawing.Size(151, 21);
            this.cmbToMonth.TabIndex = 97;
            // 
            // cmbFromMonth
            // 
            this.cmbFromMonth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFromMonth.FormattingEnabled = true;
            this.cmbFromMonth.Location = new System.Drawing.Point(65, 188);
            this.cmbFromMonth.Name = "cmbFromMonth";
            this.cmbFromMonth.Size = new System.Drawing.Size(151, 21);
            this.cmbFromMonth.TabIndex = 97;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(20, 223);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(23, 13);
            this.label4.TabIndex = 95;
            this.label4.Text = "To:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 166);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(33, 13);
            this.label3.TabIndex = 95;
            this.label3.Text = "From:";
            // 
            // rbtnTaxReport
            // 
            this.rbtnTaxReport.AutoSize = true;
            this.rbtnTaxReport.Checked = true;
            this.rbtnTaxReport.Location = new System.Drawing.Point(14, 71);
            this.rbtnTaxReport.Name = "rbtnTaxReport";
            this.rbtnTaxReport.Size = new System.Drawing.Size(110, 17);
            this.rbtnTaxReport.TabIndex = 99;
            this.rbtnTaxReport.TabStop = true;
            this.rbtnTaxReport.Text = "Tax Adjust Report";
            this.rbtnTaxReport.UseVisualStyleBackColor = true;
            // 
            // rbtnEmployeeReport
            // 
            this.rbtnEmployeeReport.AutoSize = true;
            this.rbtnEmployeeReport.Location = new System.Drawing.Point(14, 31);
            this.rbtnEmployeeReport.Name = "rbtnEmployeeReport";
            this.rbtnEmployeeReport.Size = new System.Drawing.Size(106, 17);
            this.rbtnEmployeeReport.TabIndex = 99;
            this.rbtnEmployeeReport.TabStop = true;
            this.rbtnEmployeeReport.Text = "Employee Report";
            this.rbtnEmployeeReport.UseVisualStyleBackColor = true;
            this.rbtnEmployeeReport.CheckedChanged += new System.EventHandler(this.rbtnEmployeeReport_CheckedChanged);
            // 
            // gbTaxAdjsut
            // 
            this.gbTaxAdjsut.Controls.Add(this.cmbFaculty);
            this.gbTaxAdjsut.Controls.Add(this.label5);
            this.gbTaxAdjsut.Location = new System.Drawing.Point(134, 11);
            this.gbTaxAdjsut.Name = "gbTaxAdjsut";
            this.gbTaxAdjsut.Size = new System.Drawing.Size(200, 91);
            this.gbTaxAdjsut.TabIndex = 100;
            this.gbTaxAdjsut.TabStop = false;
            this.gbTaxAdjsut.Visible = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(31, 19);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(44, 13);
            this.label5.TabIndex = 95;
            this.label5.Text = "Faculty:";
            // 
            // cmbFaculty
            // 
            this.cmbFaculty.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbFaculty.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbFaculty.FormattingEnabled = true;
            this.cmbFaculty.Location = new System.Drawing.Point(31, 46);
            this.cmbFaculty.Name = "cmbFaculty";
            this.cmbFaculty.Size = new System.Drawing.Size(159, 21);
            this.cmbFaculty.TabIndex = 94;
            // 
            // frmEmployeeAnnualReportSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(452, 288);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmEmployeeAnnualReportSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Employee Annual Report Settings";
            this.Load += new System.EventHandler(this.frmEmployeeAnnualReportSettings_Load);
            this.panel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.gbEmployeeReport.ResumeLayout(false);
            this.gbEmployeeReport.PerformLayout();
            this.gbTaxAdjsut.ResumeLayout(false);
            this.gbTaxAdjsut.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbEmployeeList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnShow;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbYear;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cmbToMonth;
        private System.Windows.Forms.ComboBox cmbFromMonth;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox gbEmployeeReport;
        private System.Windows.Forms.RadioButton rbtnEmployeeReport;
        private System.Windows.Forms.RadioButton rbtnTaxReport;
        private System.Windows.Forms.GroupBox gbTaxAdjsut;
        private System.Windows.Forms.ComboBox cmbFaculty;
        private System.Windows.Forms.Label label5;
    }
}