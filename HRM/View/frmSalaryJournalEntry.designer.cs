namespace HRM.View
{
    partial class frmSalaryJournalEntry
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSalaryJournalEntry));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.grdSalaryJournal = new SourceGrid.Grid();
            this.label1 = new System.Windows.Forms.Label();
            this.lblMonthYear = new System.Windows.Forms.Label();
            this.cmboBankAccount = new SComponents.SComboBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.lblVoucherNo = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnDelete = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.grdSalaryJournal);
            this.groupBox1.Location = new System.Drawing.Point(27, 83);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(475, 343);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // grdSalaryJournal
            // 
            this.grdSalaryJournal.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdSalaryJournal.EnableSort = true;
            this.grdSalaryJournal.Location = new System.Drawing.Point(6, 19);
            this.grdSalaryJournal.Name = "grdSalaryJournal";
            this.grdSalaryJournal.OptimizeMode = SourceGrid.CellOptimizeMode.ForRows;
            this.grdSalaryJournal.SelectionMode = SourceGrid.GridSelectionMode.Cell;
            this.grdSalaryJournal.Size = new System.Drawing.Size(463, 318);
            this.grdSalaryJournal.TabIndex = 1;
            this.grdSalaryJournal.TabStop = true;
            this.grdSalaryJournal.ToolTipText = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(188, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(140, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Journal Voucher";
            // 
            // lblMonthYear
            // 
            this.lblMonthYear.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMonthYear.Location = new System.Drawing.Point(157, 39);
            this.lblMonthYear.Name = "lblMonthYear";
            this.lblMonthYear.Size = new System.Drawing.Size(200, 13);
            this.lblMonthYear.TabIndex = 2;
            this.lblMonthYear.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // cmboBankAccount
            // 
            this.cmboBankAccount.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.cmboBankAccount.FocusLostColor = System.Drawing.Color.White;
            this.cmboBankAccount.FormattingEnabled = true;
            this.cmboBankAccount.Location = new System.Drawing.Point(363, 58);
            this.cmboBankAccount.Name = "cmboBankAccount";
            this.cmboBankAccount.Size = new System.Drawing.Size(139, 21);
            this.cmboBankAccount.TabIndex = 143;
            this.cmboBankAccount.SelectedIndexChanged += new System.EventHandler(this.cmboBankAccount_SelectedIndexChanged);
            // 
            // btnSave
            // 
            this.btnSave.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.btnSave.Location = new System.Drawing.Point(338, 432);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 144;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(319, 61);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Bank :";
            // 
            // lblVoucherNo
            // 
            this.lblVoucherNo.AutoSize = true;
            this.lblVoucherNo.Location = new System.Drawing.Point(100, 61);
            this.lblVoucherNo.Name = "lblVoucherNo";
            this.lblVoucherNo.Size = new System.Drawing.Size(0, 13);
            this.lblVoucherNo.TabIndex = 145;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(24, 61);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 13);
            this.label3.TabIndex = 146;
            this.label3.Text = "Voucher No.:";
            // 
            // btnDelete
            // 
            this.btnDelete.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.btnDelete.Location = new System.Drawing.Point(419, 432);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 147;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // frmSalaryJournalEntry
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(535, 491);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblVoucherNo);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.cmboBankAccount);
            this.Controls.Add(this.lblMonthYear);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmSalaryJournalEntry";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.frmSalaryJournalEntry_Load);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblMonthYear;
        private SourceGrid.Grid grdSalaryJournal;
        private SComponents.SComboBox cmboBankAccount;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblVoucherNo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnDelete;
    }
}