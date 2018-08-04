using SComponents;
namespace Inventory
{
    partial class frmChequeSummarySettings
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
            this.rdbChequeGiven = new System.Windows.Forms.RadioButton();
            this.rdbChequeReceived = new System.Windows.Forms.RadioButton();
            this.pnlStatus = new System.Windows.Forms.Panel();
            this.rdbUnCleared = new System.Windows.Forms.RadioButton();
            this.rdbCleared = new System.Windows.Forms.RadioButton();
            this.rdbBoth = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.chkBank = new System.Windows.Forms.CheckBox();
            this.chkParty = new System.Windows.Forms.CheckBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnShow = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rdbChequeAll = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chkDateRange = new System.Windows.Forms.CheckBox();
            this.btnToDate = new System.Windows.Forms.Button();
            this.txtFromDate = new System.Windows.Forms.MaskedTextBox();
            this.btnFromDate = new System.Windows.Forms.Button();
            this.txtToDate = new System.Windows.Forms.MaskedTextBox();
            this.cboParty = new SComboBox();
            this.cboBanks = new SComboBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.cboProjectName = new SComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnsavestate = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.btnSelectAccClass = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.cboMonths = new SComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.pnlStatus.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // rdbChequeGiven
            // 
            this.rdbChequeGiven.AutoSize = true;
            this.rdbChequeGiven.Location = new System.Drawing.Point(141, 21);
            this.rdbChequeGiven.Name = "rdbChequeGiven";
            this.rdbChequeGiven.Size = new System.Drawing.Size(53, 17);
            this.rdbChequeGiven.TabIndex = 0;
            this.rdbChequeGiven.Text = "Given";
            this.rdbChequeGiven.UseVisualStyleBackColor = true;
            // 
            // rdbChequeReceived
            // 
            this.rdbChequeReceived.AutoSize = true;
            this.rdbChequeReceived.Location = new System.Drawing.Point(216, 23);
            this.rdbChequeReceived.Name = "rdbChequeReceived";
            this.rdbChequeReceived.Size = new System.Drawing.Size(71, 17);
            this.rdbChequeReceived.TabIndex = 1;
            this.rdbChequeReceived.Text = "Received";
            this.rdbChequeReceived.UseVisualStyleBackColor = true;
            // 
            // pnlStatus
            // 
            this.pnlStatus.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pnlStatus.Controls.Add(this.rdbUnCleared);
            this.pnlStatus.Controls.Add(this.rdbCleared);
            this.pnlStatus.Controls.Add(this.rdbBoth);
            this.pnlStatus.Location = new System.Drawing.Point(4, 50);
            this.pnlStatus.Name = "pnlStatus";
            this.pnlStatus.Size = new System.Drawing.Size(114, 103);
            this.pnlStatus.TabIndex = 2;
            // 
            // rdbUnCleared
            // 
            this.rdbUnCleared.AutoSize = true;
            this.rdbUnCleared.Location = new System.Drawing.Point(13, 51);
            this.rdbUnCleared.Name = "rdbUnCleared";
            this.rdbUnCleared.Size = new System.Drawing.Size(74, 17);
            this.rdbUnCleared.TabIndex = 4;
            this.rdbUnCleared.Text = "Uncleared";
            this.rdbUnCleared.UseVisualStyleBackColor = true;
            // 
            // rdbCleared
            // 
            this.rdbCleared.AutoSize = true;
            this.rdbCleared.Location = new System.Drawing.Point(13, 31);
            this.rdbCleared.Name = "rdbCleared";
            this.rdbCleared.Size = new System.Drawing.Size(61, 17);
            this.rdbCleared.TabIndex = 3;
            this.rdbCleared.Text = "Cleared";
            this.rdbCleared.UseVisualStyleBackColor = true;
            // 
            // rdbBoth
            // 
            this.rdbBoth.AutoSize = true;
            this.rdbBoth.Checked = true;
            this.rdbBoth.Location = new System.Drawing.Point(13, 8);
            this.rdbBoth.Name = "rdbBoth";
            this.rdbBoth.Size = new System.Drawing.Size(47, 17);
            this.rdbBoth.TabIndex = 2;
            this.rdbBoth.TabStop = true;
            this.rdbBoth.Text = "Both";
            this.rdbBoth.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(30, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "From";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(23, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "To ";
            // 
            // chkBank
            // 
            this.chkBank.AutoSize = true;
            this.chkBank.Location = new System.Drawing.Point(36, 19);
            this.chkBank.Name = "chkBank";
            this.chkBank.Size = new System.Drawing.Size(51, 17);
            this.chkBank.TabIndex = 5;
            this.chkBank.Text = "Bank";
            this.chkBank.UseVisualStyleBackColor = true;
            this.chkBank.CheckedChanged += new System.EventHandler(this.chkBank_CheckedChanged);
            // 
            // chkParty
            // 
            this.chkParty.AutoSize = true;
            this.chkParty.Location = new System.Drawing.Point(36, 48);
            this.chkParty.Name = "chkParty";
            this.chkParty.Size = new System.Drawing.Size(50, 17);
            this.chkParty.TabIndex = 6;
            this.chkParty.Text = "Party";
            this.chkParty.UseVisualStyleBackColor = true;
            this.chkParty.CheckedChanged += new System.EventHandler(this.chkParty_CheckedChanged);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(8, 46);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 26);
            this.btnCancel.TabIndex = 146;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnShow
            // 
            this.btnShow.Location = new System.Drawing.Point(9, 12);
            this.btnShow.Name = "btnShow";
            this.btnShow.Size = new System.Drawing.Size(80, 26);
            this.btnShow.TabIndex = 145;
            this.btnShow.Text = "&Show";
            this.btnShow.UseVisualStyleBackColor = true;
            this.btnShow.Click += new System.EventHandler(this.btnShow_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox1.Controls.Add(this.rdbChequeAll);
            this.groupBox1.Controls.Add(this.rdbChequeReceived);
            this.groupBox1.Controls.Add(this.rdbChequeGiven);
            this.groupBox1.Location = new System.Drawing.Point(3, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(303, 48);
            this.groupBox1.TabIndex = 147;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Cheque";
            // 
            // rdbChequeAll
            // 
            this.rdbChequeAll.AutoSize = true;
            this.rdbChequeAll.Checked = true;
            this.rdbChequeAll.Location = new System.Drawing.Point(61, 23);
            this.rdbChequeAll.Name = "rdbChequeAll";
            this.rdbChequeAll.Size = new System.Drawing.Size(36, 17);
            this.rdbChequeAll.TabIndex = 2;
            this.rdbChequeAll.TabStop = true;
            this.rdbChequeAll.Text = "All";
            this.rdbChequeAll.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox2.Controls.Add(this.cboMonths);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.chkDateRange);
            this.groupBox2.Controls.Add(this.btnToDate);
            this.groupBox2.Controls.Add(this.txtFromDate);
            this.groupBox2.Controls.Add(this.btnFromDate);
            this.groupBox2.Controls.Add(this.txtToDate);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Location = new System.Drawing.Point(121, 51);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(185, 102);
            this.groupBox2.TabIndex = 148;
            this.groupBox2.TabStop = false;
            // 
            // chkDateRange
            // 
            this.chkDateRange.AutoSize = true;
            this.chkDateRange.Location = new System.Drawing.Point(4, -1);
            this.chkDateRange.Name = "chkDateRange";
            this.chkDateRange.Size = new System.Drawing.Size(108, 17);
            this.chkDateRange.TabIndex = 149;
            this.chkDateRange.Text = "Transaction Date";
            this.chkDateRange.UseVisualStyleBackColor = true;
            this.chkDateRange.CheckedChanged += new System.EventHandler(this.chkDateRange_CheckedChanged);
            // 
            // btnToDate
            // 
            this.btnToDate.Image = global::Inventory.Properties.Resources.dateIcon;
            this.btnToDate.Location = new System.Drawing.Point(143, 50);
            this.btnToDate.Name = "btnToDate";
            this.btnToDate.Size = new System.Drawing.Size(26, 23);
            this.btnToDate.TabIndex = 64;
            this.btnToDate.UseVisualStyleBackColor = true;
            this.btnToDate.Click += new System.EventHandler(this.btnToDate_Click);
            // 
            // txtFromDate
            // 
            this.txtFromDate.Location = new System.Drawing.Point(51, 24);
            this.txtFromDate.Mask = "##/##/####";
            this.txtFromDate.Name = "txtFromDate";
            this.txtFromDate.Size = new System.Drawing.Size(86, 20);
            this.txtFromDate.TabIndex = 63;
            // 
            // btnFromDate
            // 
            this.btnFromDate.Image = global::Inventory.Properties.Resources.dateIcon;
            this.btnFromDate.Location = new System.Drawing.Point(143, 21);
            this.btnFromDate.Name = "btnFromDate";
            this.btnFromDate.Size = new System.Drawing.Size(26, 23);
            this.btnFromDate.TabIndex = 62;
            this.btnFromDate.UseVisualStyleBackColor = true;
            this.btnFromDate.Click += new System.EventHandler(this.btnFromDate_Click);
            // 
            // txtToDate
            // 
            this.txtToDate.Location = new System.Drawing.Point(51, 49);
            this.txtToDate.Mask = "##/##/####";
            this.txtToDate.Name = "txtToDate";
            this.txtToDate.Size = new System.Drawing.Size(86, 20);
            this.txtToDate.TabIndex = 61;
            // 
            // cboParty
            // 
            this.cboParty.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboParty.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.cboParty.FocusLostColor = System.Drawing.Color.White;
            this.cboParty.FormattingEnabled = true;
            this.cboParty.Location = new System.Drawing.Point(141, 48);
            this.cboParty.Name = "cboParty";
            this.cboParty.Size = new System.Drawing.Size(147, 21);
            this.cboParty.TabIndex = 144;
            // 
            // cboBanks
            // 
            this.cboBanks.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboBanks.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.cboBanks.FocusLostColor = System.Drawing.Color.White;
            this.cboBanks.FormattingEnabled = true;
            this.cboBanks.Location = new System.Drawing.Point(140, 19);
            this.cboBanks.Name = "cboBanks";
            this.cboBanks.Size = new System.Drawing.Size(147, 21);
            this.cboBanks.TabIndex = 143;
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox3.Controls.Add(this.cboProjectName);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Location = new System.Drawing.Point(4, 232);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(302, 50);
            this.groupBox3.TabIndex = 149;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Project";
            // 
            // cboProjectName
            // 
            this.cboProjectName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboProjectName.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.cboProjectName.FocusLostColor = System.Drawing.Color.White;
            this.cboProjectName.FormattingEnabled = true;
            this.cboProjectName.Location = new System.Drawing.Point(139, 22);
            this.cboProjectName.Name = "cboProjectName";
            this.cboProjectName.Size = new System.Drawing.Size(148, 21);
            this.cboProjectName.TabIndex = 59;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(68, 22);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(46, 13);
            this.label5.TabIndex = 58;
            this.label5.Text = "Project: ";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.LightSteelBlue;
            this.panel1.Controls.Add(this.btnsavestate);
            this.panel1.Controls.Add(this.btnShow);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Location = new System.Drawing.Point(306, -3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(95, 343);
            this.panel1.TabIndex = 150;
            // 
            // btnsavestate
            // 
            this.btnsavestate.Location = new System.Drawing.Point(8, 78);
            this.btnsavestate.Name = "btnsavestate";
            this.btnsavestate.Size = new System.Drawing.Size(80, 26);
            this.btnsavestate.TabIndex = 147;
            this.btnsavestate.Text = "Save State";
            this.btnsavestate.UseVisualStyleBackColor = true;
            this.btnsavestate.Click += new System.EventHandler(this.btnsavestate_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.BackColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox4.Controls.Add(this.btnSelectAccClass);
            this.groupBox4.Location = new System.Drawing.Point(2, 283);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(302, 57);
            this.groupBox4.TabIndex = 151;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Accounting Class";
            // 
            // btnSelectAccClass
            // 
            this.btnSelectAccClass.Location = new System.Drawing.Point(139, 19);
            this.btnSelectAccClass.Name = "btnSelectAccClass";
            this.btnSelectAccClass.Size = new System.Drawing.Size(148, 23);
            this.btnSelectAccClass.TabIndex = 3;
            this.btnSelectAccClass.Text = "Select Account Class";
            this.btnSelectAccClass.UseVisualStyleBackColor = true;
            this.btnSelectAccClass.Click += new System.EventHandler(this.btnSelectAccClass_Click_1);
            // 
            // groupBox5
            // 
            this.groupBox5.BackColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox5.Controls.Add(this.chkBank);
            this.groupBox5.Controls.Add(this.cboBanks);
            this.groupBox5.Controls.Add(this.chkParty);
            this.groupBox5.Controls.Add(this.cboParty);
            this.groupBox5.Location = new System.Drawing.Point(3, 155);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(303, 74);
            this.groupBox5.TabIndex = 152;
            this.groupBox5.TabStop = false;
            // 
            // cboMonths
            // 
            this.cboMonths.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMonths.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.cboMonths.FocusLostColor = System.Drawing.Color.White;
            this.cboMonths.FormattingEnabled = true;
            this.cboMonths.Location = new System.Drawing.Point(78, 75);
            this.cboMonths.Name = "cboMonths";
            this.cboMonths.Size = new System.Drawing.Size(91, 21);
            this.cboMonths.TabIndex = 151;
            this.cboMonths.SelectedIndexChanged += new System.EventHandler(this.cboMonths_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 78);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 13);
            this.label3.TabIndex = 150;
            this.label3.Text = "End of Month";
            // 
            // frmChequeSummarySettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(399, 341);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.pnlStatus);
            this.Name = "frmChequeSummarySettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Cheque Report Settings";
            this.Load += new System.EventHandler(this.frmChequeSummarySettings_Load);
            this.pnlStatus.ResumeLayout(false);
            this.pnlStatus.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RadioButton rdbChequeGiven;
        private System.Windows.Forms.RadioButton rdbChequeReceived;
        private System.Windows.Forms.Panel pnlStatus;
        private System.Windows.Forms.RadioButton rdbUnCleared;
        private System.Windows.Forms.RadioButton rdbCleared;
        private System.Windows.Forms.RadioButton rdbBoth;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chkBank;
        private System.Windows.Forms.CheckBox chkParty;
        private SComboBox cboBanks;
        private SComboBox cboParty;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnShow;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnToDate;
        private System.Windows.Forms.MaskedTextBox txtFromDate;
        private System.Windows.Forms.Button btnFromDate;
        private System.Windows.Forms.MaskedTextBox txtToDate;
        private System.Windows.Forms.CheckBox chkDateRange;
        private System.Windows.Forms.RadioButton rdbChequeAll;
        private System.Windows.Forms.GroupBox groupBox3;
        private SComboBox cboProjectName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button btnSelectAccClass;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Button btnsavestate;
        private SComboBox cboMonths;
        private System.Windows.Forms.Label label3;
    }
}