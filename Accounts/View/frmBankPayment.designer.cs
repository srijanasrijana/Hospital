using SComponents;
namespace Accounts
{
    partial class frmBankPayment
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmBankPayment));
            this.BPtxtRemarks = new SComponents.STextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnList = new System.Windows.Forms.Button();
            this.btnSetup = new System.Windows.Forms.Button();
            this.btnPrintPreview = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.chkRecurring = new System.Windows.Forms.CheckBox();
            this.btnPaste = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnLast = new System.Windows.Forms.Button();
            this.btnCopy = new System.Windows.Forms.Button();
            this.btnFirst = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnPrev = new System.Windows.Forms.Button();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.grdBankPayment = new SourceGrid.Grid();
            this.BPlblTotalAmount = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.BPbtnAddAccClass = new System.Windows.Forms.Button();
            this.treeAccClass = new SComponents.STreeView(this.components);
            this.label2 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.BPtxtfifth = new SComponents.STextBox();
            this.BPtxtfourth = new SComponents.STextBox();
            this.BPtxtthird = new SComponents.STextBox();
            this.BPtxtsecond = new SComponents.STextBox();
            this.BPtxtfirst = new SComponents.STextBox();
            this.BPlblfourth = new System.Windows.Forms.Label();
            this.BPlblsecond = new System.Windows.Forms.Label();
            this.BPlblthird = new System.Windows.Forms.Label();
            this.BPlblfifth = new System.Windows.Forms.Label();
            this.BPlblfirst = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.BPbtn_groupvoucherposting = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.BPlblBankCurrBalHidden = new System.Windows.Forms.Label();
            this.BPlblBankCurrentBalance = new System.Windows.Forms.Label();
            this.BPbtnDate = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.BPcboProjectName = new SComponents.SComboBox();
            this.BPcboSeriesName = new SComponents.SComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.BPlblVouNo = new System.Windows.Forms.Label();
            this.BPtxtDate = new SComponents.SMaskedTextBox();
            this.BPcmboBankAccount = new SComponents.SComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.BPtxtBankID = new SComponents.STextBox();
            this.BPtxtVchNo = new SComponents.STextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.BPbtnSave = new System.Windows.Forms.Button();
            this.BPbtnNew = new System.Windows.Forms.Button();
            this.BPbtnCancel = new System.Windows.Forms.Button();
            this.BPbtnEdit = new System.Windows.Forms.Button();
            this.BPchkDoNotClose = new System.Windows.Forms.CheckBox();
            this.BPbtnDelete = new System.Windows.Forms.Button();
            this.BPchkPrntWhileSaving = new System.Windows.Forms.CheckBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.statusJournal = new System.Windows.Forms.ToolStripStatusLabel();
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
            // BPtxtRemarks
            // 
            this.BPtxtRemarks.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BPtxtRemarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.BPtxtRemarks.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.BPtxtRemarks.FocusLostColor = System.Drawing.Color.White;
            this.BPtxtRemarks.Location = new System.Drawing.Point(72, 338);
            this.BPtxtRemarks.Name = "BPtxtRemarks";
            this.BPtxtRemarks.Size = new System.Drawing.Size(732, 20);
            this.BPtxtRemarks.TabIndex = 118;
            this.BPtxtRemarks.KeyDown += new System.Windows.Forms.KeyEventHandler(this.bPtxtRemarks_KeyDown);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.CadetBlue;
            this.panel1.Controls.Add(this.btnList);
            this.panel1.Controls.Add(this.btnSetup);
            this.panel1.Controls.Add(this.btnPrintPreview);
            this.panel1.Controls.Add(this.btnPrint);
            this.panel1.Controls.Add(this.chkRecurring);
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
            this.panel1.Size = new System.Drawing.Size(820, 30);
            this.panel1.TabIndex = 127;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // btnList
            // 
            this.btnList.Location = new System.Drawing.Point(238, 4);
            this.btnList.Name = "btnList";
            this.btnList.Size = new System.Drawing.Size(53, 23);
            this.btnList.TabIndex = 152;
            this.btnList.Text = "≡ List";
            this.btnList.UseVisualStyleBackColor = true;
            this.btnList.Click += new System.EventHandler(this.btnList_Click);
            // 
            // btnSetup
            // 
            this.btnSetup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSetup.Location = new System.Drawing.Point(710, 4);
            this.btnSetup.Name = "btnSetup";
            this.btnSetup.Size = new System.Drawing.Size(42, 23);
            this.btnSetup.TabIndex = 140;
            this.btnSetup.Text = "setup";
            this.btnSetup.UseVisualStyleBackColor = true;
            this.btnSetup.Click += new System.EventHandler(this.btnSetup_Click);
            // 
            // btnPrintPreview
            // 
            this.btnPrintPreview.Image = global::Accounts.Properties.Resources.print_preview;
            this.btnPrintPreview.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPrintPreview.Location = new System.Drawing.Point(469, 4);
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
            this.btnPrint.Location = new System.Drawing.Point(410, 4);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(58, 23);
            this.btnPrint.TabIndex = 2;
            this.btnPrint.Text = "Print";
            this.btnPrint.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // chkRecurring
            // 
            this.chkRecurring.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkRecurring.AutoSize = true;
            this.chkRecurring.ForeColor = System.Drawing.Color.White;
            this.chkRecurring.Location = new System.Drawing.Point(634, 8);
            this.chkRecurring.Margin = new System.Windows.Forms.Padding(2);
            this.chkRecurring.Name = "chkRecurring";
            this.chkRecurring.Size = new System.Drawing.Size(72, 17);
            this.chkRecurring.TabIndex = 139;
            this.chkRecurring.Text = "Recurrin&g";
            this.chkRecurring.UseVisualStyleBackColor = true;
            this.chkRecurring.CheckedChanged += new System.EventHandler(this.chkRecurring_CheckedChanged);
            // 
            // btnPaste
            // 
            this.btnPaste.Image = global::Accounts.Properties.Resources.paste;
            this.btnPaste.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPaste.Location = new System.Drawing.Point(351, 4);
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
            this.btnExport.Location = new System.Drawing.Point(755, 3);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(61, 23);
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
            this.btnExit.Location = new System.Drawing.Point(564, 4);
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
            this.btnLast.Location = new System.Drawing.Point(179, 4);
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
            this.btnCopy.Location = new System.Drawing.Point(292, 4);
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
            this.btnFirst.Location = new System.Drawing.Point(2, 4);
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
            this.btnNext.Location = new System.Drawing.Point(120, 4);
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
            this.btnPrev.Location = new System.Drawing.Point(61, 4);
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
            this.tabPage1.Controls.Add(this.grdBankPayment);
            this.tabPage1.Controls.Add(this.BPlblTotalAmount);
            this.tabPage1.Controls.Add(this.label7);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(793, 172);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Details";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // grdBankPayment
            // 
            this.grdBankPayment.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdBankPayment.EnableSort = true;
            this.grdBankPayment.Location = new System.Drawing.Point(3, 3);
            this.grdBankPayment.Name = "grdBankPayment";
            this.grdBankPayment.OptimizeMode = SourceGrid.CellOptimizeMode.ForRows;
            this.grdBankPayment.SelectionMode = SourceGrid.GridSelectionMode.Cell;
            this.grdBankPayment.Size = new System.Drawing.Size(784, 150);
            this.grdBankPayment.TabIndex = 1;
            this.grdBankPayment.TabStop = true;
            this.grdBankPayment.ToolTipText = "";
            // 
            // BPlblTotalAmount
            // 
            this.BPlblTotalAmount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BPlblTotalAmount.Location = new System.Drawing.Point(531, 156);
            this.BPlblTotalAmount.Name = "BPlblTotalAmount";
            this.BPlblTotalAmount.Size = new System.Drawing.Size(155, 13);
            this.BPlblTotalAmount.TabIndex = 23;
            this.BPlblTotalAmount.Text = "0.00";
            this.BPlblTotalAmount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(488, 156);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(37, 13);
            this.label7.TabIndex = 22;
            this.label7.Text = "Total: ";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.BPbtnAddAccClass);
            this.tabPage2.Controls.Add(this.treeAccClass);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(793, 172);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = " Account Class";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // BPbtnAddAccClass
            // 
            this.BPbtnAddAccClass.Location = new System.Drawing.Point(422, 6);
            this.BPbtnAddAccClass.Name = "BPbtnAddAccClass";
            this.BPbtnAddAccClass.Size = new System.Drawing.Size(109, 23);
            this.BPbtnAddAccClass.TabIndex = 30;
            this.BPbtnAddAccClass.Text = "Add Account Class";
            this.BPbtnAddAccClass.UseVisualStyleBackColor = true;
            this.BPbtnAddAccClass.Click += new System.EventHandler(this.BPbtnAddAccClass_Click);
            // 
            // treeAccClass
            // 
            this.treeAccClass.AutoCheckChild = true;
            this.treeAccClass.CheckBoxes = true;
            this.treeAccClass.Location = new System.Drawing.Point(0, 6);
            this.treeAccClass.Name = "treeAccClass";
            this.treeAccClass.Size = new System.Drawing.Size(416, 241);
            this.treeAccClass.TabIndex = 29;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 340);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 119;
            this.label2.Text = "Narration:";
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(7, 135);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(801, 198);
            this.tabControl1.TabIndex = 115;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.BPtxtfifth);
            this.tabPage3.Controls.Add(this.BPtxtfourth);
            this.tabPage3.Controls.Add(this.BPtxtthird);
            this.tabPage3.Controls.Add(this.BPtxtsecond);
            this.tabPage3.Controls.Add(this.BPtxtfirst);
            this.tabPage3.Controls.Add(this.BPlblfourth);
            this.tabPage3.Controls.Add(this.BPlblsecond);
            this.tabPage3.Controls.Add(this.BPlblthird);
            this.tabPage3.Controls.Add(this.BPlblfifth);
            this.tabPage3.Controls.Add(this.BPlblfirst);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(793, 172);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Additional Fields";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // BPtxtfifth
            // 
            this.BPtxtfifth.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.BPtxtfifth.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.BPtxtfifth.FocusLostColor = System.Drawing.Color.White;
            this.BPtxtfifth.Location = new System.Drawing.Point(107, 136);
            this.BPtxtfifth.Name = "BPtxtfifth";
            this.BPtxtfifth.Size = new System.Drawing.Size(182, 20);
            this.BPtxtfifth.TabIndex = 40;
            this.BPtxtfifth.Visible = false;
            // 
            // BPtxtfourth
            // 
            this.BPtxtfourth.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.BPtxtfourth.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.BPtxtfourth.FocusLostColor = System.Drawing.Color.White;
            this.BPtxtfourth.Location = new System.Drawing.Point(107, 102);
            this.BPtxtfourth.Name = "BPtxtfourth";
            this.BPtxtfourth.Size = new System.Drawing.Size(182, 20);
            this.BPtxtfourth.TabIndex = 39;
            this.BPtxtfourth.Visible = false;
            // 
            // BPtxtthird
            // 
            this.BPtxtthird.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.BPtxtthird.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.BPtxtthird.FocusLostColor = System.Drawing.Color.White;
            this.BPtxtthird.Location = new System.Drawing.Point(107, 68);
            this.BPtxtthird.Name = "BPtxtthird";
            this.BPtxtthird.Size = new System.Drawing.Size(182, 20);
            this.BPtxtthird.TabIndex = 38;
            this.BPtxtthird.Visible = false;
            // 
            // BPtxtsecond
            // 
            this.BPtxtsecond.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.BPtxtsecond.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.BPtxtsecond.FocusLostColor = System.Drawing.Color.White;
            this.BPtxtsecond.Location = new System.Drawing.Point(107, 35);
            this.BPtxtsecond.Name = "BPtxtsecond";
            this.BPtxtsecond.Size = new System.Drawing.Size(182, 20);
            this.BPtxtsecond.TabIndex = 37;
            this.BPtxtsecond.Visible = false;
            // 
            // BPtxtfirst
            // 
            this.BPtxtfirst.BackColor = System.Drawing.Color.White;
            this.BPtxtfirst.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.BPtxtfirst.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.BPtxtfirst.FocusLostColor = System.Drawing.Color.White;
            this.BPtxtfirst.Location = new System.Drawing.Point(107, 8);
            this.BPtxtfirst.Name = "BPtxtfirst";
            this.BPtxtfirst.Size = new System.Drawing.Size(182, 20);
            this.BPtxtfirst.TabIndex = 36;
            this.BPtxtfirst.Visible = false;
            // 
            // BPlblfourth
            // 
            this.BPlblfourth.AutoSize = true;
            this.BPlblfourth.Location = new System.Drawing.Point(7, 108);
            this.BPlblfourth.Name = "BPlblfourth";
            this.BPlblfourth.Size = new System.Drawing.Size(80, 13);
            this.BPlblfourth.TabIndex = 35;
            this.BPlblfourth.Text = "Optional Field 4";
            this.BPlblfourth.Visible = false;
            // 
            // BPlblsecond
            // 
            this.BPlblsecond.AutoSize = true;
            this.BPlblsecond.Location = new System.Drawing.Point(7, 41);
            this.BPlblsecond.Name = "BPlblsecond";
            this.BPlblsecond.Size = new System.Drawing.Size(80, 13);
            this.BPlblsecond.TabIndex = 34;
            this.BPlblsecond.Text = "Optional Field 2";
            this.BPlblsecond.Visible = false;
            // 
            // BPlblthird
            // 
            this.BPlblthird.AutoSize = true;
            this.BPlblthird.Location = new System.Drawing.Point(7, 73);
            this.BPlblthird.Name = "BPlblthird";
            this.BPlblthird.Size = new System.Drawing.Size(80, 13);
            this.BPlblthird.TabIndex = 33;
            this.BPlblthird.Text = "Optional Field 3";
            this.BPlblthird.Visible = false;
            // 
            // BPlblfifth
            // 
            this.BPlblfifth.AutoSize = true;
            this.BPlblfifth.Location = new System.Drawing.Point(7, 142);
            this.BPlblfifth.Name = "BPlblfifth";
            this.BPlblfifth.Size = new System.Drawing.Size(80, 13);
            this.BPlblfifth.TabIndex = 32;
            this.BPlblfifth.Text = "Optional Field 5";
            this.BPlblfifth.Visible = false;
            // 
            // BPlblfirst
            // 
            this.BPlblfirst.AutoSize = true;
            this.BPlblfirst.Location = new System.Drawing.Point(7, 9);
            this.BPlblfirst.Name = "BPlblfirst";
            this.BPlblfirst.Size = new System.Drawing.Size(80, 13);
            this.BPlblfirst.TabIndex = 31;
            this.BPlblfirst.Text = "Optional Field 1";
            this.BPlblfirst.Visible = false;
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.BPbtn_groupvoucherposting);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.BPlblBankCurrBalHidden);
            this.groupBox3.Controls.Add(this.BPlblBankCurrentBalance);
            this.groupBox3.Controls.Add(this.BPbtnDate);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.BPcboProjectName);
            this.groupBox3.Controls.Add(this.BPcboSeriesName);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.BPlblVouNo);
            this.groupBox3.Controls.Add(this.BPtxtDate);
            this.groupBox3.Controls.Add(this.BPcmboBankAccount);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.BPtxtBankID);
            this.groupBox3.Controls.Add(this.BPtxtVchNo);
            this.groupBox3.Location = new System.Drawing.Point(3, 30);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(805, 95);
            this.groupBox3.TabIndex = 136;
            this.groupBox3.TabStop = false;
            this.groupBox3.Enter += new System.EventHandler(this.groupBox3_Enter);
            // 
            // BPbtn_groupvoucherposting
            // 
            this.BPbtn_groupvoucherposting.Location = new System.Drawing.Point(637, 42);
            this.BPbtn_groupvoucherposting.Name = "BPbtn_groupvoucherposting";
            this.BPbtn_groupvoucherposting.Size = new System.Drawing.Size(138, 23);
            this.BPbtn_groupvoucherposting.TabIndex = 151;
            this.BPbtn_groupvoucherposting.Text = "Group  Voucher Posting";
            this.BPbtn_groupvoucherposting.UseVisualStyleBackColor = true;
            this.BPbtn_groupvoucherposting.Click += new System.EventHandler(this.BPbtn_groupvoucherposting_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(286, 41);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(86, 13);
            this.label4.TabIndex = 150;
            this.label4.Text = "Current Balance:";
            // 
            // BPlblBankCurrBalHidden
            // 
            this.BPlblBankCurrBalHidden.AutoSize = true;
            this.BPlblBankCurrBalHidden.Location = new System.Drawing.Point(298, 67);
            this.BPlblBankCurrBalHidden.Name = "BPlblBankCurrBalHidden";
            this.BPlblBankCurrBalHidden.Size = new System.Drawing.Size(28, 13);
            this.BPlblBankCurrBalHidden.TabIndex = 149;
            this.BPlblBankCurrBalHidden.Text = "0.00";
            this.BPlblBankCurrBalHidden.Visible = false;
            // 
            // BPlblBankCurrentBalance
            // 
            this.BPlblBankCurrentBalance.AutoSize = true;
            this.BPlblBankCurrentBalance.Location = new System.Drawing.Point(378, 42);
            this.BPlblBankCurrentBalance.Name = "BPlblBankCurrentBalance";
            this.BPlblBankCurrentBalance.Size = new System.Drawing.Size(28, 13);
            this.BPlblBankCurrentBalance.TabIndex = 148;
            this.BPlblBankCurrentBalance.Text = "0.00";
            // 
            // BPbtnDate
            // 
            this.BPbtnDate.Image = global::Accounts.Properties.Resources.dateIcon;
            this.BPbtnDate.Location = new System.Drawing.Point(749, 11);
            this.BPbtnDate.Name = "BPbtnDate";
            this.BPbtnDate.Size = new System.Drawing.Size(26, 23);
            this.BPbtnDate.TabIndex = 147;
            this.BPbtnDate.UseVisualStyleBackColor = true;
            this.BPbtnDate.Click += new System.EventHandler(this.BPbtnDate_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(10, 67);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(46, 13);
            this.label9.TabIndex = 146;
            this.label9.Text = "Project :";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 15);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(42, 13);
            this.label8.TabIndex = 145;
            this.label8.Text = "Series :";
            // 
            // BPcboProjectName
            // 
            this.BPcboProjectName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.BPcboProjectName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.BPcboProjectName.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.BPcboProjectName.FocusLostColor = System.Drawing.Color.White;
            this.BPcboProjectName.FormattingEnabled = true;
            this.BPcboProjectName.Location = new System.Drawing.Point(95, 64);
            this.BPcboProjectName.Name = "BPcboProjectName";
            this.BPcboProjectName.Size = new System.Drawing.Size(178, 21);
            this.BPcboProjectName.TabIndex = 144;
            this.BPcboProjectName.SelectedIndexChanged += new System.EventHandler(this.cboProjectName_SelectedIndexChanged);
            // 
            // BPcboSeriesName
            // 
            this.BPcboSeriesName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.BPcboSeriesName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.BPcboSeriesName.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.BPcboSeriesName.FocusLostColor = System.Drawing.Color.White;
            this.BPcboSeriesName.FormattingEnabled = true;
            this.BPcboSeriesName.Location = new System.Drawing.Point(95, 13);
            this.BPcboSeriesName.Name = "BPcboSeriesName";
            this.BPcboSeriesName.Size = new System.Drawing.Size(178, 21);
            this.BPcboSeriesName.TabIndex = 143;
            this.BPcboSeriesName.SelectedIndexChanged += new System.EventHandler(this.BPcboSeriesName_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(581, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(33, 13);
            this.label3.TabIndex = 138;
            this.label3.Text = "Date:";
            // 
            // BPlblVouNo
            // 
            this.BPlblVouNo.AutoSize = true;
            this.BPlblVouNo.Location = new System.Drawing.Point(298, 17);
            this.BPlblVouNo.Name = "BPlblVouNo";
            this.BPlblVouNo.Size = new System.Drawing.Size(60, 13);
            this.BPlblVouNo.TabIndex = 137;
            this.BPlblVouNo.Text = "Voucher #:";
            // 
            // BPtxtDate
            // 
            this.BPtxtDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.BPtxtDate.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.BPtxtDate.FocusLostColor = System.Drawing.Color.White;
            this.BPtxtDate.Location = new System.Drawing.Point(620, 13);
            this.BPtxtDate.Mask = "0000/00/00";
            this.BPtxtDate.Name = "BPtxtDate";
            this.BPtxtDate.Size = new System.Drawing.Size(123, 20);
            this.BPtxtDate.TabIndex = 139;
            // 
            // BPcmboBankAccount
            // 
            this.BPcmboBankAccount.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.BPcmboBankAccount.FocusLostColor = System.Drawing.Color.White;
            this.BPcmboBankAccount.FormattingEnabled = true;
            this.BPcmboBankAccount.Location = new System.Drawing.Point(95, 39);
            this.BPcmboBankAccount.Name = "BPcmboBankAccount";
            this.BPcmboBankAccount.Size = new System.Drawing.Size(178, 21);
            this.BPcmboBankAccount.TabIndex = 142;
            this.BPcmboBankAccount.SelectedIndexChanged += new System.EventHandler(this.cmboBankAccount_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(11, 39);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(78, 13);
            this.label6.TabIndex = 141;
            this.label6.Text = "Bank Account:";
            // 
            // BPtxtBankID
            // 
            this.BPtxtBankID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.BPtxtBankID.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.BPtxtBankID.FocusLostColor = System.Drawing.Color.White;
            this.BPtxtBankID.Location = new System.Drawing.Point(450, 39);
            this.BPtxtBankID.Name = "BPtxtBankID";
            this.BPtxtBankID.Size = new System.Drawing.Size(88, 20);
            this.BPtxtBankID.TabIndex = 140;
            this.BPtxtBankID.Visible = false;
            // 
            // BPtxtVchNo
            // 
            this.BPtxtVchNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.BPtxtVchNo.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.BPtxtVchNo.FocusLostColor = System.Drawing.Color.White;
            this.BPtxtVchNo.Location = new System.Drawing.Point(361, 13);
            this.BPtxtVchNo.Name = "BPtxtVchNo";
            this.BPtxtVchNo.Size = new System.Drawing.Size(177, 20);
            this.BPtxtVchNo.TabIndex = 136;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.BPbtnSave);
            this.groupBox1.Controls.Add(this.BPbtnNew);
            this.groupBox1.Controls.Add(this.BPbtnCancel);
            this.groupBox1.Controls.Add(this.BPbtnEdit);
            this.groupBox1.Controls.Add(this.BPchkDoNotClose);
            this.groupBox1.Controls.Add(this.BPbtnDelete);
            this.groupBox1.Controls.Add(this.BPchkPrntWhileSaving);
            this.groupBox1.Location = new System.Drawing.Point(10, 358);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(794, 48);
            this.groupBox1.TabIndex = 137;
            this.groupBox1.TabStop = false;
            // 
            // BPbtnSave
            // 
            this.BPbtnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BPbtnSave.Image = global::Accounts.Properties.Resources.save;
            this.BPbtnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BPbtnSave.Location = new System.Drawing.Point(549, 14);
            this.BPbtnSave.Name = "BPbtnSave";
            this.BPbtnSave.Size = new System.Drawing.Size(75, 29);
            this.BPbtnSave.TabIndex = 131;
            this.BPbtnSave.Text = "&Save";
            this.BPbtnSave.UseVisualStyleBackColor = true;
            this.BPbtnSave.Click += new System.EventHandler(this.BPbtnSave_Click);
            // 
            // BPbtnNew
            // 
            this.BPbtnNew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BPbtnNew.Image = global::Accounts.Properties.Resources.edit_add;
            this.BPbtnNew.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BPbtnNew.Location = new System.Drawing.Point(387, 12);
            this.BPbtnNew.Name = "BPbtnNew";
            this.BPbtnNew.Size = new System.Drawing.Size(75, 30);
            this.BPbtnNew.TabIndex = 130;
            this.BPbtnNew.Text = "&New";
            this.BPbtnNew.UseVisualStyleBackColor = true;
            this.BPbtnNew.Click += new System.EventHandler(this.BPbtnNew_Click);
            // 
            // BPbtnCancel
            // 
            this.BPbtnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BPbtnCancel.Image = global::Accounts.Properties.Resources.gnome_window_close;
            this.BPbtnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BPbtnCancel.Location = new System.Drawing.Point(711, 14);
            this.BPbtnCancel.Name = "BPbtnCancel";
            this.BPbtnCancel.Size = new System.Drawing.Size(75, 29);
            this.BPbtnCancel.TabIndex = 134;
            this.BPbtnCancel.Text = "&Cancel";
            this.BPbtnCancel.UseVisualStyleBackColor = true;
            this.BPbtnCancel.Click += new System.EventHandler(this.BPbtnCancel_Click);
            // 
            // BPbtnEdit
            // 
            this.BPbtnEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BPbtnEdit.Image = global::Accounts.Properties.Resources.document_edit;
            this.BPbtnEdit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BPbtnEdit.Location = new System.Drawing.Point(468, 13);
            this.BPbtnEdit.Name = "BPbtnEdit";
            this.BPbtnEdit.Size = new System.Drawing.Size(75, 29);
            this.BPbtnEdit.TabIndex = 133;
            this.BPbtnEdit.Text = "&Edit";
            this.BPbtnEdit.UseVisualStyleBackColor = true;
            this.BPbtnEdit.Click += new System.EventHandler(this.BPbtnEdit_Clicks);
            // 
            // BPchkDoNotClose
            // 
            this.BPchkDoNotClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.BPchkDoNotClose.AutoSize = true;
            this.BPchkDoNotClose.Location = new System.Drawing.Point(18, 19);
            this.BPchkDoNotClose.Name = "BPchkDoNotClose";
            this.BPchkDoNotClose.Size = new System.Drawing.Size(86, 17);
            this.BPchkDoNotClose.TabIndex = 135;
            this.BPchkDoNotClose.Text = "Do not close";
            this.BPchkDoNotClose.UseVisualStyleBackColor = true;
            // 
            // BPbtnDelete
            // 
            this.BPbtnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BPbtnDelete.Image = global::Accounts.Properties.Resources.document_delete;
            this.BPbtnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BPbtnDelete.Location = new System.Drawing.Point(630, 14);
            this.BPbtnDelete.Name = "BPbtnDelete";
            this.BPbtnDelete.Size = new System.Drawing.Size(75, 29);
            this.BPbtnDelete.TabIndex = 132;
            this.BPbtnDelete.Text = "&Delete";
            this.BPbtnDelete.UseVisualStyleBackColor = true;
            this.BPbtnDelete.Click += new System.EventHandler(this.BPbtnDelete_Click);
            // 
            // BPchkPrntWhileSaving
            // 
            this.BPchkPrntWhileSaving.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.BPchkPrntWhileSaving.AutoSize = true;
            this.BPchkPrntWhileSaving.Location = new System.Drawing.Point(110, 19);
            this.BPchkPrntWhileSaving.Name = "BPchkPrntWhileSaving";
            this.BPchkPrntWhileSaving.Size = new System.Drawing.Size(108, 17);
            this.BPchkPrntWhileSaving.TabIndex = 136;
            this.BPchkPrntWhileSaving.Text = "Print while saving";
            this.BPchkPrntWhileSaving.UseVisualStyleBackColor = true;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusJournal});
            this.statusStrip1.Location = new System.Drawing.Point(0, 410);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(820, 22);
            this.statusStrip1.TabIndex = 138;
            this.statusStrip1.Text = "statusStrip1";
            this.statusStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.statusStrip1_ItemClicked);
            // 
            // statusJournal
            // 
            this.statusJournal.Name = "statusJournal";
            this.statusJournal.Size = new System.Drawing.Size(313, 17);
            this.statusJournal.Text = "Press CTRL+R in the Narration Field for Previous Narration";
            // 
            // frmBankPayment
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(820, 432);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.BPtxtRemarks);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.groupBox3);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "frmBankPayment";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Bank Payment";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.BPfrmBankPayment_FormClosing);
            this.Load += new System.EventHandler(this.frmBankPayment_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmBankPayment_KeyDown);
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

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnPaste;
        private System.Windows.Forms.Button btnCopy;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnLast;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnPrev;
        private System.Windows.Forms.TabPage tabPage1;
        private SourceGrid.Grid grdBankPayment;
        private System.Windows.Forms.Label BPlblTotalAmount;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TabControl tabControl1;
        private STreeView treeAccClass;
        private System.Windows.Forms.Button BPbtnAddAccClass;
        private System.Windows.Forms.Button btnFirst;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private SComboBox BPcboProjectName;
        private SComboBox BPcboSeriesName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label BPlblVouNo;
        private SMaskedTextBox BPtxtDate;
        private SComboBox BPcmboBankAccount;
        private System.Windows.Forms.Label label6;
        private STextBox BPtxtBankID;
        private STextBox BPtxtVchNo;
        private System.Windows.Forms.Button BPbtnDate;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button BPbtnSave;
        private System.Windows.Forms.Button BPbtnNew;
        private System.Windows.Forms.Button BPbtnCancel;
        private System.Windows.Forms.Button BPbtnEdit;
        private System.Windows.Forms.CheckBox BPchkDoNotClose;
        private System.Windows.Forms.Button BPbtnDelete;
        private System.Windows.Forms.CheckBox BPchkPrntWhileSaving;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel statusJournal;
        private System.Windows.Forms.Label BPlblBankCurrentBalance;
        private System.Windows.Forms.Label BPlblBankCurrBalHidden;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button BPbtn_groupvoucherposting;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.TabPage tabPage3;
        private STextBox BPtxtfifth;
        private STextBox BPtxtfourth;
        private STextBox BPtxtthird;
        private STextBox BPtxtsecond;
        private STextBox BPtxtfirst;
        private System.Windows.Forms.Label BPlblfourth;
        private System.Windows.Forms.Label BPlblsecond;
        private System.Windows.Forms.Label BPlblthird;
        private System.Windows.Forms.Label BPlblfifth;
        private System.Windows.Forms.Label BPlblfirst;
        private System.Windows.Forms.Button btnPrintPreview;
        private System.Windows.Forms.Button btnSetup;
        private System.Windows.Forms.CheckBox chkRecurring;
        private System.Windows.Forms.Button btnList;
        private STextBox BPtxtRemarks;

    }
}