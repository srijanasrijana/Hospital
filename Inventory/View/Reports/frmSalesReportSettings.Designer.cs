using SComponents;
namespace Inventory
{
    partial class frmSalesReportSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSalesReportSettings));
            this.btnShow = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rdParty = new System.Windows.Forms.RadioButton();
            this.rdProduct = new System.Windows.Forms.RadioButton();
            this.grpParty = new System.Windows.Forms.GroupBox();
            this.rdPartyGroup = new System.Windows.Forms.RadioButton();
            this.rdpartySingle = new System.Windows.Forms.RadioButton();
            this.rdPartyAll = new System.Windows.Forms.RadioButton();
            this.cboPartyGroup = new SComponents.SComboBox();
            this.cboPartySingle = new SComponents.SComboBox();
            this.grpProduct = new System.Windows.Forms.GroupBox();
            this.cboProductGroup = new SComponents.SComboBox();
            this.cboProductSingle = new SComponents.SComboBox();
            this.rdProductGroup = new System.Windows.Forms.RadioButton();
            this.rdProductSingle = new System.Windows.Forms.RadioButton();
            this.rdProductAll = new System.Windows.Forms.RadioButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.cboVoucherwise = new SComponents.SComboBox();
            this.cboDeotwise = new SComponents.SComboBox();
            this.chkVoucherwise = new System.Windows.Forms.CheckBox();
            this.chkDepot = new System.Windows.Forms.CheckBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.chkProject = new System.Windows.Forms.CheckBox();
            this.cboProjectName = new SComponents.SComboBox();
            this.chkSalesAccount = new System.Windows.Forms.CheckBox();
            this.cboSalesAccount = new SComponents.SComboBox();
            this.chkDateRange = new System.Windows.Forms.CheckBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.cboMonths = new SComponents.SComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnToDate = new System.Windows.Forms.Button();
            this.btnFromDate = new System.Windows.Forms.Button();
            this.txtFromDate = new System.Windows.Forms.MaskedTextBox();
            this.txtToDate = new System.Windows.Forms.MaskedTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnSelectAccClass = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnsavestate = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.grpParty.SuspendLayout();
            this.grpProduct.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnShow
            // 
            this.btnShow.Location = new System.Drawing.Point(8, 13);
            this.btnShow.Name = "btnShow";
            this.btnShow.Size = new System.Drawing.Size(75, 23);
            this.btnShow.TabIndex = 12;
            this.btnShow.Text = "Show";
            this.btnShow.UseVisualStyleBackColor = true;
            this.btnShow.Click += new System.EventHandler(this.btnShow_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Image = global::Inventory.Properties.Resources.gnome_window_close;
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(8, 49);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 19;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox1.Controls.Add(this.rdParty);
            this.groupBox1.Controls.Add(this.rdProduct);
            this.groupBox1.Location = new System.Drawing.Point(2, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(123, 90);
            this.groupBox1.TabIndex = 52;
            this.groupBox1.TabStop = false;
            // 
            // rdParty
            // 
            this.rdParty.AutoSize = true;
            this.rdParty.Location = new System.Drawing.Point(13, 55);
            this.rdParty.Name = "rdParty";
            this.rdParty.Size = new System.Drawing.Size(49, 17);
            this.rdParty.TabIndex = 0;
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
            // grpParty
            // 
            this.grpParty.BackColor = System.Drawing.Color.WhiteSmoke;
            this.grpParty.Controls.Add(this.rdPartyGroup);
            this.grpParty.Controls.Add(this.rdpartySingle);
            this.grpParty.Controls.Add(this.rdPartyAll);
            this.grpParty.Controls.Add(this.cboPartyGroup);
            this.grpParty.Controls.Add(this.cboPartySingle);
            this.grpParty.Location = new System.Drawing.Point(132, 1);
            this.grpParty.Name = "grpParty";
            this.grpParty.Size = new System.Drawing.Size(282, 90);
            this.grpParty.TabIndex = 56;
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
            this.rdPartyGroup.CheckedChanged += new System.EventHandler(this.rdPartyGroup_CheckedChanged);
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
            this.cboPartyGroup.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cboPartyGroup.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
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
            // grpProduct
            // 
            this.grpProduct.BackColor = System.Drawing.Color.WhiteSmoke;
            this.grpProduct.Controls.Add(this.cboProductGroup);
            this.grpProduct.Controls.Add(this.cboProductSingle);
            this.grpProduct.Controls.Add(this.rdProductGroup);
            this.grpProduct.Controls.Add(this.rdProductSingle);
            this.grpProduct.Controls.Add(this.rdProductAll);
            this.grpProduct.Location = new System.Drawing.Point(133, 2);
            this.grpProduct.Name = "grpProduct";
            this.grpProduct.Size = new System.Drawing.Size(282, 90);
            this.grpProduct.TabIndex = 61;
            this.grpProduct.TabStop = false;
            // 
            // cboProductGroup
            // 
            this.cboProductGroup.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cboProductGroup.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
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
            this.rdProductGroup.Location = new System.Drawing.Point(6, 56);
            this.rdProductGroup.Name = "rdProductGroup";
            this.rdProductGroup.Size = new System.Drawing.Size(94, 17);
            this.rdProductGroup.TabIndex = 0;
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
            this.rdProductSingle.TabIndex = 0;
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
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox3.Controls.Add(this.cboVoucherwise);
            this.groupBox3.Controls.Add(this.cboDeotwise);
            this.groupBox3.Controls.Add(this.chkVoucherwise);
            this.groupBox3.Controls.Add(this.chkDepot);
            this.groupBox3.Location = new System.Drawing.Point(4, 97);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(409, 46);
            this.groupBox3.TabIndex = 65;
            this.groupBox3.TabStop = false;
            // 
            // cboVoucherwise
            // 
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
            this.chkVoucherwise.TabIndex = 73;
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
            this.chkDepot.TabIndex = 72;
            this.chkDepot.Text = "Depot";
            this.chkDepot.UseVisualStyleBackColor = true;
            this.chkDepot.CheckedChanged += new System.EventHandler(this.chkDepot_CheckedChanged);
            // 
            // groupBox4
            // 
            this.groupBox4.BackColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox4.Controls.Add(this.chkProject);
            this.groupBox4.Controls.Add(this.cboProjectName);
            this.groupBox4.Controls.Add(this.chkSalesAccount);
            this.groupBox4.Controls.Add(this.cboSalesAccount);
            this.groupBox4.Location = new System.Drawing.Point(6, 148);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(412, 40);
            this.groupBox4.TabIndex = 66;
            this.groupBox4.TabStop = false;
            // 
            // chkProject
            // 
            this.chkProject.AutoSize = true;
            this.chkProject.Location = new System.Drawing.Point(227, 17);
            this.chkProject.Name = "chkProject";
            this.chkProject.Size = new System.Drawing.Size(59, 17);
            this.chkProject.TabIndex = 55;
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
            this.cboProjectName.TabIndex = 39;
            // 
            // chkSalesAccount
            // 
            this.chkSalesAccount.AutoSize = true;
            this.chkSalesAccount.Location = new System.Drawing.Point(7, 17);
            this.chkSalesAccount.Name = "chkSalesAccount";
            this.chkSalesAccount.Size = new System.Drawing.Size(74, 17);
            this.chkSalesAccount.TabIndex = 53;
            this.chkSalesAccount.Text = "Sales A/C";
            this.chkSalesAccount.UseVisualStyleBackColor = true;
            this.chkSalesAccount.CheckedChanged += new System.EventHandler(this.chkSalesAccount_CheckedChanged);
            // 
            // cboSalesAccount
            // 
            this.cboSalesAccount.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboSalesAccount.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboSalesAccount.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.cboSalesAccount.FocusLostColor = System.Drawing.Color.White;
            this.cboSalesAccount.FormattingEnabled = true;
            this.cboSalesAccount.Location = new System.Drawing.Point(105, 13);
            this.cboSalesAccount.Name = "cboSalesAccount";
            this.cboSalesAccount.Size = new System.Drawing.Size(101, 21);
            this.cboSalesAccount.TabIndex = 54;
            // 
            // chkDateRange
            // 
            this.chkDateRange.AutoSize = true;
            this.chkDateRange.Location = new System.Drawing.Point(14, 198);
            this.chkDateRange.Name = "chkDateRange";
            this.chkDateRange.Size = new System.Drawing.Size(117, 17);
            this.chkDateRange.TabIndex = 68;
            this.chkDateRange.Text = "Select Date Range";
            this.chkDateRange.UseVisualStyleBackColor = true;
            this.chkDateRange.CheckedChanged += new System.EventHandler(this.chkDateRange_CheckedChanged);
            // 
            // groupBox5
            // 
            this.groupBox5.BackColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox5.Controls.Add(this.cboMonths);
            this.groupBox5.Controls.Add(this.label2);
            this.groupBox5.Controls.Add(this.btnToDate);
            this.groupBox5.Controls.Add(this.btnFromDate);
            this.groupBox5.Controls.Add(this.txtFromDate);
            this.groupBox5.Controls.Add(this.txtToDate);
            this.groupBox5.Controls.Add(this.label1);
            this.groupBox5.Controls.Add(this.label3);
            this.groupBox5.Location = new System.Drawing.Point(6, 210);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(412, 69);
            this.groupBox5.TabIndex = 67;
            this.groupBox5.TabStop = false;
            // 
            // cboMonths
            // 
            this.cboMonths.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMonths.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.cboMonths.FocusLostColor = System.Drawing.Color.White;
            this.cboMonths.FormattingEnabled = true;
            this.cboMonths.Location = new System.Drawing.Point(83, 42);
            this.cboMonths.Name = "cboMonths";
            this.cboMonths.Size = new System.Drawing.Size(120, 21);
            this.cboMonths.TabIndex = 63;
            this.cboMonths.SelectedIndexChanged += new System.EventHandler(this.cboMonths_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 13);
            this.label2.TabIndex = 62;
            this.label2.Text = "End of Month";
            // 
            // btnToDate
            // 
            this.btnToDate.Image = global::Inventory.Properties.Resources.dateIcon;
            this.btnToDate.Location = new System.Drawing.Point(373, 13);
            this.btnToDate.Name = "btnToDate";
            this.btnToDate.Size = new System.Drawing.Size(26, 25);
            this.btnToDate.TabIndex = 9;
            this.btnToDate.UseVisualStyleBackColor = true;
            this.btnToDate.Click += new System.EventHandler(this.btnToDate_Click);
            // 
            // btnFromDate
            // 
            this.btnFromDate.Image = global::Inventory.Properties.Resources.dateIcon;
            this.btnFromDate.Location = new System.Drawing.Point(177, 13);
            this.btnFromDate.Name = "btnFromDate";
            this.btnFromDate.Size = new System.Drawing.Size(26, 25);
            this.btnFromDate.TabIndex = 8;
            this.btnFromDate.UseVisualStyleBackColor = true;
            this.btnFromDate.Click += new System.EventHandler(this.btnFromDate_Click);
            // 
            // txtFromDate
            // 
            this.txtFromDate.Location = new System.Drawing.Point(42, 16);
            this.txtFromDate.Mask = "##/##/####";
            this.txtFromDate.Name = "txtFromDate";
            this.txtFromDate.Size = new System.Drawing.Size(131, 20);
            this.txtFromDate.TabIndex = 6;
            // 
            // txtToDate
            // 
            this.txtToDate.Location = new System.Drawing.Point(246, 16);
            this.txtToDate.Mask = "##/##/####";
            this.txtToDate.Name = "txtToDate";
            this.txtToDate.Size = new System.Drawing.Size(121, 20);
            this.txtToDate.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(224, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(23, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "To:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(33, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "From:";
            // 
            // btnSelectAccClass
            // 
            this.btnSelectAccClass.Location = new System.Drawing.Point(246, 14);
            this.btnSelectAccClass.Name = "btnSelectAccClass";
            this.btnSelectAccClass.Size = new System.Drawing.Size(160, 23);
            this.btnSelectAccClass.TabIndex = 89;
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
            this.panel1.Location = new System.Drawing.Point(419, -1);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(92, 327);
            this.panel1.TabIndex = 90;
            // 
            // btnsavestate
            // 
            this.btnsavestate.Location = new System.Drawing.Point(8, 82);
            this.btnsavestate.Name = "btnsavestate";
            this.btnsavestate.Size = new System.Drawing.Size(76, 23);
            this.btnsavestate.TabIndex = 66;
            this.btnsavestate.Text = "Save State";
            this.btnsavestate.UseVisualStyleBackColor = true;
            this.btnsavestate.Click += new System.EventHandler(this.btnsavestate_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox2.Controls.Add(this.btnSelectAccClass);
            this.groupBox2.Location = new System.Drawing.Point(6, 281);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(412, 45);
            this.groupBox2.TabIndex = 91;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Accounting Class";
            // 
            // frmSalesReportSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(509, 330);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.chkDateRange);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.grpProduct);
            this.Controls.Add(this.grpParty);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "frmSalesReportSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Sales Report Settings";
            this.Load += new System.EventHandler(this.frmSalesReport_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.grpParty.ResumeLayout(false);
            this.grpParty.PerformLayout();
            this.grpProduct.ResumeLayout(false);
            this.grpProduct.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnShow;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rdParty;
        private System.Windows.Forms.RadioButton rdProduct;
        private System.Windows.Forms.GroupBox grpParty;
        private System.Windows.Forms.RadioButton rdPartyGroup;
        private System.Windows.Forms.RadioButton rdpartySingle;
        private System.Windows.Forms.RadioButton rdPartyAll;
        private SComboBox cboPartyGroup;
        private SComboBox cboPartySingle;
        private System.Windows.Forms.GroupBox grpProduct;
        private SComboBox cboProductGroup;
        private SComboBox cboProductSingle;
        private System.Windows.Forms.RadioButton rdProductGroup;
        private System.Windows.Forms.RadioButton rdProductSingle;
        private System.Windows.Forms.RadioButton rdProductAll;
        private System.Windows.Forms.GroupBox groupBox3;
        private SComboBox cboVoucherwise;
        private SComboBox cboDeotwise;
        private System.Windows.Forms.CheckBox chkVoucherwise;
        private System.Windows.Forms.CheckBox chkDepot;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.CheckBox chkProject;
        private SComboBox cboProjectName;
        private System.Windows.Forms.CheckBox chkSalesAccount;
        private SComboBox cboSalesAccount;
        private System.Windows.Forms.CheckBox chkDateRange;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.MaskedTextBox txtFromDate;
        private System.Windows.Forms.MaskedTextBox txtToDate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnSelectAccClass;
        private System.Windows.Forms.Button btnToDate;
        private System.Windows.Forms.Button btnFromDate;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnsavestate;
        private SComboBox cboMonths;
        private System.Windows.Forms.Label label2;
    }
}