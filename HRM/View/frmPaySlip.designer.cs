using SComponents;
namespace HRM
{
    partial class frmPaySlip
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPaySlip));
            this.button1 = new System.Windows.Forms.Button();
            this.cmbMonth = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.cmbEmployeeList = new System.Windows.Forms.ComboBox();
            this.chkApplyInsurance = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.cmbFaculty = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cmbDepartment = new System.Windows.Forms.ComboBox();
            this.chkFestiveMonth = new System.Windows.Forms.CheckBox();
            this.cmbYear = new System.Windows.Forms.ComboBox();
            this.btnExportExcel = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btndate = new System.Windows.Forms.Button();
            this.lblnumberofdays = new System.Windows.Forms.Label();
            this.txtdate = new System.Windows.Forms.MaskedTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblMsg = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tppayslip = new System.Windows.Forms.TabPage();
            this.grdPaySlips = new SourceGrid.Grid();
            this.txtnarration = new SComponents.STextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnclose = new System.Windows.Forms.Button();
            this.btnpay = new System.Windows.Forms.Button();
            this.tpadditionalallowances = new System.Windows.Forms.TabPage();
            this.btncancel = new System.Windows.Forms.Button();
            this.btndone = new System.Windows.Forms.Button();
            this.grdaddition = new SourceGrid.Grid();
            this.tpotherdeduction = new System.Windows.Forms.TabPage();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.grdDeduction = new SourceGrid.Grid();
            this.grpPosting = new System.Windows.Forms.GroupBox();
            this.chkJournalPosting = new System.Windows.Forms.CheckBox();
            this.btnJournalEntry = new System.Windows.Forms.Button();
            this.chkByCash = new System.Windows.Forms.CheckBox();
            this.lblCashParty = new System.Windows.Forms.Label();
            this.cmbBankName = new SComponents.SComboBox();
            this.btnAdjust = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tppayslip.SuspendLayout();
            this.tpadditionalallowances.SuspendLayout();
            this.tpotherdeduction.SuspendLayout();
            this.grpPosting.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(1132, 11);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 28);
            this.button1.TabIndex = 1;
            this.button1.Text = "Check All";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // cmbMonth
            // 
            this.cmbMonth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMonth.FormattingEnabled = true;
            this.cmbMonth.Location = new System.Drawing.Point(256, 22);
            this.cmbMonth.Name = "cmbMonth";
            this.cmbMonth.Size = new System.Drawing.Size(72, 21);
            this.cmbMonth.TabIndex = 2;
            this.cmbMonth.SelectionChangeCommitted += new System.EventHandler(this.cmbMonth_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.BackColor = System.Drawing.Color.Cornsilk;
            this.groupBox1.Controls.Add(this.btnSearch);
            this.groupBox1.Controls.Add(this.cmbEmployeeList);
            this.groupBox1.Controls.Add(this.chkApplyInsurance);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.cmbFaculty);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.cmbDepartment);
            this.groupBox1.Controls.Add(this.chkFestiveMonth);
            this.groupBox1.Controls.Add(this.cmbYear);
            this.groupBox1.Controls.Add(this.btnExportExcel);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.btndate);
            this.groupBox1.Controls.Add(this.lblnumberofdays);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.txtdate);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.cmbMonth);
            this.groupBox1.Location = new System.Drawing.Point(2, 1);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1217, 64);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Filter";
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(807, 18);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(58, 23);
            this.btnSearch.TabIndex = 94;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Visible = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // cmbEmployeeList
            // 
            this.cmbEmployeeList.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbEmployeeList.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbEmployeeList.FormattingEnabled = true;
            this.cmbEmployeeList.Location = new System.Drawing.Point(651, 20);
            this.cmbEmployeeList.Name = "cmbEmployeeList";
            this.cmbEmployeeList.Size = new System.Drawing.Size(152, 21);
            this.cmbEmployeeList.TabIndex = 93;
            this.cmbEmployeeList.Leave += new System.EventHandler(this.btnSearch_Click);
            // 
            // chkApplyInsurance
            // 
            this.chkApplyInsurance.AutoSize = true;
            this.chkApplyInsurance.Location = new System.Drawing.Point(1030, 44);
            this.chkApplyInsurance.Name = "chkApplyInsurance";
            this.chkApplyInsurance.Size = new System.Drawing.Size(102, 17);
            this.chkApplyInsurance.TabIndex = 92;
            this.chkApplyInsurance.Text = "Apply Insurance";
            this.chkApplyInsurance.UseVisualStyleBackColor = true;
            this.chkApplyInsurance.Visible = false;
            this.chkApplyInsurance.CheckedChanged += new System.EventHandler(this.chkApplyInsurance_CheckedChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(11, 25);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(44, 13);
            this.label7.TabIndex = 91;
            this.label7.Text = "Faculty:";
            // 
            // cmbFaculty
            // 
            this.cmbFaculty.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFaculty.FormattingEnabled = true;
            this.cmbFaculty.Location = new System.Drawing.Point(55, 22);
            this.cmbFaculty.Name = "cmbFaculty";
            this.cmbFaculty.Size = new System.Drawing.Size(149, 21);
            this.cmbFaculty.TabIndex = 0;
            this.cmbFaculty.SelectionChangeCommitted += new System.EventHandler(this.cmbMonth_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(656, 55);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 13);
            this.label6.TabIndex = 89;
            this.label6.Text = "Department:";
            this.label6.Visible = false;
            this.label6.Click += new System.EventHandler(this.label6_Click);
            // 
            // cmbDepartment
            // 
            this.cmbDepartment.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDepartment.FormattingEnabled = true;
            this.cmbDepartment.Location = new System.Drawing.Point(722, 52);
            this.cmbDepartment.Name = "cmbDepartment";
            this.cmbDepartment.Size = new System.Drawing.Size(131, 21);
            this.cmbDepartment.TabIndex = 1;
            this.cmbDepartment.Visible = false;
            this.cmbDepartment.SelectedIndexChanged += new System.EventHandler(this.cmbDepartment_SelectedIndexChanged);
            this.cmbDepartment.SelectionChangeCommitted += new System.EventHandler(this.cmbMonth_SelectedIndexChanged);
            // 
            // chkFestiveMonth
            // 
            this.chkFestiveMonth.AutoSize = true;
            this.chkFestiveMonth.Location = new System.Drawing.Point(550, 25);
            this.chkFestiveMonth.Name = "chkFestiveMonth";
            this.chkFestiveMonth.Size = new System.Drawing.Size(93, 17);
            this.chkFestiveMonth.TabIndex = 4;
            this.chkFestiveMonth.Text = "Festive Month";
            this.chkFestiveMonth.UseVisualStyleBackColor = true;
            this.chkFestiveMonth.CheckedChanged += new System.EventHandler(this.chkFestiveMonth_CheckedChanged);
            // 
            // cmbYear
            // 
            this.cmbYear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbYear.FormattingEnabled = true;
            this.cmbYear.Location = new System.Drawing.Point(363, 22);
            this.cmbYear.Name = "cmbYear";
            this.cmbYear.Size = new System.Drawing.Size(57, 21);
            this.cmbYear.TabIndex = 3;
            this.cmbYear.SelectionChangeCommitted += new System.EventHandler(this.cmbMonth_SelectedIndexChanged);
            // 
            // btnExportExcel
            // 
            this.btnExportExcel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExportExcel.Location = new System.Drawing.Point(886, 17);
            this.btnExportExcel.Name = "btnExportExcel";
            this.btnExportExcel.Size = new System.Drawing.Size(75, 28);
            this.btnExportExcel.TabIndex = 86;
            this.btnExportExcel.Text = "Export CSV";
            this.btnExportExcel.UseVisualStyleBackColor = true;
            this.btnExportExcel.Visible = false;
            this.btnExportExcel.Click += new System.EventHandler(this.btnExportExcel_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(334, 26);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(32, 13);
            this.label5.TabIndex = 65;
            this.label5.Text = "Year:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(216, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 13);
            this.label1.TabIndex = 64;
            this.label1.Text = "Month:";
            // 
            // btndate
            // 
            this.btndate.Location = new System.Drawing.Point(1132, 20);
            this.btndate.Name = "btndate";
            this.btndate.Size = new System.Drawing.Size(26, 25);
            this.btndate.TabIndex = 6;
            this.btndate.UseVisualStyleBackColor = true;
            this.btndate.Click += new System.EventHandler(this.btndate_Click);
            // 
            // lblnumberofdays
            // 
            this.lblnumberofdays.AutoSize = true;
            this.lblnumberofdays.BackColor = System.Drawing.SystemColors.Highlight;
            this.lblnumberofdays.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblnumberofdays.Location = new System.Drawing.Point(514, 25);
            this.lblnumberofdays.Name = "lblnumberofdays";
            this.lblnumberofdays.Size = new System.Drawing.Size(15, 15);
            this.lblnumberofdays.TabIndex = 8;
            this.lblnumberofdays.Text = "0";
            // 
            // txtdate
            // 
            this.txtdate.Location = new System.Drawing.Point(1006, 23);
            this.txtdate.Mask = "##/##/####";
            this.txtdate.Name = "txtdate";
            this.txtdate.Size = new System.Drawing.Size(126, 20);
            this.txtdate.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(967, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(33, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Date:";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(422, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Number of Days:";
            // 
            // lblMsg
            // 
            this.lblMsg.AutoSize = true;
            this.lblMsg.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMsg.Location = new System.Drawing.Point(127, 71);
            this.lblMsg.Name = "lblMsg";
            this.lblMsg.Size = new System.Drawing.Size(0, 16);
            this.lblMsg.TabIndex = 67;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tppayslip);
            this.tabControl1.Controls.Add(this.tpadditionalallowances);
            this.tabControl1.Controls.Add(this.tpotherdeduction);
            this.tabControl1.Location = new System.Drawing.Point(2, 109);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1217, 438);
            this.tabControl1.TabIndex = 6;
            // 
            // tppayslip
            // 
            this.tppayslip.Controls.Add(this.grdPaySlips);
            this.tppayslip.Controls.Add(this.txtnarration);
            this.tppayslip.Controls.Add(this.label4);
            this.tppayslip.Controls.Add(this.btnclose);
            this.tppayslip.Controls.Add(this.btnpay);
            this.tppayslip.Location = new System.Drawing.Point(4, 22);
            this.tppayslip.Name = "tppayslip";
            this.tppayslip.Padding = new System.Windows.Forms.Padding(3);
            this.tppayslip.Size = new System.Drawing.Size(1209, 412);
            this.tppayslip.TabIndex = 0;
            this.tppayslip.Text = "Salary Sheet";
            this.tppayslip.UseVisualStyleBackColor = true;
            this.tppayslip.Click += new System.EventHandler(this.tppayslip_Click);
            // 
            // grdPaySlips
            // 
            this.grdPaySlips.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdPaySlips.EnableSort = true;
            this.grdPaySlips.Location = new System.Drawing.Point(6, 6);
            this.grdPaySlips.Name = "grdPaySlips";
            this.grdPaySlips.OptimizeMode = SourceGrid.CellOptimizeMode.ForRows;
            this.grdPaySlips.SelectionMode = SourceGrid.GridSelectionMode.Cell;
            this.grdPaySlips.Size = new System.Drawing.Size(1197, 369);
            this.grdPaySlips.TabIndex = 4;
            this.grdPaySlips.TabStop = true;
            this.grdPaySlips.ToolTipText = "";
            // 
            // txtnarration
            // 
            this.txtnarration.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.txtnarration.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtnarration.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtnarration.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtnarration.FocusLostColor = System.Drawing.Color.White;
            this.txtnarration.Location = new System.Drawing.Point(78, 384);
            this.txtnarration.Name = "txtnarration";
            this.txtnarration.Size = new System.Drawing.Size(937, 20);
            this.txtnarration.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(18, 386);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Narration:";
            // 
            // btnclose
            // 
            this.btnclose.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnclose.Location = new System.Drawing.Point(1128, 378);
            this.btnclose.Name = "btnclose";
            this.btnclose.Size = new System.Drawing.Size(75, 31);
            this.btnclose.TabIndex = 2;
            this.btnclose.Text = "Close";
            this.btnclose.UseVisualStyleBackColor = true;
            this.btnclose.Click += new System.EventHandler(this.btnclose_Click);
            // 
            // btnpay
            // 
            this.btnpay.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnpay.Location = new System.Drawing.Point(1036, 378);
            this.btnpay.Name = "btnpay";
            this.btnpay.Size = new System.Drawing.Size(86, 31);
            this.btnpay.TabIndex = 1;
            this.btnpay.Text = "Save";
            this.btnpay.UseVisualStyleBackColor = true;
            this.btnpay.Click += new System.EventHandler(this.btnpay_Click);
            // 
            // tpadditionalallowances
            // 
            this.tpadditionalallowances.Controls.Add(this.btncancel);
            this.tpadditionalallowances.Controls.Add(this.btndone);
            this.tpadditionalallowances.Controls.Add(this.grdaddition);
            this.tpadditionalallowances.Location = new System.Drawing.Point(4, 22);
            this.tpadditionalallowances.Name = "tpadditionalallowances";
            this.tpadditionalallowances.Padding = new System.Windows.Forms.Padding(3);
            this.tpadditionalallowances.Size = new System.Drawing.Size(1209, 425);
            this.tpadditionalallowances.TabIndex = 1;
            this.tpadditionalallowances.Text = "Additional Allowances";
            this.tpadditionalallowances.UseVisualStyleBackColor = true;
            // 
            // btncancel
            // 
            this.btncancel.Location = new System.Drawing.Point(1125, 360);
            this.btncancel.Name = "btncancel";
            this.btncancel.Size = new System.Drawing.Size(75, 31);
            this.btncancel.TabIndex = 3;
            this.btncancel.Text = "Cancel";
            this.btncancel.UseVisualStyleBackColor = true;
            this.btncancel.Click += new System.EventHandler(this.btncancel_Click);
            // 
            // btndone
            // 
            this.btndone.Location = new System.Drawing.Point(1033, 360);
            this.btndone.Name = "btndone";
            this.btndone.Size = new System.Drawing.Size(75, 31);
            this.btndone.TabIndex = 2;
            this.btndone.Text = "Done";
            this.btndone.UseVisualStyleBackColor = true;
            this.btndone.Click += new System.EventHandler(this.btndone_Click);
            // 
            // grdaddition
            // 
            this.grdaddition.EnableSort = true;
            this.grdaddition.Location = new System.Drawing.Point(6, 6);
            this.grdaddition.Name = "grdaddition";
            this.grdaddition.OptimizeMode = SourceGrid.CellOptimizeMode.ForRows;
            this.grdaddition.SelectionMode = SourceGrid.GridSelectionMode.Cell;
            this.grdaddition.Size = new System.Drawing.Size(1197, 348);
            this.grdaddition.TabIndex = 1;
            this.grdaddition.TabStop = true;
            this.grdaddition.ToolTipText = "";
            // 
            // tpotherdeduction
            // 
            this.tpotherdeduction.Controls.Add(this.button2);
            this.tpotherdeduction.Controls.Add(this.button3);
            this.tpotherdeduction.Controls.Add(this.grdDeduction);
            this.tpotherdeduction.Location = new System.Drawing.Point(4, 22);
            this.tpotherdeduction.Name = "tpotherdeduction";
            this.tpotherdeduction.Padding = new System.Windows.Forms.Padding(3);
            this.tpotherdeduction.Size = new System.Drawing.Size(1209, 425);
            this.tpotherdeduction.TabIndex = 2;
            this.tpotherdeduction.Text = "Other Deduction";
            this.tpotherdeduction.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(1125, 360);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 31);
            this.button2.TabIndex = 5;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(1033, 360);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 31);
            this.button3.TabIndex = 4;
            this.button3.Text = "Done";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // grdDeduction
            // 
            this.grdDeduction.EnableSort = true;
            this.grdDeduction.Location = new System.Drawing.Point(4, 5);
            this.grdDeduction.Name = "grdDeduction";
            this.grdDeduction.OptimizeMode = SourceGrid.CellOptimizeMode.ForRows;
            this.grdDeduction.SelectionMode = SourceGrid.GridSelectionMode.Cell;
            this.grdDeduction.Size = new System.Drawing.Size(1199, 349);
            this.grdDeduction.TabIndex = 2;
            this.grdDeduction.TabStop = true;
            this.grdDeduction.ToolTipText = "";
            // 
            // grpPosting
            // 
            this.grpPosting.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.grpPosting.Controls.Add(this.chkJournalPosting);
            this.grpPosting.Controls.Add(this.btnJournalEntry);
            this.grpPosting.Controls.Add(this.chkByCash);
            this.grpPosting.Controls.Add(this.lblCashParty);
            this.grpPosting.Controls.Add(this.cmbBankName);
            this.grpPosting.Location = new System.Drawing.Point(809, 56);
            this.grpPosting.Name = "grpPosting";
            this.grpPosting.Size = new System.Drawing.Size(404, 55);
            this.grpPosting.TabIndex = 86;
            this.grpPosting.TabStop = false;
            // 
            // chkJournalPosting
            // 
            this.chkJournalPosting.AutoSize = true;
            this.chkJournalPosting.Checked = true;
            this.chkJournalPosting.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkJournalPosting.Location = new System.Drawing.Point(21, 3);
            this.chkJournalPosting.Name = "chkJournalPosting";
            this.chkJournalPosting.Size = new System.Drawing.Size(98, 17);
            this.chkJournalPosting.TabIndex = 90;
            this.chkJournalPosting.Text = "Journal Posting";
            this.chkJournalPosting.UseVisualStyleBackColor = true;
            this.chkJournalPosting.CheckedChanged += new System.EventHandler(this.chkJournalPosting_CheckedChanged);
            // 
            // btnJournalEntry
            // 
            this.btnJournalEntry.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnJournalEntry.Location = new System.Drawing.Point(310, 24);
            this.btnJournalEntry.Name = "btnJournalEntry";
            this.btnJournalEntry.Size = new System.Drawing.Size(86, 23);
            this.btnJournalEntry.TabIndex = 86;
            this.btnJournalEntry.Text = "Journal Entry";
            this.btnJournalEntry.UseVisualStyleBackColor = true;
            this.btnJournalEntry.Click += new System.EventHandler(this.btnJournalEntry_Click);
            // 
            // chkByCash
            // 
            this.chkByCash.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chkByCash.AutoSize = true;
            this.chkByCash.Location = new System.Drawing.Point(238, 30);
            this.chkByCash.Name = "chkByCash";
            this.chkByCash.Size = new System.Drawing.Size(65, 17);
            this.chkByCash.TabIndex = 89;
            this.chkByCash.Text = "By Cash";
            this.chkByCash.UseVisualStyleBackColor = true;
            this.chkByCash.CheckedChanged += new System.EventHandler(this.chkbycash_CheckedChanged);
            // 
            // lblCashParty
            // 
            this.lblCashParty.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCashParty.AutoSize = true;
            this.lblCashParty.Location = new System.Drawing.Point(9, 34);
            this.lblCashParty.Name = "lblCashParty";
            this.lblCashParty.Size = new System.Drawing.Size(66, 13);
            this.lblCashParty.TabIndex = 87;
            this.lblCashParty.Text = "Bank Name:";
            // 
            // cmbBankName
            // 
            this.cmbBankName.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbBankName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBankName.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.cmbBankName.FocusLostColor = System.Drawing.Color.White;
            this.cmbBankName.FormattingEnabled = true;
            this.cmbBankName.Location = new System.Drawing.Point(81, 26);
            this.cmbBankName.Name = "cmbBankName";
            this.cmbBankName.Size = new System.Drawing.Size(144, 21);
            this.cmbBankName.TabIndex = 88;
            // 
            // btnAdjust
            // 
            this.btnAdjust.Location = new System.Drawing.Point(9, 69);
            this.btnAdjust.Name = "btnAdjust";
            this.btnAdjust.Size = new System.Drawing.Size(105, 23);
            this.btnAdjust.TabIndex = 87;
            this.btnAdjust.Text = "Salary Adjustment";
            this.btnAdjust.UseVisualStyleBackColor = true;
            this.btnAdjust.Click += new System.EventHandler(this.btnAdjust_Click);
            // 
            // frmPaySlip
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1224, 546);
            this.Controls.Add(this.btnAdjust);
            this.Controls.Add(this.grpPosting);
            this.Controls.Add(this.lblMsg);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmPaySlip";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Salary Sheet";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmPaySlip_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tppayslip.ResumeLayout(false);
            this.tppayslip.PerformLayout();
            this.tpadditionalallowances.ResumeLayout(false);
            this.tpotherdeduction.ResumeLayout(false);
            this.grpPosting.ResumeLayout(false);
            this.grpPosting.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox cmbMonth;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btndate;
        private System.Windows.Forms.MaskedTextBox txtdate;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tppayslip;
        private System.Windows.Forms.TabPage tpadditionalallowances;
        private SourceGrid.Grid grdaddition;
        private System.Windows.Forms.Button btncancel;
        private System.Windows.Forms.Button btndone;
        private System.Windows.Forms.Button btnclose;
        private System.Windows.Forms.Button btnpay;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblnumberofdays;
        private STextBox txtnarration;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TabPage tpotherdeduction;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private SourceGrid.Grid grdDeduction;
        private System.Windows.Forms.ComboBox cmbYear;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblMsg;
        private System.Windows.Forms.Button btnExportExcel;
        private System.Windows.Forms.GroupBox grpPosting;
        private System.Windows.Forms.Button btnJournalEntry;
        private System.Windows.Forms.CheckBox chkByCash;
        private System.Windows.Forms.Label lblCashParty;
        private SComboBox cmbBankName;
        private System.Windows.Forms.CheckBox chkFestiveMonth;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cmbDepartment;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cmbFaculty;
        private System.Windows.Forms.CheckBox chkApplyInsurance;
        private System.Windows.Forms.Button btnAdjust;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.ComboBox cmbEmployeeList;
        private System.Windows.Forms.CheckBox chkJournalPosting;
        private SourceGrid.Grid grdPaySlips;
    }
}