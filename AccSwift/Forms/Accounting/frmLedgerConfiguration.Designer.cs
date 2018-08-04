using SComponents;
namespace Inventory
{
    partial class frmLedgerConfiguration
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmLedgerConfiguration));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabLedger = new System.Windows.Forms.TabPage();
            this.btnSaveLedgerFormat = new System.Windows.Forms.Button();
            this.lblLedgerCodeFormat = new System.Windows.Forms.Label();
            this.chkLdrCompulsory = new System.Windows.Forms.CheckBox();
            this.btnLdrNumberingFormat = new System.Windows.Forms.Button();
            this.label26 = new System.Windows.Forms.Label();
            this.cmbLdrNumberingType = new SComboBox();
            this.tabGroup = new System.Windows.Forms.TabPage();
            this.btnGrpSave = new System.Windows.Forms.Button();
            this.lblGrpFormat = new System.Windows.Forms.Label();
            this.cboGrpCompulsory = new System.Windows.Forms.CheckBox();
            this.btnGrpNumberingFormat = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbGrpNumberingType = new SComboBox();
            this.tabControl1.SuspendLayout();
            this.tabLedger.SuspendLayout();
            this.tabGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabLedger);
            this.tabControl1.Controls.Add(this.tabGroup);
            this.tabControl1.Location = new System.Drawing.Point(11, 20);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(359, 225);
            this.tabControl1.TabIndex = 2;
            // 
            // tabLedger
            // 
            this.tabLedger.Controls.Add(this.btnSaveLedgerFormat);
            this.tabLedger.Controls.Add(this.lblLedgerCodeFormat);
            this.tabLedger.Controls.Add(this.chkLdrCompulsory);
            this.tabLedger.Controls.Add(this.btnLdrNumberingFormat);
            this.tabLedger.Controls.Add(this.label26);
            this.tabLedger.Controls.Add(this.cmbLdrNumberingType);
            this.tabLedger.Location = new System.Drawing.Point(4, 22);
            this.tabLedger.Name = "tabLedger";
            this.tabLedger.Padding = new System.Windows.Forms.Padding(3);
            this.tabLedger.Size = new System.Drawing.Size(351, 199);
            this.tabLedger.TabIndex = 0;
            this.tabLedger.Text = "Ledger";
            this.tabLedger.UseVisualStyleBackColor = true;
            // 
            // btnSaveLedgerFormat
            // 
            this.btnSaveLedgerFormat.Location = new System.Drawing.Point(120, 152);
            this.btnSaveLedgerFormat.Name = "btnSaveLedgerFormat";
            this.btnSaveLedgerFormat.Size = new System.Drawing.Size(75, 23);
            this.btnSaveLedgerFormat.TabIndex = 19;
            this.btnSaveLedgerFormat.Text = "Save";
            this.btnSaveLedgerFormat.UseVisualStyleBackColor = true;
            this.btnSaveLedgerFormat.Click += new System.EventHandler(this.btnSaveLedgerFormat_Click);
            // 
            // lblLedgerCodeFormat
            // 
            this.lblLedgerCodeFormat.AutoSize = true;
            this.lblLedgerCodeFormat.Location = new System.Drawing.Point(141, 106);
            this.lblLedgerCodeFormat.Name = "lblLedgerCodeFormat";
            this.lblLedgerCodeFormat.Size = new System.Drawing.Size(82, 13);
            this.lblLedgerCodeFormat.TabIndex = 18;
            this.lblLedgerCodeFormat.Text = "LDR-001-CUST";
            // 
            // chkLdrCompulsory
            // 
            this.chkLdrCompulsory.AutoSize = true;
            this.chkLdrCompulsory.Checked = true;
            this.chkLdrCompulsory.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkLdrCompulsory.Enabled = false;
            this.chkLdrCompulsory.Location = new System.Drawing.Point(115, 64);
            this.chkLdrCompulsory.Name = "chkLdrCompulsory";
            this.chkLdrCompulsory.Size = new System.Drawing.Size(80, 17);
            this.chkLdrCompulsory.TabIndex = 17;
            this.chkLdrCompulsory.Text = "Compulsory";
            this.chkLdrCompulsory.UseVisualStyleBackColor = true;
            // 
            // btnLdrNumberingFormat
            // 
            this.btnLdrNumberingFormat.Enabled = false;
            this.btnLdrNumberingFormat.Location = new System.Drawing.Point(18, 101);
            this.btnLdrNumberingFormat.Name = "btnLdrNumberingFormat";
            this.btnLdrNumberingFormat.Size = new System.Drawing.Size(107, 23);
            this.btnLdrNumberingFormat.TabIndex = 15;
            this.btnLdrNumberingFormat.Text = "Numbering Format";
            this.btnLdrNumberingFormat.UseVisualStyleBackColor = true;
            this.btnLdrNumberingFormat.Click += new System.EventHandler(this.btnNumberingFormat_Click);
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(15, 25);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(85, 13);
            this.label26.TabIndex = 16;
            this.label26.Text = "Numbering Type\r\n";
            // 
            // cmbLdrNumberingType
            // 
            this.cmbLdrNumberingType.BackColor = System.Drawing.Color.White;
            this.cmbLdrNumberingType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLdrNumberingType.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.cmbLdrNumberingType.FocusLostColor = System.Drawing.Color.White;
            this.cmbLdrNumberingType.FormattingEnabled = true;
            this.cmbLdrNumberingType.Location = new System.Drawing.Point(115, 22);
            this.cmbLdrNumberingType.Name = "cmbLdrNumberingType";
            this.cmbLdrNumberingType.Size = new System.Drawing.Size(141, 21);
            this.cmbLdrNumberingType.TabIndex = 14;
            this.cmbLdrNumberingType.SelectedIndexChanged += new System.EventHandler(this.cmbNumberingType_SelectedIndexChanged);
            // 
            // tabGroup
            // 
            this.tabGroup.Controls.Add(this.btnGrpSave);
            this.tabGroup.Controls.Add(this.lblGrpFormat);
            this.tabGroup.Controls.Add(this.cboGrpCompulsory);
            this.tabGroup.Controls.Add(this.btnGrpNumberingFormat);
            this.tabGroup.Controls.Add(this.label2);
            this.tabGroup.Controls.Add(this.cmbGrpNumberingType);
            this.tabGroup.Location = new System.Drawing.Point(4, 22);
            this.tabGroup.Name = "tabGroup";
            this.tabGroup.Padding = new System.Windows.Forms.Padding(3);
            this.tabGroup.Size = new System.Drawing.Size(351, 199);
            this.tabGroup.TabIndex = 1;
            this.tabGroup.Text = "Group";
            this.tabGroup.UseVisualStyleBackColor = true;
            // 
            // btnGrpSave
            // 
            this.btnGrpSave.Location = new System.Drawing.Point(120, 157);
            this.btnGrpSave.Name = "btnGrpSave";
            this.btnGrpSave.Size = new System.Drawing.Size(75, 23);
            this.btnGrpSave.TabIndex = 25;
            this.btnGrpSave.Text = "Save";
            this.btnGrpSave.UseVisualStyleBackColor = true;
            this.btnGrpSave.Click += new System.EventHandler(this.btnGrpSave_Click);
            // 
            // lblGrpFormat
            // 
            this.lblGrpFormat.AutoSize = true;
            this.lblGrpFormat.Location = new System.Drawing.Point(160, 105);
            this.lblGrpFormat.Name = "lblGrpFormat";
            this.lblGrpFormat.Size = new System.Drawing.Size(82, 13);
            this.lblGrpFormat.TabIndex = 24;
            this.lblGrpFormat.Text = "LDR-001-CUST";
            // 
            // cboGrpCompulsory
            // 
            this.cboGrpCompulsory.AutoSize = true;
            this.cboGrpCompulsory.Checked = true;
            this.cboGrpCompulsory.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cboGrpCompulsory.Enabled = false;
            this.cboGrpCompulsory.Location = new System.Drawing.Point(120, 62);
            this.cboGrpCompulsory.Name = "cboGrpCompulsory";
            this.cboGrpCompulsory.Size = new System.Drawing.Size(80, 17);
            this.cboGrpCompulsory.TabIndex = 23;
            this.cboGrpCompulsory.Text = "Compulsory";
            this.cboGrpCompulsory.UseVisualStyleBackColor = true;
            // 
            // btnGrpNumberingFormat
            // 
            this.btnGrpNumberingFormat.Enabled = false;
            this.btnGrpNumberingFormat.Location = new System.Drawing.Point(28, 100);
            this.btnGrpNumberingFormat.Name = "btnGrpNumberingFormat";
            this.btnGrpNumberingFormat.Size = new System.Drawing.Size(107, 23);
            this.btnGrpNumberingFormat.TabIndex = 21;
            this.btnGrpNumberingFormat.Text = "Numbering Format";
            this.btnGrpNumberingFormat.UseVisualStyleBackColor = true;
            this.btnGrpNumberingFormat.Click += new System.EventHandler(this.btnGrpNumberingFormat_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 13);
            this.label2.TabIndex = 22;
            this.label2.Text = "Numbering Type\r\n";
            // 
            // cmbGrpNumberingType
            // 
            this.cmbGrpNumberingType.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.cmbGrpNumberingType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGrpNumberingType.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.cmbGrpNumberingType.FocusLostColor = System.Drawing.Color.White;
            this.cmbGrpNumberingType.FormattingEnabled = true;
            this.cmbGrpNumberingType.Location = new System.Drawing.Point(115, 21);
            this.cmbGrpNumberingType.Name = "cmbGrpNumberingType";
            this.cmbGrpNumberingType.Size = new System.Drawing.Size(141, 21);
            this.cmbGrpNumberingType.TabIndex = 20;
            this.cmbGrpNumberingType.SelectedIndexChanged += new System.EventHandler(this.cmbGrpNumberingType_SelectedIndexChanged);
            // 
            // frmLedgerConfiguration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(399, 278);
            this.Controls.Add(this.tabControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmLedgerConfiguration";
            this.Text = "Ledger Configuration";
            this.Load += new System.EventHandler(this.frmLedgerConfiguration_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabLedger.ResumeLayout(false);
            this.tabLedger.PerformLayout();
            this.tabGroup.ResumeLayout(false);
            this.tabGroup.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabLedger;
        private System.Windows.Forms.TabPage tabGroup;
        private System.Windows.Forms.Button btnSaveLedgerFormat;
        private System.Windows.Forms.Label lblLedgerCodeFormat;
        private System.Windows.Forms.CheckBox chkLdrCompulsory;
        private System.Windows.Forms.Button btnLdrNumberingFormat;
        private System.Windows.Forms.Label label26;
        private SComboBox cmbLdrNumberingType;
        private System.Windows.Forms.Button btnGrpSave;
        private System.Windows.Forms.Label lblGrpFormat;
        private System.Windows.Forms.CheckBox cboGrpCompulsory;
        private System.Windows.Forms.Button btnGrpNumberingFormat;
        private System.Windows.Forms.Label label2;
        private SComboBox cmbGrpNumberingType;
    }
}