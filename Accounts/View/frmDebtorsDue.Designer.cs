using SComponents;
namespace Accounts
{
    partial class frmDebtorsDue
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDebtorsDue));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rdbtnOverDue = new System.Windows.Forms.RadioButton();
            this.rdbtnDuebills = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cboMonths = new SComponents.SComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnDate = new System.Windows.Forms.Button();
            this.txttodate = new SComponents.SMaskedTextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnshow = new System.Windows.Forms.Button();
            this.btncancel = new System.Windows.Forms.Button();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.cboProjectName = new SComponents.SComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.btnSelectAccClass = new System.Windows.Forms.Button();
            this.chkshowalldebtors = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbdebtorsaccount = new System.Windows.Forms.ComboBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rdbtnOverDue);
            this.groupBox1.Controls.Add(this.rdbtnDuebills);
            this.groupBox1.Location = new System.Drawing.Point(2, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(263, 44);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // rdbtnOverDue
            // 
            this.rdbtnOverDue.AutoSize = true;
            this.rdbtnOverDue.Location = new System.Drawing.Point(96, 19);
            this.rdbtnOverDue.Name = "rdbtnOverDue";
            this.rdbtnOverDue.Size = new System.Drawing.Size(92, 17);
            this.rdbtnOverDue.TabIndex = 2;
            this.rdbtnOverDue.TabStop = true;
            this.rdbtnOverDue.Text = "Over Due Bills";
            this.rdbtnOverDue.UseVisualStyleBackColor = true;
            // 
            // rdbtnDuebills
            // 
            this.rdbtnDuebills.AutoSize = true;
            this.rdbtnDuebills.Location = new System.Drawing.Point(6, 19);
            this.rdbtnDuebills.Name = "rdbtnDuebills";
            this.rdbtnDuebills.Size = new System.Drawing.Size(66, 17);
            this.rdbtnDuebills.TabIndex = 1;
            this.rdbtnDuebills.TabStop = true;
            this.rdbtnDuebills.Text = "Due Bills";
            this.rdbtnDuebills.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cboMonths);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.btnDate);
            this.groupBox2.Controls.Add(this.txttodate);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Location = new System.Drawing.Point(1, 50);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(264, 69);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            // 
            // cboMonths
            // 
            this.cboMonths.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMonths.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.cboMonths.FocusLostColor = System.Drawing.Color.White;
            this.cboMonths.FormattingEnabled = true;
            this.cboMonths.Location = new System.Drawing.Point(87, 37);
            this.cboMonths.Name = "cboMonths";
            this.cboMonths.Size = new System.Drawing.Size(108, 21);
            this.cboMonths.TabIndex = 63;
            this.cboMonths.SelectedIndexChanged += new System.EventHandler(this.cboMonths_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 13);
            this.label1.TabIndex = 62;
            this.label1.Text = "End of Month";
            // 
            // btnDate
            // 
            this.btnDate.Image = global::Accounts.Properties.Resources.dateIcon;
            this.btnDate.Location = new System.Drawing.Point(169, 8);
            this.btnDate.Name = "btnDate";
            this.btnDate.Size = new System.Drawing.Size(26, 23);
            this.btnDate.TabIndex = 23;
            this.btnDate.UseVisualStyleBackColor = true;
            this.btnDate.Click += new System.EventHandler(this.btnDate_Click);
            // 
            // txttodate
            // 
            this.txttodate.BackColor = System.Drawing.Color.White;
            this.txttodate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txttodate.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.txttodate.FocusLostColor = System.Drawing.Color.White;
            this.txttodate.Location = new System.Drawing.Point(67, 11);
            this.txttodate.Mask = "0000/00/00";
            this.txttodate.Name = "txttodate";
            this.txttodate.Size = new System.Drawing.Size(84, 20);
            this.txttodate.TabIndex = 22;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(19, 15);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(36, 13);
            this.label4.TabIndex = 21;
            this.label4.Text = "As On";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.LightSteelBlue;
            this.panel1.Controls.Add(this.btnshow);
            this.panel1.Controls.Add(this.btncancel);
            this.panel1.Location = new System.Drawing.Point(267, 1);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(88, 254);
            this.panel1.TabIndex = 65;
            // 
            // btnshow
            // 
            this.btnshow.Location = new System.Drawing.Point(3, 7);
            this.btnshow.Name = "btnshow";
            this.btnshow.Size = new System.Drawing.Size(78, 31);
            this.btnshow.TabIndex = 22;
            this.btnshow.Text = "Show";
            this.btnshow.UseVisualStyleBackColor = true;
            this.btnshow.Click += new System.EventHandler(this.btnshow_Click);
            // 
            // btncancel
            // 
            this.btncancel.Image = global::Accounts.Properties.Resources.gnome_window_close;
            this.btncancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btncancel.Location = new System.Drawing.Point(4, 45);
            this.btncancel.Name = "btncancel";
            this.btncancel.Size = new System.Drawing.Size(78, 32);
            this.btncancel.TabIndex = 61;
            this.btncancel.Text = "&Cancel";
            this.btncancel.UseVisualStyleBackColor = true;
            this.btncancel.Click += new System.EventHandler(this.btncancel_Click);
            // 
            // groupBox6
            // 
            this.groupBox6.BackColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox6.Controls.Add(this.cboProjectName);
            this.groupBox6.Controls.Add(this.label7);
            this.groupBox6.Location = new System.Drawing.Point(2, 215);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(262, 40);
            this.groupBox6.TabIndex = 69;
            this.groupBox6.TabStop = false;
            // 
            // cboProjectName
            // 
            this.cboProjectName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboProjectName.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.cboProjectName.FocusLostColor = System.Drawing.Color.White;
            this.cboProjectName.FormattingEnabled = true;
            this.cboProjectName.Location = new System.Drawing.Point(53, 13);
            this.cboProjectName.Name = "cboProjectName";
            this.cboProjectName.Size = new System.Drawing.Size(142, 21);
            this.cboProjectName.TabIndex = 39;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(5, 16);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(46, 13);
            this.label7.TabIndex = 38;
            this.label7.Text = "Project: ";
            // 
            // groupBox4
            // 
            this.groupBox4.BackColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox4.Controls.Add(this.btnSelectAccClass);
            this.groupBox4.Location = new System.Drawing.Point(2, 167);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(263, 46);
            this.groupBox4.TabIndex = 68;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Accounting Class";
            // 
            // btnSelectAccClass
            // 
            this.btnSelectAccClass.Location = new System.Drawing.Point(34, 19);
            this.btnSelectAccClass.Name = "btnSelectAccClass";
            this.btnSelectAccClass.Size = new System.Drawing.Size(160, 23);
            this.btnSelectAccClass.TabIndex = 34;
            this.btnSelectAccClass.Text = "Select Account Class";
            this.btnSelectAccClass.UseVisualStyleBackColor = true;
            this.btnSelectAccClass.Click += new System.EventHandler(this.btnSelectAccClass_Click);
            // 
            // chkshowalldebtors
            // 
            this.chkshowalldebtors.AutoSize = true;
            this.chkshowalldebtors.Checked = true;
            this.chkshowalldebtors.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkshowalldebtors.Location = new System.Drawing.Point(4, 121);
            this.chkshowalldebtors.Name = "chkshowalldebtors";
            this.chkshowalldebtors.Size = new System.Drawing.Size(107, 17);
            this.chkshowalldebtors.TabIndex = 72;
            this.chkshowalldebtors.Text = "Show All Debtors";
            this.chkshowalldebtors.UseVisualStyleBackColor = true;
            this.chkshowalldebtors.CheckedChanged += new System.EventHandler(this.chkshowalldebtors_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 144);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(90, 13);
            this.label2.TabIndex = 70;
            this.label2.Text = "Debtors Account:";
            // 
            // cmbdebtorsaccount
            // 
            this.cmbdebtorsaccount.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbdebtorsaccount.FormattingEnabled = true;
            this.cmbdebtorsaccount.Location = new System.Drawing.Point(101, 140);
            this.cmbdebtorsaccount.Name = "cmbdebtorsaccount";
            this.cmbdebtorsaccount.Size = new System.Drawing.Size(160, 21);
            this.cmbdebtorsaccount.TabIndex = 71;
            // 
            // frmDebtorsDue
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(350, 259);
            this.Controls.Add(this.chkshowalldebtors);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmbdebtorsaccount);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmDebtorsDue";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Debtors Due";
            this.Load += new System.EventHandler(this.frmDebtorsDue_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rdbtnOverDue;
        private System.Windows.Forms.RadioButton rdbtnDuebills;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnDate;
        private SMaskedTextBox txttodate;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnshow;
        private System.Windows.Forms.Button btncancel;
        private System.Windows.Forms.GroupBox groupBox6;
        private SComboBox cboProjectName;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button btnSelectAccClass;
        private System.Windows.Forms.CheckBox chkshowalldebtors;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbdebtorsaccount;
        private SComboBox cboMonths;
        private System.Windows.Forms.Label label1;
    }
}