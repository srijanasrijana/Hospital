namespace HRM.View.Reports
{
    partial class frmDetailEmployeeReportSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDetailEmployeeReportSettings));
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnShow = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.cboDepartment = new SComponents.SComboBox();
            this.chkDepartment = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cboFaculty = new SComponents.SComboBox();
            this.chkFaculty = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cboDesignation = new SComponents.SComboBox();
            this.chkDesignation = new System.Windows.Forms.CheckBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.cboStatus = new SComponents.SComboBox();
            this.chkStatus = new System.Windows.Forms.CheckBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.cboLevel = new SComponents.SComboBox();
            this.chkLevel = new System.Windows.Forms.CheckBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.cboJobType = new SComponents.SComboBox();
            this.chkJobType = new System.Windows.Forms.CheckBox();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.chkIsPatreon = new System.Windows.Forms.CheckBox();
            this.panel1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox9.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.LightSteelBlue;
            this.panel1.Controls.Add(this.btnShow);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(285, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(92, 344);
            this.panel1.TabIndex = 95;
            // 
            // btnShow
            // 
            this.btnShow.Location = new System.Drawing.Point(8, 13);
            this.btnShow.Name = "btnShow";
            this.btnShow.Size = new System.Drawing.Size(75, 23);
            this.btnShow.TabIndex = 12;
            this.btnShow.Text = "Show";
            this.btnShow.UseVisualStyleBackColor = true;
            this.btnShow.Click += new System.EventHandler(this.btnShow_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(8, 47);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 19;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox3.Controls.Add(this.cboDepartment);
            this.groupBox3.Controls.Add(this.chkDepartment);
            this.groupBox3.Location = new System.Drawing.Point(11, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(268, 42);
            this.groupBox3.TabIndex = 96;
            this.groupBox3.TabStop = false;
            // 
            // cboDepartment
            // 
            this.cboDepartment.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboDepartment.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboDepartment.Enabled = false;
            this.cboDepartment.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.cboDepartment.FocusLostColor = System.Drawing.Color.White;
            this.cboDepartment.FormattingEnabled = true;
            this.cboDepartment.Location = new System.Drawing.Point(108, 12);
            this.cboDepartment.Name = "cboDepartment";
            this.cboDepartment.Size = new System.Drawing.Size(132, 21);
            this.cboDepartment.TabIndex = 74;
            // 
            // chkDepartment
            // 
            this.chkDepartment.AutoSize = true;
            this.chkDepartment.Location = new System.Drawing.Point(12, 14);
            this.chkDepartment.Name = "chkDepartment";
            this.chkDepartment.Size = new System.Drawing.Size(81, 17);
            this.chkDepartment.TabIndex = 72;
            this.chkDepartment.Text = "Department";
            this.chkDepartment.UseVisualStyleBackColor = true;
            this.chkDepartment.CheckedChanged += new System.EventHandler(this.chkDepartment_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox1.Controls.Add(this.cboFaculty);
            this.groupBox1.Controls.Add(this.chkFaculty);
            this.groupBox1.Location = new System.Drawing.Point(11, 51);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(268, 42);
            this.groupBox1.TabIndex = 97;
            this.groupBox1.TabStop = false;
            // 
            // cboFaculty
            // 
            this.cboFaculty.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboFaculty.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboFaculty.Enabled = false;
            this.cboFaculty.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.cboFaculty.FocusLostColor = System.Drawing.Color.White;
            this.cboFaculty.FormattingEnabled = true;
            this.cboFaculty.Location = new System.Drawing.Point(108, 12);
            this.cboFaculty.Name = "cboFaculty";
            this.cboFaculty.Size = new System.Drawing.Size(132, 21);
            this.cboFaculty.TabIndex = 74;
            // 
            // chkFaculty
            // 
            this.chkFaculty.AutoSize = true;
            this.chkFaculty.Location = new System.Drawing.Point(12, 14);
            this.chkFaculty.Name = "chkFaculty";
            this.chkFaculty.Size = new System.Drawing.Size(60, 17);
            this.chkFaculty.TabIndex = 72;
            this.chkFaculty.Text = "Faculty";
            this.chkFaculty.UseVisualStyleBackColor = true;
            this.chkFaculty.CheckedChanged += new System.EventHandler(this.chkFaculty_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox2.Controls.Add(this.cboDesignation);
            this.groupBox2.Controls.Add(this.chkDesignation);
            this.groupBox2.Location = new System.Drawing.Point(11, 99);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(268, 42);
            this.groupBox2.TabIndex = 98;
            this.groupBox2.TabStop = false;
            // 
            // cboDesignation
            // 
            this.cboDesignation.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboDesignation.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboDesignation.Enabled = false;
            this.cboDesignation.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.cboDesignation.FocusLostColor = System.Drawing.Color.White;
            this.cboDesignation.FormattingEnabled = true;
            this.cboDesignation.Location = new System.Drawing.Point(108, 12);
            this.cboDesignation.Name = "cboDesignation";
            this.cboDesignation.Size = new System.Drawing.Size(132, 21);
            this.cboDesignation.TabIndex = 74;
            // 
            // chkDesignation
            // 
            this.chkDesignation.AutoSize = true;
            this.chkDesignation.Location = new System.Drawing.Point(12, 14);
            this.chkDesignation.Name = "chkDesignation";
            this.chkDesignation.Size = new System.Drawing.Size(82, 17);
            this.chkDesignation.TabIndex = 72;
            this.chkDesignation.Text = "Designation";
            this.chkDesignation.UseVisualStyleBackColor = true;
            this.chkDesignation.CheckedChanged += new System.EventHandler(this.chkDesignation_CheckedChanged);
            // 
            // groupBox4
            // 
            this.groupBox4.BackColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox4.Controls.Add(this.cboStatus);
            this.groupBox4.Controls.Add(this.chkStatus);
            this.groupBox4.Location = new System.Drawing.Point(12, 147);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(268, 42);
            this.groupBox4.TabIndex = 99;
            this.groupBox4.TabStop = false;
            // 
            // cboStatus
            // 
            this.cboStatus.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboStatus.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboStatus.Enabled = false;
            this.cboStatus.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.cboStatus.FocusLostColor = System.Drawing.Color.White;
            this.cboStatus.FormattingEnabled = true;
            this.cboStatus.Items.AddRange(new object[] {
            "Continue",
            "Leave",
            "Break",
            "Retired"});
            this.cboStatus.Location = new System.Drawing.Point(108, 12);
            this.cboStatus.Name = "cboStatus";
            this.cboStatus.Size = new System.Drawing.Size(132, 21);
            this.cboStatus.TabIndex = 74;
            // 
            // chkStatus
            // 
            this.chkStatus.AutoSize = true;
            this.chkStatus.Location = new System.Drawing.Point(12, 14);
            this.chkStatus.Name = "chkStatus";
            this.chkStatus.Size = new System.Drawing.Size(56, 17);
            this.chkStatus.TabIndex = 72;
            this.chkStatus.Text = "Status";
            this.chkStatus.UseVisualStyleBackColor = true;
            this.chkStatus.CheckedChanged += new System.EventHandler(this.chkStatus_CheckedChanged);
            // 
            // groupBox5
            // 
            this.groupBox5.BackColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox5.Controls.Add(this.cboLevel);
            this.groupBox5.Controls.Add(this.chkLevel);
            this.groupBox5.Location = new System.Drawing.Point(11, 195);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(268, 42);
            this.groupBox5.TabIndex = 100;
            this.groupBox5.TabStop = false;
            // 
            // cboLevel
            // 
            this.cboLevel.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboLevel.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboLevel.Enabled = false;
            this.cboLevel.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.cboLevel.FocusLostColor = System.Drawing.Color.White;
            this.cboLevel.FormattingEnabled = true;
            this.cboLevel.Location = new System.Drawing.Point(108, 12);
            this.cboLevel.Name = "cboLevel";
            this.cboLevel.Size = new System.Drawing.Size(132, 21);
            this.cboLevel.TabIndex = 74;
            // 
            // chkLevel
            // 
            this.chkLevel.AutoSize = true;
            this.chkLevel.Location = new System.Drawing.Point(12, 14);
            this.chkLevel.Name = "chkLevel";
            this.chkLevel.Size = new System.Drawing.Size(52, 17);
            this.chkLevel.TabIndex = 72;
            this.chkLevel.Text = "Level";
            this.chkLevel.UseVisualStyleBackColor = true;
            this.chkLevel.CheckedChanged += new System.EventHandler(this.chkLevel_CheckedChanged);
            // 
            // groupBox6
            // 
            this.groupBox6.BackColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox6.Controls.Add(this.cboJobType);
            this.groupBox6.Controls.Add(this.chkJobType);
            this.groupBox6.Location = new System.Drawing.Point(12, 243);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(268, 42);
            this.groupBox6.TabIndex = 101;
            this.groupBox6.TabStop = false;
            // 
            // cboJobType
            // 
            this.cboJobType.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboJobType.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboJobType.Enabled = false;
            this.cboJobType.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.cboJobType.FocusLostColor = System.Drawing.Color.White;
            this.cboJobType.FormattingEnabled = true;
            this.cboJobType.Items.AddRange(new object[] {
            "Permanent",
            "Contract",
            "Part Time"});
            this.cboJobType.Location = new System.Drawing.Point(108, 12);
            this.cboJobType.Name = "cboJobType";
            this.cboJobType.Size = new System.Drawing.Size(132, 21);
            this.cboJobType.TabIndex = 74;
            // 
            // chkJobType
            // 
            this.chkJobType.AutoSize = true;
            this.chkJobType.Location = new System.Drawing.Point(12, 14);
            this.chkJobType.Name = "chkJobType";
            this.chkJobType.Size = new System.Drawing.Size(70, 17);
            this.chkJobType.TabIndex = 72;
            this.chkJobType.Text = "Job Type";
            this.chkJobType.UseVisualStyleBackColor = true;
            this.chkJobType.CheckedChanged += new System.EventHandler(this.chkJobType_CheckedChanged);
            // 
            // groupBox9
            // 
            this.groupBox9.Controls.Add(this.chkIsPatreon);
            this.groupBox9.Location = new System.Drawing.Point(12, 288);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(269, 49);
            this.groupBox9.TabIndex = 102;
            this.groupBox9.TabStop = false;
            // 
            // chkIsPatreon
            // 
            this.chkIsPatreon.AutoSize = true;
            this.chkIsPatreon.Location = new System.Drawing.Point(12, 19);
            this.chkIsPatreon.Name = "chkIsPatreon";
            this.chkIsPatreon.Size = new System.Drawing.Size(74, 17);
            this.chkIsPatreon.TabIndex = 0;
            this.chkIsPatreon.Text = "Is Patreon";
            this.chkIsPatreon.UseVisualStyleBackColor = true;
            // 
            // frmDetailEmployeeReportSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(377, 344);
            this.Controls.Add(this.groupBox9);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmDetailEmployeeReportSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Detail Employee Report Settings";
            this.Load += new System.EventHandler(this.frmDetailEmployeeReportSettings_Load);
            this.panel1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox9.ResumeLayout(false);
            this.groupBox9.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnShow;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox groupBox3;
        private SComponents.SComboBox cboDepartment;
        private System.Windows.Forms.CheckBox chkDepartment;
        private System.Windows.Forms.GroupBox groupBox1;
        private SComponents.SComboBox cboFaculty;
        private System.Windows.Forms.CheckBox chkFaculty;
        private System.Windows.Forms.GroupBox groupBox2;
        private SComponents.SComboBox cboDesignation;
        private System.Windows.Forms.CheckBox chkDesignation;
        private System.Windows.Forms.GroupBox groupBox4;
        private SComponents.SComboBox cboStatus;
        private System.Windows.Forms.CheckBox chkStatus;
        private System.Windows.Forms.GroupBox groupBox5;
        private SComponents.SComboBox cboLevel;
        private System.Windows.Forms.CheckBox chkLevel;
        private System.Windows.Forms.GroupBox groupBox6;
        private SComponents.SComboBox cboJobType;
        private System.Windows.Forms.CheckBox chkJobType;
        private System.Windows.Forms.GroupBox groupBox9;
        private System.Windows.Forms.CheckBox chkIsPatreon;
    }
}