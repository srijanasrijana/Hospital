using SComponents;
namespace AccSwift.Forms
{
    partial class frmUserPreference
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmUserPreference));
            this.label8 = new System.Windows.Forms.Label();
            this.tvUserPreference = new System.Windows.Forms.TreeView();
            this.lblBreadCrumb = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnApply = new System.Windows.Forms.Button();
            this.tabGeneralMain = new System.Windows.Forms.TabPage();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbAccountClass = new SComponents.SComboBox();
            this.groupBox13 = new System.Windows.Forms.GroupBox();
            this.tvAccClass = new SComponents.STreeView(this.components);
            this.groupBox12 = new System.Windows.Forms.GroupBox();
            this.cmbSalesAccount = new SComponents.SComboBox();
            this.label28 = new System.Windows.Forms.Label();
            this.groupBox11 = new System.Windows.Forms.GroupBox();
            this.cmbPurchaseAccount = new SComponents.SComboBox();
            this.label26 = new System.Windows.Forms.Label();
            this.tabGMOptions = new System.Windows.Forms.TabPage();
            this.groupBox15 = new System.Windows.Forms.GroupBox();
            this.txtPassword = new SComponents.STextBox();
            this.txtUserEmail = new SComponents.STextBox();
            this.txtServerPort = new SComponents.STextBox();
            this.txtMailServer = new SComponents.STextBox();
            this.label34 = new System.Windows.Forms.Label();
            this.label33 = new System.Windows.Forms.Label();
            this.label32 = new System.Windows.Forms.Label();
            this.label31 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.rdbDecimalFormatInNegative = new System.Windows.Forms.RadioButton();
            this.rdbDecimalFormatInBracket = new System.Windows.Forms.RadioButton();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.chkCommaSeparated = new System.Windows.Forms.CheckBox();
            this.cmbDecimalPlaces = new SComponents.SComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbDateFormat = new SComponents.SComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.rbDateNepali = new System.Windows.Forms.RadioButton();
            this.rbDateEnglish = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.tabGMAccounting = new System.Windows.Forms.TabPage();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.cmbCashAccount = new SComponents.SComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.grpNegativeBank = new System.Windows.Forms.GroupBox();
            this.cmbBankAccount = new SComponents.SComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.tabSettings = new System.Windows.Forms.TabControl();
            this.tabCompanyInfo = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtCompanyName = new System.Windows.Forms.TextBox();
            this.txtCompanySlogan = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.txtCompanyPhone = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtCompanyPAN = new System.Windows.Forms.TextBox();
            this.Company_City = new System.Windows.Forms.Label();
            this.txtCompanyCity = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.txtCompanyAddress = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox2.SuspendLayout();
            this.tabGeneralMain.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox13.SuspendLayout();
            this.groupBox12.SuspendLayout();
            this.groupBox11.SuspendLayout();
            this.tabGMOptions.SuspendLayout();
            this.groupBox15.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabGMAccounting.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.grpNegativeBank.SuspendLayout();
            this.tabSettings.SuspendLayout();
            this.tabCompanyInfo.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(2, 6);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(128, 13);
            this.label8.TabIndex = 17;
            this.label8.Text = "Select the settings below:";
            // 
            // tvUserPreference
            // 
            this.tvUserPreference.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.tvUserPreference.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tvUserPreference.FullRowSelect = true;
            this.tvUserPreference.HideSelection = false;
            this.tvUserPreference.Location = new System.Drawing.Point(3, 25);
            this.tvUserPreference.MinimumSize = new System.Drawing.Size(40, 4);
            this.tvUserPreference.Name = "tvUserPreference";
            this.tvUserPreference.Size = new System.Drawing.Size(196, 466);
            this.tvUserPreference.TabIndex = 18;
            this.tvUserPreference.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvUserPreference_AfterSelect);
            // 
            // lblBreadCrumb
            // 
            this.lblBreadCrumb.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.lblBreadCrumb.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblBreadCrumb.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBreadCrumb.ForeColor = System.Drawing.Color.White;
            this.lblBreadCrumb.Location = new System.Drawing.Point(201, -2);
            this.lblBreadCrumb.Name = "lblBreadCrumb";
            this.lblBreadCrumb.Size = new System.Drawing.Size(448, 28);
            this.lblBreadCrumb.TabIndex = 19;
            this.lblBreadCrumb.Text = " Current Selection: General > System";
            this.lblBreadCrumb.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnOK);
            this.groupBox2.Controls.Add(this.btnCancel);
            this.groupBox2.Controls.Add(this.btnApply);
            this.groupBox2.Location = new System.Drawing.Point(201, 445);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(448, 46);
            this.groupBox2.TabIndex = 20;
            this.groupBox2.TabStop = false;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(203, 14);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 26);
            this.btnOK.TabIndex = 12;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(365, 14);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 26);
            this.btnCancel.TabIndex = 14;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnApply
            // 
            this.btnApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnApply.Location = new System.Drawing.Point(284, 14);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(75, 26);
            this.btnApply.TabIndex = 13;
            this.btnApply.Text = "Apply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // tabGeneralMain
            // 
            this.tabGeneralMain.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.tabGeneralMain.Controls.Add(this.groupBox4);
            this.tabGeneralMain.Controls.Add(this.groupBox13);
            this.tabGeneralMain.Controls.Add(this.groupBox12);
            this.tabGeneralMain.Controls.Add(this.groupBox11);
            this.tabGeneralMain.Location = new System.Drawing.Point(4, 4);
            this.tabGeneralMain.Name = "tabGeneralMain";
            this.tabGeneralMain.Padding = new System.Windows.Forms.Padding(3);
            this.tabGeneralMain.Size = new System.Drawing.Size(440, 401);
            this.tabGeneralMain.TabIndex = 1;
            this.tabGeneralMain.Text = "GeneralMain";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label1);
            this.groupBox4.Controls.Add(this.cmbAccountClass);
            this.groupBox4.Location = new System.Drawing.Point(3, 173);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(390, 52);
            this.groupBox4.TabIndex = 25;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Accounting Class";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(129, 13);
            this.label1.TabIndex = 42;
            this.label1.Text = "Default Accounting Class:";
            // 
            // cmbAccountClass
            // 
            this.cmbAccountClass.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAccountClass.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.cmbAccountClass.FocusLostColor = System.Drawing.Color.White;
            this.cmbAccountClass.FormattingEnabled = true;
            this.cmbAccountClass.Location = new System.Drawing.Point(159, 16);
            this.cmbAccountClass.Name = "cmbAccountClass";
            this.cmbAccountClass.Size = new System.Drawing.Size(152, 21);
            this.cmbAccountClass.TabIndex = 41;
            // 
            // groupBox13
            // 
            this.groupBox13.Controls.Add(this.tvAccClass);
            this.groupBox13.Location = new System.Drawing.Point(3, 260);
            this.groupBox13.Name = "groupBox13";
            this.groupBox13.Size = new System.Drawing.Size(397, 118);
            this.groupBox13.TabIndex = 24;
            this.groupBox13.TabStop = false;
            this.groupBox13.Text = "Choose Accounting Class";
            this.groupBox13.Visible = false;
            // 
            // tvAccClass
            // 
            this.tvAccClass.AutoCheckChild = true;
            this.tvAccClass.CheckBoxes = true;
            this.tvAccClass.Location = new System.Drawing.Point(7, 18);
            this.tvAccClass.Name = "tvAccClass";
            this.tvAccClass.Size = new System.Drawing.Size(384, 93);
            this.tvAccClass.TabIndex = 28;
            // 
            // groupBox12
            // 
            this.groupBox12.Controls.Add(this.cmbSalesAccount);
            this.groupBox12.Controls.Add(this.label28);
            this.groupBox12.Location = new System.Drawing.Point(3, 91);
            this.groupBox12.Name = "groupBox12";
            this.groupBox12.Size = new System.Drawing.Size(400, 69);
            this.groupBox12.TabIndex = 22;
            this.groupBox12.TabStop = false;
            this.groupBox12.Text = "Sales Account";
            // 
            // cmbSalesAccount
            // 
            this.cmbSalesAccount.BackColor = System.Drawing.Color.White;
            this.cmbSalesAccount.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSalesAccount.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.cmbSalesAccount.FocusLostColor = System.Drawing.Color.White;
            this.cmbSalesAccount.FormattingEnabled = true;
            this.cmbSalesAccount.Location = new System.Drawing.Point(166, 22);
            this.cmbSalesAccount.Name = "cmbSalesAccount";
            this.cmbSalesAccount.Size = new System.Drawing.Size(152, 21);
            this.cmbSalesAccount.TabIndex = 18;
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Location = new System.Drawing.Point(14, 30);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(116, 13);
            this.label28.TabIndex = 0;
            this.label28.Text = "Default Sales Account:";
            // 
            // groupBox11
            // 
            this.groupBox11.Controls.Add(this.cmbPurchaseAccount);
            this.groupBox11.Controls.Add(this.label26);
            this.groupBox11.Location = new System.Drawing.Point(3, 15);
            this.groupBox11.Name = "groupBox11";
            this.groupBox11.Size = new System.Drawing.Size(400, 70);
            this.groupBox11.TabIndex = 21;
            this.groupBox11.TabStop = false;
            this.groupBox11.Text = "Purchase Account";
            // 
            // cmbPurchaseAccount
            // 
            this.cmbPurchaseAccount.BackColor = System.Drawing.Color.White;
            this.cmbPurchaseAccount.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPurchaseAccount.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.cmbPurchaseAccount.FocusLostColor = System.Drawing.Color.White;
            this.cmbPurchaseAccount.FormattingEnabled = true;
            this.cmbPurchaseAccount.Location = new System.Drawing.Point(166, 27);
            this.cmbPurchaseAccount.Name = "cmbPurchaseAccount";
            this.cmbPurchaseAccount.Size = new System.Drawing.Size(152, 21);
            this.cmbPurchaseAccount.TabIndex = 18;
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(14, 30);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(135, 13);
            this.label26.TabIndex = 0;
            this.label26.Text = "Default Purchase Account:";
            // 
            // tabGMOptions
            // 
            this.tabGMOptions.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.tabGMOptions.Controls.Add(this.groupBox15);
            this.tabGMOptions.Controls.Add(this.groupBox3);
            this.tabGMOptions.Controls.Add(this.groupBox5);
            this.tabGMOptions.Controls.Add(this.groupBox1);
            this.tabGMOptions.Location = new System.Drawing.Point(4, 4);
            this.tabGMOptions.Name = "tabGMOptions";
            this.tabGMOptions.Padding = new System.Windows.Forms.Padding(3);
            this.tabGMOptions.Size = new System.Drawing.Size(440, 401);
            this.tabGMOptions.TabIndex = 0;
            this.tabGMOptions.Text = "GMOptions";
            // 
            // groupBox15
            // 
            this.groupBox15.BackColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox15.Controls.Add(this.txtPassword);
            this.groupBox15.Controls.Add(this.txtUserEmail);
            this.groupBox15.Controls.Add(this.txtServerPort);
            this.groupBox15.Controls.Add(this.txtMailServer);
            this.groupBox15.Controls.Add(this.label34);
            this.groupBox15.Controls.Add(this.label33);
            this.groupBox15.Controls.Add(this.label32);
            this.groupBox15.Controls.Add(this.label31);
            this.groupBox15.Location = new System.Drawing.Point(6, 243);
            this.groupBox15.Name = "groupBox15";
            this.groupBox15.Size = new System.Drawing.Size(434, 156);
            this.groupBox15.TabIndex = 21;
            this.groupBox15.TabStop = false;
            this.groupBox15.Text = "Email Settings";
            // 
            // txtPassword
            // 
            this.txtPassword.BackColor = System.Drawing.Color.White;
            this.txtPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPassword.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtPassword.FocusLostColor = System.Drawing.Color.White;
            this.txtPassword.Location = new System.Drawing.Point(157, 117);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(225, 20);
            this.txtPassword.TabIndex = 16;
            // 
            // txtUserEmail
            // 
            this.txtUserEmail.BackColor = System.Drawing.Color.White;
            this.txtUserEmail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtUserEmail.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtUserEmail.FocusLostColor = System.Drawing.Color.White;
            this.txtUserEmail.Location = new System.Drawing.Point(157, 85);
            this.txtUserEmail.Name = "txtUserEmail";
            this.txtUserEmail.Size = new System.Drawing.Size(225, 20);
            this.txtUserEmail.TabIndex = 15;
            this.txtUserEmail.TextChanged += new System.EventHandler(this.txtUserEmail_TextChanged);
            // 
            // txtServerPort
            // 
            this.txtServerPort.BackColor = System.Drawing.Color.White;
            this.txtServerPort.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtServerPort.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtServerPort.FocusLostColor = System.Drawing.Color.White;
            this.txtServerPort.Location = new System.Drawing.Point(157, 50);
            this.txtServerPort.Name = "txtServerPort";
            this.txtServerPort.Size = new System.Drawing.Size(225, 20);
            this.txtServerPort.TabIndex = 14;
            // 
            // txtMailServer
            // 
            this.txtMailServer.BackColor = System.Drawing.Color.White;
            this.txtMailServer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtMailServer.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtMailServer.FocusLostColor = System.Drawing.Color.White;
            this.txtMailServer.Location = new System.Drawing.Point(157, 16);
            this.txtMailServer.Name = "txtMailServer";
            this.txtMailServer.Size = new System.Drawing.Size(225, 20);
            this.txtMailServer.TabIndex = 13;
            // 
            // label34
            // 
            this.label34.AutoSize = true;
            this.label34.Location = new System.Drawing.Point(21, 117);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(56, 13);
            this.label34.TabIndex = 12;
            this.label34.Text = "Password:";
            // 
            // label33
            // 
            this.label33.AutoSize = true;
            this.label33.Location = new System.Drawing.Point(18, 85);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(60, 13);
            this.label33.TabIndex = 11;
            this.label33.Text = "User Email:";
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.Location = new System.Drawing.Point(18, 50);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(63, 13);
            this.label32.TabIndex = 10;
            this.label32.Text = "Server Port:";
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.Location = new System.Drawing.Point(18, 19);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(66, 13);
            this.label31.TabIndex = 9;
            this.label31.Text = "Mail Server :";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.rdbDecimalFormatInNegative);
            this.groupBox3.Controls.Add(this.rdbDecimalFormatInBracket);
            this.groupBox3.Location = new System.Drawing.Point(6, 165);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(403, 75);
            this.groupBox3.TabIndex = 20;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Decimal Format";
            // 
            // rdbDecimalFormatInNegative
            // 
            this.rdbDecimalFormatInNegative.AutoSize = true;
            this.rdbDecimalFormatInNegative.Location = new System.Drawing.Point(39, 46);
            this.rdbDecimalFormatInNegative.Name = "rdbDecimalFormatInNegative";
            this.rdbDecimalFormatInNegative.Size = new System.Drawing.Size(61, 17);
            this.rdbDecimalFormatInNegative.TabIndex = 19;
            this.rdbDecimalFormatInNegative.TabStop = true;
            this.rdbDecimalFormatInNegative.Text = "-422.30";
            this.rdbDecimalFormatInNegative.UseVisualStyleBackColor = true;
            // 
            // rdbDecimalFormatInBracket
            // 
            this.rdbDecimalFormatInBracket.AutoSize = true;
            this.rdbDecimalFormatInBracket.Location = new System.Drawing.Point(39, 23);
            this.rdbDecimalFormatInBracket.Name = "rdbDecimalFormatInBracket";
            this.rdbDecimalFormatInBracket.Size = new System.Drawing.Size(64, 17);
            this.rdbDecimalFormatInBracket.TabIndex = 18;
            this.rdbDecimalFormatInBracket.TabStop = true;
            this.rdbDecimalFormatInBracket.Text = "(422.30)";
            this.rdbDecimalFormatInBracket.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.chkCommaSeparated);
            this.groupBox5.Controls.Add(this.cmbDecimalPlaces);
            this.groupBox5.Controls.Add(this.label7);
            this.groupBox5.Location = new System.Drawing.Point(6, 103);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(403, 56);
            this.groupBox5.TabIndex = 20;
            this.groupBox5.TabStop = false;
            // 
            // chkCommaSeparated
            // 
            this.chkCommaSeparated.AutoSize = true;
            this.chkCommaSeparated.Location = new System.Drawing.Point(224, 23);
            this.chkCommaSeparated.Name = "chkCommaSeparated";
            this.chkCommaSeparated.Size = new System.Drawing.Size(156, 17);
            this.chkCommaSeparated.TabIndex = 19;
            this.chkCommaSeparated.Text = "Comma Separated numbers";
            this.chkCommaSeparated.UseVisualStyleBackColor = true;
            // 
            // cmbDecimalPlaces
            // 
            this.cmbDecimalPlaces.BackColor = System.Drawing.Color.White;
            this.cmbDecimalPlaces.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDecimalPlaces.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.cmbDecimalPlaces.FocusLostColor = System.Drawing.Color.White;
            this.cmbDecimalPlaces.FormattingEnabled = true;
            this.cmbDecimalPlaces.Location = new System.Drawing.Point(151, 19);
            this.cmbDecimalPlaces.Name = "cmbDecimalPlaces";
            this.cmbDecimalPlaces.Size = new System.Drawing.Size(51, 21);
            this.cmbDecimalPlaces.TabIndex = 17;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(13, 22);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(132, 13);
            this.label7.TabIndex = 16;
            this.label7.Text = "Number of Decimal Places";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.cmbDateFormat);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.rbDateNepali);
            this.groupBox1.Controls.Add(this.rbDateEnglish);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(6, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(403, 94);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Date";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label5.Location = new System.Drawing.Point(272, 61);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(67, 15);
            this.label5.TabIndex = 6;
            this.label5.Text = "2011/12/28";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(221, 61);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(45, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Sample:";
            // 
            // cmbDateFormat
            // 
            this.cmbDateFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDateFormat.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.cmbDateFormat.FocusLostColor = System.Drawing.Color.White;
            this.cmbDateFormat.FormattingEnabled = true;
            this.cmbDateFormat.Items.AddRange(new object[] {
            "YYYY/MM/DD",
            "DD/MM/YYYY",
            "MM/DD/YYYY"});
            this.cmbDateFormat.Location = new System.Drawing.Point(74, 55);
            this.cmbDateFormat.Name = "cmbDateFormat";
            this.cmbDateFormat.Size = new System.Drawing.Size(108, 21);
            this.cmbDateFormat.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 58);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(42, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Format:";
            // 
            // rbDateNepali
            // 
            this.rbDateNepali.AutoSize = true;
            this.rbDateNepali.Location = new System.Drawing.Point(176, 23);
            this.rbDateNepali.Name = "rbDateNepali";
            this.rbDateNepali.Size = new System.Drawing.Size(55, 17);
            this.rbDateNepali.TabIndex = 2;
            this.rbDateNepali.TabStop = true;
            this.rbDateNepali.Text = "Nepali";
            this.rbDateNepali.UseVisualStyleBackColor = true;
            // 
            // rbDateEnglish
            // 
            this.rbDateEnglish.AutoSize = true;
            this.rbDateEnglish.Location = new System.Drawing.Point(98, 23);
            this.rbDateEnglish.Name = "rbDateEnglish";
            this.rbDateEnglish.Size = new System.Drawing.Size(59, 17);
            this.rbDateEnglish.TabIndex = 1;
            this.rbDateEnglish.TabStop = true;
            this.rbDateEnglish.Text = "English";
            this.rbDateEnglish.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Default Date:";
            // 
            // tabGMAccounting
            // 
            this.tabGMAccounting.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.tabGMAccounting.Controls.Add(this.groupBox6);
            this.tabGMAccounting.Controls.Add(this.grpNegativeBank);
            this.tabGMAccounting.Location = new System.Drawing.Point(4, 4);
            this.tabGMAccounting.Name = "tabGMAccounting";
            this.tabGMAccounting.Size = new System.Drawing.Size(440, 401);
            this.tabGMAccounting.TabIndex = 2;
            this.tabGMAccounting.Text = "GMAccounting";
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.cmbCashAccount);
            this.groupBox6.Controls.Add(this.label11);
            this.groupBox6.Location = new System.Drawing.Point(9, 5);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(400, 71);
            this.groupBox6.TabIndex = 20;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Cash Account";
            // 
            // cmbCashAccount
            // 
            this.cmbCashAccount.BackColor = System.Drawing.Color.White;
            this.cmbCashAccount.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCashAccount.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.cmbCashAccount.FocusLostColor = System.Drawing.Color.White;
            this.cmbCashAccount.FormattingEnabled = true;
            this.cmbCashAccount.Location = new System.Drawing.Point(134, 27);
            this.cmbCashAccount.Name = "cmbCashAccount";
            this.cmbCashAccount.Size = new System.Drawing.Size(105, 21);
            this.cmbCashAccount.TabIndex = 18;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(14, 30);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(114, 13);
            this.label11.TabIndex = 0;
            this.label11.Text = "Default Cash Account:";
            // 
            // grpNegativeBank
            // 
            this.grpNegativeBank.Controls.Add(this.cmbBankAccount);
            this.grpNegativeBank.Controls.Add(this.label13);
            this.grpNegativeBank.Location = new System.Drawing.Point(9, 82);
            this.grpNegativeBank.Name = "grpNegativeBank";
            this.grpNegativeBank.Size = new System.Drawing.Size(399, 71);
            this.grpNegativeBank.TabIndex = 15;
            this.grpNegativeBank.TabStop = false;
            this.grpNegativeBank.Text = "Bank Account";
            // 
            // cmbBankAccount
            // 
            this.cmbBankAccount.BackColor = System.Drawing.Color.White;
            this.cmbBankAccount.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBankAccount.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.cmbBankAccount.FocusLostColor = System.Drawing.Color.White;
            this.cmbBankAccount.FormattingEnabled = true;
            this.cmbBankAccount.Location = new System.Drawing.Point(133, 24);
            this.cmbBankAccount.Name = "cmbBankAccount";
            this.cmbBankAccount.Size = new System.Drawing.Size(105, 21);
            this.cmbBankAccount.TabIndex = 20;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(13, 27);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(115, 13);
            this.label13.TabIndex = 19;
            this.label13.Text = "Default Bank Account:";
            // 
            // tabSettings
            // 
            this.tabSettings.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.tabSettings.Controls.Add(this.tabGMAccounting);
            this.tabSettings.Controls.Add(this.tabGMOptions);
            this.tabSettings.Controls.Add(this.tabGeneralMain);
            this.tabSettings.Controls.Add(this.tabCompanyInfo);
            this.tabSettings.ItemSize = new System.Drawing.Size(0, 1);
            this.tabSettings.Location = new System.Drawing.Point(201, 29);
            this.tabSettings.Multiline = true;
            this.tabSettings.Name = "tabSettings";
            this.tabSettings.SelectedIndex = 0;
            this.tabSettings.Size = new System.Drawing.Size(448, 410);
            this.tabSettings.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabSettings.TabIndex = 21;
            // 
            // tabCompanyInfo
            // 
            this.tabCompanyInfo.Controls.Add(this.panel1);
            this.tabCompanyInfo.Controls.Add(this.label6);
            this.tabCompanyInfo.Location = new System.Drawing.Point(4, 4);
            this.tabCompanyInfo.Name = "tabCompanyInfo";
            this.tabCompanyInfo.Size = new System.Drawing.Size(440, 401);
            this.tabCompanyInfo.TabIndex = 3;
            this.tabCompanyInfo.Text = "Company Informations";
            this.tabCompanyInfo.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txtCompanyName);
            this.panel1.Controls.Add(this.txtCompanySlogan);
            this.panel1.Controls.Add(this.label15);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.txtCompanyPhone);
            this.panel1.Controls.Add(this.label10);
            this.panel1.Controls.Add(this.txtCompanyPAN);
            this.panel1.Controls.Add(this.Company_City);
            this.panel1.Controls.Add(this.txtCompanyCity);
            this.panel1.Controls.Add(this.label14);
            this.panel1.Controls.Add(this.txtCompanyAddress);
            this.panel1.Controls.Add(this.label16);
            this.panel1.Location = new System.Drawing.Point(20, 25);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(416, 306);
            this.panel1.TabIndex = 13;
            // 
            // txtCompanyName
            // 
            this.txtCompanyName.Location = new System.Drawing.Point(110, 14);
            this.txtCompanyName.Name = "txtCompanyName";
            this.txtCompanyName.Size = new System.Drawing.Size(293, 20);
            this.txtCompanyName.TabIndex = 0;
            // 
            // txtCompanySlogan
            // 
            this.txtCompanySlogan.Location = new System.Drawing.Point(110, 264);
            this.txtCompanySlogan.Name = "txtCompanySlogan";
            this.txtCompanySlogan.Size = new System.Drawing.Size(293, 20);
            this.txtCompanySlogan.TabIndex = 12;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(17, 266);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(87, 13);
            this.label15.TabIndex = 6;
            this.label15.Text = "Company Slogan";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(21, 15);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(82, 13);
            this.label9.TabIndex = 2;
            this.label9.Text = "Company Name";
            // 
            // txtCompanyPhone
            // 
            this.txtCompanyPhone.Location = new System.Drawing.Point(110, 214);
            this.txtCompanyPhone.Name = "txtCompanyPhone";
            this.txtCompanyPhone.Size = new System.Drawing.Size(293, 20);
            this.txtCompanyPhone.TabIndex = 11;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(18, 64);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(92, 13);
            this.label10.TabIndex = 3;
            this.label10.Text = "Company Address";
            // 
            // txtCompanyPAN
            // 
            this.txtCompanyPAN.Location = new System.Drawing.Point(110, 166);
            this.txtCompanyPAN.Name = "txtCompanyPAN";
            this.txtCompanyPAN.Size = new System.Drawing.Size(293, 20);
            this.txtCompanyPAN.TabIndex = 10;
            // 
            // Company_City
            // 
            this.Company_City.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.Company_City.AutoSize = true;
            this.Company_City.Location = new System.Drawing.Point(21, 113);
            this.Company_City.Name = "Company_City";
            this.Company_City.Size = new System.Drawing.Size(71, 13);
            this.Company_City.TabIndex = 4;
            this.Company_City.Text = "Company City";
            // 
            // txtCompanyCity
            // 
            this.txtCompanyCity.Location = new System.Drawing.Point(110, 110);
            this.txtCompanyCity.Name = "txtCompanyCity";
            this.txtCompanyCity.Size = new System.Drawing.Size(293, 20);
            this.txtCompanyCity.TabIndex = 9;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(21, 169);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(76, 13);
            this.label14.TabIndex = 5;
            this.label14.Text = "Company PAN";
            // 
            // txtCompanyAddress
            // 
            this.txtCompanyAddress.Location = new System.Drawing.Point(110, 61);
            this.txtCompanyAddress.Name = "txtCompanyAddress";
            this.txtCompanyAddress.Size = new System.Drawing.Size(293, 20);
            this.txtCompanyAddress.TabIndex = 8;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(18, 217);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(85, 13);
            this.label16.TabIndex = 7;
            this.label16.Text = "Company Phone";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(4, 4);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(229, 17);
            this.label6.TabIndex = 1;
            this.label6.Text = "Add the Company Informations";
            // 
            // frmUserPreference
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(651, 498);
            this.Controls.Add(this.tabSettings);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.lblBreadCrumb);
            this.Controls.Add(this.tvUserPreference);
            this.Controls.Add(this.label8);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmUserPreference";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "User Preference";
            this.Load += new System.EventHandler(this.frmUserPreference_Load);
            this.groupBox2.ResumeLayout(false);
            this.tabGeneralMain.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox13.ResumeLayout(false);
            this.groupBox12.ResumeLayout(false);
            this.groupBox12.PerformLayout();
            this.groupBox11.ResumeLayout(false);
            this.groupBox11.PerformLayout();
            this.tabGMOptions.ResumeLayout(false);
            this.groupBox15.ResumeLayout(false);
            this.groupBox15.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabGMAccounting.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.grpNegativeBank.ResumeLayout(false);
            this.grpNegativeBank.PerformLayout();
            this.tabSettings.ResumeLayout(false);
            this.tabCompanyInfo.ResumeLayout(false);
            this.tabCompanyInfo.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TreeView tvUserPreference;
        private System.Windows.Forms.Label lblBreadCrumb;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.TabPage tabGeneralMain;
        private System.Windows.Forms.GroupBox groupBox12;
        private SComboBox cmbSalesAccount;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.GroupBox groupBox11;
        private SComboBox cmbPurchaseAccount;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.TabPage tabGMOptions;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton rdbDecimalFormatInNegative;
        private System.Windows.Forms.RadioButton rdbDecimalFormatInBracket;
        private System.Windows.Forms.GroupBox groupBox5;
        private SComboBox cmbDecimalPlaces;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private SComboBox cmbDateFormat;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RadioButton rbDateNepali;
        private System.Windows.Forms.RadioButton rbDateEnglish;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TabPage tabGMAccounting;
        private System.Windows.Forms.GroupBox groupBox6;
        private SComboBox cmbCashAccount;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.GroupBox grpNegativeBank;
        private SComboBox cmbBankAccount;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TabControl tabSettings;
        private System.Windows.Forms.CheckBox chkCommaSeparated;
        private System.Windows.Forms.GroupBox groupBox13;
        private STreeView tvAccClass;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label1;
        private SComboBox cmbAccountClass;
        private System.Windows.Forms.GroupBox groupBox15;
        private STextBox txtPassword;
        private STextBox txtUserEmail;
        private STextBox txtServerPort;
        private STextBox txtMailServer;
        private System.Windows.Forms.Label label34;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.TabPage tabCompanyInfo;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox txtCompanyName;
        private System.Windows.Forms.TextBox txtCompanySlogan;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtCompanyPhone;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtCompanyPAN;
        private System.Windows.Forms.Label Company_City;
        private System.Windows.Forms.TextBox txtCompanyCity;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox txtCompanyAddress;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label6;
    }
}