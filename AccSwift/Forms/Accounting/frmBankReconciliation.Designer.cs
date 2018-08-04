using SComponents;
namespace Inventory
{
    partial class frmBankReconciliation
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmBankReconciliation));
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.chkPrintWhileSaving = new System.Windows.Forms.CheckBox();
            this.label11 = new System.Windows.Forms.Label();
            this.lblBankStatementBalance = new System.Windows.Forms.Label();
            this.lblBankReconcilationDiff = new System.Windows.Forms.Label();
            this.btnAddAccClass = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.label16 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbtnpayments = new System.Windows.Forms.RadioButton();
            this.rbtnreceived = new System.Windows.Forms.RadioButton();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.grdAdjustment = new SourceGrid.Grid();
            this.label10 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.txtVchNo = new STextBox();
            this.cboSeriesName = new SComboBox();
            this.txtDate = new SMaskedTextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.treeAccClass = new STreeView(this.components);
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.button5 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnSelectAccClass = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.btnDate = new System.Windows.Forms.Button();
            this.txtFromDate = new System.Windows.Forms.MaskedTextBox();
            this.txtToDate = new System.Windows.Forms.MaskedTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtBankStatementBalance = new STextBox();
            this.btnShow = new System.Windows.Forms.Button();
            this.grdBankReconciliation = new SourceGrid.Grid();
            this.Banks1 = new System.Windows.Forms.Label();
            this.cmboBanks = new SComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cmboParty = new SComboBox();
            this.grpSelect = new System.Windows.Forms.GroupBox();
            this.rbtnPayment = new System.Windows.Forms.RadioButton();
            this.rbtnReceipt = new System.Windows.Forms.RadioButton();
            this.rbtnAll = new System.Windows.Forms.RadioButton();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.txtfifth = new STextBox();
            this.txtfourth = new STextBox();
            this.txtthird = new STextBox();
            this.txtsecond = new STextBox();
            this.txtfirst = new STextBox();
            this.lblfourth = new System.Windows.Forms.Label();
            this.lblsecond = new System.Windows.Forms.Label();
            this.lblthird = new System.Windows.Forms.Label();
            this.lblfifth = new System.Windows.Forms.Label();
            this.lblfirst = new System.Windows.Forms.Label();
            this.tabPage2.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.grpSelect.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(26, 348);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(46, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Cleared:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(27, 397);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(171, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "Balance Reconciliation Difference:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(26, 368);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(137, 13);
            this.label7.TabIndex = 10;
            this.label7.Text = "Balance as per Bank Book:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(223, 348);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(46, 13);
            this.label8.TabIndex = 10;
            this.label8.Text = "Cleared:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(376, 348);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(47, 13);
            this.label9.TabIndex = 10;
            this.label9.Text = "Bounce:";
            // 
            // chkPrintWhileSaving
            // 
            this.chkPrintWhileSaving.AutoSize = true;
            this.chkPrintWhileSaving.Location = new System.Drawing.Point(326, 219);
            this.chkPrintWhileSaving.Name = "chkPrintWhileSaving";
            this.chkPrintWhileSaving.Size = new System.Drawing.Size(113, 17);
            this.chkPrintWhileSaving.TabIndex = 11;
            this.chkPrintWhileSaving.Text = "Print While Saving";
            this.chkPrintWhileSaving.UseVisualStyleBackColor = true;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(7, 7);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(121, 13);
            this.label11.TabIndex = 38;
            this.label11.Text = "Opening Balance as on:";
            // 
            // lblBankStatementBalance
            // 
            this.lblBankStatementBalance.AutoSize = true;
            this.lblBankStatementBalance.Location = new System.Drawing.Point(213, 368);
            this.lblBankStatementBalance.Name = "lblBankStatementBalance";
            this.lblBankStatementBalance.Size = new System.Drawing.Size(28, 13);
            this.lblBankStatementBalance.TabIndex = 45;
            this.lblBankStatementBalance.Text = "0.00";
            // 
            // lblBankReconcilationDiff
            // 
            this.lblBankReconcilationDiff.AutoSize = true;
            this.lblBankReconcilationDiff.Location = new System.Drawing.Point(213, 398);
            this.lblBankReconcilationDiff.Name = "lblBankReconcilationDiff";
            this.lblBankReconcilationDiff.Size = new System.Drawing.Size(28, 13);
            this.lblBankReconcilationDiff.TabIndex = 46;
            this.lblBankReconcilationDiff.Text = "0.00";
            // 
            // btnAddAccClass
            // 
            this.btnAddAccClass.Location = new System.Drawing.Point(381, 9);
            this.btnAddAccClass.Name = "btnAddAccClass";
            this.btnAddAccClass.Size = new System.Drawing.Size(109, 23);
            this.btnAddAccClass.TabIndex = 34;
            this.btnAddAccClass.Text = "Add Account Class";
            this.btnAddAccClass.UseVisualStyleBackColor = true;
            this.btnAddAccClass.Click += new System.EventHandler(this.btnAddAccClass_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.tabControl2);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(722, 292);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Adjustment";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabPage4);
            this.tabControl2.Controls.Add(this.tabPage5);
            this.tabControl2.Controls.Add(this.tabPage3);
            this.tabControl2.Location = new System.Drawing.Point(2, 6);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(710, 265);
            this.tabControl2.TabIndex = 0;
            this.tabControl2.SelectedIndexChanged += new System.EventHandler(this.tabControl2_SelectedIndexChanged);
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.label16);
            this.tabPage4.Controls.Add(this.groupBox1);
            this.tabPage4.Controls.Add(this.label15);
            this.tabPage4.Controls.Add(this.label14);
            this.tabPage4.Controls.Add(this.btnCancel);
            this.tabPage4.Controls.Add(this.button2);
            this.tabPage4.Controls.Add(this.grdAdjustment);
            this.tabPage4.Controls.Add(this.label10);
            this.tabPage4.Controls.Add(this.button3);
            this.tabPage4.Controls.Add(this.txtVchNo);
            this.tabPage4.Controls.Add(this.cboSeriesName);
            this.tabPage4.Controls.Add(this.txtDate);
            this.tabPage4.Controls.Add(this.btnSave);
            this.tabPage4.Controls.Add(this.label13);
            this.tabPage4.Controls.Add(this.label12);
            this.tabPage4.Controls.Add(this.chkPrintWhileSaving);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(702, 239);
            this.tabPage4.TabIndex = 0;
            this.tabPage4.Text = "Voucher";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(150, 13);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(44, 13);
            this.label16.TabIndex = 152;
            this.label16.Text = "ledgerid";
            this.label16.Visible = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbtnpayments);
            this.groupBox1.Controls.Add(this.rbtnreceived);
            this.groupBox1.Location = new System.Drawing.Point(337, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(345, 37);
            this.groupBox1.TabIndex = 151;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Bank Info";
            // 
            // rbtnpayments
            // 
            this.rbtnpayments.AutoSize = true;
            this.rbtnpayments.Location = new System.Drawing.Point(269, 13);
            this.rbtnpayments.Name = "rbtnpayments";
            this.rbtnpayments.Size = new System.Drawing.Size(66, 17);
            this.rbtnpayments.TabIndex = 1;
            this.rbtnpayments.TabStop = true;
            this.rbtnpayments.Text = "Payment";
            this.rbtnpayments.UseVisualStyleBackColor = true;
            // 
            // rbtnreceived
            // 
            this.rbtnreceived.AutoSize = true;
            this.rbtnreceived.Location = new System.Drawing.Point(171, 13);
            this.rbtnreceived.Name = "rbtnreceived";
            this.rbtnreceived.Size = new System.Drawing.Size(62, 17);
            this.rbtnreceived.TabIndex = 0;
            this.rbtnreceived.TabStop = true;
            this.rbtnreceived.Text = "Receipt";
            this.rbtnreceived.UseVisualStyleBackColor = true;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(59, 13);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(71, 13);
            this.label15.TabIndex = 150;
            this.label15.Text = "Ledger Name";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(11, 13);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(38, 13);
            this.label14.TabIndex = 149;
            this.label14.Text = "Bank :";
            // 
            // btnCancel
            // 
            this.btnCancel.Image = global::Inventory.Properties.Resources.gnome_window_close;
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(612, 213);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(77, 23);
            this.btnCancel.TabIndex = 37;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Image = global::Inventory.Properties.Resources.dateIcon;
            this.button2.Location = new System.Drawing.Point(656, 42);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(26, 23);
            this.button2.TabIndex = 148;
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // grdAdjustment
            // 
            this.grdAdjustment.Location = new System.Drawing.Point(6, 79);
            this.grdAdjustment.Name = "grdAdjustment";
            this.grdAdjustment.OptimizeMode = SourceGrid.CellOptimizeMode.ForRows;
            this.grdAdjustment.SelectionMode = SourceGrid.GridSelectionMode.Cell;
            this.grdAdjustment.Size = new System.Drawing.Size(690, 125);
            this.grdAdjustment.TabIndex = 0;
            this.grdAdjustment.TabStop = true;
            this.grdAdjustment.ToolTipText = "";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 51);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(42, 13);
            this.label10.TabIndex = 47;
            this.label10.Text = "Series :";
            // 
            // button3
            // 
            this.button3.Image = global::Inventory.Properties.Resources.print;
            this.button3.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button3.Location = new System.Drawing.Point(535, 213);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(71, 23);
            this.button3.TabIndex = 35;
            this.button3.Text = "Print";
            this.button3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // txtVchNo
            // 
            this.txtVchNo.BackColor = System.Drawing.Color.White;
            this.txtVchNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtVchNo.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtVchNo.FocusLostColor = System.Drawing.Color.White;
            this.txtVchNo.Location = new System.Drawing.Point(337, 48);
            this.txtVchNo.Name = "txtVchNo";
            this.txtVchNo.Size = new System.Drawing.Size(135, 20);
            this.txtVchNo.TabIndex = 49;
            // 
            // cboSeriesName
            // 
            this.cboSeriesName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboSeriesName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboSeriesName.BackColor = System.Drawing.Color.White;
            this.cboSeriesName.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.cboSeriesName.FocusLostColor = System.Drawing.Color.White;
            this.cboSeriesName.FormattingEnabled = true;
            this.cboSeriesName.Location = new System.Drawing.Point(62, 48);
            this.cboSeriesName.Name = "cboSeriesName";
            this.cboSeriesName.Size = new System.Drawing.Size(159, 21);
            this.cboSeriesName.TabIndex = 48;
            this.cboSeriesName.SelectedIndexChanged += new System.EventHandler(this.cboSeriesName_SelectedIndexChanged);
            this.cboSeriesName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cboSeriesName_KeyDown);
            // 
            // txtDate
            // 
            this.txtDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDate.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.txtDate.FocusLostColor = System.Drawing.Color.White;
            this.txtDate.Location = new System.Drawing.Point(576, 45);
            this.txtDate.Mask = "0000/00/00";
            this.txtDate.Name = "txtDate";
            this.txtDate.Size = new System.Drawing.Size(74, 20);
            this.txtDate.TabIndex = 40;
            // 
            // btnSave
            // 
            this.btnSave.Image = global::Inventory.Properties.Resources.save;
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSave.Location = new System.Drawing.Point(454, 213);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 34;
            this.btnSave.Text = "&Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(257, 48);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(60, 13);
            this.label13.TabIndex = 50;
            this.label13.Text = "Voucher #:";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(513, 48);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(33, 13);
            this.label12.TabIndex = 41;
            this.label12.Text = "Date:";
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.btnAddAccClass);
            this.tabPage5.Controls.Add(this.treeAccClass);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(702, 239);
            this.tabPage5.TabIndex = 1;
            this.tabPage5.Text = "Account Class";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // treeAccClass
            // 
            this.treeAccClass.AutoCheckChild = true;
            this.treeAccClass.CheckBoxes = true;
            this.treeAccClass.Location = new System.Drawing.Point(3, 6);
            this.treeAccClass.Name = "treeAccClass";
            this.treeAccClass.Size = new System.Drawing.Size(372, 139);
            this.treeAccClass.TabIndex = 33;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.button5);
            this.tabPage1.Controls.Add(this.button4);
            this.tabPage1.Controls.Add(this.panel1);
            this.tabPage1.Controls.Add(this.grdBankReconciliation);
            this.tabPage1.Controls.Add(this.Banks1);
            this.tabPage1.Controls.Add(this.cmboBanks);
            this.tabPage1.Controls.Add(this.label5);
            this.tabPage1.Controls.Add(this.cmboParty);
            this.tabPage1.Controls.Add(this.grpSelect);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(722, 292);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Statement";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // button5
            // 
            this.button5.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button5.Location = new System.Drawing.Point(556, 266);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(119, 23);
            this.button5.TabIndex = 36;
            this.button5.Text = "&Retrieve";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button4
            // 
            this.button4.Image = global::Inventory.Properties.Resources.save;
            this.button4.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button4.Location = new System.Drawing.Point(441, 266);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(109, 23);
            this.button4.TabIndex = 35;
            this.button4.Text = "&Save State";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnSelectAccClass);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.btnDate);
            this.panel1.Controls.Add(this.txtFromDate);
            this.panel1.Controls.Add(this.txtToDate);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.txtBankStatementBalance);
            this.panel1.Controls.Add(this.btnShow);
            this.panel1.Location = new System.Drawing.Point(323, 57);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(396, 92);
            this.panel1.TabIndex = 10;
            // 
            // btnSelectAccClass
            // 
            this.btnSelectAccClass.Location = new System.Drawing.Point(9, 64);
            this.btnSelectAccClass.Name = "btnSelectAccClass";
            this.btnSelectAccClass.Size = new System.Drawing.Size(120, 23);
            this.btnSelectAccClass.TabIndex = 11;
            this.btnSelectAccClass.Text = "Select Account Class";
            this.btnSelectAccClass.UseVisualStyleBackColor = true;
            this.btnSelectAccClass.Click += new System.EventHandler(this.btnSelectAccClass_Click_1);
            // 
            // button1
            // 
            this.button1.Image = global::Inventory.Properties.Resources.dateIcon;
            this.button1.Location = new System.Drawing.Point(358, 8);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(26, 23);
            this.button1.TabIndex = 149;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnDate
            // 
            this.btnDate.Image = global::Inventory.Properties.Resources.dateIcon;
            this.btnDate.Location = new System.Drawing.Point(170, 7);
            this.btnDate.Name = "btnDate";
            this.btnDate.Size = new System.Drawing.Size(26, 23);
            this.btnDate.TabIndex = 148;
            this.btnDate.UseVisualStyleBackColor = true;
            this.btnDate.Click += new System.EventHandler(this.btnDate_Click);
            // 
            // txtFromDate
            // 
            this.txtFromDate.Location = new System.Drawing.Point(42, 10);
            this.txtFromDate.Mask = "##/##/####";
            this.txtFromDate.Name = "txtFromDate";
            this.txtFromDate.Size = new System.Drawing.Size(122, 20);
            this.txtFromDate.TabIndex = 43;
            // 
            // txtToDate
            // 
            this.txtToDate.Location = new System.Drawing.Point(229, 8);
            this.txtToDate.Mask = "##/##/####";
            this.txtToDate.Name = "txtToDate";
            this.txtToDate.Size = new System.Drawing.Size(123, 20);
            this.txtToDate.TabIndex = 44;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "From:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(204, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(23, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "To:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 48);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(157, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Balance as per Bank Statement";
            // 
            // txtBankStatementBalance
            // 
            this.txtBankStatementBalance.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBankStatementBalance.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtBankStatementBalance.FocusLostColor = System.Drawing.Color.White;
            this.txtBankStatementBalance.Location = new System.Drawing.Point(173, 44);
            this.txtBankStatementBalance.Name = "txtBankStatementBalance";
            this.txtBankStatementBalance.Size = new System.Drawing.Size(137, 20);
            this.txtBankStatementBalance.TabIndex = 7;
            // 
            // btnShow
            // 
            this.btnShow.Location = new System.Drawing.Point(316, 44);
            this.btnShow.Name = "btnShow";
            this.btnShow.Size = new System.Drawing.Size(63, 23);
            this.btnShow.TabIndex = 8;
            this.btnShow.Text = "Show";
            this.btnShow.UseVisualStyleBackColor = true;
            this.btnShow.Click += new System.EventHandler(this.btnShow_Click);
            // 
            // grdBankReconciliation
            // 
            this.grdBankReconciliation.Location = new System.Drawing.Point(3, 155);
            this.grdBankReconciliation.Name = "grdBankReconciliation";
            this.grdBankReconciliation.OptimizeMode = SourceGrid.CellOptimizeMode.ForRows;
            this.grdBankReconciliation.SelectionMode = SourceGrid.GridSelectionMode.Cell;
            this.grdBankReconciliation.Size = new System.Drawing.Size(710, 105);
            this.grdBankReconciliation.TabIndex = 0;
            this.grdBankReconciliation.TabStop = true;
            this.grdBankReconciliation.ToolTipText = "";
           // this.grdBankReconciliation.Paint += new System.Windows.Forms.PaintEventHandler(this.grdBankReconciliation_Paint);
            // 
            // Banks1
            // 
            this.Banks1.AutoSize = true;
            this.Banks1.Location = new System.Drawing.Point(6, 11);
            this.Banks1.Name = "Banks1";
            this.Banks1.Size = new System.Drawing.Size(37, 13);
            this.Banks1.TabIndex = 9;
            this.Banks1.Text = "Banks";
            // 
            // cmboBanks
            // 
            this.cmboBanks.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.cmboBanks.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmboBanks.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.cmboBanks.FocusLostColor = System.Drawing.Color.White;
            this.cmboBanks.FormattingEnabled = true;
            this.cmboBanks.Location = new System.Drawing.Point(64, 8);
            this.cmboBanks.Name = "cmboBanks";
            this.cmboBanks.Size = new System.Drawing.Size(219, 21);
            this.cmboBanks.TabIndex = 0;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 52);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(31, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Party";
            // 
            // cmboParty
            // 
            this.cmboParty.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmboParty.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.cmboParty.FocusLostColor = System.Drawing.Color.White;
            this.cmboParty.FormattingEnabled = true;
            this.cmboParty.Location = new System.Drawing.Point(64, 52);
            this.cmboParty.Name = "cmboParty";
            this.cmboParty.Size = new System.Drawing.Size(219, 21);
            this.cmboParty.TabIndex = 3;
            // 
            // grpSelect
            // 
            this.grpSelect.Controls.Add(this.rbtnPayment);
            this.grpSelect.Controls.Add(this.rbtnReceipt);
            this.grpSelect.Controls.Add(this.rbtnAll);
            this.grpSelect.Location = new System.Drawing.Point(323, 3);
            this.grpSelect.Name = "grpSelect";
            this.grpSelect.Size = new System.Drawing.Size(396, 48);
            this.grpSelect.TabIndex = 2;
            this.grpSelect.TabStop = false;
            this.grpSelect.Text = "Select";
            this.grpSelect.Enter += new System.EventHandler(this.grpSelect_Enter);
            // 
            // rbtnPayment
            // 
            this.rbtnPayment.AutoSize = true;
            this.rbtnPayment.Location = new System.Drawing.Point(249, 19);
            this.rbtnPayment.Name = "rbtnPayment";
            this.rbtnPayment.Size = new System.Drawing.Size(71, 17);
            this.rbtnPayment.TabIndex = 0;
            this.rbtnPayment.Text = "Payments";
            this.rbtnPayment.UseVisualStyleBackColor = true;
            // 
            // rbtnReceipt
            // 
            this.rbtnReceipt.AutoSize = true;
            this.rbtnReceipt.Location = new System.Drawing.Point(131, 19);
            this.rbtnReceipt.Name = "rbtnReceipt";
            this.rbtnReceipt.Size = new System.Drawing.Size(62, 17);
            this.rbtnReceipt.TabIndex = 0;
            this.rbtnReceipt.Text = "Receipt";
            this.rbtnReceipt.UseVisualStyleBackColor = true;
            // 
            // rbtnAll
            // 
            this.rbtnAll.AutoSize = true;
            this.rbtnAll.Checked = true;
            this.rbtnAll.Location = new System.Drawing.Point(24, 19);
            this.rbtnAll.Name = "rbtnAll";
            this.rbtnAll.Size = new System.Drawing.Size(36, 17);
            this.rbtnAll.TabIndex = 0;
            this.rbtnAll.TabStop = true;
            this.rbtnAll.Text = "All";
            this.rbtnAll.UseVisualStyleBackColor = true;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(10, 26);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(730, 318);
            this.tabControl1.TabIndex = 39;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.txtfifth);
            this.tabPage3.Controls.Add(this.txtfourth);
            this.tabPage3.Controls.Add(this.txtthird);
            this.tabPage3.Controls.Add(this.txtsecond);
            this.tabPage3.Controls.Add(this.txtfirst);
            this.tabPage3.Controls.Add(this.lblfourth);
            this.tabPage3.Controls.Add(this.lblsecond);
            this.tabPage3.Controls.Add(this.lblthird);
            this.tabPage3.Controls.Add(this.lblfifth);
            this.tabPage3.Controls.Add(this.lblfirst);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(702, 239);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Additional Fields";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // txtfifth
            // 
            this.txtfifth.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtfifth.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtfifth.FocusLostColor = System.Drawing.Color.White;
            this.txtfifth.Location = new System.Drawing.Point(107, 169);
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
            this.txtfourth.Location = new System.Drawing.Point(107, 130);
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
            this.txtthird.Location = new System.Drawing.Point(107, 92);
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
            this.txtsecond.Location = new System.Drawing.Point(107, 52);
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
            this.txtfirst.Location = new System.Drawing.Point(107, 18);
            this.txtfirst.Name = "txtfirst";
            this.txtfirst.Size = new System.Drawing.Size(182, 20);
            this.txtfirst.TabIndex = 36;
            this.txtfirst.Visible = false;
            // 
            // lblfourth
            // 
            this.lblfourth.AutoSize = true;
            this.lblfourth.Location = new System.Drawing.Point(7, 136);
            this.lblfourth.Name = "lblfourth";
            this.lblfourth.Size = new System.Drawing.Size(80, 13);
            this.lblfourth.TabIndex = 35;
            this.lblfourth.Text = "Optional Field 4";
            this.lblfourth.Visible = false;
            // 
            // lblsecond
            // 
            this.lblsecond.AutoSize = true;
            this.lblsecond.Location = new System.Drawing.Point(7, 58);
            this.lblsecond.Name = "lblsecond";
            this.lblsecond.Size = new System.Drawing.Size(80, 13);
            this.lblsecond.TabIndex = 34;
            this.lblsecond.Text = "Optional Field 2";
            this.lblsecond.Visible = false;
            // 
            // lblthird
            // 
            this.lblthird.AutoSize = true;
            this.lblthird.Location = new System.Drawing.Point(7, 97);
            this.lblthird.Name = "lblthird";
            this.lblthird.Size = new System.Drawing.Size(80, 13);
            this.lblthird.TabIndex = 33;
            this.lblthird.Text = "Optional Field 3";
            this.lblthird.Visible = false;
            // 
            // lblfifth
            // 
            this.lblfifth.AutoSize = true;
            this.lblfifth.Location = new System.Drawing.Point(7, 175);
            this.lblfifth.Name = "lblfifth";
            this.lblfifth.Size = new System.Drawing.Size(80, 13);
            this.lblfifth.TabIndex = 32;
            this.lblfifth.Text = "Optional Field 5";
            this.lblfifth.Visible = false;
            // 
            // lblfirst
            // 
            this.lblfirst.AutoSize = true;
            this.lblfirst.Location = new System.Drawing.Point(7, 19);
            this.lblfirst.Name = "lblfirst";
            this.lblfirst.Size = new System.Drawing.Size(80, 13);
            this.lblfirst.TabIndex = 31;
            this.lblfirst.Text = "Optional Field 1";
            this.lblfirst.Visible = false;
            // 
            // frmBankReconciliation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(749, 445);
            this.Controls.Add(this.lblBankReconcilationDiff);
            this.Controls.Add(this.lblBankStatementBalance);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label4);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "frmBankReconciliation";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Bank Reconciliation";
            this.Load += new System.EventHandler(this.frmBankReconciliation_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmBankReconciliation_KeyDown);
            this.tabPage2.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabPage5.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.grpSelect.ResumeLayout(false);
            this.grpSelect.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.CheckBox chkPrintWhileSaving;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label lblBankStatementBalance;
        private System.Windows.Forms.Label lblBankReconcilationDiff;
        private System.Windows.Forms.Button btnAddAccClass;
        private STreeView treeAccClass;
        private System.Windows.Forms.TabPage tabPage2;
        private STextBox txtVchNo;
        private SourceGrid.Grid grdAdjustment;
        private SMaskedTextBox txtDate;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private SComboBox cboSeriesName;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.MaskedTextBox txtFromDate;
        private System.Windows.Forms.MaskedTextBox txtToDate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private STextBox txtBankStatementBalance;
        private System.Windows.Forms.Button btnShow;
        private SourceGrid.Grid grdBankReconciliation;
        private System.Windows.Forms.Label Banks1;
        private SComboBox cmboBanks;
        private System.Windows.Forms.Label label5;
        private SComboBox cmboParty;
        private System.Windows.Forms.GroupBox grpSelect;
        private System.Windows.Forms.RadioButton rbtnPayment;
        private System.Windows.Forms.RadioButton rbtnReceipt;
        private System.Windows.Forms.RadioButton rbtnAll;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnDate;
        private System.Windows.Forms.Button btnSelectAccClass;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbtnpayments;
        private System.Windows.Forms.RadioButton rbtnreceived;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.TabPage tabPage3;
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
    }
}