using SComponents;
namespace Inventory
{
    partial class frmPurchaseReportSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPurchaseReportSettings));
            this.grpProduct = new System.Windows.Forms.GroupBox();
            this.cboProductGroup = new SComponents.SComboBox();
            this.cboProductSingle = new SComponents.SComboBox();
            this.rdProductGroup = new System.Windows.Forms.RadioButton();
            this.rdProductSingle = new System.Windows.Forms.RadioButton();
            this.rdProductAll = new System.Windows.Forms.RadioButton();
            this.grpParty = new System.Windows.Forms.GroupBox();
            this.rdPartyGroup = new System.Windows.Forms.RadioButton();
            this.rdpartySingle = new System.Windows.Forms.RadioButton();
            this.rdPartyAll = new System.Windows.Forms.RadioButton();
            this.cboPartyGroup = new SComponents.SComboBox();
            this.cboPartySingle = new SComponents.SComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chkProject = new System.Windows.Forms.CheckBox();
            this.cboProjectName = new SComponents.SComboBox();
            this.chkPurchaseAccount = new System.Windows.Forms.CheckBox();
            this.cboPurchaseAccount = new SComponents.SComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rdParty = new System.Windows.Forms.RadioButton();
            this.rdProduct = new System.Windows.Forms.RadioButton();
            this.btnShow = new System.Windows.Forms.Button();
            this.grpDate = new System.Windows.Forms.GroupBox();
            this.cboMonths = new SComponents.SComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnToDate = new System.Windows.Forms.Button();
            this.btnFromDate = new System.Windows.Forms.Button();
            this.txtFromDate = new System.Windows.Forms.MaskedTextBox();
            this.txtToDate = new System.Windows.Forms.MaskedTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.cboVoucherwise = new SComponents.SComboBox();
            this.cboDeotwise = new SComponents.SComboBox();
            this.chkVoucherwise = new System.Windows.Forms.CheckBox();
            this.chkDepot = new System.Windows.Forms.CheckBox();
            this.chkDateRange = new System.Windows.Forms.CheckBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSelectAccClass = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnsavestate = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.grpProduct.SuspendLayout();
            this.grpParty.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.grpDate.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpProduct
            // 
            this.grpProduct.BackColor = System.Drawing.Color.WhiteSmoke;
            this.grpProduct.Controls.Add(this.cboProductGroup);
            this.grpProduct.Controls.Add(this.cboProductSingle);
            this.grpProduct.Controls.Add(this.rdProductGroup);
            this.grpProduct.Controls.Add(this.rdProductSingle);
            this.grpProduct.Controls.Add(this.rdProductAll);
            this.grpProduct.Location = new System.Drawing.Point(128, 2);
            this.grpProduct.Name = "grpProduct";
            this.grpProduct.Size = new System.Drawing.Size(278, 90);
            this.grpProduct.TabIndex = 60;
            this.grpProduct.TabStop = false;
            // 
            // cboProductGroup
            // 
            this.cboProductGroup.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cboProductGroup.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboProductGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboProductGroup.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.cboProductGroup.FocusLostColor = System.Drawing.Color.White;
            this.cboProductGroup.FormattingEnabled = true;
            this.cboProductGroup.Location = new System.Drawing.Point(110, 55);
            this.cboProductGroup.Name = "cboProductGroup";
            this.cboProductGroup.Size = new System.Drawing.Size(156, 21);
            this.cboProductGroup.TabIndex = 64;
            // 
            // cboProductSingle
            // 
            this.cboProductSingle.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cboProductSingle.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboProductSingle.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.cboProductSingle.FocusLostColor = System.Drawing.Color.White;
            this.cboProductSingle.FormattingEnabled = true;
            this.cboProductSingle.Location = new System.Drawing.Point(110, 28);
            this.cboProductSingle.Name = "cboProductSingle";
            this.cboProductSingle.Size = new System.Drawing.Size(156, 21);
            this.cboProductSingle.TabIndex = 6;
            // 
            // rdProductGroup
            // 
            this.rdProductGroup.AutoSize = true;
            this.rdProductGroup.Location = new System.Drawing.Point(2, 56);
            this.rdProductGroup.Name = "rdProductGroup";
            this.rdProductGroup.Size = new System.Drawing.Size(94, 17);
            this.rdProductGroup.TabIndex = 2;
            this.rdProductGroup.TabStop = true;
            this.rdProductGroup.Text = "Product Group";
            this.rdProductGroup.UseVisualStyleBackColor = true;
            this.rdProductGroup.CheckedChanged += new System.EventHandler(this.rdProductGroup_CheckedChanged);
            // 
            // rdProductSingle
            // 
            this.rdProductSingle.AutoSize = true;
            this.rdProductSingle.Location = new System.Drawing.Point(3, 32);
            this.rdProductSingle.Name = "rdProductSingle";
            this.rdProductSingle.Size = new System.Drawing.Size(94, 17);
            this.rdProductSingle.TabIndex = 1;
            this.rdProductSingle.TabStop = true;
            this.rdProductSingle.Text = "Single Product";
            this.rdProductSingle.UseVisualStyleBackColor = true;
            this.rdProductSingle.CheckedChanged += new System.EventHandler(this.rdProductSingle_CheckedChanged);
            // 
            // rdProductAll
            // 
            this.rdProductAll.AutoSize = true;
            this.rdProductAll.Location = new System.Drawing.Point(3, 9);
            this.rdProductAll.Name = "rdProductAll";
            this.rdProductAll.Size = new System.Drawing.Size(81, 17);
            this.rdProductAll.TabIndex = 0;
            this.rdProductAll.TabStop = true;
            this.rdProductAll.Text = "All Products";
            this.rdProductAll.UseVisualStyleBackColor = true;
            this.rdProductAll.CheckedChanged += new System.EventHandler(this.rdProductAll_CheckedChanged);
            // 
            // grpParty
            // 
            this.grpParty.BackColor = System.Drawing.Color.WhiteSmoke;
            this.grpParty.Controls.Add(this.rdPartyGroup);
            this.grpParty.Controls.Add(this.rdpartySingle);
            this.grpParty.Controls.Add(this.rdPartyAll);
            this.grpParty.Controls.Add(this.cboPartyGroup);
            this.grpParty.Controls.Add(this.cboPartySingle);
            this.grpParty.Location = new System.Drawing.Point(129, 2);
            this.grpParty.Name = "grpParty";
            this.grpParty.Size = new System.Drawing.Size(278, 90);
            this.grpParty.TabIndex = 55;
            this.grpParty.TabStop = false;
            // 
            // rdPartyGroup
            // 
            this.rdPartyGroup.AutoSize = true;
            this.rdPartyGroup.Location = new System.Drawing.Point(9, 58);
            this.rdPartyGroup.Name = "rdPartyGroup";
            this.rdPartyGroup.Size = new System.Drawing.Size(81, 17);
            this.rdPartyGroup.TabIndex = 80;
            this.rdPartyGroup.TabStop = true;
            this.rdPartyGroup.Text = "Party Group";
            this.rdPartyGroup.UseVisualStyleBackColor = true;
            this.rdPartyGroup.CheckedChanged += new System.EventHandler(this.rdPartyGroup_CheckedChanged_1);
            // 
            // rdpartySingle
            // 
            this.rdpartySingle.AutoSize = true;
            this.rdpartySingle.Location = new System.Drawing.Point(9, 32);
            this.rdpartySingle.Name = "rdpartySingle";
            this.rdpartySingle.Size = new System.Drawing.Size(81, 17);
            this.rdpartySingle.TabIndex = 80;
            this.rdpartySingle.TabStop = true;
            this.rdpartySingle.Text = "Single Party";
            this.rdpartySingle.UseVisualStyleBackColor = true;
            this.rdpartySingle.CheckedChanged += new System.EventHandler(this.rdpartySingle_CheckedChanged);
            // 
            // rdPartyAll
            // 
            this.rdPartyAll.AutoSize = true;
            this.rdPartyAll.Location = new System.Drawing.Point(9, 9);
            this.rdPartyAll.Name = "rdPartyAll";
            this.rdPartyAll.Size = new System.Drawing.Size(63, 17);
            this.rdPartyAll.TabIndex = 80;
            this.rdPartyAll.TabStop = true;
            this.rdPartyAll.Text = "All Party";
            this.rdPartyAll.UseVisualStyleBackColor = true;
            // 
            // cboPartyGroup
            // 
            this.cboPartyGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPartyGroup.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.cboPartyGroup.FocusLostColor = System.Drawing.Color.White;
            this.cboPartyGroup.FormattingEnabled = true;
            this.cboPartyGroup.Location = new System.Drawing.Point(115, 55);
            this.cboPartyGroup.Name = "cboPartyGroup";
            this.cboPartyGroup.Size = new System.Drawing.Size(148, 21);
            this.cboPartyGroup.TabIndex = 73;
            // 
            // cboPartySingle
            // 
            this.cboPartySingle.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cboPartySingle.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboPartySingle.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.cboPartySingle.FocusLostColor = System.Drawing.Color.White;
            this.cboPartySingle.FormattingEnabled = true;
            this.cboPartySingle.Location = new System.Drawing.Point(115, 28);
            this.cboPartySingle.Name = "cboPartySingle";
            this.cboPartySingle.Size = new System.Drawing.Size(148, 21);
            this.cboPartySingle.TabIndex = 72;
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox2.Controls.Add(this.chkProject);
            this.groupBox2.Controls.Add(this.cboProjectName);
            this.groupBox2.Controls.Add(this.chkPurchaseAccount);
            this.groupBox2.Controls.Add(this.cboPurchaseAccount);
            this.groupBox2.Location = new System.Drawing.Point(3, 150);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(412, 44);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            // 
            // chkProject
            // 
            this.chkProject.AutoSize = true;
            this.chkProject.Location = new System.Drawing.Point(227, 17);
            this.chkProject.Name = "chkProject";
            this.chkProject.Size = new System.Drawing.Size(59, 17);
            this.chkProject.TabIndex = 2;
            this.chkProject.Text = "Project";
            this.chkProject.UseVisualStyleBackColor = true;
            this.chkProject.CheckedChanged += new System.EventHandler(this.chkProject_CheckedChanged);
            // 
            // cboProjectName
            // 
            this.cboProjectName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboProjectName.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.cboProjectName.FocusLostColor = System.Drawing.Color.White;
            this.cboProjectName.FormattingEnabled = true;
            this.cboProjectName.Location = new System.Drawing.Point(295, 13);
            this.cboProjectName.Name = "cboProjectName";
            this.cboProjectName.Size = new System.Drawing.Size(105, 21);
            this.cboProjectName.TabIndex = 3;
            // 
            // chkPurchaseAccount
            // 
            this.chkPurchaseAccount.AutoSize = true;
            this.chkPurchaseAccount.Location = new System.Drawing.Point(7, 17);
            this.chkPurchaseAccount.Name = "chkPurchaseAccount";
            this.chkPurchaseAccount.Size = new System.Drawing.Size(93, 17);
            this.chkPurchaseAccount.TabIndex = 0;
            this.chkPurchaseAccount.Text = "Purchase A/C";
            this.chkPurchaseAccount.UseVisualStyleBackColor = true;
            this.chkPurchaseAccount.CheckedChanged += new System.EventHandler(this.chkPurchaseAccount_CheckedChanged);
            // 
            // cboPurchaseAccount
            // 
            this.cboPurchaseAccount.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboPurchaseAccount.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboPurchaseAccount.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPurchaseAccount.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.cboPurchaseAccount.FocusLostColor = System.Drawing.Color.White;
            this.cboPurchaseAccount.FormattingEnabled = true;
            this.cboPurchaseAccount.Location = new System.Drawing.Point(105, 13);
            this.cboPurchaseAccount.Name = "cboPurchaseAccount";
            this.cboPurchaseAccount.Size = new System.Drawing.Size(101, 21);
            this.cboPurchaseAccount.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox1.Controls.Add(this.rdParty);
            this.groupBox1.Controls.Add(this.rdProduct);
            this.groupBox1.Location = new System.Drawing.Point(4, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(123, 90);
            this.groupBox1.TabIndex = 51;
            this.groupBox1.TabStop = false;
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // rdParty
            // 
            this.rdParty.AutoSize = true;
            this.rdParty.Location = new System.Drawing.Point(13, 55);
            this.rdParty.Name = "rdParty";
            this.rdParty.Size = new System.Drawing.Size(49, 17);
            this.rdParty.TabIndex = 1;
            this.rdParty.TabStop = true;
            this.rdParty.Text = "Party";
            this.rdParty.UseVisualStyleBackColor = true;
            this.rdParty.CheckedChanged += new System.EventHandler(this.rdParty_CheckedChanged);
            // 
            // rdProduct
            // 
            this.rdProduct.AutoSize = true;
            this.rdProduct.Location = new System.Drawing.Point(13, 19);
            this.rdProduct.Name = "rdProduct";
            this.rdProduct.Size = new System.Drawing.Size(62, 17);
            this.rdProduct.TabIndex = 0;
            this.rdProduct.TabStop = true;
            this.rdProduct.Text = "Product";
            this.rdProduct.UseVisualStyleBackColor = true;
            this.rdProduct.CheckedChanged += new System.EventHandler(this.rdProduct_CheckedChanged);
            // 
            // btnShow
            // 
            this.btnShow.Location = new System.Drawing.Point(11, 12);
            this.btnShow.Name = "btnShow";
            this.btnShow.Size = new System.Drawing.Size(75, 23);
            this.btnShow.TabIndex = 4;
            this.btnShow.Text = "Show";
            this.btnShow.UseVisualStyleBackColor = true;
            this.btnShow.Click += new System.EventHandler(this.btnShow_Click);
            // 
            // grpDate
            // 
            this.grpDate.BackColor = System.Drawing.Color.WhiteSmoke;
            this.grpDate.Controls.Add(this.cboMonths);
            this.grpDate.Controls.Add(this.label1);
            this.grpDate.Controls.Add(this.btnToDate);
            this.grpDate.Controls.Add(this.btnFromDate);
            this.grpDate.Controls.Add(this.txtFromDate);
            this.grpDate.Controls.Add(this.txtToDate);
            this.grpDate.Controls.Add(this.label2);
            this.grpDate.Controls.Add(this.label6);
            this.grpDate.Location = new System.Drawing.Point(4, 212);
            this.grpDate.Name = "grpDate";
            this.grpDate.Size = new System.Drawing.Size(412, 69);
            this.grpDate.TabIndex = 56;
            this.grpDate.TabStop = false;
            // 
            // cboMonths
            // 
            this.cboMonths.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMonths.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.cboMonths.FocusLostColor = System.Drawing.Color.White;
            this.cboMonths.FormattingEnabled = true;
            this.cboMonths.Location = new System.Drawing.Point(91, 45);
            this.cboMonths.Name = "cboMonths";
            this.cboMonths.Size = new System.Drawing.Size(124, 21);
            this.cboMonths.TabIndex = 63;
            this.cboMonths.SelectedIndexChanged += new System.EventHandler(this.cboMonths_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 13);
            this.label1.TabIndex = 62;
            this.label1.Text = "End of Month";
            // 
            // btnToDate
            // 
            this.btnToDate.Image = global::Inventory.Properties.Resources.dateIcon;
            this.btnToDate.Location = new System.Drawing.Point(385, 13);
            this.btnToDate.Name = "btnToDate";
            this.btnToDate.Size = new System.Drawing.Size(26, 25);
            this.btnToDate.TabIndex = 5;
            this.btnToDate.UseVisualStyleBackColor = true;
            this.btnToDate.Click += new System.EventHandler(this.btnToDate_Click);
            // 
            // btnFromDate
            // 
            this.btnFromDate.Image = global::Inventory.Properties.Resources.dateIcon;
            this.btnFromDate.Location = new System.Drawing.Point(190, 14);
            this.btnFromDate.Name = "btnFromDate";
            this.btnFromDate.Size = new System.Drawing.Size(26, 25);
            this.btnFromDate.TabIndex = 4;
            this.btnFromDate.UseVisualStyleBackColor = true;
            this.btnFromDate.Click += new System.EventHandler(this.btnFromDate_Click);
            // 
            // txtFromDate
            // 
            this.txtFromDate.Location = new System.Drawing.Point(53, 17);
            this.txtFromDate.Mask = "##/##/####";
            this.txtFromDate.Name = "txtFromDate";
            this.txtFromDate.Size = new System.Drawing.Size(131, 20);
            this.txtFromDate.TabIndex = 1;
            // 
            // txtToDate
            // 
            this.txtToDate.Location = new System.Drawing.Point(261, 16);
            this.txtToDate.Mask = "##/##/####";
            this.txtToDate.Name = "txtToDate";
            this.txtToDate.Size = new System.Drawing.Size(121, 20);
            this.txtToDate.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(232, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(23, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "To:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(14, 20);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(33, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "From:";
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox3.Controls.Add(this.cboVoucherwise);
            this.groupBox3.Controls.Add(this.cboDeotwise);
            this.groupBox3.Controls.Add(this.chkVoucherwise);
            this.groupBox3.Controls.Add(this.chkDepot);
            this.groupBox3.Location = new System.Drawing.Point(3, 98);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(409, 46);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            // 
            // cboVoucherwise
            // 
            this.cboVoucherwise.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboVoucherwise.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.cboVoucherwise.FocusLostColor = System.Drawing.Color.White;
            this.cboVoucherwise.FormattingEnabled = true;
            this.cboVoucherwise.Location = new System.Drawing.Point(292, 15);
            this.cboVoucherwise.Name = "cboVoucherwise";
            this.cboVoucherwise.Size = new System.Drawing.Size(105, 21);
            this.cboVoucherwise.TabIndex = 75;
            // 
            // cboDeotwise
            // 
            this.cboDeotwise.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDeotwise.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.cboDeotwise.FocusLostColor = System.Drawing.Color.White;
            this.cboDeotwise.FormattingEnabled = true;
            this.cboDeotwise.Location = new System.Drawing.Point(102, 15);
            this.cboDeotwise.Name = "cboDeotwise";
            this.cboDeotwise.Size = new System.Drawing.Size(101, 21);
            this.cboDeotwise.TabIndex = 74;
            // 
            // chkVoucherwise
            // 
            this.chkVoucherwise.AutoSize = true;
            this.chkVoucherwise.Location = new System.Drawing.Point(221, 19);
            this.chkVoucherwise.Name = "chkVoucherwise";
            this.chkVoucherwise.Size = new System.Drawing.Size(66, 17);
            this.chkVoucherwise.TabIndex = 1;
            this.chkVoucherwise.Text = "Voucher";
            this.chkVoucherwise.UseVisualStyleBackColor = true;
            this.chkVoucherwise.CheckedChanged += new System.EventHandler(this.chkVoucherwise_CheckedChanged);
            // 
            // chkDepot
            // 
            this.chkDepot.AutoSize = true;
            this.chkDepot.Location = new System.Drawing.Point(6, 17);
            this.chkDepot.Name = "chkDepot";
            this.chkDepot.Size = new System.Drawing.Size(55, 17);
            this.chkDepot.TabIndex = 0;
            this.chkDepot.Text = "Depot";
            this.chkDepot.UseVisualStyleBackColor = true;
            this.chkDepot.CheckedChanged += new System.EventHandler(this.chkProductDepot_CheckedChanged);
            // 
            // chkDateRange
            // 
            this.chkDateRange.AutoSize = true;
            this.chkDateRange.Location = new System.Drawing.Point(10, 200);
            this.chkDateRange.Name = "chkDateRange";
            this.chkDateRange.Size = new System.Drawing.Size(117, 17);
            this.chkDateRange.TabIndex = 2;
            this.chkDateRange.Text = "Select Date Range";
            this.chkDateRange.UseVisualStyleBackColor = true;
            this.chkDateRange.CheckedChanged += new System.EventHandler(this.chkDateRange_CheckedChanged);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Image = global::Inventory.Properties.Resources.gnome_window_close;
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(12, 47);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(74, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnSelectAccClass
            // 
            this.btnSelectAccClass.Location = new System.Drawing.Point(257, 14);
            this.btnSelectAccClass.Name = "btnSelectAccClass";
            this.btnSelectAccClass.Size = new System.Drawing.Size(144, 23);
            this.btnSelectAccClass.TabIndex = 3;
            this.btnSelectAccClass.Text = "Select Account Class";
            this.btnSelectAccClass.UseVisualStyleBackColor = true;
            this.btnSelectAccClass.Click += new System.EventHandler(this.btnSelectAccClass_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.LightSteelBlue;
            this.panel1.Controls.Add(this.btnsavestate);
            this.panel1.Controls.Add(this.btnShow);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Location = new System.Drawing.Point(415, -1);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(97, 333);
            this.panel1.TabIndex = 61;
            // 
            // btnsavestate
            // 
            this.btnsavestate.Location = new System.Drawing.Point(11, 80);
            this.btnsavestate.Name = "btnsavestate";
            this.btnsavestate.Size = new System.Drawing.Size(76, 23);
            this.btnsavestate.TabIndex = 65;
            this.btnsavestate.Text = "Save State";
            this.btnsavestate.UseVisualStyleBackColor = true;
            this.btnsavestate.Click += new System.EventHandler(this.btnsavestate_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.BackColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox4.Controls.Add(this.btnSelectAccClass);
            this.groupBox4.Location = new System.Drawing.Point(5, 287);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(407, 45);
            this.groupBox4.TabIndex = 62;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Accounting Class";
            // 
            // frmPurchaseReportSettings
            // 
            this.AcceptButton = this.btnShow;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(511, 338);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.chkDateRange);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.grpDate);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.grpProduct);
            this.Controls.Add(this.grpParty);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "frmPurchaseReportSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Purchase Report Settings";
            this.Load += new System.EventHandler(this.frmPurchaseReportSettings_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmPurchaseReportSettings_KeyDown);
            this.grpProduct.ResumeLayout(false);
            this.grpProduct.PerformLayout();
            this.grpParty.ResumeLayout(false);
            this.grpParty.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.grpDate.ResumeLayout(false);
            this.grpDate.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grpProduct;
        private SComboBox cboProductSingle;
        private System.Windows.Forms.RadioButton rdProductAll;
        private System.Windows.Forms.GroupBox grpParty;
        private SComboBox cboPurchaseAccount;
        private SComboBox cboProjectName;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rdParty;
        private System.Windows.Forms.RadioButton rdProduct;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnShow;
        private System.Windows.Forms.GroupBox grpDate;
        private System.Windows.Forms.MaskedTextBox txtFromDate;
        private System.Windows.Forms.MaskedTextBox txtToDate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox chkPurchaseAccount;
        private SComboBox cboProductGroup;
        private SComboBox cboPartyGroup;
        private SComboBox cboPartySingle;
        private System.Windows.Forms.RadioButton rdPartyAll;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton rdpartySingle;
        private SComboBox cboVoucherwise;
        private SComboBox cboDeotwise;
        private System.Windows.Forms.CheckBox chkVoucherwise;
        private System.Windows.Forms.CheckBox chkDepot;
        private System.Windows.Forms.RadioButton rdPartyGroup;
        private System.Windows.Forms.RadioButton rdProductSingle;
        private System.Windows.Forms.RadioButton rdProductGroup;
        private System.Windows.Forms.CheckBox chkDateRange;
        private System.Windows.Forms.CheckBox chkProject;
        private System.Windows.Forms.Button btnSelectAccClass;
        private System.Windows.Forms.Button btnToDate;
        private System.Windows.Forms.Button btnFromDate;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button btnsavestate;
        private SComboBox cboMonths;
        private System.Windows.Forms.Label label1;

    }
}