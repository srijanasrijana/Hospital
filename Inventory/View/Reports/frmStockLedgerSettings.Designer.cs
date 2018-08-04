using SComponents;
namespace Inventory
{
    partial class frmStockLedgerSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmStockLedgerSettings));
            this.txtFromDate = new System.Windows.Forms.MaskedTextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cboMonths = new SComponents.SComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnToDate = new System.Windows.Forms.Button();
            this.btnFromDate = new System.Windows.Forms.Button();
            this.txtToDate = new System.Windows.Forms.MaskedTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnShow = new System.Windows.Forms.Button();
            this.chkProject = new System.Windows.Forms.CheckBox();
            this.cboProjectName = new SComponents.SComboBox();
            this.cboDepotwise = new SComponents.SComboBox();
            this.chkDepot = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.chkDateRange = new System.Windows.Forms.CheckBox();
            this.button2 = new System.Windows.Forms.Button();
            this.grpProduct = new System.Windows.Forms.GroupBox();
            this.cboProductGroup = new SComponents.SComboBox();
            this.cboProductSingle = new SComponents.SComboBox();
            this.rdProductGroup = new System.Windows.Forms.RadioButton();
            this.rdProductSingle = new System.Windows.Forms.RadioButton();
            this.rdProductAll = new System.Windows.Forms.RadioButton();
            this.btnSelectAccClass = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnsavestate = new System.Windows.Forms.Button();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.grpProduct.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtFromDate
            // 
            this.txtFromDate.Location = new System.Drawing.Point(49, 16);
            this.txtFromDate.Mask = "##/##/####";
            this.txtFromDate.Name = "txtFromDate";
            this.txtFromDate.Size = new System.Drawing.Size(104, 20);
            this.txtFromDate.TabIndex = 6;
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
            this.groupBox2.Location = new System.Drawing.Point(1, 139);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(381, 71);
            this.groupBox2.TabIndex = 88;
            this.groupBox2.TabStop = false;
            // 
            // cboMonths
            // 
            this.cboMonths.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMonths.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.cboMonths.FocusLostColor = System.Drawing.Color.White;
            this.cboMonths.FormattingEnabled = true;
            this.cboMonths.Location = new System.Drawing.Point(78, 45);
            this.cboMonths.Name = "cboMonths";
            this.cboMonths.Size = new System.Drawing.Size(107, 21);
            this.cboMonths.TabIndex = 63;
            this.cboMonths.SelectedIndexChanged += new System.EventHandler(this.cboMonths_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 13);
            this.label2.TabIndex = 62;
            this.label2.Text = "End of Month";
            // 
            // btnToDate
            // 
            this.btnToDate.Image = global::Inventory.Properties.Resources.dateIcon;
            this.btnToDate.Location = new System.Drawing.Point(349, 17);
            this.btnToDate.Name = "btnToDate";
            this.btnToDate.Size = new System.Drawing.Size(26, 25);
            this.btnToDate.TabIndex = 9;
            this.btnToDate.UseVisualStyleBackColor = true;
            this.btnToDate.Click += new System.EventHandler(this.btnToDate_Click);
            // 
            // btnFromDate
            // 
            this.btnFromDate.Image = global::Inventory.Properties.Resources.dateIcon;
            this.btnFromDate.Location = new System.Drawing.Point(159, 14);
            this.btnFromDate.Name = "btnFromDate";
            this.btnFromDate.Size = new System.Drawing.Size(26, 25);
            this.btnFromDate.TabIndex = 8;
            this.btnFromDate.UseVisualStyleBackColor = true;
            this.btnFromDate.Click += new System.EventHandler(this.btnFromDate_Click);
            // 
            // txtToDate
            // 
            this.txtToDate.Location = new System.Drawing.Point(225, 19);
            this.txtToDate.Mask = "##/##/####";
            this.txtToDate.Name = "txtToDate";
            this.txtToDate.Size = new System.Drawing.Size(121, 20);
            this.txtToDate.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(192, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(23, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "To:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(33, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "From:";
            // 
            // btnShow
            // 
            this.btnShow.Location = new System.Drawing.Point(9, 7);
            this.btnShow.Name = "btnShow";
            this.btnShow.Size = new System.Drawing.Size(83, 28);
            this.btnShow.TabIndex = 89;
            this.btnShow.Text = "&Show";
            this.btnShow.UseVisualStyleBackColor = true;
            this.btnShow.Click += new System.EventHandler(this.btnShow_Click);
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
            this.cboProjectName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboProjectName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboProjectName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboProjectName.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.cboProjectName.FocusLostColor = System.Drawing.Color.White;
            this.cboProjectName.FormattingEnabled = true;
            this.cboProjectName.Location = new System.Drawing.Point(257, 16);
            this.cboProjectName.Name = "cboProjectName";
            this.cboProjectName.Size = new System.Drawing.Size(105, 21);
            this.cboProjectName.TabIndex = 39;
            // 
            // cboDepotwise
            // 
            this.cboDepotwise.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboDepotwise.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
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
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox3.Controls.Add(this.chkProject);
            this.groupBox3.Controls.Add(this.cboProjectName);
            this.groupBox3.Controls.Add(this.cboDepotwise);
            this.groupBox3.Controls.Add(this.chkDepot);
            this.groupBox3.Location = new System.Drawing.Point(2, 68);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(378, 46);
            this.groupBox3.TabIndex = 91;
            this.groupBox3.TabStop = false;
            this.groupBox3.Enter += new System.EventHandler(this.groupBox3_Enter);
            // 
            // chkDateRange
            // 
            this.chkDateRange.AutoSize = true;
            this.chkDateRange.Location = new System.Drawing.Point(8, 120);
            this.chkDateRange.Name = "chkDateRange";
            this.chkDateRange.Size = new System.Drawing.Size(117, 17);
            this.chkDateRange.TabIndex = 92;
            this.chkDateRange.Text = "Select Date Range";
            this.chkDateRange.UseVisualStyleBackColor = true;
            this.chkDateRange.CheckedChanged += new System.EventHandler(this.chkDateRange_CheckedChanged);
            // 
            // button2
            // 
            this.button2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button2.Location = new System.Drawing.Point(9, 41);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(83, 28);
            this.button2.TabIndex = 90;
            this.button2.Text = "&Cancel";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // grpProduct
            // 
            this.grpProduct.BackColor = System.Drawing.Color.WhiteSmoke;
            this.grpProduct.Controls.Add(this.cboProductGroup);
            this.grpProduct.Controls.Add(this.cboProductSingle);
            this.grpProduct.Controls.Add(this.rdProductGroup);
            this.grpProduct.Controls.Add(this.rdProductSingle);
            this.grpProduct.Controls.Add(this.rdProductAll);
            this.grpProduct.Location = new System.Drawing.Point(2, 2);
            this.grpProduct.Name = "grpProduct";
            this.grpProduct.Size = new System.Drawing.Size(377, 66);
            this.grpProduct.TabIndex = 93;
            this.grpProduct.TabStop = false;
            // 
            // cboProductGroup
            // 
            this.cboProductGroup.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboProductGroup.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboProductGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboProductGroup.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.cboProductGroup.FocusLostColor = System.Drawing.Color.White;
            this.cboProductGroup.FormattingEnabled = true;
            this.cboProductGroup.Location = new System.Drawing.Point(207, 13);
            this.cboProductGroup.Name = "cboProductGroup";
            this.cboProductGroup.Size = new System.Drawing.Size(156, 21);
            this.cboProductGroup.TabIndex = 64;
            this.cboProductGroup.Visible = false;
            // 
            // cboProductSingle
            // 
            this.cboProductSingle.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboProductSingle.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboProductSingle.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.cboProductSingle.FocusLostColor = System.Drawing.Color.White;
            this.cboProductSingle.FormattingEnabled = true;
            this.cboProductSingle.Location = new System.Drawing.Point(207, 28);
            this.cboProductSingle.Name = "cboProductSingle";
            this.cboProductSingle.Size = new System.Drawing.Size(156, 21);
            this.cboProductSingle.TabIndex = 6;
            // 
            // rdProductGroup
            // 
            this.rdProductGroup.AutoSize = true;
            this.rdProductGroup.Enabled = false;
            this.rdProductGroup.Location = new System.Drawing.Point(6, 26);
            this.rdProductGroup.Name = "rdProductGroup";
            this.rdProductGroup.Size = new System.Drawing.Size(94, 17);
            this.rdProductGroup.TabIndex = 0;
            this.rdProductGroup.Text = "Product Group";
            this.rdProductGroup.UseVisualStyleBackColor = true;
            this.rdProductGroup.Visible = false;
            this.rdProductGroup.CheckedChanged += new System.EventHandler(this.rdProductGroup_CheckedChanged);
            // 
            // rdProductSingle
            // 
            this.rdProductSingle.AutoSize = true;
            this.rdProductSingle.Checked = true;
            this.rdProductSingle.Location = new System.Drawing.Point(85, 31);
            this.rdProductSingle.Name = "rdProductSingle";
            this.rdProductSingle.Size = new System.Drawing.Size(94, 17);
            this.rdProductSingle.TabIndex = 0;
            this.rdProductSingle.TabStop = true;
            this.rdProductSingle.Text = "Single Product";
            this.rdProductSingle.UseVisualStyleBackColor = true;
            this.rdProductSingle.CheckedChanged += new System.EventHandler(this.rdProductSingle_CheckedChanged);
            // 
            // rdProductAll
            // 
            this.rdProductAll.AutoSize = true;
            this.rdProductAll.Location = new System.Drawing.Point(3, 9);
            this.rdProductAll.Name = "rdProductAll";
            this.rdProductAll.Size = new System.Drawing.Size(81, 17);
            this.rdProductAll.TabIndex = 0;
            this.rdProductAll.Text = "All Products";
            this.rdProductAll.UseVisualStyleBackColor = true;
            this.rdProductAll.Visible = false;
            // 
            // btnSelectAccClass
            // 
            this.btnSelectAccClass.Location = new System.Drawing.Point(102, 14);
            this.btnSelectAccClass.Name = "btnSelectAccClass";
            this.btnSelectAccClass.Size = new System.Drawing.Size(148, 29);
            this.btnSelectAccClass.TabIndex = 94;
            this.btnSelectAccClass.Text = "Select Account Class";
            this.btnSelectAccClass.UseVisualStyleBackColor = true;
            this.btnSelectAccClass.Click += new System.EventHandler(this.btnSelectAccClass_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox1.Controls.Add(this.btnSelectAccClass);
            this.groupBox1.Location = new System.Drawing.Point(0, 209);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(381, 54);
            this.groupBox1.TabIndex = 95;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Accounting Class";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.LightSteelBlue;
            this.panel1.Controls.Add(this.btnsavestate);
            this.panel1.Controls.Add(this.btnShow);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Location = new System.Drawing.Point(381, -1);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(101, 264);
            this.panel1.TabIndex = 96;
            // 
            // btnsavestate
            // 
            this.btnsavestate.Location = new System.Drawing.Point(9, 75);
            this.btnsavestate.Name = "btnsavestate";
            this.btnsavestate.Size = new System.Drawing.Size(83, 28);
            this.btnsavestate.TabIndex = 153;
            this.btnsavestate.Text = "Sa&ve State";
            this.btnsavestate.UseVisualStyleBackColor = true;
            this.btnsavestate.Click += new System.EventHandler(this.btnsavestate_Click);
            // 
            // frmStockLedgerSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(481, 266);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.grpProduct);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.chkDateRange);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmStockLedgerSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Stock Ledger Settings";
            this.Load += new System.EventHandler(this.frmStockLedgerSettings_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.grpProduct.ResumeLayout(false);
            this.grpProduct.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.MaskedTextBox txtFromDate;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.MaskedTextBox txtToDate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnShow;
        private System.Windows.Forms.CheckBox chkProject;
        private SComboBox cboProjectName;
        private SComboBox cboDepotwise;
        private System.Windows.Forms.CheckBox chkDepot;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox chkDateRange;
        private System.Windows.Forms.GroupBox grpProduct;
        private SComboBox cboProductGroup;
        private SComboBox cboProductSingle;
        private System.Windows.Forms.RadioButton rdProductGroup;
        private System.Windows.Forms.RadioButton rdProductSingle;
        private System.Windows.Forms.RadioButton rdProductAll;
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