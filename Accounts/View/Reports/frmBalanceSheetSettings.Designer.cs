using SComponents;
namespace Accounts
{
    partial class frmBalanceSheetSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmBalanceSheetSettings));
            this.chkShowZeroBal = new System.Windows.Forms.CheckBox();
            this.rdbtnDetail = new System.Windows.Forms.RadioButton();
            this.rdSummary = new System.Windows.Forms.RadioButton();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtToDate = new System.Windows.Forms.MaskedTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnShow = new System.Windows.Forms.Button();
            this.chkShowSecLevGrpDet = new System.Windows.Forms.CheckBox();
            this.btnSelectAccClass = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.cboProjectName = new SComponents.SComboBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.sComboBox2 = new SComponents.SComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.sTextBox1 = new SComponents.STextBox();
            this.sComboBox3 = new SComponents.SComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtRate = new SComponents.STextBox();
            this.sComboBox1 = new SComponents.SComboBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.btnToday = new System.Windows.Forms.Button();
            this.btnDate = new System.Windows.Forms.Button();
            this.cboMonths = new SComponents.SComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.chkDate = new System.Windows.Forms.CheckBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.sComboBox4 = new SComponents.SComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.sTextBox2 = new SComponents.STextBox();
            this.sComboBox5 = new SComponents.SComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.cmbbalancesheetstyle = new SComponents.SComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnsavestate = new System.Windows.Forms.Button();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.groupBox9.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkShowZeroBal
            // 
            this.chkShowZeroBal.AutoSize = true;
            this.chkShowZeroBal.Location = new System.Drawing.Point(55, 15);
            this.chkShowZeroBal.Name = "chkShowZeroBal";
            this.chkShowZeroBal.Size = new System.Drawing.Size(120, 17);
            this.chkShowZeroBal.TabIndex = 30;
            this.chkShowZeroBal.Text = "Show Zero Balance";
            this.chkShowZeroBal.UseVisualStyleBackColor = true;
            this.chkShowZeroBal.KeyDown += new System.Windows.Forms.KeyEventHandler(this.chkShowZeroBal_KeyDown);
            // 
            // rdbtnDetail
            // 
            this.rdbtnDetail.AutoSize = true;
            this.rdbtnDetail.Location = new System.Drawing.Point(97, 17);
            this.rdbtnDetail.Name = "rdbtnDetail";
            this.rdbtnDetail.Size = new System.Drawing.Size(52, 17);
            this.rdbtnDetail.TabIndex = 29;
            this.rdbtnDetail.TabStop = true;
            this.rdbtnDetail.Text = "Detail";
            this.rdbtnDetail.UseVisualStyleBackColor = true;
            this.rdbtnDetail.KeyDown += new System.Windows.Forms.KeyEventHandler(this.rdbtnDetail_KeyDown);
            // 
            // rdSummary
            // 
            this.rdSummary.AutoSize = true;
            this.rdSummary.Location = new System.Drawing.Point(14, 16);
            this.rdSummary.Name = "rdSummary";
            this.rdSummary.Size = new System.Drawing.Size(68, 17);
            this.rdSummary.TabIndex = 28;
            this.rdSummary.TabStop = true;
            this.rdSummary.Text = "Summary";
            this.rdSummary.UseVisualStyleBackColor = true;
            this.rdSummary.KeyDown += new System.Windows.Forms.KeyEventHandler(this.rdSummary_KeyDown);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(221, 18);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(30, 13);
            this.label4.TabIndex = 26;
            this.label4.Text = "Rate";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 17);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 13);
            this.label3.TabIndex = 24;
            this.label3.Text = "Currency";
            // 
            // txtToDate
            // 
            this.txtToDate.Location = new System.Drawing.Point(162, 32);
            this.txtToDate.Mask = "##/##/####";
            this.txtToDate.Name = "txtToDate";
            this.txtToDate.Size = new System.Drawing.Size(126, 20);
            this.txtToDate.TabIndex = 7;
            this.txtToDate.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtToDate_KeyDown);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(175, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "End of Date:";
            // 
            // btnShow
            // 
            this.btnShow.Location = new System.Drawing.Point(10, 7);
            this.btnShow.Name = "btnShow";
            this.btnShow.Size = new System.Drawing.Size(75, 23);
            this.btnShow.TabIndex = 22;
            this.btnShow.Text = "Show";
            this.btnShow.UseVisualStyleBackColor = true;
            this.btnShow.Click += new System.EventHandler(this.btnShow_Click);
            // 
            // chkShowSecLevGrpDet
            // 
            this.chkShowSecLevGrpDet.AutoSize = true;
            this.chkShowSecLevGrpDet.Location = new System.Drawing.Point(203, 15);
            this.chkShowSecLevGrpDet.Name = "chkShowSecLevGrpDet";
            this.chkShowSecLevGrpDet.Size = new System.Drawing.Size(189, 17);
            this.chkShowSecLevGrpDet.TabIndex = 33;
            this.chkShowSecLevGrpDet.Text = "Show Second Level Group Details";
            this.chkShowSecLevGrpDet.UseVisualStyleBackColor = true;
            this.chkShowSecLevGrpDet.Visible = false;
            this.chkShowSecLevGrpDet.KeyDown += new System.Windows.Forms.KeyEventHandler(this.chkShowSecLevGrpDet_KeyDown);
            // 
            // btnSelectAccClass
            // 
            this.btnSelectAccClass.Location = new System.Drawing.Point(225, 11);
            this.btnSelectAccClass.Name = "btnSelectAccClass";
            this.btnSelectAccClass.Size = new System.Drawing.Size(160, 23);
            this.btnSelectAccClass.TabIndex = 34;
            this.btnSelectAccClass.Text = "Select Account Class";
            this.btnSelectAccClass.UseVisualStyleBackColor = true;
            this.btnSelectAccClass.Click += new System.EventHandler(this.btnSelectAccClass_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 17);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(46, 13);
            this.label5.TabIndex = 52;
            this.label5.Text = "Project: ";
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox2.Controls.Add(this.btnSelectAccClass);
            this.groupBox2.Location = new System.Drawing.Point(0, 70);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(399, 40);
            this.groupBox2.TabIndex = 50;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Accounting Class";
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox1.Controls.Add(this.rdSummary);
            this.groupBox1.Controls.Add(this.rdbtnDetail);
            this.groupBox1.Location = new System.Drawing.Point(0, 205);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(186, 41);
            this.groupBox1.TabIndex = 56;
            this.groupBox1.TabStop = false;
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.cboProjectName);
            this.groupBox3.Location = new System.Drawing.Point(188, 205);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(211, 40);
            this.groupBox3.TabIndex = 57;
            this.groupBox3.TabStop = false;
            // 
            // cboProjectName
            // 
            this.cboProjectName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboProjectName.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.cboProjectName.FocusLostColor = System.Drawing.Color.White;
            this.cboProjectName.FormattingEnabled = true;
            this.cboProjectName.Location = new System.Drawing.Point(60, 14);
            this.cboProjectName.Name = "cboProjectName";
            this.cboProjectName.Size = new System.Drawing.Size(148, 21);
            this.cboProjectName.TabIndex = 53;
            this.cboProjectName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cboProjectName_KeyDown);
            // 
            // groupBox4
            // 
            this.groupBox4.BackColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox4.Controls.Add(this.groupBox7);
            this.groupBox4.Controls.Add(this.label4);
            this.groupBox4.Controls.Add(this.txtRate);
            this.groupBox4.Controls.Add(this.sComboBox1);
            this.groupBox4.Controls.Add(this.label3);
            this.groupBox4.Location = new System.Drawing.Point(0, 153);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(399, 43);
            this.groupBox4.TabIndex = 58;
            this.groupBox4.TabStop = false;
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.sComboBox2);
            this.groupBox7.Controls.Add(this.label8);
            this.groupBox7.Controls.Add(this.label9);
            this.groupBox7.Controls.Add(this.sTextBox1);
            this.groupBox7.Controls.Add(this.sComboBox3);
            this.groupBox7.Controls.Add(this.label10);
            this.groupBox7.Location = new System.Drawing.Point(1, -59);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(399, 40);
            this.groupBox7.TabIndex = 59;
            this.groupBox7.TabStop = false;
            // 
            // sComboBox2
            // 
            this.sComboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.sComboBox2.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.sComboBox2.FocusLostColor = System.Drawing.Color.White;
            this.sComboBox2.FormattingEnabled = true;
            this.sComboBox2.Location = new System.Drawing.Point(165, -9);
            this.sComboBox2.Name = "sComboBox2";
            this.sComboBox2.Size = new System.Drawing.Size(148, 21);
            this.sComboBox2.TabIndex = 54;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(17, -6);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(106, 13);
            this.label8.TabIndex = 25;
            this.label8.Text = "Balance Sheet Style:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(221, 15);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(30, 13);
            this.label9.TabIndex = 26;
            this.label9.Text = "Rate";
            // 
            // sTextBox1
            // 
            this.sTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.sTextBox1.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.sTextBox1.FocusLostColor = System.Drawing.Color.White;
            this.sTextBox1.Location = new System.Drawing.Point(284, 13);
            this.sTextBox1.Name = "sTextBox1";
            this.sTextBox1.Size = new System.Drawing.Size(54, 20);
            this.sTextBox1.TabIndex = 27;
            // 
            // sComboBox3
            // 
            this.sComboBox3.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.sComboBox3.FocusLostColor = System.Drawing.Color.White;
            this.sComboBox3.FormattingEnabled = true;
            this.sComboBox3.Location = new System.Drawing.Point(58, 13);
            this.sComboBox3.Name = "sComboBox3";
            this.sComboBox3.Size = new System.Drawing.Size(125, 21);
            this.sComboBox3.TabIndex = 25;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(7, 16);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(49, 13);
            this.label10.TabIndex = 24;
            this.label10.Text = "Currency";
            // 
            // txtRate
            // 
            this.txtRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtRate.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtRate.FocusLostColor = System.Drawing.Color.White;
            this.txtRate.Location = new System.Drawing.Point(284, 14);
            this.txtRate.Name = "txtRate";
            this.txtRate.Size = new System.Drawing.Size(54, 20);
            this.txtRate.TabIndex = 27;
            this.txtRate.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtRate_KeyDown);
            // 
            // sComboBox1
            // 
            this.sComboBox1.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.sComboBox1.FocusLostColor = System.Drawing.Color.White;
            this.sComboBox1.FormattingEnabled = true;
            this.sComboBox1.Location = new System.Drawing.Point(63, 15);
            this.sComboBox1.Name = "sComboBox1";
            this.sComboBox1.Size = new System.Drawing.Size(125, 21);
            this.sComboBox1.TabIndex = 25;
            this.sComboBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.sComboBox1_KeyDown);
            // 
            // groupBox5
            // 
            this.groupBox5.BackColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox5.Controls.Add(this.btnToday);
            this.groupBox5.Controls.Add(this.btnDate);
            this.groupBox5.Controls.Add(this.cboMonths);
            this.groupBox5.Controls.Add(this.label1);
            this.groupBox5.Controls.Add(this.chkDate);
            this.groupBox5.Controls.Add(this.txtToDate);
            this.groupBox5.Controls.Add(this.label2);
            this.groupBox5.Location = new System.Drawing.Point(0, 2);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(399, 62);
            this.groupBox5.TabIndex = 59;
            this.groupBox5.TabStop = false;
            // 
            // btnToday
            // 
            this.btnToday.Location = new System.Drawing.Point(329, 27);
            this.btnToday.Name = "btnToday";
            this.btnToday.Size = new System.Drawing.Size(56, 25);
            this.btnToday.TabIndex = 63;
            this.btnToday.Text = "&Today";
            this.btnToday.UseVisualStyleBackColor = true;
            this.btnToday.Click += new System.EventHandler(this.btnToday_Click);
            // 
            // btnDate
            // 
            this.btnDate.Image = global::Accounts.Properties.Resources.dateIcon;
            this.btnDate.Location = new System.Drawing.Point(303, 27);
            this.btnDate.Name = "btnDate";
            this.btnDate.Size = new System.Drawing.Size(26, 25);
            this.btnDate.TabIndex = 62;
            this.btnDate.UseVisualStyleBackColor = true;
            this.btnDate.Click += new System.EventHandler(this.btnDate_Click);
            // 
            // cboMonths
            // 
            this.cboMonths.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMonths.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.cboMonths.FocusLostColor = System.Drawing.Color.White;
            this.cboMonths.FormattingEnabled = true;
            this.cboMonths.Location = new System.Drawing.Point(10, 32);
            this.cboMonths.Name = "cboMonths";
            this.cboMonths.Size = new System.Drawing.Size(139, 21);
            this.cboMonths.TabIndex = 61;
            this.cboMonths.SelectedIndexChanged += new System.EventHandler(this.cboMonths_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(38, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 13);
            this.label1.TabIndex = 60;
            this.label1.Text = "End of Month";
            // 
            // chkDate
            // 
            this.chkDate.AutoSize = true;
            this.chkDate.Checked = true;
            this.chkDate.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDate.Location = new System.Drawing.Point(0, 0);
            this.chkDate.Name = "chkDate";
            this.chkDate.Size = new System.Drawing.Size(93, 17);
            this.chkDate.TabIndex = 54;
            this.chkDate.Text = "Click For Date";
            this.chkDate.UseVisualStyleBackColor = true;
            this.chkDate.CheckedChanged += new System.EventHandler(this.chkDate_CheckedChanged);
            // 
            // btnCancel
            // 
            this.btnCancel.Image = global::Accounts.Properties.Resources.gnome_window_close;
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(10, 45);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 61;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(74, 15);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(106, 13);
            this.label6.TabIndex = 25;
            this.label6.Text = "Balance Sheet Style:";
            // 
            // groupBox8
            // 
            this.groupBox8.BackColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox8.Controls.Add(this.groupBox9);
            this.groupBox8.Controls.Add(this.label6);
            this.groupBox8.Controls.Add(this.cmbbalancesheetstyle);
            this.groupBox8.Location = new System.Drawing.Point(0, 113);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(399, 41);
            this.groupBox8.TabIndex = 62;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "Style";
            // 
            // groupBox9
            // 
            this.groupBox9.Controls.Add(this.sComboBox4);
            this.groupBox9.Controls.Add(this.label11);
            this.groupBox9.Controls.Add(this.label12);
            this.groupBox9.Controls.Add(this.sTextBox2);
            this.groupBox9.Controls.Add(this.sComboBox5);
            this.groupBox9.Controls.Add(this.label13);
            this.groupBox9.Location = new System.Drawing.Point(1, -59);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(399, 40);
            this.groupBox9.TabIndex = 59;
            this.groupBox9.TabStop = false;
            // 
            // sComboBox4
            // 
            this.sComboBox4.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.sComboBox4.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.sComboBox4.FocusLostColor = System.Drawing.Color.White;
            this.sComboBox4.FormattingEnabled = true;
            this.sComboBox4.Location = new System.Drawing.Point(165, -9);
            this.sComboBox4.Name = "sComboBox4";
            this.sComboBox4.Size = new System.Drawing.Size(148, 21);
            this.sComboBox4.TabIndex = 54;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(17, -6);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(106, 13);
            this.label11.TabIndex = 25;
            this.label11.Text = "Balance Sheet Style:";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(221, 15);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(30, 13);
            this.label12.TabIndex = 26;
            this.label12.Text = "Rate";
            // 
            // sTextBox2
            // 
            this.sTextBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.sTextBox2.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.sTextBox2.FocusLostColor = System.Drawing.Color.White;
            this.sTextBox2.Location = new System.Drawing.Point(284, 13);
            this.sTextBox2.Name = "sTextBox2";
            this.sTextBox2.Size = new System.Drawing.Size(54, 20);
            this.sTextBox2.TabIndex = 27;
            // 
            // sComboBox5
            // 
            this.sComboBox5.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.sComboBox5.FocusLostColor = System.Drawing.Color.White;
            this.sComboBox5.FormattingEnabled = true;
            this.sComboBox5.Location = new System.Drawing.Point(58, 13);
            this.sComboBox5.Name = "sComboBox5";
            this.sComboBox5.Size = new System.Drawing.Size(125, 21);
            this.sComboBox5.TabIndex = 25;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(7, 16);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(49, 13);
            this.label13.TabIndex = 24;
            this.label13.Text = "Currency";
            // 
            // cmbbalancesheetstyle
            // 
            this.cmbbalancesheetstyle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbbalancesheetstyle.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.cmbbalancesheetstyle.FocusLostColor = System.Drawing.Color.White;
            this.cmbbalancesheetstyle.FormattingEnabled = true;
            this.cmbbalancesheetstyle.Items.AddRange(new object[] {
            "Vertical",
            "T-Format",
            "Standard"});
            this.cmbbalancesheetstyle.Location = new System.Drawing.Point(229, 14);
            this.cmbbalancesheetstyle.Name = "cmbbalancesheetstyle";
            this.cmbbalancesheetstyle.Size = new System.Drawing.Size(160, 21);
            this.cmbbalancesheetstyle.TabIndex = 54;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.LightSteelBlue;
            this.panel1.Controls.Add(this.btnsavestate);
            this.panel1.Controls.Add(this.btnShow);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Location = new System.Drawing.Point(399, -1);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(91, 293);
            this.panel1.TabIndex = 63;
            // 
            // btnsavestate
            // 
            this.btnsavestate.Location = new System.Drawing.Point(10, 80);
            this.btnsavestate.Name = "btnsavestate";
            this.btnsavestate.Size = new System.Drawing.Size(75, 23);
            this.btnsavestate.TabIndex = 62;
            this.btnsavestate.Text = "Save State";
            this.btnsavestate.UseVisualStyleBackColor = true;
            this.btnsavestate.Click += new System.EventHandler(this.btnsavestate_Click);
            // 
            // groupBox6
            // 
            this.groupBox6.BackColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox6.Controls.Add(this.chkShowZeroBal);
            this.groupBox6.Controls.Add(this.chkShowSecLevGrpDet);
            this.groupBox6.Location = new System.Drawing.Point(-3, 251);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(399, 38);
            this.groupBox6.TabIndex = 60;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Info";
            this.groupBox6.Enter += new System.EventHandler(this.groupBox6_Enter);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(141, 670);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(120, 17);
            this.checkBox1.TabIndex = 30;
            this.checkBox1.Text = "Show Zero Balance";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.chkShowZeroBal_KeyDown);
            // 
            // frmBalanceSheetSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(489, 291);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox8);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox5);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "frmBalanceSheetSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Balance Sheet Settings";
            this.Load += new System.EventHandler(this.frmBalanceSheetSettings_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmBalanceSheetSettings_KeyDown);
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            this.groupBox9.ResumeLayout(false);
            this.groupBox9.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkShowZeroBal;
        private System.Windows.Forms.RadioButton rdbtnDetail;
        private System.Windows.Forms.RadioButton rdSummary;
        private STextBox txtRate;
        private System.Windows.Forms.Label label4;
        private SComboBox sComboBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.MaskedTextBox txtToDate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnShow;
        private System.Windows.Forms.CheckBox chkShowSecLevGrpDet;
        private System.Windows.Forms.Button btnSelectAccClass;
        private SComboBox cboProjectName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.CheckBox chkDate;
        private SComboBox cboMonths;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnToday;
        private System.Windows.Forms.Button btnDate;
        private System.Windows.Forms.Label label6;
        private SComboBox cmbbalancesheetstyle;
        private System.Windows.Forms.GroupBox groupBox7;
        private SComboBox sComboBox2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private STextBox sTextBox1;
        private SComboBox sComboBox3;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.GroupBox groupBox9;
        private SComboBox sComboBox4;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private STextBox sTextBox2;
        private SComboBox sComboBox5;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Button btnsavestate;

    }
}