namespace AccSwift
{
    partial class frmBconVoucherConfiguration
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmBconVoucherConfiguration));
            this.label27 = new System.Windows.Forms.Label();
            this.grpManualNumValidation = new System.Windows.Forms.GroupBox();
            this.cmbBlankVouNum = new AccSwift.SComboBox();
            this.label28 = new System.Windows.Forms.Label();
            this.cmbDuplicateVouNum = new AccSwift.SComboBox();
            this.label26 = new System.Windows.Forms.Label();
            this.tbVoucherConfig = new System.Windows.Forms.TabPage();
            this.Values = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label10 = new System.Windows.Forms.Label();
            this.Sno = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnCancel = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.Particulars = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnSave = new System.Windows.Forms.Button();
            this.grpPaddingDetails = new System.Windows.Forms.GroupBox();
            this.txtPaddingChar = new AccSwift.STextBox();
            this.txtTotalLengthNumPart = new AccSwift.STextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.btnNumberingFormat = new System.Windows.Forms.Button();
            this.chkSpecifyEndNo = new System.Windows.Forms.CheckBox();
            this.chkFixLength = new System.Windows.Forms.CheckBox();
            this.label20 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.grpAutoNumbering = new System.Windows.Forms.GroupBox();
            this.txtStartingNO = new AccSwift.STextBox();
            this.cboRenumberingFrq = new AccSwift.SComboBox();
            this.label22 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.grpEndingDetails = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.txtWarningMsg = new AccSwift.STextBox();
            this.label25 = new System.Windows.Forms.Label();
            this.txtWarningVouLeft = new AccSwift.STextBox();
            this.txtEndingNo = new AccSwift.STextBox();
            this.tabJournalVouConfig = new System.Windows.Forms.TabControl();
            this.tbNumberingConfig = new System.Windows.Forms.TabPage();
            this.cmbNumberingType = new AccSwift.SComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtSeriesName = new AccSwift.SMaskedTextBox();
            this.grpManualNumValidation.SuspendLayout();
            this.grpPaddingDetails.SuspendLayout();
            this.grpAutoNumbering.SuspendLayout();
            this.grpEndingDetails.SuspendLayout();
            this.tabJournalVouConfig.SuspendLayout();
            this.tbNumberingConfig.SuspendLayout();
            this.SuspendLayout();
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
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(18, 39);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(85, 13);
            this.label26.TabIndex = 13;
            this.label26.Text = "Numbering Type\r\n";
            // 
            // tbVoucherConfig
            // 
            this.tbVoucherConfig.Location = new System.Drawing.Point(4, 22);
            this.tbVoucherConfig.Name = "tbVoucherConfig";
            this.tbVoucherConfig.Padding = new System.Windows.Forms.Padding(3);
            this.tbVoucherConfig.Size = new System.Drawing.Size(596, 490);
            this.tbVoucherConfig.TabIndex = 2;
            this.tbVoucherConfig.Text = "Voucher Configuration";
            this.tbVoucherConfig.UseVisualStyleBackColor = true;
            // 
            // Values
            // 
            this.Values.DisplayIndex = 2;
            this.Values.Text = "Values";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(610, 228);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(0, 13);
            this.label10.TabIndex = 72;
            // 
            // Sno
            // 
            this.Sno.DisplayIndex = 0;
            this.Sno.Text = "S.No.";
            this.Sno.Width = 40;
            // 
            // btnCancel
            // 
            this.btnCancel.Image = ((System.Drawing.Image)(resources.GetObject("btnCancel.Image")));
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(416, 584);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 75;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(607, 194);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(0, 13);
            this.label9.TabIndex = 71;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(602, 85);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(0, 13);
            this.label6.TabIndex = 73;
            // 
            // Particulars
            // 
            this.Particulars.DisplayIndex = 1;
            this.Particulars.Text = "Particulars";
            this.Particulars.Width = 100;
            // 
            // btnSave
            // 
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSave.Location = new System.Drawing.Point(316, 584);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 74;
            this.btnSave.Text = "&Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
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
            this.grpEndingDetails.Location = new System.Drawing.Point(0, 66);
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
            // tabJournalVouConfig
            // 
            this.tabJournalVouConfig.Controls.Add(this.tbNumberingConfig);
            this.tabJournalVouConfig.Controls.Add(this.tbVoucherConfig);
            this.tabJournalVouConfig.Location = new System.Drawing.Point(12, 44);
            this.tabJournalVouConfig.Name = "tabJournalVouConfig";
            this.tabJournalVouConfig.SelectedIndex = 0;
            this.tabJournalVouConfig.Size = new System.Drawing.Size(604, 516);
            this.tabJournalVouConfig.TabIndex = 69;
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
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 13);
            this.label1.TabIndex = 70;
            this.label1.Text = "Series Name";
            // 
            // txtSeriesName
            // 
            this.txtSeriesName.BackColor = System.Drawing.Color.White;
            this.txtSeriesName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSeriesName.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.txtSeriesName.FocusLostColor = System.Drawing.Color.White;
            this.txtSeriesName.Location = new System.Drawing.Point(107, 9);
            this.txtSeriesName.Name = "txtSeriesName";
            this.txtSeriesName.Size = new System.Drawing.Size(163, 20);
            this.txtSeriesName.TabIndex = 68;
            // 
            // frmBconVoucherConfiguration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(689, 624);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtSeriesName);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.tabJournalVouConfig);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "frmBconVoucherConfiguration";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Bank Reconciliation Voucher Configuration";
            this.Load += new System.EventHandler(this.frmPurchaseVoucherConfiguration_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmPurchaseVoucherConfiguration_KeyDown);
            this.grpManualNumValidation.ResumeLayout(false);
            this.grpManualNumValidation.PerformLayout();
            this.grpPaddingDetails.ResumeLayout(false);
            this.grpPaddingDetails.PerformLayout();
            this.grpAutoNumbering.ResumeLayout(false);
            this.grpAutoNumbering.PerformLayout();
            this.grpEndingDetails.ResumeLayout(false);
            this.grpEndingDetails.PerformLayout();
            this.tabJournalVouConfig.ResumeLayout(false);
            this.tbNumberingConfig.ResumeLayout(false);
            this.tbNumberingConfig.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label27;
        private STextBox txtStartingNO;
        private SComboBox cmbBlankVouNum;
        private SComboBox cmbNumberingType;
        private System.Windows.Forms.GroupBox grpManualNumValidation;
        private System.Windows.Forms.Label label28;
        private SComboBox cmbDuplicateVouNum;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.TabPage tbVoucherConfig;
        private System.Windows.Forms.ColumnHeader Values;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ColumnHeader Sno;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ColumnHeader Particulars;
        private SMaskedTextBox txtSeriesName;
        private STextBox txtPaddingChar;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.GroupBox grpPaddingDetails;
        private STextBox txtTotalLengthNumPart;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Button btnNumberingFormat;
        private System.Windows.Forms.CheckBox chkSpecifyEndNo;
        private System.Windows.Forms.CheckBox chkFixLength;
        private STextBox txtEndingNo;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label21;
        private SComboBox cboRenumberingFrq;
        private System.Windows.Forms.GroupBox grpAutoNumbering;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.GroupBox grpEndingDetails;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label24;
        private STextBox txtWarningMsg;
        private System.Windows.Forms.Label label25;
        private STextBox txtWarningVouLeft;
        private System.Windows.Forms.TabControl tabJournalVouConfig;
        private System.Windows.Forms.TabPage tbNumberingConfig;
        private System.Windows.Forms.Label label1;

    }
}