using SComponents;
namespace Accounts
{
    partial class frmCashReceipt
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCashReceipt));
            this.CRchkPrntWhileSaving = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnSetup = new System.Windows.Forms.Button();
            this.btnList = new System.Windows.Forms.Button();
            this.CRchkRecurring = new System.Windows.Forms.CheckBox();
            this.btnPrintPreview = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnPaste = new System.Windows.Forms.Button();
            this.btnCopy = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnLast = new System.Windows.Forms.Button();
            this.btnFirst = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnPrev = new System.Windows.Forms.Button();
            this.CRbtnCancel = new System.Windows.Forms.Button();
            this.CRchkDoNotClose = new System.Windows.Forms.CheckBox();
            this.CRbtnEdit = new System.Windows.Forms.Button();
            this.CRbtnDelete = new System.Windows.Forms.Button();
            this.CRbtnNew = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblVouNo = new System.Windows.Forms.Label();
            this.CRbtnSave = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.CRcboProjectName = new SComponents.SComboBox();
            this.CRcboSeriesName = new SComponents.SComboBox();
            this.CRcmboCashAccount = new SComponents.SComboBox();
            this.CRtxtCashReceiptID = new SComponents.STextBox();
            this.CRtxtDate = new SComponents.SMaskedTextBox();
            this.CRtxtRemarks = new SComponents.STextBox();
            this.CRtxtVchNo = new SComponents.STextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.CRbtnAddAccClass = new System.Windows.Forms.Button();
            this.treeAccClass = new SComponents.STreeView(this.components);
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.grdCashReceipt = new SourceGrid.Grid();
            this.lblDifferenceAmount = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblDebitTotal = new System.Windows.Forms.Label();
            this.lblCreditTotal = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.CRtxtfifth = new SComponents.STextBox();
            this.CRtxtfourth = new SComponents.STextBox();
            this.CRtxtthird = new SComponents.STextBox();
            this.CRtxtsecond = new SComponents.STextBox();
            this.CRtxtfirst = new SComponents.STextBox();
            this.CRlblfourth = new System.Windows.Forms.Label();
            this.CRlblsecond = new System.Windows.Forms.Label();
            this.CRlblthird = new System.Windows.Forms.Label();
            this.CRlblfifth = new System.Windows.Forms.Label();
            this.CRlblfirst = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label10 = new System.Windows.Forms.Label();
            this.CRlblCurrentBalance = new System.Windows.Forms.Label();
            this.CRbtn_groupvoucherposting = new System.Windows.Forms.Button();
            this.CRbtnDate = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.statusJournal = new System.Windows.Forms.ToolStripStatusLabel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblCurrBal = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // CRchkPrntWhileSaving
            // 
            this.CRchkPrntWhileSaving.AutoSize = true;
            this.CRchkPrntWhileSaving.Location = new System.Drawing.Point(101, 19);
            this.CRchkPrntWhileSaving.Name = "CRchkPrntWhileSaving";
            this.CRchkPrntWhileSaving.Size = new System.Drawing.Size(108, 17);
            this.CRchkPrntWhileSaving.TabIndex = 1;
            this.CRchkPrntWhileSaving.Text = "Print while saving";
            this.CRchkPrntWhileSaving.UseVisualStyleBackColor = true;
            this.CRchkPrntWhileSaving.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CRchkPrntWhileSaving_KeyDown);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.CadetBlue;
            this.panel1.Controls.Add(this.btnSetup);
            this.panel1.Controls.Add(this.btnList);
            this.panel1.Controls.Add(this.CRchkRecurring);
            this.panel1.Controls.Add(this.btnPrintPreview);
            this.panel1.Controls.Add(this.btnExport);
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
            this.panel1.Size = new System.Drawing.Size(866, 33);
            this.panel1.TabIndex = 37;
            // 
            // btnSetup
            // 
            this.btnSetup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSetup.Location = new System.Drawing.Point(744, 5);
            this.btnSetup.Name = "btnSetup";
            this.btnSetup.Size = new System.Drawing.Size(43, 23);
            this.btnSetup.TabIndex = 159;
            this.btnSetup.Text = "setup";
            this.btnSetup.UseVisualStyleBackColor = true;
            this.btnSetup.Click += new System.EventHandler(this.btnSetup_Click);
            // 
            // btnList
            // 
            this.btnList.Location = new System.Drawing.Point(244, 4);
            this.btnList.Name = "btnList";
            this.btnList.Size = new System.Drawing.Size(53, 23);
            this.btnList.TabIndex = 153;
            this.btnList.Text = "≡ List";
            this.btnList.UseVisualStyleBackColor = true;
            this.btnList.Click += new System.EventHandler(this.btnList_Click);
            // 
            // CRchkRecurring
            // 
            this.CRchkRecurring.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CRchkRecurring.AutoSize = true;
            this.CRchkRecurring.ForeColor = System.Drawing.Color.White;
            this.CRchkRecurring.Location = new System.Drawing.Point(672, 8);
            this.CRchkRecurring.Margin = new System.Windows.Forms.Padding(2);
            this.CRchkRecurring.Name = "CRchkRecurring";
            this.CRchkRecurring.Size = new System.Drawing.Size(72, 17);
            this.CRchkRecurring.TabIndex = 158;
            this.CRchkRecurring.Text = "Recurrin&g";
            this.CRchkRecurring.UseVisualStyleBackColor = true;
            this.CRchkRecurring.CheckedChanged += new System.EventHandler(this.CRchkRecurring_CheckedChanged);
            // 
            // btnPrintPreview
            // 
            this.btnPrintPreview.Image = global::Accounts.Properties.Resources.print_preview;
            this.btnPrintPreview.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPrintPreview.Location = new System.Drawing.Point(479, 4);
            this.btnPrintPreview.Name = "btnPrintPreview";
            this.btnPrintPreview.Size = new System.Drawing.Size(94, 23);
            this.btnPrintPreview.TabIndex = 30;
            this.btnPrintPreview.Text = "Pr&int Preview";
            this.btnPrintPreview.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnPrintPreview.UseVisualStyleBackColor = true;
            this.btnPrintPreview.Click += new System.EventHandler(this.CRbtnPrintPreview_Click);
            // 
            // btnExport
            // 
            this.btnExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExport.Enabled = false;
            this.btnExport.Image = global::Accounts.Properties.Resources.export1;
            this.btnExport.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnExport.Location = new System.Drawing.Point(795, 4);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(60, 23);
            this.btnExport.TabIndex = 27;
            this.btnExport.Text = "&Export";
            this.btnExport.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnPaste
            // 
            this.btnPaste.Image = global::Accounts.Properties.Resources.paste;
            this.btnPaste.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPaste.Location = new System.Drawing.Point(359, 4);
            this.btnPaste.Name = "btnPaste";
            this.btnPaste.Size = new System.Drawing.Size(58, 23);
            this.btnPaste.TabIndex = 24;
            this.btnPaste.Text = "Paste";
            this.btnPaste.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnPaste.UseVisualStyleBackColor = true;
            this.btnPaste.Click += new System.EventHandler(this.btnPaste_Click);
            // 
            // btnCopy
            // 
            this.btnCopy.Image = global::Accounts.Properties.Resources.copy;
            this.btnCopy.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCopy.Location = new System.Drawing.Point(299, 4);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(58, 23);
            this.btnCopy.TabIndex = 23;
            this.btnCopy.Text = "Copy";
            this.btnCopy.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCopy.UseVisualStyleBackColor = true;
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
            // 
            // btnExit
            // 
            this.btnExit.Image = global::Accounts.Properties.Resources.ExitButton;
            this.btnExit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnExit.Location = new System.Drawing.Point(575, 4);
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
            this.btnLast.Location = new System.Drawing.Point(184, 4);
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
            this.btnFirst.Location = new System.Drawing.Point(4, 4);
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
            this.btnPrint.Location = new System.Drawing.Point(419, 4);
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
            this.btnNext.Location = new System.Drawing.Point(124, 4);
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
            this.btnPrev.Location = new System.Drawing.Point(64, 4);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(58, 23);
            this.btnPrev.TabIndex = 0;
            this.btnPrev.Text = "&Prev";
            this.btnPrev.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnPrev.UseVisualStyleBackColor = true;
            this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // CRbtnCancel
            // 
            this.CRbtnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CRbtnCancel.Image = global::Accounts.Properties.Resources.gnome_window_close;
            this.CRbtnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.CRbtnCancel.Location = new System.Drawing.Point(765, 13);
            this.CRbtnCancel.Name = "CRbtnCancel";
            this.CRbtnCancel.Size = new System.Drawing.Size(75, 29);
            this.CRbtnCancel.TabIndex = 1;
            this.CRbtnCancel.Text = "&Cancel";
            this.CRbtnCancel.UseVisualStyleBackColor = true;
            this.CRbtnCancel.Click += new System.EventHandler(this.CRbtnCancel_Click);
            // 
            // CRchkDoNotClose
            // 
            this.CRchkDoNotClose.AutoSize = true;
            this.CRchkDoNotClose.Location = new System.Drawing.Point(9, 19);
            this.CRchkDoNotClose.Name = "CRchkDoNotClose";
            this.CRchkDoNotClose.Size = new System.Drawing.Size(86, 17);
            this.CRchkDoNotClose.TabIndex = 0;
            this.CRchkDoNotClose.Text = "Do not close";
            this.CRchkDoNotClose.UseVisualStyleBackColor = true;
            this.CRchkDoNotClose.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CRchkDoNotClose_KeyDown);
            // 
            // CRbtnEdit
            // 
            this.CRbtnEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CRbtnEdit.Image = global::Accounts.Properties.Resources.document_edit;
            this.CRbtnEdit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.CRbtnEdit.Location = new System.Drawing.Point(522, 13);
            this.CRbtnEdit.Name = "CRbtnEdit";
            this.CRbtnEdit.Size = new System.Drawing.Size(75, 29);
            this.CRbtnEdit.TabIndex = 5;
            this.CRbtnEdit.Text = "&Edit";
            this.CRbtnEdit.UseVisualStyleBackColor = true;
            this.CRbtnEdit.Click += new System.EventHandler(this.CRbtnEdit_Click);
            // 
            // CRbtnDelete
            // 
            this.CRbtnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CRbtnDelete.Image = global::Accounts.Properties.Resources.document_delete;
            this.CRbtnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.CRbtnDelete.Location = new System.Drawing.Point(684, 13);
            this.CRbtnDelete.Name = "CRbtnDelete";
            this.CRbtnDelete.Size = new System.Drawing.Size(75, 29);
            this.CRbtnDelete.TabIndex = 0;
            this.CRbtnDelete.Text = "&Delete";
            this.CRbtnDelete.UseVisualStyleBackColor = true;
            this.CRbtnDelete.Click += new System.EventHandler(this.CRbtnDelete_Click);
            // 
            // CRbtnNew
            // 
            this.CRbtnNew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CRbtnNew.Image = global::Accounts.Properties.Resources.edit_add;
            this.CRbtnNew.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.CRbtnNew.Location = new System.Drawing.Point(441, 13);
            this.CRbtnNew.Name = "CRbtnNew";
            this.CRbtnNew.Size = new System.Drawing.Size(75, 29);
            this.CRbtnNew.TabIndex = 2;
            this.CRbtnNew.Text = "&New";
            this.CRbtnNew.UseVisualStyleBackColor = true;
            this.CRbtnNew.Click += new System.EventHandler(this.CRbtnNew_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(645, 20);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(33, 13);
            this.label3.TabIndex = 30;
            this.label3.Text = "Date:";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(184, 337);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 29;
            this.label2.Text = "Narration:";
            // 
            // lblVouNo
            // 
            this.lblVouNo.AutoSize = true;
            this.lblVouNo.Location = new System.Drawing.Point(350, 18);
            this.lblVouNo.Name = "lblVouNo";
            this.lblVouNo.Size = new System.Drawing.Size(60, 13);
            this.lblVouNo.TabIndex = 27;
            this.lblVouNo.Text = "Voucher #:";
            // 
            // CRbtnSave
            // 
            this.CRbtnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CRbtnSave.Image = global::Accounts.Properties.Resources.save;
            this.CRbtnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.CRbtnSave.Location = new System.Drawing.Point(603, 13);
            this.CRbtnSave.Name = "CRbtnSave";
            this.CRbtnSave.Size = new System.Drawing.Size(75, 29);
            this.CRbtnSave.TabIndex = 6;
            this.CRbtnSave.Text = "&Save";
            this.CRbtnSave.UseVisualStyleBackColor = true;
            this.CRbtnSave.Click += new System.EventHandler(this.CRbtnSave_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(19, 44);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(77, 13);
            this.label6.TabIndex = 41;
            this.label6.Text = "Cash Account:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(21, 18);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(42, 13);
            this.label8.TabIndex = 44;
            this.label8.Text = "Series :";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(19, 70);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(46, 13);
            this.label12.TabIndex = 76;
            this.label12.Text = "Project :";
            // 
            // CRcboProjectName
            // 
            this.CRcboProjectName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.CRcboProjectName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.CRcboProjectName.BackColor = System.Drawing.Color.White;
            this.CRcboProjectName.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.CRcboProjectName.FocusLostColor = System.Drawing.Color.White;
            this.CRcboProjectName.FormattingEnabled = true;
            this.CRcboProjectName.Location = new System.Drawing.Point(109, 67);
            this.CRcboProjectName.Name = "CRcboProjectName";
            this.CRcboProjectName.Size = new System.Drawing.Size(210, 21);
            this.CRcboProjectName.TabIndex = 5;
            this.CRcboProjectName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CRcboProjectName_KeyDown);
            // 
            // CRcboSeriesName
            // 
            this.CRcboSeriesName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.CRcboSeriesName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.CRcboSeriesName.BackColor = System.Drawing.Color.White;
            this.CRcboSeriesName.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.CRcboSeriesName.FocusLostColor = System.Drawing.Color.White;
            this.CRcboSeriesName.FormattingEnabled = true;
            this.CRcboSeriesName.Location = new System.Drawing.Point(109, 15);
            this.CRcboSeriesName.Name = "CRcboSeriesName";
            this.CRcboSeriesName.Size = new System.Drawing.Size(210, 21);
            this.CRcboSeriesName.TabIndex = 0;
            this.CRcboSeriesName.SelectedIndexChanged += new System.EventHandler(this.CRcboSeriesName_SelectedIndexChanged);
            this.CRcboSeriesName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CRcboSeriesName_KeyDown);
            // 
            // CRcmboCashAccount
            // 
            this.CRcmboCashAccount.BackColor = System.Drawing.Color.White;
            this.CRcmboCashAccount.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.CRcmboCashAccount.FocusLostColor = System.Drawing.Color.White;
            this.CRcmboCashAccount.FormattingEnabled = true;
            this.CRcmboCashAccount.Location = new System.Drawing.Point(109, 41);
            this.CRcmboCashAccount.Name = "CRcmboCashAccount";
            this.CRcmboCashAccount.Size = new System.Drawing.Size(210, 21);
            this.CRcmboCashAccount.TabIndex = 3;
            this.CRcmboCashAccount.SelectedIndexChanged += new System.EventHandler(this.CRcmboCashAccount_SelectedIndexChanged);
            this.CRcmboCashAccount.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CRcmboCashAccount_KeyDown);
            // 
            // CRtxtCashReceiptID
            // 
            this.CRtxtCashReceiptID.BackColor = System.Drawing.Color.White;
            this.CRtxtCashReceiptID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.CRtxtCashReceiptID.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.CRtxtCashReceiptID.FocusLostColor = System.Drawing.Color.White;
            this.CRtxtCashReceiptID.Location = new System.Drawing.Point(649, 68);
            this.CRtxtCashReceiptID.Name = "CRtxtCashReceiptID";
            this.CRtxtCashReceiptID.Size = new System.Drawing.Size(74, 20);
            this.CRtxtCashReceiptID.TabIndex = 4;
            this.CRtxtCashReceiptID.Visible = false;
            // 
            // CRtxtDate
            // 
            this.CRtxtDate.BackColor = System.Drawing.Color.White;
            this.CRtxtDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.CRtxtDate.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.CRtxtDate.FocusLostColor = System.Drawing.Color.White;
            this.CRtxtDate.Location = new System.Drawing.Point(689, 13);
            this.CRtxtDate.Mask = "0000/00/00";
            this.CRtxtDate.Name = "CRtxtDate";
            this.CRtxtDate.Size = new System.Drawing.Size(108, 20);
            this.CRtxtDate.TabIndex = 2;
            this.CRtxtDate.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CRtxtDate_KeyDown);
            // 
            // CRtxtRemarks
            // 
            this.CRtxtRemarks.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CRtxtRemarks.BackColor = System.Drawing.Color.White;
            this.CRtxtRemarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.CRtxtRemarks.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.CRtxtRemarks.FocusLostColor = System.Drawing.Color.White;
            this.CRtxtRemarks.Location = new System.Drawing.Point(255, 335);
            this.CRtxtRemarks.Name = "CRtxtRemarks";
            this.CRtxtRemarks.Size = new System.Drawing.Size(594, 20);
            this.CRtxtRemarks.TabIndex = 1;
            this.CRtxtRemarks.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CRtxtRemarks_KeyDown);
            // 
            // CRtxtVchNo
            // 
            this.CRtxtVchNo.BackColor = System.Drawing.Color.White;
            this.CRtxtVchNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.CRtxtVchNo.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.CRtxtVchNo.FocusLostColor = System.Drawing.Color.White;
            this.CRtxtVchNo.Location = new System.Drawing.Point(416, 16);
            this.CRtxtVchNo.Name = "CRtxtVchNo";
            this.CRtxtVchNo.Size = new System.Drawing.Size(198, 20);
            this.CRtxtVchNo.TabIndex = 1;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.CRbtnAddAccClass);
            this.tabPage2.Controls.Add(this.treeAccClass);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(842, 175);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Account Class";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // CRbtnAddAccClass
            // 
            this.CRbtnAddAccClass.Location = new System.Drawing.Point(441, 6);
            this.CRbtnAddAccClass.Name = "CRbtnAddAccClass";
            this.CRbtnAddAccClass.Size = new System.Drawing.Size(109, 27);
            this.CRbtnAddAccClass.TabIndex = 31;
            this.CRbtnAddAccClass.Text = "Add Account Class";
            this.CRbtnAddAccClass.UseVisualStyleBackColor = true;
            this.CRbtnAddAccClass.Click += new System.EventHandler(this.CRbtnAddAccClass_Click);
            // 
            // treeAccClass
            // 
            this.treeAccClass.AutoCheckChild = true;
            this.treeAccClass.CheckBoxes = true;
            this.treeAccClass.Dock = System.Windows.Forms.DockStyle.Left;
            this.treeAccClass.Location = new System.Drawing.Point(3, 3);
            this.treeAccClass.Name = "treeAccClass";
            this.treeAccClass.Size = new System.Drawing.Size(432, 169);
            this.treeAccClass.TabIndex = 29;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.grdCashReceipt);
            this.tabPage1.Controls.Add(this.lblDifferenceAmount);
            this.tabPage1.Controls.Add(this.label5);
            this.tabPage1.Controls.Add(this.label7);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.lblDebitTotal);
            this.tabPage1.Controls.Add(this.lblCreditTotal);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(842, 175);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Details";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // grdCashReceipt
            // 
            this.grdCashReceipt.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdCashReceipt.EnableSort = true;
            this.grdCashReceipt.Location = new System.Drawing.Point(6, 14);
            this.grdCashReceipt.Name = "grdCashReceipt";
            this.grdCashReceipt.OptimizeMode = SourceGrid.CellOptimizeMode.ForRows;
            this.grdCashReceipt.SelectionMode = SourceGrid.GridSelectionMode.Cell;
            this.grdCashReceipt.Size = new System.Drawing.Size(829, 144);
            this.grdCashReceipt.TabIndex = 0;
            this.grdCashReceipt.TabStop = true;
            this.grdCashReceipt.ToolTipText = "";
            // 
            // lblDifferenceAmount
            // 
            this.lblDifferenceAmount.Location = new System.Drawing.Point(641, 225);
            this.lblDifferenceAmount.Name = "lblDifferenceAmount";
            this.lblDifferenceAmount.Size = new System.Drawing.Size(103, 13);
            this.lblDifferenceAmount.TabIndex = 23;
            this.lblDifferenceAmount.Text = "0.00";
            this.lblDifferenceAmount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(357, 225);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(64, 13);
            this.label5.TabIndex = 1;
            this.label5.Text = "Credit Total:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(571, 225);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(59, 13);
            this.label7.TabIndex = 22;
            this.label7.Text = "Difference:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(176, 225);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(62, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Debit Total:";
            // 
            // lblDebitTotal
            // 
            this.lblDebitTotal.Location = new System.Drawing.Point(244, 225);
            this.lblDebitTotal.Name = "lblDebitTotal";
            this.lblDebitTotal.Size = new System.Drawing.Size(108, 13);
            this.lblDebitTotal.TabIndex = 2;
            this.lblDebitTotal.Text = "0.00";
            this.lblDebitTotal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblCreditTotal
            // 
            this.lblCreditTotal.Location = new System.Drawing.Point(427, 225);
            this.lblCreditTotal.Name = "lblCreditTotal";
            this.lblCreditTotal.Size = new System.Drawing.Size(138, 13);
            this.lblCreditTotal.TabIndex = 12;
            this.lblCreditTotal.Text = "0.00";
            this.lblCreditTotal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(4, 128);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(850, 201);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.CRtxtfifth);
            this.tabPage3.Controls.Add(this.CRtxtfourth);
            this.tabPage3.Controls.Add(this.CRtxtthird);
            this.tabPage3.Controls.Add(this.CRtxtsecond);
            this.tabPage3.Controls.Add(this.CRtxtfirst);
            this.tabPage3.Controls.Add(this.CRlblfourth);
            this.tabPage3.Controls.Add(this.CRlblsecond);
            this.tabPage3.Controls.Add(this.CRlblthird);
            this.tabPage3.Controls.Add(this.CRlblfifth);
            this.tabPage3.Controls.Add(this.CRlblfirst);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(842, 175);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Additional Fields";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // CRtxtfifth
            // 
            this.CRtxtfifth.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.CRtxtfifth.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.CRtxtfifth.FocusLostColor = System.Drawing.Color.White;
            this.CRtxtfifth.Location = new System.Drawing.Point(109, 140);
            this.CRtxtfifth.Name = "CRtxtfifth";
            this.CRtxtfifth.Size = new System.Drawing.Size(182, 20);
            this.CRtxtfifth.TabIndex = 40;
            this.CRtxtfifth.Visible = false;
            // 
            // CRtxtfourth
            // 
            this.CRtxtfourth.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.CRtxtfourth.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.CRtxtfourth.FocusLostColor = System.Drawing.Color.White;
            this.CRtxtfourth.Location = new System.Drawing.Point(109, 112);
            this.CRtxtfourth.Name = "CRtxtfourth";
            this.CRtxtfourth.Size = new System.Drawing.Size(182, 20);
            this.CRtxtfourth.TabIndex = 39;
            this.CRtxtfourth.Visible = false;
            // 
            // CRtxtthird
            // 
            this.CRtxtthird.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.CRtxtthird.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.CRtxtthird.FocusLostColor = System.Drawing.Color.White;
            this.CRtxtthird.Location = new System.Drawing.Point(109, 79);
            this.CRtxtthird.Name = "CRtxtthird";
            this.CRtxtthird.Size = new System.Drawing.Size(182, 20);
            this.CRtxtthird.TabIndex = 38;
            this.CRtxtthird.Visible = false;
            // 
            // CRtxtsecond
            // 
            this.CRtxtsecond.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.CRtxtsecond.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.CRtxtsecond.FocusLostColor = System.Drawing.Color.White;
            this.CRtxtsecond.Location = new System.Drawing.Point(109, 43);
            this.CRtxtsecond.Name = "CRtxtsecond";
            this.CRtxtsecond.Size = new System.Drawing.Size(182, 20);
            this.CRtxtsecond.TabIndex = 37;
            this.CRtxtsecond.Visible = false;
            // 
            // CRtxtfirst
            // 
            this.CRtxtfirst.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.CRtxtfirst.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.CRtxtfirst.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.CRtxtfirst.FocusLostColor = System.Drawing.Color.White;
            this.CRtxtfirst.Location = new System.Drawing.Point(109, 13);
            this.CRtxtfirst.Name = "CRtxtfirst";
            this.CRtxtfirst.Size = new System.Drawing.Size(182, 20);
            this.CRtxtfirst.TabIndex = 36;
            this.CRtxtfirst.Visible = false;
            // 
            // CRlblfourth
            // 
            this.CRlblfourth.AutoSize = true;
            this.CRlblfourth.Location = new System.Drawing.Point(9, 118);
            this.CRlblfourth.Name = "CRlblfourth";
            this.CRlblfourth.Size = new System.Drawing.Size(80, 13);
            this.CRlblfourth.TabIndex = 35;
            this.CRlblfourth.Text = "Optional Field 4";
            this.CRlblfourth.Visible = false;
            // 
            // CRlblsecond
            // 
            this.CRlblsecond.AutoSize = true;
            this.CRlblsecond.Location = new System.Drawing.Point(9, 49);
            this.CRlblsecond.Name = "CRlblsecond";
            this.CRlblsecond.Size = new System.Drawing.Size(80, 13);
            this.CRlblsecond.TabIndex = 34;
            this.CRlblsecond.Text = "Optional Field 2";
            this.CRlblsecond.Visible = false;
            // 
            // CRlblthird
            // 
            this.CRlblthird.AutoSize = true;
            this.CRlblthird.Location = new System.Drawing.Point(9, 84);
            this.CRlblthird.Name = "CRlblthird";
            this.CRlblthird.Size = new System.Drawing.Size(80, 13);
            this.CRlblthird.TabIndex = 33;
            this.CRlblthird.Text = "Optional Field 3";
            this.CRlblthird.Visible = false;
            // 
            // CRlblfifth
            // 
            this.CRlblfifth.AutoSize = true;
            this.CRlblfifth.Location = new System.Drawing.Point(9, 146);
            this.CRlblfifth.Name = "CRlblfifth";
            this.CRlblfifth.Size = new System.Drawing.Size(80, 13);
            this.CRlblfifth.TabIndex = 32;
            this.CRlblfifth.Text = "Optional Field 5";
            this.CRlblfifth.Visible = false;
            // 
            // CRlblfirst
            // 
            this.CRlblfirst.AutoSize = true;
            this.CRlblfirst.Location = new System.Drawing.Point(9, 14);
            this.CRlblfirst.Name = "CRlblfirst";
            this.CRlblfirst.Size = new System.Drawing.Size(80, 13);
            this.CRlblfirst.TabIndex = 31;
            this.CRlblfirst.Text = "Optional Field 1";
            this.CRlblfirst.Visible = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.CRlblCurrentBalance);
            this.groupBox2.Controls.Add(this.CRbtn_groupvoucherposting);
            this.groupBox2.Controls.Add(this.CRbtnDate);
            this.groupBox2.Controls.Add(this.CRcboSeriesName);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.CRtxtVchNo);
            this.groupBox2.Controls.Add(this.CRcboProjectName);
            this.groupBox2.Controls.Add(this.lblVouNo);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.CRtxtDate);
            this.groupBox2.Controls.Add(this.CRcmboCashAccount);
            this.groupBox2.Controls.Add(this.CRtxtCashReceiptID);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Location = new System.Drawing.Point(3, 30);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(847, 94);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(343, 40);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(86, 13);
            this.label10.TabIndex = 157;
            this.label10.Text = "Current Balance:";
            // 
            // CRlblCurrentBalance
            // 
            this.CRlblCurrentBalance.AutoSize = true;
            this.CRlblCurrentBalance.Location = new System.Drawing.Point(436, 40);
            this.CRlblCurrentBalance.Name = "CRlblCurrentBalance";
            this.CRlblCurrentBalance.Size = new System.Drawing.Size(28, 13);
            this.CRlblCurrentBalance.TabIndex = 156;
            this.CRlblCurrentBalance.Text = "0.00";
            // 
            // CRbtn_groupvoucherposting
            // 
            this.CRbtn_groupvoucherposting.Location = new System.Drawing.Point(689, 39);
            this.CRbtn_groupvoucherposting.Name = "CRbtn_groupvoucherposting";
            this.CRbtn_groupvoucherposting.Size = new System.Drawing.Size(138, 23);
            this.CRbtn_groupvoucherposting.TabIndex = 155;
            this.CRbtn_groupvoucherposting.Text = "Group  Voucher Posting";
            this.CRbtn_groupvoucherposting.UseVisualStyleBackColor = true;
            this.CRbtn_groupvoucherposting.Click += new System.EventHandler(this.CRbtn_groupvoucherposting_Click);
            // 
            // CRbtnDate
            // 
            this.CRbtnDate.Image = global::Accounts.Properties.Resources.dateIcon;
            this.CRbtnDate.Location = new System.Drawing.Point(803, 10);
            this.CRbtnDate.Name = "CRbtnDate";
            this.CRbtnDate.Size = new System.Drawing.Size(26, 23);
            this.CRbtnDate.TabIndex = 3;
            this.CRbtnDate.UseVisualStyleBackColor = true;
            this.CRbtnDate.Click += new System.EventHandler(this.CRbtnDate_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusJournal});
            this.statusStrip1.Location = new System.Drawing.Point(0, 408);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(866, 22);
            this.statusStrip1.TabIndex = 149;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // statusJournal
            // 
            this.statusJournal.Name = "statusJournal";
            this.statusJournal.Size = new System.Drawing.Size(313, 17);
            this.statusJournal.Text = "Press CTRL+R in the Narration Field for Previous Narration";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.CRbtnSave);
            this.groupBox1.Controls.Add(this.CRbtnNew);
            this.groupBox1.Controls.Add(this.CRbtnDelete);
            this.groupBox1.Controls.Add(this.CRbtnEdit);
            this.groupBox1.Controls.Add(this.CRchkDoNotClose);
            this.groupBox1.Controls.Add(this.CRchkPrntWhileSaving);
            this.groupBox1.Controls.Add(this.CRbtnCancel);
            this.groupBox1.Location = new System.Drawing.Point(3, 355);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(846, 48);
            this.groupBox1.TabIndex = 148;
            this.groupBox1.TabStop = false;
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // lblCurrBal
            // 
            this.lblCurrBal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblCurrBal.AutoSize = true;
            this.lblCurrBal.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrBal.Location = new System.Drawing.Point(78, 342);
            this.lblCurrBal.Name = "lblCurrBal";
            this.lblCurrBal.Size = new System.Drawing.Size(63, 13);
            this.lblCurrBal.TabIndex = 150;
            this.lblCurrBal.Text = "Narration:";
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(5, 328);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(86, 13);
            this.label9.TabIndex = 151;
            this.label9.Text = "Current Balance:";
            // 
            // frmCashReceipt
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(866, 430);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.lblCurrBal);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.CRtxtRemarks);
            this.Controls.Add(this.label2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "frmCashReceipt";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Cash Receipt                  ";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmCashReceipt_FormClosing);
            this.Load += new System.EventHandler(this.frmCashReceipt_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmCashReceipt_KeyDown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private STextBox CRtxtCashReceiptID;
        private System.Windows.Forms.CheckBox CRchkPrntWhileSaving;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnPaste;
        private System.Windows.Forms.Button btnCopy;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnLast;
        private System.Windows.Forms.Button btnFirst;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnPrev;
        private System.Windows.Forms.Button CRbtnCancel;
        private System.Windows.Forms.CheckBox CRchkDoNotClose;
        private System.Windows.Forms.Button CRbtnEdit;
        private System.Windows.Forms.Button CRbtnDelete;
        private System.Windows.Forms.Button CRbtnNew;
        private SMaskedTextBox CRtxtDate;
        private System.Windows.Forms.Label label3;
        private STextBox CRtxtRemarks;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblVouNo;
        private System.Windows.Forms.Button CRbtnSave;
        private STextBox CRtxtVchNo;
        private System.Windows.Forms.Label label6;
        private SComboBox CRcmboCashAccount;
        private SComboBox CRcboSeriesName;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label12;
        private SComboBox CRcboProjectName;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button CRbtnAddAccClass;
        private STreeView treeAccClass;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Label lblDifferenceAmount;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblDebitTotal;
        private System.Windows.Forms.Label lblCreditTotal;
        private System.Windows.Forms.TabControl tabControl1;
        private SourceGrid.Grid grdCashReceipt;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button CRbtnDate;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel statusJournal;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblCurrBal;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button CRbtn_groupvoucherposting;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label CRlblCurrentBalance;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.TabPage tabPage3;
        private STextBox CRtxtfifth;
        private STextBox CRtxtfourth;
        private STextBox CRtxtthird;
        private STextBox CRtxtsecond;
        private STextBox CRtxtfirst;
        private System.Windows.Forms.Label CRlblfourth;
        private System.Windows.Forms.Label CRlblsecond;
        private System.Windows.Forms.Label CRlblthird;
        private System.Windows.Forms.Label CRlblfifth;
        private System.Windows.Forms.Label CRlblfirst;
        private System.Windows.Forms.Button btnPrintPreview;
        private System.Windows.Forms.Button btnSetup;
        private System.Windows.Forms.CheckBox CRchkRecurring;
        private System.Windows.Forms.Button btnList;

    }
}