using SComponents;
namespace Inventory
{
    partial class frmVATReportSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmVATReportSettings));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rdSummary = new System.Windows.Forms.RadioButton();
            this.rdDetail = new System.Windows.Forms.RadioButton();
            this.chkCollected = new System.Windows.Forms.CheckBox();
            this.chkPaid = new System.Windows.Forms.CheckBox();
            this.btneReturn = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnShow = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.grpDateRange = new System.Windows.Forms.GroupBox();
            this.btnToDate = new System.Windows.Forms.Button();
            this.btnFromDate = new System.Windows.Forms.Button();
            this.txtToDate = new SMaskedTextBox();
            this.txtFromDate = new SMaskedTextBox();
            this.btnSelectAccClass = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.cboProjectName = new SComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnsavestate = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cboMonths = new SComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.grpDateRange.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox1.Controls.Add(this.rdSummary);
            this.groupBox1.Controls.Add(this.rdDetail);
            this.groupBox1.Location = new System.Drawing.Point(3, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(345, 54);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // rdSummary
            // 
            this.rdSummary.AutoSize = true;
            this.rdSummary.Location = new System.Drawing.Point(32, 19);
            this.rdSummary.Name = "rdSummary";
            this.rdSummary.Size = new System.Drawing.Size(68, 17);
            this.rdSummary.TabIndex = 0;
            this.rdSummary.TabStop = true;
            this.rdSummary.Text = "Summary";
            this.rdSummary.UseVisualStyleBackColor = true;
            // 
            // rdDetail
            // 
            this.rdDetail.AutoSize = true;
            this.rdDetail.Location = new System.Drawing.Point(155, 19);
            this.rdDetail.Name = "rdDetail";
            this.rdDetail.Size = new System.Drawing.Size(52, 17);
            this.rdDetail.TabIndex = 0;
            this.rdDetail.TabStop = true;
            this.rdDetail.Text = "Detail";
            this.rdDetail.UseVisualStyleBackColor = true;
            // 
            // chkCollected
            // 
            this.chkCollected.AutoSize = true;
            this.chkCollected.Checked = true;
            this.chkCollected.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCollected.Location = new System.Drawing.Point(12, 278);
            this.chkCollected.Name = "chkCollected";
            this.chkCollected.Size = new System.Drawing.Size(73, 17);
            this.chkCollected.TabIndex = 2;
            this.chkCollected.Text = "Collected:";
            this.chkCollected.UseVisualStyleBackColor = true;
            // 
            // chkPaid
            // 
            this.chkPaid.AutoSize = true;
            this.chkPaid.Checked = true;
            this.chkPaid.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkPaid.Location = new System.Drawing.Point(105, 278);
            this.chkPaid.Name = "chkPaid";
            this.chkPaid.Size = new System.Drawing.Size(50, 17);
            this.chkPaid.TabIndex = 2;
            this.chkPaid.Text = "Paid:";
            this.chkPaid.UseVisualStyleBackColor = true;
            // 
            // btneReturn
            // 
            this.btneReturn.Location = new System.Drawing.Point(3, 74);
            this.btneReturn.Name = "btneReturn";
            this.btneReturn.Size = new System.Drawing.Size(82, 23);
            this.btneReturn.TabIndex = 3;
            this.btneReturn.Text = "e-Return";
            this.btneReturn.UseVisualStyleBackColor = true;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(3, 41);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(82, 23);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnShow
            // 
            this.btnShow.Location = new System.Drawing.Point(3, 10);
            this.btnShow.Name = "btnShow";
            this.btnShow.Size = new System.Drawing.Size(82, 23);
            this.btnShow.TabIndex = 3;
            this.btnShow.Text = "Show";
            this.btnShow.UseVisualStyleBackColor = true;
            this.btnShow.Click += new System.EventHandler(this.btnShow_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(37, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "From:";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(37, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(23, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "To:";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // grpDateRange
            // 
            this.grpDateRange.BackColor = System.Drawing.Color.WhiteSmoke;
            this.grpDateRange.Controls.Add(this.cboMonths);
            this.grpDateRange.Controls.Add(this.label3);
            this.grpDateRange.Controls.Add(this.btnToDate);
            this.grpDateRange.Controls.Add(this.btnFromDate);
            this.grpDateRange.Controls.Add(this.txtToDate);
            this.grpDateRange.Controls.Add(this.txtFromDate);
            this.grpDateRange.Controls.Add(this.label2);
            this.grpDateRange.Controls.Add(this.label1);
            this.grpDateRange.Location = new System.Drawing.Point(3, 59);
            this.grpDateRange.Name = "grpDateRange";
            this.grpDateRange.Size = new System.Drawing.Size(345, 98);
            this.grpDateRange.TabIndex = 1;
            this.grpDateRange.TabStop = false;
            this.grpDateRange.Text = "Date Range";
            // 
            // btnToDate
            // 
            this.btnToDate.Image = global::Inventory.Properties.Resources.dateIcon;
            this.btnToDate.Location = new System.Drawing.Point(211, 47);
            this.btnToDate.Name = "btnToDate";
            this.btnToDate.Size = new System.Drawing.Size(26, 23);
            this.btnToDate.TabIndex = 157;
            this.btnToDate.UseVisualStyleBackColor = true;
            this.btnToDate.Click += new System.EventHandler(this.btnToDate_Click);
            // 
            // btnFromDate
            // 
            this.btnFromDate.Image = global::Inventory.Properties.Resources.dateIcon;
            this.btnFromDate.Location = new System.Drawing.Point(211, 17);
            this.btnFromDate.Name = "btnFromDate";
            this.btnFromDate.Size = new System.Drawing.Size(26, 23);
            this.btnFromDate.TabIndex = 156;
            this.btnFromDate.UseVisualStyleBackColor = true;
            this.btnFromDate.Click += new System.EventHandler(this.btnFromDate_Click);
            // 
            // txtToDate
            // 
            this.txtToDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtToDate.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.txtToDate.FocusLostColor = System.Drawing.Color.White;
            this.txtToDate.Location = new System.Drawing.Point(89, 46);
            this.txtToDate.Mask = "##/##/####";
            this.txtToDate.Name = "txtToDate";
            this.txtToDate.Size = new System.Drawing.Size(92, 20);
            this.txtToDate.TabIndex = 3;
            this.txtToDate.MaskInputRejected += new System.Windows.Forms.MaskInputRejectedEventHandler(this.txtToDate_MaskInputRejected);
            // 
            // txtFromDate
            // 
            this.txtFromDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtFromDate.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.txtFromDate.FocusLostColor = System.Drawing.Color.White;
            this.txtFromDate.Location = new System.Drawing.Point(89, 20);
            this.txtFromDate.Mask = "##/##/####";
            this.txtFromDate.Name = "txtFromDate";
            this.txtFromDate.Size = new System.Drawing.Size(92, 20);
            this.txtFromDate.TabIndex = 2;
            this.txtFromDate.MaskInputRejected += new System.Windows.Forms.MaskInputRejectedEventHandler(this.txtFromDate_MaskInputRejected);
            // 
            // btnSelectAccClass
            // 
            this.btnSelectAccClass.Location = new System.Drawing.Point(186, 16);
            this.btnSelectAccClass.Name = "btnSelectAccClass";
            this.btnSelectAccClass.Size = new System.Drawing.Size(148, 23);
            this.btnSelectAccClass.TabIndex = 2;
            this.btnSelectAccClass.Text = "Select Account Class";
            this.btnSelectAccClass.UseVisualStyleBackColor = true;
            this.btnSelectAccClass.Click += new System.EventHandler(this.btnSelectAccClass_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox3.Controls.Add(this.cboProjectName);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Location = new System.Drawing.Point(5, 217);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(339, 52);
            this.groupBox3.TabIndex = 62;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Project";
            // 
            // cboProjectName
            // 
            this.cboProjectName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboProjectName.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.cboProjectName.FocusLostColor = System.Drawing.Color.White;
            this.cboProjectName.FormattingEnabled = true;
            this.cboProjectName.Location = new System.Drawing.Point(177, 22);
            this.cboProjectName.Name = "cboProjectName";
            this.cboProjectName.Size = new System.Drawing.Size(148, 21);
            this.cboProjectName.TabIndex = 59;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(125, 25);
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
            this.panel1.Controls.Add(this.btnClose);
            this.panel1.Controls.Add(this.btneReturn);
            this.panel1.Location = new System.Drawing.Point(349, -1);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(93, 270);
            this.panel1.TabIndex = 63;
            // 
            // btnsavestate
            // 
            this.btnsavestate.Location = new System.Drawing.Point(3, 107);
            this.btnsavestate.Name = "btnsavestate";
            this.btnsavestate.Size = new System.Drawing.Size(82, 23);
            this.btnsavestate.TabIndex = 66;
            this.btnsavestate.Text = "Save State";
            this.btnsavestate.UseVisualStyleBackColor = true;
            this.btnsavestate.Click += new System.EventHandler(this.btnsavestate_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox2.Controls.Add(this.btnSelectAccClass);
            this.groupBox2.Location = new System.Drawing.Point(3, 163);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(345, 48);
            this.groupBox2.TabIndex = 64;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Accounting Class";
            // 
            // cboMonths
            // 
            this.cboMonths.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMonths.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.cboMonths.FocusLostColor = System.Drawing.Color.White;
            this.cboMonths.FormattingEnabled = true;
            this.cboMonths.Location = new System.Drawing.Point(117, 72);
            this.cboMonths.Name = "cboMonths";
            this.cboMonths.Size = new System.Drawing.Size(120, 21);
            this.cboMonths.TabIndex = 159;
            this.cboMonths.SelectedIndexChanged += new System.EventHandler(this.cboMonths_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(37, 80);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 13);
            this.label3.TabIndex = 158;
            this.label3.Text = "End of Month";
            // 
            // frmVATReportSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(440, 297);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.grpDateRange);
            this.Controls.Add(this.chkPaid);
            this.Controls.Add(this.chkCollected);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "frmVATReportSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "VAT Report Settings";
            this.Load += new System.EventHandler(this.frmVATReportSettings_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmVATReportSettings_KeyDown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.grpDateRange.ResumeLayout(false);
            this.grpDateRange.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rdSummary;
        private System.Windows.Forms.RadioButton rdDetail;
        private System.Windows.Forms.CheckBox chkCollected;
        private System.Windows.Forms.CheckBox chkPaid;
        private System.Windows.Forms.Button btneReturn;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnShow;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private SMaskedTextBox txtFromDate;
        private SMaskedTextBox txtToDate;
        private System.Windows.Forms.GroupBox grpDateRange;
        private System.Windows.Forms.Button btnToDate;
        private System.Windows.Forms.Button btnFromDate;
        private System.Windows.Forms.Button btnSelectAccClass;
        private System.Windows.Forms.GroupBox groupBox3;
        private SComboBox cboProjectName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnsavestate;
        private SComboBox cboMonths;
        private System.Windows.Forms.Label label3;
    }
}