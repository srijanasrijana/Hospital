using SComponents;
namespace AccSwift
{
    partial class frmNewCompany
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmNewCompany));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabCompanyInfo = new System.Windows.Forms.TabPage();
            this.txtCompanyCode = new SComponents.STextBox();
            this.label21 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.label20 = new System.Windows.Forms.Label();
            this.txtDBName = new SComponents.STextBox();
            this.txtDateBookBegin = new SComponents.SMaskedTextBox();
            this.txtDateFY = new SComponents.SMaskedTextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label123 = new System.Windows.Forms.Label();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.picLogo = new System.Windows.Forms.PictureBox();
            this.txtPan = new SComponents.STextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txtPOBox = new SComponents.STextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtWebsite = new SComponents.STextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtEmail = new SComponents.STextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtTel = new SComponents.STextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtZone = new SComponents.STextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtDistrict = new SComponents.STextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtCity = new SComponents.STextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtAddress2 = new SComponents.STextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtAddress1 = new SComponents.STextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtCompanyName = new SComponents.STextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabSettings = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.rdbDecimalFormatInNegative = new System.Windows.Forms.RadioButton();
            this.rdbDecimalFormatInBracket = new System.Windows.Forms.RadioButton();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.chkCommaSeparated = new System.Windows.Forms.CheckBox();
            this.cboDecimalPlaces = new SComponents.SComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label15 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.cboDateFormat = new SComponents.SComboBox();
            this.label17 = new System.Windows.Forms.Label();
            this.rbDateNepali = new System.Windows.Forms.RadioButton();
            this.rbDateEnglish = new System.Windows.Forms.RadioButton();
            this.label18 = new System.Windows.Forms.Label();
            this.rbLangNepali = new System.Windows.Forms.RadioButton();
            this.label19 = new System.Windows.Forms.Label();
            this.rbLangEnglish = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnCreate = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabCompanyInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).BeginInit();
            this.tabSettings.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabCompanyInfo);
            this.tabControl1.Controls.Add(this.tabSettings);
            this.tabControl1.Location = new System.Drawing.Point(31, 33);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(745, 362);
            this.tabControl1.TabIndex = 45;
            // 
            // tabCompanyInfo
            // 
            this.tabCompanyInfo.Controls.Add(this.txtCompanyCode);
            this.tabCompanyInfo.Controls.Add(this.label21);
            this.tabCompanyInfo.Controls.Add(this.button2);
            this.tabCompanyInfo.Controls.Add(this.button1);
            this.tabCompanyInfo.Controls.Add(this.radioButton1);
            this.tabCompanyInfo.Controls.Add(this.radioButton2);
            this.tabCompanyInfo.Controls.Add(this.label20);
            this.tabCompanyInfo.Controls.Add(this.txtDBName);
            this.tabCompanyInfo.Controls.Add(this.txtDateBookBegin);
            this.tabCompanyInfo.Controls.Add(this.txtDateFY);
            this.tabCompanyInfo.Controls.Add(this.label12);
            this.tabCompanyInfo.Controls.Add(this.label14);
            this.tabCompanyInfo.Controls.Add(this.label123);
            this.tabCompanyInfo.Controls.Add(this.btnBrowse);
            this.tabCompanyInfo.Controls.Add(this.picLogo);
            this.tabCompanyInfo.Controls.Add(this.txtPan);
            this.tabCompanyInfo.Controls.Add(this.label11);
            this.tabCompanyInfo.Controls.Add(this.txtPOBox);
            this.tabCompanyInfo.Controls.Add(this.label10);
            this.tabCompanyInfo.Controls.Add(this.txtWebsite);
            this.tabCompanyInfo.Controls.Add(this.label9);
            this.tabCompanyInfo.Controls.Add(this.txtEmail);
            this.tabCompanyInfo.Controls.Add(this.label8);
            this.tabCompanyInfo.Controls.Add(this.txtTel);
            this.tabCompanyInfo.Controls.Add(this.label7);
            this.tabCompanyInfo.Controls.Add(this.txtZone);
            this.tabCompanyInfo.Controls.Add(this.label6);
            this.tabCompanyInfo.Controls.Add(this.txtDistrict);
            this.tabCompanyInfo.Controls.Add(this.label5);
            this.tabCompanyInfo.Controls.Add(this.txtCity);
            this.tabCompanyInfo.Controls.Add(this.label4);
            this.tabCompanyInfo.Controls.Add(this.txtAddress2);
            this.tabCompanyInfo.Controls.Add(this.label3);
            this.tabCompanyInfo.Controls.Add(this.txtAddress1);
            this.tabCompanyInfo.Controls.Add(this.label2);
            this.tabCompanyInfo.Controls.Add(this.txtCompanyName);
            this.tabCompanyInfo.Controls.Add(this.label1);
            this.tabCompanyInfo.Location = new System.Drawing.Point(4, 22);
            this.tabCompanyInfo.Name = "tabCompanyInfo";
            this.tabCompanyInfo.Padding = new System.Windows.Forms.Padding(3);
            this.tabCompanyInfo.Size = new System.Drawing.Size(737, 336);
            this.tabCompanyInfo.TabIndex = 0;
            this.tabCompanyInfo.Text = "General Info";
            this.tabCompanyInfo.UseVisualStyleBackColor = true;
            this.tabCompanyInfo.Click += new System.EventHandler(this.tabCompanyInfo_Click);
            // 
            // txtCompanyCode
            // 
            this.txtCompanyCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCompanyCode.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtCompanyCode.FocusLostColor = System.Drawing.Color.White;
            this.txtCompanyCode.Location = new System.Drawing.Point(165, 48);
            this.txtCompanyCode.MaxLength = 4;
            this.txtCompanyCode.Name = "txtCompanyCode";
            this.txtCompanyCode.Size = new System.Drawing.Size(100, 20);
            this.txtCompanyCode.TabIndex = 77;
            this.txtCompanyCode.TextChanged += new System.EventHandler(this.txtCompanyCode_TextChanged);
            this.txtCompanyCode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtCompanyCode_KeyDown);
            this.txtCompanyCode.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtCompanyCode_KeyPress);
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(77, 50);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(82, 13);
            this.label21.TabIndex = 76;
            this.label21.Text = "Company Code:";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(246, 391);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(24, 21);
            this.button2.TabIndex = 75;
            this.button2.Text = "...";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(246, 366);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(24, 21);
            this.button1.TabIndex = 74;
            this.button1.Text = "...";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(243, 346);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(55, 17);
            this.radioButton1.TabIndex = 73;
            this.radioButton1.Text = "Nepali";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Checked = true;
            this.radioButton2.Location = new System.Drawing.Point(165, 346);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(59, 17);
            this.radioButton2.TabIndex = 72;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "English";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(83, 348);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(70, 13);
            this.label20.TabIndex = 71;
            this.label20.Text = "Default Date:";
            // 
            // txtDBName
            // 
            this.txtDBName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDBName.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtDBName.FocusLostColor = System.Drawing.Color.White;
            this.txtDBName.Location = new System.Drawing.Point(507, 225);
            this.txtDBName.Name = "txtDBName";
            this.txtDBName.Size = new System.Drawing.Size(100, 20);
            this.txtDBName.TabIndex = 70;
            this.txtDBName.Visible = false;
            // 
            // txtDateBookBegin
            // 
            this.txtDateBookBegin.BackColor = System.Drawing.Color.White;
            this.txtDateBookBegin.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDateBookBegin.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.txtDateBookBegin.FocusLostColor = System.Drawing.Color.White;
            this.txtDateBookBegin.Location = new System.Drawing.Point(165, 392);
            this.txtDateBookBegin.Mask = "0000/00/00";
            this.txtDateBookBegin.Name = "txtDateBookBegin";
            this.txtDateBookBegin.Size = new System.Drawing.Size(75, 20);
            this.txtDateBookBegin.TabIndex = 67;
            // 
            // txtDateFY
            // 
            this.txtDateFY.BackColor = System.Drawing.Color.White;
            this.txtDateFY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDateFY.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.txtDateFY.FocusLostColor = System.Drawing.Color.White;
            this.txtDateFY.Location = new System.Drawing.Point(165, 367);
            this.txtDateFY.Mask = "0000/00/00";
            this.txtDateFY.Name = "txtDateFY";
            this.txtDateFY.Size = new System.Drawing.Size(75, 20);
            this.txtDateFY.TabIndex = 66;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(71, 369);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(88, 13);
            this.label12.TabIndex = 65;
            this.label12.Text = "Fiscal Year From:";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(49, 392);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(110, 13);
            this.label14.TabIndex = 64;
            this.label14.Text = "Books Begining From:";
            // 
            // label123
            // 
            this.label123.AutoSize = true;
            this.label123.Location = new System.Drawing.Point(504, 21);
            this.label123.Name = "label123";
            this.label123.Size = new System.Drawing.Size(81, 13);
            this.label123.TabIndex = 63;
            this.label123.Text = "Company Logo:";
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(601, 188);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnBrowse.TabIndex = 62;
            this.btnBrowse.Text = "&Browse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // picLogo
            // 
            this.picLogo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picLogo.Location = new System.Drawing.Point(507, 37);
            this.picLogo.Name = "picLogo";
            this.picLogo.Size = new System.Drawing.Size(171, 145);
            this.picLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picLogo.TabIndex = 61;
            this.picLogo.TabStop = false;
            // 
            // txtPan
            // 
            this.txtPan.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPan.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtPan.FocusLostColor = System.Drawing.Color.White;
            this.txtPan.Location = new System.Drawing.Point(165, 308);
            this.txtPan.Name = "txtPan";
            this.txtPan.Size = new System.Drawing.Size(296, 20);
            this.txtPan.TabIndex = 60;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(107, 284);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(52, 13);
            this.label11.TabIndex = 59;
            this.label11.Text = "P.O. Box:";
            // 
            // txtPOBox
            // 
            this.txtPOBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPOBox.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtPOBox.FocusLostColor = System.Drawing.Color.White;
            this.txtPOBox.Location = new System.Drawing.Point(165, 282);
            this.txtPOBox.Name = "txtPOBox";
            this.txtPOBox.Size = new System.Drawing.Size(296, 20);
            this.txtPOBox.TabIndex = 58;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(110, 310);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(49, 13);
            this.label10.TabIndex = 57;
            this.label10.Text = "PAN No:";
            // 
            // txtWebsite
            // 
            this.txtWebsite.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtWebsite.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtWebsite.FocusLostColor = System.Drawing.Color.White;
            this.txtWebsite.Location = new System.Drawing.Point(165, 256);
            this.txtWebsite.Name = "txtWebsite";
            this.txtWebsite.Size = new System.Drawing.Size(296, 20);
            this.txtWebsite.TabIndex = 56;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(110, 258);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(49, 13);
            this.label9.TabIndex = 55;
            this.label9.Text = "Website:";
            // 
            // txtEmail
            // 
            this.txtEmail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtEmail.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtEmail.FocusLostColor = System.Drawing.Color.White;
            this.txtEmail.Location = new System.Drawing.Point(165, 230);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(296, 20);
            this.txtEmail.TabIndex = 54;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(124, 232);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(35, 13);
            this.label8.TabIndex = 53;
            this.label8.Text = "Email:";
            // 
            // txtTel
            // 
            this.txtTel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTel.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtTel.FocusLostColor = System.Drawing.Color.White;
            this.txtTel.Location = new System.Drawing.Point(165, 204);
            this.txtTel.Name = "txtTel";
            this.txtTel.Size = new System.Drawing.Size(296, 20);
            this.txtTel.TabIndex = 52;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(134, 204);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(25, 13);
            this.label7.TabIndex = 51;
            this.label7.Text = "Tel:";
            // 
            // txtZone
            // 
            this.txtZone.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtZone.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtZone.FocusLostColor = System.Drawing.Color.White;
            this.txtZone.Location = new System.Drawing.Point(165, 178);
            this.txtZone.Name = "txtZone";
            this.txtZone.Size = new System.Drawing.Size(296, 20);
            this.txtZone.TabIndex = 50;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(132, 128);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(27, 13);
            this.label6.TabIndex = 49;
            this.label6.Text = "City:";
            // 
            // txtDistrict
            // 
            this.txtDistrict.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDistrict.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtDistrict.FocusLostColor = System.Drawing.Color.White;
            this.txtDistrict.Location = new System.Drawing.Point(165, 152);
            this.txtDistrict.Name = "txtDistrict";
            this.txtDistrict.Size = new System.Drawing.Size(296, 20);
            this.txtDistrict.TabIndex = 48;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(117, 154);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(42, 13);
            this.label5.TabIndex = 47;
            this.label5.Text = "District:";
            // 
            // txtCity
            // 
            this.txtCity.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCity.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtCity.FocusLostColor = System.Drawing.Color.White;
            this.txtCity.Location = new System.Drawing.Point(165, 126);
            this.txtCity.Name = "txtCity";
            this.txtCity.Size = new System.Drawing.Size(296, 20);
            this.txtCity.TabIndex = 46;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(124, 180);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 13);
            this.label4.TabIndex = 45;
            this.label4.Text = "Zone:";
            // 
            // txtAddress2
            // 
            this.txtAddress2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtAddress2.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtAddress2.FocusLostColor = System.Drawing.Color.White;
            this.txtAddress2.Location = new System.Drawing.Point(165, 100);
            this.txtAddress2.Name = "txtAddress2";
            this.txtAddress2.Size = new System.Drawing.Size(296, 20);
            this.txtAddress2.TabIndex = 44;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(102, 102);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 13);
            this.label3.TabIndex = 43;
            this.label3.Text = "Address 2:";
            // 
            // txtAddress1
            // 
            this.txtAddress1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtAddress1.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtAddress1.FocusLostColor = System.Drawing.Color.White;
            this.txtAddress1.Location = new System.Drawing.Point(165, 74);
            this.txtAddress1.Name = "txtAddress1";
            this.txtAddress1.Size = new System.Drawing.Size(296, 20);
            this.txtAddress1.TabIndex = 42;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(102, 76);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 13);
            this.label2.TabIndex = 41;
            this.label2.Text = "Address 1:";
            // 
            // txtCompanyName
            // 
            this.txtCompanyName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtCompanyName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCompanyName.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtCompanyName.FocusLostColor = System.Drawing.Color.White;
            this.txtCompanyName.Location = new System.Drawing.Point(165, 22);
            this.txtCompanyName.Name = "txtCompanyName";
            this.txtCompanyName.Size = new System.Drawing.Size(296, 20);
            this.txtCompanyName.TabIndex = 0;
            this.txtCompanyName.Leave += new System.EventHandler(this.txtCompanyName_Leave);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(74, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 13);
            this.label1.TabIndex = 39;
            this.label1.Text = "Company Name:";
            // 
            // tabSettings
            // 
            this.tabSettings.Controls.Add(this.groupBox3);
            this.tabSettings.Controls.Add(this.groupBox5);
            this.tabSettings.Controls.Add(this.groupBox1);
            this.tabSettings.Controls.Add(this.rbLangNepali);
            this.tabSettings.Controls.Add(this.label19);
            this.tabSettings.Controls.Add(this.rbLangEnglish);
            this.tabSettings.Location = new System.Drawing.Point(4, 22);
            this.tabSettings.Name = "tabSettings";
            this.tabSettings.Size = new System.Drawing.Size(737, 336);
            this.tabSettings.TabIndex = 2;
            this.tabSettings.Text = "General Settings";
            this.tabSettings.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.rdbDecimalFormatInNegative);
            this.groupBox3.Controls.Add(this.rdbDecimalFormatInBracket);
            this.groupBox3.Location = new System.Drawing.Point(274, 117);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(154, 91);
            this.groupBox3.TabIndex = 25;
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
            this.rdbDecimalFormatInNegative.Text = "-422.30";
            this.rdbDecimalFormatInNegative.UseVisualStyleBackColor = true;
            // 
            // rdbDecimalFormatInBracket
            // 
            this.rdbDecimalFormatInBracket.AutoSize = true;
            this.rdbDecimalFormatInBracket.Checked = true;
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
            this.groupBox5.Controls.Add(this.cboDecimalPlaces);
            this.groupBox5.Controls.Add(this.label13);
            this.groupBox5.Location = new System.Drawing.Point(50, 117);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(218, 91);
            this.groupBox5.TabIndex = 26;
            this.groupBox5.TabStop = false;
            // 
            // chkCommaSeparated
            // 
            this.chkCommaSeparated.AutoSize = true;
            this.chkCommaSeparated.Checked = true;
            this.chkCommaSeparated.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCommaSeparated.Location = new System.Drawing.Point(15, 57);
            this.chkCommaSeparated.Name = "chkCommaSeparated";
            this.chkCommaSeparated.Size = new System.Drawing.Size(156, 17);
            this.chkCommaSeparated.TabIndex = 18;
            this.chkCommaSeparated.Text = "Comma Separated numbers";
            this.chkCommaSeparated.UseVisualStyleBackColor = true;
            // 
            // cboDecimalPlaces
            // 
            this.cboDecimalPlaces.BackColor = System.Drawing.Color.White;
            this.cboDecimalPlaces.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDecimalPlaces.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.cboDecimalPlaces.FocusLostColor = System.Drawing.Color.White;
            this.cboDecimalPlaces.FormattingEnabled = true;
            this.cboDecimalPlaces.Location = new System.Drawing.Point(151, 19);
            this.cboDecimalPlaces.Name = "cboDecimalPlaces";
            this.cboDecimalPlaces.Size = new System.Drawing.Size(51, 21);
            this.cboDecimalPlaces.TabIndex = 17;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(13, 22);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(132, 13);
            this.label13.TabIndex = 16;
            this.label13.Text = "Number of Decimal Places";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label15);
            this.groupBox1.Controls.Add(this.label16);
            this.groupBox1.Controls.Add(this.cboDateFormat);
            this.groupBox1.Controls.Add(this.label17);
            this.groupBox1.Controls.Add(this.rbDateNepali);
            this.groupBox1.Controls.Add(this.rbDateEnglish);
            this.groupBox1.Controls.Add(this.label18);
            this.groupBox1.Location = new System.Drawing.Point(50, 17);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(378, 94);
            this.groupBox1.TabIndex = 21;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Date";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label15.Location = new System.Drawing.Point(260, 56);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(67, 15);
            this.label15.TabIndex = 6;
            this.label15.Text = "2011/12/28";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(209, 56);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(45, 13);
            this.label16.TabIndex = 5;
            this.label16.Text = "Sample:";
            // 
            // cboDateFormat
            // 
            this.cboDateFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDateFormat.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.cboDateFormat.FocusLostColor = System.Drawing.Color.White;
            this.cboDateFormat.FormattingEnabled = true;
            this.cboDateFormat.Items.AddRange(new object[] {
            "YYYY/MM/DD",
            "DD/MM/YYYY",
            "MM/DD/YYYY"});
            this.cboDateFormat.Location = new System.Drawing.Point(74, 55);
            this.cboDateFormat.Name = "cboDateFormat";
            this.cboDateFormat.Size = new System.Drawing.Size(108, 21);
            this.cboDateFormat.TabIndex = 4;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(20, 58);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(42, 13);
            this.label17.TabIndex = 3;
            this.label17.Text = "Format:";
            // 
            // rbDateNepali
            // 
            this.rbDateNepali.AutoSize = true;
            this.rbDateNepali.Checked = true;
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
            this.rbDateEnglish.Text = "English";
            this.rbDateEnglish.UseVisualStyleBackColor = true;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(16, 25);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(70, 13);
            this.label18.TabIndex = 0;
            this.label18.Text = "Default Date:";
            // 
            // rbLangNepali
            // 
            this.rbLangNepali.AutoSize = true;
            this.rbLangNepali.Checked = true;
            this.rbLangNepali.Location = new System.Drawing.Point(213, 228);
            this.rbLangNepali.Name = "rbLangNepali";
            this.rbLangNepali.Size = new System.Drawing.Size(55, 17);
            this.rbLangNepali.TabIndex = 24;
            this.rbLangNepali.TabStop = true;
            this.rbLangNepali.Text = "Nepali";
            this.rbLangNepali.UseVisualStyleBackColor = true;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(50, 230);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(95, 13);
            this.label19.TabIndex = 22;
            this.label19.Text = "Default Language:";
            // 
            // rbLangEnglish
            // 
            this.rbLangEnglish.AutoSize = true;
            this.rbLangEnglish.Location = new System.Drawing.Point(148, 228);
            this.rbLangEnglish.Name = "rbLangEnglish";
            this.rbLangEnglish.Size = new System.Drawing.Size(59, 17);
            this.rbLangEnglish.TabIndex = 23;
            this.rbLangEnglish.Text = "English";
            this.rbLangEnglish.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnCreate);
            this.groupBox2.Controls.Add(this.btnCancel);
            this.groupBox2.Location = new System.Drawing.Point(35, 401);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(741, 47);
            this.groupBox2.TabIndex = 46;
            this.groupBox2.TabStop = false;
            this.groupBox2.Enter += new System.EventHandler(this.groupBox2_Enter);
            // 
            // btnCreate
            // 
            this.btnCreate.Image = global::Inventory.Properties.Resources.save;
            this.btnCreate.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCreate.Location = new System.Drawing.Point(527, 15);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(75, 23);
            this.btnCreate.TabIndex = 24;
            this.btnCreate.Text = "&Create";
            this.btnCreate.UseVisualStyleBackColor = true;
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Image = global::Inventory.Properties.Resources.gnome_window_close;
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(621, 15);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 25;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // frmNewCompany
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 456);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.groupBox2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "frmNewCompany";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Create  Company";
            this.Load += new System.EventHandler(this.frmNewCompany_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmNewCompany_KeyDown);
            this.tabControl1.ResumeLayout(false);
            this.tabCompanyInfo.ResumeLayout(false);
            this.tabCompanyInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).EndInit();
            this.tabSettings.ResumeLayout(false);
            this.tabSettings.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabCompanyInfo;
        private System.Windows.Forms.TabPage tabSettings;
        private STextBox txtDBName;
        private System.Windows.Forms.Label label123;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.PictureBox picLogo;
        private STextBox txtPan;
        private System.Windows.Forms.Label label11;
        private STextBox txtPOBox;
        private System.Windows.Forms.Label label10;
        private STextBox txtWebsite;
        private System.Windows.Forms.Label label9;
        private STextBox txtEmail;
        private System.Windows.Forms.Label label8;
        private STextBox txtTel;
        private System.Windows.Forms.Label label7;
        private STextBox txtZone;
        private System.Windows.Forms.Label label6;
        private STextBox txtDistrict;
        private System.Windows.Forms.Label label5;
        private STextBox txtCity;
        private System.Windows.Forms.Label label4;
        private STextBox txtAddress2;
        private System.Windows.Forms.Label label3;
        private STextBox txtAddress1;
        private System.Windows.Forms.Label label2;
        private STextBox txtCompanyName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnCreate;
        private System.Windows.Forms.Button btnCancel;
        private STextBox txtCompanyCode;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.Label label20;
        private SMaskedTextBox txtDateBookBegin;
        private SMaskedTextBox txtDateFY;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton rdbDecimalFormatInNegative;
        private System.Windows.Forms.RadioButton rdbDecimalFormatInBracket;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.CheckBox chkCommaSeparated;
        private SComboBox cboDecimalPlaces;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label16;
        private SComboBox cboDateFormat;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.RadioButton rbDateNepali;
        private System.Windows.Forms.RadioButton rbDateEnglish;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.RadioButton rbLangNepali;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.RadioButton rbLangEnglish;
    }
}