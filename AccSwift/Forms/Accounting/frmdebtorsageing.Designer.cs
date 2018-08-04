using SComponents;
namespace Inventory.Forms.Accounting
{
    partial class frmdebtorsageing
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmdebtorsageing));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnToday = new System.Windows.Forms.Button();
            this.btnDate = new System.Windows.Forms.Button();
            this.txtToDate = new System.Windows.Forms.MaskedTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rbtnbillwiseageing = new System.Windows.Forms.RadioButton();
            this.rbtnageing = new System.Windows.Forms.RadioButton();
            this.chkshowalldebtors = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.txtfourthperiod = new STextBox();
            this.txtthirdperiod = new STextBox();
            this.txtsecondperiod = new STextBox();
            this.txtfirstperiod = new STextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbdebtorsaccount = new System.Windows.Forms.ComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.btnSelectAccClass = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.chkshowvoucherbalance = new System.Windows.Forms.CheckBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.cboProjectName = new SComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.cboMonths = new SComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox1.Controls.Add(this.cboMonths);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.btnToday);
            this.groupBox1.Controls.Add(this.btnDate);
            this.groupBox1.Controls.Add(this.txtToDate);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(402, 77);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Date Range";
            // 
            // btnToday
            // 
            this.btnToday.Location = new System.Drawing.Point(292, 22);
            this.btnToday.Name = "btnToday";
            this.btnToday.Size = new System.Drawing.Size(56, 25);
            this.btnToday.TabIndex = 68;
            this.btnToday.Text = "&Today";
            this.btnToday.UseVisualStyleBackColor = true;
            this.btnToday.Click += new System.EventHandler(this.btnToday_Click);
            // 
            // btnDate
            // 
            this.btnDate.Image = global::Inventory.Properties.Resources.dateIcon;
            this.btnDate.Location = new System.Drawing.Point(264, 22);
            this.btnDate.Name = "btnDate";
            this.btnDate.Size = new System.Drawing.Size(26, 25);
            this.btnDate.TabIndex = 67;
            this.btnDate.UseVisualStyleBackColor = true;
            this.btnDate.Click += new System.EventHandler(this.btnDate_Click);
            // 
            // txtToDate
            // 
            this.txtToDate.Location = new System.Drawing.Point(134, 25);
            this.txtToDate.Mask = "##/##/####";
            this.txtToDate.Name = "txtToDate";
            this.txtToDate.Size = new System.Drawing.Size(126, 20);
            this.txtToDate.TabIndex = 66;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(56, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "As on Date:";
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox2.Controls.Add(this.rbtnbillwiseageing);
            this.groupBox2.Controls.Add(this.rbtnageing);
            this.groupBox2.Location = new System.Drawing.Point(0, 83);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(402, 54);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Criteria";
            // 
            // rbtnbillwiseageing
            // 
            this.rbtnbillwiseageing.AutoSize = true;
            this.rbtnbillwiseageing.Location = new System.Drawing.Point(182, 25);
            this.rbtnbillwiseageing.Name = "rbtnbillwiseageing";
            this.rbtnbillwiseageing.Size = new System.Drawing.Size(95, 17);
            this.rbtnbillwiseageing.TabIndex = 1;
            this.rbtnbillwiseageing.Text = "Billwise Ageing";
            this.rbtnbillwiseageing.UseVisualStyleBackColor = true;
            // 
            // rbtnageing
            // 
            this.rbtnageing.AutoSize = true;
            this.rbtnageing.Checked = true;
            this.rbtnageing.Location = new System.Drawing.Point(49, 25);
            this.rbtnageing.Name = "rbtnageing";
            this.rbtnageing.Size = new System.Drawing.Size(58, 17);
            this.rbtnageing.TabIndex = 0;
            this.rbtnageing.TabStop = true;
            this.rbtnageing.Text = "Ageing";
            this.rbtnageing.UseVisualStyleBackColor = true;
            // 
            // chkshowalldebtors
            // 
            this.chkshowalldebtors.AutoSize = true;
            this.chkshowalldebtors.Checked = true;
            this.chkshowalldebtors.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkshowalldebtors.Location = new System.Drawing.Point(7, 147);
            this.chkshowalldebtors.Name = "chkshowalldebtors";
            this.chkshowalldebtors.Size = new System.Drawing.Size(107, 17);
            this.chkshowalldebtors.TabIndex = 2;
            this.chkshowalldebtors.Text = "Show All Debtors";
            this.chkshowalldebtors.UseVisualStyleBackColor = true;
            this.chkshowalldebtors.CheckedChanged += new System.EventHandler(this.chkshowalldebtors_CheckedChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox3.Controls.Add(this.txtfourthperiod);
            this.groupBox3.Controls.Add(this.txtthirdperiod);
            this.groupBox3.Controls.Add(this.txtsecondperiod);
            this.groupBox3.Controls.Add(this.txtfirstperiod);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Location = new System.Drawing.Point(0, 172);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(402, 93);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Choose Ageing Period";
            // 
            // txtfourthperiod
            // 
            this.txtfourthperiod.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtfourthperiod.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtfourthperiod.FocusLostColor = System.Drawing.Color.White;
            this.txtfourthperiod.Location = new System.Drawing.Point(244, 65);
            this.txtfourthperiod.Name = "txtfourthperiod";
            this.txtfourthperiod.Size = new System.Drawing.Size(52, 20);
            this.txtfourthperiod.TabIndex = 9;
            this.txtfourthperiod.Text = "60";
            // 
            // txtthirdperiod
            // 
            this.txtthirdperiod.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtthirdperiod.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtthirdperiod.FocusLostColor = System.Drawing.Color.White;
            this.txtthirdperiod.Location = new System.Drawing.Point(80, 64);
            this.txtthirdperiod.Name = "txtthirdperiod";
            this.txtthirdperiod.Size = new System.Drawing.Size(50, 20);
            this.txtthirdperiod.TabIndex = 8;
            this.txtthirdperiod.Text = "45";
            // 
            // txtsecondperiod
            // 
            this.txtsecondperiod.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtsecondperiod.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtsecondperiod.FocusLostColor = System.Drawing.Color.White;
            this.txtsecondperiod.Location = new System.Drawing.Point(245, 26);
            this.txtsecondperiod.Name = "txtsecondperiod";
            this.txtsecondperiod.Size = new System.Drawing.Size(51, 20);
            this.txtsecondperiod.TabIndex = 7;
            this.txtsecondperiod.Text = "30";
            // 
            // txtfirstperiod
            // 
            this.txtfirstperiod.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtfirstperiod.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtfirstperiod.FocusLostColor = System.Drawing.Color.White;
            this.txtfirstperiod.Location = new System.Drawing.Point(81, 25);
            this.txtfirstperiod.Name = "txtfirstperiod";
            this.txtfirstperiod.Size = new System.Drawing.Size(49, 20);
            this.txtfirstperiod.TabIndex = 6;
            this.txtfirstperiod.Text = "15";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(156, 69);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(73, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "Fourth Period:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 66);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(67, 13);
            this.label6.TabIndex = 4;
            this.label6.Text = "Third Period:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(156, 29);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(80, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Second Period:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 26);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(62, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "First Period:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(146, 152);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(90, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Debtors Account:";
            // 
            // cmbdebtorsaccount
            // 
            this.cmbdebtorsaccount.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbdebtorsaccount.FormattingEnabled = true;
            this.cmbdebtorsaccount.Location = new System.Drawing.Point(242, 146);
            this.cmbdebtorsaccount.Name = "cmbdebtorsaccount";
            this.cmbdebtorsaccount.Size = new System.Drawing.Size(160, 21);
            this.cmbdebtorsaccount.TabIndex = 1;
            this.cmbdebtorsaccount.SelectedIndexChanged += new System.EventHandler(this.cmbdebtorsaccount_SelectedIndexChanged);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.LightSteelBlue;
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Location = new System.Drawing.Point(404, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(93, 316);
            this.panel1.TabIndex = 64;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(7, 7);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(78, 31);
            this.button1.TabIndex = 22;
            this.button1.Text = "Show";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Image = global::Inventory.Properties.Resources.gnome_window_close;
            this.button2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button2.Location = new System.Drawing.Point(8, 45);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(78, 32);
            this.button2.TabIndex = 61;
            this.button2.Text = "&Cancel";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // btnSelectAccClass
            // 
            this.btnSelectAccClass.Location = new System.Drawing.Point(10, 17);
            this.btnSelectAccClass.Name = "btnSelectAccClass";
            this.btnSelectAccClass.Size = new System.Drawing.Size(160, 23);
            this.btnSelectAccClass.TabIndex = 34;
            this.btnSelectAccClass.Text = "Select Account Class";
            this.btnSelectAccClass.UseVisualStyleBackColor = true;
            this.btnSelectAccClass.Click += new System.EventHandler(this.btnSelectAccClass_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.BackColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox4.Controls.Add(this.btnSelectAccClass);
            this.groupBox4.Location = new System.Drawing.Point(0, 269);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(184, 46);
            this.groupBox4.TabIndex = 65;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Accounting Class";
            // 
            // chkshowvoucherbalance
            // 
            this.chkshowvoucherbalance.AutoSize = true;
            this.chkshowvoucherbalance.Location = new System.Drawing.Point(3, 317);
            this.chkshowvoucherbalance.Name = "chkshowvoucherbalance";
            this.chkshowvoucherbalance.Size = new System.Drawing.Size(138, 17);
            this.chkshowvoucherbalance.TabIndex = 66;
            this.chkshowvoucherbalance.Text = "Show Voucher Balance";
            this.chkshowvoucherbalance.UseVisualStyleBackColor = true;
            this.chkshowvoucherbalance.Visible = false;
            // 
            // groupBox6
            // 
            this.groupBox6.BackColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox6.Controls.Add(this.cboProjectName);
            this.groupBox6.Controls.Add(this.label7);
            this.groupBox6.Location = new System.Drawing.Point(190, 275);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(203, 40);
            this.groupBox6.TabIndex = 67;
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
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(8, 16);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(46, 13);
            this.label7.TabIndex = 38;
            this.label7.Text = "Project: ";
            // 
            // cboMonths
            // 
            this.cboMonths.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMonths.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.cboMonths.FocusLostColor = System.Drawing.Color.White;
            this.cboMonths.FormattingEnabled = true;
            this.cboMonths.Location = new System.Drawing.Point(134, 50);
            this.cboMonths.Name = "cboMonths";
            this.cboMonths.Size = new System.Drawing.Size(126, 21);
            this.cboMonths.TabIndex = 70;
            this.cboMonths.SelectedIndexChanged += new System.EventHandler(this.cboMonths_SelectedIndexChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(57, 53);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(71, 13);
            this.label8.TabIndex = 69;
            this.label8.Text = "End of Month";
            // 
            // frmdebtorsageing
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(496, 335);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.chkshowvoucherbalance);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.chkshowalldebtors);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmbdebtorsaccount);
            this.Controls.Add(this.groupBox2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmdebtorsageing";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Debtors Ageing";
            this.Load += new System.EventHandler(this.frmdebtorsageing_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnToday;
        private System.Windows.Forms.Button btnDate;
        private System.Windows.Forms.MaskedTextBox txtToDate;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rbtnbillwiseageing;
        private System.Windows.Forms.RadioButton rbtnageing;
        private System.Windows.Forms.CheckBox chkshowalldebtors;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbdebtorsaccount;
        private STextBox txtfourthperiod;
        private STextBox txtthirdperiod;
        private STextBox txtsecondperiod;
        private STextBox txtfirstperiod;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button btnSelectAccClass;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.CheckBox chkshowvoucherbalance;
        private System.Windows.Forms.GroupBox groupBox6;
        private SComboBox cboProjectName;
        private System.Windows.Forms.Label label7;
        private SComboBox cboMonths;
        private System.Windows.Forms.Label label8;
    }
}