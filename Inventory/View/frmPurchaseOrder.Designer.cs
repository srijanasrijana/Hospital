using SComponents;
namespace Inventory
{
    partial class frmPurchaseOrder
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPurchaseOrder));
            this.btnDate = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.btnAddAccClass = new System.Windows.Forms.Button();
            this.treeAccClass = new SComponents.STreeView(this.components);
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.lblNetAmout = new System.Windows.Forms.Label();
            this.lblTotalQty = new System.Windows.Forms.Label();
            this.lablel4 = new System.Windows.Forms.Label();
            this.lablel1 = new System.Windows.Forms.Label();
            this.grdPurchaseOrder = new SourceGrid.Grid();
            this.btnPrintPreview = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnPaste = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnCopy = new System.Windows.Forms.Button();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.txtDate = new SComponents.SMaskedTextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnList = new System.Windows.Forms.Button();
            this.btnSetup = new System.Windows.Forms.Button();
            this.chkRecurring = new System.Windows.Forms.CheckBox();
            this.btnLast = new System.Windows.Forms.Button();
            this.btnFirst = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnPrev = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.label5 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.chkDoNotClose = new System.Windows.Forms.CheckBox();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnNew = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.txtRemarks = new SComponents.STextBox();
            this.cboCashParty = new SComponents.SComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtPurchaseOrderID = new SComponents.STextBox();
            this.cboProjectName = new SComponents.SComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtOrderNo = new SComponents.STextBox();
            this.tabPage2.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnDate
            // 
            this.btnDate.Image = global::Inventory.Properties.Resources.dateIcon;
            this.btnDate.Location = new System.Drawing.Point(608, 47);
            this.btnDate.Name = "btnDate";
            this.btnDate.Size = new System.Drawing.Size(26, 23);
            this.btnDate.TabIndex = 197;
            this.btnDate.UseVisualStyleBackColor = true;
            this.btnDate.Click += new System.EventHandler(this.btnDate_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(393, 52);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(33, 13);
            this.label4.TabIndex = 198;
            this.label4.Text = "Date:";
            // 
            // btnAddAccClass
            // 
            this.btnAddAccClass.Location = new System.Drawing.Point(485, 3);
            this.btnAddAccClass.Name = "btnAddAccClass";
            this.btnAddAccClass.Size = new System.Drawing.Size(109, 23);
            this.btnAddAccClass.TabIndex = 33;
            this.btnAddAccClass.Text = "Add Account Class";
            this.btnAddAccClass.UseVisualStyleBackColor = true;
            this.btnAddAccClass.Click += new System.EventHandler(this.btnAddAccClass_Click);
            // 
            // treeAccClass
            // 
            this.treeAccClass.AutoCheckChild = true;
            this.treeAccClass.CheckBoxes = true;
            this.treeAccClass.Location = new System.Drawing.Point(3, 0);
            this.treeAccClass.Name = "treeAccClass";
            this.treeAccClass.Size = new System.Drawing.Size(476, 267);
            this.treeAccClass.TabIndex = 28;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.btnAddAccClass);
            this.tabPage2.Controls.Add(this.treeAccClass);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(869, 270);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Account Class";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // lblNetAmout
            // 
            this.lblNetAmout.Location = new System.Drawing.Point(626, 247);
            this.lblNetAmout.Name = "lblNetAmout";
            this.lblNetAmout.Size = new System.Drawing.Size(49, 13);
            this.lblNetAmout.TabIndex = 12;
            this.lblNetAmout.Text = "0.00";
            this.lblNetAmout.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTotalQty
            // 
            this.lblTotalQty.Location = new System.Drawing.Point(162, 247);
            this.lblTotalQty.Name = "lblTotalQty";
            this.lblTotalQty.Size = new System.Drawing.Size(49, 13);
            this.lblTotalQty.TabIndex = 12;
            this.lblTotalQty.Text = "0.00";
            this.lblTotalQty.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lablel4
            // 
            this.lablel4.AutoSize = true;
            this.lablel4.Location = new System.Drawing.Point(52, 247);
            this.lablel4.Name = "lablel4";
            this.lablel4.Size = new System.Drawing.Size(56, 13);
            this.lablel4.TabIndex = 1;
            this.lablel4.Text = "Total Qty.:";
            // 
            // lablel1
            // 
            this.lablel1.AutoSize = true;
            this.lablel1.Location = new System.Drawing.Point(530, 247);
            this.lablel1.Name = "lablel1";
            this.lablel1.Size = new System.Drawing.Size(66, 13);
            this.lablel1.TabIndex = 1;
            this.lablel1.Text = "Net Amount:";
            // 
            // grdPurchaseOrder
            // 
            this.grdPurchaseOrder.EnableSort = true;
            this.grdPurchaseOrder.Location = new System.Drawing.Point(3, 6);
            this.grdPurchaseOrder.Name = "grdPurchaseOrder";
            this.grdPurchaseOrder.OptimizeMode = SourceGrid.CellOptimizeMode.ForRows;
            this.grdPurchaseOrder.SelectionMode = SourceGrid.GridSelectionMode.Cell;
            this.grdPurchaseOrder.Size = new System.Drawing.Size(860, 232);
            this.grdPurchaseOrder.TabIndex = 0;
            this.grdPurchaseOrder.TabStop = true;
            this.grdPurchaseOrder.ToolTipText = "";
            // 
            // btnPrintPreview
            // 
            this.btnPrintPreview.Image = global::Inventory.Properties.Resources.print_preview;
            this.btnPrintPreview.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPrintPreview.Location = new System.Drawing.Point(520, 10);
            this.btnPrintPreview.Name = "btnPrintPreview";
            this.btnPrintPreview.Size = new System.Drawing.Size(94, 23);
            this.btnPrintPreview.TabIndex = 25;
            this.btnPrintPreview.Text = "Pr&int Preview";
            this.btnPrintPreview.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnPrintPreview.UseVisualStyleBackColor = true;
            this.btnPrintPreview.Click += new System.EventHandler(this.btnPrintPreview_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.Image = global::Inventory.Properties.Resources.print;
            this.btnPrint.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPrint.Location = new System.Drawing.Point(456, 10);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(58, 23);
            this.btnPrint.TabIndex = 2;
            this.btnPrint.Text = "Print";
            this.btnPrint.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnPaste
            // 
            this.btnPaste.Image = global::Inventory.Properties.Resources.paste;
            this.btnPaste.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPaste.Location = new System.Drawing.Point(392, 10);
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
            this.btnExit.Image = global::Inventory.Properties.Resources.ExitButton;
            this.btnExit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnExit.Location = new System.Drawing.Point(620, 10);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(58, 23);
            this.btnExit.TabIndex = 22;
            this.btnExit.Text = "Exit";
            this.btnExit.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnCopy
            // 
            this.btnCopy.Image = global::Inventory.Properties.Resources.copy;
            this.btnCopy.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCopy.Location = new System.Drawing.Point(328, 10);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(58, 23);
            this.btnCopy.TabIndex = 23;
            this.btnCopy.Text = "Copy";
            this.btnCopy.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCopy.UseVisualStyleBackColor = true;
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.lblNetAmout);
            this.tabPage1.Controls.Add(this.lblTotalQty);
            this.tabPage1.Controls.Add(this.lablel4);
            this.tabPage1.Controls.Add(this.lablel1);
            this.tabPage1.Controls.Add(this.grdPurchaseOrder);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(869, 270);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Details";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // txtDate
            // 
            this.txtDate.BackColor = System.Drawing.Color.White;
            this.txtDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDate.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.txtDate.FocusLostColor = System.Drawing.Color.White;
            this.txtDate.Location = new System.Drawing.Point(460, 50);
            this.txtDate.Mask = "0000/00/00";
            this.txtDate.Name = "txtDate";
            this.txtDate.Size = new System.Drawing.Size(133, 20);
            this.txtDate.TabIndex = 2;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.CadetBlue;
            this.panel1.Controls.Add(this.btnList);
            this.panel1.Controls.Add(this.btnSetup);
            this.panel1.Controls.Add(this.btnPrintPreview);
            this.panel1.Controls.Add(this.chkRecurring);
            this.panel1.Controls.Add(this.btnPrint);
            this.panel1.Controls.Add(this.btnPaste);
            this.panel1.Controls.Add(this.btnLast);
            this.panel1.Controls.Add(this.btnExit);
            this.panel1.Controls.Add(this.btnFirst);
            this.panel1.Controls.Add(this.btnCopy);
            this.panel1.Controls.Add(this.btnNext);
            this.panel1.Controls.Add(this.btnPrev);
            this.panel1.Location = new System.Drawing.Point(-1, -1);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(919, 41);
            this.panel1.TabIndex = 193;
            // 
            // btnList
            // 
            this.btnList.Location = new System.Drawing.Point(269, 10);
            this.btnList.Name = "btnList";
            this.btnList.Size = new System.Drawing.Size(53, 23);
            this.btnList.TabIndex = 59;
            this.btnList.Text = "List";
            this.btnList.UseVisualStyleBackColor = true;
            this.btnList.Click += new System.EventHandler(this.btnList_Click);
            // 
            // btnSetup
            // 
            this.btnSetup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSetup.Location = new System.Drawing.Point(806, 9);
            this.btnSetup.Name = "btnSetup";
            this.btnSetup.Size = new System.Drawing.Size(43, 23);
            this.btnSetup.TabIndex = 204;
            this.btnSetup.Text = "setup";
            this.btnSetup.UseVisualStyleBackColor = true;
            this.btnSetup.Click += new System.EventHandler(this.btnSetup_Click);
            // 
            // chkRecurring
            // 
            this.chkRecurring.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkRecurring.AutoSize = true;
            this.chkRecurring.ForeColor = System.Drawing.Color.White;
            this.chkRecurring.Location = new System.Drawing.Point(726, 12);
            this.chkRecurring.Margin = new System.Windows.Forms.Padding(2);
            this.chkRecurring.Name = "chkRecurring";
            this.chkRecurring.Size = new System.Drawing.Size(72, 17);
            this.chkRecurring.TabIndex = 203;
            this.chkRecurring.Text = "Recurrin&g";
            this.chkRecurring.UseVisualStyleBackColor = true;
            this.chkRecurring.CheckedChanged += new System.EventHandler(this.chkRecurring_CheckedChanged);
            // 
            // btnLast
            // 
            this.btnLast.Image = global::Inventory.Properties.Resources.last;
            this.btnLast.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnLast.Location = new System.Drawing.Point(205, 10);
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
            this.btnFirst.Location = new System.Drawing.Point(13, 10);
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
            this.btnNext.Image = global::Inventory.Properties.Resources.forward;
            this.btnNext.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnNext.Location = new System.Drawing.Point(141, 10);
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
            this.btnPrev.Location = new System.Drawing.Point(77, 10);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(58, 23);
            this.btnPrev.TabIndex = 0;
            this.btnPrev.Text = "&Prev";
            this.btnPrev.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnPrev.UseVisualStyleBackColor = true;
            this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(6, 111);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(877, 296);
            this.tabControl1.TabIndex = 182;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(14, 422);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(56, 13);
            this.label5.TabIndex = 176;
            this.label5.Text = "*Remarks:";
            // 
            // btnCancel
            // 
            this.btnCancel.Image = global::Inventory.Properties.Resources.gnome_window_close;
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(808, 451);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Image = global::Inventory.Properties.Resources.document_edit;
            this.btnEdit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEdit.Location = new System.Drawing.Point(565, 450);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(75, 23);
            this.btnEdit.TabIndex = 6;
            this.btnEdit.Text = "&Edit";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // chkDoNotClose
            // 
            this.chkDoNotClose.AutoSize = true;
            this.chkDoNotClose.Location = new System.Drawing.Point(10, 452);
            this.chkDoNotClose.Name = "chkDoNotClose";
            this.chkDoNotClose.Size = new System.Drawing.Size(86, 17);
            this.chkDoNotClose.TabIndex = 188;
            this.chkDoNotClose.Text = "Do not close";
            this.chkDoNotClose.UseVisualStyleBackColor = true;
            // 
            // btnDelete
            // 
            this.btnDelete.Image = global::Inventory.Properties.Resources.document_delete;
            this.btnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDelete.Location = new System.Drawing.Point(727, 450);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 8;
            this.btnDelete.Text = "&Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnSave
            // 
            this.btnSave.Image = global::Inventory.Properties.Resources.save;
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSave.Location = new System.Drawing.Point(646, 451);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 7;
            this.btnSave.Text = "&Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnNew
            // 
            this.btnNew.Image = global::Inventory.Properties.Resources.edit_add;
            this.btnNew.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNew.Location = new System.Drawing.Point(484, 450);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(75, 23);
            this.btnNew.TabIndex = 5;
            this.btnNew.Text = "&New";
            this.btnNew.UseVisualStyleBackColor = true;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(14, 59);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(53, 13);
            this.label9.TabIndex = 191;
            this.label9.Text = "Order No:";
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(155, 452);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(108, 17);
            this.checkBox2.TabIndex = 189;
            this.checkBox2.Text = "Print while saving";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // txtRemarks
            // 
            this.txtRemarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtRemarks.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtRemarks.FocusLostColor = System.Drawing.Color.White;
            this.txtRemarks.Location = new System.Drawing.Point(81, 418);
            this.txtRemarks.Multiline = true;
            this.txtRemarks.Name = "txtRemarks";
            this.txtRemarks.Size = new System.Drawing.Size(798, 21);
            this.txtRemarks.TabIndex = 1;
            // 
            // cboCashParty
            // 
            this.cboCashParty.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCashParty.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.cboCashParty.FocusLostColor = System.Drawing.Color.White;
            this.cboCashParty.FormattingEnabled = true;
            this.cboCashParty.Location = new System.Drawing.Point(127, 79);
            this.cboCashParty.Name = "cboCashParty";
            this.cboCashParty.Size = new System.Drawing.Size(205, 21);
            this.cboCashParty.TabIndex = 3;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(11, 87);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(86, 13);
            this.label10.TabIndex = 178;
            this.label10.Text = "*Cash/Party A/C";
            // 
            // txtPurchaseOrderID
            // 
            this.txtPurchaseOrderID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPurchaseOrderID.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtPurchaseOrderID.FocusLostColor = System.Drawing.Color.White;
            this.txtPurchaseOrderID.Location = new System.Drawing.Point(687, 82);
            this.txtPurchaseOrderID.Name = "txtPurchaseOrderID";
            this.txtPurchaseOrderID.Size = new System.Drawing.Size(100, 20);
            this.txtPurchaseOrderID.TabIndex = 192;
            // 
            // cboProjectName
            // 
            this.cboProjectName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboProjectName.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.cboProjectName.FocusLostColor = System.Drawing.Color.White;
            this.cboProjectName.FormattingEnabled = true;
            this.cboProjectName.Location = new System.Drawing.Point(460, 79);
            this.cboProjectName.Name = "cboProjectName";
            this.cboProjectName.Size = new System.Drawing.Size(174, 21);
            this.cboProjectName.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(393, 82);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 13);
            this.label1.TabIndex = 200;
            this.label1.Text = "Project :";
            // 
            // txtOrderNo
            // 
            this.txtOrderNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtOrderNo.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtOrderNo.FocusLostColor = System.Drawing.Color.White;
            this.txtOrderNo.Location = new System.Drawing.Point(127, 52);
            this.txtOrderNo.Multiline = true;
            this.txtOrderNo.Name = "txtOrderNo";
            this.txtOrderNo.ReadOnly = true;
            this.txtOrderNo.Size = new System.Drawing.Size(205, 21);
            this.txtOrderNo.TabIndex = 0;
            // 
            // frmPurchaseOrder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(895, 485);
            this.Controls.Add(this.txtOrderNo);
            this.Controls.Add(this.cboProjectName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnDate);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtDate);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.chkDoNotClose);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnNew);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.checkBox2);
            this.Controls.Add(this.txtRemarks);
            this.Controls.Add(this.cboCashParty);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.txtPurchaseOrderID);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmPurchaseOrder";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Purchase Order";
            this.Load += new System.EventHandler(this.frmPurchaseOrder_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmPurchaseOrder_KeyDown);
            this.tabPage2.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnDate;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnAddAccClass;
        private STreeView treeAccClass;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label lblNetAmout;
        private System.Windows.Forms.Label lblTotalQty;
        private System.Windows.Forms.Label lablel4;
        private System.Windows.Forms.Label lablel1;
        private SourceGrid.Grid grdPurchaseOrder;
        private System.Windows.Forms.TabPage tabPage1;
        private SMaskedTextBox txtDate;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnPaste;
        private System.Windows.Forms.Button btnCopy;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnLast;
        private System.Windows.Forms.Button btnFirst;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnPrev;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.CheckBox chkDoNotClose;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.CheckBox checkBox2;
        private STextBox txtRemarks;
        private SComboBox cboCashParty;
        private System.Windows.Forms.Label label10;
        private STextBox txtPurchaseOrderID;
        private SComboBox cboProjectName;
        private System.Windows.Forms.Label label1;
        private STextBox txtOrderNo;
        private System.Windows.Forms.Button btnPrintPreview;
        private System.Windows.Forms.Button btnSetup;
        private System.Windows.Forms.CheckBox chkRecurring;
        private System.Windows.Forms.Button btnList;
    }
}