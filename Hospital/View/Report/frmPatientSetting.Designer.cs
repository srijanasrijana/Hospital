namespace Hospital.View.Report
{
    partial class frmPatientSetting
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPatientSetting));
            this.btnsavestate = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.chkDateRange = new System.Windows.Forms.CheckBox();
            this.grpParty = new System.Windows.Forms.GroupBox();
            this.rdPatientAll = new System.Windows.Forms.RadioButton();
            this.cboPatientAll = new SComponents.SComboBox();
            this.btnShow = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtToDate = new System.Windows.Forms.MaskedTextBox();
            this.txtFromDate = new System.Windows.Forms.MaskedTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cboMonths = new SComponents.SComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnToDate = new System.Windows.Forms.Button();
            this.btnFromDate = new System.Windows.Forms.Button();
            this.rdpartySingle = new System.Windows.Forms.RadioButton();
            this.grpParty.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnsavestate
            // 
            this.btnsavestate.Location = new System.Drawing.Point(10, 72);
            this.btnsavestate.Name = "btnsavestate";
            this.btnsavestate.Size = new System.Drawing.Size(75, 24);
            this.btnsavestate.TabIndex = 151;
            this.btnsavestate.Text = "Save State";
            this.btnsavestate.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button2.Location = new System.Drawing.Point(10, 39);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 89;
            this.button2.Text = "&Cancel";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // chkDateRange
            // 
            this.chkDateRange.AutoSize = true;
            this.chkDateRange.Location = new System.Drawing.Point(6, 95);
            this.chkDateRange.Name = "chkDateRange";
            this.chkDateRange.Size = new System.Drawing.Size(117, 17);
            this.chkDateRange.TabIndex = 100;
            this.chkDateRange.Text = "Select Date Range";
            this.chkDateRange.UseVisualStyleBackColor = true;
            this.chkDateRange.CheckedChanged += new System.EventHandler(this.chkDateRange_CheckedChanged);
            // 
            // grpParty
            // 
            this.grpParty.BackColor = System.Drawing.Color.WhiteSmoke;
            this.grpParty.Controls.Add(this.rdpartySingle);
            this.grpParty.Controls.Add(this.rdPatientAll);
            this.grpParty.Controls.Add(this.cboPatientAll);
            this.grpParty.Location = new System.Drawing.Point(6, 7);
            this.grpParty.Name = "grpParty";
            this.grpParty.Size = new System.Drawing.Size(348, 82);
            this.grpParty.TabIndex = 97;
            this.grpParty.TabStop = false;
            this.grpParty.Enter += new System.EventHandler(this.grpParty_Enter);
            // 
            // rdPatientAll
            // 
            this.rdPatientAll.AutoSize = true;
            this.rdPatientAll.Location = new System.Drawing.Point(12, 19);
            this.rdPatientAll.Name = "rdPatientAll";
            this.rdPatientAll.Size = new System.Drawing.Size(75, 17);
            this.rdPatientAll.TabIndex = 1;
            this.rdPatientAll.Text = "All Partient";
            this.rdPatientAll.UseVisualStyleBackColor = true;
            this.rdPatientAll.CheckedChanged += new System.EventHandler(this.rdPatientAll_CheckedChanged);
            // 
            // cboPatientAll
            // 
            this.cboPatientAll.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboPatientAll.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboPatientAll.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPatientAll.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.cboPatientAll.FocusLostColor = System.Drawing.Color.White;
            this.cboPatientAll.FormattingEnabled = true;
            this.cboPatientAll.Location = new System.Drawing.Point(187, 38);
            this.cboPatientAll.Name = "cboPatientAll";
            this.cboPatientAll.Size = new System.Drawing.Size(148, 21);
            this.cboPatientAll.TabIndex = 81;
            // 
            // btnShow
            // 
            this.btnShow.Location = new System.Drawing.Point(10, 8);
            this.btnShow.Name = "btnShow";
            this.btnShow.Size = new System.Drawing.Size(75, 23);
            this.btnShow.TabIndex = 88;
            this.btnShow.Text = "Show";
            this.btnShow.UseVisualStyleBackColor = true;
            this.btnShow.Click += new System.EventHandler(this.btnShow_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.LightSteelBlue;
            this.panel1.Controls.Add(this.btnsavestate);
            this.panel1.Controls.Add(this.btnShow);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Location = new System.Drawing.Point(378, 1);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(92, 184);
            this.panel1.TabIndex = 102;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(33, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "From:";
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
            // txtToDate
            // 
            this.txtToDate.Location = new System.Drawing.Point(219, 16);
            this.txtToDate.Mask = "##/##/####";
            this.txtToDate.Name = "txtToDate";
            this.txtToDate.Size = new System.Drawing.Size(121, 20);
            this.txtToDate.TabIndex = 7;
            // 
            // txtFromDate
            // 
            this.txtFromDate.Location = new System.Drawing.Point(47, 16);
            this.txtFromDate.Mask = "##/##/####";
            this.txtFromDate.Name = "txtFromDate";
            this.txtFromDate.Size = new System.Drawing.Size(104, 20);
            this.txtFromDate.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 13);
            this.label2.TabIndex = 62;
            this.label2.Text = "End of Month";
            // 
            // cboMonths
            // 
            this.cboMonths.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMonths.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.cboMonths.FocusLostColor = System.Drawing.Color.White;
            this.cboMonths.FormattingEnabled = true;
            this.cboMonths.Location = new System.Drawing.Point(74, 40);
            this.cboMonths.Name = "cboMonths";
            this.cboMonths.Size = new System.Drawing.Size(108, 21);
            this.cboMonths.TabIndex = 63;
            this.cboMonths.SelectedIndexChanged += new System.EventHandler(this.cboMonths_SelectedIndexChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox2.Controls.Add(this.btnToDate);
            this.groupBox2.Controls.Add(this.btnFromDate);
            this.groupBox2.Controls.Add(this.cboMonths);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.txtFromDate);
            this.groupBox2.Controls.Add(this.txtToDate);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Location = new System.Drawing.Point(1, 113);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(375, 72);
            this.groupBox2.TabIndex = 98;
            this.groupBox2.TabStop = false;
            // 
            // btnToDate
            // 
            this.btnToDate.Image = ((System.Drawing.Image)(resources.GetObject("btnToDate.Image")));
            this.btnToDate.Location = new System.Drawing.Point(346, 14);
            this.btnToDate.Name = "btnToDate";
            this.btnToDate.Size = new System.Drawing.Size(26, 23);
            this.btnToDate.TabIndex = 153;
            this.btnToDate.UseVisualStyleBackColor = true;
            this.btnToDate.Click += new System.EventHandler(this.btnToDate_Click);
            // 
            // btnFromDate
            // 
            this.btnFromDate.Image = ((System.Drawing.Image)(resources.GetObject("btnFromDate.Image")));
            this.btnFromDate.Location = new System.Drawing.Point(153, 13);
            this.btnFromDate.Name = "btnFromDate";
            this.btnFromDate.Size = new System.Drawing.Size(26, 23);
            this.btnFromDate.TabIndex = 152;
            this.btnFromDate.UseVisualStyleBackColor = true;
            this.btnFromDate.Click += new System.EventHandler(this.btnFromDate_Click);
            // 
            // rdpartySingle
            // 
            this.rdpartySingle.AutoSize = true;
            this.rdpartySingle.Location = new System.Drawing.Point(12, 42);
            this.rdpartySingle.Name = "rdpartySingle";
            this.rdpartySingle.Size = new System.Drawing.Size(90, 17);
            this.rdpartySingle.TabIndex = 85;
            this.rdpartySingle.Text = "Single Patient";
            this.rdpartySingle.UseVisualStyleBackColor = true;
            this.rdpartySingle.CheckedChanged += new System.EventHandler(this.rdpartySingle_CheckedChanged);
            // 
            // frmPatientSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(471, 196);
            this.Controls.Add(this.chkDateRange);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.grpParty);
            this.Controls.Add(this.panel1);
            this.MaximizeBox = false;
            this.Name = "frmPatientSetting";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmPatientSetting";
            this.Load += new System.EventHandler(this.frmPatientSetting_Load);
            this.grpParty.ResumeLayout(false);
            this.grpParty.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnsavestate;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.CheckBox chkDateRange;
        private System.Windows.Forms.GroupBox grpParty;
        private System.Windows.Forms.RadioButton rdPatientAll;
        private SComponents.SComboBox cboPatientAll;
        private System.Windows.Forms.Button btnShow;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnFromDate;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.MaskedTextBox txtToDate;
        private System.Windows.Forms.MaskedTextBox txtFromDate;
        private System.Windows.Forms.Label label2;
        private SComponents.SComboBox cboMonths;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnToDate;
        private System.Windows.Forms.RadioButton rdpartySingle;
    }
}