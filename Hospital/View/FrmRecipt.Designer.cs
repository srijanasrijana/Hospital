namespace Hospital.View
{
    partial class FrmRecipt
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmRecipt));
            this.txtRegNo = new System.Windows.Forms.TextBox();
            this.printDocument1 = new System.Drawing.Printing.PrintDocument();
            this.label7 = new System.Windows.Forms.Label();
            this.timerProductPIN = new System.Windows.Forms.Timer(this.components);
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnRCancel = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnNew = new System.Windows.Forms.Button();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.chkDoNotClose = new System.Windows.Forms.CheckBox();
            this.txtAddress = new SComponents.STextBox();
            this.btnExport = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label9 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtRegDate = new SComponents.SMaskedTextBox();
            this.label81 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtRemarks = new SComponents.STextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtMobile = new SComponents.STextBox();
            this.btnStartDate = new System.Windows.Forms.Button();
            this.txtGender = new SComponents.STextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tbSalarySheet = new System.Windows.Forms.TabPage();
            this.grdSalarySheet = new SourceGrid.Grid();
            this.lblNetAmout = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnTenderAmount = new System.Windows.Forms.Button();
            this.txtAge = new SComponents.STextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtTelephone = new SComponents.STextBox();
            this.cboName = new SComponents.SComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtCity = new SComponents.STextBox();
            this.chkRecurring = new System.Windows.Forms.CheckBox();
            this.lblPostStatus = new System.Windows.Forms.Label();
            this.btnSetup = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnPrev = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnCopy = new System.Windows.Forms.Button();
            this.btnFirst = new System.Windows.Forms.Button();
            this.btnLast = new System.Windows.Forms.Button();
            this.btnPaste = new System.Windows.Forms.Button();
            this.btnPrintPreview = new System.Windows.Forms.Button();
            this.btnList = new System.Windows.Forms.Button();
            this.groupBox3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tbSalarySheet.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtRegNo
            // 
            this.txtRegNo.BackColor = System.Drawing.Color.AntiqueWhite;
            this.txtRegNo.Enabled = false;
            this.txtRegNo.Location = new System.Drawing.Point(118, 60);
            this.txtRegNo.Name = "txtRegNo";
            this.txtRegNo.Size = new System.Drawing.Size(218, 20);
            this.txtRegNo.TabIndex = 240;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(8, 63);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(94, 13);
            this.label7.TabIndex = 241;
            this.label7.Text = "Patient RegNo:";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(1137, 11);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 12;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.btnRCancel);
            this.groupBox3.Controls.Add(this.btnEdit);
            this.groupBox3.Controls.Add(this.btnDelete);
            this.groupBox3.Controls.Add(this.btnSave);
            this.groupBox3.Controls.Add(this.btnNew);
            this.groupBox3.Controls.Add(this.checkBox2);
            this.groupBox3.Controls.Add(this.chkDoNotClose);
            this.groupBox3.Controls.Add(this.btnCancel);
            this.groupBox3.Location = new System.Drawing.Point(10, 371);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox3.Size = new System.Drawing.Size(1088, 38);
            this.groupBox3.TabIndex = 248;
            this.groupBox3.TabStop = false;
            // 
            // btnRCancel
            // 
            this.btnRCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRCancel.Image = ((System.Drawing.Image)(resources.GetObject("btnRCancel.Image")));
            this.btnRCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRCancel.Location = new System.Drawing.Point(983, 11);
            this.btnRCancel.Name = "btnRCancel";
            this.btnRCancel.Size = new System.Drawing.Size(75, 23);
            this.btnRCancel.TabIndex = 30;
            this.btnRCancel.Text = "&Cancel";
            this.btnRCancel.UseVisualStyleBackColor = true;
            // 
            // btnEdit
            // 
            this.btnEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEdit.Image = ((System.Drawing.Image)(resources.GetObject("btnEdit.Image")));
            this.btnEdit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEdit.Location = new System.Drawing.Point(740, 11);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(75, 23);
            this.btnEdit.TabIndex = 27;
            this.btnEdit.Text = "&Edit";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDelete.Image = ((System.Drawing.Image)(resources.GetObject("btnDelete.Image")));
            this.btnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDelete.Location = new System.Drawing.Point(902, 11);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 29;
            this.btnDelete.Text = "&Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSave.Location = new System.Drawing.Point(821, 11);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 28;
            this.btnSave.Text = "&Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnNew
            // 
            this.btnNew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNew.Image = ((System.Drawing.Image)(resources.GetObject("btnNew.Image")));
            this.btnNew.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNew.Location = new System.Drawing.Point(659, 11);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(75, 23);
            this.btnNew.TabIndex = 26;
            this.btnNew.Text = "&New";
            this.btnNew.UseVisualStyleBackColor = true;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(181, 15);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(108, 17);
            this.checkBox2.TabIndex = 25;
            this.checkBox2.Text = "Print while saving";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // chkDoNotClose
            // 
            this.chkDoNotClose.AutoSize = true;
            this.chkDoNotClose.Checked = true;
            this.chkDoNotClose.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDoNotClose.Location = new System.Drawing.Point(36, 15);
            this.chkDoNotClose.Name = "chkDoNotClose";
            this.chkDoNotClose.Size = new System.Drawing.Size(86, 17);
            this.chkDoNotClose.TabIndex = 24;
            this.chkDoNotClose.Text = "Do not close";
            this.chkDoNotClose.UseVisualStyleBackColor = true;
            // 
            // txtAddress
            // 
            this.txtAddress.BackColor = System.Drawing.Color.FloralWhite;
            this.txtAddress.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtAddress.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtAddress.FocusLostColor = System.Drawing.Color.White;
            this.txtAddress.Location = new System.Drawing.Point(118, 119);
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.Size = new System.Drawing.Size(218, 20);
            this.txtAddress.TabIndex = 225;
            // 
            // btnExport
            // 
            this.btnExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExport.Enabled = false;
            this.btnExport.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnExport.Location = new System.Drawing.Point(1178, 3);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(62, 23);
            this.btnExport.TabIndex = 27;
            this.btnExport.Text = "&Export";
            this.btnExport.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnExport.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.CadetBlue;
            this.panel1.Controls.Add(this.btnList);
            this.panel1.Controls.Add(this.btnSetup);
            this.panel1.Controls.Add(this.lblPostStatus);
            this.panel1.Controls.Add(this.chkRecurring);
            this.panel1.Controls.Add(this.btnPrintPreview);
            this.panel1.Controls.Add(this.btnExport);
            this.panel1.Controls.Add(this.btnPaste);
            this.panel1.Controls.Add(this.btnLast);
            this.panel1.Controls.Add(this.btnFirst);
            this.panel1.Controls.Add(this.btnCopy);
            this.panel1.Controls.Add(this.btnNext);
            this.panel1.Controls.Add(this.btnPrev);
            this.panel1.Controls.Add(this.btnExit);
            this.panel1.Controls.Add(this.btnPrint);
            this.panel1.Location = new System.Drawing.Point(-2, 7);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1111, 31);
            this.panel1.TabIndex = 237;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(9, 95);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(71, 13);
            this.label9.TabIndex = 235;
            this.label9.Text = "Patient Name";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.ForeColor = System.Drawing.Color.DarkBlue;
            this.label17.Location = new System.Drawing.Point(964, 115);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(0, 15);
            this.label17.TabIndex = 255;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.DarkBlue;
            this.label1.Location = new System.Drawing.Point(761, 115);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 15);
            this.label1.TabIndex = 254;
            // 
            // txtRegDate
            // 
            this.txtRegDate.BackColor = System.Drawing.Color.White;
            this.txtRegDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtRegDate.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.txtRegDate.FocusLostColor = System.Drawing.Color.White;
            this.txtRegDate.Location = new System.Drawing.Point(867, 60);
            this.txtRegDate.Mask = "0000/00/00";
            this.txtRegDate.Name = "txtRegDate";
            this.txtRegDate.Size = new System.Drawing.Size(182, 20);
            this.txtRegDate.TabIndex = 250;
            this.txtRegDate.ValidatingType = typeof(System.DateTime);
            // 
            // label81
            // 
            this.label81.AutoSize = true;
            this.label81.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label81.ForeColor = System.Drawing.Color.DarkBlue;
            this.label81.Location = new System.Drawing.Point(757, 60);
            this.label81.Name = "label81";
            this.label81.Size = new System.Drawing.Size(0, 15);
            this.label81.TabIndex = 252;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.DarkBlue;
            this.label5.Location = new System.Drawing.Point(756, 91);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(0, 15);
            this.label5.TabIndex = 251;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 126);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 13);
            this.label3.TabIndex = 259;
            this.label3.Text = "Address";
            // 
            // txtRemarks
            // 
            this.txtRemarks.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.txtRemarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtRemarks.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtRemarks.FocusLostColor = System.Drawing.Color.White;
            this.txtRemarks.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.txtRemarks.Location = new System.Drawing.Point(760, 114);
            this.txtRemarks.Multiline = true;
            this.txtRemarks.Name = "txtRemarks";
            this.txtRemarks.Size = new System.Drawing.Size(321, 29);
            this.txtRemarks.TabIndex = 261;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(755, 93);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(98, 13);
            this.label4.TabIndex = 260;
            this.label4.Text = "*Reason\'s For Visit:";
            // 
            // txtMobile
            // 
            this.txtMobile.BackColor = System.Drawing.Color.FloralWhite;
            this.txtMobile.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtMobile.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtMobile.FocusLostColor = System.Drawing.Color.White;
            this.txtMobile.Location = new System.Drawing.Point(460, 88);
            this.txtMobile.Name = "txtMobile";
            this.txtMobile.Size = new System.Drawing.Size(207, 20);
            this.txtMobile.TabIndex = 243;
            // 
            // btnStartDate
            // 
            this.btnStartDate.Image = ((System.Drawing.Image)(resources.GetObject("btnStartDate.Image")));
            this.btnStartDate.Location = new System.Drawing.Point(1055, 58);
            this.btnStartDate.Name = "btnStartDate";
            this.btnStartDate.Size = new System.Drawing.Size(26, 23);
            this.btnStartDate.TabIndex = 263;
            this.btnStartDate.UseVisualStyleBackColor = true;
            this.btnStartDate.Click += new System.EventHandler(this.btnRegistrationDate_Click);
            // 
            // txtGender
            // 
            this.txtGender.BackColor = System.Drawing.Color.FloralWhite;
            this.txtGender.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtGender.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtGender.FocusLostColor = System.Drawing.Color.White;
            this.txtGender.Location = new System.Drawing.Point(476, 119);
            this.txtGender.Name = "txtGender";
            this.txtGender.Size = new System.Drawing.Size(91, 20);
            this.txtGender.TabIndex = 265;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(573, 121);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(26, 13);
            this.label10.TabIndex = 269;
            this.label10.Text = "Age";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(365, 91);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(58, 13);
            this.label11.TabIndex = 270;
            this.label11.Text = "Mobile No:";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(442, 121);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(28, 13);
            this.label18.TabIndex = 274;
            this.label18.Text = "Sex:";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(757, 62);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(89, 13);
            this.label13.TabIndex = 275;
            this.label13.Text = "Registration Date";
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tbSalarySheet);
            this.tabControl1.Location = new System.Drawing.Point(10, 186);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1088, 130);
            this.tabControl1.TabIndex = 276;
            // 
            // tbSalarySheet
            // 
            this.tbSalarySheet.Controls.Add(this.grdSalarySheet);
            this.tbSalarySheet.Location = new System.Drawing.Point(4, 22);
            this.tbSalarySheet.Name = "tbSalarySheet";
            this.tbSalarySheet.Padding = new System.Windows.Forms.Padding(3);
            this.tbSalarySheet.Size = new System.Drawing.Size(1080, 104);
            this.tbSalarySheet.TabIndex = 0;
            this.tbSalarySheet.Text = "Detail";
            this.tbSalarySheet.UseVisualStyleBackColor = true;
            // 
            // grdSalarySheet
            // 
            this.grdSalarySheet.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdSalarySheet.EnableSort = true;
            this.grdSalarySheet.Location = new System.Drawing.Point(6, 6);
            this.grdSalarySheet.Name = "grdSalarySheet";
            this.grdSalarySheet.OptimizeMode = SourceGrid.CellOptimizeMode.ForRows;
            this.grdSalarySheet.SelectionMode = SourceGrid.GridSelectionMode.Cell;
            this.grdSalarySheet.Size = new System.Drawing.Size(1061, 92);
            this.grdSalarySheet.TabIndex = 223;
            this.grdSalarySheet.TabStop = true;
            this.grdSalarySheet.ToolTipText = "";
            // 
            // lblNetAmout
            // 
            this.lblNetAmout.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblNetAmout.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNetAmout.Location = new System.Drawing.Point(891, 340);
            this.lblNetAmout.Name = "lblNetAmout";
            this.lblNetAmout.Size = new System.Drawing.Size(59, 13);
            this.lblNetAmout.TabIndex = 222;
            this.lblNetAmout.Text = "0.00";
            this.lblNetAmout.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Location = new System.Drawing.Point(753, 329);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox2.Size = new System.Drawing.Size(194, 6);
            this.groupBox2.TabIndex = 286;
            this.groupBox2.TabStop = false;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(749, 338);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(136, 17);
            this.label2.TabIndex = 282;
            this.label2.Text = "Grand Total (Rs):";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnTenderAmount
            // 
            this.btnTenderAmount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTenderAmount.Location = new System.Drawing.Point(958, 322);
            this.btnTenderAmount.Name = "btnTenderAmount";
            this.btnTenderAmount.Size = new System.Drawing.Size(142, 48);
            this.btnTenderAmount.TabIndex = 281;
            this.btnTenderAmount.Text = "Tender Amount";
            this.btnTenderAmount.UseVisualStyleBackColor = true;
            this.btnTenderAmount.Click += new System.EventHandler(this.btnTenderAmount_Click);
            // 
            // txtAge
            // 
            this.txtAge.BackColor = System.Drawing.Color.FloralWhite;
            this.txtAge.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtAge.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtAge.FocusLostColor = System.Drawing.Color.White;
            this.txtAge.Location = new System.Drawing.Point(605, 119);
            this.txtAge.Name = "txtAge";
            this.txtAge.Size = new System.Drawing.Size(62, 20);
            this.txtAge.TabIndex = 287;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(362, 63);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(78, 13);
            this.label6.TabIndex = 289;
            this.label6.Text = "Telephone No:";
            // 
            // txtTelephone
            // 
            this.txtTelephone.BackColor = System.Drawing.Color.FloralWhite;
            this.txtTelephone.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTelephone.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtTelephone.FocusLostColor = System.Drawing.Color.White;
            this.txtTelephone.Location = new System.Drawing.Point(460, 56);
            this.txtTelephone.Name = "txtTelephone";
            this.txtTelephone.Size = new System.Drawing.Size(207, 20);
            this.txtTelephone.TabIndex = 288;
            // 
            // cboName
            // 
            this.cboName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboName.BackColor = System.Drawing.Color.FloralWhite;
            this.cboName.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.cboName.FocusLostColor = System.Drawing.Color.White;
            this.cboName.FormattingEnabled = true;
            this.cboName.Location = new System.Drawing.Point(118, 90);
            this.cboName.Name = "cboName";
            this.cboName.Size = new System.Drawing.Size(218, 21);
            this.cboName.TabIndex = 4;
            this.cboName.SelectedIndexChanged += new System.EventHandler(this.cboName_SelectedIndexChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(14, 157);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(24, 13);
            this.label8.TabIndex = 290;
            this.label8.Text = "City";
            // 
            // txtCity
            // 
            this.txtCity.BackColor = System.Drawing.Color.FloralWhite;
            this.txtCity.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCity.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtCity.FocusLostColor = System.Drawing.Color.White;
            this.txtCity.Location = new System.Drawing.Point(118, 150);
            this.txtCity.Name = "txtCity";
            this.txtCity.Size = new System.Drawing.Size(218, 20);
            this.txtCity.TabIndex = 291;
            // 
            // chkRecurring
            // 
            this.chkRecurring.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkRecurring.AutoSize = true;
            this.chkRecurring.ForeColor = System.Drawing.Color.White;
            this.chkRecurring.Location = new System.Drawing.Point(928, 6);
            this.chkRecurring.Margin = new System.Windows.Forms.Padding(2);
            this.chkRecurring.Name = "chkRecurring";
            this.chkRecurring.Size = new System.Drawing.Size(72, 17);
            this.chkRecurring.TabIndex = 30;
            this.chkRecurring.Text = "Recurrin&g";
            this.chkRecurring.UseVisualStyleBackColor = true;
            // 
            // lblPostStatus
            // 
            this.lblPostStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPostStatus.AutoSize = true;
            this.lblPostStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPostStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.lblPostStatus.Location = new System.Drawing.Point(796, 2);
            this.lblPostStatus.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblPostStatus.Name = "lblPostStatus";
            this.lblPostStatus.Size = new System.Drawing.Size(80, 20);
            this.lblPostStatus.TabIndex = 31;
            this.lblPostStatus.Text = "POSTED";
            this.lblPostStatus.Click += new System.EventHandler(this.lblPostStatus_Click_1);
            // 
            // btnSetup
            // 
            this.btnSetup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSetup.Location = new System.Drawing.Point(1008, 3);
            this.btnSetup.Name = "btnSetup";
            this.btnSetup.Size = new System.Drawing.Size(43, 23);
            this.btnSetup.TabIndex = 33;
            this.btnSetup.Text = "setup";
            this.btnSetup.UseVisualStyleBackColor = true;
            // 
            // btnPrint
            // 
            this.btnPrint.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPrint.Location = new System.Drawing.Point(432, 3);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(58, 23);
            this.btnPrint.TabIndex = 2;
            this.btnPrint.Text = "Print";
            this.btnPrint.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnPrint.UseVisualStyleBackColor = true;
            // 
            // btnExit
            // 
            this.btnExit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnExit.Location = new System.Drawing.Point(592, 3);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(58, 23);
            this.btnExit.TabIndex = 22;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = true;
            // 
            // btnPrev
            // 
            this.btnPrev.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPrev.Location = new System.Drawing.Point(65, 3);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(58, 23);
            this.btnPrev.TabIndex = 0;
            this.btnPrev.Text = "&Prev";
            this.btnPrev.UseVisualStyleBackColor = true;
            // 
            // btnNext
            // 
            this.btnNext.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnNext.Location = new System.Drawing.Point(127, 3);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(58, 23);
            this.btnNext.TabIndex = 1;
            this.btnNext.Text = "&Next";
            this.btnNext.UseVisualStyleBackColor = true;
            // 
            // btnCopy
            // 
            this.btnCopy.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCopy.Location = new System.Drawing.Point(308, 3);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(58, 23);
            this.btnCopy.TabIndex = 23;
            this.btnCopy.Text = "Copy";
            this.btnCopy.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCopy.UseVisualStyleBackColor = true;
            // 
            // btnFirst
            // 
            this.btnFirst.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnFirst.Location = new System.Drawing.Point(3, 3);
            this.btnFirst.Name = "btnFirst";
            this.btnFirst.Size = new System.Drawing.Size(58, 23);
            this.btnFirst.TabIndex = 3;
            this.btnFirst.Text = "First";
            this.btnFirst.UseVisualStyleBackColor = true;
            this.btnFirst.Click += new System.EventHandler(this.btnFirst_Click);
            // 
            // btnLast
            // 
            this.btnLast.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnLast.Location = new System.Drawing.Point(189, 3);
            this.btnLast.Name = "btnLast";
            this.btnLast.Size = new System.Drawing.Size(58, 23);
            this.btnLast.TabIndex = 4;
            this.btnLast.Text = "Last";
            this.btnLast.UseVisualStyleBackColor = true;
            // 
            // btnPaste
            // 
            this.btnPaste.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPaste.Location = new System.Drawing.Point(370, 3);
            this.btnPaste.Name = "btnPaste";
            this.btnPaste.Size = new System.Drawing.Size(58, 23);
            this.btnPaste.TabIndex = 24;
            this.btnPaste.Text = "Paste";
            this.btnPaste.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnPaste.UseVisualStyleBackColor = true;
            // 
            // btnPrintPreview
            // 
            this.btnPrintPreview.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPrintPreview.Location = new System.Drawing.Point(494, 3);
            this.btnPrintPreview.Name = "btnPrintPreview";
            this.btnPrintPreview.Size = new System.Drawing.Size(94, 23);
            this.btnPrintPreview.TabIndex = 29;
            this.btnPrintPreview.Text = "Pr&int Preview";
            this.btnPrintPreview.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnPrintPreview.UseVisualStyleBackColor = true;
            // 
            // btnList
            // 
            this.btnList.Location = new System.Drawing.Point(251, 3);
            this.btnList.Name = "btnList";
            this.btnList.Size = new System.Drawing.Size(53, 23);
            this.btnList.TabIndex = 34;
            this.btnList.Text = "≡   List";
            this.btnList.UseVisualStyleBackColor = true;
            // 
            // FrmRecipt
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1107, 415);
            this.Controls.Add(this.txtCity);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.cboName);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtTelephone);
            this.Controls.Add(this.txtAge);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.lblNetAmout);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnTenderAmount);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.txtGender);
            this.Controls.Add(this.btnStartDate);
            this.Controls.Add(this.txtRemarks);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtRegDate);
            this.Controls.Add(this.label81);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtRegNo);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.txtMobile);
            this.Controls.Add(this.txtAddress);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label9);
            this.Name = "FrmRecipt";
            this.Text = " FrmReceipt";
            this.Load += new System.EventHandler(this.FrmPatientsDoctor_Load);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tbSalarySheet.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtRegNo;
        private System.Drawing.Printing.PrintDocument printDocument1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Timer timerProductPIN;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.CheckBox chkDoNotClose;
        private SComponents.STextBox txtAddress;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label1;
        private SComponents.SMaskedTextBox txtRegDate;
        private System.Windows.Forms.Label label81;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
        private SComponents.STextBox txtRemarks;
        private System.Windows.Forms.Label label4;
        private SComponents.STextBox txtMobile;
        private System.Windows.Forms.Button btnStartDate;
        private SComponents.STextBox txtGender;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tbSalarySheet;
        private System.Windows.Forms.Label lblNetAmout;
        private SourceGrid.Grid grdSalarySheet;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnTenderAmount;
        private System.Windows.Forms.Button btnRCancel;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnNew;
        private SComponents.STextBox txtAge;
        private System.Windows.Forms.Label label6;
        private SComponents.STextBox txtTelephone;
        private SComponents.SComboBox cboName;
        private System.Windows.Forms.Label label8;
        private SComponents.STextBox txtCity;
        private System.Windows.Forms.Button btnList;
        private System.Windows.Forms.Button btnSetup;
        private System.Windows.Forms.Label lblPostStatus;
        private System.Windows.Forms.CheckBox chkRecurring;
        private System.Windows.Forms.Button btnPrintPreview;
        private System.Windows.Forms.Button btnPaste;
        private System.Windows.Forms.Button btnLast;
        private System.Windows.Forms.Button btnFirst;
        private System.Windows.Forms.Button btnCopy;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnPrev;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnPrint;
    }
}