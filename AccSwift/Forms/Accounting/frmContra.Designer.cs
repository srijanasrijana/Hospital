using SComponents;
namespace Inventory
{
    partial class frmContra
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmContra));
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.chkDoNotClose = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnPrintPreview = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnPaste = new System.Windows.Forms.Button();
            this.btnCopy = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnLast = new System.Windows.Forms.Button();
            this.btnFirst = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnPrev = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnNew = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblDifferenceAmount = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.grdContra = new SourceGrid.Grid();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.label7 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblDebitTotal = new System.Windows.Forms.Label();
            this.lblCreditTotal = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.btnAddAccClass = new System.Windows.Forms.Button();
            this.treeAccClass = new STreeView(this.components);
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
            this.label6 = new System.Windows.Forms.Label();
            this.cboSeriesName = new SComboBox();
            this.txtContraID = new STextBox();
            this.txtDate = new SMaskedTextBox();
            this.txtVchNo = new STextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtRemarks = new STextBox();
            this.cboProjectName = new SComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnDate = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.statusJournal = new System.Windows.Forms.ToolStripStatusLabel();
            this.panel1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(100, 19);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(108, 17);
            this.checkBox2.TabIndex = 39;
            this.checkBox2.Text = "Print while saving";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // chkDoNotClose
            // 
            this.chkDoNotClose.AutoSize = true;
            this.chkDoNotClose.Checked = true;
            this.chkDoNotClose.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDoNotClose.Location = new System.Drawing.Point(8, 19);
            this.chkDoNotClose.Name = "chkDoNotClose";
            this.chkDoNotClose.Size = new System.Drawing.Size(86, 17);
            this.chkDoNotClose.TabIndex = 38;
            this.chkDoNotClose.Text = "Do not close";
            this.chkDoNotClose.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.CadetBlue;
            this.panel1.Controls.Add(this.btnPrintPreview);
            this.panel1.Controls.Add(this.btnExport);
            this.panel1.Controls.Add(this.btnPaste);
            this.panel1.Controls.Add(this.btnCopy);
            this.panel1.Controls.Add(this.btnExit);
            this.panel1.Controls.Add(this.btnLast);
            this.panel1.Controls.Add(this.btnFirst);
            this.panel1.Controls.Add(this.button3);
            this.panel1.Controls.Add(this.btnNext);
            this.panel1.Controls.Add(this.btnPrev);
            this.panel1.Location = new System.Drawing.Point(1, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(796, 29);
            this.panel1.TabIndex = 37;
            // 
            // btnPrintPreview
            // 
            this.btnPrintPreview.Image = global::Inventory.Properties.Resources.print_preview;
            this.btnPrintPreview.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPrintPreview.Location = new System.Drawing.Point(416, 4);
            this.btnPrintPreview.Name = "btnPrintPreview";
            this.btnPrintPreview.Size = new System.Drawing.Size(94, 23);
            this.btnPrintPreview.TabIndex = 31;
            this.btnPrintPreview.Text = "Pr&int Preview";
            this.btnPrintPreview.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnPrintPreview.UseVisualStyleBackColor = true;
            this.btnPrintPreview.Click += new System.EventHandler(this.btnPrintPreview_Click);
            // 
            // btnExport
            // 
            this.btnExport.Enabled = false;
            this.btnExport.Image = global::Inventory.Properties.Resources.export1;
            this.btnExport.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnExport.Location = new System.Drawing.Point(727, 3);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(62, 23);
            this.btnExport.TabIndex = 27;
            this.btnExport.Text = "&Export";
            this.btnExport.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnPaste
            // 
            this.btnPaste.Image = global::Inventory.Properties.Resources.paste;
            this.btnPaste.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPaste.Location = new System.Drawing.Point(297, 3);
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
            this.btnCopy.Image = global::Inventory.Properties.Resources.copy;
            this.btnCopy.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCopy.Location = new System.Drawing.Point(238, 3);
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
            this.btnExit.Image = global::Inventory.Properties.Resources.ExitButton;
            this.btnExit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnExit.Location = new System.Drawing.Point(513, 4);
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
            this.btnLast.Image = global::Inventory.Properties.Resources.last;
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
            this.btnFirst.Image = global::Inventory.Properties.Resources.first;
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
            // button3
            // 
            this.button3.Image = global::Inventory.Properties.Resources.print;
            this.button3.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button3.Location = new System.Drawing.Point(356, 3);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(58, 23);
            this.button3.TabIndex = 2;
            this.button3.Text = "Print";
            this.button3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // btnNext
            // 
            this.btnNext.Image = global::Inventory.Properties.Resources.forward;
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
            this.btnPrev.Image = global::Inventory.Properties.Resources.back;
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
            // btnCancel
            // 
            this.btnCancel.Image = global::Inventory.Properties.Resources.gnome_window_close;
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(698, 13);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 29);
            this.btnCancel.TabIndex = 36;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Image = global::Inventory.Properties.Resources.document_edit;
            this.btnEdit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEdit.Location = new System.Drawing.Point(449, 13);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(75, 29);
            this.btnEdit.TabIndex = 35;
            this.btnEdit.Text = "&Edit";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Image = global::Inventory.Properties.Resources.document_delete;
            this.btnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDelete.Location = new System.Drawing.Point(611, 13);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 29);
            this.btnDelete.TabIndex = 34;
            this.btnDelete.Text = "&Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnSave
            // 
            this.btnSave.Image = global::Inventory.Properties.Resources.save;
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSave.Location = new System.Drawing.Point(530, 13);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 29);
            this.btnSave.TabIndex = 33;
            this.btnSave.Text = "&Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnNew
            // 
            this.btnNew.Image = global::Inventory.Properties.Resources.edit_add;
            this.btnNew.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNew.Location = new System.Drawing.Point(368, 13);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(75, 29);
            this.btnNew.TabIndex = 32;
            this.btnNew.Text = "&New";
            this.btnNew.UseVisualStyleBackColor = true;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(631, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(33, 13);
            this.label3.TabIndex = 30;
            this.label3.Text = "Date:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(0, 13);
            this.label2.TabIndex = 29;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(345, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 27;
            this.label1.Text = "Voucher #:";
            this.label1.Click += new System.EventHandler(this.label1_Click);
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
            this.label5.TabIndex = 10;
            this.label5.Text = "Credit Total:";
            // 
            // grdContra
            // 
            this.grdContra.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdContra.Location = new System.Drawing.Point(4, 3);
            this.grdContra.Name = "grdContra";
            this.grdContra.OptimizeMode = SourceGrid.CellOptimizeMode.ForRows;
            this.grdContra.SelectionMode = SourceGrid.GridSelectionMode.Cell;
            this.grdContra.Size = new System.Drawing.Size(775, 162);
            this.grdContra.TabIndex = 1;
            this.grdContra.TabStop = true;
            this.grdContra.ToolTipText = "";
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.grdContra);
            this.tabPage1.Controls.Add(this.lblDifferenceAmount);
            this.tabPage1.Controls.Add(this.label5);
            this.tabPage1.Controls.Add(this.label7);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.lblDebitTotal);
            this.tabPage1.Controls.Add(this.lblCreditTotal);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(779, 193);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Details";
            this.tabPage1.UseVisualStyleBackColor = true;
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
            this.label4.TabIndex = 9;
            this.label4.Text = "Debit Total:";
            // 
            // lblDebitTotal
            // 
            this.lblDebitTotal.Location = new System.Drawing.Point(244, 225);
            this.lblDebitTotal.Name = "lblDebitTotal";
            this.lblDebitTotal.Size = new System.Drawing.Size(108, 13);
            this.lblDebitTotal.TabIndex = 11;
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
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(5, 104);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(787, 219);
            this.tabControl1.TabIndex = 25;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.btnAddAccClass);
            this.tabPage2.Controls.Add(this.treeAccClass);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(779, 193);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Classes";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // btnAddAccClass
            // 
            this.btnAddAccClass.Location = new System.Drawing.Point(402, 6);
            this.btnAddAccClass.Name = "btnAddAccClass";
            this.btnAddAccClass.Size = new System.Drawing.Size(109, 23);
            this.btnAddAccClass.TabIndex = 36;
            this.btnAddAccClass.Text = "Add Account Class";
            this.btnAddAccClass.UseVisualStyleBackColor = true;
            this.btnAddAccClass.Click += new System.EventHandler(this.btnAddAccClass_Click);
            // 
            // treeAccClass
            // 
            this.treeAccClass.AutoCheckChild = true;
            this.treeAccClass.CheckBoxes = true;
            this.treeAccClass.Location = new System.Drawing.Point(-2, -3);
            this.treeAccClass.Name = "treeAccClass";
            this.treeAccClass.Size = new System.Drawing.Size(398, 259);
            this.treeAccClass.TabIndex = 31;
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
            this.tabPage3.Size = new System.Drawing.Size(779, 193);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Additional Fields";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // txtfifth
            // 
            this.txtfifth.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtfifth.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtfifth.FocusLostColor = System.Drawing.Color.White;
            this.txtfifth.Location = new System.Drawing.Point(110, 164);
            this.txtfifth.Name = "txtfifth";
            this.txtfifth.Size = new System.Drawing.Size(182, 20);
            this.txtfifth.TabIndex = 30;
            this.txtfifth.Visible = false;
            // 
            // txtfourth
            // 
            this.txtfourth.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtfourth.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtfourth.FocusLostColor = System.Drawing.Color.White;
            this.txtfourth.Location = new System.Drawing.Point(110, 125);
            this.txtfourth.Name = "txtfourth";
            this.txtfourth.Size = new System.Drawing.Size(182, 20);
            this.txtfourth.TabIndex = 29;
            this.txtfourth.Visible = false;
            // 
            // txtthird
            // 
            this.txtthird.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtthird.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtthird.FocusLostColor = System.Drawing.Color.White;
            this.txtthird.Location = new System.Drawing.Point(110, 87);
            this.txtthird.Name = "txtthird";
            this.txtthird.Size = new System.Drawing.Size(182, 20);
            this.txtthird.TabIndex = 28;
            this.txtthird.Visible = false;
            // 
            // txtsecond
            // 
            this.txtsecond.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtsecond.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtsecond.FocusLostColor = System.Drawing.Color.White;
            this.txtsecond.Location = new System.Drawing.Point(110, 47);
            this.txtsecond.Name = "txtsecond";
            this.txtsecond.Size = new System.Drawing.Size(182, 20);
            this.txtsecond.TabIndex = 27;
            this.txtsecond.Visible = false;
            // 
            // txtfirst
            // 
            this.txtfirst.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtfirst.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtfirst.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtfirst.FocusLostColor = System.Drawing.Color.White;
            this.txtfirst.Location = new System.Drawing.Point(110, 13);
            this.txtfirst.Name = "txtfirst";
            this.txtfirst.Size = new System.Drawing.Size(182, 20);
            this.txtfirst.TabIndex = 26;
            this.txtfirst.Visible = false;
            // 
            // lblfourth
            // 
            this.lblfourth.AutoSize = true;
            this.lblfourth.Location = new System.Drawing.Point(10, 131);
            this.lblfourth.Name = "lblfourth";
            this.lblfourth.Size = new System.Drawing.Size(80, 13);
            this.lblfourth.TabIndex = 25;
            this.lblfourth.Text = "Optional Field 4";
            this.lblfourth.Visible = false;
            // 
            // lblsecond
            // 
            this.lblsecond.AutoSize = true;
            this.lblsecond.Location = new System.Drawing.Point(10, 53);
            this.lblsecond.Name = "lblsecond";
            this.lblsecond.Size = new System.Drawing.Size(80, 13);
            this.lblsecond.TabIndex = 24;
            this.lblsecond.Text = "Optional Field 2";
            this.lblsecond.Visible = false;
            // 
            // lblthird
            // 
            this.lblthird.AutoSize = true;
            this.lblthird.Location = new System.Drawing.Point(10, 92);
            this.lblthird.Name = "lblthird";
            this.lblthird.Size = new System.Drawing.Size(80, 13);
            this.lblthird.TabIndex = 23;
            this.lblthird.Text = "Optional Field 3";
            this.lblthird.Visible = false;
            // 
            // lblfifth
            // 
            this.lblfifth.AutoSize = true;
            this.lblfifth.Location = new System.Drawing.Point(10, 170);
            this.lblfifth.Name = "lblfifth";
            this.lblfifth.Size = new System.Drawing.Size(80, 13);
            this.lblfifth.TabIndex = 22;
            this.lblfifth.Text = "Optional Field 5";
            this.lblfifth.Visible = false;
            // 
            // lblfirst
            // 
            this.lblfirst.AutoSize = true;
            this.lblfirst.Location = new System.Drawing.Point(10, 14);
            this.lblfirst.Name = "lblfirst";
            this.lblfirst.Size = new System.Drawing.Size(80, 13);
            this.lblfirst.TabIndex = 21;
            this.lblfirst.Text = "Optional Field 1";
            this.lblfirst.Visible = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(15, 15);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(42, 13);
            this.label6.TabIndex = 42;
            this.label6.Text = "Series :";
            // 
            // cboSeriesName
            // 
            this.cboSeriesName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboSeriesName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboSeriesName.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.cboSeriesName.FocusLostColor = System.Drawing.Color.White;
            this.cboSeriesName.FormattingEnabled = true;
            this.cboSeriesName.Location = new System.Drawing.Point(113, 13);
            this.cboSeriesName.Name = "cboSeriesName";
            this.cboSeriesName.Size = new System.Drawing.Size(207, 21);
            this.cboSeriesName.TabIndex = 41;
            this.cboSeriesName.SelectedIndexChanged += new System.EventHandler(this.cboSeriesName_SelectedIndexChanged);
            // 
            // txtContraID
            // 
            this.txtContraID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtContraID.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtContraID.FocusLostColor = System.Drawing.Color.White;
            this.txtContraID.Location = new System.Drawing.Point(411, 41);
            this.txtContraID.Name = "txtContraID";
            this.txtContraID.Size = new System.Drawing.Size(88, 20);
            this.txtContraID.TabIndex = 40;
            // 
            // txtDate
            // 
            this.txtDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDate.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.txtDate.FocusLostColor = System.Drawing.Color.White;
            this.txtDate.Location = new System.Drawing.Point(676, 13);
            this.txtDate.Mask = "0000/00/00";
            this.txtDate.Name = "txtDate";
            this.txtDate.Size = new System.Drawing.Size(74, 20);
            this.txtDate.TabIndex = 31;
            // 
            // txtVchNo
            // 
            this.txtVchNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtVchNo.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtVchNo.FocusLostColor = System.Drawing.Color.White;
            this.txtVchNo.Location = new System.Drawing.Point(411, 13);
            this.txtVchNo.Name = "txtVchNo";
            this.txtVchNo.Size = new System.Drawing.Size(194, 20);
            this.txtVchNo.TabIndex = 26;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(17, 330);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(53, 13);
            this.label8.TabIndex = 44;
            this.label8.Text = "Narration:";
            // 
            // txtRemarks
            // 
            this.txtRemarks.BackColor = System.Drawing.Color.White;
            this.txtRemarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtRemarks.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtRemarks.FocusLostColor = System.Drawing.Color.White;
            this.txtRemarks.Location = new System.Drawing.Point(115, 328);
            this.txtRemarks.Name = "txtRemarks";
            this.txtRemarks.Size = new System.Drawing.Size(673, 20);
            this.txtRemarks.TabIndex = 43;
            // 
            // cboProjectName
            // 
            this.cboProjectName.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.cboProjectName.FocusLostColor = System.Drawing.Color.White;
            this.cboProjectName.FormattingEnabled = true;
            this.cboProjectName.Location = new System.Drawing.Point(113, 40);
            this.cboProjectName.Name = "cboProjectName";
            this.cboProjectName.Size = new System.Drawing.Size(207, 21);
            this.cboProjectName.TabIndex = 45;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(15, 43);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(46, 13);
            this.label9.TabIndex = 46;
            this.label9.Text = "Project :";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnDate);
            this.groupBox2.Controls.Add(this.cboProjectName);
            this.groupBox2.Controls.Add(this.cboSeriesName);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.txtVchNo);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.txtDate);
            this.groupBox2.Controls.Add(this.txtContraID);
            this.groupBox2.Location = new System.Drawing.Point(2, 30);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(790, 68);
            this.groupBox2.TabIndex = 79;
            this.groupBox2.TabStop = false;
            this.groupBox2.Enter += new System.EventHandler(this.groupBox2_Enter);
            // 
            // btnDate
            // 
            this.btnDate.Image = global::Inventory.Properties.Resources.dateIcon;
            this.btnDate.Location = new System.Drawing.Point(750, 11);
            this.btnDate.Name = "btnDate";
            this.btnDate.Size = new System.Drawing.Size(26, 23);
            this.btnDate.TabIndex = 3;
            this.btnDate.UseVisualStyleBackColor = true;
            this.btnDate.Click += new System.EventHandler(this.btnDate_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnNew);
            this.groupBox1.Controls.Add(this.btnSave);
            this.groupBox1.Controls.Add(this.btnDelete);
            this.groupBox1.Controls.Add(this.btnEdit);
            this.groupBox1.Controls.Add(this.checkBox2);
            this.groupBox1.Controls.Add(this.btnCancel);
            this.groupBox1.Controls.Add(this.chkDoNotClose);
            this.groupBox1.Location = new System.Drawing.Point(5, 351);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(783, 48);
            this.groupBox1.TabIndex = 149;
            this.groupBox1.TabStop = false;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusJournal});
            this.statusStrip1.Location = new System.Drawing.Point(0, 399);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(797, 22);
            this.statusStrip1.TabIndex = 150;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // statusJournal
            // 
            this.statusJournal.Name = "statusJournal";
            this.statusJournal.Size = new System.Drawing.Size(313, 17);
            this.statusJournal.Text = "Press CTRL+R in the Narration Field for Previous Narration";
            // 
            // frmContra
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(797, 421);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.txtRemarks);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "frmContra";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Contra";
            this.Load += new System.EventHandler(this.frmContra_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmContra_KeyDown);
            this.panel1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
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

        private STextBox txtContraID;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.CheckBox chkDoNotClose;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnPaste;
        private System.Windows.Forms.Button btnCopy;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnLast;
        private System.Windows.Forms.Button btnFirst;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnPrev;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnNew;
        private SMaskedTextBox txtDate;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblDifferenceAmount;
        private System.Windows.Forms.Label label5;
        private SourceGrid.Grid grdContra;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblDebitTotal;
        private System.Windows.Forms.Label lblCreditTotal;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage2;
        private STextBox txtVchNo;
        private SComboBox cboSeriesName;
        private System.Windows.Forms.Label label6;
        private STreeView treeAccClass;
        private System.Windows.Forms.Button btnAddAccClass;
        private System.Windows.Forms.Label label8;
        private STextBox txtRemarks;
        private SComboBox cboProjectName;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnDate;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel statusJournal;
        private System.Windows.Forms.Button btnExport;
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
        private System.Windows.Forms.Button btnPrintPreview;
    }
}