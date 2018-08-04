using SComponents;
namespace Inventory
{
    partial class frmTrialBalanceSetting
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmTrialBalanceSetting));
            this.btnShow = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.rdSummary = new System.Windows.Forms.RadioButton();
            this.rbtnDetail = new System.Windows.Forms.RadioButton();
            this.chkShowZeroBal = new System.Windows.Forms.CheckBox();
            this.chkShowSecLevGrpDet = new System.Windows.Forms.CheckBox();
            this.btnSelectAccClass = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbtnledgersonly = new System.Windows.Forms.RadioButton();
            this.rdAllGrps = new System.Windows.Forms.RadioButton();
            this.rdOnlyPrimaryGrps = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.chkDate = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnToday = new System.Windows.Forms.Button();
            this.btnDate = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.txtToDate = new System.Windows.Forms.MaskedTextBox();
            this.cboMonths = new SComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.txtRate = new STextBox();
            this.cboCurrency = new SComboBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.cboProjectName = new SComboBox();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnsavestate = new System.Windows.Forms.Button();
            this.chkshowpreviousyear = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnShow
            // 
            this.btnShow.Location = new System.Drawing.Point(8, 9);
            this.btnShow.Name = "btnShow";
            this.btnShow.Size = new System.Drawing.Size(80, 26);
            this.btnShow.TabIndex = 9;
            this.btnShow.Text = "&Show";
            this.btnShow.UseVisualStyleBackColor = true;
            this.btnShow.Click += new System.EventHandler(this.btnShow_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 19);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "Currency";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(251, 19);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(30, 13);
            this.label4.TabIndex = 13;
            this.label4.Text = "Rate";
            // 
            // rdSummary
            // 
            this.rdSummary.AutoSize = true;
            this.rdSummary.Location = new System.Drawing.Point(9, 15);
            this.rdSummary.Name = "rdSummary";
            this.rdSummary.Size = new System.Drawing.Size(68, 17);
            this.rdSummary.TabIndex = 15;
            this.rdSummary.TabStop = true;
            this.rdSummary.Text = "Summary";
            this.rdSummary.UseVisualStyleBackColor = true;
            this.rdSummary.KeyDown += new System.Windows.Forms.KeyEventHandler(this.rdSummary_KeyDown);
            // 
            // rbtnDetail
            // 
            this.rbtnDetail.AutoSize = true;
            this.rbtnDetail.Location = new System.Drawing.Point(86, 15);
            this.rbtnDetail.Name = "rbtnDetail";
            this.rbtnDetail.Size = new System.Drawing.Size(52, 17);
            this.rbtnDetail.TabIndex = 16;
            this.rbtnDetail.TabStop = true;
            this.rbtnDetail.Text = "Detail";
            this.rbtnDetail.UseVisualStyleBackColor = true;
            this.rbtnDetail.KeyDown += new System.Windows.Forms.KeyEventHandler(this.rbtnDetail_KeyDown);
            // 
            // chkShowZeroBal
            // 
            this.chkShowZeroBal.AutoSize = true;
            this.chkShowZeroBal.Location = new System.Drawing.Point(13, 14);
            this.chkShowZeroBal.Name = "chkShowZeroBal";
            this.chkShowZeroBal.Size = new System.Drawing.Size(120, 17);
            this.chkShowZeroBal.TabIndex = 17;
            this.chkShowZeroBal.Text = "Show Zero Balance";
            this.chkShowZeroBal.UseVisualStyleBackColor = true;
            this.chkShowZeroBal.KeyDown += new System.Windows.Forms.KeyEventHandler(this.chkShowZeroBal_KeyDown);
            // 
            // chkShowSecLevGrpDet
            // 
            this.chkShowSecLevGrpDet.AutoSize = true;
            this.chkShowSecLevGrpDet.Location = new System.Drawing.Point(173, 14);
            this.chkShowSecLevGrpDet.Name = "chkShowSecLevGrpDet";
            this.chkShowSecLevGrpDet.Size = new System.Drawing.Size(189, 17);
            this.chkShowSecLevGrpDet.TabIndex = 20;
            this.chkShowSecLevGrpDet.Text = "Show Second Level Group Details";
            this.chkShowSecLevGrpDet.UseVisualStyleBackColor = true;
            this.chkShowSecLevGrpDet.KeyDown += new System.Windows.Forms.KeyEventHandler(this.chkShowSecLevGrpDet_KeyDown);
            // 
            // btnSelectAccClass
            // 
            this.btnSelectAccClass.Location = new System.Drawing.Point(222, 10);
            this.btnSelectAccClass.Name = "btnSelectAccClass";
            this.btnSelectAccClass.Size = new System.Drawing.Size(141, 23);
            this.btnSelectAccClass.TabIndex = 35;
            this.btnSelectAccClass.Text = "Select Account Class";
            this.btnSelectAccClass.UseVisualStyleBackColor = true;
            this.btnSelectAccClass.Click += new System.EventHandler(this.btnSelectAccClass_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox1.Controls.Add(this.rbtnledgersonly);
            this.groupBox1.Controls.Add(this.rdAllGrps);
            this.groupBox1.Controls.Add(this.rdOnlyPrimaryGrps);
            this.groupBox1.Location = new System.Drawing.Point(1, 72);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(370, 48);
            this.groupBox1.TabIndex = 37;
            this.groupBox1.TabStop = false;
            // 
            // rbtnledgersonly
            // 
            this.rbtnledgersonly.AutoSize = true;
            this.rbtnledgersonly.Location = new System.Drawing.Point(272, 19);
            this.rbtnledgersonly.Name = "rbtnledgersonly";
            this.rbtnledgersonly.Size = new System.Drawing.Size(87, 17);
            this.rbtnledgersonly.TabIndex = 39;
            this.rbtnledgersonly.TabStop = true;
            this.rbtnledgersonly.Text = "Only Ledgers";
            this.rbtnledgersonly.UseVisualStyleBackColor = true;
            // 
            // rdAllGrps
            // 
            this.rdAllGrps.AutoSize = true;
            this.rdAllGrps.Location = new System.Drawing.Point(7, 19);
            this.rdAllGrps.Name = "rdAllGrps";
            this.rdAllGrps.Size = new System.Drawing.Size(73, 17);
            this.rdAllGrps.TabIndex = 38;
            this.rdAllGrps.TabStop = true;
            this.rdAllGrps.Text = "All Groups";
            this.rdAllGrps.UseVisualStyleBackColor = true;
            this.rdAllGrps.CheckedChanged += new System.EventHandler(this.rdAllGrps_CheckedChanged);
            this.rdAllGrps.KeyDown += new System.Windows.Forms.KeyEventHandler(this.rdAllGrps_KeyDown);
            // 
            // rdOnlyPrimaryGrps
            // 
            this.rdOnlyPrimaryGrps.AutoSize = true;
            this.rdOnlyPrimaryGrps.Location = new System.Drawing.Point(119, 19);
            this.rdOnlyPrimaryGrps.Name = "rdOnlyPrimaryGrps";
            this.rdOnlyPrimaryGrps.Size = new System.Drawing.Size(120, 17);
            this.rdOnlyPrimaryGrps.TabIndex = 37;
            this.rdOnlyPrimaryGrps.TabStop = true;
            this.rdOnlyPrimaryGrps.Text = "Only Primary Groups";
            this.rdOnlyPrimaryGrps.UseVisualStyleBackColor = true;
            this.rdOnlyPrimaryGrps.CheckedChanged += new System.EventHandler(this.rdOnlyPrimaryGrps_CheckedChanged);
            this.rdOnlyPrimaryGrps.KeyDown += new System.Windows.Forms.KeyEventHandler(this.rdOnlyPrimaryGrps_KeyDown);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 13);
            this.label2.TabIndex = 38;
            this.label2.Text = "Project: ";
            // 
            // chkDate
            // 
            this.chkDate.AutoSize = true;
            this.chkDate.Checked = true;
            this.chkDate.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDate.Location = new System.Drawing.Point(9, 0);
            this.chkDate.Name = "chkDate";
            this.chkDate.Size = new System.Drawing.Size(93, 17);
            this.chkDate.TabIndex = 40;
            this.chkDate.Text = "Click For Date";
            this.chkDate.UseVisualStyleBackColor = true;
            this.chkDate.CheckedChanged += new System.EventHandler(this.chkDate_CheckedChanged_1);
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox2.Controls.Add(this.btnToday);
            this.groupBox2.Controls.Add(this.btnDate);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.txtToDate);
            this.groupBox2.Controls.Add(this.cboMonths);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.chkDate);
            this.groupBox2.Location = new System.Drawing.Point(1, 2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(370, 70);
            this.groupBox2.TabIndex = 44;
            this.groupBox2.TabStop = false;
            // 
            // btnToday
            // 
            this.btnToday.Location = new System.Drawing.Point(301, 37);
            this.btnToday.Name = "btnToday";
            this.btnToday.Size = new System.Drawing.Size(56, 23);
            this.btnToday.TabIndex = 61;
            this.btnToday.Text = "&Today";
            this.btnToday.UseVisualStyleBackColor = true;
            this.btnToday.Click += new System.EventHandler(this.btnToday_Click);
            // 
            // btnDate
            // 
            this.btnDate.Image = global::Inventory.Properties.Resources.dateIcon;
            this.btnDate.Location = new System.Drawing.Point(266, 37);
            this.btnDate.Name = "btnDate";
            this.btnDate.Size = new System.Drawing.Size(26, 23);
            this.btnDate.TabIndex = 60;
            this.btnDate.UseVisualStyleBackColor = true;
            this.btnDate.Click += new System.EventHandler(this.btnDate_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(182, 22);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(72, 13);
            this.label6.TabIndex = 56;
            this.label6.Text = "At the End of:";
            // 
            // txtToDate
            // 
            this.txtToDate.Location = new System.Drawing.Point(185, 38);
            this.txtToDate.Mask = "##/##/####";
            this.txtToDate.Name = "txtToDate";
            this.txtToDate.Size = new System.Drawing.Size(74, 20);
            this.txtToDate.TabIndex = 57;
            // 
            // cboMonths
            // 
            this.cboMonths.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMonths.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.cboMonths.FocusLostColor = System.Drawing.Color.White;
            this.cboMonths.FormattingEnabled = true;
            this.cboMonths.Location = new System.Drawing.Point(38, 38);
            this.cboMonths.Name = "cboMonths";
            this.cboMonths.Size = new System.Drawing.Size(139, 21);
            this.cboMonths.TabIndex = 59;
            this.cboMonths.SelectedIndexChanged += new System.EventHandler(this.cboMonths_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(35, 22);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(71, 13);
            this.label5.TabIndex = 58;
            this.label5.Text = "End of Month";
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox3.Controls.Add(this.btnSelectAccClass);
            this.groupBox3.Location = new System.Drawing.Point(1, 120);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(369, 39);
            this.groupBox3.TabIndex = 45;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Accounting Class";
            // 
            // groupBox4
            // 
            this.groupBox4.BackColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox4.Controls.Add(this.txtRate);
            this.groupBox4.Controls.Add(this.label4);
            this.groupBox4.Controls.Add(this.cboCurrency);
            this.groupBox4.Controls.Add(this.label3);
            this.groupBox4.Location = new System.Drawing.Point(1, 203);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(370, 46);
            this.groupBox4.TabIndex = 46;
            this.groupBox4.TabStop = false;
            // 
            // txtRate
            // 
            this.txtRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtRate.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtRate.FocusLostColor = System.Drawing.Color.White;
            this.txtRate.Location = new System.Drawing.Point(286, 16);
            this.txtRate.Name = "txtRate";
            this.txtRate.Size = new System.Drawing.Size(76, 20);
            this.txtRate.TabIndex = 14;
            this.txtRate.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtRate_KeyDown);
            // 
            // cboCurrency
            // 
            this.cboCurrency.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.cboCurrency.FocusLostColor = System.Drawing.Color.White;
            this.cboCurrency.FormattingEnabled = true;
            this.cboCurrency.Location = new System.Drawing.Point(61, 16);
            this.cboCurrency.Name = "cboCurrency";
            this.cboCurrency.Size = new System.Drawing.Size(176, 21);
            this.cboCurrency.TabIndex = 12;
            this.cboCurrency.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cboCurrency_KeyDown);
            // 
            // groupBox5
            // 
            this.groupBox5.BackColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox5.Controls.Add(this.rdSummary);
            this.groupBox5.Controls.Add(this.rbtnDetail);
            this.groupBox5.Location = new System.Drawing.Point(1, 161);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(164, 38);
            this.groupBox5.TabIndex = 47;
            this.groupBox5.TabStop = false;
            // 
            // groupBox6
            // 
            this.groupBox6.BackColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox6.Controls.Add(this.cboProjectName);
            this.groupBox6.Controls.Add(this.label2);
            this.groupBox6.Location = new System.Drawing.Point(166, 161);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(203, 40);
            this.groupBox6.TabIndex = 48;
            this.groupBox6.TabStop = false;
            // 
            // cboProjectName
            // 
            this.cboProjectName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboProjectName.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.cboProjectName.FocusLostColor = System.Drawing.Color.White;
            this.cboProjectName.FormattingEnabled = true;
            this.cboProjectName.Location = new System.Drawing.Point(57, 13);
            this.cboProjectName.Name = "cboProjectName";
            this.cboProjectName.Size = new System.Drawing.Size(142, 21);
            this.cboProjectName.TabIndex = 39;
            this.cboProjectName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cboProjectName_KeyDown);
            // 
            // groupBox7
            // 
            this.groupBox7.BackColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox7.Controls.Add(this.chkShowSecLevGrpDet);
            this.groupBox7.Controls.Add(this.chkShowZeroBal);
            this.groupBox7.Location = new System.Drawing.Point(1, 251);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(370, 39);
            this.groupBox7.TabIndex = 49;
            this.groupBox7.TabStop = false;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(8, 41);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 26);
            this.btnCancel.TabIndex = 50;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.LightSteelBlue;
            this.panel1.Controls.Add(this.btnsavestate);
            this.panel1.Controls.Add(this.btnShow);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Location = new System.Drawing.Point(373, -1);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(94, 330);
            this.panel1.TabIndex = 51;
            // 
            // btnsavestate
            // 
            this.btnsavestate.Location = new System.Drawing.Point(8, 78);
            this.btnsavestate.Name = "btnsavestate";
            this.btnsavestate.Size = new System.Drawing.Size(79, 25);
            this.btnsavestate.TabIndex = 63;
            this.btnsavestate.Text = "Save State";
            this.btnsavestate.UseVisualStyleBackColor = true;
            this.btnsavestate.Click += new System.EventHandler(this.btnsavestate_Click);
            // 
            // chkshowpreviousyear
            // 
            this.chkshowpreviousyear.AutoSize = true;
            this.chkshowpreviousyear.Location = new System.Drawing.Point(14, 300);
            this.chkshowpreviousyear.Name = "chkshowpreviousyear";
            this.chkshowpreviousyear.Size = new System.Drawing.Size(122, 17);
            this.chkshowpreviousyear.TabIndex = 52;
            this.chkshowpreviousyear.Text = "Show Previous Year";
            this.chkshowpreviousyear.UseVisualStyleBackColor = true;
            // 
            // frmTrialBalanceSetting
            // 
            this.AcceptButton = this.btnShow;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(465, 327);
            this.Controls.Add(this.chkshowpreviousyear);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox7);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.ForeColor = System.Drawing.SystemColors.MenuText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "frmTrialBalanceSetting";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Trial Balance Settings";
            this.Load += new System.EventHandler(this.frmTrialBalanceSetting_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmTrialBalanceSetting_KeyDown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnShow;
        private System.Windows.Forms.Label label3;
        private SComboBox cboCurrency;
        private System.Windows.Forms.Label label4;
        private STextBox txtRate;
        private System.Windows.Forms.RadioButton rdSummary;
        private System.Windows.Forms.RadioButton rbtnDetail;
        private System.Windows.Forms.CheckBox chkShowZeroBal;
        private System.Windows.Forms.CheckBox chkShowSecLevGrpDet;
        private System.Windows.Forms.Button btnSelectAccClass;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rdAllGrps;
        private System.Windows.Forms.RadioButton rdOnlyPrimaryGrps;
        private SComboBox cboProjectName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chkDate;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnToday;
        private System.Windows.Forms.Button btnDate;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.MaskedTextBox txtToDate;
        private SComboBox cboMonths;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.RadioButton rbtnledgersonly;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnsavestate;
        private System.Windows.Forms.CheckBox chkshowpreviousyear;
    }
}