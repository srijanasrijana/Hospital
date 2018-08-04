using SComponents;
namespace Accounts
{
    partial class frmCashPayment
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCashPayment));
            this.btnPaste = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnCopy = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.CPchkPrntWhileSaving = new System.Windows.Forms.CheckBox();
            this.btnLast = new System.Windows.Forms.Button();
            this.btnFirst = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnPrev = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.CPbtnAddAccClass = new System.Windows.Forms.Button();
            this.treeAccClass = new SComponents.STreeView(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnSetup = new System.Windows.Forms.Button();
            this.btnPrintPreview = new System.Windows.Forms.Button();
            this.chkRecurring = new System.Windows.Forms.CheckBox();
            this.btnList = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.CPbtnCancel = new System.Windows.Forms.Button();
            this.CPchkDoNotClose = new System.Windows.Forms.CheckBox();
            this.CPbtnEdit = new System.Windows.Forms.Button();
            this.CPbtnDelete = new System.Windows.Forms.Button();
            this.CPbtnNew = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.grdCashPayment = new SourceGrid.Grid();
            this.CPlblDifferenceAmount = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.CPlblCreditTotal = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.CPlblDebitTotal = new System.Windows.Forms.Label();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.CPtxtfifth = new SComponents.STextBox();
            this.CPtxtfourth = new SComponents.STextBox();
            this.CPtxtthird = new SComponents.STextBox();
            this.CPtxtsecond = new SComponents.STextBox();
            this.CPtxtfirst = new SComponents.STextBox();
            this.CPlblfourth = new System.Windows.Forms.Label();
            this.CPlblsecond = new System.Windows.Forms.Label();
            this.CPlblthird = new System.Windows.Forms.Label();
            this.CPlblfifth = new System.Windows.Forms.Label();
            this.CPlblfirst = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblVouNo = new System.Windows.Forms.Label();
            this.CPbtnSave = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.CPlblCurrentBalance = new System.Windows.Forms.Label();
            this.CPbtn_groupvoucherposting = new System.Windows.Forms.Button();
            this.CPbtnDate = new System.Windows.Forms.Button();
            this.CPcboSeriesName = new SComponents.SComboBox();
            this.CPcboProjectName = new SComponents.SComboBox();
            this.CPtxtVchNo = new SComponents.STextBox();
            this.CPtxtDate = new SComponents.SMaskedTextBox();
            this.CPtxtCashPaymentID = new SComponents.STextBox();
            this.CPcmboCashAccount = new SComponents.SComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.statusJournal = new System.Windows.Forms.ToolStripStatusLabel();
            this.CPtxtRemarks = new SComponents.STextBox();
            this.tabPage2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnPaste
            // 
            this.btnPaste.Image = global::Accounts.Properties.Resources.paste;
            this.btnPaste.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPaste.Location = new System.Drawing.Point(353, 3);
            this.btnPaste.Name = "btnPaste";
            this.btnPaste.Size = new System.Drawing.Size(58, 23);
            this.btnPaste.TabIndex = 24;
            this.btnPaste.Text = "Paste";
            this.btnPaste.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnPaste.UseVisualStyleBackColor = true;
            this.btnPaste.Click += new System.EventHandler(this.btnPaste_Click);
            // 
            // btnExit
            // 
            this.btnExit.Image = global::Accounts.Properties.Resources.ExitButton;
            this.btnExit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnExit.Location = new System.Drawing.Point(566, 3);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(58, 23);
            this.btnExit.TabIndex = 22;
            this.btnExit.Text = "Exit";
            this.btnExit.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.CPbtnExit_Click);
            // 
            // btnCopy
            // 
            this.btnCopy.Image = global::Accounts.Properties.Resources.copy;
            this.btnCopy.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCopy.Location = new System.Drawing.Point(294, 3);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(58, 23);
            this.btnCopy.TabIndex = 23;
            this.btnCopy.Text = "Copy";
            this.btnCopy.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCopy.UseVisualStyleBackColor = true;
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(52, 48);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(77, 13);
            this.label6.TabIndex = 59;
            this.label6.Text = "Cash Account:";
            // 
            // CPchkPrntWhileSaving
            // 
            this.CPchkPrntWhileSaving.AutoSize = true;
            this.CPchkPrntWhileSaving.Location = new System.Drawing.Point(99, 18);
            this.CPchkPrntWhileSaving.Name = "CPchkPrntWhileSaving";
            this.CPchkPrntWhileSaving.Size = new System.Drawing.Size(108, 17);
            this.CPchkPrntWhileSaving.TabIndex = 57;
            this.CPchkPrntWhileSaving.Text = "Print while saving";
            this.CPchkPrntWhileSaving.UseVisualStyleBackColor = true;
            this.CPchkPrntWhileSaving.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CPchkPrntWhileSaving_KeyDown);
            // 
            // btnLast
            // 
            this.btnLast.Image = global::Accounts.Properties.Resources.last;
            this.btnLast.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnLast.Location = new System.Drawing.Point(181, 3);
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
            this.btnPrint.Location = new System.Drawing.Point(412, 3);
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
            this.btnNext.Location = new System.Drawing.Point(122, 3);
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
            this.btnPrev.Location = new System.Drawing.Point(63, 3);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(58, 23);
            this.btnPrev.TabIndex = 0;
            this.btnPrev.Text = "&Prev";
            this.btnPrev.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnPrev.UseVisualStyleBackColor = true;
            this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.CPbtnAddAccClass);
            this.tabPage2.Controls.Add(this.treeAccClass);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(842, 175);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Account Class";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // CPbtnAddAccClass
            // 
            this.CPbtnAddAccClass.Location = new System.Drawing.Point(378, 6);
            this.CPbtnAddAccClass.Name = "CPbtnAddAccClass";
            this.CPbtnAddAccClass.Size = new System.Drawing.Size(109, 23);
            this.CPbtnAddAccClass.TabIndex = 31;
            this.CPbtnAddAccClass.Text = "Add Account Class";
            this.CPbtnAddAccClass.UseVisualStyleBackColor = true;
            this.CPbtnAddAccClass.Click += new System.EventHandler(this.btnAddAccClass_Click);
            // 
            // treeAccClass
            // 
            this.treeAccClass.AutoCheckChild = true;
            this.treeAccClass.CheckBoxes = true;
            this.treeAccClass.Location = new System.Drawing.Point(3, 6);
            this.treeAccClass.Name = "treeAccClass";
            this.treeAccClass.Size = new System.Drawing.Size(369, 169);
            this.treeAccClass.TabIndex = 28;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.CadetBlue;
            this.panel1.Controls.Add(this.btnSetup);
            this.panel1.Controls.Add(this.btnPrintPreview);
            this.panel1.Controls.Add(this.chkRecurring);
            this.panel1.Controls.Add(this.btnList);
            this.panel1.Controls.Add(this.btnExport);
            this.panel1.Controls.Add(this.btnPaste);
            this.panel1.Controls.Add(this.btnLast);
            this.panel1.Controls.Add(this.btnCopy);
            this.panel1.Controls.Add(this.btnFirst);
            this.panel1.Controls.Add(this.btnPrint);
            this.panel1.Controls.Add(this.btnNext);
            this.panel1.Controls.Add(this.btnExit);
            this.panel1.Controls.Add(this.btnPrev);
            this.panel1.Location = new System.Drawing.Point(-1, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(868, 32);
            this.panel1.TabIndex = 55;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // btnSetup
            // 
            this.btnSetup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSetup.Location = new System.Drawing.Point(744, 3);
            this.btnSetup.Name = "btnSetup";
            this.btnSetup.Size = new System.Drawing.Size(43, 23);
            this.btnSetup.TabIndex = 161;
            this.btnSetup.Text = "setup";
            this.btnSetup.UseVisualStyleBackColor = true;
            this.btnSetup.Click += new System.EventHandler(this.btnSetup_Click);
            // 
            // btnPrintPreview
            // 
            this.btnPrintPreview.Image = global::Accounts.Properties.Resources.print_preview;
            this.btnPrintPreview.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPrintPreview.Location = new System.Drawing.Point(471, 3);
            this.btnPrintPreview.Name = "btnPrintPreview";
            this.btnPrintPreview.Size = new System.Drawing.Size(94, 23);
            this.btnPrintPreview.TabIndex = 31;
            this.btnPrintPreview.Text = "Pr&int Preview";
            this.btnPrintPreview.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnPrintPreview.UseVisualStyleBackColor = true;
            this.btnPrintPreview.Click += new System.EventHandler(this.btnPrintPreview_Click);
            // 
            // chkRecurring
            // 
            this.chkRecurring.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkRecurring.AutoSize = true;
            this.chkRecurring.ForeColor = System.Drawing.Color.White;
            this.chkRecurring.Location = new System.Drawing.Point(674, 6);
            this.chkRecurring.Margin = new System.Windows.Forms.Padding(2);
            this.chkRecurring.Name = "chkRecurring";
            this.chkRecurring.Size = new System.Drawing.Size(72, 17);
            this.chkRecurring.TabIndex = 160;
            this.chkRecurring.Text = "Recurrin&g";
            this.chkRecurring.UseVisualStyleBackColor = true;
            this.chkRecurring.CheckedChanged += new System.EventHandler(this.CPchkRecurring_CheckedChanged);
            // 
            // btnList
            // 
            this.btnList.Location = new System.Drawing.Point(240, 3);
            this.btnList.Name = "btnList";
            this.btnList.Size = new System.Drawing.Size(53, 23);
            this.btnList.TabIndex = 154;
            this.btnList.Text = "≡ List";
            this.btnList.UseVisualStyleBackColor = true;
            this.btnList.Click += new System.EventHandler(this.btnList_Click);
            // 
            // btnExport
            // 
            this.btnExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExport.Enabled = false;
            this.btnExport.Image = global::Accounts.Properties.Resources.export1;
            this.btnExport.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnExport.Location = new System.Drawing.Point(793, 3);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(62, 23);
            this.btnExport.TabIndex = 27;
            this.btnExport.Text = "&Export";
            this.btnExport.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // CPbtnCancel
            // 
            this.CPbtnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CPbtnCancel.Image = global::Accounts.Properties.Resources.gnome_window_close;
            this.CPbtnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.CPbtnCancel.Location = new System.Drawing.Point(764, 12);
            this.CPbtnCancel.Name = "CPbtnCancel";
            this.CPbtnCancel.Size = new System.Drawing.Size(75, 30);
            this.CPbtnCancel.TabIndex = 54;
            this.CPbtnCancel.Text = "&Cancel";
            this.CPbtnCancel.UseVisualStyleBackColor = true;
            this.CPbtnCancel.Click += new System.EventHandler(this.CPbtnCancel_Click);
            // 
            // CPchkDoNotClose
            // 
            this.CPchkDoNotClose.AutoSize = true;
            this.CPchkDoNotClose.Location = new System.Drawing.Point(7, 18);
            this.CPchkDoNotClose.Name = "CPchkDoNotClose";
            this.CPchkDoNotClose.Size = new System.Drawing.Size(86, 17);
            this.CPchkDoNotClose.TabIndex = 56;
            this.CPchkDoNotClose.Text = "Do not close";
            this.CPchkDoNotClose.UseVisualStyleBackColor = true;
            this.CPchkDoNotClose.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CPchkDoNotClose_KeyDown);
            // 
            // CPbtnEdit
            // 
            this.CPbtnEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CPbtnEdit.Image = global::Accounts.Properties.Resources.document_edit;
            this.CPbtnEdit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.CPbtnEdit.Location = new System.Drawing.Point(520, 12);
            this.CPbtnEdit.Name = "CPbtnEdit";
            this.CPbtnEdit.Size = new System.Drawing.Size(75, 30);
            this.CPbtnEdit.TabIndex = 53;
            this.CPbtnEdit.Text = "&Edit";
            this.CPbtnEdit.UseVisualStyleBackColor = true;
            this.CPbtnEdit.Click += new System.EventHandler(this.CPbtnEdit_Click);
            // 
            // CPbtnDelete
            // 
            this.CPbtnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CPbtnDelete.Image = global::Accounts.Properties.Resources.document_delete;
            this.CPbtnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.CPbtnDelete.Location = new System.Drawing.Point(683, 12);
            this.CPbtnDelete.Name = "CPbtnDelete";
            this.CPbtnDelete.Size = new System.Drawing.Size(75, 30);
            this.CPbtnDelete.TabIndex = 52;
            this.CPbtnDelete.Text = "&Delete";
            this.CPbtnDelete.UseVisualStyleBackColor = true;
            this.CPbtnDelete.Click += new System.EventHandler(this.CPbtnDelete_Click);
            // 
            // CPbtnNew
            // 
            this.CPbtnNew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CPbtnNew.Image = global::Accounts.Properties.Resources.edit_add;
            this.CPbtnNew.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.CPbtnNew.Location = new System.Drawing.Point(439, 12);
            this.CPbtnNew.Name = "CPbtnNew";
            this.CPbtnNew.Size = new System.Drawing.Size(75, 30);
            this.CPbtnNew.TabIndex = 50;
            this.CPbtnNew.Text = "&New";
            this.CPbtnNew.UseVisualStyleBackColor = true;
            this.CPbtnNew.Click += new System.EventHandler(this.CPbtnNew_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(666, 21);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(33, 13);
            this.label3.TabIndex = 48;
            this.label3.Text = "Date:";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(4, 138);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(850, 201);
            this.tabControl1.TabIndex = 43;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.grdCashPayment);
            this.tabPage1.Controls.Add(this.CPlblDifferenceAmount);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.label5);
            this.tabPage1.Controls.Add(this.CPlblCreditTotal);
            this.tabPage1.Controls.Add(this.label7);
            this.tabPage1.Controls.Add(this.CPlblDebitTotal);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(842, 175);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Details";
            this.tabPage1.UseVisualStyleBackColor = true;
            this.tabPage1.Click += new System.EventHandler(this.tabPage1_Click);
            // 
            // grdCashPayment
            // 
            this.grdCashPayment.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdCashPayment.EnableSort = true;
            this.grdCashPayment.Location = new System.Drawing.Point(3, 3);
            this.grdCashPayment.Name = "grdCashPayment";
            this.grdCashPayment.OptimizeMode = SourceGrid.CellOptimizeMode.ForRows;
            this.grdCashPayment.SelectionMode = SourceGrid.GridSelectionMode.Cell;
            this.grdCashPayment.Size = new System.Drawing.Size(839, 144);
            this.grdCashPayment.TabIndex = 1;
            this.grdCashPayment.TabStop = true;
            this.grdCashPayment.ToolTipText = "";
            // 
            // CPlblDifferenceAmount
            // 
            this.CPlblDifferenceAmount.Location = new System.Drawing.Point(593, 151);
            this.CPlblDifferenceAmount.Name = "CPlblDifferenceAmount";
            this.CPlblDifferenceAmount.Size = new System.Drawing.Size(103, 13);
            this.CPlblDifferenceAmount.TabIndex = 23;
            this.CPlblDifferenceAmount.Text = "0.00";
            this.CPlblDifferenceAmount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.CPlblDifferenceAmount.Click += new System.EventHandler(this.lblDifferenceAmount_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(128, 151);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(62, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Debit Total:";
            this.label4.Click += new System.EventHandler(this.label4_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(309, 151);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(64, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Credit Total:";
            // 
            // CPlblCreditTotal
            // 
            this.CPlblCreditTotal.Location = new System.Drawing.Point(379, 151);
            this.CPlblCreditTotal.Name = "CPlblCreditTotal";
            this.CPlblCreditTotal.Size = new System.Drawing.Size(138, 13);
            this.CPlblCreditTotal.TabIndex = 12;
            this.CPlblCreditTotal.Text = "0.00";
            this.CPlblCreditTotal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.CPlblCreditTotal.Click += new System.EventHandler(this.lblCreditTotal_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(523, 151);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(59, 13);
            this.label7.TabIndex = 22;
            this.label7.Text = "Difference:";
            this.label7.Click += new System.EventHandler(this.label7_Click);
            // 
            // CPlblDebitTotal
            // 
            this.CPlblDebitTotal.Location = new System.Drawing.Point(196, 151);
            this.CPlblDebitTotal.Name = "CPlblDebitTotal";
            this.CPlblDebitTotal.Size = new System.Drawing.Size(108, 13);
            this.CPlblDebitTotal.TabIndex = 11;
            this.CPlblDebitTotal.Text = "0.00";
            this.CPlblDebitTotal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.CPtxtfifth);
            this.tabPage3.Controls.Add(this.CPtxtfourth);
            this.tabPage3.Controls.Add(this.CPtxtthird);
            this.tabPage3.Controls.Add(this.CPtxtsecond);
            this.tabPage3.Controls.Add(this.CPtxtfirst);
            this.tabPage3.Controls.Add(this.CPlblfourth);
            this.tabPage3.Controls.Add(this.CPlblsecond);
            this.tabPage3.Controls.Add(this.CPlblthird);
            this.tabPage3.Controls.Add(this.CPlblfifth);
            this.tabPage3.Controls.Add(this.CPlblfirst);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(842, 175);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Additional Fields";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // CPtxtfifth
            // 
            this.CPtxtfifth.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.CPtxtfifth.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.CPtxtfifth.FocusLostColor = System.Drawing.Color.White;
            this.CPtxtfifth.Location = new System.Drawing.Point(112, 135);
            this.CPtxtfifth.Name = "CPtxtfifth";
            this.CPtxtfifth.Size = new System.Drawing.Size(182, 20);
            this.CPtxtfifth.TabIndex = 40;
            this.CPtxtfifth.Visible = false;
            // 
            // CPtxtfourth
            // 
            this.CPtxtfourth.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.CPtxtfourth.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.CPtxtfourth.FocusLostColor = System.Drawing.Color.White;
            this.CPtxtfourth.Location = new System.Drawing.Point(112, 106);
            this.CPtxtfourth.Name = "CPtxtfourth";
            this.CPtxtfourth.Size = new System.Drawing.Size(182, 20);
            this.CPtxtfourth.TabIndex = 39;
            this.CPtxtfourth.Visible = false;
            // 
            // CPtxtthird
            // 
            this.CPtxtthird.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.CPtxtthird.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.CPtxtthird.FocusLostColor = System.Drawing.Color.White;
            this.CPtxtthird.Location = new System.Drawing.Point(112, 73);
            this.CPtxtthird.Name = "CPtxtthird";
            this.CPtxtthird.Size = new System.Drawing.Size(182, 20);
            this.CPtxtthird.TabIndex = 38;
            this.CPtxtthird.Visible = false;
            // 
            // CPtxtsecond
            // 
            this.CPtxtsecond.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.CPtxtsecond.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.CPtxtsecond.FocusLostColor = System.Drawing.Color.White;
            this.CPtxtsecond.Location = new System.Drawing.Point(112, 41);
            this.CPtxtsecond.Name = "CPtxtsecond";
            this.CPtxtsecond.Size = new System.Drawing.Size(182, 20);
            this.CPtxtsecond.TabIndex = 37;
            this.CPtxtsecond.Visible = false;
            // 
            // CPtxtfirst
            // 
            this.CPtxtfirst.BackColor = System.Drawing.Color.White;
            this.CPtxtfirst.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.CPtxtfirst.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.CPtxtfirst.FocusLostColor = System.Drawing.Color.White;
            this.CPtxtfirst.Location = new System.Drawing.Point(112, 13);
            this.CPtxtfirst.Name = "CPtxtfirst";
            this.CPtxtfirst.Size = new System.Drawing.Size(182, 20);
            this.CPtxtfirst.TabIndex = 36;
            this.CPtxtfirst.Visible = false;
            // 
            // CPlblfourth
            // 
            this.CPlblfourth.AutoSize = true;
            this.CPlblfourth.Location = new System.Drawing.Point(12, 112);
            this.CPlblfourth.Name = "CPlblfourth";
            this.CPlblfourth.Size = new System.Drawing.Size(80, 13);
            this.CPlblfourth.TabIndex = 35;
            this.CPlblfourth.Text = "Optional Field 4";
            this.CPlblfourth.Visible = false;
            // 
            // CPlblsecond
            // 
            this.CPlblsecond.AutoSize = true;
            this.CPlblsecond.Location = new System.Drawing.Point(12, 47);
            this.CPlblsecond.Name = "CPlblsecond";
            this.CPlblsecond.Size = new System.Drawing.Size(80, 13);
            this.CPlblsecond.TabIndex = 34;
            this.CPlblsecond.Text = "Optional Field 2";
            this.CPlblsecond.Visible = false;
            // 
            // CPlblthird
            // 
            this.CPlblthird.AutoSize = true;
            this.CPlblthird.Location = new System.Drawing.Point(12, 78);
            this.CPlblthird.Name = "CPlblthird";
            this.CPlblthird.Size = new System.Drawing.Size(80, 13);
            this.CPlblthird.TabIndex = 33;
            this.CPlblthird.Text = "Optional Field 3";
            this.CPlblthird.Visible = false;
            // 
            // CPlblfifth
            // 
            this.CPlblfifth.AutoSize = true;
            this.CPlblfifth.Location = new System.Drawing.Point(12, 141);
            this.CPlblfifth.Name = "CPlblfifth";
            this.CPlblfifth.Size = new System.Drawing.Size(80, 13);
            this.CPlblfifth.TabIndex = 32;
            this.CPlblfifth.Text = "Optional Field 5";
            this.CPlblfifth.Visible = false;
            // 
            // CPlblfirst
            // 
            this.CPlblfirst.AutoSize = true;
            this.CPlblfirst.Location = new System.Drawing.Point(12, 14);
            this.CPlblfirst.Name = "CPlblfirst";
            this.CPlblfirst.Size = new System.Drawing.Size(80, 13);
            this.CPlblfirst.TabIndex = 31;
            this.CPlblfirst.Text = "Optional Field 1";
            this.CPlblfirst.Visible = false;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 348);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 47;
            this.label2.Text = "Narration:";
            // 
            // lblVouNo
            // 
            this.lblVouNo.AutoSize = true;
            this.lblVouNo.Location = new System.Drawing.Point(372, 21);
            this.lblVouNo.Name = "lblVouNo";
            this.lblVouNo.Size = new System.Drawing.Size(60, 13);
            this.lblVouNo.TabIndex = 45;
            this.lblVouNo.Text = "Voucher #:";
            // 
            // CPbtnSave
            // 
            this.CPbtnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CPbtnSave.Image = global::Accounts.Properties.Resources.save;
            this.CPbtnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.CPbtnSave.Location = new System.Drawing.Point(601, 12);
            this.CPbtnSave.Name = "CPbtnSave";
            this.CPbtnSave.Size = new System.Drawing.Size(75, 30);
            this.CPbtnSave.TabIndex = 51;
            this.CPbtnSave.Text = "&Save";
            this.CPbtnSave.UseVisualStyleBackColor = true;
            this.CPbtnSave.Click += new System.EventHandler(this.CPbtnSave_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(52, 19);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(42, 13);
            this.label8.TabIndex = 62;
            this.label8.Text = "Series :";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(50, 75);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(46, 13);
            this.label12.TabIndex = 70;
            this.label12.Text = "Project :";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.CPlblCurrentBalance);
            this.groupBox2.Controls.Add(this.CPbtn_groupvoucherposting);
            this.groupBox2.Controls.Add(this.CPbtnDate);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.CPcboSeriesName);
            this.groupBox2.Controls.Add(this.CPcboProjectName);
            this.groupBox2.Controls.Add(this.CPtxtVchNo);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.lblVouNo);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.CPtxtDate);
            this.groupBox2.Controls.Add(this.CPtxtCashPaymentID);
            this.groupBox2.Controls.Add(this.CPcmboCashAccount);
            this.groupBox2.Location = new System.Drawing.Point(4, 34);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(846, 99);
            this.groupBox2.TabIndex = 71;
            this.groupBox2.TabStop = false;
            this.groupBox2.Enter += new System.EventHandler(this.groupBox2_Enter);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(346, 50);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(86, 13);
            this.label9.TabIndex = 155;
            this.label9.Text = "Current Balance:";
            // 
            // CPlblCurrentBalance
            // 
            this.CPlblCurrentBalance.AutoSize = true;
            this.CPlblCurrentBalance.Location = new System.Drawing.Point(435, 50);
            this.CPlblCurrentBalance.Name = "CPlblCurrentBalance";
            this.CPlblCurrentBalance.Size = new System.Drawing.Size(28, 13);
            this.CPlblCurrentBalance.TabIndex = 154;
            this.CPlblCurrentBalance.Text = "0.00";
            // 
            // CPbtn_groupvoucherposting
            // 
            this.CPbtn_groupvoucherposting.Location = new System.Drawing.Point(683, 48);
            this.CPbtn_groupvoucherposting.Name = "CPbtn_groupvoucherposting";
            this.CPbtn_groupvoucherposting.Size = new System.Drawing.Size(138, 23);
            this.CPbtn_groupvoucherposting.TabIndex = 71;
            this.CPbtn_groupvoucherposting.Text = "Group  Voucher Posting";
            this.CPbtn_groupvoucherposting.UseVisualStyleBackColor = true;
            this.CPbtn_groupvoucherposting.Click += new System.EventHandler(this.CPbtn_groupvoucherposting_Click);
            // 
            // CPbtnDate
            // 
            this.CPbtnDate.Image = global::Accounts.Properties.Resources.dateIcon;
            this.CPbtnDate.Location = new System.Drawing.Point(795, 16);
            this.CPbtnDate.Name = "CPbtnDate";
            this.CPbtnDate.Size = new System.Drawing.Size(26, 23);
            this.CPbtnDate.TabIndex = 3;
            this.CPbtnDate.UseVisualStyleBackColor = true;
            this.CPbtnDate.Click += new System.EventHandler(this.CPbtnDate_Click);
            // 
            // CPcboSeriesName
            // 
            this.CPcboSeriesName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.CPcboSeriesName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.CPcboSeriesName.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.CPcboSeriesName.FocusLostColor = System.Drawing.Color.White;
            this.CPcboSeriesName.FormattingEnabled = true;
            this.CPcboSeriesName.Location = new System.Drawing.Point(135, 16);
            this.CPcboSeriesName.Name = "CPcboSeriesName";
            this.CPcboSeriesName.Size = new System.Drawing.Size(205, 21);
            this.CPcboSeriesName.TabIndex = 61;
            this.CPcboSeriesName.SelectedIndexChanged += new System.EventHandler(this.CPcboSeriesName_SelectedIndexChanged);
            this.CPcboSeriesName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CPcboSeriesName_KeyDown);
            // 
            // CPcboProjectName
            // 
            this.CPcboProjectName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.CPcboProjectName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.CPcboProjectName.BackColor = System.Drawing.Color.White;
            this.CPcboProjectName.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.CPcboProjectName.FocusLostColor = System.Drawing.Color.White;
            this.CPcboProjectName.FormattingEnabled = true;
            this.CPcboProjectName.Location = new System.Drawing.Point(135, 72);
            this.CPcboProjectName.Name = "CPcboProjectName";
            this.CPcboProjectName.Size = new System.Drawing.Size(205, 21);
            this.CPcboProjectName.TabIndex = 69;
            this.CPcboProjectName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CPcboProjectName_KeyDown);
            // 
            // CPtxtVchNo
            // 
            this.CPtxtVchNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.CPtxtVchNo.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.CPtxtVchNo.FocusLostColor = System.Drawing.Color.White;
            this.CPtxtVchNo.Location = new System.Drawing.Point(438, 19);
            this.CPtxtVchNo.Name = "CPtxtVchNo";
            this.CPtxtVchNo.Size = new System.Drawing.Size(181, 20);
            this.CPtxtVchNo.TabIndex = 44;
            // 
            // CPtxtDate
            // 
            this.CPtxtDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.CPtxtDate.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.CPtxtDate.FocusLostColor = System.Drawing.Color.White;
            this.CPtxtDate.Location = new System.Drawing.Point(700, 19);
            this.CPtxtDate.Mask = "0000/00/00";
            this.CPtxtDate.Name = "CPtxtDate";
            this.CPtxtDate.Size = new System.Drawing.Size(89, 20);
            this.CPtxtDate.TabIndex = 49;
            this.CPtxtDate.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CPtxtDate_KeyDown);
            // 
            // CPtxtCashPaymentID
            // 
            this.CPtxtCashPaymentID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.CPtxtCashPaymentID.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.CPtxtCashPaymentID.FocusLostColor = System.Drawing.Color.White;
            this.CPtxtCashPaymentID.Location = new System.Drawing.Point(738, 73);
            this.CPtxtCashPaymentID.Name = "CPtxtCashPaymentID";
            this.CPtxtCashPaymentID.Size = new System.Drawing.Size(88, 20);
            this.CPtxtCashPaymentID.TabIndex = 58;
            this.CPtxtCashPaymentID.Visible = false;
            // 
            // CPcmboCashAccount
            // 
            this.CPcmboCashAccount.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.CPcmboCashAccount.FocusLostColor = System.Drawing.Color.White;
            this.CPcmboCashAccount.FormattingEnabled = true;
            this.CPcmboCashAccount.Location = new System.Drawing.Point(135, 45);
            this.CPcmboCashAccount.Name = "CPcmboCashAccount";
            this.CPcmboCashAccount.Size = new System.Drawing.Size(205, 21);
            this.CPcmboCashAccount.TabIndex = 60;
            this.CPcmboCashAccount.SelectedIndexChanged += new System.EventHandler(this.CPcmboCashAccount_SelectedIndexChanged);
            this.CPcmboCashAccount.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CPcmboCashAccount_KeyDown);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.CPbtnEdit);
            this.groupBox1.Controls.Add(this.CPbtnSave);
            this.groupBox1.Controls.Add(this.CPchkPrntWhileSaving);
            this.groupBox1.Controls.Add(this.CPbtnNew);
            this.groupBox1.Controls.Add(this.CPbtnDelete);
            this.groupBox1.Controls.Add(this.CPbtnCancel);
            this.groupBox1.Controls.Add(this.CPchkDoNotClose);
            this.groupBox1.Location = new System.Drawing.Point(4, 369);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(846, 48);
            this.groupBox1.TabIndex = 146;
            this.groupBox1.TabStop = false;
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusJournal});
            this.statusStrip1.Location = new System.Drawing.Point(0, 417);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(866, 22);
            this.statusStrip1.TabIndex = 147;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // statusJournal
            // 
            this.statusJournal.Name = "statusJournal";
            this.statusJournal.Size = new System.Drawing.Size(313, 17);
            this.statusJournal.Text = "Press CTRL+R in the Narration Field for Previous Narration";
            // 
            // CPtxtRemarks
            // 
            this.CPtxtRemarks.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CPtxtRemarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.CPtxtRemarks.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.CPtxtRemarks.FocusLostColor = System.Drawing.Color.White;
            this.CPtxtRemarks.Location = new System.Drawing.Point(103, 346);
            this.CPtxtRemarks.Name = "CPtxtRemarks";
            this.CPtxtRemarks.Size = new System.Drawing.Size(747, 20);
            this.CPtxtRemarks.TabIndex = 46;
            this.CPtxtRemarks.TextChanged += new System.EventHandler(this.txtRemarks_TextChanged);
            this.CPtxtRemarks.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CPtxtRemarks_KeyDown);
            // 
            // frmCashPayment
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(866, 439);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.CPtxtRemarks);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.label2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "frmCashPayment";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Cash Payment";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmCashPayment_FormClosing);
            this.Load += new System.EventHandler(this.frmCashPayment_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmCashPayment_KeyDown);
            this.tabPage2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnPaste;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnCopy;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox CPchkPrntWhileSaving;
        private System.Windows.Forms.Button btnLast;
        private System.Windows.Forms.Button btnFirst;
        private SComboBox CPcmboCashAccount;
        private STextBox CPtxtCashPaymentID;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnPrev;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button CPbtnCancel;
        private System.Windows.Forms.CheckBox CPchkDoNotClose;
        private System.Windows.Forms.Button CPbtnEdit;
        private System.Windows.Forms.Button CPbtnDelete;
        private System.Windows.Forms.Button CPbtnNew;
        private SMaskedTextBox CPtxtDate;
        private System.Windows.Forms.Label label3;
        private STextBox CPtxtRemarks;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private SourceGrid.Grid grdCashPayment;
        private System.Windows.Forms.Label CPlblDifferenceAmount;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label CPlblDebitTotal;
        private System.Windows.Forms.Label CPlblCreditTotal;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblVouNo;
        private System.Windows.Forms.Button CPbtnSave;
        private SComboBox CPcboSeriesName;
        private System.Windows.Forms.Label label8;
        private STreeView treeAccClass;
        private System.Windows.Forms.Button CPbtnAddAccClass;
        private System.Windows.Forms.Label label12;
        private SComboBox CPcboProjectName;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button CPbtnDate;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel statusJournal;
        private System.Windows.Forms.Button CPbtn_groupvoucherposting;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label CPlblCurrentBalance;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.TabPage tabPage3;
        private STextBox CPtxtfifth;
        private STextBox CPtxtfourth;
        private STextBox CPtxtthird;
        private STextBox CPtxtsecond;
        private STextBox CPtxtfirst;
        private System.Windows.Forms.Label CPlblfourth;
        private System.Windows.Forms.Label CPlblsecond;
        private System.Windows.Forms.Label CPlblthird;
        private System.Windows.Forms.Label CPlblfifth;
        private System.Windows.Forms.Label CPlblfirst;
        private System.Windows.Forms.Button btnPrintPreview;
        private System.Windows.Forms.Button btnSetup;
        private System.Windows.Forms.CheckBox chkRecurring;
        private System.Windows.Forms.Button btnList;
        private STextBox CPtxtVchNo;
    }
}