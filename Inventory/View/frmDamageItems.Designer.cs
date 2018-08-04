using SComponents;
namespace Inventory
{
    partial class frmDamageItems
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDamageItems));
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnPrintPreview = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnPaste = new System.Windows.Forms.Button();
            this.btnCopy = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnLast = new System.Windows.Forms.Button();
            this.btnFirst = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnPrev = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.lblVouNo = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblDepot = new System.Windows.Forms.Label();
            this.btnDate = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.grddamage = new SourceGrid.Grid();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.btnAddAccClass = new System.Windows.Forms.Button();
            this.treeAccClass = new SComponents.STreeView(this.components);
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.txtfifth = new SComponents.STextBox();
            this.txtfourth = new SComponents.STextBox();
            this.txtthird = new SComponents.STextBox();
            this.txtsecond = new SComponents.STextBox();
            this.txtfirst = new SComponents.STextBox();
            this.lblfourth = new System.Windows.Forms.Label();
            this.lblsecond = new System.Windows.Forms.Label();
            this.lblthird = new System.Windows.Forms.Label();
            this.lblfifth = new System.Windows.Forms.Label();
            this.lblfirst = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnNew = new System.Windows.Forms.Button();
            this.txtDamageProductID = new SComponents.STextBox();
            this.txtDate = new SComponents.SMaskedTextBox();
            this.cboProjectName = new SComponents.SComboBox();
            this.cboDepot = new SComponents.SComboBox();
            this.cboSeriesName = new SComponents.SComboBox();
            this.txtVoucherNo = new SComponents.STextBox();
            this.panel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.CadetBlue;
            this.panel1.Controls.Add(this.btnPrintPreview);
            this.panel1.Controls.Add(this.btnExport);
            this.panel1.Controls.Add(this.btnPrint);
            this.panel1.Controls.Add(this.btnPaste);
            this.panel1.Controls.Add(this.btnCopy);
            this.panel1.Controls.Add(this.btnExit);
            this.panel1.Controls.Add(this.btnLast);
            this.panel1.Controls.Add(this.btnFirst);
            this.panel1.Controls.Add(this.btnNext);
            this.panel1.Controls.Add(this.btnPrev);
            this.panel1.Location = new System.Drawing.Point(5, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(788, 38);
            this.panel1.TabIndex = 142;
            // 
            // btnPrintPreview
            // 
            this.btnPrintPreview.Image = global::Inventory.Properties.Resources.print_preview;
            this.btnPrintPreview.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPrintPreview.Location = new System.Drawing.Point(461, 8);
            this.btnPrintPreview.Name = "btnPrintPreview";
            this.btnPrintPreview.Size = new System.Drawing.Size(94, 23);
            this.btnPrintPreview.TabIndex = 221;
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
            this.btnExport.Location = new System.Drawing.Point(717, 7);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(62, 23);
            this.btnExport.TabIndex = 220;
            this.btnExport.Text = "&Export";
            this.btnExport.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.Image = global::Inventory.Properties.Resources.print;
            this.btnPrint.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPrint.Location = new System.Drawing.Point(397, 7);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(58, 23);
            this.btnPrint.TabIndex = 219;
            this.btnPrint.Text = "Print";
            this.btnPrint.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnPaste
            // 
            this.btnPaste.Image = global::Inventory.Properties.Resources.print;
            this.btnPaste.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPaste.Location = new System.Drawing.Point(333, 7);
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
            this.btnCopy.Image = global::Inventory.Properties.Resources.print;
            this.btnCopy.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCopy.Location = new System.Drawing.Point(269, 7);
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
            this.btnExit.Image = global::Inventory.Properties.Resources.left1_2_;
            this.btnExit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnExit.Location = new System.Drawing.Point(564, 9);
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
            this.btnLast.Image = global::Inventory.Properties.Resources._1299891326_control_stop_000_small;
            this.btnLast.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnLast.Location = new System.Drawing.Point(205, 7);
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
            this.btnFirst.Image = global::Inventory.Properties.Resources.left1_2_;
            this.btnFirst.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnFirst.Location = new System.Drawing.Point(13, 7);
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
            this.btnNext.Image = global::Inventory.Properties.Resources.right1_2_;
            this.btnNext.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnNext.Location = new System.Drawing.Point(141, 7);
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
            this.btnPrev.Image = global::Inventory.Properties.Resources.left1_2_;
            this.btnPrev.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPrev.Location = new System.Drawing.Point(77, 7);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(58, 23);
            this.btnPrev.TabIndex = 0;
            this.btnPrev.Text = "&Prev";
            this.btnPrev.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnPrev.UseVisualStyleBackColor = true;
            this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(7, 50);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(67, 13);
            this.label9.TabIndex = 146;
            this.label9.Text = "Series Name";
            // 
            // lblVouNo
            // 
            this.lblVouNo.AutoSize = true;
            this.lblVouNo.Location = new System.Drawing.Point(288, 54);
            this.lblVouNo.Name = "lblVouNo";
            this.lblVouNo.Size = new System.Drawing.Size(71, 13);
            this.lblVouNo.TabIndex = 144;
            this.lblVouNo.Text = "*Voucher No:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(288, 83);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 13);
            this.label2.TabIndex = 150;
            this.label2.Text = "Project :";
            // 
            // lblDepot
            // 
            this.lblDepot.AutoSize = true;
            this.lblDepot.Location = new System.Drawing.Point(7, 84);
            this.lblDepot.Name = "lblDepot";
            this.lblDepot.Size = new System.Drawing.Size(82, 13);
            this.lblDepot.TabIndex = 147;
            this.lblDepot.Text = "Depot/Location";
            // 
            // btnDate
            // 
            this.btnDate.Image = global::Inventory.Properties.Resources.dateIcon;
            this.btnDate.Location = new System.Drawing.Point(746, 51);
            this.btnDate.Name = "btnDate";
            this.btnDate.Size = new System.Drawing.Size(26, 23);
            this.btnDate.TabIndex = 210;
            this.btnDate.UseVisualStyleBackColor = true;
            this.btnDate.Click += new System.EventHandler(this.btnDate_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(583, 57);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(37, 13);
            this.label4.TabIndex = 208;
            this.label4.Text = "*Date:";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(3, 110);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(794, 249);
            this.tabControl1.TabIndex = 211;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.grddamage);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(786, 223);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Details";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // grddamage
            // 
            this.grddamage.EnableSort = true;
            this.grddamage.Location = new System.Drawing.Point(3, 6);
            this.grddamage.Name = "grddamage";
            this.grddamage.OptimizeMode = SourceGrid.CellOptimizeMode.ForRows;
            this.grddamage.SelectionMode = SourceGrid.GridSelectionMode.Cell;
            this.grddamage.Size = new System.Drawing.Size(774, 182);
            this.grddamage.TabIndex = 0;
            this.grddamage.TabStop = true;
            this.grddamage.ToolTipText = "";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.btnAddAccClass);
            this.tabPage2.Controls.Add(this.treeAccClass);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(786, 223);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Account Class";
            this.tabPage2.UseVisualStyleBackColor = true;
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
            this.treeAccClass.Size = new System.Drawing.Size(476, 217);
            this.treeAccClass.TabIndex = 28;
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
            this.tabPage3.Size = new System.Drawing.Size(786, 223);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Additional Fields";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // txtfifth
            // 
            this.txtfifth.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtfifth.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtfifth.FocusLostColor = System.Drawing.Color.White;
            this.txtfifth.Location = new System.Drawing.Point(117, 167);
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
            this.txtfourth.Location = new System.Drawing.Point(117, 128);
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
            this.txtthird.Location = new System.Drawing.Point(117, 90);
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
            this.txtsecond.Location = new System.Drawing.Point(117, 50);
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
            this.txtfirst.Location = new System.Drawing.Point(117, 16);
            this.txtfirst.Name = "txtfirst";
            this.txtfirst.Size = new System.Drawing.Size(182, 20);
            this.txtfirst.TabIndex = 26;
            this.txtfirst.Visible = false;
            // 
            // lblfourth
            // 
            this.lblfourth.AutoSize = true;
            this.lblfourth.Location = new System.Drawing.Point(17, 134);
            this.lblfourth.Name = "lblfourth";
            this.lblfourth.Size = new System.Drawing.Size(80, 13);
            this.lblfourth.TabIndex = 25;
            this.lblfourth.Text = "Optional Field 4";
            this.lblfourth.Visible = false;
            // 
            // lblsecond
            // 
            this.lblsecond.AutoSize = true;
            this.lblsecond.Location = new System.Drawing.Point(17, 56);
            this.lblsecond.Name = "lblsecond";
            this.lblsecond.Size = new System.Drawing.Size(80, 13);
            this.lblsecond.TabIndex = 24;
            this.lblsecond.Text = "Optional Field 2";
            this.lblsecond.Visible = false;
            // 
            // lblthird
            // 
            this.lblthird.AutoSize = true;
            this.lblthird.Location = new System.Drawing.Point(17, 95);
            this.lblthird.Name = "lblthird";
            this.lblthird.Size = new System.Drawing.Size(80, 13);
            this.lblthird.TabIndex = 23;
            this.lblthird.Text = "Optional Field 3";
            this.lblthird.Visible = false;
            // 
            // lblfifth
            // 
            this.lblfifth.AutoSize = true;
            this.lblfifth.Location = new System.Drawing.Point(17, 173);
            this.lblfifth.Name = "lblfifth";
            this.lblfifth.Size = new System.Drawing.Size(80, 13);
            this.lblfifth.TabIndex = 22;
            this.lblfifth.Text = "Optional Field 5";
            this.lblfifth.Visible = false;
            // 
            // lblfirst
            // 
            this.lblfirst.AutoSize = true;
            this.lblfirst.Location = new System.Drawing.Point(17, 17);
            this.lblfirst.Name = "lblfirst";
            this.lblfirst.Size = new System.Drawing.Size(80, 13);
            this.lblfirst.TabIndex = 21;
            this.lblfirst.Text = "Optional Field 1";
            this.lblfirst.Visible = false;
            // 
            // btnCancel
            // 
            this.btnCancel.Image = global::Inventory.Properties.Resources.gnome_window_close;
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(714, 363);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 216;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Image = global::Inventory.Properties.Resources.document_edit;
            this.btnEdit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEdit.Location = new System.Drawing.Point(471, 362);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(75, 23);
            this.btnEdit.TabIndex = 213;
            this.btnEdit.Text = "&Edit";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Image = global::Inventory.Properties.Resources.document_delete;
            this.btnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDelete.Location = new System.Drawing.Point(633, 362);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 215;
            this.btnDelete.Text = "&Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnSave
            // 
            this.btnSave.Image = global::Inventory.Properties.Resources.save;
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSave.Location = new System.Drawing.Point(552, 363);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 214;
            this.btnSave.Text = "&Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnNew
            // 
            this.btnNew.Image = global::Inventory.Properties.Resources.edit_add;
            this.btnNew.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNew.Location = new System.Drawing.Point(390, 362);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(75, 23);
            this.btnNew.TabIndex = 212;
            this.btnNew.Text = "&New";
            this.btnNew.UseVisualStyleBackColor = true;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // txtDamageProductID
            // 
            this.txtDamageProductID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDamageProductID.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtDamageProductID.FocusLostColor = System.Drawing.Color.White;
            this.txtDamageProductID.Location = new System.Drawing.Point(659, 93);
            this.txtDamageProductID.Name = "txtDamageProductID";
            this.txtDamageProductID.Size = new System.Drawing.Size(100, 20);
            this.txtDamageProductID.TabIndex = 218;
            this.txtDamageProductID.Visible = false;
            // 
            // txtDate
            // 
            this.txtDate.BackColor = System.Drawing.Color.White;
            this.txtDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDate.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.txtDate.FocusLostColor = System.Drawing.Color.White;
            this.txtDate.Location = new System.Drawing.Point(635, 52);
            this.txtDate.Mask = "0000/00/00";
            this.txtDate.Name = "txtDate";
            this.txtDate.Size = new System.Drawing.Size(105, 20);
            this.txtDate.TabIndex = 217;
            // 
            // cboProjectName
            // 
            this.cboProjectName.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.cboProjectName.FocusLostColor = System.Drawing.Color.White;
            this.cboProjectName.FormattingEnabled = true;
            this.cboProjectName.Location = new System.Drawing.Point(389, 82);
            this.cboProjectName.Name = "cboProjectName";
            this.cboProjectName.Size = new System.Drawing.Size(156, 21);
            this.cboProjectName.TabIndex = 149;
            // 
            // cboDepot
            // 
            this.cboDepot.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.cboDepot.FocusLostColor = System.Drawing.Color.White;
            this.cboDepot.FormattingEnabled = true;
            this.cboDepot.Location = new System.Drawing.Point(107, 80);
            this.cboDepot.Name = "cboDepot";
            this.cboDepot.Size = new System.Drawing.Size(162, 21);
            this.cboDepot.TabIndex = 148;
            // 
            // cboSeriesName
            // 
            this.cboSeriesName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboSeriesName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboSeriesName.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.cboSeriesName.FocusLostColor = System.Drawing.Color.White;
            this.cboSeriesName.FormattingEnabled = true;
            this.cboSeriesName.Location = new System.Drawing.Point(107, 49);
            this.cboSeriesName.Name = "cboSeriesName";
            this.cboSeriesName.Size = new System.Drawing.Size(163, 21);
            this.cboSeriesName.TabIndex = 143;
            this.cboSeriesName.SelectedIndexChanged += new System.EventHandler(this.cboSeriesName_SelectedIndexChanged);
            // 
            // txtVoucherNo
            // 
            this.txtVoucherNo.BackColor = System.Drawing.Color.White;
            this.txtVoucherNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtVoucherNo.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtVoucherNo.FocusLostColor = System.Drawing.Color.White;
            this.txtVoucherNo.Location = new System.Drawing.Point(390, 52);
            this.txtVoucherNo.Name = "txtVoucherNo";
            this.txtVoucherNo.ReadOnly = true;
            this.txtVoucherNo.Size = new System.Drawing.Size(155, 20);
            this.txtVoucherNo.TabIndex = 145;
            // 
            // frmDamageItems
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(795, 391);
            this.Controls.Add(this.txtDamageProductID);
            this.Controls.Add(this.txtDate);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnNew);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.btnDate);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cboProjectName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cboDepot);
            this.Controls.Add(this.lblDepot);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.cboSeriesName);
            this.Controls.Add(this.txtVoucherNo);
            this.Controls.Add(this.lblVouNo);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmDamageItems";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Damage Items";
            this.Load += new System.EventHandler(this.frmDamageItems_Load);
            this.panel1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnPaste;
        private System.Windows.Forms.Button btnCopy;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnLast;
        private System.Windows.Forms.Button btnFirst;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnPrev;
        private System.Windows.Forms.Label label9;
        private SComboBox cboSeriesName;
        private STextBox txtVoucherNo;
        private System.Windows.Forms.Label lblVouNo;
        private SComboBox cboProjectName;
        private System.Windows.Forms.Label label2;
        private SComboBox cboDepot;
        private System.Windows.Forms.Label lblDepot;
        private System.Windows.Forms.Button btnDate;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private SourceGrid.Grid grddamage;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button btnAddAccClass;
        private STreeView treeAccClass;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnNew;
        private SMaskedTextBox txtDate;
        private STextBox txtDamageProductID;
        private System.Windows.Forms.Button btnPrint;
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