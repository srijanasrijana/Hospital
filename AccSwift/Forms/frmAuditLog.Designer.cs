namespace AccSwift.Forms
{
    partial class frmAuditLog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAuditLog));
            this.label1 = new System.Windows.Forms.Label();
            this.cmbuser = new System.Windows.Forms.ComboBox();
            this.panelhead = new System.Windows.Forms.Panel();
            this.btnToDate = new System.Windows.Forms.Button();
            this.chkallusers = new System.Windows.Forms.CheckBox();
            this.btnFromDate = new System.Windows.Forms.Button();
            this.txtFromDate = new System.Windows.Forms.MaskedTextBox();
            this.txtToDate = new System.Windows.Forms.MaskedTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.panelbody = new System.Windows.Forms.Panel();
            this.tabControlauditlog = new System.Windows.Forms.TabControl();
            this.tabPagemaster = new System.Windows.Forms.TabPage();
            this.chkproductgroup = new System.Windows.Forms.CheckBox();
            this.chkdepot = new System.Windows.Forms.CheckBox();
            this.chkledger = new System.Windows.Forms.CheckBox();
            this.chkaccountgroup = new System.Windows.Forms.CheckBox();
            this.chkledgerconfiguration = new System.Windows.Forms.CheckBox();
            this.chkvoucherconfiguration = new System.Windows.Forms.CheckBox();
            this.chkproduct = new System.Windows.Forms.CheckBox();
            this.chkservice = new System.Windows.Forms.CheckBox();
            this.chkslab = new System.Windows.Forms.CheckBox();
            this.chkaccountclass = new System.Windows.Forms.CheckBox();
            this.tabPagetransaction = new System.Windows.Forms.TabPage();
            this.chkbulkvoucherposting = new System.Windows.Forms.CheckBox();
            this.chksalesorder = new System.Windows.Forms.CheckBox();
            this.chkdamageitems = new System.Windows.Forms.CheckBox();
            this.chksalesinvoice = new System.Windows.Forms.CheckBox();
            this.chkcashreceipt = new System.Windows.Forms.CheckBox();
            this.chkcontra = new System.Windows.Forms.CheckBox();
            this.chkpurchasereturn = new System.Windows.Forms.CheckBox();
            this.chksalesreturn = new System.Windows.Forms.CheckBox();
            this.chkstocktransfer = new System.Windows.Forms.CheckBox();
            this.chkbankreceipt = new System.Windows.Forms.CheckBox();
            this.chkcashpayment = new System.Windows.Forms.CheckBox();
            this.chkdebitnote = new System.Windows.Forms.CheckBox();
            this.chkbankpayment = new System.Windows.Forms.CheckBox();
            this.chkbankreconciliation = new System.Windows.Forms.CheckBox();
            this.chkcreditnote = new System.Windows.Forms.CheckBox();
            this.chkpurchaseinvoice = new System.Windows.Forms.CheckBox();
            this.chkpurchaseorder = new System.Windows.Forms.CheckBox();
            this.chkjournal = new System.Windows.Forms.CheckBox();
            this.panelfooter = new System.Windows.Forms.Panel();
            this.chkSelectAll = new System.Windows.Forms.CheckBox();
            this.btnShow = new System.Windows.Forms.Button();
            this.btnclose = new System.Windows.Forms.Button();
            this.panelhead.SuspendLayout();
            this.panelbody.SuspendLayout();
            this.tabControlauditlog.SuspendLayout();
            this.tabPagemaster.SuspendLayout();
            this.tabPagetransaction.SuspendLayout();
            this.panelfooter.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "User:";
            // 
            // cmbuser
            // 
            this.cmbuser.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbuser.Enabled = false;
            this.cmbuser.FormattingEnabled = true;
            this.cmbuser.Location = new System.Drawing.Point(49, 6);
            this.cmbuser.Name = "cmbuser";
            this.cmbuser.Size = new System.Drawing.Size(160, 21);
            this.cmbuser.TabIndex = 1;
            // 
            // panelhead
            // 
            this.panelhead.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.panelhead.Controls.Add(this.btnToDate);
            this.panelhead.Controls.Add(this.chkallusers);
            this.panelhead.Controls.Add(this.btnFromDate);
            this.panelhead.Controls.Add(this.txtFromDate);
            this.panelhead.Controls.Add(this.label1);
            this.panelhead.Controls.Add(this.txtToDate);
            this.panelhead.Controls.Add(this.cmbuser);
            this.panelhead.Controls.Add(this.label2);
            this.panelhead.Controls.Add(this.label3);
            this.panelhead.Location = new System.Drawing.Point(1, 3);
            this.panelhead.Name = "panelhead";
            this.panelhead.Size = new System.Drawing.Size(631, 55);
            this.panelhead.TabIndex = 2;
            this.panelhead.Paint += new System.Windows.Forms.PaintEventHandler(this.panelhead_Paint);
            // 
            // btnToDate
            // 
            this.btnToDate.Image = global::Inventory.Properties.Resources.dateIcon;
            this.btnToDate.Location = new System.Drawing.Point(575, 7);
            this.btnToDate.Name = "btnToDate";
            this.btnToDate.Size = new System.Drawing.Size(26, 23);
            this.btnToDate.TabIndex = 155;
            this.btnToDate.UseVisualStyleBackColor = true;
            this.btnToDate.Click += new System.EventHandler(this.btnToDate_Click);
            // 
            // chkallusers
            // 
            this.chkallusers.AutoSize = true;
            this.chkallusers.Checked = true;
            this.chkallusers.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkallusers.Location = new System.Drawing.Point(14, 33);
            this.chkallusers.Name = "chkallusers";
            this.chkallusers.Size = new System.Drawing.Size(67, 17);
            this.chkallusers.TabIndex = 2;
            this.chkallusers.Text = "All Users";
            this.chkallusers.UseVisualStyleBackColor = true;
            this.chkallusers.CheckedChanged += new System.EventHandler(this.chkboxallusers_CheckedChanged);
            // 
            // btnFromDate
            // 
            this.btnFromDate.Image = global::Inventory.Properties.Resources.dateIcon;
            this.btnFromDate.Location = new System.Drawing.Point(387, 6);
            this.btnFromDate.Name = "btnFromDate";
            this.btnFromDate.Size = new System.Drawing.Size(26, 23);
            this.btnFromDate.TabIndex = 154;
            this.btnFromDate.UseVisualStyleBackColor = true;
            this.btnFromDate.Click += new System.EventHandler(this.btnFromDate_Click);
            // 
            // txtFromDate
            // 
            this.txtFromDate.Location = new System.Drawing.Point(262, 7);
            this.txtFromDate.Mask = "##/##/####";
            this.txtFromDate.Name = "txtFromDate";
            this.txtFromDate.Size = new System.Drawing.Size(122, 20);
            this.txtFromDate.TabIndex = 152;
            // 
            // txtToDate
            // 
            this.txtToDate.Location = new System.Drawing.Point(446, 7);
            this.txtToDate.Mask = "##/##/####";
            this.txtToDate.Name = "txtToDate";
            this.txtToDate.Size = new System.Drawing.Size(123, 20);
            this.txtToDate.TabIndex = 153;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(223, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(33, 13);
            this.label2.TabIndex = 150;
            this.label2.Text = "From:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(421, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(23, 13);
            this.label3.TabIndex = 151;
            this.label3.Text = "To:";
            // 
            // panelbody
            // 
            this.panelbody.Controls.Add(this.tabControlauditlog);
            this.panelbody.Location = new System.Drawing.Point(1, 64);
            this.panelbody.Name = "panelbody";
            this.panelbody.Size = new System.Drawing.Size(631, 320);
            this.panelbody.TabIndex = 3;
            // 
            // tabControlauditlog
            // 
            this.tabControlauditlog.Controls.Add(this.tabPagemaster);
            this.tabControlauditlog.Controls.Add(this.tabPagetransaction);
            this.tabControlauditlog.Location = new System.Drawing.Point(3, 3);
            this.tabControlauditlog.Name = "tabControlauditlog";
            this.tabControlauditlog.SelectedIndex = 0;
            this.tabControlauditlog.Size = new System.Drawing.Size(622, 312);
            this.tabControlauditlog.TabIndex = 0;
            // 
            // tabPagemaster
            // 
            this.tabPagemaster.Controls.Add(this.chkproductgroup);
            this.tabPagemaster.Controls.Add(this.chkdepot);
            this.tabPagemaster.Controls.Add(this.chkledger);
            this.tabPagemaster.Controls.Add(this.chkaccountgroup);
            this.tabPagemaster.Controls.Add(this.chkledgerconfiguration);
            this.tabPagemaster.Controls.Add(this.chkvoucherconfiguration);
            this.tabPagemaster.Controls.Add(this.chkproduct);
            this.tabPagemaster.Controls.Add(this.chkservice);
            this.tabPagemaster.Controls.Add(this.chkslab);
            this.tabPagemaster.Controls.Add(this.chkaccountclass);
            this.tabPagemaster.Location = new System.Drawing.Point(4, 22);
            this.tabPagemaster.Name = "tabPagemaster";
            this.tabPagemaster.Padding = new System.Windows.Forms.Padding(3);
            this.tabPagemaster.Size = new System.Drawing.Size(614, 286);
            this.tabPagemaster.TabIndex = 0;
            this.tabPagemaster.Text = "Master";
            this.tabPagemaster.UseVisualStyleBackColor = true;
            // 
            // chkproductgroup
            // 
            this.chkproductgroup.AutoSize = true;
            this.chkproductgroup.Checked = true;
            this.chkproductgroup.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkproductgroup.Location = new System.Drawing.Point(439, 95);
            this.chkproductgroup.Name = "chkproductgroup";
            this.chkproductgroup.Size = new System.Drawing.Size(95, 17);
            this.chkproductgroup.TabIndex = 9;
            this.chkproductgroup.Text = "Product Group";
            this.chkproductgroup.UseVisualStyleBackColor = true;
            // 
            // chkdepot
            // 
            this.chkdepot.AutoSize = true;
            this.chkdepot.Checked = true;
            this.chkdepot.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkdepot.Location = new System.Drawing.Point(19, 130);
            this.chkdepot.Name = "chkdepot";
            this.chkdepot.Size = new System.Drawing.Size(55, 17);
            this.chkdepot.TabIndex = 8;
            this.chkdepot.Text = "Depot";
            this.chkdepot.UseVisualStyleBackColor = true;
            // 
            // chkledger
            // 
            this.chkledger.AutoSize = true;
            this.chkledger.Checked = true;
            this.chkledger.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkledger.Location = new System.Drawing.Point(439, 19);
            this.chkledger.Name = "chkledger";
            this.chkledger.Size = new System.Drawing.Size(59, 17);
            this.chkledger.TabIndex = 7;
            this.chkledger.Text = "Ledger";
            this.chkledger.UseVisualStyleBackColor = true;
            // 
            // chkaccountgroup
            // 
            this.chkaccountgroup.AutoSize = true;
            this.chkaccountgroup.Checked = true;
            this.chkaccountgroup.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkaccountgroup.Location = new System.Drawing.Point(255, 19);
            this.chkaccountgroup.Name = "chkaccountgroup";
            this.chkaccountgroup.Size = new System.Drawing.Size(98, 17);
            this.chkaccountgroup.TabIndex = 6;
            this.chkaccountgroup.Text = "Account Group";
            this.chkaccountgroup.UseVisualStyleBackColor = true;
            // 
            // chkledgerconfiguration
            // 
            this.chkledgerconfiguration.AutoSize = true;
            this.chkledgerconfiguration.Checked = true;
            this.chkledgerconfiguration.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkledgerconfiguration.Location = new System.Drawing.Point(253, 95);
            this.chkledgerconfiguration.Name = "chkledgerconfiguration";
            this.chkledgerconfiguration.Size = new System.Drawing.Size(124, 17);
            this.chkledgerconfiguration.TabIndex = 5;
            this.chkledgerconfiguration.Text = "Ledger Configuration";
            this.chkledgerconfiguration.UseVisualStyleBackColor = true;
            // 
            // chkvoucherconfiguration
            // 
            this.chkvoucherconfiguration.AutoSize = true;
            this.chkvoucherconfiguration.Checked = true;
            this.chkvoucherconfiguration.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkvoucherconfiguration.Location = new System.Drawing.Point(21, 95);
            this.chkvoucherconfiguration.Name = "chkvoucherconfiguration";
            this.chkvoucherconfiguration.Size = new System.Drawing.Size(131, 17);
            this.chkvoucherconfiguration.TabIndex = 4;
            this.chkvoucherconfiguration.Text = "Voucher Configuration";
            this.chkvoucherconfiguration.UseVisualStyleBackColor = true;
            // 
            // chkproduct
            // 
            this.chkproduct.AutoSize = true;
            this.chkproduct.Checked = true;
            this.chkproduct.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkproduct.Location = new System.Drawing.Point(439, 55);
            this.chkproduct.Name = "chkproduct";
            this.chkproduct.Size = new System.Drawing.Size(63, 17);
            this.chkproduct.TabIndex = 3;
            this.chkproduct.Text = "Product";
            this.chkproduct.UseVisualStyleBackColor = true;
            this.chkproduct.CheckedChanged += new System.EventHandler(this.chkproduct_CheckedChanged);
            // 
            // chkservice
            // 
            this.chkservice.AutoSize = true;
            this.chkservice.Checked = true;
            this.chkservice.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkservice.Location = new System.Drawing.Point(255, 55);
            this.chkservice.Name = "chkservice";
            this.chkservice.Size = new System.Drawing.Size(62, 17);
            this.chkservice.TabIndex = 2;
            this.chkservice.Text = "Service";
            this.chkservice.UseVisualStyleBackColor = true;
            // 
            // chkslab
            // 
            this.chkslab.AutoSize = true;
            this.chkslab.Checked = true;
            this.chkslab.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkslab.Location = new System.Drawing.Point(21, 55);
            this.chkslab.Name = "chkslab";
            this.chkslab.Size = new System.Drawing.Size(47, 17);
            this.chkslab.TabIndex = 1;
            this.chkslab.Text = "Slab";
            this.chkslab.UseVisualStyleBackColor = true;
            // 
            // chkaccountclass
            // 
            this.chkaccountclass.AutoSize = true;
            this.chkaccountclass.Checked = true;
            this.chkaccountclass.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkaccountclass.Location = new System.Drawing.Point(21, 19);
            this.chkaccountclass.Name = "chkaccountclass";
            this.chkaccountclass.Size = new System.Drawing.Size(94, 17);
            this.chkaccountclass.TabIndex = 0;
            this.chkaccountclass.Text = "Account Class";
            this.chkaccountclass.UseVisualStyleBackColor = true;
            // 
            // tabPagetransaction
            // 
            this.tabPagetransaction.Controls.Add(this.chkbulkvoucherposting);
            this.tabPagetransaction.Controls.Add(this.chksalesorder);
            this.tabPagetransaction.Controls.Add(this.chkdamageitems);
            this.tabPagetransaction.Controls.Add(this.chksalesinvoice);
            this.tabPagetransaction.Controls.Add(this.chkcashreceipt);
            this.tabPagetransaction.Controls.Add(this.chkcontra);
            this.tabPagetransaction.Controls.Add(this.chkpurchasereturn);
            this.tabPagetransaction.Controls.Add(this.chksalesreturn);
            this.tabPagetransaction.Controls.Add(this.chkstocktransfer);
            this.tabPagetransaction.Controls.Add(this.chkbankreceipt);
            this.tabPagetransaction.Controls.Add(this.chkcashpayment);
            this.tabPagetransaction.Controls.Add(this.chkdebitnote);
            this.tabPagetransaction.Controls.Add(this.chkbankpayment);
            this.tabPagetransaction.Controls.Add(this.chkbankreconciliation);
            this.tabPagetransaction.Controls.Add(this.chkcreditnote);
            this.tabPagetransaction.Controls.Add(this.chkpurchaseinvoice);
            this.tabPagetransaction.Controls.Add(this.chkpurchaseorder);
            this.tabPagetransaction.Controls.Add(this.chkjournal);
            this.tabPagetransaction.Location = new System.Drawing.Point(4, 22);
            this.tabPagetransaction.Name = "tabPagetransaction";
            this.tabPagetransaction.Padding = new System.Windows.Forms.Padding(3);
            this.tabPagetransaction.Size = new System.Drawing.Size(614, 286);
            this.tabPagetransaction.TabIndex = 1;
            this.tabPagetransaction.Text = "Transaction";
            this.tabPagetransaction.UseVisualStyleBackColor = true;
            // 
            // chkbulkvoucherposting
            // 
            this.chkbulkvoucherposting.AutoSize = true;
            this.chkbulkvoucherposting.Checked = true;
            this.chkbulkvoucherposting.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkbulkvoucherposting.Location = new System.Drawing.Point(449, 228);
            this.chkbulkvoucherposting.Name = "chkbulkvoucherposting";
            this.chkbulkvoucherposting.Size = new System.Drawing.Size(128, 17);
            this.chkbulkvoucherposting.TabIndex = 19;
            this.chkbulkvoucherposting.Text = "Bulk Voucher Posting";
            this.chkbulkvoucherposting.UseVisualStyleBackColor = true;
            // 
            // chksalesorder
            // 
            this.chksalesorder.AutoSize = true;
            this.chksalesorder.Checked = true;
            this.chksalesorder.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chksalesorder.Location = new System.Drawing.Point(242, 182);
            this.chksalesorder.Name = "chksalesorder";
            this.chksalesorder.Size = new System.Drawing.Size(81, 17);
            this.chksalesorder.TabIndex = 17;
            this.chksalesorder.Text = "Sales Order";
            this.chksalesorder.UseVisualStyleBackColor = true;
            // 
            // chkdamageitems
            // 
            this.chkdamageitems.AutoSize = true;
            this.chkdamageitems.Checked = true;
            this.chkdamageitems.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkdamageitems.Location = new System.Drawing.Point(242, 228);
            this.chkdamageitems.Name = "chkdamageitems";
            this.chkdamageitems.Size = new System.Drawing.Size(94, 17);
            this.chkdamageitems.TabIndex = 16;
            this.chkdamageitems.Text = "Damage Items";
            this.chkdamageitems.UseVisualStyleBackColor = true;
            // 
            // chksalesinvoice
            // 
            this.chksalesinvoice.AutoSize = true;
            this.chksalesinvoice.Checked = true;
            this.chksalesinvoice.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chksalesinvoice.Location = new System.Drawing.Point(449, 182);
            this.chksalesinvoice.Name = "chksalesinvoice";
            this.chksalesinvoice.Size = new System.Drawing.Size(90, 17);
            this.chksalesinvoice.TabIndex = 15;
            this.chksalesinvoice.Text = "Sales Invoice";
            this.chksalesinvoice.UseVisualStyleBackColor = true;
            // 
            // chkcashreceipt
            // 
            this.chkcashreceipt.AutoSize = true;
            this.chkcashreceipt.Checked = true;
            this.chkcashreceipt.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkcashreceipt.Location = new System.Drawing.Point(20, 58);
            this.chkcashreceipt.Name = "chkcashreceipt";
            this.chkcashreceipt.Size = new System.Drawing.Size(90, 17);
            this.chkcashreceipt.TabIndex = 14;
            this.chkcashreceipt.Text = "Cash Receipt";
            this.chkcashreceipt.UseVisualStyleBackColor = true;
            // 
            // chkcontra
            // 
            this.chkcontra.AutoSize = true;
            this.chkcontra.Checked = true;
            this.chkcontra.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkcontra.Location = new System.Drawing.Point(20, 101);
            this.chkcontra.Name = "chkcontra";
            this.chkcontra.Size = new System.Drawing.Size(57, 17);
            this.chkcontra.TabIndex = 13;
            this.chkcontra.Text = "Contra";
            this.chkcontra.UseVisualStyleBackColor = true;
            // 
            // chkpurchasereturn
            // 
            this.chkpurchasereturn.AutoSize = true;
            this.chkpurchasereturn.Checked = true;
            this.chkpurchasereturn.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkpurchasereturn.Location = new System.Drawing.Point(20, 137);
            this.chkpurchasereturn.Name = "chkpurchasereturn";
            this.chkpurchasereturn.Size = new System.Drawing.Size(106, 17);
            this.chkpurchasereturn.TabIndex = 12;
            this.chkpurchasereturn.Text = "Purchase Return";
            this.chkpurchasereturn.UseVisualStyleBackColor = true;
            // 
            // chksalesreturn
            // 
            this.chksalesreturn.AutoSize = true;
            this.chksalesreturn.Checked = true;
            this.chksalesreturn.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chksalesreturn.Location = new System.Drawing.Point(20, 182);
            this.chksalesreturn.Name = "chksalesreturn";
            this.chksalesreturn.Size = new System.Drawing.Size(87, 17);
            this.chksalesreturn.TabIndex = 11;
            this.chksalesreturn.Text = "Sales Return";
            this.chksalesreturn.UseVisualStyleBackColor = true;
            // 
            // chkstocktransfer
            // 
            this.chkstocktransfer.AutoSize = true;
            this.chkstocktransfer.Checked = true;
            this.chkstocktransfer.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkstocktransfer.Location = new System.Drawing.Point(20, 228);
            this.chkstocktransfer.Name = "chkstocktransfer";
            this.chkstocktransfer.Size = new System.Drawing.Size(96, 17);
            this.chkstocktransfer.TabIndex = 10;
            this.chkstocktransfer.Text = "Stock Transfer";
            this.chkstocktransfer.UseVisualStyleBackColor = true;
            // 
            // chkbankreceipt
            // 
            this.chkbankreceipt.AutoSize = true;
            this.chkbankreceipt.Checked = true;
            this.chkbankreceipt.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkbankreceipt.Location = new System.Drawing.Point(242, 18);
            this.chkbankreceipt.Name = "chkbankreceipt";
            this.chkbankreceipt.Size = new System.Drawing.Size(91, 17);
            this.chkbankreceipt.TabIndex = 8;
            this.chkbankreceipt.Text = "Bank Receipt";
            this.chkbankreceipt.UseVisualStyleBackColor = true;
            // 
            // chkcashpayment
            // 
            this.chkcashpayment.AutoSize = true;
            this.chkcashpayment.Checked = true;
            this.chkcashpayment.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkcashpayment.Location = new System.Drawing.Point(242, 58);
            this.chkcashpayment.Name = "chkcashpayment";
            this.chkcashpayment.Size = new System.Drawing.Size(94, 17);
            this.chkcashpayment.TabIndex = 7;
            this.chkcashpayment.Text = "Cash Payment";
            this.chkcashpayment.UseVisualStyleBackColor = true;
            // 
            // chkdebitnote
            // 
            this.chkdebitnote.AutoSize = true;
            this.chkdebitnote.Checked = true;
            this.chkdebitnote.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkdebitnote.Location = new System.Drawing.Point(242, 101);
            this.chkdebitnote.Name = "chkdebitnote";
            this.chkdebitnote.Size = new System.Drawing.Size(77, 17);
            this.chkdebitnote.TabIndex = 6;
            this.chkdebitnote.Text = "Debit Note";
            this.chkdebitnote.UseVisualStyleBackColor = true;
            // 
            // chkbankpayment
            // 
            this.chkbankpayment.AutoSize = true;
            this.chkbankpayment.Checked = true;
            this.chkbankpayment.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkbankpayment.Location = new System.Drawing.Point(449, 18);
            this.chkbankpayment.Name = "chkbankpayment";
            this.chkbankpayment.Size = new System.Drawing.Size(95, 17);
            this.chkbankpayment.TabIndex = 5;
            this.chkbankpayment.Text = "Bank Payment";
            this.chkbankpayment.UseVisualStyleBackColor = true;
            // 
            // chkbankreconciliation
            // 
            this.chkbankreconciliation.AutoSize = true;
            this.chkbankreconciliation.Checked = true;
            this.chkbankreconciliation.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkbankreconciliation.Location = new System.Drawing.Point(449, 58);
            this.chkbankreconciliation.Name = "chkbankreconciliation";
            this.chkbankreconciliation.Size = new System.Drawing.Size(121, 17);
            this.chkbankreconciliation.TabIndex = 4;
            this.chkbankreconciliation.Text = "Bank Reconciliation";
            this.chkbankreconciliation.UseVisualStyleBackColor = true;
            this.chkbankreconciliation.CheckedChanged += new System.EventHandler(this.checkBox14_CheckedChanged);
            // 
            // chkcreditnote
            // 
            this.chkcreditnote.AutoSize = true;
            this.chkcreditnote.Checked = true;
            this.chkcreditnote.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkcreditnote.Location = new System.Drawing.Point(449, 101);
            this.chkcreditnote.Name = "chkcreditnote";
            this.chkcreditnote.Size = new System.Drawing.Size(79, 17);
            this.chkcreditnote.TabIndex = 3;
            this.chkcreditnote.Text = "Credit Note";
            this.chkcreditnote.UseVisualStyleBackColor = true;
            // 
            // chkpurchaseinvoice
            // 
            this.chkpurchaseinvoice.AutoSize = true;
            this.chkpurchaseinvoice.Checked = true;
            this.chkpurchaseinvoice.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkpurchaseinvoice.Location = new System.Drawing.Point(449, 137);
            this.chkpurchaseinvoice.Name = "chkpurchaseinvoice";
            this.chkpurchaseinvoice.Size = new System.Drawing.Size(109, 17);
            this.chkpurchaseinvoice.TabIndex = 2;
            this.chkpurchaseinvoice.Text = "Purchase Invoice";
            this.chkpurchaseinvoice.UseVisualStyleBackColor = true;
            this.chkpurchaseinvoice.CheckedChanged += new System.EventHandler(this.chkpurchaseinvoice_CheckedChanged);
            // 
            // chkpurchaseorder
            // 
            this.chkpurchaseorder.AutoSize = true;
            this.chkpurchaseorder.Checked = true;
            this.chkpurchaseorder.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkpurchaseorder.Location = new System.Drawing.Point(242, 137);
            this.chkpurchaseorder.Name = "chkpurchaseorder";
            this.chkpurchaseorder.Size = new System.Drawing.Size(100, 17);
            this.chkpurchaseorder.TabIndex = 1;
            this.chkpurchaseorder.Text = "Purchase Order";
            this.chkpurchaseorder.UseVisualStyleBackColor = true;
            // 
            // chkjournal
            // 
            this.chkjournal.AutoSize = true;
            this.chkjournal.Checked = true;
            this.chkjournal.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkjournal.Location = new System.Drawing.Point(20, 18);
            this.chkjournal.Name = "chkjournal";
            this.chkjournal.Size = new System.Drawing.Size(60, 17);
            this.chkjournal.TabIndex = 0;
            this.chkjournal.Text = "Journal";
            this.chkjournal.UseVisualStyleBackColor = true;
            this.chkjournal.CheckedChanged += new System.EventHandler(this.chkjournal_CheckedChanged);
            // 
            // panelfooter
            // 
            this.panelfooter.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.panelfooter.Controls.Add(this.chkSelectAll);
            this.panelfooter.Controls.Add(this.btnShow);
            this.panelfooter.Controls.Add(this.btnclose);
            this.panelfooter.Location = new System.Drawing.Point(1, 385);
            this.panelfooter.Name = "panelfooter";
            this.panelfooter.Size = new System.Drawing.Size(631, 40);
            this.panelfooter.TabIndex = 4;
            this.panelfooter.Paint += new System.Windows.Forms.PaintEventHandler(this.panelfooter_Paint);
            // 
            // chkSelectAll
            // 
            this.chkSelectAll.AutoSize = true;
            this.chkSelectAll.Checked = true;
            this.chkSelectAll.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSelectAll.Location = new System.Drawing.Point(14, 13);
            this.chkSelectAll.Name = "chkSelectAll";
            this.chkSelectAll.Size = new System.Drawing.Size(70, 17);
            this.chkSelectAll.TabIndex = 4;
            this.chkSelectAll.Text = "Select All";
            this.chkSelectAll.UseVisualStyleBackColor = true;
            this.chkSelectAll.CheckedChanged += new System.EventHandler(this.chkSelectAll_CheckedChanged);
            // 
            // btnShow
            // 
            this.btnShow.Image = global::Inventory.Properties.Resources.save;
            this.btnShow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnShow.Location = new System.Drawing.Point(456, 7);
            this.btnShow.Name = "btnShow";
            this.btnShow.Size = new System.Drawing.Size(75, 29);
            this.btnShow.TabIndex = 2;
            this.btnShow.Text = "&Show";
            this.btnShow.UseVisualStyleBackColor = true;
            this.btnShow.Click += new System.EventHandler(this.btnShow_Click);
            // 
            // btnclose
            // 
            this.btnclose.Image = global::Inventory.Properties.Resources.ExitButton;
            this.btnclose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnclose.Location = new System.Drawing.Point(548, 6);
            this.btnclose.Name = "btnclose";
            this.btnclose.Size = new System.Drawing.Size(75, 29);
            this.btnclose.TabIndex = 1;
            this.btnclose.Text = "&Close";
            this.btnclose.UseVisualStyleBackColor = true;
            this.btnclose.Click += new System.EventHandler(this.btnclose_Click);
            // 
            // frmAuditLog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(636, 426);
            this.Controls.Add(this.panelfooter);
            this.Controls.Add(this.panelbody);
            this.Controls.Add(this.panelhead);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmAuditLog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Audit Log";
            this.Load += new System.EventHandler(this.frmAuditLog_Load);
            this.panelhead.ResumeLayout(false);
            this.panelhead.PerformLayout();
            this.panelbody.ResumeLayout(false);
            this.tabControlauditlog.ResumeLayout(false);
            this.tabPagemaster.ResumeLayout(false);
            this.tabPagemaster.PerformLayout();
            this.tabPagetransaction.ResumeLayout(false);
            this.tabPagetransaction.PerformLayout();
            this.panelfooter.ResumeLayout(false);
            this.panelfooter.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbuser;
        private System.Windows.Forms.Panel panelhead;
        private System.Windows.Forms.CheckBox chkallusers;
        private System.Windows.Forms.Panel panelbody;
        private System.Windows.Forms.Button btnToDate;
        private System.Windows.Forms.Button btnFromDate;
        private System.Windows.Forms.MaskedTextBox txtFromDate;
        private System.Windows.Forms.MaskedTextBox txtToDate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panelfooter;
        private System.Windows.Forms.Button btnShow;
        private System.Windows.Forms.Button btnclose;
        private System.Windows.Forms.TabControl tabControlauditlog;
        private System.Windows.Forms.TabPage tabPagemaster;
        private System.Windows.Forms.TabPage tabPagetransaction;
        private System.Windows.Forms.CheckBox chkvoucherconfiguration;
        private System.Windows.Forms.CheckBox chkproduct;
        private System.Windows.Forms.CheckBox chkservice;
        private System.Windows.Forms.CheckBox chkslab;
        private System.Windows.Forms.CheckBox chkaccountclass;
        private System.Windows.Forms.CheckBox chkledgerconfiguration;
        private System.Windows.Forms.CheckBox chkledger;
        private System.Windows.Forms.CheckBox chkaccountgroup;
        private System.Windows.Forms.CheckBox chkdepot;
        private System.Windows.Forms.CheckBox chkbulkvoucherposting;
        private System.Windows.Forms.CheckBox chksalesorder;
        private System.Windows.Forms.CheckBox chkdamageitems;
        private System.Windows.Forms.CheckBox chksalesinvoice;
        private System.Windows.Forms.CheckBox chkcashreceipt;
        private System.Windows.Forms.CheckBox chkcontra;
        private System.Windows.Forms.CheckBox chkpurchasereturn;
        private System.Windows.Forms.CheckBox chksalesreturn;
        private System.Windows.Forms.CheckBox chkstocktransfer;
        private System.Windows.Forms.CheckBox chkbankreceipt;
        private System.Windows.Forms.CheckBox chkcashpayment;
        private System.Windows.Forms.CheckBox chkdebitnote;
        private System.Windows.Forms.CheckBox chkbankpayment;
        private System.Windows.Forms.CheckBox chkbankreconciliation;
        private System.Windows.Forms.CheckBox chkcreditnote;
        private System.Windows.Forms.CheckBox chkpurchaseinvoice;
        private System.Windows.Forms.CheckBox chkpurchaseorder;
        private System.Windows.Forms.CheckBox chkjournal;
        private System.Windows.Forms.CheckBox chkproductgroup;
        private System.Windows.Forms.CheckBox chkSelectAll;
    }
}