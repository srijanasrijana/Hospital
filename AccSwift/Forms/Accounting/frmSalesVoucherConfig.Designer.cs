using SComponents;
namespace Inventory
{
    partial class frmSalesVoucherConfig
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSalesVoucherConfig));
            this.txtStartingNO = new STextBox();
            this.cboRenumberingFrq = new SComboBox();
            this.tabJournalVouConfig = new System.Windows.Forms.TabControl();
            this.tbNumberingConfig = new System.Windows.Forms.TabPage();
            this.btnNumberingFormat = new System.Windows.Forms.Button();
            this.grpAutoNumbering = new System.Windows.Forms.GroupBox();
            this.chkSpecifyEndNo = new System.Windows.Forms.CheckBox();
            this.chkFixLength = new System.Windows.Forms.CheckBox();
            this.label20 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.grpEndingDetails = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.txtWarningMsg = new STextBox();
            this.label25 = new System.Windows.Forms.Label();
            this.txtWarningVouLeft = new STextBox();
            this.txtEndingNo = new STextBox();
            this.grpPaddingDetails = new System.Windows.Forms.GroupBox();
            this.txtPaddingChar = new STextBox();
            this.txtTotalLengthNumPart = new STextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.grpManualNumValidation = new System.Windows.Forms.GroupBox();
            this.cmbBlankVouNum = new SComboBox();
            this.label27 = new System.Windows.Forms.Label();
            this.label28 = new System.Windows.Forms.Label();
            this.cmbDuplicateVouNum = new SComboBox();
            this.cmbNumberingType = new SComboBox();
            this.tbVoucherConfig = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkoptionalfield5 = new System.Windows.Forms.CheckBox();
            this.chkoptionalfield4 = new System.Windows.Forms.CheckBox();
            this.chkoptionalfield3 = new System.Windows.Forms.CheckBox();
            this.chkoptionalfield2 = new System.Windows.Forms.CheckBox();
            this.chkoptionalfield1 = new System.Windows.Forms.CheckBox();
            this.chkField5 = new System.Windows.Forms.CheckBox();
            this.chkField4 = new System.Windows.Forms.CheckBox();
            this.chkField3 = new System.Windows.Forms.CheckBox();
            this.chkField2 = new System.Windows.Forms.CheckBox();
            this.chkField1 = new System.Windows.Forms.CheckBox();
            this.label17 = new System.Windows.Forms.Label();
            this.txtfifth = new STextBox();
            this.txtfourth = new STextBox();
            this.txtthird = new STextBox();
            this.txtsecond = new STextBox();
            this.txtfirst = new STextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.lblfourth = new System.Windows.Forms.Label();
            this.lblsecond = new System.Windows.Forms.Label();
            this.lblthird = new System.Windows.Forms.Label();
            this.lblfifth = new System.Windows.Forms.Label();
            this.lblfirst = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtoptionalfield = new STextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.Values = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Sno = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Particulars = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnCancel = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtSeriesName = new SMaskedTextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.tabJournalVouConfig.SuspendLayout();
            this.tbNumberingConfig.SuspendLayout();
            this.grpAutoNumbering.SuspendLayout();
            this.grpEndingDetails.SuspendLayout();
            this.grpPaddingDetails.SuspendLayout();
            this.grpManualNumValidation.SuspendLayout();
            this.tbVoucherConfig.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtStartingNO
            // 
            this.txtStartingNO.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtStartingNO.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtStartingNO.FocusLostColor = System.Drawing.Color.White;
            this.txtStartingNO.Location = new System.Drawing.Point(183, 19);
            this.txtStartingNO.Name = "txtStartingNO";
            this.txtStartingNO.Size = new System.Drawing.Size(175, 20);
            this.txtStartingNO.TabIndex = 0;
            // 
            // cboRenumberingFrq
            // 
            this.cboRenumberingFrq.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboRenumberingFrq.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.cboRenumberingFrq.FocusLostColor = System.Drawing.Color.White;
            this.cboRenumberingFrq.FormattingEnabled = true;
            this.cboRenumberingFrq.Items.AddRange(new object[] {
            "Daily",
            "Annual"});
            this.cboRenumberingFrq.Location = new System.Drawing.Point(183, 187);
            this.cboRenumberingFrq.Name = "cboRenumberingFrq";
            this.cboRenumberingFrq.Size = new System.Drawing.Size(175, 21);
            this.cboRenumberingFrq.TabIndex = 2;
            this.cboRenumberingFrq.SelectedIndexChanged += new System.EventHandler(this.cboRenumberingFrq_SelectedIndexChanged);
            // 
            // tabJournalVouConfig
            // 
            this.tabJournalVouConfig.Controls.Add(this.tbNumberingConfig);
            this.tabJournalVouConfig.Controls.Add(this.tbVoucherConfig);
            this.tabJournalVouConfig.Location = new System.Drawing.Point(12, 57);
            this.tabJournalVouConfig.Name = "tabJournalVouConfig";
            this.tabJournalVouConfig.SelectedIndex = 0;
            this.tabJournalVouConfig.Size = new System.Drawing.Size(604, 516);
            this.tabJournalVouConfig.TabIndex = 48;
            // 
            // tbNumberingConfig
            // 
            this.tbNumberingConfig.Controls.Add(this.btnNumberingFormat);
            this.tbNumberingConfig.Controls.Add(this.grpAutoNumbering);
            this.tbNumberingConfig.Controls.Add(this.label26);
            this.tbNumberingConfig.Controls.Add(this.grpManualNumValidation);
            this.tbNumberingConfig.Controls.Add(this.cmbNumberingType);
            this.tbNumberingConfig.Location = new System.Drawing.Point(4, 22);
            this.tbNumberingConfig.Name = "tbNumberingConfig";
            this.tbNumberingConfig.Padding = new System.Windows.Forms.Padding(3);
            this.tbNumberingConfig.Size = new System.Drawing.Size(596, 490);
            this.tbNumberingConfig.TabIndex = 0;
            this.tbNumberingConfig.Text = "Numbering Configuration";
            this.tbNumberingConfig.UseVisualStyleBackColor = true;
            // 
            // btnNumberingFormat
            // 
            this.btnNumberingFormat.Location = new System.Drawing.Point(471, 34);
            this.btnNumberingFormat.Name = "btnNumberingFormat";
            this.btnNumberingFormat.Size = new System.Drawing.Size(107, 23);
            this.btnNumberingFormat.TabIndex = 1;
            this.btnNumberingFormat.Text = "Numbering Format";
            this.btnNumberingFormat.UseVisualStyleBackColor = true;
            this.btnNumberingFormat.Click += new System.EventHandler(this.btnNumberingFormat_Click);
            // 
            // grpAutoNumbering
            // 
            this.grpAutoNumbering.Controls.Add(this.txtStartingNO);
            this.grpAutoNumbering.Controls.Add(this.chkSpecifyEndNo);
            this.grpAutoNumbering.Controls.Add(this.chkFixLength);
            this.grpAutoNumbering.Controls.Add(this.cboRenumberingFrq);
            this.grpAutoNumbering.Controls.Add(this.label20);
            this.grpAutoNumbering.Controls.Add(this.label21);
            this.grpAutoNumbering.Controls.Add(this.label22);
            this.grpAutoNumbering.Controls.Add(this.label23);
            this.grpAutoNumbering.Controls.Add(this.grpEndingDetails);
            this.grpAutoNumbering.Controls.Add(this.grpPaddingDetails);
            this.grpAutoNumbering.Location = new System.Drawing.Point(1, 156);
            this.grpAutoNumbering.Name = "grpAutoNumbering";
            this.grpAutoNumbering.Size = new System.Drawing.Size(577, 320);
            this.grpAutoNumbering.TabIndex = 16;
            this.grpAutoNumbering.TabStop = false;
            this.grpAutoNumbering.Text = "Auto Numbering Info";
            // 
            // chkSpecifyEndNo
            // 
            this.chkSpecifyEndNo.AutoSize = true;
            this.chkSpecifyEndNo.Location = new System.Drawing.Point(183, 46);
            this.chkSpecifyEndNo.Name = "chkSpecifyEndNo";
            this.chkSpecifyEndNo.Size = new System.Drawing.Size(15, 14);
            this.chkSpecifyEndNo.TabIndex = 1;
            this.chkSpecifyEndNo.UseVisualStyleBackColor = true;
            this.chkSpecifyEndNo.CheckedChanged += new System.EventHandler(this.chkSpecifyEndNo_CheckedChanged);
            // 
            // chkFixLength
            // 
            this.chkFixLength.AutoSize = true;
            this.chkFixLength.Location = new System.Drawing.Point(183, 216);
            this.chkFixLength.Name = "chkFixLength";
            this.chkFixLength.Size = new System.Drawing.Size(15, 14);
            this.chkFixLength.TabIndex = 3;
            this.chkFixLength.UseVisualStyleBackColor = true;
            this.chkFixLength.CheckedChanged += new System.EventHandler(this.chkFixLength_CheckedChanged);
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(20, 216);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(132, 13);
            this.label20.TabIndex = 8;
            this.label20.Text = "Fix Length of Numeric Part";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(20, 187);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(123, 13);
            this.label21.TabIndex = 7;
            this.label21.Text = "Renumbering Frequency";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(12, 46);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(98, 13);
            this.label22.TabIndex = 4;
            this.label22.Text = "Specify Ending No.";
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(12, 23);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(63, 13);
            this.label23.TabIndex = 4;
            this.label23.Text = "Starting No.";
            // 
            // grpEndingDetails
            // 
            this.grpEndingDetails.Controls.Add(this.label2);
            this.grpEndingDetails.Controls.Add(this.label24);
            this.grpEndingDetails.Controls.Add(this.txtWarningMsg);
            this.grpEndingDetails.Controls.Add(this.label25);
            this.grpEndingDetails.Controls.Add(this.txtWarningVouLeft);
            this.grpEndingDetails.Controls.Add(this.txtEndingNo);
            this.grpEndingDetails.Location = new System.Drawing.Point(0, 70);
            this.grpEndingDetails.Name = "grpEndingDetails";
            this.grpEndingDetails.Size = new System.Drawing.Size(577, 108);
            this.grpEndingDetails.TabIndex = 6;
            this.grpEndingDetails.TabStop = false;
            this.grpEndingDetails.Text = "Ending Details";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(111, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Warning Voucher Left";
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(15, 79);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(93, 13);
            this.label24.TabIndex = 9;
            this.label24.Text = "Warning Message";
            // 
            // txtWarningMsg
            // 
            this.txtWarningMsg.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtWarningMsg.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtWarningMsg.FocusLostColor = System.Drawing.Color.White;
            this.txtWarningMsg.Location = new System.Drawing.Point(181, 77);
            this.txtWarningMsg.Name = "txtWarningMsg";
            this.txtWarningMsg.Size = new System.Drawing.Size(175, 20);
            this.txtWarningMsg.TabIndex = 2;
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(18, 25);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(60, 13);
            this.label25.TabIndex = 7;
            this.label25.Text = "Ending No.";
            // 
            // txtWarningVouLeft
            // 
            this.txtWarningVouLeft.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtWarningVouLeft.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtWarningVouLeft.FocusLostColor = System.Drawing.Color.White;
            this.txtWarningVouLeft.Location = new System.Drawing.Point(181, 47);
            this.txtWarningVouLeft.Name = "txtWarningVouLeft";
            this.txtWarningVouLeft.Size = new System.Drawing.Size(175, 20);
            this.txtWarningVouLeft.TabIndex = 1;
            // 
            // txtEndingNo
            // 
            this.txtEndingNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtEndingNo.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtEndingNo.FocusLostColor = System.Drawing.Color.White;
            this.txtEndingNo.Location = new System.Drawing.Point(181, 18);
            this.txtEndingNo.Name = "txtEndingNo";
            this.txtEndingNo.Size = new System.Drawing.Size(175, 20);
            this.txtEndingNo.TabIndex = 0;
            // 
            // grpPaddingDetails
            // 
            this.grpPaddingDetails.Controls.Add(this.txtPaddingChar);
            this.grpPaddingDetails.Controls.Add(this.txtTotalLengthNumPart);
            this.grpPaddingDetails.Controls.Add(this.label18);
            this.grpPaddingDetails.Controls.Add(this.label19);
            this.grpPaddingDetails.Location = new System.Drawing.Point(0, 242);
            this.grpPaddingDetails.Name = "grpPaddingDetails";
            this.grpPaddingDetails.Size = new System.Drawing.Size(577, 96);
            this.grpPaddingDetails.TabIndex = 15;
            this.grpPaddingDetails.TabStop = false;
            this.grpPaddingDetails.Text = "Padding Details";
            // 
            // txtPaddingChar
            // 
            this.txtPaddingChar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPaddingChar.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtPaddingChar.FocusLostColor = System.Drawing.Color.White;
            this.txtPaddingChar.Location = new System.Drawing.Point(183, 52);
            this.txtPaddingChar.Name = "txtPaddingChar";
            this.txtPaddingChar.Size = new System.Drawing.Size(175, 20);
            this.txtPaddingChar.TabIndex = 1;
            // 
            // txtTotalLengthNumPart
            // 
            this.txtTotalLengthNumPart.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTotalLengthNumPart.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtTotalLengthNumPart.FocusLostColor = System.Drawing.Color.White;
            this.txtTotalLengthNumPart.Location = new System.Drawing.Point(183, 20);
            this.txtTotalLengthNumPart.Name = "txtTotalLengthNumPart";
            this.txtTotalLengthNumPart.Size = new System.Drawing.Size(175, 20);
            this.txtTotalLengthNumPart.TabIndex = 0;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(20, 27);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(145, 13);
            this.label18.TabIndex = 11;
            this.label18.Text = "Total Length Of Numeric Part";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(23, 54);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(95, 13);
            this.label19.TabIndex = 12;
            this.label19.Text = "Padding Character";
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(18, 39);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(85, 13);
            this.label26.TabIndex = 13;
            this.label26.Text = "Numbering Type\r\n";
            this.label26.Click += new System.EventHandler(this.label26_Click);
            // 
            // grpManualNumValidation
            // 
            this.grpManualNumValidation.Controls.Add(this.cmbBlankVouNum);
            this.grpManualNumValidation.Controls.Add(this.label27);
            this.grpManualNumValidation.Controls.Add(this.label28);
            this.grpManualNumValidation.Controls.Add(this.cmbDuplicateVouNum);
            this.grpManualNumValidation.Location = new System.Drawing.Point(1, 68);
            this.grpManualNumValidation.Name = "grpManualNumValidation";
            this.grpManualNumValidation.Size = new System.Drawing.Size(577, 82);
            this.grpManualNumValidation.TabIndex = 15;
            this.grpManualNumValidation.TabStop = false;
            this.grpManualNumValidation.Text = "Manual Numbering Validation";
            // 
            // cmbBlankVouNum
            // 
            this.cmbBlankVouNum.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBlankVouNum.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.cmbBlankVouNum.FocusLostColor = System.Drawing.Color.White;
            this.cmbBlankVouNum.FormattingEnabled = true;
            this.cmbBlankVouNum.Items.AddRange(new object[] {
            "Warning Only",
            "Dont Allow",
            "No action"});
            this.cmbBlankVouNum.Location = new System.Drawing.Point(183, 46);
            this.cmbBlankVouNum.Name = "cmbBlankVouNum";
            this.cmbBlankVouNum.Size = new System.Drawing.Size(175, 21);
            this.cmbBlankVouNum.TabIndex = 1;
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Location = new System.Drawing.Point(11, 49);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(117, 13);
            this.label27.TabIndex = 2;
            this.label27.Text = "Blank Voucher Number";
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Location = new System.Drawing.Point(12, 24);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(135, 13);
            this.label28.TabIndex = 2;
            this.label28.Text = "Duplicate Voucher Number";
            // 
            // cmbDuplicateVouNum
            // 
            this.cmbDuplicateVouNum.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDuplicateVouNum.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.cmbDuplicateVouNum.FocusLostColor = System.Drawing.Color.White;
            this.cmbDuplicateVouNum.FormattingEnabled = true;
            this.cmbDuplicateVouNum.Items.AddRange(new object[] {
            "Warning Only",
            "Dont Allow",
            "No Action"});
            this.cmbDuplicateVouNum.Location = new System.Drawing.Point(183, 16);
            this.cmbDuplicateVouNum.Name = "cmbDuplicateVouNum";
            this.cmbDuplicateVouNum.Size = new System.Drawing.Size(175, 21);
            this.cmbDuplicateVouNum.TabIndex = 0;
            // 
            // cmbNumberingType
            // 
            this.cmbNumberingType.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.cmbNumberingType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbNumberingType.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.cmbNumberingType.FocusLostColor = System.Drawing.Color.White;
            this.cmbNumberingType.FormattingEnabled = true;
            this.cmbNumberingType.Items.AddRange(new object[] {
            "Automatic",
            "Manual",
            "Not Required"});
            this.cmbNumberingType.Location = new System.Drawing.Point(184, 31);
            this.cmbNumberingType.Name = "cmbNumberingType";
            this.cmbNumberingType.Size = new System.Drawing.Size(175, 21);
            this.cmbNumberingType.TabIndex = 0;
            this.cmbNumberingType.SelectedIndexChanged += new System.EventHandler(this.cmbNumberingType_SelectedIndexChanged);
            // 
            // tbVoucherConfig
            // 
            this.tbVoucherConfig.Controls.Add(this.groupBox1);
            this.tbVoucherConfig.Controls.Add(this.label5);
            this.tbVoucherConfig.Controls.Add(this.txtoptionalfield);
            this.tbVoucherConfig.Controls.Add(this.label4);
            this.tbVoucherConfig.Location = new System.Drawing.Point(4, 22);
            this.tbVoucherConfig.Name = "tbVoucherConfig";
            this.tbVoucherConfig.Padding = new System.Windows.Forms.Padding(3);
            this.tbVoucherConfig.Size = new System.Drawing.Size(596, 490);
            this.tbVoucherConfig.TabIndex = 2;
            this.tbVoucherConfig.Text = "Voucher Configuration";
            this.tbVoucherConfig.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkoptionalfield5);
            this.groupBox1.Controls.Add(this.chkoptionalfield4);
            this.groupBox1.Controls.Add(this.chkoptionalfield3);
            this.groupBox1.Controls.Add(this.chkoptionalfield2);
            this.groupBox1.Controls.Add(this.chkoptionalfield1);
            this.groupBox1.Controls.Add(this.chkField5);
            this.groupBox1.Controls.Add(this.chkField4);
            this.groupBox1.Controls.Add(this.chkField3);
            this.groupBox1.Controls.Add(this.chkField2);
            this.groupBox1.Controls.Add(this.chkField1);
            this.groupBox1.Controls.Add(this.label17);
            this.groupBox1.Controls.Add(this.txtfifth);
            this.groupBox1.Controls.Add(this.txtfourth);
            this.groupBox1.Controls.Add(this.txtthird);
            this.groupBox1.Controls.Add(this.txtsecond);
            this.groupBox1.Controls.Add(this.txtfirst);
            this.groupBox1.Controls.Add(this.label16);
            this.groupBox1.Controls.Add(this.lblfourth);
            this.groupBox1.Controls.Add(this.lblsecond);
            this.groupBox1.Controls.Add(this.lblthird);
            this.groupBox1.Controls.Add(this.lblfifth);
            this.groupBox1.Controls.Add(this.lblfirst);
            this.groupBox1.Location = new System.Drawing.Point(6, 49);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(581, 234);
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Optional Field Information";
            // 
            // chkoptionalfield5
            // 
            this.chkoptionalfield5.AutoSize = true;
            this.chkoptionalfield5.Enabled = false;
            this.chkoptionalfield5.Location = new System.Drawing.Point(8, 211);
            this.chkoptionalfield5.Name = "chkoptionalfield5";
            this.chkoptionalfield5.Size = new System.Drawing.Size(15, 14);
            this.chkoptionalfield5.TabIndex = 21;
            this.chkoptionalfield5.UseVisualStyleBackColor = true;
            this.chkoptionalfield5.CheckedChanged += new System.EventHandler(this.chkoptionalfield5_CheckedChanged);
            // 
            // chkoptionalfield4
            // 
            this.chkoptionalfield4.AutoSize = true;
            this.chkoptionalfield4.Enabled = false;
            this.chkoptionalfield4.Location = new System.Drawing.Point(8, 172);
            this.chkoptionalfield4.Name = "chkoptionalfield4";
            this.chkoptionalfield4.Size = new System.Drawing.Size(15, 14);
            this.chkoptionalfield4.TabIndex = 20;
            this.chkoptionalfield4.UseVisualStyleBackColor = true;
            this.chkoptionalfield4.CheckedChanged += new System.EventHandler(this.chkoptionalfield4_CheckedChanged);
            // 
            // chkoptionalfield3
            // 
            this.chkoptionalfield3.AutoSize = true;
            this.chkoptionalfield3.Enabled = false;
            this.chkoptionalfield3.Location = new System.Drawing.Point(8, 133);
            this.chkoptionalfield3.Name = "chkoptionalfield3";
            this.chkoptionalfield3.Size = new System.Drawing.Size(15, 14);
            this.chkoptionalfield3.TabIndex = 19;
            this.chkoptionalfield3.UseVisualStyleBackColor = true;
            this.chkoptionalfield3.CheckedChanged += new System.EventHandler(this.chkoptionalfield3_CheckedChanged);
            // 
            // chkoptionalfield2
            // 
            this.chkoptionalfield2.AutoSize = true;
            this.chkoptionalfield2.Enabled = false;
            this.chkoptionalfield2.Location = new System.Drawing.Point(8, 95);
            this.chkoptionalfield2.Name = "chkoptionalfield2";
            this.chkoptionalfield2.Size = new System.Drawing.Size(15, 14);
            this.chkoptionalfield2.TabIndex = 18;
            this.chkoptionalfield2.UseVisualStyleBackColor = true;
            this.chkoptionalfield2.CheckedChanged += new System.EventHandler(this.chkoptionalfield2_CheckedChanged);
            // 
            // chkoptionalfield1
            // 
            this.chkoptionalfield1.AutoSize = true;
            this.chkoptionalfield1.Location = new System.Drawing.Point(8, 56);
            this.chkoptionalfield1.Name = "chkoptionalfield1";
            this.chkoptionalfield1.Size = new System.Drawing.Size(15, 14);
            this.chkoptionalfield1.TabIndex = 17;
            this.chkoptionalfield1.UseVisualStyleBackColor = true;
            this.chkoptionalfield1.CheckedChanged += new System.EventHandler(this.chkoptionalfield1_CheckedChanged);
            // 
            // chkField5
            // 
            this.chkField5.AutoSize = true;
            this.chkField5.Enabled = false;
            this.chkField5.Location = new System.Drawing.Point(409, 204);
            this.chkField5.Name = "chkField5";
            this.chkField5.Size = new System.Drawing.Size(15, 14);
            this.chkField5.TabIndex = 16;
            this.chkField5.UseVisualStyleBackColor = true;
            // 
            // chkField4
            // 
            this.chkField4.AutoSize = true;
            this.chkField4.Enabled = false;
            this.chkField4.Location = new System.Drawing.Point(409, 167);
            this.chkField4.Name = "chkField4";
            this.chkField4.Size = new System.Drawing.Size(15, 14);
            this.chkField4.TabIndex = 15;
            this.chkField4.UseVisualStyleBackColor = true;
            // 
            // chkField3
            // 
            this.chkField3.AutoSize = true;
            this.chkField3.Enabled = false;
            this.chkField3.Location = new System.Drawing.Point(409, 127);
            this.chkField3.Name = "chkField3";
            this.chkField3.Size = new System.Drawing.Size(15, 14);
            this.chkField3.TabIndex = 14;
            this.chkField3.UseVisualStyleBackColor = true;
            // 
            // chkField2
            // 
            this.chkField2.AutoSize = true;
            this.chkField2.Enabled = false;
            this.chkField2.Location = new System.Drawing.Point(409, 89);
            this.chkField2.Name = "chkField2";
            this.chkField2.Size = new System.Drawing.Size(15, 14);
            this.chkField2.TabIndex = 13;
            this.chkField2.UseVisualStyleBackColor = true;
            // 
            // chkField1
            // 
            this.chkField1.AutoSize = true;
            this.chkField1.Enabled = false;
            this.chkField1.Location = new System.Drawing.Point(409, 54);
            this.chkField1.Name = "chkField1";
            this.chkField1.Size = new System.Drawing.Size(15, 14);
            this.chkField1.TabIndex = 12;
            this.chkField1.UseVisualStyleBackColor = true;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.Location = new System.Drawing.Point(390, 30);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(58, 13);
            this.label17.TabIndex = 11;
            this.label17.Text = "Required";
            // 
            // txtfifth
            // 
            this.txtfifth.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtfifth.Enabled = false;
            this.txtfifth.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtfifth.FocusLostColor = System.Drawing.Color.White;
            this.txtfifth.Location = new System.Drawing.Point(214, 204);
            this.txtfifth.Name = "txtfifth";
            this.txtfifth.Size = new System.Drawing.Size(182, 20);
            this.txtfifth.TabIndex = 10;
            // 
            // txtfourth
            // 
            this.txtfourth.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtfourth.Enabled = false;
            this.txtfourth.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtfourth.FocusLostColor = System.Drawing.Color.White;
            this.txtfourth.Location = new System.Drawing.Point(214, 165);
            this.txtfourth.Name = "txtfourth";
            this.txtfourth.Size = new System.Drawing.Size(182, 20);
            this.txtfourth.TabIndex = 9;
            // 
            // txtthird
            // 
            this.txtthird.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtthird.Enabled = false;
            this.txtthird.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtthird.FocusLostColor = System.Drawing.Color.White;
            this.txtthird.Location = new System.Drawing.Point(214, 127);
            this.txtthird.Name = "txtthird";
            this.txtthird.Size = new System.Drawing.Size(182, 20);
            this.txtthird.TabIndex = 8;
            // 
            // txtsecond
            // 
            this.txtsecond.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtsecond.Enabled = false;
            this.txtsecond.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtsecond.FocusLostColor = System.Drawing.Color.White;
            this.txtsecond.Location = new System.Drawing.Point(214, 87);
            this.txtsecond.Name = "txtsecond";
            this.txtsecond.Size = new System.Drawing.Size(182, 20);
            this.txtsecond.TabIndex = 7;
            // 
            // txtfirst
            // 
            this.txtfirst.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtfirst.Enabled = false;
            this.txtfirst.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtfirst.FocusLostColor = System.Drawing.Color.White;
            this.txtfirst.Location = new System.Drawing.Point(214, 53);
            this.txtfirst.Name = "txtfirst";
            this.txtfirst.Size = new System.Drawing.Size(182, 20);
            this.txtfirst.TabIndex = 6;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.Location = new System.Drawing.Point(264, 30);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(70, 13);
            this.label16.TabIndex = 5;
            this.label16.Text = "Field Name";
            // 
            // lblfourth
            // 
            this.lblfourth.AutoSize = true;
            this.lblfourth.Location = new System.Drawing.Point(23, 172);
            this.lblfourth.Name = "lblfourth";
            this.lblfourth.Size = new System.Drawing.Size(80, 13);
            this.lblfourth.TabIndex = 4;
            this.lblfourth.Text = "Optional Field 4";
            // 
            // lblsecond
            // 
            this.lblsecond.AutoSize = true;
            this.lblsecond.Location = new System.Drawing.Point(23, 94);
            this.lblsecond.Name = "lblsecond";
            this.lblsecond.Size = new System.Drawing.Size(80, 13);
            this.lblsecond.TabIndex = 3;
            this.lblsecond.Text = "Optional Field 2";
            // 
            // lblthird
            // 
            this.lblthird.AutoSize = true;
            this.lblthird.Location = new System.Drawing.Point(23, 133);
            this.lblthird.Name = "lblthird";
            this.lblthird.Size = new System.Drawing.Size(80, 13);
            this.lblthird.TabIndex = 2;
            this.lblthird.Text = "Optional Field 3";
            // 
            // lblfifth
            // 
            this.lblfifth.AutoSize = true;
            this.lblfifth.Location = new System.Drawing.Point(23, 211);
            this.lblfifth.Name = "lblfifth";
            this.lblfifth.Size = new System.Drawing.Size(80, 13);
            this.lblfifth.TabIndex = 1;
            this.lblfifth.Text = "Optional Field 5";
            // 
            // lblfirst
            // 
            this.lblfirst.AutoSize = true;
            this.lblfirst.Location = new System.Drawing.Point(23, 55);
            this.lblfirst.Name = "lblfirst";
            this.lblfirst.Size = new System.Drawing.Size(80, 13);
            this.lblfirst.TabIndex = 0;
            this.lblfirst.Text = "Optional Field 1";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(249, 17);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(39, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Max(5)";
            this.label5.Visible = false;
            // 
            // txtoptionalfield
            // 
            this.txtoptionalfield.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtoptionalfield.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtoptionalfield.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtoptionalfield.FocusLostColor = System.Drawing.Color.White;
            this.txtoptionalfield.Location = new System.Drawing.Point(200, 13);
            this.txtoptionalfield.Name = "txtoptionalfield";
            this.txtoptionalfield.Size = new System.Drawing.Size(45, 20);
            this.txtoptionalfield.TabIndex = 11;
            this.txtoptionalfield.Visible = false;
            this.txtoptionalfield.Leave += new System.EventHandler(this.txtoptionalfield_Leave);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(8, 15);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(168, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Choose No Of Optional Field";
            this.label4.Visible = false;
            // 
            // Values
            // 
            this.Values.DisplayIndex = 2;
            this.Values.Text = "Values";
            // 
            // Sno
            // 
            this.Sno.DisplayIndex = 0;
            this.Sno.Text = "S.No.";
            this.Sno.Width = 40;
            // 
            // Particulars
            // 
            this.Particulars.DisplayIndex = 1;
            this.Particulars.Text = "Particulars";
            this.Particulars.Width = 100;
            // 
            // btnCancel
            // 
            this.btnCancel.Image = ((System.Drawing.Image)(resources.GetObject("btnCancel.Image")));
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(98, 580);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 67;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(494, 237);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(0, 13);
            this.label10.TabIndex = 58;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(491, 203);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(0, 13);
            this.label9.TabIndex = 57;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(486, 94);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(0, 13);
            this.label6.TabIndex = 59;
            // 
            // txtSeriesName
            // 
            this.txtSeriesName.BackColor = System.Drawing.Color.White;
            this.txtSeriesName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSeriesName.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.txtSeriesName.FocusLostColor = System.Drawing.Color.White;
            this.txtSeriesName.Location = new System.Drawing.Point(98, 18);
            this.txtSeriesName.Name = "txtSeriesName";
            this.txtSeriesName.Size = new System.Drawing.Size(163, 20);
            this.txtSeriesName.TabIndex = 47;
            // 
            // btnSave
            // 
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSave.Location = new System.Drawing.Point(16, 580);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 66;
            this.btnSave.Text = "&Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 13);
            this.label1.TabIndex = 49;
            this.label1.Text = "Series Name";
            // 
            // frmSalesVoucherConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(620, 612);
            this.Controls.Add(this.tabJournalVouConfig);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtSeriesName);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "frmSalesVoucherConfig";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Sales Voucher Configuration";
            this.Load += new System.EventHandler(this.frmSalesVoucherConfig_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmSalesVoucherConfig_KeyDown);
            this.tabJournalVouConfig.ResumeLayout(false);
            this.tbNumberingConfig.ResumeLayout(false);
            this.tbNumberingConfig.PerformLayout();
            this.grpAutoNumbering.ResumeLayout(false);
            this.grpAutoNumbering.PerformLayout();
            this.grpEndingDetails.ResumeLayout(false);
            this.grpEndingDetails.PerformLayout();
            this.grpPaddingDetails.ResumeLayout(false);
            this.grpPaddingDetails.PerformLayout();
            this.grpManualNumValidation.ResumeLayout(false);
            this.grpManualNumValidation.PerformLayout();
            this.tbVoucherConfig.ResumeLayout(false);
            this.tbVoucherConfig.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private STextBox txtStartingNO;
        private SComboBox cboRenumberingFrq;
        private System.Windows.Forms.TabControl tabJournalVouConfig;
        private System.Windows.Forms.TabPage tbNumberingConfig;
        private System.Windows.Forms.Button btnNumberingFormat;
        private System.Windows.Forms.GroupBox grpAutoNumbering;
        private System.Windows.Forms.CheckBox chkSpecifyEndNo;
        private System.Windows.Forms.CheckBox chkFixLength;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.GroupBox grpEndingDetails;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label24;
        private STextBox txtWarningMsg;
        private System.Windows.Forms.Label label25;
        private STextBox txtWarningVouLeft;
        private STextBox txtEndingNo;
        private System.Windows.Forms.GroupBox grpPaddingDetails;
        private STextBox txtPaddingChar;
        private STextBox txtTotalLengthNumPart;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.GroupBox grpManualNumValidation;
        private SComboBox cmbBlankVouNum;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.Label label28;
        private SComboBox cmbDuplicateVouNum;
        private SComboBox cmbNumberingType;
        private System.Windows.Forms.TabPage tbVoucherConfig;
        private System.Windows.Forms.ColumnHeader Values;
        private System.Windows.Forms.ColumnHeader Sno;
        private System.Windows.Forms.ColumnHeader Particulars;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label6;
        private SMaskedTextBox txtSeriesName;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label5;
        private STextBox txtoptionalfield;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chkoptionalfield5;
        private System.Windows.Forms.CheckBox chkoptionalfield4;
        private System.Windows.Forms.CheckBox chkoptionalfield3;
        private System.Windows.Forms.CheckBox chkoptionalfield2;
        private System.Windows.Forms.CheckBox chkoptionalfield1;
        private System.Windows.Forms.CheckBox chkField5;
        private System.Windows.Forms.CheckBox chkField4;
        private System.Windows.Forms.CheckBox chkField3;
        private System.Windows.Forms.CheckBox chkField2;
        private System.Windows.Forms.CheckBox chkField1;
        private System.Windows.Forms.Label label17;
        private STextBox txtfifth;
        private STextBox txtfourth;
        private STextBox txtthird;
        private STextBox txtsecond;
        private STextBox txtfirst;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label lblfourth;
        private System.Windows.Forms.Label lblsecond;
        private System.Windows.Forms.Label lblthird;
        private System.Windows.Forms.Label lblfifth;
        private System.Windows.Forms.Label lblfirst;


    }
}