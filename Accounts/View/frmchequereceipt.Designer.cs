using SComponents;
namespace Accounts
{
    partial class frmchequereceipt
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmchequereceipt));
            this.tcchequereceipt = new System.Windows.Forms.TabControl();
            this.tpdetails = new System.Windows.Forms.TabPage();
            this.grdchequereceipt = new SourceGrid.Grid();
            this.tpaccountingclass = new System.Windows.Forms.TabPage();
            this.btnAddAccClass = new System.Windows.Forms.Button();
            this.treeAccClass = new SComponents.STreeView(this.components);
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.txtfifth = new SComponents.STextBox();
            this.txtfourth = new SComponents.STextBox();
            this.txtthird = new SComponents.STextBox();
            this.txtsecond = new SComponents.STextBox();
            this.txtfirst = new SComponents.STextBox();
            this.lblfourth = new System.Windows.Forms.Label();
            this.lblsecond = new System.Windows.Forms.Label();
            this.lblthird = new System.Windows.Forms.Label();
            this.lblfifth = new System.Windows.Forms.Label();
            this.lblfirst = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.txtchequereceiptid = new SComponents.STextBox();
            this.btnDate = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.cboProjectName = new SComponents.SComboBox();
            this.txtVchNo = new SComponents.STextBox();
            this.txtBankID = new SComponents.STextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtDate = new SComponents.SMaskedTextBox();
            this.cboSeriesName = new SComponents.SComboBox();
            this.lblVouNo = new System.Windows.Forms.Label();
            this.comboBankAccount = new SComponents.SComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnPrintPreview = new System.Windows.Forms.Button();
            this.btnPaste = new System.Windows.Forms.Button();
            this.btnCopy = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnLast = new System.Windows.Forms.Button();
            this.btnFirst = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnPrev = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnNew = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.chkDoNotClose = new System.Windows.Forms.CheckBox();
            this.chkPrntWhileSaving = new System.Windows.Forms.CheckBox();
            this.btnDelete = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txtRemarks = new SComponents.STextBox();
            this.tcchequereceipt.SuspendLayout();
            this.tpdetails.SuspendLayout();
            this.tpaccountingclass.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tcchequereceipt
            // 
            this.tcchequereceipt.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tcchequereceipt.Controls.Add(this.tpdetails);
            this.tcchequereceipt.Controls.Add(this.tpaccountingclass);
            this.tcchequereceipt.Controls.Add(this.tabPage1);
            this.tcchequereceipt.Location = new System.Drawing.Point(-2, 131);
            this.tcchequereceipt.Name = "tcchequereceipt";
            this.tcchequereceipt.SelectedIndex = 0;
            this.tcchequereceipt.Size = new System.Drawing.Size(808, 277);
            this.tcchequereceipt.TabIndex = 0;
            // 
            // tpdetails
            // 
            this.tpdetails.Controls.Add(this.grdchequereceipt);
            this.tpdetails.Location = new System.Drawing.Point(4, 22);
            this.tpdetails.Name = "tpdetails";
            this.tpdetails.Padding = new System.Windows.Forms.Padding(3);
            this.tpdetails.Size = new System.Drawing.Size(800, 251);
            this.tpdetails.TabIndex = 0;
            this.tpdetails.Text = "Details";
            this.tpdetails.UseVisualStyleBackColor = true;
            // 
            // grdchequereceipt
            // 
            this.grdchequereceipt.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdchequereceipt.EnableSort = true;
            this.grdchequereceipt.Location = new System.Drawing.Point(4, 6);
            this.grdchequereceipt.Name = "grdchequereceipt";
            this.grdchequereceipt.OptimizeMode = SourceGrid.CellOptimizeMode.ForRows;
            this.grdchequereceipt.SelectionMode = SourceGrid.GridSelectionMode.Cell;
            this.grdchequereceipt.Size = new System.Drawing.Size(794, 240);
            this.grdchequereceipt.TabIndex = 2;
            this.grdchequereceipt.TabStop = true;
            this.grdchequereceipt.ToolTipText = "";
            // 
            // tpaccountingclass
            // 
            this.tpaccountingclass.Controls.Add(this.btnAddAccClass);
            this.tpaccountingclass.Controls.Add(this.treeAccClass);
            this.tpaccountingclass.Location = new System.Drawing.Point(4, 22);
            this.tpaccountingclass.Name = "tpaccountingclass";
            this.tpaccountingclass.Padding = new System.Windows.Forms.Padding(3);
            this.tpaccountingclass.Size = new System.Drawing.Size(800, 251);
            this.tpaccountingclass.TabIndex = 1;
            this.tpaccountingclass.Text = "AccountingClass";
            this.tpaccountingclass.UseVisualStyleBackColor = true;
            // 
            // btnAddAccClass
            // 
            this.btnAddAccClass.Location = new System.Drawing.Point(402, 6);
            this.btnAddAccClass.Name = "btnAddAccClass";
            this.btnAddAccClass.Size = new System.Drawing.Size(109, 45);
            this.btnAddAccClass.TabIndex = 32;
            this.btnAddAccClass.Text = "Add Account Class";
            this.btnAddAccClass.UseVisualStyleBackColor = true;
            this.btnAddAccClass.Click += new System.EventHandler(this.btnAddAccClass_Click);
            // 
            // treeAccClass
            // 
            this.treeAccClass.AutoCheckChild = true;
            this.treeAccClass.CheckBoxes = true;
            this.treeAccClass.Location = new System.Drawing.Point(5, 5);
            this.treeAccClass.Name = "treeAccClass";
            this.treeAccClass.Size = new System.Drawing.Size(391, 243);
            this.treeAccClass.TabIndex = 30;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.txtfifth);
            this.tabPage1.Controls.Add(this.txtfourth);
            this.tabPage1.Controls.Add(this.txtthird);
            this.tabPage1.Controls.Add(this.txtsecond);
            this.tabPage1.Controls.Add(this.txtfirst);
            this.tabPage1.Controls.Add(this.lblfourth);
            this.tabPage1.Controls.Add(this.lblsecond);
            this.tabPage1.Controls.Add(this.lblthird);
            this.tabPage1.Controls.Add(this.lblfifth);
            this.tabPage1.Controls.Add(this.lblfirst);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(800, 251);
            this.tabPage1.TabIndex = 2;
            this.tabPage1.Text = "Additional Fields";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // txtfifth
            // 
            this.txtfifth.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtfifth.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtfifth.FocusLostColor = System.Drawing.Color.White;
            this.txtfifth.Location = new System.Drawing.Point(110, 165);
            this.txtfifth.Name = "txtfifth";
            this.txtfifth.Size = new System.Drawing.Size(182, 20);
            this.txtfifth.TabIndex = 40;
            this.txtfifth.Visible = false;
            // 
            // txtfourth
            // 
            this.txtfourth.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtfourth.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtfourth.FocusLostColor = System.Drawing.Color.White;
            this.txtfourth.Location = new System.Drawing.Point(110, 126);
            this.txtfourth.Name = "txtfourth";
            this.txtfourth.Size = new System.Drawing.Size(182, 20);
            this.txtfourth.TabIndex = 39;
            this.txtfourth.Visible = false;
            // 
            // txtthird
            // 
            this.txtthird.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtthird.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtthird.FocusLostColor = System.Drawing.Color.White;
            this.txtthird.Location = new System.Drawing.Point(110, 88);
            this.txtthird.Name = "txtthird";
            this.txtthird.Size = new System.Drawing.Size(182, 20);
            this.txtthird.TabIndex = 38;
            this.txtthird.Visible = false;
            // 
            // txtsecond
            // 
            this.txtsecond.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtsecond.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtsecond.FocusLostColor = System.Drawing.Color.White;
            this.txtsecond.Location = new System.Drawing.Point(110, 48);
            this.txtsecond.Name = "txtsecond";
            this.txtsecond.Size = new System.Drawing.Size(182, 20);
            this.txtsecond.TabIndex = 37;
            this.txtsecond.Visible = false;
            // 
            // txtfirst
            // 
            this.txtfirst.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtfirst.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtfirst.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtfirst.FocusLostColor = System.Drawing.Color.White;
            this.txtfirst.Location = new System.Drawing.Point(110, 14);
            this.txtfirst.Name = "txtfirst";
            this.txtfirst.Size = new System.Drawing.Size(182, 20);
            this.txtfirst.TabIndex = 36;
            this.txtfirst.Visible = false;
            // 
            // lblfourth
            // 
            this.lblfourth.AutoSize = true;
            this.lblfourth.Location = new System.Drawing.Point(10, 132);
            this.lblfourth.Name = "lblfourth";
            this.lblfourth.Size = new System.Drawing.Size(80, 13);
            this.lblfourth.TabIndex = 35;
            this.lblfourth.Text = "Optional Field 4";
            this.lblfourth.Visible = false;
            // 
            // lblsecond
            // 
            this.lblsecond.AutoSize = true;
            this.lblsecond.Location = new System.Drawing.Point(10, 54);
            this.lblsecond.Name = "lblsecond";
            this.lblsecond.Size = new System.Drawing.Size(80, 13);
            this.lblsecond.TabIndex = 34;
            this.lblsecond.Text = "Optional Field 2";
            this.lblsecond.Visible = false;
            // 
            // lblthird
            // 
            this.lblthird.AutoSize = true;
            this.lblthird.Location = new System.Drawing.Point(10, 93);
            this.lblthird.Name = "lblthird";
            this.lblthird.Size = new System.Drawing.Size(80, 13);
            this.lblthird.TabIndex = 33;
            this.lblthird.Text = "Optional Field 3";
            this.lblthird.Visible = false;
            // 
            // lblfifth
            // 
            this.lblfifth.AutoSize = true;
            this.lblfifth.Location = new System.Drawing.Point(10, 171);
            this.lblfifth.Name = "lblfifth";
            this.lblfifth.Size = new System.Drawing.Size(80, 13);
            this.lblfifth.TabIndex = 32;
            this.lblfifth.Text = "Optional Field 5";
            this.lblfifth.Visible = false;
            // 
            // lblfirst
            // 
            this.lblfirst.AutoSize = true;
            this.lblfirst.Location = new System.Drawing.Point(10, 15);
            this.lblfirst.Name = "lblfirst";
            this.lblfirst.Size = new System.Drawing.Size(80, 13);
            this.lblfirst.TabIndex = 31;
            this.lblfirst.Text = "Optional Field 1";
            this.lblfirst.Visible = false;
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.txtchequereceiptid);
            this.groupBox3.Controls.Add(this.btnDate);
            this.groupBox3.Controls.Add(this.label12);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.cboProjectName);
            this.groupBox3.Controls.Add(this.txtVchNo);
            this.groupBox3.Controls.Add(this.txtBankID);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.txtDate);
            this.groupBox3.Controls.Add(this.cboSeriesName);
            this.groupBox3.Controls.Add(this.lblVouNo);
            this.groupBox3.Controls.Add(this.comboBankAccount);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Location = new System.Drawing.Point(6, 32);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(803, 95);
            this.groupBox3.TabIndex = 145;
            this.groupBox3.TabStop = false;
            // 
            // txtchequereceiptid
            // 
            this.txtchequereceiptid.BackColor = System.Drawing.Color.White;
            this.txtchequereceiptid.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtchequereceiptid.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtchequereceiptid.FocusLostColor = System.Drawing.Color.White;
            this.txtchequereceiptid.Location = new System.Drawing.Point(386, 43);
            this.txtchequereceiptid.Name = "txtchequereceiptid";
            this.txtchequereceiptid.Size = new System.Drawing.Size(88, 20);
            this.txtchequereceiptid.TabIndex = 148;
            this.txtchequereceiptid.Visible = false;
            // 
            // btnDate
            // 
            this.btnDate.Image = global::Accounts.Properties.Resources.dateIcon;
            this.btnDate.Location = new System.Drawing.Point(653, 14);
            this.btnDate.Name = "btnDate";
            this.btnDate.Size = new System.Drawing.Size(26, 23);
            this.btnDate.TabIndex = 147;
            this.btnDate.UseVisualStyleBackColor = true;
            this.btnDate.Click += new System.EventHandler(this.btnDate_Click);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(14, 69);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(46, 13);
            this.label12.TabIndex = 143;
            this.label12.Text = "Project :";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 43);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(78, 13);
            this.label6.TabIndex = 113;
            this.label6.Text = "Bank Account:";
            // 
            // cboProjectName
            // 
            this.cboProjectName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboProjectName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboProjectName.BackColor = System.Drawing.Color.White;
            this.cboProjectName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboProjectName.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.cboProjectName.FocusLostColor = System.Drawing.Color.White;
            this.cboProjectName.FormattingEnabled = true;
            this.cboProjectName.Location = new System.Drawing.Point(98, 69);
            this.cboProjectName.Name = "cboProjectName";
            this.cboProjectName.Size = new System.Drawing.Size(158, 21);
            this.cboProjectName.TabIndex = 142;
            // 
            // txtVchNo
            // 
            this.txtVchNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtVchNo.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtVchNo.FocusLostColor = System.Drawing.Color.White;
            this.txtVchNo.Location = new System.Drawing.Point(358, 16);
            this.txtVchNo.Name = "txtVchNo";
            this.txtVchNo.Size = new System.Drawing.Size(156, 20);
            this.txtVchNo.TabIndex = 98;
            // 
            // txtBankID
            // 
            this.txtBankID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBankID.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtBankID.FocusLostColor = System.Drawing.Color.White;
            this.txtBankID.Location = new System.Drawing.Point(281, 43);
            this.txtBankID.Name = "txtBankID";
            this.txtBankID.Size = new System.Drawing.Size(88, 20);
            this.txtBankID.TabIndex = 112;
            this.txtBankID.Visible = false;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(13, 18);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(42, 13);
            this.label8.TabIndex = 135;
            this.label8.Text = "Series :";
            // 
            // txtDate
            // 
            this.txtDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDate.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.txtDate.FocusLostColor = System.Drawing.Color.White;
            this.txtDate.Location = new System.Drawing.Point(577, 16);
            this.txtDate.Mask = "0000/00/00";
            this.txtDate.Name = "txtDate";
            this.txtDate.Size = new System.Drawing.Size(74, 20);
            this.txtDate.TabIndex = 103;
            // 
            // cboSeriesName
            // 
            this.cboSeriesName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboSeriesName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboSeriesName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSeriesName.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.cboSeriesName.FocusLostColor = System.Drawing.Color.White;
            this.cboSeriesName.FormattingEnabled = true;
            this.cboSeriesName.Location = new System.Drawing.Point(98, 14);
            this.cboSeriesName.Name = "cboSeriesName";
            this.cboSeriesName.Size = new System.Drawing.Size(158, 21);
            this.cboSeriesName.TabIndex = 134;
            this.cboSeriesName.SelectedIndexChanged += new System.EventHandler(this.cboSeriesName_SelectedIndexChanged);
            // 
            // lblVouNo
            // 
            this.lblVouNo.AutoSize = true;
            this.lblVouNo.Location = new System.Drawing.Point(287, 18);
            this.lblVouNo.Name = "lblVouNo";
            this.lblVouNo.Size = new System.Drawing.Size(60, 13);
            this.lblVouNo.TabIndex = 99;
            this.lblVouNo.Text = "Voucher #:";
            // 
            // comboBankAccount
            // 
            this.comboBankAccount.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBankAccount.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.comboBankAccount.FocusLostColor = System.Drawing.Color.White;
            this.comboBankAccount.FormattingEnabled = true;
            this.comboBankAccount.Location = new System.Drawing.Point(98, 41);
            this.comboBankAccount.Name = "comboBankAccount";
            this.comboBankAccount.Size = new System.Drawing.Size(158, 21);
            this.comboBankAccount.TabIndex = 114;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(535, 18);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(33, 13);
            this.label3.TabIndex = 102;
            this.label3.Text = "Date:";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.CadetBlue;
            this.panel1.Controls.Add(this.btnExport);
            this.panel1.Controls.Add(this.btnPrintPreview);
            this.panel1.Controls.Add(this.btnPaste);
            this.panel1.Controls.Add(this.btnCopy);
            this.panel1.Controls.Add(this.btnExit);
            this.panel1.Controls.Add(this.btnLast);
            this.panel1.Controls.Add(this.btnFirst);
            this.panel1.Controls.Add(this.btnPrint);
            this.panel1.Controls.Add(this.btnNext);
            this.panel1.Controls.Add(this.btnPrev);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(810, 32);
            this.panel1.TabIndex = 146;
            // 
            // btnExport
            // 
            this.btnExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExport.Enabled = false;
            this.btnExport.Image = global::Accounts.Properties.Resources.export1;
            this.btnExport.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnExport.Location = new System.Drawing.Point(738, 3);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(62, 23);
            this.btnExport.TabIndex = 33;
            this.btnExport.Text = "&Export";
            this.btnExport.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnExport.UseVisualStyleBackColor = true;
            // 
            // btnPrintPreview
            // 
            this.btnPrintPreview.Image = global::Accounts.Properties.Resources.print_preview;
            this.btnPrintPreview.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPrintPreview.Location = new System.Drawing.Point(417, 3);
            this.btnPrintPreview.Name = "btnPrintPreview";
            this.btnPrintPreview.Size = new System.Drawing.Size(94, 23);
            this.btnPrintPreview.TabIndex = 32;
            this.btnPrintPreview.Text = "Pr&int Preview";
            this.btnPrintPreview.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnPrintPreview.UseVisualStyleBackColor = true;
            this.btnPrintPreview.Click += new System.EventHandler(this.btnPrintPreview_Click);
            // 
            // btnPaste
            // 
            this.btnPaste.Image = global::Accounts.Properties.Resources.paste;
            this.btnPaste.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPaste.Location = new System.Drawing.Point(298, 3);
            this.btnPaste.Name = "btnPaste";
            this.btnPaste.Size = new System.Drawing.Size(58, 23);
            this.btnPaste.TabIndex = 24;
            this.btnPaste.Text = "Paste";
            this.btnPaste.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnPaste.UseVisualStyleBackColor = true;
            // 
            // btnCopy
            // 
            this.btnCopy.Image = global::Accounts.Properties.Resources.copy;
            this.btnCopy.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCopy.Location = new System.Drawing.Point(239, 3);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(58, 23);
            this.btnCopy.TabIndex = 23;
            this.btnCopy.Text = "Copy";
            this.btnCopy.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCopy.UseVisualStyleBackColor = true;
            // 
            // btnExit
            // 
            this.btnExit.Image = global::Accounts.Properties.Resources.ExitButton;
            this.btnExit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnExit.Location = new System.Drawing.Point(516, 3);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(58, 23);
            this.btnExit.TabIndex = 22;
            this.btnExit.Text = "Exit";
            this.btnExit.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnLast
            // 
            this.btnLast.Image = global::Accounts.Properties.Resources.last;
            this.btnLast.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnLast.Location = new System.Drawing.Point(180, 3);
            this.btnLast.Name = "btnLast";
            this.btnLast.Size = new System.Drawing.Size(58, 23);
            this.btnLast.TabIndex = 4;
            this.btnLast.Text = "Last";
            this.btnLast.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLast.UseVisualStyleBackColor = true;
            this.btnLast.Click += new System.EventHandler(this.btnLast_Click);
            // 
            // btnFirst
            // 
            this.btnFirst.Image = global::Accounts.Properties.Resources.first;
            this.btnFirst.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnFirst.Location = new System.Drawing.Point(4, 3);
            this.btnFirst.Name = "btnFirst";
            this.btnFirst.Size = new System.Drawing.Size(58, 23);
            this.btnFirst.TabIndex = 3;
            this.btnFirst.Text = "First";
            this.btnFirst.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnFirst.UseVisualStyleBackColor = true;
            this.btnFirst.Click += new System.EventHandler(this.btnFirst_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.Image = global::Accounts.Properties.Resources.print;
            this.btnPrint.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPrint.Location = new System.Drawing.Point(357, 3);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(58, 23);
            this.btnPrint.TabIndex = 2;
            this.btnPrint.Text = "Print";
            this.btnPrint.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnNext
            // 
            this.btnNext.Image = global::Accounts.Properties.Resources.forward;
            this.btnNext.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnNext.Location = new System.Drawing.Point(121, 3);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(58, 23);
            this.btnNext.TabIndex = 1;
            this.btnNext.Text = "&Next";
            this.btnNext.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnPrev
            // 
            this.btnPrev.Image = global::Accounts.Properties.Resources.back;
            this.btnPrev.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPrev.Location = new System.Drawing.Point(62, 3);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(58, 23);
            this.btnPrev.TabIndex = 0;
            this.btnPrev.Text = "&Prev";
            this.btnPrev.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnPrev.UseVisualStyleBackColor = true;
            this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnEdit);
            this.groupBox1.Controls.Add(this.btnNew);
            this.groupBox1.Controls.Add(this.btnCancel);
            this.groupBox1.Controls.Add(this.btnSave);
            this.groupBox1.Controls.Add(this.chkDoNotClose);
            this.groupBox1.Controls.Add(this.chkPrntWhileSaving);
            this.groupBox1.Controls.Add(this.btnDelete);
            this.groupBox1.Location = new System.Drawing.Point(5, 439);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(801, 48);
            this.groupBox1.TabIndex = 147;
            this.groupBox1.TabStop = false;
            // 
            // btnEdit
            // 
            this.btnEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEdit.Image = global::Accounts.Properties.Resources.document_edit;
            this.btnEdit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEdit.Location = new System.Drawing.Point(419, 14);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(75, 28);
            this.btnEdit.TabIndex = 107;
            this.btnEdit.Text = "&Edit";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnNew
            // 
            this.btnNew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNew.Image = global::Accounts.Properties.Resources.edit_add;
            this.btnNew.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNew.Location = new System.Drawing.Point(338, 13);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(75, 28);
            this.btnNew.TabIndex = 104;
            this.btnNew.Text = "&New";
            this.btnNew.UseVisualStyleBackColor = true;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Image = global::Accounts.Properties.Resources.gnome_window_close;
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(662, 15);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 28);
            this.btnCancel.TabIndex = 108;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Image = global::Accounts.Properties.Resources.save;
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSave.Location = new System.Drawing.Point(500, 15);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 28);
            this.btnSave.TabIndex = 105;
            this.btnSave.Text = "&Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // chkDoNotClose
            // 
            this.chkDoNotClose.AutoSize = true;
            this.chkDoNotClose.Location = new System.Drawing.Point(6, 20);
            this.chkDoNotClose.Name = "chkDoNotClose";
            this.chkDoNotClose.Size = new System.Drawing.Size(86, 17);
            this.chkDoNotClose.TabIndex = 110;
            this.chkDoNotClose.Text = "Do not close";
            this.chkDoNotClose.UseVisualStyleBackColor = true;
            // 
            // chkPrntWhileSaving
            // 
            this.chkPrntWhileSaving.AutoSize = true;
            this.chkPrntWhileSaving.Location = new System.Drawing.Point(96, 20);
            this.chkPrntWhileSaving.Name = "chkPrntWhileSaving";
            this.chkPrntWhileSaving.Size = new System.Drawing.Size(108, 17);
            this.chkPrntWhileSaving.TabIndex = 111;
            this.chkPrntWhileSaving.Text = "Print while saving";
            this.chkPrntWhileSaving.UseVisualStyleBackColor = true;
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDelete.Image = global::Accounts.Properties.Resources.document_delete;
            this.btnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDelete.Location = new System.Drawing.Point(581, 15);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 28);
            this.btnDelete.TabIndex = 106;
            this.btnDelete.Text = "&Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 420);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 149;
            this.label2.Text = "Narration:";
            // 
            // txtRemarks
            // 
            this.txtRemarks.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtRemarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtRemarks.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtRemarks.FocusLostColor = System.Drawing.Color.White;
            this.txtRemarks.Location = new System.Drawing.Point(103, 417);
            this.txtRemarks.Name = "txtRemarks";
            this.txtRemarks.Size = new System.Drawing.Size(648, 20);
            this.txtRemarks.TabIndex = 148;
            // 
            // frmchequereceipt
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(810, 492);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtRemarks);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.tcchequereceipt);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmchequereceipt";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Cheque Receipt";
            this.Load += new System.EventHandler(this.frmchequereceipt_Load);
            this.tcchequereceipt.ResumeLayout(false);
            this.tpdetails.ResumeLayout(false);
            this.tpaccountingclass.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tcchequereceipt;
        private System.Windows.Forms.TabPage tpdetails;
        private System.Windows.Forms.TabPage tpaccountingclass;
        private STreeView treeAccClass;
        private System.Windows.Forms.Button btnAddAccClass;
        private SourceGrid.Grid grdchequereceipt;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnDate;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label6;
        private SComboBox cboProjectName;
        private STextBox txtVchNo;
        private STextBox txtBankID;
        private System.Windows.Forms.Label label8;
        private SMaskedTextBox txtDate;
        private SComboBox cboSeriesName;
        private System.Windows.Forms.Label lblVouNo;
        private SComboBox comboBankAccount;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnPaste;
        private System.Windows.Forms.Button btnCopy;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnLast;
        private System.Windows.Forms.Button btnFirst;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnPrev;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.CheckBox chkDoNotClose;
        private System.Windows.Forms.CheckBox chkPrntWhileSaving;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Label label2;
        private STextBox txtRemarks;
        private STextBox txtchequereceiptid;
        private System.Windows.Forms.TabPage tabPage1;
        private STextBox txtfifth;
        private STextBox txtfourth;
        private STextBox txtthird;
        private STextBox txtsecond;
        private STextBox txtfirst;
        private System.Windows.Forms.Label lblfourth;
        private System.Windows.Forms.Label lblsecond;
        private System.Windows.Forms.Label lblthird;
        private System.Windows.Forms.Label lblfifth;
        private System.Windows.Forms.Label lblfirst;
        private System.Windows.Forms.Button btnPrintPreview;
        private System.Windows.Forms.Button btnExport;
    }
}