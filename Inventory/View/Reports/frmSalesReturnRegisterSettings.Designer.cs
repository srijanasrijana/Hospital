using SComponents;
namespace Inventory
{
    partial class frmSalesReturnRegisterSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSalesReturnRegisterSettings));
            this.cboPartySingle = new SComboBox();
            this.chkDateRange = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.chkProject = new System.Windows.Forms.CheckBox();
            this.cboProjectName = new SComboBox();
            this.cboDepotwise = new SComboBox();
            this.chkDepot = new System.Windows.Forms.CheckBox();
            this.rdPartyGroup = new System.Windows.Forms.RadioButton();
            this.rdpartySingle = new System.Windows.Forms.RadioButton();
            this.rdPartyAll = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.btnShow = new System.Windows.Forms.Button();
            this.txtToDate = new System.Windows.Forms.MaskedTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnToDate = new System.Windows.Forms.Button();
            this.btnFromDate = new System.Windows.Forms.Button();
            this.txtFromDate = new System.Windows.Forms.MaskedTextBox();
            this.cboPartyGroup = new SComboBox();
            this.grpParty = new System.Windows.Forms.GroupBox();
            this.button2 = new System.Windows.Forms.Button();
            this.btnSelectAccClass = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnsavestate = new System.Windows.Forms.Button();
            this.cboMonths = new SComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.grpParty.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cboPartySingle
            // 
            this.cboPartySingle.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboPartySingle.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboPartySingle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPartySingle.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.cboPartySingle.FocusLostColor = System.Drawing.Color.White;
            this.cboPartySingle.FormattingEnabled = true;
            this.cboPartySingle.Location = new System.Drawing.Point(215, 24);
            this.cboPartySingle.Name = "cboPartySingle";
            this.cboPartySingle.Size = new System.Drawing.Size(148, 21);
            this.cboPartySingle.TabIndex = 81;
            // 
            // chkDateRange
            // 
            this.chkDateRange.AutoSize = true;
            this.chkDateRange.Location = new System.Drawing.Point(9, 135);
            this.chkDateRange.Name = "chkDateRange";
            this.chkDateRange.Size = new System.Drawing.Size(117, 17);
            this.chkDateRange.TabIndex = 85;
            this.chkDateRange.Text = "Select Date Range";
            this.chkDateRange.UseVisualStyleBackColor = true;
            this.chkDateRange.CheckedChanged += new System.EventHandler(this.chkDateRange_CheckedChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox3.Controls.Add(this.chkProject);
            this.groupBox3.Controls.Add(this.cboProjectName);
            this.groupBox3.Controls.Add(this.cboDepotwise);
            this.groupBox3.Controls.Add(this.chkDepot);
            this.groupBox3.Location = new System.Drawing.Point(2, 85);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(378, 46);
            this.groupBox3.TabIndex = 84;
            this.groupBox3.TabStop = false;
            // 
            // chkProject
            // 
            this.chkProject.AutoSize = true;
            this.chkProject.Location = new System.Drawing.Point(189, 17);
            this.chkProject.Name = "chkProject";
            this.chkProject.Size = new System.Drawing.Size(59, 17);
            this.chkProject.TabIndex = 55;
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
            this.cboProjectName.Location = new System.Drawing.Point(257, 13);
            this.cboProjectName.Name = "cboProjectName";
            this.cboProjectName.Size = new System.Drawing.Size(105, 21);
            this.cboProjectName.TabIndex = 39;
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
            this.cboDepotwise.TabIndex = 74;
            // 
            // chkDepot
            // 
            this.chkDepot.AutoSize = true;
            this.chkDepot.Location = new System.Drawing.Point(6, 17);
            this.chkDepot.Name = "chkDepot";
            this.chkDepot.Size = new System.Drawing.Size(55, 17);
            this.chkDepot.TabIndex = 72;
            this.chkDepot.Text = "Depot";
            this.chkDepot.UseVisualStyleBackColor = true;
            this.chkDepot.CheckedChanged += new System.EventHandler(this.chkDepot_CheckedChanged);
            // 
            // rdPartyGroup
            // 
            this.rdPartyGroup.AutoSize = true;
            this.rdPartyGroup.Location = new System.Drawing.Point(7, 56);
            this.rdPartyGroup.Name = "rdPartyGroup";
            this.rdPartyGroup.Size = new System.Drawing.Size(81, 17);
            this.rdPartyGroup.TabIndex = 83;
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
            this.rdpartySingle.TabIndex = 84;
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
            this.rdPartyAll.TabIndex = 85;
            this.rdPartyAll.TabStop = true;
            this.rdPartyAll.Text = "All Party";
            this.rdPartyAll.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(33, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "From:";
            // 
            // btnShow
            // 
            this.btnShow.Location = new System.Drawing.Point(8, 6);
            this.btnShow.Name = "btnShow";
            this.btnShow.Size = new System.Drawing.Size(75, 23);
            this.btnShow.TabIndex = 82;
            this.btnShow.Text = "Show";
            this.btnShow.UseVisualStyleBackColor = true;
            this.btnShow.Click += new System.EventHandler(this.btnShow_Click);
            // 
            // txtToDate
            // 
            this.txtToDate.Location = new System.Drawing.Point(218, 17);
            this.txtToDate.Mask = "##/##/####";
            this.txtToDate.Name = "txtToDate";
            this.txtToDate.Size = new System.Drawing.Size(121, 20);
            this.txtToDate.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(189, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(23, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "To:";
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
            this.groupBox2.Location = new System.Drawing.Point(1, 155);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(381, 75);
            this.groupBox2.TabIndex = 81;
            this.groupBox2.TabStop = false;
            // 
            // btnToDate
            // 
            this.btnToDate.Image = global::Inventory.Properties.Resources.dateIcon;
            this.btnToDate.Location = new System.Drawing.Point(345, 14);
            this.btnToDate.Name = "btnToDate";
            this.btnToDate.Size = new System.Drawing.Size(26, 25);
            this.btnToDate.TabIndex = 9;
            this.btnToDate.UseVisualStyleBackColor = true;
            this.btnToDate.Click += new System.EventHandler(this.btnToDate_Click);
            // 
            // btnFromDate
            // 
            this.btnFromDate.Image = global::Inventory.Properties.Resources.dateIcon;
            this.btnFromDate.Location = new System.Drawing.Point(157, 14);
            this.btnFromDate.Name = "btnFromDate";
            this.btnFromDate.Size = new System.Drawing.Size(26, 25);
            this.btnFromDate.TabIndex = 8;
            this.btnFromDate.UseVisualStyleBackColor = true;
            this.btnFromDate.Click += new System.EventHandler(this.btnFromDate_Click);
            // 
            // txtFromDate
            // 
            this.txtFromDate.Location = new System.Drawing.Point(50, 16);
            this.txtFromDate.Mask = "##/##/####";
            this.txtFromDate.Name = "txtFromDate";
            this.txtFromDate.Size = new System.Drawing.Size(104, 20);
            this.txtFromDate.TabIndex = 6;
            // 
            // cboPartyGroup
            // 
            this.cboPartyGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPartyGroup.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.cboPartyGroup.FocusLostColor = System.Drawing.Color.White;
            this.cboPartyGroup.FormattingEnabled = true;
            this.cboPartyGroup.Location = new System.Drawing.Point(215, 51);
            this.cboPartyGroup.Name = "cboPartyGroup";
            this.cboPartyGroup.Size = new System.Drawing.Size(148, 21);
            this.cboPartyGroup.TabIndex = 82;
            // 
            // grpParty
            // 
            this.grpParty.BackColor = System.Drawing.Color.WhiteSmoke;
            this.grpParty.Controls.Add(this.rdPartyGroup);
            this.grpParty.Controls.Add(this.rdpartySingle);
            this.grpParty.Controls.Add(this.rdPartyAll);
            this.grpParty.Controls.Add(this.cboPartyGroup);
            this.grpParty.Controls.Add(this.cboPartySingle);
            this.grpParty.Location = new System.Drawing.Point(1, 2);
            this.grpParty.Name = "grpParty";
            this.grpParty.Size = new System.Drawing.Size(378, 78);
            this.grpParty.TabIndex = 86;
            this.grpParty.TabStop = false;
            // 
            // button2
            // 
            this.button2.Image = global::Inventory.Properties.Resources.gnome_window_close;
            this.button2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button2.Location = new System.Drawing.Point(8, 35);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 83;
            this.button2.Text = "&Cancel";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // btnSelectAccClass
            // 
            this.btnSelectAccClass.Location = new System.Drawing.Point(216, 19);
            this.btnSelectAccClass.Name = "btnSelectAccClass";
            this.btnSelectAccClass.Size = new System.Drawing.Size(149, 23);
            this.btnSelectAccClass.TabIndex = 87;
            this.btnSelectAccClass.Text = "Select Account Class";
            this.btnSelectAccClass.UseVisualStyleBackColor = true;
            this.btnSelectAccClass.Click += new System.EventHandler(this.btnSelectAccClass_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox1.Controls.Add(this.btnSelectAccClass);
            this.groupBox1.Location = new System.Drawing.Point(1, 236);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(381, 53);
            this.groupBox1.TabIndex = 88;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Accounting Class";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.LightSteelBlue;
            this.panel1.Controls.Add(this.btnsavestate);
            this.panel1.Controls.Add(this.btnShow);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Location = new System.Drawing.Point(379, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(92, 289);
            this.panel1.TabIndex = 89;
            // 
            // btnsavestate
            // 
            this.btnsavestate.Location = new System.Drawing.Point(8, 64);
            this.btnsavestate.Name = "btnsavestate";
            this.btnsavestate.Size = new System.Drawing.Size(75, 24);
            this.btnsavestate.TabIndex = 152;
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
            this.cboMonths.Location = new System.Drawing.Point(79, 42);
            this.cboMonths.Name = "cboMonths";
            this.cboMonths.Size = new System.Drawing.Size(104, 21);
            this.cboMonths.TabIndex = 63;
            this.cboMonths.SelectedIndexChanged += new System.EventHandler(this.cboMonths_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 13);
            this.label2.TabIndex = 62;
            this.label2.Text = "End of Month";
            // 
            // frmSalesReturnRegisterSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(470, 293);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.chkDateRange);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.grpParty);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmSalesReturnRegisterSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Sales Return RegisterSettings";
            this.Load += new System.EventHandler(this.frmSalesReturnRegisterSettings_Load);
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

        private SComboBox cboPartySingle;
        private System.Windows.Forms.CheckBox chkDateRange;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox chkProject;
        private SComboBox cboProjectName;
        private SComboBox cboDepotwise;
        private System.Windows.Forms.CheckBox chkDepot;
        private System.Windows.Forms.RadioButton rdPartyGroup;
        private System.Windows.Forms.RadioButton rdpartySingle;
        private System.Windows.Forms.RadioButton rdPartyAll;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button btnShow;
        private System.Windows.Forms.MaskedTextBox txtToDate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.MaskedTextBox txtFromDate;
        private SComboBox cboPartyGroup;
        private System.Windows.Forms.GroupBox grpParty;
        private System.Windows.Forms.Button btnSelectAccClass;
        private System.Windows.Forms.Button btnToDate;
        private System.Windows.Forms.Button btnFromDate;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnsavestate;
        private SComboBox cboMonths;
        private System.Windows.Forms.Label label2;
    }
}