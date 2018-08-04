using SComponents;
namespace Inventory
{
    partial class frmProfitLossAccSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmProfitLossAccSettings));
            this.chkShowSecLevGrpDet = new System.Windows.Forms.CheckBox();
            this.chkDate = new System.Windows.Forms.CheckBox();
            this.btnShow = new System.Windows.Forms.Button();
            this.txtToDate = new System.Windows.Forms.MaskedTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.chkShowZeroBal = new System.Windows.Forms.CheckBox();
            this.rdbtnDetail = new System.Windows.Forms.RadioButton();
            this.label4 = new System.Windows.Forms.Label();
            this.rdbtnSummary = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rdbtnVertical = new System.Windows.Forms.RadioButton();
            this.btnSelectAccClass = new System.Windows.Forms.Button();
            this.rbtnTformate = new System.Windows.Forms.RadioButton();
            this.txtRate = new STextBox();
            this.cboCurrency = new SComboBox();
            this.cboProjectName = new SComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnToday = new System.Windows.Forms.Button();
            this.btnDate = new System.Windows.Forms.Button();
            this.cboMonths = new SComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnsavestate = new System.Windows.Forms.Button();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkShowSecLevGrpDet
            // 
            this.chkShowSecLevGrpDet.AutoSize = true;
            this.chkShowSecLevGrpDet.Location = new System.Drawing.Point(204, 15);
            this.chkShowSecLevGrpDet.Name = "chkShowSecLevGrpDet";
            this.chkShowSecLevGrpDet.Size = new System.Drawing.Size(189, 17);
            this.chkShowSecLevGrpDet.TabIndex = 47;
            this.chkShowSecLevGrpDet.Text = "Show Second Level Group Details";
            this.chkShowSecLevGrpDet.UseVisualStyleBackColor = true;
            this.chkShowSecLevGrpDet.KeyDown += new System.Windows.Forms.KeyEventHandler(this.chkShowSecLevGrpDet_KeyDown);
            // 
            // chkDate
            // 
            this.chkDate.AutoSize = true;
            this.chkDate.Checked = true;
            this.chkDate.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDate.Location = new System.Drawing.Point(0, 0);
            this.chkDate.Name = "chkDate";
            this.chkDate.Size = new System.Drawing.Size(93, 17);
            this.chkDate.TabIndex = 35;
            this.chkDate.Text = "Click For Date";
            this.chkDate.UseVisualStyleBackColor = true;
            this.chkDate.CheckedChanged += new System.EventHandler(this.chkDate_CheckedChanged);
            // 
            // btnShow
            // 
            this.btnShow.Location = new System.Drawing.Point(9, 17);
            this.btnShow.Name = "btnShow";
            this.btnShow.Size = new System.Drawing.Size(75, 23);
            this.btnShow.TabIndex = 36;
            this.btnShow.Text = "Show";
            this.btnShow.UseVisualStyleBackColor = true;
            this.btnShow.Click += new System.EventHandler(this.btnShow_Click);
            // 
            // txtToDate
            // 
            this.txtToDate.Location = new System.Drawing.Point(179, 32);
            this.txtToDate.Mask = "##/##/####";
            this.txtToDate.Name = "txtToDate";
            this.txtToDate.Size = new System.Drawing.Size(126, 20);
            this.txtToDate.TabIndex = 7;
            this.txtToDate.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtToDate_KeyDown);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(195, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "End of Date:";
            // 
            // chkShowZeroBal
            // 
            this.chkShowZeroBal.AutoSize = true;
            this.chkShowZeroBal.Location = new System.Drawing.Point(21, 15);
            this.chkShowZeroBal.Name = "chkShowZeroBal";
            this.chkShowZeroBal.Size = new System.Drawing.Size(120, 17);
            this.chkShowZeroBal.TabIndex = 44;
            this.chkShowZeroBal.Text = "Show Zero Balance";
            this.chkShowZeroBal.UseVisualStyleBackColor = true;
            this.chkShowZeroBal.KeyDown += new System.Windows.Forms.KeyEventHandler(this.chkShowZeroBal_KeyDown);
            // 
            // rdbtnDetail
            // 
            this.rdbtnDetail.AutoSize = true;
            this.rdbtnDetail.Location = new System.Drawing.Point(108, 20);
            this.rdbtnDetail.Name = "rdbtnDetail";
            this.rdbtnDetail.Size = new System.Drawing.Size(52, 17);
            this.rdbtnDetail.TabIndex = 43;
            this.rdbtnDetail.TabStop = true;
            this.rdbtnDetail.Text = "Detail";
            this.rdbtnDetail.UseVisualStyleBackColor = true;
            this.rdbtnDetail.KeyDown += new System.Windows.Forms.KeyEventHandler(this.rdbtnDetail_KeyDown);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(275, 21);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(30, 13);
            this.label4.TabIndex = 40;
            this.label4.Text = "Rate";
            // 
            // rdbtnSummary
            // 
            this.rdbtnSummary.AutoSize = true;
            this.rdbtnSummary.Location = new System.Drawing.Point(25, 20);
            this.rdbtnSummary.Name = "rdbtnSummary";
            this.rdbtnSummary.Size = new System.Drawing.Size(68, 17);
            this.rdbtnSummary.TabIndex = 42;
            this.rdbtnSummary.TabStop = true;
            this.rdbtnSummary.Text = "Summary";
            this.rdbtnSummary.UseVisualStyleBackColor = true;
            this.rdbtnSummary.KeyDown += new System.Windows.Forms.KeyEventHandler(this.rdbtnSummary_KeyDown);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 20);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 13);
            this.label3.TabIndex = 38;
            this.label3.Text = "Currency";
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox2.Controls.Add(this.rdbtnVertical);
            this.groupBox2.Controls.Add(this.btnSelectAccClass);
            this.groupBox2.Controls.Add(this.rbtnTformate);
            this.groupBox2.Location = new System.Drawing.Point(2, 80);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(407, 49);
            this.groupBox2.TabIndex = 49;
            this.groupBox2.TabStop = false;
            // 
            // rdbtnVertical
            // 
            this.rdbtnVertical.AutoSize = true;
            this.rdbtnVertical.Location = new System.Drawing.Point(158, 19);
            this.rdbtnVertical.Name = "rdbtnVertical";
            this.rdbtnVertical.Size = new System.Drawing.Size(60, 17);
            this.rdbtnVertical.TabIndex = 1;
            this.rdbtnVertical.TabStop = true;
            this.rdbtnVertical.Text = "Vertical";
            this.rdbtnVertical.UseVisualStyleBackColor = true;
            this.rdbtnVertical.KeyDown += new System.Windows.Forms.KeyEventHandler(this.rdbtnVertical_KeyDown);
            // 
            // btnSelectAccClass
            // 
            this.btnSelectAccClass.Location = new System.Drawing.Point(273, 16);
            this.btnSelectAccClass.Name = "btnSelectAccClass";
            this.btnSelectAccClass.Size = new System.Drawing.Size(120, 23);
            this.btnSelectAccClass.TabIndex = 52;
            this.btnSelectAccClass.Text = "Select Account Class";
            this.btnSelectAccClass.UseVisualStyleBackColor = true;
            this.btnSelectAccClass.Click += new System.EventHandler(this.btnSelectAccClass_Click);
            // 
            // rbtnTformate
            // 
            this.rbtnTformate.AutoSize = true;
            this.rbtnTformate.Location = new System.Drawing.Point(21, 19);
            this.rbtnTformate.Name = "rbtnTformate";
            this.rbtnTformate.Size = new System.Drawing.Size(67, 17);
            this.rbtnTformate.TabIndex = 0;
            this.rbtnTformate.TabStop = true;
            this.rbtnTformate.Text = "T-Format";
            this.rbtnTformate.UseVisualStyleBackColor = true;
            this.rbtnTformate.KeyDown += new System.Windows.Forms.KeyEventHandler(this.rbtnTformate_KeyDown);
            // 
            // txtRate
            // 
            this.txtRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtRate.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtRate.FocusLostColor = System.Drawing.Color.White;
            this.txtRate.Location = new System.Drawing.Point(339, 16);
            this.txtRate.Name = "txtRate";
            this.txtRate.Size = new System.Drawing.Size(54, 20);
            this.txtRate.TabIndex = 41;
            this.txtRate.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtRate_KeyDown);
            // 
            // cboCurrency
            // 
            this.cboCurrency.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.cboCurrency.FocusLostColor = System.Drawing.Color.White;
            this.cboCurrency.FormattingEnabled = true;
            this.cboCurrency.Location = new System.Drawing.Point(93, 15);
            this.cboCurrency.Name = "cboCurrency";
            this.cboCurrency.Size = new System.Drawing.Size(125, 21);
            this.cboCurrency.TabIndex = 39;
            this.cboCurrency.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cboCurrency_KeyDown);
            // 
            // cboProjectName
            // 
            this.cboProjectName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboProjectName.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.cboProjectName.FocusLostColor = System.Drawing.Color.White;
            this.cboProjectName.FormattingEnabled = true;
            this.cboProjectName.Location = new System.Drawing.Point(51, 16);
            this.cboProjectName.Name = "cboProjectName";
            this.cboProjectName.Size = new System.Drawing.Size(160, 21);
            this.cboProjectName.TabIndex = 51;
            this.cboProjectName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cboProjectName_KeyDown);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 22);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(46, 13);
            this.label5.TabIndex = 50;
            this.label5.Text = "Project: ";
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox3.Controls.Add(this.btnToday);
            this.groupBox3.Controls.Add(this.btnDate);
            this.groupBox3.Controls.Add(this.cboMonths);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.chkDate);
            this.groupBox3.Controls.Add(this.txtToDate);
            this.groupBox3.Location = new System.Drawing.Point(0, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(407, 66);
            this.groupBox3.TabIndex = 53;
            this.groupBox3.TabStop = false;
            // 
            // btnToday
            // 
            this.btnToday.Location = new System.Drawing.Point(337, 29);
            this.btnToday.Name = "btnToday";
            this.btnToday.Size = new System.Drawing.Size(56, 25);
            this.btnToday.TabIndex = 65;
            this.btnToday.Text = "&Today";
            this.btnToday.UseVisualStyleBackColor = true;
            this.btnToday.Click += new System.EventHandler(this.btnToday_Click);
            // 
            // btnDate
            // 
            this.btnDate.Image = global::Inventory.Properties.Resources.dateIcon;
            this.btnDate.Location = new System.Drawing.Point(311, 29);
            this.btnDate.Name = "btnDate";
            this.btnDate.Size = new System.Drawing.Size(26, 25);
            this.btnDate.TabIndex = 64;
            this.btnDate.UseVisualStyleBackColor = true;
            this.btnDate.Click += new System.EventHandler(this.btnDate_Click);
            // 
            // cboMonths
            // 
            this.cboMonths.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMonths.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.cboMonths.FocusLostColor = System.Drawing.Color.White;
            this.cboMonths.FormattingEnabled = true;
            this.cboMonths.Location = new System.Drawing.Point(13, 32);
            this.cboMonths.Name = "cboMonths";
            this.cboMonths.Size = new System.Drawing.Size(139, 21);
            this.cboMonths.TabIndex = 63;
            this.cboMonths.SelectedIndexChanged += new System.EventHandler(this.cboMonths_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(43, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 13);
            this.label1.TabIndex = 62;
            this.label1.Text = "End of Month";
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.cboCurrency);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.txtRate);
            this.groupBox1.Location = new System.Drawing.Point(2, 130);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(407, 44);
            this.groupBox1.TabIndex = 57;
            this.groupBox1.TabStop = false;
            // 
            // groupBox4
            // 
            this.groupBox4.BackColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox4.Controls.Add(this.rdbtnSummary);
            this.groupBox4.Controls.Add(this.rdbtnDetail);
            this.groupBox4.Location = new System.Drawing.Point(2, 172);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(173, 48);
            this.groupBox4.TabIndex = 58;
            this.groupBox4.TabStop = false;
            // 
            // groupBox5
            // 
            this.groupBox5.BackColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox5.Controls.Add(this.label5);
            this.groupBox5.Controls.Add(this.cboProjectName);
            this.groupBox5.Location = new System.Drawing.Point(184, 172);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(225, 48);
            this.groupBox5.TabIndex = 59;
            this.groupBox5.TabStop = false;
            // 
            // groupBox6
            // 
            this.groupBox6.BackColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox6.Controls.Add(this.chkShowZeroBal);
            this.groupBox6.Controls.Add(this.chkShowSecLevGrpDet);
            this.groupBox6.Location = new System.Drawing.Point(2, 219);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(407, 45);
            this.groupBox6.TabIndex = 60;
            this.groupBox6.TabStop = false;
            // 
            // btnCancel
            // 
            this.btnCancel.Image = global::Inventory.Properties.Resources.gnome_window_close;
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(10, 49);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 62;
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
            this.panel1.Location = new System.Drawing.Point(408, -2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(92, 266);
            this.panel1.TabIndex = 63;
            // 
            // btnsavestate
            // 
            this.btnsavestate.Location = new System.Drawing.Point(9, 81);
            this.btnsavestate.Name = "btnsavestate";
            this.btnsavestate.Size = new System.Drawing.Size(76, 23);
            this.btnsavestate.TabIndex = 64;
            this.btnsavestate.Text = "Save State";
            this.btnsavestate.UseVisualStyleBackColor = true;
            this.btnsavestate.Click += new System.EventHandler(this.btnsavestate_Click);
            // 
            // frmProfitLossAccSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(499, 276);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox6);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "frmProfitLossAccSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Profit & Loss Account Settings";
            this.Load += new System.EventHandler(this.frmProfitLossAccSettings_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmProfitLossAccSettings_KeyDown);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox chkShowSecLevGrpDet;
        private System.Windows.Forms.CheckBox chkDate;
        private STextBox txtRate;
        private System.Windows.Forms.Button btnShow;
        private System.Windows.Forms.MaskedTextBox txtToDate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chkShowZeroBal;
        private System.Windows.Forms.RadioButton rdbtnDetail;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.RadioButton rdbtnSummary;
        private SComboBox cboCurrency;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rdbtnVertical;
        private System.Windows.Forms.RadioButton rbtnTformate;
        private SComboBox cboProjectName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnSelectAccClass;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Button btnCancel;
        private SComboBox cboMonths;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnToday;
        private System.Windows.Forms.Button btnDate;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnsavestate;
    }
}