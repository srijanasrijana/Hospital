using SComponents;
namespace Inventory
{
    partial class frmDayBookSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDayBookSettings));
            this.chkDate = new System.Windows.Forms.CheckBox();
            this.grpDate = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.btnDate = new System.Windows.Forms.Button();
            this.txtFromDate = new System.Windows.Forms.MaskedTextBox();
            this.txtToDate = new System.Windows.Forms.MaskedTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.rbtnTransactionWise = new System.Windows.Forms.RadioButton();
            this.rbtnCashBalanceWise = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnShow = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSelectAccClass = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnsavestate = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cboProjectName = new SComboBox();
            this.cboMonths = new SComboBox();
            this.grpDate.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkDate
            // 
            this.chkDate.AutoSize = true;
            this.chkDate.Location = new System.Drawing.Point(3, 5);
            this.chkDate.Name = "chkDate";
            this.chkDate.Size = new System.Drawing.Size(143, 17);
            this.chkDate.TabIndex = 0;
            this.chkDate.Text = "Click For Date Selection:";
            this.chkDate.UseVisualStyleBackColor = true;
            this.chkDate.CheckedChanged += new System.EventHandler(this.chkDate_CheckedChanged);
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
            this.grpDate.Location = new System.Drawing.Point(3, 17);
            this.grpDate.Name = "grpDate";
            this.grpDate.Size = new System.Drawing.Size(280, 102);
            this.grpDate.TabIndex = 1;
            this.grpDate.TabStop = false;
            // 
            // button1
            // 
            this.button1.Image = global::Inventory.Properties.Resources.dateIcon;
            this.button1.Location = new System.Drawing.Point(192, 44);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(26, 25);
            this.button1.TabIndex = 5;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnDate
            // 
            this.btnDate.Image = global::Inventory.Properties.Resources.dateIcon;
            this.btnDate.Location = new System.Drawing.Point(191, 12);
            this.btnDate.Name = "btnDate";
            this.btnDate.Size = new System.Drawing.Size(26, 25);
            this.btnDate.TabIndex = 2;
            this.btnDate.UseVisualStyleBackColor = true;
            this.btnDate.Click += new System.EventHandler(this.btnDate_Click);
            // 
            // txtFromDate
            // 
            this.txtFromDate.Location = new System.Drawing.Point(60, 14);
            this.txtFromDate.Mask = "##/##/####";
            this.txtFromDate.Name = "txtFromDate";
            this.txtFromDate.Size = new System.Drawing.Size(125, 20);
            this.txtFromDate.TabIndex = 1;
            // 
            // txtToDate
            // 
            this.txtToDate.Location = new System.Drawing.Point(59, 47);
            this.txtToDate.Mask = "##/##/####";
            this.txtToDate.Name = "txtToDate";
            this.txtToDate.Size = new System.Drawing.Size(126, 20);
            this.txtToDate.TabIndex = 4;
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
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(21, 21);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(33, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "From:";
            // 
            // rbtnTransactionWise
            // 
            this.rbtnTransactionWise.AutoSize = true;
            this.rbtnTransactionWise.Location = new System.Drawing.Point(6, 19);
            this.rbtnTransactionWise.Name = "rbtnTransactionWise";
            this.rbtnTransactionWise.Size = new System.Drawing.Size(108, 17);
            this.rbtnTransactionWise.TabIndex = 0;
            this.rbtnTransactionWise.TabStop = true;
            this.rbtnTransactionWise.Text = "Transaction Wise";
            this.rbtnTransactionWise.UseVisualStyleBackColor = true;
            // 
            // rbtnCashBalanceWise
            // 
            this.rbtnCashBalanceWise.AutoSize = true;
            this.rbtnCashBalanceWise.Checked = true;
            this.rbtnCashBalanceWise.Location = new System.Drawing.Point(147, 19);
            this.rbtnCashBalanceWise.Name = "rbtnCashBalanceWise";
            this.rbtnCashBalanceWise.Size = new System.Drawing.Size(118, 17);
            this.rbtnCashBalanceWise.TabIndex = 1;
            this.rbtnCashBalanceWise.TabStop = true;
            this.rbtnCashBalanceWise.Text = "Cash Balance Wise";
            this.rbtnCashBalanceWise.UseVisualStyleBackColor = false;
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox1.Controls.Add(this.rbtnCashBalanceWise);
            this.groupBox1.Controls.Add(this.rbtnTransactionWise);
            this.groupBox1.Location = new System.Drawing.Point(3, 120);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(279, 50);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            // 
            // btnShow
            // 
            this.btnShow.Location = new System.Drawing.Point(10, 9);
            this.btnShow.Name = "btnShow";
            this.btnShow.Size = new System.Drawing.Size(73, 23);
            this.btnShow.TabIndex = 4;
            this.btnShow.Text = "Show";
            this.btnShow.UseVisualStyleBackColor = true;
            this.btnShow.Click += new System.EventHandler(this.btnShow_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Image = global::Inventory.Properties.Resources.gnome_window_close;
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(11, 38);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(73, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSelectAccClass
            // 
            this.btnSelectAccClass.Location = new System.Drawing.Point(116, 19);
            this.btnSelectAccClass.Name = "btnSelectAccClass";
            this.btnSelectAccClass.Size = new System.Drawing.Size(150, 23);
            this.btnSelectAccClass.TabIndex = 3;
            this.btnSelectAccClass.Text = "Select Account Class";
            this.btnSelectAccClass.UseVisualStyleBackColor = true;
            this.btnSelectAccClass.Click += new System.EventHandler(this.btnSelectAccClass_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(62, 20);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(46, 13);
            this.label5.TabIndex = 58;
            this.label5.Text = "Project: ";
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox3.Controls.Add(this.cboProjectName);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Location = new System.Drawing.Point(3, 232);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(280, 52);
            this.groupBox3.TabIndex = 60;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Project";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.LightSteelBlue;
            this.panel1.Controls.Add(this.btnsavestate);
            this.panel1.Controls.Add(this.btnShow);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Location = new System.Drawing.Point(285, -1);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(91, 285);
            this.panel1.TabIndex = 61;
            // 
            // btnsavestate
            // 
            this.btnsavestate.Location = new System.Drawing.Point(8, 69);
            this.btnsavestate.Name = "btnsavestate";
            this.btnsavestate.Size = new System.Drawing.Size(76, 23);
            this.btnsavestate.TabIndex = 65;
            this.btnsavestate.Text = "Save State";
            this.btnsavestate.UseVisualStyleBackColor = true;
            this.btnsavestate.Click += new System.EventHandler(this.btnsavestate_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox2.Controls.Add(this.btnSelectAccClass);
            this.groupBox2.Location = new System.Drawing.Point(2, 175);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(280, 53);
            this.groupBox2.TabIndex = 62;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Accounting Class";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 78);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 13);
            this.label1.TabIndex = 60;
            this.label1.Text = "End of Month";
            // 
            // cboProjectName
            // 
            this.cboProjectName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboProjectName.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.cboProjectName.FocusLostColor = System.Drawing.Color.White;
            this.cboProjectName.FormattingEnabled = true;
            this.cboProjectName.Location = new System.Drawing.Point(117, 17);
            this.cboProjectName.Name = "cboProjectName";
            this.cboProjectName.Size = new System.Drawing.Size(148, 21);
            this.cboProjectName.TabIndex = 59;
            // 
            // cboMonths
            // 
            this.cboMonths.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMonths.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.cboMonths.FocusLostColor = System.Drawing.Color.White;
            this.cboMonths.FormattingEnabled = true;
            this.cboMonths.Location = new System.Drawing.Point(98, 75);
            this.cboMonths.Name = "cboMonths";
            this.cboMonths.Size = new System.Drawing.Size(120, 21);
            this.cboMonths.TabIndex = 61;
            this.cboMonths.SelectedIndexChanged += new System.EventHandler(this.cboMonths_SelectedIndexChanged);
            // 
            // frmDayBookSettings
            // 
            this.AcceptButton = this.btnShow;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(375, 285);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.chkDate);
            this.Controls.Add(this.grpDate);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "frmDayBookSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Day Book Settings";
            this.Load += new System.EventHandler(this.frmDayBookSettings_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmDayBookSettings_KeyDown);
            this.grpDate.ResumeLayout(false);
            this.grpDate.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkDate;
        private System.Windows.Forms.GroupBox grpDate;
        private System.Windows.Forms.MaskedTextBox txtFromDate;
        private System.Windows.Forms.MaskedTextBox txtToDate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.RadioButton rbtnTransactionWise;
        private System.Windows.Forms.RadioButton rbtnCashBalanceWise;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnDate;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnShow;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSelectAccClass;
        private SComboBox cboProjectName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnsavestate;
        private SComboBox cboMonths;
        private System.Windows.Forms.Label label1;
    }
}