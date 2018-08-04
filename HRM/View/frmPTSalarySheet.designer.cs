namespace HRM.View
{
    partial class frmPTSalarySheet
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPTSalarySheet));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tbSalarySheet = new System.Windows.Forms.TabPage();
            this.label6 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblNetAmount = new System.Windows.Forms.Label();
            this.lblTax = new System.Windows.Forms.Label();
            this.lblAmount = new System.Windows.Forms.Label();
            this.lblRate = new System.Windows.Forms.Label();
            this.lblQty = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.grdSalarySheet = new SourceGrid.Grid();
            this.txtDate = new SComponents.SMaskedTextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtRemarks = new SComponents.STextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnJournalEntry = new System.Windows.Forms.Button();
            this.chkbycash = new System.Windows.Forms.CheckBox();
            this.lblCashParty = new System.Windows.Forms.Label();
            this.cmbbankname = new SComponents.SComboBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.lblSerialNo = new System.Windows.Forms.Label();
            this.lblSerialHeader = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtGoto = new System.Windows.Forms.TextBox();
            this.btnPrintPreview = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnGoto = new System.Windows.Forms.Button();
            this.btnLast = new System.Windows.Forms.Button();
            this.btnFirst = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnPrev = new System.Windows.Forms.Button();
            this.btnNew = new System.Windows.Forms.Button();
            this.lblMsg = new System.Windows.Forms.Label();
            this.btnDate = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tbSalarySheet.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tbSalarySheet);
            this.tabControl1.Location = new System.Drawing.Point(14, 84);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1058, 404);
            this.tabControl1.TabIndex = 0;
            // 
            // tbSalarySheet
            // 
            this.tbSalarySheet.Controls.Add(this.label6);
            this.tbSalarySheet.Controls.Add(this.label3);
            this.tbSalarySheet.Controls.Add(this.label2);
            this.tbSalarySheet.Controls.Add(this.lblNetAmount);
            this.tbSalarySheet.Controls.Add(this.lblTax);
            this.tbSalarySheet.Controls.Add(this.lblAmount);
            this.tbSalarySheet.Controls.Add(this.lblRate);
            this.tbSalarySheet.Controls.Add(this.lblQty);
            this.tbSalarySheet.Controls.Add(this.label1);
            this.tbSalarySheet.Controls.Add(this.grdSalarySheet);
            this.tbSalarySheet.Location = new System.Drawing.Point(4, 22);
            this.tbSalarySheet.Name = "tbSalarySheet";
            this.tbSalarySheet.Padding = new System.Windows.Forms.Padding(3);
            this.tbSalarySheet.Size = new System.Drawing.Size(1050, 378);
            this.tbSalarySheet.TabIndex = 0;
            this.tbSalarySheet.Text = "Salary Sheet";
            this.tbSalarySheet.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(682, 350);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(57, 13);
            this.label6.TabIndex = 222;
            this.label6.Text = "Amount :";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(797, 349);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 13);
            this.label3.TabIndex = 221;
            this.label3.Text = "Tax :";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(889, 349);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(81, 13);
            this.label2.TabIndex = 220;
            this.label2.Text = "Net Amount :";
            // 
            // lblNetAmount
            // 
            this.lblNetAmount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblNetAmount.AutoSize = true;
            this.lblNetAmount.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNetAmount.Location = new System.Drawing.Point(971, 349);
            this.lblNetAmount.Name = "lblNetAmount";
            this.lblNetAmount.Size = new System.Drawing.Size(14, 13);
            this.lblNetAmount.TabIndex = 219;
            this.lblNetAmount.Text = "0";
            // 
            // lblTax
            // 
            this.lblTax.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTax.AutoSize = true;
            this.lblTax.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTax.Location = new System.Drawing.Point(831, 349);
            this.lblTax.Name = "lblTax";
            this.lblTax.Size = new System.Drawing.Size(14, 13);
            this.lblTax.TabIndex = 218;
            this.lblTax.Text = "0";
            // 
            // lblAmount
            // 
            this.lblAmount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblAmount.AutoSize = true;
            this.lblAmount.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAmount.Location = new System.Drawing.Point(737, 350);
            this.lblAmount.Name = "lblAmount";
            this.lblAmount.Size = new System.Drawing.Size(14, 13);
            this.lblAmount.TabIndex = 217;
            this.lblAmount.Text = "0";
            // 
            // lblRate
            // 
            this.lblRate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblRate.AutoSize = true;
            this.lblRate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRate.Location = new System.Drawing.Point(576, 350);
            this.lblRate.Name = "lblRate";
            this.lblRate.Size = new System.Drawing.Size(58, 13);
            this.lblRate.TabIndex = 216;
            this.lblRate.Text = "Classes :";
            // 
            // lblQty
            // 
            this.lblQty.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblQty.AutoSize = true;
            this.lblQty.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblQty.Location = new System.Drawing.Point(634, 350);
            this.lblQty.Name = "lblQty";
            this.lblQty.Size = new System.Drawing.Size(14, 13);
            this.lblQty.TabIndex = 215;
            this.lblQty.Text = "0";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(108, 350);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 13);
            this.label1.TabIndex = 214;
            this.label1.Text = "Total :";
            // 
            // grdSalarySheet
            // 
            this.grdSalarySheet.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdSalarySheet.EnableSort = true;
            this.grdSalarySheet.Location = new System.Drawing.Point(7, 10);
            this.grdSalarySheet.Name = "grdSalarySheet";
            this.grdSalarySheet.OptimizeMode = SourceGrid.CellOptimizeMode.ForRows;
            this.grdSalarySheet.SelectionMode = SourceGrid.GridSelectionMode.Cell;
            this.grdSalarySheet.Size = new System.Drawing.Size(1036, 333);
            this.grdSalarySheet.TabIndex = 1;
            this.grdSalarySheet.TabStop = true;
            this.grdSalarySheet.ToolTipText = "";
            this.grdSalarySheet.Paint += new System.Windows.Forms.PaintEventHandler(this.grdSalarySheet_Paint);
            // 
            // txtDate
            // 
            this.txtDate.BackColor = System.Drawing.Color.FloralWhite;
            this.txtDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDate.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.txtDate.FocusLostColor = System.Drawing.Color.White;
            this.txtDate.Location = new System.Drawing.Point(60, 57);
            this.txtDate.Name = "txtDate";
            this.txtDate.Size = new System.Drawing.Size(158, 20);
            this.txtDate.TabIndex = 209;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 60);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(33, 13);
            this.label4.TabIndex = 208;
            this.label4.Text = "Date:";
            // 
            // txtRemarks
            // 
            this.txtRemarks.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtRemarks.BackColor = System.Drawing.Color.FloralWhite;
            this.txtRemarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtRemarks.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtRemarks.FocusLostColor = System.Drawing.Color.White;
            this.txtRemarks.Location = new System.Drawing.Point(90, 498);
            this.txtRemarks.Name = "txtRemarks";
            this.txtRemarks.Size = new System.Drawing.Size(809, 20);
            this.txtRemarks.TabIndex = 216;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(22, 501);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 13);
            this.label5.TabIndex = 215;
            this.label5.Text = "Narration:";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.btnJournalEntry);
            this.groupBox2.Controls.Add(this.chkbycash);
            this.groupBox2.Controls.Add(this.lblCashParty);
            this.groupBox2.Controls.Add(this.cmbbankname);
            this.groupBox2.Location = new System.Drawing.Point(671, 42);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(393, 45);
            this.groupBox2.TabIndex = 217;
            this.groupBox2.TabStop = false;
            // 
            // btnJournalEntry
            // 
            this.btnJournalEntry.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnJournalEntry.Location = new System.Drawing.Point(301, 13);
            this.btnJournalEntry.Name = "btnJournalEntry";
            this.btnJournalEntry.Size = new System.Drawing.Size(86, 23);
            this.btnJournalEntry.TabIndex = 86;
            this.btnJournalEntry.Text = "Journal Entry";
            this.btnJournalEntry.UseVisualStyleBackColor = true;
            this.btnJournalEntry.Click += new System.EventHandler(this.btnJournalEntry_Click);
            // 
            // chkbycash
            // 
            this.chkbycash.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chkbycash.AutoSize = true;
            this.chkbycash.Location = new System.Drawing.Point(220, 17);
            this.chkbycash.Name = "chkbycash";
            this.chkbycash.Size = new System.Drawing.Size(65, 17);
            this.chkbycash.TabIndex = 89;
            this.chkbycash.Text = "By Cash";
            this.chkbycash.UseVisualStyleBackColor = true;
            this.chkbycash.CheckedChanged += new System.EventHandler(this.chkbycash_CheckedChanged);
            // 
            // lblCashParty
            // 
            this.lblCashParty.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCashParty.AutoSize = true;
            this.lblCashParty.Location = new System.Drawing.Point(9, 19);
            this.lblCashParty.Name = "lblCashParty";
            this.lblCashParty.Size = new System.Drawing.Size(66, 13);
            this.lblCashParty.TabIndex = 87;
            this.lblCashParty.Text = "Bank Name:";
            // 
            // cmbbankname
            // 
            this.cmbbankname.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbbankname.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbbankname.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.cmbbankname.FocusLostColor = System.Drawing.Color.White;
            this.cmbbankname.FormattingEnabled = true;
            this.cmbbankname.Location = new System.Drawing.Point(81, 16);
            this.cmbbankname.Name = "cmbbankname";
            this.cmbbankname.Size = new System.Drawing.Size(133, 21);
            this.cmbbankname.TabIndex = 88;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(993, 497);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 218;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // lblSerialNo
            // 
            this.lblSerialNo.AutoSize = true;
            this.lblSerialNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSerialNo.Location = new System.Drawing.Point(340, 61);
            this.lblSerialNo.Name = "lblSerialNo";
            this.lblSerialNo.Size = new System.Drawing.Size(14, 13);
            this.lblSerialNo.TabIndex = 222;
            this.lblSerialNo.Text = "0";
            // 
            // lblSerialHeader
            // 
            this.lblSerialHeader.AutoSize = true;
            this.lblSerialHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSerialHeader.Location = new System.Drawing.Point(271, 61);
            this.lblSerialHeader.Name = "lblSerialHeader";
            this.lblSerialHeader.Size = new System.Drawing.Size(71, 13);
            this.lblSerialHeader.TabIndex = 221;
            this.lblSerialHeader.Text = "Serial No. :";
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.CadetBlue;
            this.panel1.Controls.Add(this.txtGoto);
            this.panel1.Controls.Add(this.btnPrintPreview);
            this.panel1.Controls.Add(this.btnExport);
            this.panel1.Controls.Add(this.btnGoto);
            this.panel1.Controls.Add(this.btnLast);
            this.panel1.Controls.Add(this.btnFirst);
            this.panel1.Controls.Add(this.btnPrint);
            this.panel1.Controls.Add(this.btnNext);
            this.panel1.Controls.Add(this.btnPrev);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1083, 41);
            this.panel1.TabIndex = 223;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // txtGoto
            // 
            this.txtGoto.Location = new System.Drawing.Point(269, 12);
            this.txtGoto.Name = "txtGoto";
            this.txtGoto.Size = new System.Drawing.Size(46, 20);
            this.txtGoto.TabIndex = 30;
            this.txtGoto.TextChanged += new System.EventHandler(this.txtGoto_TextChanged);
            this.txtGoto.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtGoto_KeyDown);
            // 
            // btnPrintPreview
            // 
            this.btnPrintPreview.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPrintPreview.Location = new System.Drawing.Point(436, 10);
            this.btnPrintPreview.Name = "btnPrintPreview";
            this.btnPrintPreview.Size = new System.Drawing.Size(94, 23);
            this.btnPrintPreview.TabIndex = 29;
            this.btnPrintPreview.Text = "Pr&int Preview";
            this.btnPrintPreview.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnPrintPreview.UseVisualStyleBackColor = true;
            this.btnPrintPreview.Click += new System.EventHandler(this.btnPrintPreview_Click);
            // 
            // btnExport
            // 
            this.btnExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExport.Enabled = false;
            this.btnExport.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnExport.Location = new System.Drawing.Point(1009, 10);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(62, 23);
            this.btnExport.TabIndex = 27;
            this.btnExport.Text = "&Export";
            this.btnExport.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Visible = false;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnGoto
            // 
            this.btnGoto.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnGoto.Location = new System.Drawing.Point(316, 10);
            this.btnGoto.Name = "btnGoto";
            this.btnGoto.Size = new System.Drawing.Size(51, 23);
            this.btnGoto.TabIndex = 23;
            this.btnGoto.Text = "Go To";
            this.btnGoto.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnGoto.UseVisualStyleBackColor = true;
            this.btnGoto.Click += new System.EventHandler(this.btnGoto_Click);
            // 
            // btnLast
            // 
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
            // btnPrint
            // 
            this.btnPrint.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPrint.Location = new System.Drawing.Point(372, 10);
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
            // btnNew
            // 
            this.btnNew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNew.Location = new System.Drawing.Point(912, 497);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(75, 23);
            this.btnNew.TabIndex = 224;
            this.btnNew.Text = "New";
            this.btnNew.UseVisualStyleBackColor = true;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // lblMsg
            // 
            this.lblMsg.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblMsg.AutoSize = true;
            this.lblMsg.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMsg.Location = new System.Drawing.Point(22, 472);
            this.lblMsg.Name = "lblMsg";
            this.lblMsg.Size = new System.Drawing.Size(0, 16);
            this.lblMsg.TabIndex = 225;
            // 
            // btnDate
            // 
            this.btnDate.BackColor = System.Drawing.Color.FloralWhite;
            this.btnDate.Location = new System.Drawing.Point(224, 56);
            this.btnDate.Name = "btnDate";
            this.btnDate.Size = new System.Drawing.Size(26, 23);
            this.btnDate.TabIndex = 210;
            this.btnDate.UseVisualStyleBackColor = false;
            this.btnDate.Click += new System.EventHandler(this.btnDate_Click);
            // 
            // frmPTSalarySheet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1083, 531);
            this.Controls.Add(this.lblMsg);
            this.Controls.Add(this.btnNew);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lblSerialNo);
            this.Controls.Add(this.lblSerialHeader);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.txtRemarks);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btnDate);
            this.Controls.Add(this.txtDate);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tabControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmPTSalarySheet";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Part Time Salary Sheet";
            this.Load += new System.EventHandler(this.frmPTSalarySheet_Load);
            this.tabControl1.ResumeLayout(false);
            this.tbSalarySheet.ResumeLayout(false);
            this.tbSalarySheet.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tbSalarySheet;
        private SourceGrid.Grid grdSalarySheet;
        private System.Windows.Forms.Button btnDate;
        private SComponents.SMaskedTextBox txtDate;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblNetAmount;
        private System.Windows.Forms.Label lblTax;
        private System.Windows.Forms.Label lblAmount;
        private System.Windows.Forms.Label lblRate;
        private System.Windows.Forms.Label lblQty;
        private System.Windows.Forms.Label label1;
        private SComponents.STextBox txtRemarks;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnJournalEntry;
        private System.Windows.Forms.CheckBox chkbycash;
        private System.Windows.Forms.Label lblCashParty;
        private SComponents.SComboBox cmbbankname;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label lblSerialNo;
        private System.Windows.Forms.Label lblSerialHeader;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnPrintPreview;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnGoto;
        private System.Windows.Forms.Button btnLast;
        private System.Windows.Forms.Button btnFirst;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnPrev;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblMsg;
        private System.Windows.Forms.TextBox txtGoto;
    }
}