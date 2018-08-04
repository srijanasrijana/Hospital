using SComponents;
namespace Accounts
{
    partial class frmBankReceipt
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmBankReceipt));
            this.BRbtnCancel = new System.Windows.Forms.Button();
            this.grdBankReceipt = new SourceGrid.Grid();
            this.BRchkDoNotClose = new System.Windows.Forms.CheckBox();
            this.BRbtnEdit = new System.Windows.Forms.Button();
            this.BRbtnDelete = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.BRchkPrntWhileSaving = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnList = new System.Windows.Forms.Button();
            this.btnSetup = new System.Windows.Forms.Button();
            this.btnPrintPreview = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.BRchkRecurring = new System.Windows.Forms.CheckBox();
            this.btnPaste = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnLast = new System.Windows.Forms.Button();
            this.btnCopy = new System.Windows.Forms.Button();
            this.btnFirst = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnPrev = new System.Windows.Forms.Button();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.BRlblTotalAmount = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.BRbtnAddAccClass = new System.Windows.Forms.Button();
            this.treeAccClass = new SComponents.STreeView(this.components);
            this.label3 = new System.Windows.Forms.Label();
            this.lblVouNo = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.BRbtnSave = new System.Windows.Forms.Button();
            this.BRbtnNew = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.BRtxtfifth = new SComponents.STextBox();
            this.BRtxtfourth = new SComponents.STextBox();
            this.BRtxtthird = new SComponents.STextBox();
            this.BRtxtsecond = new SComponents.STextBox();
            this.BRtxtfirst = new SComponents.STextBox();
            this.BRlblfourth = new System.Windows.Forms.Label();
            this.BRlblsecond = new System.Windows.Forms.Label();
            this.BRlblthird = new System.Windows.Forms.Label();
            this.BRlblfifth = new System.Windows.Forms.Label();
            this.BRlblfirst = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.BRbtn_groupvoucherposting = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.BRlblBankCurrBalHidden = new System.Windows.Forms.Label();
            this.BRlblBankCurrentBalance = new System.Windows.Forms.Label();
            this.BRbtnDate = new System.Windows.Forms.Button();
            this.BRcboProjectName = new SComponents.SComboBox();
            this.BRtxtVchNo = new SComponents.STextBox();
            this.BRtxtBankID = new SComponents.STextBox();
            this.BRtxtDate = new SComponents.SMaskedTextBox();
            this.BRcboSeriesName = new SComponents.SComboBox();
            this.BRcomboBankAccount = new SComponents.SComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.statusJournal = new System.Windows.Forms.ToolStripStatusLabel();
            this.BRtxtRemarks = new SComponents.STextBox();
            this.panel1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // BRbtnCancel
            // 
            this.BRbtnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BRbtnCancel.Image = global::Accounts.Properties.Resources.gnome_window_close;
            this.BRbtnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BRbtnCancel.Location = new System.Drawing.Point(757, 15);
            this.BRbtnCancel.Name = "BRbtnCancel";
            this.BRbtnCancel.Size = new System.Drawing.Size(75, 28);
            this.BRbtnCancel.TabIndex = 108;
            this.BRbtnCancel.Text = "&Cancel";
            this.BRbtnCancel.UseVisualStyleBackColor = true;
            this.BRbtnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // grdBankReceipt
            // 
            this.grdBankReceipt.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdBankReceipt.EnableSort = true;
            this.grdBankReceipt.Location = new System.Drawing.Point(3, 3);
            this.grdBankReceipt.Name = "grdBankReceipt";
            this.grdBankReceipt.OptimizeMode = SourceGrid.CellOptimizeMode.ForRows;
            this.grdBankReceipt.SelectionMode = SourceGrid.GridSelectionMode.Cell;
            this.grdBankReceipt.Size = new System.Drawing.Size(834, 167);
            this.grdBankReceipt.TabIndex = 1;
            this.grdBankReceipt.TabStop = true;
            this.grdBankReceipt.ToolTipText = "";
            this.grdBankReceipt.Paint += new System.Windows.Forms.PaintEventHandler(this.grdBankReceipt_Paint);
            // 
            // BRchkDoNotClose
            // 
            this.BRchkDoNotClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.BRchkDoNotClose.AutoSize = true;
            this.BRchkDoNotClose.Location = new System.Drawing.Point(6, 20);
            this.BRchkDoNotClose.Name = "BRchkDoNotClose";
            this.BRchkDoNotClose.Size = new System.Drawing.Size(86, 17);
            this.BRchkDoNotClose.TabIndex = 110;
            this.BRchkDoNotClose.Text = "Do not close";
            this.BRchkDoNotClose.UseVisualStyleBackColor = true;
            this.BRchkDoNotClose.KeyDown += new System.Windows.Forms.KeyEventHandler(this.BRchkDoNotClose_KeyDown);
            // 
            // BRbtnEdit
            // 
            this.BRbtnEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BRbtnEdit.Image = global::Accounts.Properties.Resources.document_edit;
            this.BRbtnEdit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BRbtnEdit.Location = new System.Drawing.Point(514, 14);
            this.BRbtnEdit.Name = "BRbtnEdit";
            this.BRbtnEdit.Size = new System.Drawing.Size(75, 28);
            this.BRbtnEdit.TabIndex = 107;
            this.BRbtnEdit.Text = "&Edit";
            this.BRbtnEdit.UseVisualStyleBackColor = true;
            this.BRbtnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // BRbtnDelete
            // 
            this.BRbtnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BRbtnDelete.Image = global::Accounts.Properties.Resources.document_delete;
            this.BRbtnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BRbtnDelete.Location = new System.Drawing.Point(676, 15);
            this.BRbtnDelete.Name = "BRbtnDelete";
            this.BRbtnDelete.Size = new System.Drawing.Size(75, 28);
            this.BRbtnDelete.TabIndex = 106;
            this.BRbtnDelete.Text = "&Delete";
            this.BRbtnDelete.UseVisualStyleBackColor = true;
            this.BRbtnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(24, 41);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(78, 13);
            this.label6.TabIndex = 113;
            this.label6.Text = "Bank Account:";
            // 
            // BRchkPrntWhileSaving
            // 
            this.BRchkPrntWhileSaving.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.BRchkPrntWhileSaving.AutoSize = true;
            this.BRchkPrntWhileSaving.Location = new System.Drawing.Point(96, 20);
            this.BRchkPrntWhileSaving.Name = "BRchkPrntWhileSaving";
            this.BRchkPrntWhileSaving.Size = new System.Drawing.Size(108, 17);
            this.BRchkPrntWhileSaving.TabIndex = 111;
            this.BRchkPrntWhileSaving.Text = "Print while saving";
            this.BRchkPrntWhileSaving.UseVisualStyleBackColor = true;
            this.BRchkPrntWhileSaving.KeyDown += new System.Windows.Forms.KeyEventHandler(this.BRchkPrntWhileSaving_KeyDown);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.CadetBlue;
            this.panel1.Controls.Add(this.btnList);
            this.panel1.Controls.Add(this.btnSetup);
            this.panel1.Controls.Add(this.btnPrintPreview);
            this.panel1.Controls.Add(this.btnPrint);
            this.panel1.Controls.Add(this.BRchkRecurring);
            this.panel1.Controls.Add(this.btnPaste);
            this.panel1.Controls.Add(this.btnExport);
            this.panel1.Controls.Add(this.btnExit);
            this.panel1.Controls.Add(this.btnLast);
            this.panel1.Controls.Add(this.btnCopy);
            this.panel1.Controls.Add(this.btnFirst);
            this.panel1.Controls.Add(this.btnNext);
            this.panel1.Controls.Add(this.btnPrev);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(859, 32);
            this.panel1.TabIndex = 109;
            // 
            // btnList
            // 
            this.btnList.Location = new System.Drawing.Point(244, 3);
            this.btnList.Name = "btnList";
            this.btnList.Size = new System.Drawing.Size(53, 23);
            this.btnList.TabIndex = 153;
            this.btnList.Text = "≡ List";
            this.btnList.UseVisualStyleBackColor = true;
            this.btnList.Click += new System.EventHandler(this.btnList_Click);
            // 
            // btnSetup
            // 
            this.btnSetup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSetup.AutoSize = true;
            this.btnSetup.Location = new System.Drawing.Point(735, 3);
            this.btnSetup.Name = "btnSetup";
            this.btnSetup.Size = new System.Drawing.Size(43, 23);
            this.btnSetup.TabIndex = 37;
            this.btnSetup.Text = "setup";
            this.btnSetup.UseVisualStyleBackColor = true;
            this.btnSetup.Click += new System.EventHandler(this.btnSetup_Click);
            // 
            // btnPrintPreview
            // 
            this.btnPrintPreview.Image = global::Accounts.Properties.Resources.print_preview;
            this.btnPrintPreview.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPrintPreview.Location = new System.Drawing.Point(479, 3);
            this.btnPrintPreview.Name = "btnPrintPreview";
            this.btnPrintPreview.Size = new System.Drawing.Size(94, 23);
            this.btnPrintPreview.TabIndex = 31;
            this.btnPrintPreview.Text = "Pr&int Preview";
            this.btnPrintPreview.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnPrintPreview.UseVisualStyleBackColor = true;
            this.btnPrintPreview.Click += new System.EventHandler(this.btnPrintPreview_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.Image = global::Accounts.Properties.Resources.print;
            this.btnPrint.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPrint.Location = new System.Drawing.Point(419, 3);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(58, 23);
            this.btnPrint.TabIndex = 2;
            this.btnPrint.Text = "Print";
            this.btnPrint.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // BRchkRecurring
            // 
            this.BRchkRecurring.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BRchkRecurring.AutoSize = true;
            this.BRchkRecurring.ForeColor = System.Drawing.Color.White;
            this.BRchkRecurring.Location = new System.Drawing.Point(667, 7);
            this.BRchkRecurring.Margin = new System.Windows.Forms.Padding(2);
            this.BRchkRecurring.Name = "BRchkRecurring";
            this.BRchkRecurring.Size = new System.Drawing.Size(72, 17);
            this.BRchkRecurring.TabIndex = 36;
            this.BRchkRecurring.Text = "Recurrin&g";
            this.BRchkRecurring.UseVisualStyleBackColor = true;
            this.BRchkRecurring.CheckedChanged += new System.EventHandler(this.BRchkRecurring_CheckedChanged);
            // 
            // btnPaste
            // 
            this.btnPaste.Image = global::Accounts.Properties.Resources.paste;
            this.btnPaste.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPaste.Location = new System.Drawing.Point(359, 3);
            this.btnPaste.Name = "btnPaste";
            this.btnPaste.Size = new System.Drawing.Size(58, 23);
            this.btnPaste.TabIndex = 24;
            this.btnPaste.Text = "Paste";
            this.btnPaste.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnPaste.UseVisualStyleBackColor = true;
            this.btnPaste.Click += new System.EventHandler(this.btnPaste_Click);
            // 
            // btnExport
            // 
            this.btnExport.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExport.Enabled = false;
            this.btnExport.Image = global::Accounts.Properties.Resources.export1;
            this.btnExport.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnExport.Location = new System.Drawing.Point(784, 3);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(62, 23);
            this.btnExport.TabIndex = 27;
            this.btnExport.Text = "&Export";
            this.btnExport.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnExit
            // 
            this.btnExit.Image = global::Accounts.Properties.Resources.ExitButton;
            this.btnExit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnExit.Location = new System.Drawing.Point(575, 3);
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
            this.btnLast.Location = new System.Drawing.Point(184, 3);
            this.btnLast.Name = "btnLast";
            this.btnLast.Size = new System.Drawing.Size(58, 23);
            this.btnLast.TabIndex = 4;
            this.btnLast.Text = "Last";
            this.btnLast.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLast.UseVisualStyleBackColor = true;
            this.btnLast.Click += new System.EventHandler(this.btnLast_Click);
            // 
            // btnCopy
            // 
            this.btnCopy.Image = global::Accounts.Properties.Resources.copy;
            this.btnCopy.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCopy.Location = new System.Drawing.Point(299, 3);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(58, 23);
            this.btnCopy.TabIndex = 23;
            this.btnCopy.Text = "Copy";
            this.btnCopy.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCopy.UseVisualStyleBackColor = true;
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
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
            // btnNext
            // 
            this.btnNext.Image = global::Accounts.Properties.Resources.forward;
            this.btnNext.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnNext.Location = new System.Drawing.Point(124, 3);
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
            this.btnPrev.Location = new System.Drawing.Point(64, 3);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(58, 23);
            this.btnPrev.TabIndex = 0;
            this.btnPrev.Text = "&Prev";
            this.btnPrev.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnPrev.UseVisualStyleBackColor = true;
            this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.BRlblTotalAmount);
            this.tabPage1.Controls.Add(this.label7);
            this.tabPage1.Controls.Add(this.grdBankReceipt);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(843, 192);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Details";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // BRlblTotalAmount
            // 
            this.BRlblTotalAmount.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.BRlblTotalAmount.Location = new System.Drawing.Point(575, 173);
            this.BRlblTotalAmount.Name = "BRlblTotalAmount";
            this.BRlblTotalAmount.Size = new System.Drawing.Size(155, 13);
            this.BRlblTotalAmount.TabIndex = 25;
            this.BRlblTotalAmount.Text = "0.00";
            this.BRlblTotalAmount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label7
            // 
            this.label7.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(532, 173);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(37, 13);
            this.label7.TabIndex = 24;
            this.label7.Text = "Total: ";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.BRbtnAddAccClass);
            this.tabPage2.Controls.Add(this.treeAccClass);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(843, 192);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = " Account Class";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // BRbtnAddAccClass
            // 
            this.BRbtnAddAccClass.Location = new System.Drawing.Point(397, 6);
            this.BRbtnAddAccClass.Name = "BRbtnAddAccClass";
            this.BRbtnAddAccClass.Size = new System.Drawing.Size(109, 23);
            this.BRbtnAddAccClass.TabIndex = 31;
            this.BRbtnAddAccClass.Text = "Add Account Class";
            this.BRbtnAddAccClass.UseVisualStyleBackColor = true;
            this.BRbtnAddAccClass.Click += new System.EventHandler(this.btnAddAccClass_Click);
            // 
            // treeAccClass
            // 
            this.treeAccClass.AutoCheckChild = true;
            this.treeAccClass.CheckBoxes = true;
            this.treeAccClass.Location = new System.Drawing.Point(0, 6);
            this.treeAccClass.Name = "treeAccClass";
            this.treeAccClass.Size = new System.Drawing.Size(391, 241);
            this.treeAccClass.TabIndex = 29;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(634, 20);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(33, 13);
            this.label3.TabIndex = 102;
            this.label3.Text = "Date:";
            // 
            // lblVouNo
            // 
            this.lblVouNo.AutoSize = true;
            this.lblVouNo.Location = new System.Drawing.Point(334, 20);
            this.lblVouNo.Name = "lblVouNo";
            this.lblVouNo.Size = new System.Drawing.Size(60, 13);
            this.lblVouNo.TabIndex = 99;
            this.lblVouNo.Text = "Voucher #:";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(21, 364);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 101;
            this.label2.Text = "Narration:";
            // 
            // BRbtnSave
            // 
            this.BRbtnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BRbtnSave.Image = global::Accounts.Properties.Resources.save;
            this.BRbtnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BRbtnSave.Location = new System.Drawing.Point(595, 15);
            this.BRbtnSave.Name = "BRbtnSave";
            this.BRbtnSave.Size = new System.Drawing.Size(75, 28);
            this.BRbtnSave.TabIndex = 105;
            this.BRbtnSave.Text = "&Save";
            this.BRbtnSave.UseVisualStyleBackColor = true;
            this.BRbtnSave.Click += new System.EventHandler(this.BRbtnSave_Click);
            // 
            // BRbtnNew
            // 
            this.BRbtnNew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BRbtnNew.Image = global::Accounts.Properties.Resources.edit_add;
            this.BRbtnNew.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BRbtnNew.Location = new System.Drawing.Point(433, 13);
            this.BRbtnNew.Name = "BRbtnNew";
            this.BRbtnNew.Size = new System.Drawing.Size(75, 28);
            this.BRbtnNew.TabIndex = 104;
            this.BRbtnNew.Text = "&New";
            this.BRbtnNew.UseVisualStyleBackColor = true;
            this.BRbtnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(5, 139);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(851, 218);
            this.tabControl1.TabIndex = 97;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.BRtxtfifth);
            this.tabPage3.Controls.Add(this.BRtxtfourth);
            this.tabPage3.Controls.Add(this.BRtxtthird);
            this.tabPage3.Controls.Add(this.BRtxtsecond);
            this.tabPage3.Controls.Add(this.BRtxtfirst);
            this.tabPage3.Controls.Add(this.BRlblfourth);
            this.tabPage3.Controls.Add(this.BRlblsecond);
            this.tabPage3.Controls.Add(this.BRlblthird);
            this.tabPage3.Controls.Add(this.BRlblfifth);
            this.tabPage3.Controls.Add(this.BRlblfirst);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(843, 192);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Additional Fields";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // BRtxtfifth
            // 
            this.BRtxtfifth.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.BRtxtfifth.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.BRtxtfifth.FocusLostColor = System.Drawing.Color.White;
            this.BRtxtfifth.Location = new System.Drawing.Point(109, 162);
            this.BRtxtfifth.Name = "BRtxtfifth";
            this.BRtxtfifth.Size = new System.Drawing.Size(182, 20);
            this.BRtxtfifth.TabIndex = 40;
            this.BRtxtfifth.Visible = false;
            // 
            // BRtxtfourth
            // 
            this.BRtxtfourth.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.BRtxtfourth.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.BRtxtfourth.FocusLostColor = System.Drawing.Color.White;
            this.BRtxtfourth.Location = new System.Drawing.Point(109, 123);
            this.BRtxtfourth.Name = "BRtxtfourth";
            this.BRtxtfourth.Size = new System.Drawing.Size(182, 20);
            this.BRtxtfourth.TabIndex = 39;
            this.BRtxtfourth.Visible = false;
            // 
            // BRtxtthird
            // 
            this.BRtxtthird.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.BRtxtthird.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.BRtxtthird.FocusLostColor = System.Drawing.Color.White;
            this.BRtxtthird.Location = new System.Drawing.Point(109, 85);
            this.BRtxtthird.Name = "BRtxtthird";
            this.BRtxtthird.Size = new System.Drawing.Size(182, 20);
            this.BRtxtthird.TabIndex = 38;
            this.BRtxtthird.Visible = false;
            // 
            // BRtxtsecond
            // 
            this.BRtxtsecond.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.BRtxtsecond.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.BRtxtsecond.FocusLostColor = System.Drawing.Color.White;
            this.BRtxtsecond.Location = new System.Drawing.Point(109, 45);
            this.BRtxtsecond.Name = "BRtxtsecond";
            this.BRtxtsecond.Size = new System.Drawing.Size(182, 20);
            this.BRtxtsecond.TabIndex = 37;
            this.BRtxtsecond.Visible = false;
            // 
            // BRtxtfirst
            // 
            this.BRtxtfirst.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.BRtxtfirst.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.BRtxtfirst.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.BRtxtfirst.FocusLostColor = System.Drawing.Color.White;
            this.BRtxtfirst.Location = new System.Drawing.Point(109, 11);
            this.BRtxtfirst.Name = "BRtxtfirst";
            this.BRtxtfirst.Size = new System.Drawing.Size(182, 20);
            this.BRtxtfirst.TabIndex = 36;
            this.BRtxtfirst.Visible = false;
            // 
            // BRlblfourth
            // 
            this.BRlblfourth.AutoSize = true;
            this.BRlblfourth.Location = new System.Drawing.Point(9, 129);
            this.BRlblfourth.Name = "BRlblfourth";
            this.BRlblfourth.Size = new System.Drawing.Size(80, 13);
            this.BRlblfourth.TabIndex = 35;
            this.BRlblfourth.Text = "Optional Field 4";
            this.BRlblfourth.Visible = false;
            // 
            // BRlblsecond
            // 
            this.BRlblsecond.AutoSize = true;
            this.BRlblsecond.Location = new System.Drawing.Point(9, 51);
            this.BRlblsecond.Name = "BRlblsecond";
            this.BRlblsecond.Size = new System.Drawing.Size(80, 13);
            this.BRlblsecond.TabIndex = 34;
            this.BRlblsecond.Text = "Optional Field 2";
            this.BRlblsecond.Visible = false;
            // 
            // BRlblthird
            // 
            this.BRlblthird.AutoSize = true;
            this.BRlblthird.Location = new System.Drawing.Point(9, 90);
            this.BRlblthird.Name = "BRlblthird";
            this.BRlblthird.Size = new System.Drawing.Size(80, 13);
            this.BRlblthird.TabIndex = 33;
            this.BRlblthird.Text = "Optional Field 3";
            this.BRlblthird.Visible = false;
            // 
            // BRlblfifth
            // 
            this.BRlblfifth.AutoSize = true;
            this.BRlblfifth.Location = new System.Drawing.Point(9, 168);
            this.BRlblfifth.Name = "BRlblfifth";
            this.BRlblfifth.Size = new System.Drawing.Size(80, 13);
            this.BRlblfifth.TabIndex = 32;
            this.BRlblfifth.Text = "Optional Field 5";
            this.BRlblfifth.Visible = false;
            // 
            // BRlblfirst
            // 
            this.BRlblfirst.AutoSize = true;
            this.BRlblfirst.Location = new System.Drawing.Point(9, 12);
            this.BRlblfirst.Name = "BRlblfirst";
            this.BRlblfirst.Size = new System.Drawing.Size(80, 13);
            this.BRlblfirst.TabIndex = 31;
            this.BRlblfirst.Text = "Optional Field 1";
            this.BRlblfirst.Visible = false;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(25, 16);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(42, 13);
            this.label8.TabIndex = 135;
            this.label8.Text = "Series :";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(26, 67);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(46, 13);
            this.label12.TabIndex = 143;
            this.label12.Text = "Project :";
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.BRbtn_groupvoucherposting);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.BRlblBankCurrBalHidden);
            this.groupBox3.Controls.Add(this.BRlblBankCurrentBalance);
            this.groupBox3.Controls.Add(this.BRbtnDate);
            this.groupBox3.Controls.Add(this.label12);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.BRcboProjectName);
            this.groupBox3.Controls.Add(this.BRtxtVchNo);
            this.groupBox3.Controls.Add(this.BRtxtBankID);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.BRtxtDate);
            this.groupBox3.Controls.Add(this.BRcboSeriesName);
            this.groupBox3.Controls.Add(this.lblVouNo);
            this.groupBox3.Controls.Add(this.BRcomboBankAccount);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Location = new System.Drawing.Point(6, 38);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(850, 95);
            this.groupBox3.TabIndex = 144;
            this.groupBox3.TabStop = false;
            this.groupBox3.Enter += new System.EventHandler(this.groupBox3_Enter);
            // 
            // BRbtn_groupvoucherposting
            // 
            this.BRbtn_groupvoucherposting.Location = new System.Drawing.Point(693, 42);
            this.BRbtn_groupvoucherposting.Name = "BRbtn_groupvoucherposting";
            this.BRbtn_groupvoucherposting.Size = new System.Drawing.Size(138, 23);
            this.BRbtn_groupvoucherposting.TabIndex = 154;
            this.BRbtn_groupvoucherposting.Text = "Group  Voucher Posting";
            this.BRbtn_groupvoucherposting.UseVisualStyleBackColor = true;
            this.BRbtn_groupvoucherposting.Click += new System.EventHandler(this.BRbtn_groupvoucherposting_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(309, 46);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(86, 13);
            this.label4.TabIndex = 153;
            this.label4.Text = "Current Balance:";
            // 
            // BRlblBankCurrBalHidden
            // 
            this.BRlblBankCurrBalHidden.AutoSize = true;
            this.BRlblBankCurrBalHidden.Location = new System.Drawing.Point(328, 71);
            this.BRlblBankCurrBalHidden.Name = "BRlblBankCurrBalHidden";
            this.BRlblBankCurrBalHidden.Size = new System.Drawing.Size(28, 13);
            this.BRlblBankCurrBalHidden.TabIndex = 152;
            this.BRlblBankCurrBalHidden.Text = "0.00";
            this.BRlblBankCurrBalHidden.Visible = false;
            // 
            // BRlblBankCurrentBalance
            // 
            this.BRlblBankCurrentBalance.AutoSize = true;
            this.BRlblBankCurrentBalance.Location = new System.Drawing.Point(394, 46);
            this.BRlblBankCurrentBalance.Name = "BRlblBankCurrentBalance";
            this.BRlblBankCurrentBalance.Size = new System.Drawing.Size(28, 13);
            this.BRlblBankCurrentBalance.TabIndex = 151;
            this.BRlblBankCurrentBalance.Text = "0.00";
            // 
            // BRbtnDate
            // 
            this.BRbtnDate.Image = global::Accounts.Properties.Resources.dateIcon;
            this.BRbtnDate.Location = new System.Drawing.Point(805, 15);
            this.BRbtnDate.Name = "BRbtnDate";
            this.BRbtnDate.Size = new System.Drawing.Size(26, 23);
            this.BRbtnDate.TabIndex = 147;
            this.BRbtnDate.UseVisualStyleBackColor = true;
            this.BRbtnDate.Click += new System.EventHandler(this.BRbtnDate_Click);
            // 
            // BRcboProjectName
            // 
            this.BRcboProjectName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.BRcboProjectName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.BRcboProjectName.BackColor = System.Drawing.Color.White;
            this.BRcboProjectName.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.BRcboProjectName.FocusLostColor = System.Drawing.Color.White;
            this.BRcboProjectName.FormattingEnabled = true;
            this.BRcboProjectName.Location = new System.Drawing.Point(110, 67);
            this.BRcboProjectName.Name = "BRcboProjectName";
            this.BRcboProjectName.Size = new System.Drawing.Size(188, 21);
            this.BRcboProjectName.TabIndex = 142;
            this.BRcboProjectName.SelectedIndexChanged += new System.EventHandler(this.cboProjectName_SelectedIndexChanged);
            this.BRcboProjectName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.BRcboProjectName_KeyDown);
            // 
            // BRtxtVchNo
            // 
            this.BRtxtVchNo.BackColor = System.Drawing.Color.White;
            this.BRtxtVchNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.BRtxtVchNo.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.BRtxtVchNo.FocusLostColor = System.Drawing.Color.White;
            this.BRtxtVchNo.Location = new System.Drawing.Point(405, 18);
            this.BRtxtVchNo.Name = "BRtxtVchNo";
            this.BRtxtVchNo.Size = new System.Drawing.Size(178, 20);
            this.BRtxtVchNo.TabIndex = 98;
            // 
            // BRtxtBankID
            // 
            this.BRtxtBankID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.BRtxtBankID.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.BRtxtBankID.FocusLostColor = System.Drawing.Color.White;
            this.BRtxtBankID.Location = new System.Drawing.Point(473, 45);
            this.BRtxtBankID.Name = "BRtxtBankID";
            this.BRtxtBankID.Size = new System.Drawing.Size(88, 20);
            this.BRtxtBankID.TabIndex = 112;
            this.BRtxtBankID.Visible = false;
            // 
            // BRtxtDate
            // 
            this.BRtxtDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.BRtxtDate.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.BRtxtDate.FocusLostColor = System.Drawing.Color.White;
            this.BRtxtDate.Location = new System.Drawing.Point(673, 18);
            this.BRtxtDate.Mask = "0000/00/00";
            this.BRtxtDate.Name = "BRtxtDate";
            this.BRtxtDate.Size = new System.Drawing.Size(126, 20);
            this.BRtxtDate.TabIndex = 103;
            this.BRtxtDate.KeyDown += new System.Windows.Forms.KeyEventHandler(this.BRtxtDate_KeyDown);
            // 
            // BRcboSeriesName
            // 
            this.BRcboSeriesName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.BRcboSeriesName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.BRcboSeriesName.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.BRcboSeriesName.FocusLostColor = System.Drawing.Color.White;
            this.BRcboSeriesName.FormattingEnabled = true;
            this.BRcboSeriesName.Location = new System.Drawing.Point(110, 13);
            this.BRcboSeriesName.Name = "BRcboSeriesName";
            this.BRcboSeriesName.Size = new System.Drawing.Size(188, 21);
            this.BRcboSeriesName.TabIndex = 134;
            this.BRcboSeriesName.SelectedIndexChanged += new System.EventHandler(this.BRcboSeriesName_SelectedIndexChanged);
            this.BRcboSeriesName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.BRcboSeriesName_KeyDown);
            // 
            // BRcomboBankAccount
            // 
            this.BRcomboBankAccount.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.BRcomboBankAccount.FocusLostColor = System.Drawing.Color.White;
            this.BRcomboBankAccount.FormattingEnabled = true;
            this.BRcomboBankAccount.Location = new System.Drawing.Point(110, 39);
            this.BRcomboBankAccount.Name = "BRcomboBankAccount";
            this.BRcomboBankAccount.Size = new System.Drawing.Size(188, 21);
            this.BRcomboBankAccount.TabIndex = 114;
            this.BRcomboBankAccount.SelectedIndexChanged += new System.EventHandler(this.comboBankAccount_SelectedIndexChanged);
            this.BRcomboBankAccount.KeyDown += new System.Windows.Forms.KeyEventHandler(this.BRcomboBankAccount_KeyDown);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.BRbtnEdit);
            this.groupBox1.Controls.Add(this.BRbtnNew);
            this.groupBox1.Controls.Add(this.BRbtnCancel);
            this.groupBox1.Controls.Add(this.BRbtnSave);
            this.groupBox1.Controls.Add(this.BRchkDoNotClose);
            this.groupBox1.Controls.Add(this.BRchkPrntWhileSaving);
            this.groupBox1.Controls.Add(this.BRbtnDelete);
            this.groupBox1.Location = new System.Drawing.Point(8, 386);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(839, 48);
            this.groupBox1.TabIndex = 145;
            this.groupBox1.TabStop = false;
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusJournal});
            this.statusStrip1.Location = new System.Drawing.Point(0, 437);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(859, 22);
            this.statusStrip1.TabIndex = 146;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // statusJournal
            // 
            this.statusJournal.Name = "statusJournal";
            this.statusJournal.Size = new System.Drawing.Size(313, 17);
            this.statusJournal.Text = "Press CTRL+R in the Narration Field for Previous Narration";
            // 
            // BRtxtRemarks
            // 
            this.BRtxtRemarks.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BRtxtRemarks.BackColor = System.Drawing.Color.White;
            this.BRtxtRemarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.BRtxtRemarks.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.BRtxtRemarks.FocusLostColor = System.Drawing.Color.White;
            this.BRtxtRemarks.Location = new System.Drawing.Point(104, 362);
            this.BRtxtRemarks.Name = "BRtxtRemarks";
            this.BRtxtRemarks.Size = new System.Drawing.Size(739, 20);
            this.BRtxtRemarks.TabIndex = 100;
            this.BRtxtRemarks.KeyDown += new System.Windows.Forms.KeyEventHandler(this.BRtxtRemarks_KeyDown);
            // 
            // frmBankReceipt
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(859, 459);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.BRtxtRemarks);
            this.Controls.Add(this.tabControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "frmBankReceipt";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Bank Receipt";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmBankReceipt_FormClosing);
            this.Load += new System.EventHandler(this.frmBankReceipt_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmBankReceipt_KeyDown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private SComboBox BRcomboBankAccount;
        private System.Windows.Forms.Button BRbtnCancel;
        private SourceGrid.Grid grdBankReceipt;
        private System.Windows.Forms.CheckBox BRchkDoNotClose;
        private System.Windows.Forms.Button BRbtnEdit;
        private System.Windows.Forms.Button BRbtnDelete;
        private System.Windows.Forms.Label label6;
        private STextBox BRtxtBankID;
        private System.Windows.Forms.CheckBox BRchkPrntWhileSaving;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnPaste;
        private System.Windows.Forms.Button btnCopy;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnLast;
        private System.Windows.Forms.Button btnFirst;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnPrev;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblVouNo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button BRbtnSave;
        private System.Windows.Forms.Button BRbtnNew;
        private SMaskedTextBox BRtxtDate;
        private STextBox BRtxtRemarks;
        private System.Windows.Forms.TabControl tabControl1;
        private STextBox BRtxtVchNo;
        private SComboBox BRcboSeriesName;
        private System.Windows.Forms.Label label8;
        private STreeView treeAccClass;
        private System.Windows.Forms.Button BRbtnAddAccClass;
        private System.Windows.Forms.Label label12;
        private SComboBox BRcboProjectName;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button BRbtnDate;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel statusJournal;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label BRlblBankCurrBalHidden;
        private System.Windows.Forms.Label BRlblBankCurrentBalance;
        private System.Windows.Forms.Label BRlblTotalAmount;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button BRbtn_groupvoucherposting;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.TabPage tabPage3;
        private STextBox BRtxtfifth;
        private STextBox BRtxtfourth;
        private STextBox BRtxtthird;
        private STextBox BRtxtsecond;
        private STextBox BRtxtfirst;
        private System.Windows.Forms.Label BRlblfourth;
        private System.Windows.Forms.Label BRlblsecond;
        private System.Windows.Forms.Label BRlblthird;
        private System.Windows.Forms.Label BRlblfifth;
        private System.Windows.Forms.Label BRlblfirst;
        private System.Windows.Forms.Button btnPrintPreview;
        private System.Windows.Forms.Button btnSetup;
        private System.Windows.Forms.CheckBox BRchkRecurring;
        private System.Windows.Forms.Button btnList;

    }
}