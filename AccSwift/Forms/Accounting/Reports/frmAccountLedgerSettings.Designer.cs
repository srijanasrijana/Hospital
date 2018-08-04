using SComponents;
namespace Inventory
{
    partial class frmAccountLedgerSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAccountLedgerSettings));
            this.btnShow = new System.Windows.Forms.Button();
            this.rbtnChooseLedger = new System.Windows.Forms.RadioButton();
            this.rbtnChooseAccountGrp = new System.Windows.Forms.RadioButton();
            this.label6 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtToDate = new System.Windows.Forms.MaskedTextBox();
            this.txtFromDate = new System.Windows.Forms.MaskedTextBox();
            this.grpDate = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.btnDate = new System.Windows.Forms.Button();
            this.chkDate = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.rbtnSummary = new System.Windows.Forms.RadioButton();
            this.rbtnDetails = new System.Windows.Forms.RadioButton();
            this.cmbChooseLedger = new SComboBox();
            this.cmbChooseAccountGroup = new SComboBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.cboProjectName = new SComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnSelectAccClass = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnsavestate = new System.Windows.Forms.Button();
            this.cboMonths = new SComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.grpDate.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnShow
            // 
            this.btnShow.Location = new System.Drawing.Point(12, 13);
            this.btnShow.Name = "btnShow";
            this.btnShow.Size = new System.Drawing.Size(75, 23);
            this.btnShow.TabIndex = 3;
            this.btnShow.Text = "Show";
            this.btnShow.UseVisualStyleBackColor = true;
            this.btnShow.Click += new System.EventHandler(this.btnShow_Click);
            // 
            // rbtnChooseLedger
            // 
            this.rbtnChooseLedger.AutoSize = true;
            this.rbtnChooseLedger.Location = new System.Drawing.Point(22, 12);
            this.rbtnChooseLedger.Name = "rbtnChooseLedger";
            this.rbtnChooseLedger.Size = new System.Drawing.Size(140, 17);
            this.rbtnChooseLedger.TabIndex = 0;
            this.rbtnChooseLedger.TabStop = true;
            this.rbtnChooseLedger.Text = "Choose Account Ledger";
            this.rbtnChooseLedger.UseVisualStyleBackColor = true;
            this.rbtnChooseLedger.Click += new System.EventHandler(this.rbtnChooseLedger_Click);
            // 
            // rbtnChooseAccountGrp
            // 
            this.rbtnChooseAccountGrp.AutoSize = true;
            this.rbtnChooseAccountGrp.Location = new System.Drawing.Point(184, 12);
            this.rbtnChooseAccountGrp.Name = "rbtnChooseAccountGrp";
            this.rbtnChooseAccountGrp.Size = new System.Drawing.Size(136, 17);
            this.rbtnChooseAccountGrp.TabIndex = 1;
            this.rbtnChooseAccountGrp.TabStop = true;
            this.rbtnChooseAccountGrp.Text = "Choose Account Group";
            this.rbtnChooseAccountGrp.UseVisualStyleBackColor = true;
            this.rbtnChooseAccountGrp.Click += new System.EventHandler(this.rbtnChooseAccountGrp_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(20, 21);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(33, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "From:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(21, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(23, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "To:";
            // 
            // txtToDate
            // 
            this.txtToDate.Location = new System.Drawing.Point(59, 47);
            this.txtToDate.Mask = "##/##/####";
            this.txtToDate.Name = "txtToDate";
            this.txtToDate.Size = new System.Drawing.Size(160, 20);
            this.txtToDate.TabIndex = 4;
            // 
            // txtFromDate
            // 
            this.txtFromDate.Location = new System.Drawing.Point(60, 11);
            this.txtFromDate.Mask = "##/##/####";
            this.txtFromDate.Name = "txtFromDate";
            this.txtFromDate.Size = new System.Drawing.Size(159, 20);
            this.txtFromDate.TabIndex = 1;
            this.txtFromDate.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtFromDate_KeyDown);
            // 
            // grpDate
            // 
            this.grpDate.BackColor = System.Drawing.Color.WhiteSmoke;
            this.grpDate.Controls.Add(this.cboMonths);
            this.grpDate.Controls.Add(this.label1);
            this.grpDate.Controls.Add(this.button1);
            this.grpDate.Controls.Add(this.btnDate);
            this.grpDate.Controls.Add(this.txtFromDate);
            this.grpDate.Controls.Add(this.txtToDate);
            this.grpDate.Controls.Add(this.label2);
            this.grpDate.Controls.Add(this.label6);
            this.grpDate.Location = new System.Drawing.Point(3, 14);
            this.grpDate.Name = "grpDate";
            this.grpDate.Size = new System.Drawing.Size(335, 102);
            this.grpDate.TabIndex = 41;
            this.grpDate.TabStop = false;
            // 
            // button1
            // 
            this.button1.Image = global::Inventory.Properties.Resources.dateIcon;
            this.button1.Location = new System.Drawing.Point(225, 43);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(26, 24);
            this.button1.TabIndex = 5;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnDate
            // 
            this.btnDate.Image = global::Inventory.Properties.Resources.dateIcon;
            this.btnDate.Location = new System.Drawing.Point(225, 8);
            this.btnDate.Name = "btnDate";
            this.btnDate.Size = new System.Drawing.Size(26, 24);
            this.btnDate.TabIndex = 2;
            this.btnDate.UseVisualStyleBackColor = true;
            this.btnDate.Click += new System.EventHandler(this.btnDate_Click);
            // 
            // chkDate
            // 
            this.chkDate.AutoSize = true;
            this.chkDate.Location = new System.Drawing.Point(3, 1);
            this.chkDate.Name = "chkDate";
            this.chkDate.Size = new System.Drawing.Size(143, 17);
            this.chkDate.TabIndex = 0;
            this.chkDate.Text = "Click For Date Selection:";
            this.chkDate.UseVisualStyleBackColor = true;
            this.chkDate.CheckedChanged += new System.EventHandler(this.chkDate_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox1.Controls.Add(this.panel2);
            this.groupBox1.Controls.Add(this.cmbChooseLedger);
            this.groupBox1.Controls.Add(this.cmbChooseAccountGroup);
            this.groupBox1.Controls.Add(this.rbtnChooseAccountGrp);
            this.groupBox1.Controls.Add(this.rbtnChooseLedger);
            this.groupBox1.Location = new System.Drawing.Point(4, 118);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(335, 112);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.rbtnSummary);
            this.panel2.Controls.Add(this.rbtnDetails);
            this.panel2.Location = new System.Drawing.Point(22, 61);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(298, 37);
            this.panel2.TabIndex = 6;
            // 
            // rbtnSummary
            // 
            this.rbtnSummary.AutoSize = true;
            this.rbtnSummary.Location = new System.Drawing.Point(3, 11);
            this.rbtnSummary.Name = "rbtnSummary";
            this.rbtnSummary.Size = new System.Drawing.Size(68, 17);
            this.rbtnSummary.TabIndex = 4;
            this.rbtnSummary.TabStop = true;
            this.rbtnSummary.Text = "Summary";
            this.rbtnSummary.UseVisualStyleBackColor = true;
            // 
            // rbtnDetails
            // 
            this.rbtnDetails.AutoSize = true;
            this.rbtnDetails.Location = new System.Drawing.Point(165, 11);
            this.rbtnDetails.Name = "rbtnDetails";
            this.rbtnDetails.Size = new System.Drawing.Size(57, 17);
            this.rbtnDetails.TabIndex = 5;
            this.rbtnDetails.TabStop = true;
            this.rbtnDetails.Text = "Details";
            this.rbtnDetails.UseVisualStyleBackColor = true;
            // 
            // cmbChooseLedger
            // 
            this.cmbChooseLedger.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cmbChooseLedger.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbChooseLedger.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.cmbChooseLedger.FocusLostColor = System.Drawing.Color.White;
            this.cmbChooseLedger.FormattingEnabled = true;
            this.cmbChooseLedger.Location = new System.Drawing.Point(22, 35);
            this.cmbChooseLedger.Name = "cmbChooseLedger";
            this.cmbChooseLedger.Size = new System.Drawing.Size(140, 21);
            this.cmbChooseLedger.TabIndex = 2;
            // 
            // cmbChooseAccountGroup
            // 
            this.cmbChooseAccountGroup.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cmbChooseAccountGroup.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbChooseAccountGroup.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.cmbChooseAccountGroup.FocusLostColor = System.Drawing.Color.White;
            this.cmbChooseAccountGroup.FormattingEnabled = true;
            this.cmbChooseAccountGroup.Items.AddRange(new object[] {
            "All",
            "List Of Account Group"});
            this.cmbChooseAccountGroup.Location = new System.Drawing.Point(184, 35);
            this.cmbChooseAccountGroup.Name = "cmbChooseAccountGroup";
            this.cmbChooseAccountGroup.Size = new System.Drawing.Size(136, 21);
            this.cmbChooseAccountGroup.TabIndex = 3;
            // 
            // btnCancel
            // 
            this.btnCancel.Image = global::Inventory.Properties.Resources.gnome_window_close;
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(12, 42);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(76, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox3.Controls.Add(this.cboProjectName);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Location = new System.Drawing.Point(3, 284);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(335, 53);
            this.groupBox3.TabIndex = 61;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Project";
            // 
            // cboProjectName
            // 
            this.cboProjectName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboProjectName.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.cboProjectName.FocusLostColor = System.Drawing.Color.White;
            this.cboProjectName.FormattingEnabled = true;
            this.cboProjectName.Location = new System.Drawing.Point(179, 21);
            this.cboProjectName.Name = "cboProjectName";
            this.cboProjectName.Size = new System.Drawing.Size(148, 21);
            this.cboProjectName.TabIndex = 59;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(127, 24);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(46, 13);
            this.label5.TabIndex = 58;
            this.label5.Text = "Project: ";
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox2.Controls.Add(this.btnSelectAccClass);
            this.groupBox2.Location = new System.Drawing.Point(5, 235);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(334, 48);
            this.groupBox2.TabIndex = 62;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Accounting Class";
            // 
            // btnSelectAccClass
            // 
            this.btnSelectAccClass.Location = new System.Drawing.Point(184, 19);
            this.btnSelectAccClass.Name = "btnSelectAccClass";
            this.btnSelectAccClass.Size = new System.Drawing.Size(148, 23);
            this.btnSelectAccClass.TabIndex = 3;
            this.btnSelectAccClass.Text = "Select Account Class";
            this.btnSelectAccClass.UseVisualStyleBackColor = true;
            this.btnSelectAccClass.Click += new System.EventHandler(this.btnSelectAccClass_Click_1);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.LightSteelBlue;
            this.panel1.Controls.Add(this.btnsavestate);
            this.panel1.Controls.Add(this.btnShow);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Location = new System.Drawing.Point(342, -2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(94, 339);
            this.panel1.TabIndex = 63;
            // 
            // btnsavestate
            // 
            this.btnsavestate.Location = new System.Drawing.Point(12, 72);
            this.btnsavestate.Name = "btnsavestate";
            this.btnsavestate.Size = new System.Drawing.Size(77, 23);
            this.btnsavestate.TabIndex = 65;
            this.btnsavestate.Text = "Save State";
            this.btnsavestate.UseVisualStyleBackColor = true;
            this.btnsavestate.Click += new System.EventHandler(this.btnsavestate_Click);
            // 
            // cboMonths
            // 
            this.cboMonths.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMonths.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.cboMonths.FocusLostColor = System.Drawing.Color.White;
            this.cboMonths.FormattingEnabled = true;
            this.cboMonths.Location = new System.Drawing.Point(97, 75);
            this.cboMonths.Name = "cboMonths";
            this.cboMonths.Size = new System.Drawing.Size(154, 21);
            this.cboMonths.TabIndex = 63;
            this.cboMonths.SelectedIndexChanged += new System.EventHandler(this.cboMonths_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 78);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 13);
            this.label1.TabIndex = 62;
            this.label1.Text = "End of Month";
            // 
            // frmAccountLedgerSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(442, 339);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.chkDate);
            this.Controls.Add(this.grpDate);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "frmAccountLedgerSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Account Ledger Settings";
            this.Load += new System.EventHandler(this.frmAccountLedgerSettings_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmAccountLedgerSettings_KeyDown);
            this.grpDate.ResumeLayout(false);
            this.grpDate.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnShow;
        private SComboBox cmbChooseAccountGroup;
        private SComboBox cmbChooseLedger;
        private System.Windows.Forms.RadioButton rbtnChooseLedger;
        private System.Windows.Forms.RadioButton rbtnChooseAccountGrp;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.MaskedTextBox txtToDate;
        private System.Windows.Forms.MaskedTextBox txtFromDate;
        private System.Windows.Forms.GroupBox grpDate;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnDate;
        private System.Windows.Forms.CheckBox chkDate;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox groupBox3;
        private SComboBox cboProjectName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnSelectAccClass;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnsavestate;
        private System.Windows.Forms.RadioButton rbtnDetails;
        private System.Windows.Forms.RadioButton rbtnSummary;
        private System.Windows.Forms.Panel panel2;
        private SComboBox cboMonths;
        private System.Windows.Forms.Label label1;
    }
}