namespace AccSwift
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
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnPaste = new System.Windows.Forms.Button();
            this.btnCopy = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnLast = new System.Windows.Forms.Button();
            this.btnFirst = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnPrev = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.chkDoNotClose = new System.Windows.Forms.CheckBox();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnNew = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.grdCashReceipt = new SourceGrid.Grid();
            this.lblDifferenceAmount = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblDebitTotal = new System.Windows.Forms.Label();
            this.lblCreditTotal = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.cmboCashAccount = new AccSwift.SComboBox();
            this.txtCashReceiptID = new AccSwift.STextBox();
            this.txtDate = new AccSwift.SMaskedTextBox();
            this.txtRemarks = new AccSwift.STextBox();
            this.txtVchNo = new AccSwift.STextBox();
            this.panel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(238, 419);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(108, 17);
            this.checkBox2.TabIndex = 39;
            this.checkBox2.Text = "Print while saving";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.CadetBlue;
            this.panel1.Controls.Add(this.btnPaste);
            this.panel1.Controls.Add(this.btnCopy);
            this.panel1.Controls.Add(this.btnExit);
            this.panel1.Controls.Add(this.btnLast);
            this.panel1.Controls.Add(this.btnFirst);
            this.panel1.Controls.Add(this.button3);
            this.panel1.Controls.Add(this.btnNext);
            this.panel1.Controls.Add(this.btnPrev);
            this.panel1.Location = new System.Drawing.Point(-1, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(870, 41);
            this.panel1.TabIndex = 37;
            // 
            // btnPaste
            // 
            this.btnPaste.Image = global::AccSwift.Properties.Resources.print;
            this.btnPaste.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPaste.Location = new System.Drawing.Point(333, 10);
            this.btnPaste.Name = "btnPaste";
            this.btnPaste.Size = new System.Drawing.Size(58, 23);
            this.btnPaste.TabIndex = 24;
            this.btnPaste.Text = "Paste";
            this.btnPaste.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnPaste.UseVisualStyleBackColor = true;
            // 
            // btnCopy
            // 
            this.btnCopy.Image = global::AccSwift.Properties.Resources.print;
            this.btnCopy.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCopy.Location = new System.Drawing.Point(269, 10);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(58, 23);
            this.btnCopy.TabIndex = 23;
            this.btnCopy.Text = "Copy";
            this.btnCopy.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCopy.UseVisualStyleBackColor = true;
            // 
            // btnExit
            // 
            this.btnExit.Image = global::AccSwift.Properties.Resources.left1_2_;
            this.btnExit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnExit.Location = new System.Drawing.Point(461, 10);
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
            this.btnLast.Image = global::AccSwift.Properties.Resources._1299891326_control_stop_000_small;
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
            this.btnFirst.Image = global::AccSwift.Properties.Resources.left1_2_;
            this.btnFirst.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnFirst.Location = new System.Drawing.Point(12, 10);
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
            this.button3.Image = global::AccSwift.Properties.Resources.print;
            this.button3.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button3.Location = new System.Drawing.Point(397, 10);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(58, 23);
            this.button3.TabIndex = 2;
            this.button3.Text = "Print";
            this.button3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button3.UseVisualStyleBackColor = true;
            // 
            // btnNext
            // 
            this.btnNext.Image = global::AccSwift.Properties.Resources.right1_2_;
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
            this.btnPrev.Image = global::AccSwift.Properties.Resources.left1_2_;
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
            // btnCancel
            // 
            this.btnCancel.Image = global::AccSwift.Properties.Resources.gnome_window_close;
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(738, 415);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 36;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // chkDoNotClose
            // 
            this.chkDoNotClose.AutoSize = true;
            this.chkDoNotClose.Location = new System.Drawing.Point(48, 420);
            this.chkDoNotClose.Name = "chkDoNotClose";
            this.chkDoNotClose.Size = new System.Drawing.Size(86, 17);
            this.chkDoNotClose.TabIndex = 38;
            this.chkDoNotClose.Text = "Do not close";
            this.chkDoNotClose.UseVisualStyleBackColor = true;
            // 
            // btnEdit
            // 
            this.btnEdit.Image = global::AccSwift.Properties.Resources.document_edit;
            this.btnEdit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEdit.Location = new System.Drawing.Point(495, 414);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(75, 23);
            this.btnEdit.TabIndex = 35;
            this.btnEdit.Text = "&Edit";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Image = global::AccSwift.Properties.Resources.document_delete;
            this.btnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDelete.Location = new System.Drawing.Point(657, 413);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 34;
            this.btnDelete.Text = "&Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            // 
            // btnNew
            // 
            this.btnNew.Image = global::AccSwift.Properties.Resources.edit_add;
            this.btnNew.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNew.Location = new System.Drawing.Point(414, 413);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(75, 23);
            this.btnNew.TabIndex = 32;
            this.btnNew.Text = "&New";
            this.btnNew.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(313, 54);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(33, 13);
            this.label3.TabIndex = 30;
            this.label3.Text = "Date:";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(15, 129);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(798, 279);
            this.tabControl1.TabIndex = 25;
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
            this.tabPage1.Size = new System.Drawing.Size(790, 253);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Details";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // grdCashReceipt
            // 
            this.grdCashReceipt.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grdCashReceipt.Location = new System.Drawing.Point(3, 3);
            this.grdCashReceipt.Name = "grdCashReceipt";
            this.grdCashReceipt.OptimizeMode = SourceGrid.CellOptimizeMode.ForRows;
            this.grdCashReceipt.SelectionMode = SourceGrid.GridSelectionMode.Cell;
            this.grdCashReceipt.Size = new System.Drawing.Size(787, 222);
            this.grdCashReceipt.TabIndex = 1;
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
            this.label5.TabIndex = 10;
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
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(790, 253);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Classes";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(790, 253);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Optional Fields";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 104);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 13);
            this.label2.TabIndex = 29;
            this.label2.Text = "Remarks:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 54);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 13);
            this.label1.TabIndex = 27;
            this.label1.Text = "Voucher No.:";
            // 
            // btnSave
            // 
            this.btnSave.Image = global::AccSwift.Properties.Resources.save;
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSave.Location = new System.Drawing.Point(576, 415);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 33;
            this.btnSave.Text = "&Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(19, 80);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(77, 13);
            this.label6.TabIndex = 41;
            this.label6.Text = "Cash Account:";
            // 
            // cmboCashAccount
            // 
            this.cmboCashAccount.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmboCashAccount.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.cmboCashAccount.FocusLostColor = System.Drawing.Color.White;
            this.cmboCashAccount.FormattingEnabled = true;
            this.cmboCashAccount.Location = new System.Drawing.Point(99, 72);
            this.cmboCashAccount.Name = "cmboCashAccount";
            this.cmboCashAccount.Size = new System.Drawing.Size(158, 21);
            this.cmboCashAccount.TabIndex = 42;
            // 
            // txtCashReceiptID
            // 
            this.txtCashReceiptID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCashReceiptID.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtCashReceiptID.FocusLostColor = System.Drawing.Color.White;
            this.txtCashReceiptID.Location = new System.Drawing.Point(460, 97);
            this.txtCashReceiptID.Name = "txtCashReceiptID";
            this.txtCashReceiptID.Size = new System.Drawing.Size(88, 20);
            this.txtCashReceiptID.TabIndex = 40;
            this.txtCashReceiptID.Visible = false;
            // 
            // txtDate
            // 
            this.txtDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDate.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.txtDate.FocusLostColor = System.Drawing.Color.White;
            this.txtDate.Location = new System.Drawing.Point(356, 47);
            this.txtDate.Mask = "0000/00/00";
            this.txtDate.Name = "txtDate";
            this.txtDate.Size = new System.Drawing.Size(74, 20);
            this.txtDate.TabIndex = 31;
            // 
            // txtRemarks
            // 
            this.txtRemarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtRemarks.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtRemarks.FocusLostColor = System.Drawing.Color.White;
            this.txtRemarks.Location = new System.Drawing.Point(99, 100);
            this.txtRemarks.Name = "txtRemarks";
            this.txtRemarks.Size = new System.Drawing.Size(331, 20);
            this.txtRemarks.TabIndex = 28;
            // 
            // txtVchNo
            // 
            this.txtVchNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtVchNo.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtVchNo.FocusLostColor = System.Drawing.Color.White;
            this.txtVchNo.Location = new System.Drawing.Point(99, 47);
            this.txtVchNo.Name = "txtVchNo";
            this.txtVchNo.Size = new System.Drawing.Size(158, 20);
            this.txtVchNo.TabIndex = 26;
            // 
            // frmCashReceipt
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(867, 483);
            this.Controls.Add(this.cmboCashAccount);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtCashReceiptID);
            this.Controls.Add(this.checkBox2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.chkDoNotClose);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnNew);
            this.Controls.Add(this.txtDate);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtRemarks);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.txtVchNo);
            this.Name = "frmCashReceipt";
            this.Text = "Cash Receipt";
            this.Load += new System.EventHandler(this.frmCashReceipt_Load);
            this.panel1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private STextBox txtCashReceiptID;
        private System.Windows.Forms.CheckBox checkBox2;
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
        private System.Windows.Forms.CheckBox chkDoNotClose;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnNew;
        private SMaskedTextBox txtDate;
        private System.Windows.Forms.Label label3;
        private STextBox txtRemarks;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private SourceGrid.Grid grdCashReceipt;
        private System.Windows.Forms.Label lblDifferenceAmount;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblDebitTotal;
        private System.Windows.Forms.Label lblCreditTotal;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSave;
        private STextBox txtVchNo;
        private System.Windows.Forms.Label label6;
        private SComboBox cmboCashAccount;

    }
}