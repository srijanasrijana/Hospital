namespace Hospital.View
{
    partial class frmDoctorList
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cmbType = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbFaculty = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.cmbSpecilization = new System.Windows.Forms.ComboBox();
            this.txtLName = new SComponents.STextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtMName = new SComponents.STextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtFName = new SComponents.STextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtCode = new SComponents.STextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tabEmployees = new System.Windows.Forms.TabControl();
            this.tabEmployee = new System.Windows.Forms.TabPage();
            this.grdEmployee = new SourceGrid.Grid();
            this.btnAddEmployee = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.tabEmployees.SuspendLayout();
            this.tabEmployee.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.cmbType);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.cmbFaculty);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.cmbSpecilization);
            this.groupBox1.Controls.Add(this.txtLName);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtMName);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtFName);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.txtCode);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Location = new System.Drawing.Point(22, 453);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(725, 64);
            this.groupBox1.TabIndex = 15;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Filter By:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(628, 17);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(57, 13);
            this.label6.TabIndex = 24;
            this.label6.Text = "Job Type :";
            // 
            // cmbType
            // 
            this.cmbType.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.cmbType.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbType.FormattingEnabled = true;
            this.cmbType.Items.AddRange(new object[] {
            "All",
            "Permanent",
            "Contract",
            "Part Time"});
            this.cmbType.Location = new System.Drawing.Point(600, 32);
            this.cmbType.Name = "cmbType";
            this.cmbType.Size = new System.Drawing.Size(115, 21);
            this.cmbType.TabIndex = 6;
            this.cmbType.SelectedIndexChanged += new System.EventHandler(this.evtValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(512, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 13);
            this.label2.TabIndex = 22;
            this.label2.Text = "Faculty :";
            // 
            // cmbFaculty
            // 
            this.cmbFaculty.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.cmbFaculty.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbFaculty.FormattingEnabled = true;
            this.cmbFaculty.Location = new System.Drawing.Point(479, 32);
            this.cmbFaculty.Name = "cmbFaculty";
            this.cmbFaculty.Size = new System.Drawing.Size(115, 21);
            this.cmbFaculty.TabIndex = 5;
            this.cmbFaculty.SelectedIndexChanged += new System.EventHandler(this.evtValueChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(380, 16);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(66, 13);
            this.label7.TabIndex = 20;
            this.label7.Text = "Specilization";
            // 
            // cmbSpecilization
            // 
            this.cmbSpecilization.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.cmbSpecilization.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbSpecilization.FormattingEnabled = true;
            this.cmbSpecilization.Location = new System.Drawing.Point(358, 32);
            this.cmbSpecilization.Name = "cmbSpecilization";
            this.cmbSpecilization.Size = new System.Drawing.Size(115, 21);
            this.cmbSpecilization.TabIndex = 4;
            this.cmbSpecilization.SelectedIndexChanged += new System.EventHandler(this.evtValueChanged);
            // 
            // txtLName
            // 
            this.txtLName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtLName.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtLName.FocusLostColor = System.Drawing.Color.White;
            this.txtLName.Location = new System.Drawing.Point(272, 33);
            this.txtLName.Name = "txtLName";
            this.txtLName.Size = new System.Drawing.Size(80, 20);
            this.txtLName.TabIndex = 3;
            this.txtLName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(284, 17);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 13);
            this.label3.TabIndex = 18;
            this.label3.Text = "Last Name:";
            // 
            // txtMName
            // 
            this.txtMName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtMName.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtMName.FocusLostColor = System.Drawing.Color.White;
            this.txtMName.Location = new System.Drawing.Point(191, 33);
            this.txtMName.Name = "txtMName";
            this.txtMName.Size = new System.Drawing.Size(80, 20);
            this.txtMName.TabIndex = 2;
            this.txtMName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(199, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 13);
            this.label1.TabIndex = 17;
            this.label1.Text = "Middle Name:";
            // 
            // txtFName
            // 
            this.txtFName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtFName.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtFName.FocusLostColor = System.Drawing.Color.White;
            this.txtFName.Location = new System.Drawing.Point(110, 33);
            this.txtFName.Name = "txtFName";
            this.txtFName.Size = new System.Drawing.Size(80, 20);
            this.txtFName.TabIndex = 1;
            this.txtFName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(123, 16);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(60, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "First Name:";
            // 
            // txtCode
            // 
            this.txtCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCode.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtCode.FocusLostColor = System.Drawing.Color.White;
            this.txtCode.Location = new System.Drawing.Point(26, 33);
            this.txtCode.Name = "txtCode";
            this.txtCode.Size = new System.Drawing.Size(78, 20);
            this.txtCode.TabIndex = 0;
            this.txtCode.TextChanged += new System.EventHandler(this.txtCode_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(48, 17);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Code:";
            // 
            // tabEmployees
            // 
            this.tabEmployees.Controls.Add(this.tabEmployee);
            this.tabEmployees.Location = new System.Drawing.Point(22, 0);
            this.tabEmployees.Name = "tabEmployees";
            this.tabEmployees.SelectedIndex = 0;
            this.tabEmployees.Size = new System.Drawing.Size(729, 417);
            this.tabEmployees.TabIndex = 16;
            // 
            // tabEmployee
            // 
            this.tabEmployee.Controls.Add(this.grdEmployee);
            this.tabEmployee.Location = new System.Drawing.Point(4, 22);
            this.tabEmployee.Name = "tabEmployee";
            this.tabEmployee.Padding = new System.Windows.Forms.Padding(3);
            this.tabEmployee.Size = new System.Drawing.Size(721, 391);
            this.tabEmployee.TabIndex = 0;
            this.tabEmployee.Text = "Doctor";
            this.tabEmployee.UseVisualStyleBackColor = true;
            // 
            // grdEmployee
            // 
            this.grdEmployee.EnableSort = true;
            this.grdEmployee.Location = new System.Drawing.Point(4, 7);
            this.grdEmployee.Name = "grdEmployee";
            this.grdEmployee.OptimizeMode = SourceGrid.CellOptimizeMode.ForRows;
            this.grdEmployee.SelectionMode = SourceGrid.GridSelectionMode.Row;
            this.grdEmployee.Size = new System.Drawing.Size(709, 380);
            this.grdEmployee.TabIndex = 7;
            this.grdEmployee.TabStop = true;
            this.grdEmployee.ToolTipText = "";
            // 
            // btnAddEmployee
            // 
            this.btnAddEmployee.Location = new System.Drawing.Point(642, 424);
            this.btnAddEmployee.Name = "btnAddEmployee";
            this.btnAddEmployee.Size = new System.Drawing.Size(95, 23);
            this.btnAddEmployee.TabIndex = 17;
            this.btnAddEmployee.Text = "Add Doctor";
            this.btnAddEmployee.UseVisualStyleBackColor = true;
            this.btnAddEmployee.Click += new System.EventHandler(this.btnAddEmployee_Click);
            // 
            // frmDoctorList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(762, 529);
            this.Controls.Add(this.btnAddEmployee);
            this.Controls.Add(this.tabEmployees);
            this.Controls.Add(this.groupBox1);
            this.Name = "frmDoctorList";
            this.Text = "frmDoctorList";
            this.Load += new System.EventHandler(this.frmDoctorList_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabEmployees.ResumeLayout(false);
            this.tabEmployee.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cmbType;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbFaculty;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cmbSpecilization;
        private SComponents.STextBox txtLName;
        private System.Windows.Forms.Label label3;
        private SComponents.STextBox txtMName;
        private System.Windows.Forms.Label label1;
        private SComponents.STextBox txtFName;
        private System.Windows.Forms.Label label5;
        private SComponents.STextBox txtCode;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TabControl tabEmployees;
        private System.Windows.Forms.TabPage tabEmployee;
        private SourceGrid.Grid grdEmployee;
        private System.Windows.Forms.Button btnAddEmployee;
    }
}