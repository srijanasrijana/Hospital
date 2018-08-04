namespace Accounts.View
{
    partial class frmBudgetAllocation
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cmbCopyBudget = new System.Windows.Forms.ComboBox();
            this.btnNew = new System.Windows.Forms.Button();
            this.txtEndDate = new SComponents.SMaskedTextBox();
            this.txtStartDate = new SComponents.SMaskedTextBox();
            this.cmbBudgetName = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnDeleteAllAlloc = new System.Windows.Forms.Button();
            this.btnAmount = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.txtLedgerName = new System.Windows.Forms.TextBox();
            this.txtGroupName = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.grdAllocation = new SourceGrid.Grid();
            this.txtAllocationTotal = new System.Windows.Forms.TextBox();
            this.txtAmountAssigned = new System.Windows.Forms.TextBox();
            this.btnAllocate = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.tabDisplay = new System.Windows.Forms.TabControl();
            this.tbTree = new System.Windows.Forms.TabPage();
            this.btnToggleExpand = new System.Windows.Forms.Button();
            this.tvAccountHead = new System.Windows.Forms.TreeView();
            this.tbList = new System.Windows.Forms.TabPage();
            this.grdListView = new SourceGrid.Grid();
            this.tabAccountSetup = new System.Windows.Forms.TabControl();
            this.tabBudgetAllocation = new System.Windows.Forms.TabPage();
            this.btnCopy = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnForgetSearch = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.txtSrchParam2 = new SComponents.STextBox();
            this.cboSrchOP2 = new SComponents.SComboBox();
            this.txtSrchParam1 = new SComponents.STextBox();
            this.cboSrchOP1 = new SComponents.SComboBox();
            this.cboSrchSearchIn1 = new SComponents.SComboBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabDisplay.SuspendLayout();
            this.tbTree.SuspendLayout();
            this.tbList.SuspendLayout();
            this.tabAccountSetup.SuspendLayout();
            this.tabBudgetAllocation.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cmbCopyBudget);
            this.groupBox1.Controls.Add(this.btnNew);
            this.groupBox1.Controls.Add(this.txtEndDate);
            this.groupBox1.Controls.Add(this.txtStartDate);
            this.groupBox1.Controls.Add(this.cmbBudgetName);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(6, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(568, 106);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Budget";
            // 
            // cmbCopyBudget
            // 
            this.cmbCopyBudget.FormattingEnabled = true;
            this.cmbCopyBudget.Location = new System.Drawing.Point(156, 78);
            this.cmbCopyBudget.Name = "cmbCopyBudget";
            this.cmbCopyBudget.Size = new System.Drawing.Size(228, 21);
            this.cmbCopyBudget.TabIndex = 16;
            this.cmbCopyBudget.SelectedIndexChanged += new System.EventHandler(this.cmbCopyBudget_SelectedIndexChanged);
            // 
            // btnNew
            // 
            this.btnNew.Image = global::Accounts.Properties.Resources.edit_add;
            this.btnNew.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNew.Location = new System.Drawing.Point(390, 17);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(87, 22);
            this.btnNew.TabIndex = 10;
            this.btnNew.Text = "&New";
            this.btnNew.UseVisualStyleBackColor = true;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // txtEndDate
            // 
            this.txtEndDate.BackColor = System.Drawing.Color.White;
            this.txtEndDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtEndDate.Enabled = false;
            this.txtEndDate.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.txtEndDate.FocusLostColor = System.Drawing.Color.White;
            this.txtEndDate.Location = new System.Drawing.Point(345, 49);
            this.txtEndDate.Mask = "0000/00/00";
            this.txtEndDate.Name = "txtEndDate";
            this.txtEndDate.Size = new System.Drawing.Size(132, 20);
            this.txtEndDate.TabIndex = 11;
            // 
            // txtStartDate
            // 
            this.txtStartDate.BackColor = System.Drawing.Color.White;
            this.txtStartDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtStartDate.Enabled = false;
            this.txtStartDate.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.txtStartDate.FocusLostColor = System.Drawing.Color.White;
            this.txtStartDate.Location = new System.Drawing.Point(156, 48);
            this.txtStartDate.Mask = "0000/00/00";
            this.txtStartDate.Name = "txtStartDate";
            this.txtStartDate.Size = new System.Drawing.Size(132, 20);
            this.txtStartDate.TabIndex = 10;
            // 
            // cmbBudgetName
            // 
            this.cmbBudgetName.FormattingEnabled = true;
            this.cmbBudgetName.Location = new System.Drawing.Point(156, 18);
            this.cmbBudgetName.Name = "cmbBudgetName";
            this.cmbBudgetName.Size = new System.Drawing.Size(228, 21);
            this.cmbBudgetName.TabIndex = 1;
            this.cmbBudgetName.SelectedIndexChanged += new System.EventHandler(this.cmbBudgetName_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(74, 80);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Copy From";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(72, 51);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Starting Date";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(303, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(33, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Up to";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(72, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Budget Name";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.btnDeleteAllAlloc);
            this.groupBox2.Controls.Add(this.btnAmount);
            this.groupBox2.Controls.Add(this.btnCancel);
            this.groupBox2.Controls.Add(this.btnEdit);
            this.groupBox2.Controls.Add(this.txtLedgerName);
            this.groupBox2.Controls.Add(this.txtGroupName);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.grdAllocation);
            this.groupBox2.Controls.Add(this.txtAllocationTotal);
            this.groupBox2.Controls.Add(this.txtAmountAssigned);
            this.groupBox2.Controls.Add(this.btnAllocate);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Location = new System.Drawing.Point(6, 127);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(568, 257);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Allocation";
            // 
            // btnDeleteAllAlloc
            // 
            this.btnDeleteAllAlloc.Image = global::Accounts.Properties.Resources.document_delete;
            this.btnDeleteAllAlloc.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDeleteAllAlloc.Location = new System.Drawing.Point(6, 224);
            this.btnDeleteAllAlloc.Name = "btnDeleteAllAlloc";
            this.btnDeleteAllAlloc.Size = new System.Drawing.Size(100, 25);
            this.btnDeleteAllAlloc.TabIndex = 15;
            this.btnDeleteAllAlloc.Text = "&Delete All";
            this.btnDeleteAllAlloc.UseVisualStyleBackColor = true;
            this.btnDeleteAllAlloc.Click += new System.EventHandler(this.btnDeleteAllAlloc_Click);
            // 
            // btnAmount
            // 
            this.btnAmount.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAmount.Location = new System.Drawing.Point(317, 74);
            this.btnAmount.Name = "btnAmount";
            this.btnAmount.Size = new System.Drawing.Size(161, 23);
            this.btnAmount.TabIndex = 14;
            this.btnAmount.Text = " Assign Amount";
            this.btnAmount.UseVisualStyleBackColor = true;
            this.btnAmount.Click += new System.EventHandler(this.btnAmount_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Image = global::Accounts.Properties.Resources.gnome_window_close;
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(502, 12);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 13;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Visible = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Image = global::Accounts.Properties.Resources.document_edit;
            this.btnEdit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEdit.Location = new System.Drawing.Point(502, 41);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(75, 23);
            this.btnEdit.TabIndex = 11;
            this.btnEdit.Text = "&Edit";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Visible = false;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // txtLedgerName
            // 
            this.txtLedgerName.Enabled = false;
            this.txtLedgerName.Location = new System.Drawing.Point(157, 48);
            this.txtLedgerName.Name = "txtLedgerName";
            this.txtLedgerName.Size = new System.Drawing.Size(321, 20);
            this.txtLedgerName.TabIndex = 9;
            // 
            // txtGroupName
            // 
            this.txtGroupName.Enabled = false;
            this.txtGroupName.Location = new System.Drawing.Point(157, 19);
            this.txtGroupName.Name = "txtGroupName";
            this.txtGroupName.Size = new System.Drawing.Size(321, 20);
            this.txtGroupName.TabIndex = 8;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(76, 22);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(67, 13);
            this.label7.TabIndex = 6;
            this.label7.Text = "Group Name";
            // 
            // grdAllocation
            // 
            this.grdAllocation.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdAllocation.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.grdAllocation.EnableSort = true;
            this.grdAllocation.Location = new System.Drawing.Point(6, 108);
            this.grdAllocation.Name = "grdAllocation";
            this.grdAllocation.OptimizeMode = SourceGrid.CellOptimizeMode.ForRows;
            this.grdAllocation.SelectionMode = SourceGrid.GridSelectionMode.Row;
            this.grdAllocation.Size = new System.Drawing.Size(545, 110);
            this.grdAllocation.TabIndex = 4;
            this.grdAllocation.TabStop = true;
            this.grdAllocation.ToolTipText = "";
            this.grdAllocation.Paint += new System.Windows.Forms.PaintEventHandler(this.grdAllocation_Paint);
            // 
            // txtAllocationTotal
            // 
            this.txtAllocationTotal.Enabled = false;
            this.txtAllocationTotal.Location = new System.Drawing.Point(337, 224);
            this.txtAllocationTotal.Name = "txtAllocationTotal";
            this.txtAllocationTotal.Size = new System.Drawing.Size(140, 20);
            this.txtAllocationTotal.TabIndex = 3;
            this.txtAllocationTotal.Text = "0";
            this.txtAllocationTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtAmountAssigned
            // 
            this.txtAmountAssigned.Location = new System.Drawing.Point(147, 86);
            this.txtAmountAssigned.Name = "txtAmountAssigned";
            this.txtAmountAssigned.Size = new System.Drawing.Size(312, 20);
            this.txtAmountAssigned.TabIndex = 3;
            this.txtAmountAssigned.Text = "0";
            this.txtAmountAssigned.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtAmountAssigned.Visible = false;
            // 
            // btnAllocate
            // 
            this.btnAllocate.Location = new System.Drawing.Point(502, 68);
            this.btnAllocate.Name = "btnAllocate";
            this.btnAllocate.Size = new System.Drawing.Size(75, 23);
            this.btnAllocate.TabIndex = 2;
            this.btnAllocate.Text = "Allocate";
            this.btnAllocate.UseVisualStyleBackColor = true;
            this.btnAllocate.Visible = false;
            this.btnAllocate.Click += new System.EventHandler(this.btnAllocate_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(251, 227);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(78, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "Allocated Total";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(76, 51);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(71, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "Ledger Name";
            // 
            // btnClear
            // 
            this.btnClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClear.Location = new System.Drawing.Point(429, 391);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(70, 27);
            this.btnClear.TabIndex = 2;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(505, 390);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(70, 27);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // tabDisplay
            // 
            this.tabDisplay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.tabDisplay.Controls.Add(this.tbTree);
            this.tabDisplay.Controls.Add(this.tbList);
            this.tabDisplay.Location = new System.Drawing.Point(8, 76);
            this.tabDisplay.Name = "tabDisplay";
            this.tabDisplay.SelectedIndex = 0;
            this.tabDisplay.Size = new System.Drawing.Size(186, 446);
            this.tabDisplay.TabIndex = 7;
            // 
            // tbTree
            // 
            this.tbTree.Controls.Add(this.btnToggleExpand);
            this.tbTree.Controls.Add(this.tvAccountHead);
            this.tbTree.ImageIndex = 0;
            this.tbTree.Location = new System.Drawing.Point(4, 22);
            this.tbTree.Name = "tbTree";
            this.tbTree.Padding = new System.Windows.Forms.Padding(3);
            this.tbTree.Size = new System.Drawing.Size(178, 420);
            this.tbTree.TabIndex = 0;
            this.tbTree.Text = "TreeView";
            this.tbTree.UseVisualStyleBackColor = true;
            // 
            // btnToggleExpand
            // 
            this.btnToggleExpand.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnToggleExpand.Image = global::Accounts.Properties.Resources.tree1;
            this.btnToggleExpand.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnToggleExpand.Location = new System.Drawing.Point(3, 3);
            this.btnToggleExpand.Name = "btnToggleExpand";
            this.btnToggleExpand.Size = new System.Drawing.Size(172, 28);
            this.btnToggleExpand.TabIndex = 17;
            this.btnToggleExpand.Text = "Expand";
            this.btnToggleExpand.UseVisualStyleBackColor = true;
            this.btnToggleExpand.Click += new System.EventHandler(this.btnToggleExpand_Click);
            // 
            // tvAccountHead
            // 
            this.tvAccountHead.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tvAccountHead.Location = new System.Drawing.Point(3, 33);
            this.tvAccountHead.Name = "tvAccountHead";
            this.tvAccountHead.Size = new System.Drawing.Size(172, 384);
            this.tvAccountHead.TabIndex = 3;
            this.tvAccountHead.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvAccountHead_AfterSelect);
            // 
            // tbList
            // 
            this.tbList.Controls.Add(this.grdListView);
            this.tbList.ImageIndex = 1;
            this.tbList.Location = new System.Drawing.Point(4, 22);
            this.tbList.Name = "tbList";
            this.tbList.Padding = new System.Windows.Forms.Padding(3);
            this.tbList.Size = new System.Drawing.Size(178, 420);
            this.tbList.TabIndex = 1;
            this.tbList.Text = "ListView";
            this.tbList.UseVisualStyleBackColor = true;
            // 
            // grdListView
            // 
            this.grdListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdListView.EnableSort = true;
            this.grdListView.Location = new System.Drawing.Point(3, 3);
            this.grdListView.Name = "grdListView";
            this.grdListView.OptimizeMode = SourceGrid.CellOptimizeMode.ForRows;
            this.grdListView.SelectionMode = SourceGrid.GridSelectionMode.Cell;
            this.grdListView.Size = new System.Drawing.Size(172, 414);
            this.grdListView.TabIndex = 0;
            this.grdListView.TabStop = true;
            this.grdListView.ToolTipText = "";
            this.grdListView.Paint += new System.Windows.Forms.PaintEventHandler(this.grdListView_Paint);
            // 
            // tabAccountSetup
            // 
            this.tabAccountSetup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.tabAccountSetup.Controls.Add(this.tabBudgetAllocation);
            this.tabAccountSetup.Location = new System.Drawing.Point(196, 76);
            this.tabAccountSetup.Name = "tabAccountSetup";
            this.tabAccountSetup.SelectedIndex = 0;
            this.tabAccountSetup.Size = new System.Drawing.Size(587, 446);
            this.tabAccountSetup.TabIndex = 8;
            // 
            // tabBudgetAllocation
            // 
            this.tabBudgetAllocation.Controls.Add(this.btnCopy);
            this.tabBudgetAllocation.Controls.Add(this.btnDelete);
            this.tabBudgetAllocation.Controls.Add(this.btnSave);
            this.tabBudgetAllocation.Controls.Add(this.groupBox2);
            this.tabBudgetAllocation.Controls.Add(this.groupBox1);
            this.tabBudgetAllocation.Controls.Add(this.btnClose);
            this.tabBudgetAllocation.Controls.Add(this.btnClear);
            this.tabBudgetAllocation.Location = new System.Drawing.Point(4, 22);
            this.tabBudgetAllocation.Name = "tabBudgetAllocation";
            this.tabBudgetAllocation.Padding = new System.Windows.Forms.Padding(3);
            this.tabBudgetAllocation.Size = new System.Drawing.Size(579, 420);
            this.tabBudgetAllocation.TabIndex = 0;
            this.tabBudgetAllocation.Text = "Budget Allocation";
            this.tabBudgetAllocation.UseVisualStyleBackColor = true;
            // 
            // btnCopy
            // 
            this.btnCopy.Image = global::Accounts.Properties.Resources.copy;
            this.btnCopy.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCopy.Location = new System.Drawing.Point(396, 83);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(87, 22);
            this.btnCopy.TabIndex = 10;
            this.btnCopy.Text = "&Copy";
            this.btnCopy.UseVisualStyleBackColor = true;
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDelete.Image = global::Accounts.Properties.Resources.document_delete;
            this.btnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDelete.Location = new System.Drawing.Point(323, 388);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 27);
            this.btnDelete.TabIndex = 9;
            this.btnDelete.Text = "&Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Visible = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Image = global::Accounts.Properties.Resources.save;
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSave.Location = new System.Drawing.Point(239, 388);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 27);
            this.btnSave.TabIndex = 8;
            this.btnSave.Text = "&Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Visible = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnForgetSearch);
            this.groupBox3.Controls.Add(this.btnSearch);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Controls.Add(this.label11);
            this.groupBox3.Controls.Add(this.label12);
            this.groupBox3.Controls.Add(this.txtSrchParam2);
            this.groupBox3.Controls.Add(this.cboSrchOP2);
            this.groupBox3.Controls.Add(this.txtSrchParam1);
            this.groupBox3.Controls.Add(this.cboSrchOP1);
            this.groupBox3.Controls.Add(this.cboSrchSearchIn1);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox3.Location = new System.Drawing.Point(0, 0);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(787, 70);
            this.groupBox3.TabIndex = 9;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Search Box:";
            this.groupBox3.Enter += new System.EventHandler(this.groupBox3_Enter_1);
            // 
            // btnForgetSearch
            // 
            this.btnForgetSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnForgetSearch.Image = global::Accounts.Properties.Resources.gnome_window_close;
            this.btnForgetSearch.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnForgetSearch.Location = new System.Drawing.Point(666, 29);
            this.btnForgetSearch.Name = "btnForgetSearch";
            this.btnForgetSearch.Size = new System.Drawing.Size(59, 23);
            this.btnForgetSearch.TabIndex = 16;
            this.btnForgetSearch.Text = "&Forget";
            this.btnForgetSearch.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnForgetSearch.UseVisualStyleBackColor = true;
            this.btnForgetSearch.Click += new System.EventHandler(this.btnForgetSearch_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.Image = global::Accounts.Properties.Resources._1296834456_xmag;
            this.btnSearch.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSearch.Location = new System.Drawing.Point(594, 29);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(66, 23);
            this.btnSearch.TabIndex = 15;
            this.btnSearch.Text = "&Search";
            this.btnSearch.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(507, 16);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(55, 13);
            this.label8.TabIndex = 14;
            this.label8.Text = "Parameter";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(405, 16);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(48, 13);
            this.label9.TabIndex = 13;
            this.label9.Text = "Operator";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(300, 16);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(55, 13);
            this.label10.TabIndex = 11;
            this.label10.Text = "Parameter";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(205, 16);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(48, 13);
            this.label11.TabIndex = 10;
            this.label11.Text = "Operator";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(98, 16);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(53, 13);
            this.label12.TabIndex = 9;
            this.label12.Text = "Search In";
            // 
            // txtSrchParam2
            // 
            this.txtSrchParam2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSrchParam2.Enabled = false;
            this.txtSrchParam2.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtSrchParam2.FocusLostColor = System.Drawing.Color.White;
            this.txtSrchParam2.Location = new System.Drawing.Point(488, 32);
            this.txtSrchParam2.Name = "txtSrchParam2";
            this.txtSrchParam2.Size = new System.Drawing.Size(100, 20);
            this.txtSrchParam2.TabIndex = 7;
            // 
            // cboSrchOP2
            // 
            this.cboSrchOP2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSrchOP2.Enabled = false;
            this.cboSrchOP2.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.cboSrchOP2.FocusLostColor = System.Drawing.Color.White;
            this.cboSrchOP2.FormattingEnabled = true;
            this.cboSrchOP2.Items.AddRange(new object[] {
            "=",
            ">",
            "<",
            ">=",
            "<="});
            this.cboSrchOP2.Location = new System.Drawing.Point(388, 31);
            this.cboSrchOP2.Name = "cboSrchOP2";
            this.cboSrchOP2.Size = new System.Drawing.Size(94, 21);
            this.cboSrchOP2.TabIndex = 6;
            // 
            // txtSrchParam1
            // 
            this.txtSrchParam1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSrchParam1.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtSrchParam1.FocusLostColor = System.Drawing.Color.White;
            this.txtSrchParam1.Location = new System.Drawing.Point(282, 32);
            this.txtSrchParam1.Name = "txtSrchParam1";
            this.txtSrchParam1.Size = new System.Drawing.Size(100, 20);
            this.txtSrchParam1.TabIndex = 4;
            // 
            // cboSrchOP1
            // 
            this.cboSrchOP1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSrchOP1.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.cboSrchOP1.FocusLostColor = System.Drawing.Color.White;
            this.cboSrchOP1.FormattingEnabled = true;
            this.cboSrchOP1.Items.AddRange(new object[] {
            "Begins With",
            "Contains",
            "Greater or Equals",
            "Smaller or Equals"});
            this.cboSrchOP1.Location = new System.Drawing.Point(192, 32);
            this.cboSrchOP1.Name = "cboSrchOP1";
            this.cboSrchOP1.Size = new System.Drawing.Size(84, 21);
            this.cboSrchOP1.TabIndex = 3;
            // 
            // cboSrchSearchIn1
            // 
            this.cboSrchSearchIn1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.cboSrchSearchIn1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSrchSearchIn1.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.cboSrchSearchIn1.FocusLostColor = System.Drawing.Color.White;
            this.cboSrchSearchIn1.FormattingEnabled = true;
            this.cboSrchSearchIn1.Items.AddRange(new object[] {
            "Account Groups",
            "Ledgers",
            "Account Under",
            "Ledger Under"});
            this.cboSrchSearchIn1.Location = new System.Drawing.Point(65, 32);
            this.cboSrchSearchIn1.Name = "cboSrchSearchIn1";
            this.cboSrchSearchIn1.Size = new System.Drawing.Size(121, 21);
            this.cboSrchSearchIn1.TabIndex = 0;
            // 
            // frmBudgetAllocation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(787, 523);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.tabAccountSetup);
            this.Controls.Add(this.tabDisplay);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmBudgetAllocation";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Budget Allocation";
            this.Load += new System.EventHandler(this.Budget_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tabDisplay.ResumeLayout(false);
            this.tbTree.ResumeLayout(false);
            this.tbList.ResumeLayout(false);
            this.tabAccountSetup.ResumeLayout(false);
            this.tabBudgetAllocation.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cmbBudgetName;
        private System.Windows.Forms.TextBox txtAmountAssigned;
        private System.Windows.Forms.Button btnAllocate;
        private SourceGrid.Grid grdAllocation;
        private System.Windows.Forms.TextBox txtAllocationTotal;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.TabControl tabDisplay;
        private System.Windows.Forms.TabPage tbTree;
        private System.Windows.Forms.Button btnToggleExpand;
        private System.Windows.Forms.TreeView tvAccountHead;
        private System.Windows.Forms.TabPage tbList;
        private SourceGrid.Grid grdListView;
        private System.Windows.Forms.TabControl tabAccountSetup;
        private System.Windows.Forms.TabPage tabBudgetAllocation;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtLedgerName;
        private System.Windows.Forms.TextBox txtGroupName;
        private SComponents.SMaskedTextBox txtStartDate;
        private SComponents.SMaskedTextBox txtEndDate;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnForgetSearch;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private SComponents.STextBox txtSrchParam2;
        private SComponents.SComboBox cboSrchOP2;
        private SComponents.STextBox txtSrchParam1;
        private SComponents.SComboBox cboSrchOP1;
        private SComponents.SComboBox cboSrchSearchIn1;
        private System.Windows.Forms.Button btnAmount;
        private System.Windows.Forms.ComboBox cmbCopyBudget;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnCopy;
        private System.Windows.Forms.Button btnDeleteAllAlloc;
    }
}