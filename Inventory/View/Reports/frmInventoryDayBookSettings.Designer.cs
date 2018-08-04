using SComponents;
namespace Inventory
{
    partial class frmInventoryDayBookSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmInventoryDayBookSettings));
            this.chkDateRange = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.chkProject = new System.Windows.Forms.CheckBox();
            this.cboProjectName = new SComponents.SComboBox();
            this.cboDepotwise = new SComponents.SComboBox();
            this.chkDepot = new System.Windows.Forms.CheckBox();
            this.btnShow = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cboMonths = new SComponents.SComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnToDate = new System.Windows.Forms.Button();
            this.btnFromDate = new System.Windows.Forms.Button();
            this.txtFromDate = new System.Windows.Forms.MaskedTextBox();
            this.txtToDate = new System.Windows.Forms.MaskedTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.grpParty = new System.Windows.Forms.GroupBox();
            this.rdPartyGroup = new System.Windows.Forms.RadioButton();
            this.rdpartySingle = new System.Windows.Forms.RadioButton();
            this.rdPartyAll = new System.Windows.Forms.RadioButton();
            this.cboPartyGroup = new SComponents.SComboBox();
            this.cboPartySingle = new SComponents.SComboBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSelectAccClass = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnsavestate = new System.Windows.Forms.Button();
            this.cboVoucherType = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.grpParty.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkDateRange
            // 
            this.chkDateRange.AutoSize = true;
            this.chkDateRange.Location = new System.Drawing.Point(16, 135);
            this.chkDateRange.Name = "chkDateRange";
            this.chkDateRange.Size = new System.Drawing.Size(117, 17);
            this.chkDateRange.TabIndex = 3;
            this.chkDateRange.Text = "Select Date Range";
            this.chkDateRange.UseVisualStyleBackColor = true;
            this.chkDateRange.CheckedChanged += new System.EventHandler(this.chkDateRange_CheckedChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.cboVoucherType);
            this.groupBox3.Controls.Add(this.chkProject);
            this.groupBox3.Controls.Add(this.cboProjectName);
            this.groupBox3.Controls.Add(this.cboDepotwise);
            this.groupBox3.Controls.Add(this.chkDepot);
            this.groupBox3.Location = new System.Drawing.Point(2, 86);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(412, 46);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Enter += new System.EventHandler(this.groupBox3_Enter);
            // 
            // chkProject
            // 
            this.chkProject.AutoSize = true;
            this.chkProject.Location = new System.Drawing.Point(231, 16);
            this.chkProject.Name = "chkProject";
            this.chkProject.Size = new System.Drawing.Size(59, 17);
            this.chkProject.TabIndex = 2;
            this.chkProject.Text = "Project";
            this.chkProject.UseVisualStyleBackColor = true;
            this.chkProject.CheckedChanged += new System.EventHandler(this.chkProject_CheckedChanged);
            // 
            // cboProjectName
            // 
            this.cboProjectName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboProjectName.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.cboProjectName.FocusLostColor = System.Drawing.Color.White;
            this.cboProjectName.FormattingEnabled = true;
            this.cboProjectName.Location = new System.Drawing.Point(299, 13);
            this.cboProjectName.Name = "cboProjectName";
            this.cboProjectName.Size = new System.Drawing.Size(105, 21);
            this.cboProjectName.TabIndex = 3;
            this.cboProjectName.SelectedIndexChanged += new System.EventHandler(this.cboProjectName_SelectedIndexChanged);
            // 
            // cboDepotwise
            // 
            this.cboDepotwise.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDepotwise.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.cboDepotwise.FocusLostColor = System.Drawing.Color.White;
            this.cboDepotwise.FormattingEnabled = true;
            this.cboDepotwise.Location = new System.Drawing.Point(65, 15);
            this.cboDepotwise.Name = "cboDepotwise";
            this.cboDepotwise.Size = new System.Drawing.Size(113, 21);
            this.cboDepotwise.TabIndex = 1;
            this.cboDepotwise.Visible = false;
            this.cboDepotwise.SelectedIndexChanged += new System.EventHandler(this.cboDepotwise_SelectedIndexChanged);
            // 
            // chkDepot
            // 
            this.chkDepot.AutoSize = true;
            this.chkDepot.Location = new System.Drawing.Point(6, 17);
            this.chkDepot.Name = "chkDepot";
            this.chkDepot.Size = new System.Drawing.Size(55, 17);
            this.chkDepot.TabIndex = 0;
            this.chkDepot.Text = "Depot";
            this.chkDepot.UseVisualStyleBackColor = true;
            this.chkDepot.Visible = false;
            this.chkDepot.CheckedChanged += new System.EventHandler(this.chkDepot_CheckedChanged);
            // 
            // btnShow
            // 
            this.btnShow.Location = new System.Drawing.Point(11, 6);
            this.btnShow.Name = "btnShow";
            this.btnShow.Size = new System.Drawing.Size(75, 23);
            this.btnShow.TabIndex = 6;
            this.btnShow.Text = "Show";
            this.btnShow.UseVisualStyleBackColor = true;
            this.btnShow.Click += new System.EventHandler(this.btnShow_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox2.Controls.Add(this.cboMonths);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.btnToDate);
            this.groupBox2.Controls.Add(this.btnFromDate);
            this.groupBox2.Controls.Add(this.txtFromDate);
            this.groupBox2.Controls.Add(this.txtToDate);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Location = new System.Drawing.Point(2, 154);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(415, 72);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Enter += new System.EventHandler(this.groupBox2_Enter);
            // 
            // cboMonths
            // 
            this.cboMonths.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMonths.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.cboMonths.FocusLostColor = System.Drawing.Color.White;
            this.cboMonths.FormattingEnabled = true;
            this.cboMonths.Location = new System.Drawing.Point(299, 18);
            this.cboMonths.Name = "cboMonths";
            this.cboMonths.Size = new System.Drawing.Size(106, 21);
            this.cboMonths.TabIndex = 65;
            this.cboMonths.SelectedIndexChanged += new System.EventHandler(this.sComboBox1_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(214, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 13);
            this.label2.TabIndex = 64;
            this.label2.Text = "End of Month";
            // 
            // btnToDate
            // 
            this.btnToDate.Image = global::Inventory.Properties.Resources.dateIcon;
            this.btnToDate.Location = new System.Drawing.Point(182, 43);
            this.btnToDate.Name = "btnToDate";
            this.btnToDate.Size = new System.Drawing.Size(26, 25);
            this.btnToDate.TabIndex = 5;
            this.btnToDate.UseVisualStyleBackColor = true;
            this.btnToDate.Click += new System.EventHandler(this.btnToDate_Click);
            // 
            // btnFromDate
            // 
            this.btnFromDate.Image = global::Inventory.Properties.Resources.dateIcon;
            this.btnFromDate.Location = new System.Drawing.Point(182, 15);
            this.btnFromDate.Name = "btnFromDate";
            this.btnFromDate.Size = new System.Drawing.Size(26, 25);
            this.btnFromDate.TabIndex = 4;
            this.btnFromDate.UseVisualStyleBackColor = true;
            this.btnFromDate.Click += new System.EventHandler(this.btnFromDate_Click);
            // 
            // txtFromDate
            // 
            this.txtFromDate.Location = new System.Drawing.Point(47, 16);
            this.txtFromDate.Mask = "##/##/####";
            this.txtFromDate.Name = "txtFromDate";
            this.txtFromDate.Size = new System.Drawing.Size(129, 20);
            this.txtFromDate.TabIndex = 1;
            this.txtFromDate.MaskInputRejected += new System.Windows.Forms.MaskInputRejectedEventHandler(this.txtFromDate_MaskInputRejected);
            // 
            // txtToDate
            // 
            this.txtToDate.Location = new System.Drawing.Point(46, 48);
            this.txtToDate.Mask = "##/##/####";
            this.txtToDate.Name = "txtToDate";
            this.txtToDate.Size = new System.Drawing.Size(130, 20);
            this.txtToDate.TabIndex = 3;
            this.txtToDate.MaskInputRejected += new System.Windows.Forms.MaskInputRejectedEventHandler(this.txtToDate_MaskInputRejected);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 46);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(23, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "To:";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(33, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "From:";
            // 
            // grpParty
            // 
            this.grpParty.BackColor = System.Drawing.Color.WhiteSmoke;
            this.grpParty.Controls.Add(this.rdPartyGroup);
            this.grpParty.Controls.Add(this.rdpartySingle);
            this.grpParty.Controls.Add(this.rdPartyAll);
            this.grpParty.Controls.Add(this.cboPartyGroup);
            this.grpParty.Controls.Add(this.cboPartySingle);
            this.grpParty.Location = new System.Drawing.Point(2, 2);
            this.grpParty.Name = "grpParty";
            this.grpParty.Size = new System.Drawing.Size(412, 78);
            this.grpParty.TabIndex = 1;
            this.grpParty.TabStop = false;
            this.grpParty.Enter += new System.EventHandler(this.grpParty_Enter);
            // 
            // rdPartyGroup
            // 
            this.rdPartyGroup.AutoSize = true;
            this.rdPartyGroup.Location = new System.Drawing.Point(7, 56);
            this.rdPartyGroup.Name = "rdPartyGroup";
            this.rdPartyGroup.Size = new System.Drawing.Size(81, 17);
            this.rdPartyGroup.TabIndex = 3;
            this.rdPartyGroup.TabStop = true;
            this.rdPartyGroup.Text = "Party Group";
            this.rdPartyGroup.UseVisualStyleBackColor = true;
            this.rdPartyGroup.CheckedChanged += new System.EventHandler(this.rdPartyGroup_CheckedChanged);
            // 
            // rdpartySingle
            // 
            this.rdpartySingle.AutoSize = true;
            this.rdpartySingle.Location = new System.Drawing.Point(7, 30);
            this.rdpartySingle.Name = "rdpartySingle";
            this.rdpartySingle.Size = new System.Drawing.Size(81, 17);
            this.rdpartySingle.TabIndex = 1;
            this.rdpartySingle.TabStop = true;
            this.rdpartySingle.Text = "Single Party";
            this.rdpartySingle.UseVisualStyleBackColor = true;
            this.rdpartySingle.CheckedChanged += new System.EventHandler(this.rdpartySingle_CheckedChanged);
            // 
            // rdPartyAll
            // 
            this.rdPartyAll.AutoSize = true;
            this.rdPartyAll.Location = new System.Drawing.Point(7, 7);
            this.rdPartyAll.Name = "rdPartyAll";
            this.rdPartyAll.Size = new System.Drawing.Size(63, 17);
            this.rdPartyAll.TabIndex = 0;
            this.rdPartyAll.TabStop = true;
            this.rdPartyAll.Text = "All Party";
            this.rdPartyAll.UseVisualStyleBackColor = true;
            this.rdPartyAll.CheckedChanged += new System.EventHandler(this.rdPartyAll_CheckedChanged);
            // 
            // cboPartyGroup
            // 
            this.cboPartyGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPartyGroup.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.cboPartyGroup.FocusLostColor = System.Drawing.Color.White;
            this.cboPartyGroup.FormattingEnabled = true;
            this.cboPartyGroup.Location = new System.Drawing.Point(94, 53);
            this.cboPartyGroup.Name = "cboPartyGroup";
            this.cboPartyGroup.Size = new System.Drawing.Size(206, 21);
            this.cboPartyGroup.TabIndex = 4;
            this.cboPartyGroup.SelectedIndexChanged += new System.EventHandler(this.cboPartyGroup_SelectedIndexChanged);
            // 
            // cboPartySingle
            // 
            this.cboPartySingle.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboPartySingle.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboPartySingle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPartySingle.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.cboPartySingle.FocusLostColor = System.Drawing.Color.White;
            this.cboPartySingle.FormattingEnabled = true;
            this.cboPartySingle.Location = new System.Drawing.Point(94, 26);
            this.cboPartySingle.Name = "cboPartySingle";
            this.cboPartySingle.Size = new System.Drawing.Size(206, 21);
            this.cboPartySingle.TabIndex = 2;
            this.cboPartySingle.SelectedIndexChanged += new System.EventHandler(this.cboPartySingle_SelectedIndexChanged);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Image = global::Inventory.Properties.Resources.gnome_window_close;
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(11, 40);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.button2_Click);
            // 
            // btnSelectAccClass
            // 
            this.btnSelectAccClass.Location = new System.Drawing.Point(296, 13);
            this.btnSelectAccClass.Name = "btnSelectAccClass";
            this.btnSelectAccClass.Size = new System.Drawing.Size(105, 23);
            this.btnSelectAccClass.TabIndex = 5;
            this.btnSelectAccClass.Text = "Select Account Class";
            this.btnSelectAccClass.UseVisualStyleBackColor = true;
            this.btnSelectAccClass.Click += new System.EventHandler(this.btnSelectAccClass_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox1.Controls.Add(this.btnSelectAccClass);
            this.groupBox1.Location = new System.Drawing.Point(5, 232);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(412, 45);
            this.groupBox1.TabIndex = 62;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Accounting Class";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.LightSteelBlue;
            this.panel1.Controls.Add(this.btnsavestate);
            this.panel1.Controls.Add(this.btnShow);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Location = new System.Drawing.Point(420, -3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(93, 280);
            this.panel1.TabIndex = 63;
            // 
            // btnsavestate
            // 
            this.btnsavestate.Location = new System.Drawing.Point(9, 75);
            this.btnsavestate.Name = "btnsavestate";
            this.btnsavestate.Size = new System.Drawing.Size(80, 26);
            this.btnsavestate.TabIndex = 149;
            this.btnsavestate.Text = "Save State";
            this.btnsavestate.UseVisualStyleBackColor = true;
            this.btnsavestate.Click += new System.EventHandler(this.btnsavestate_Click);
            // 
            // cboVoucherType
            // 
            this.cboVoucherType.FormattingEnabled = true;
            this.cboVoucherType.Items.AddRange(new object[] {
            "PURCHASE",
            "SALES"});
            this.cboVoucherType.Location = new System.Drawing.Point(94, 14);
            this.cboVoucherType.Name = "cboVoucherType";
            this.cboVoucherType.Size = new System.Drawing.Size(116, 21);
            this.cboVoucherType.TabIndex = 64;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 17);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(74, 13);
            this.label4.TabIndex = 65;
            this.label4.Text = "Voucher Type";
            // 
            // frmInventoryDayBookSettings
            // 
            this.AcceptButton = this.btnShow;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(516, 280);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.chkDateRange);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.grpParty);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmInventoryDayBookSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Inventory Party Ledger Report Setting";
            this.Load += new System.EventHandler(this.frmInventoryDayBookSettings_Load);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.grpParty.ResumeLayout(false);
            this.grpParty.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkDateRange;
        private System.Windows.Forms.GroupBox groupBox3;
        private SComboBox cboDepotwise;
        private System.Windows.Forms.CheckBox chkDepot;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnShow;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.MaskedTextBox txtFromDate;
        private System.Windows.Forms.MaskedTextBox txtToDate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox chkProject;
        private SComboBox cboProjectName;
        private System.Windows.Forms.GroupBox grpParty;
        private System.Windows.Forms.RadioButton rdPartyGroup;
        private System.Windows.Forms.RadioButton rdpartySingle;
        private System.Windows.Forms.RadioButton rdPartyAll;
        private SComboBox cboPartyGroup;
        private SComboBox cboPartySingle;
        private System.Windows.Forms.Button btnSelectAccClass;
        private System.Windows.Forms.Button btnToDate;
        private System.Windows.Forms.Button btnFromDate;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnsavestate;
        private SComboBox cboMonths;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cboVoucherType;
    }
}