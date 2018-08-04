namespace HRM.View.Reports
{
    partial class frmEmployeeReportSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmEmployeeReportSettings));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnToDate = new System.Windows.Forms.Button();
            this.btnFromDate = new System.Windows.Forms.Button();
            this.txtFromDate = new System.Windows.Forms.MaskedTextBox();
            this.txtToDate = new System.Windows.Forms.MaskedTextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.chkDate = new System.Windows.Forms.CheckBox();
            this.cmbDate = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.chkRemaining = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.cmbFaculty = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbYear = new System.Windows.Forms.ComboBox();
            this.cmbMonth = new System.Windows.Forms.ComboBox();
            this.cmbDepartment = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rbtnWelFare = new System.Windows.Forms.RadioButton();
            this.rbtnTAXOnePercent = new System.Windows.Forms.RadioButton();
            this.rbtnCIT = new System.Windows.Forms.RadioButton();
            this.rbtnPension = new System.Windows.Forms.RadioButton();
            this.rbtnSSR = new System.Windows.Forms.RadioButton();
            this.rbtnPF = new System.Windows.Forms.RadioButton();
            this.rbtnSalaryList = new System.Windows.Forms.RadioButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnShow = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.rbtnTax15and25Pecent = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.groupBox3);
            this.groupBox1.Controls.Add(this.cmbDate);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.chkRemaining);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.cmbFaculty);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.cmbYear);
            this.groupBox1.Controls.Add(this.cmbMonth);
            this.groupBox1.Controls.Add(this.cmbDepartment);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Location = new System.Drawing.Point(13, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(269, 224);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnToDate);
            this.groupBox3.Controls.Add(this.btnFromDate);
            this.groupBox3.Controls.Add(this.txtFromDate);
            this.groupBox3.Controls.Add(this.txtToDate);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.chkDate);
            this.groupBox3.Location = new System.Drawing.Point(6, 143);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(258, 76);
            this.groupBox3.TabIndex = 96;
            this.groupBox3.TabStop = false;
            // 
            // btnToDate
            // 
            this.btnToDate.Image = ((System.Drawing.Image)(resources.GetObject("btnToDate.Image")));
            this.btnToDate.Location = new System.Drawing.Point(221, 44);
            this.btnToDate.Name = "btnToDate";
            this.btnToDate.Size = new System.Drawing.Size(26, 24);
            this.btnToDate.TabIndex = 102;
            this.btnToDate.UseVisualStyleBackColor = true;
            this.btnToDate.Click += new System.EventHandler(this.btnToDate_Click);
            // 
            // btnFromDate
            // 
            this.btnFromDate.Image = ((System.Drawing.Image)(resources.GetObject("btnFromDate.Image")));
            this.btnFromDate.Location = new System.Drawing.Point(221, 17);
            this.btnFromDate.Name = "btnFromDate";
            this.btnFromDate.Size = new System.Drawing.Size(26, 24);
            this.btnFromDate.TabIndex = 99;
            this.btnFromDate.UseVisualStyleBackColor = true;
            this.btnFromDate.Click += new System.EventHandler(this.btnFromDate_Click);
            // 
            // txtFromDate
            // 
            this.txtFromDate.Location = new System.Drawing.Point(85, 19);
            this.txtFromDate.Mask = "##/##/####";
            this.txtFromDate.Name = "txtFromDate";
            this.txtFromDate.Size = new System.Drawing.Size(133, 20);
            this.txtFromDate.TabIndex = 98;
            // 
            // txtToDate
            // 
            this.txtToDate.Location = new System.Drawing.Point(85, 45);
            this.txtToDate.Mask = "##/##/####";
            this.txtToDate.Name = "txtToDate";
            this.txtToDate.Size = new System.Drawing.Size(133, 20);
            this.txtToDate.TabIndex = 101;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(14, 48);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(23, 13);
            this.label4.TabIndex = 100;
            this.label4.Text = "To:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 29);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(33, 13);
            this.label5.TabIndex = 97;
            this.label5.Text = "From:";
            // 
            // chkDate
            // 
            this.chkDate.AutoSize = true;
            this.chkDate.Checked = true;
            this.chkDate.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDate.Location = new System.Drawing.Point(6, 0);
            this.chkDate.Name = "chkDate";
            this.chkDate.Size = new System.Drawing.Size(49, 17);
            this.chkDate.TabIndex = 96;
            this.chkDate.Text = "Date";
            this.chkDate.UseVisualStyleBackColor = true;
            this.chkDate.CheckedChanged += new System.EventHandler(this.chkDate_CheckedChanged);
            // 
            // cmbDate
            // 
            this.cmbDate.FormattingEnabled = true;
            this.cmbDate.Location = new System.Drawing.Point(90, 115);
            this.cmbDate.Name = "cmbDate";
            this.cmbDate.Size = new System.Drawing.Size(133, 21);
            this.cmbDate.TabIndex = 65;
            this.cmbDate.Visible = false;
            this.cmbDate.SelectedIndexChanged += new System.EventHandler(this.cmbDate_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 119);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 13);
            this.label3.TabIndex = 95;
            this.label3.Text = "Pay Slip Date";
            this.label3.Visible = false;
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // chkRemaining
            // 
            this.chkRemaining.AutoSize = true;
            this.chkRemaining.Location = new System.Drawing.Point(97, 121);
            this.chkRemaining.Name = "chkRemaining";
            this.chkRemaining.Size = new System.Drawing.Size(76, 17);
            this.chkRemaining.TabIndex = 94;
            this.chkRemaining.Text = "Remaining";
            this.chkRemaining.UseVisualStyleBackColor = true;
            this.chkRemaining.CheckedChanged += new System.EventHandler(this.chkRemaining_CheckedChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(18, 87);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(44, 13);
            this.label7.TabIndex = 93;
            this.label7.Text = "Faculty:";
            // 
            // cmbFaculty
            // 
            this.cmbFaculty.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFaculty.FormattingEnabled = true;
            this.cmbFaculty.Location = new System.Drawing.Point(90, 80);
            this.cmbFaculty.Name = "cmbFaculty";
            this.cmbFaculty.Size = new System.Drawing.Size(133, 21);
            this.cmbFaculty.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 94);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 13);
            this.label2.TabIndex = 91;
            this.label2.Text = "Department:";
            this.label2.Visible = false;
            // 
            // cmbYear
            // 
            this.cmbYear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbYear.FormattingEnabled = true;
            this.cmbYear.Location = new System.Drawing.Point(90, 49);
            this.cmbYear.Name = "cmbYear";
            this.cmbYear.Size = new System.Drawing.Size(133, 21);
            this.cmbYear.TabIndex = 1;
            this.cmbYear.SelectionChangeCommitted += new System.EventHandler(this.LoadSalarySheetDates);
            // 
            // cmbMonth
            // 
            this.cmbMonth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMonth.FormattingEnabled = true;
            this.cmbMonth.Location = new System.Drawing.Point(90, 15);
            this.cmbMonth.Name = "cmbMonth";
            this.cmbMonth.Size = new System.Drawing.Size(133, 21);
            this.cmbMonth.TabIndex = 0;
            this.cmbMonth.SelectionChangeCommitted += new System.EventHandler(this.LoadSalarySheetDates);
            // 
            // cmbDepartment
            // 
            this.cmbDepartment.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDepartment.FormattingEnabled = true;
            this.cmbDepartment.Location = new System.Drawing.Point(90, 88);
            this.cmbDepartment.Name = "cmbDepartment";
            this.cmbDepartment.Size = new System.Drawing.Size(133, 21);
            this.cmbDepartment.TabIndex = 2;
            this.cmbDepartment.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 54);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Year:";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(18, 19);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(40, 13);
            this.label6.TabIndex = 1;
            this.label6.Text = "Month:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rbtnWelFare);
            this.groupBox2.Controls.Add(this.rbtnTax15and25Pecent);
            this.groupBox2.Controls.Add(this.rbtnTAXOnePercent);
            this.groupBox2.Controls.Add(this.rbtnCIT);
            this.groupBox2.Controls.Add(this.rbtnPension);
            this.groupBox2.Controls.Add(this.rbtnSSR);
            this.groupBox2.Controls.Add(this.rbtnPF);
            this.groupBox2.Controls.Add(this.rbtnSalaryList);
            this.groupBox2.Location = new System.Drawing.Point(13, 243);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(268, 197);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            // 
            // rbtnWelFare
            // 
            this.rbtnWelFare.AutoSize = true;
            this.rbtnWelFare.Checked = true;
            this.rbtnWelFare.Location = new System.Drawing.Point(34, 173);
            this.rbtnWelFare.Name = "rbtnWelFare";
            this.rbtnWelFare.Size = new System.Drawing.Size(97, 17);
            this.rbtnWelFare.TabIndex = 6;
            this.rbtnWelFare.TabStop = true;
            this.rbtnWelFare.Text = "Welfare Report";
            this.rbtnWelFare.UseVisualStyleBackColor = true;
            // 
            // rbtnTAXOnePercent
            // 
            this.rbtnTAXOnePercent.AutoSize = true;
            this.rbtnTAXOnePercent.Checked = true;
            this.rbtnTAXOnePercent.Location = new System.Drawing.Point(34, 125);
            this.rbtnTAXOnePercent.Name = "rbtnTAXOnePercent";
            this.rbtnTAXOnePercent.Size = new System.Drawing.Size(104, 17);
            this.rbtnTAXOnePercent.TabIndex = 5;
            this.rbtnTAXOnePercent.TabStop = true;
            this.rbtnTAXOnePercent.Text = "Tax Report (1 %)";
            this.rbtnTAXOnePercent.UseVisualStyleBackColor = true;
            // 
            // rbtnCIT
            // 
            this.rbtnCIT.AutoSize = true;
            this.rbtnCIT.Checked = true;
            this.rbtnCIT.Location = new System.Drawing.Point(34, 102);
            this.rbtnCIT.Name = "rbtnCIT";
            this.rbtnCIT.Size = new System.Drawing.Size(77, 17);
            this.rbtnCIT.TabIndex = 4;
            this.rbtnCIT.TabStop = true;
            this.rbtnCIT.Text = "CIT Report";
            this.rbtnCIT.UseVisualStyleBackColor = true;
            // 
            // rbtnPension
            // 
            this.rbtnPension.AutoSize = true;
            this.rbtnPension.Checked = true;
            this.rbtnPension.Location = new System.Drawing.Point(34, 79);
            this.rbtnPension.Name = "rbtnPension";
            this.rbtnPension.Size = new System.Drawing.Size(125, 17);
            this.rbtnPension.TabIndex = 3;
            this.rbtnPension.TabStop = true;
            this.rbtnPension.Text = "Pension Fund Report";
            this.rbtnPension.UseVisualStyleBackColor = true;
            // 
            // rbtnSSR
            // 
            this.rbtnSSR.AutoSize = true;
            this.rbtnSSR.Checked = true;
            this.rbtnSSR.Location = new System.Drawing.Point(34, 56);
            this.rbtnSSR.Name = "rbtnSSR";
            this.rbtnSSR.Size = new System.Drawing.Size(120, 17);
            this.rbtnSSR.TabIndex = 2;
            this.rbtnSSR.TabStop = true;
            this.rbtnSSR.Text = "Salary Sheet Report";
            this.rbtnSSR.UseVisualStyleBackColor = true;
            // 
            // rbtnPF
            // 
            this.rbtnPF.AutoSize = true;
            this.rbtnPF.Location = new System.Drawing.Point(34, 33);
            this.rbtnPF.Name = "rbtnPF";
            this.rbtnPF.Size = new System.Drawing.Size(132, 17);
            this.rbtnPF.TabIndex = 1;
            this.rbtnPF.Text = "Provident Fund Report";
            this.rbtnPF.UseVisualStyleBackColor = true;
            // 
            // rbtnSalaryList
            // 
            this.rbtnSalaryList.AutoSize = true;
            this.rbtnSalaryList.Checked = true;
            this.rbtnSalaryList.Location = new System.Drawing.Point(34, 10);
            this.rbtnSalaryList.Name = "rbtnSalaryList";
            this.rbtnSalaryList.Size = new System.Drawing.Size(108, 17);
            this.rbtnSalaryList.TabIndex = 0;
            this.rbtnSalaryList.TabStop = true;
            this.rbtnSalaryList.Text = "Salary List Report";
            this.rbtnSalaryList.UseVisualStyleBackColor = true;
            this.rbtnSalaryList.CheckedChanged += new System.EventHandler(this.rbtnSalaryList_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.LightSteelBlue;
            this.panel1.Controls.Add(this.btnShow);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Location = new System.Drawing.Point(287, 1);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(94, 441);
            this.panel1.TabIndex = 64;
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
            // rbtnTax15and25Pecent
            // 
            this.rbtnTax15and25Pecent.AutoSize = true;
            this.rbtnTax15and25Pecent.Checked = true;
            this.rbtnTax15and25Pecent.Location = new System.Drawing.Point(33, 148);
            this.rbtnTax15and25Pecent.Name = "rbtnTax15and25Pecent";
            this.rbtnTax15and25Pecent.Size = new System.Drawing.Size(139, 17);
            this.rbtnTax15and25Pecent.TabIndex = 5;
            this.rbtnTax15and25Pecent.TabStop = true;
            this.rbtnTax15and25Pecent.Text = "Tax Report (15% && 25%)";
            this.rbtnTax15and25Pecent.UseVisualStyleBackColor = true;
            // 
            // frmEmployeeReportSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(387, 445);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmEmployeeReportSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Employee Report Setting";
            this.Load += new System.EventHandler(this.frmEmployeeReportSetting_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbMonth;
        private System.Windows.Forms.ComboBox cmbYear;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rbtnPF;
        private System.Windows.Forms.RadioButton rbtnSalaryList;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnShow;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.RadioButton rbtnSSR;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbDepartment;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cmbFaculty;
        private System.Windows.Forms.RadioButton rbtnTAXOnePercent;
        private System.Windows.Forms.RadioButton rbtnCIT;
        private System.Windows.Forms.RadioButton rbtnPension;
        private System.Windows.Forms.CheckBox chkRemaining;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox chkDate;
        private System.Windows.Forms.RadioButton rbtnWelFare;
        private System.Windows.Forms.Button btnToDate;
        private System.Windows.Forms.Button btnFromDate;
        private System.Windows.Forms.MaskedTextBox txtFromDate;
        private System.Windows.Forms.MaskedTextBox txtToDate;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cmbDate;
        private System.Windows.Forms.RadioButton rbtnTax15and25Pecent;
    }
}