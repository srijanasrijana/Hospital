namespace Common
{
    partial class frmVoucherRecurring
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
            this.rdoDaily = new System.Windows.Forms.RadioButton();
            this.rdoMonthly = new System.Windows.Forms.RadioButton();
            this.rdoYearly = new System.Windows.Forms.RadioButton();
            this.chkSunday = new System.Windows.Forms.CheckBox();
            this.chkMonday = new System.Windows.Forms.CheckBox();
            this.chkTuesday = new System.Windows.Forms.CheckBox();
            this.chkWednesday = new System.Windows.Forms.CheckBox();
            this.chkThursday = new System.Windows.Forms.CheckBox();
            this.chkFriday = new System.Windows.Forms.CheckBox();
            this.chkSaturday = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cmboDayM = new System.Windows.Forms.ComboBox();
            this.cmboMonth = new System.Windows.Forms.ComboBox();
            this.chkIsLastM = new System.Windows.Forms.CheckBox();
            this.cmboDayY = new System.Windows.Forms.ComboBox();
            this.chkIsLastY = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // rdoDaily
            // 
            this.rdoDaily.Location = new System.Drawing.Point(11, 0);
            this.rdoDaily.Name = "rdoDaily";
            this.rdoDaily.Size = new System.Drawing.Size(48, 17);
            this.rdoDaily.TabIndex = 0;
            this.rdoDaily.Text = "Daily";
            this.rdoDaily.UseVisualStyleBackColor = true;
            this.rdoDaily.CheckedChanged += new System.EventHandler(this.rdoDaily_CheckedChanged);
            // 
            // rdoMonthly
            // 
            this.rdoMonthly.AutoSize = true;
            this.rdoMonthly.Location = new System.Drawing.Point(6, 0);
            this.rdoMonthly.Name = "rdoMonthly";
            this.rdoMonthly.Size = new System.Drawing.Size(62, 17);
            this.rdoMonthly.TabIndex = 0;
            this.rdoMonthly.Text = "Monthly";
            this.rdoMonthly.UseVisualStyleBackColor = true;
            this.rdoMonthly.CheckedChanged += new System.EventHandler(this.rdoMonthly_CheckedChanged);
            // 
            // rdoYearly
            // 
            this.rdoYearly.AutoSize = true;
            this.rdoYearly.Location = new System.Drawing.Point(14, 0);
            this.rdoYearly.Name = "rdoYearly";
            this.rdoYearly.Size = new System.Drawing.Size(54, 17);
            this.rdoYearly.TabIndex = 0;
            this.rdoYearly.Text = "Yearly";
            this.rdoYearly.UseVisualStyleBackColor = true;
            this.rdoYearly.CheckedChanged += new System.EventHandler(this.rdoYearly_CheckedChanged);
            // 
            // chkSunday
            // 
            this.chkSunday.AutoSize = true;
            this.chkSunday.Location = new System.Drawing.Point(71, 21);
            this.chkSunday.Name = "chkSunday";
            this.chkSunday.Size = new System.Drawing.Size(49, 17);
            this.chkSunday.TabIndex = 1;
            this.chkSunday.Text = "SUN";
            this.chkSunday.UseVisualStyleBackColor = true;
            // 
            // chkMonday
            // 
            this.chkMonday.AutoSize = true;
            this.chkMonday.Location = new System.Drawing.Point(125, 21);
            this.chkMonday.Name = "chkMonday";
            this.chkMonday.Size = new System.Drawing.Size(51, 17);
            this.chkMonday.TabIndex = 1;
            this.chkMonday.Text = "MON";
            this.chkMonday.UseVisualStyleBackColor = true;
            // 
            // chkTuesday
            // 
            this.chkTuesday.AutoSize = true;
            this.chkTuesday.Location = new System.Drawing.Point(181, 21);
            this.chkTuesday.Name = "chkTuesday";
            this.chkTuesday.Size = new System.Drawing.Size(48, 17);
            this.chkTuesday.TabIndex = 1;
            this.chkTuesday.Text = "TUE";
            this.chkTuesday.UseVisualStyleBackColor = true;
            // 
            // chkWednesday
            // 
            this.chkWednesday.AutoSize = true;
            this.chkWednesday.Location = new System.Drawing.Point(234, 21);
            this.chkWednesday.Name = "chkWednesday";
            this.chkWednesday.Size = new System.Drawing.Size(52, 17);
            this.chkWednesday.TabIndex = 1;
            this.chkWednesday.Text = "WED";
            this.chkWednesday.UseVisualStyleBackColor = true;
            // 
            // chkThursday
            // 
            this.chkThursday.AutoSize = true;
            this.chkThursday.Location = new System.Drawing.Point(291, 21);
            this.chkThursday.Name = "chkThursday";
            this.chkThursday.Size = new System.Drawing.Size(64, 17);
            this.chkThursday.TabIndex = 1;
            this.chkThursday.Text = "THURS";
            this.chkThursday.UseVisualStyleBackColor = true;
            // 
            // chkFriday
            // 
            this.chkFriday.AutoSize = true;
            this.chkFriday.Location = new System.Drawing.Point(360, 21);
            this.chkFriday.Name = "chkFriday";
            this.chkFriday.Size = new System.Drawing.Size(43, 17);
            this.chkFriday.TabIndex = 1;
            this.chkFriday.Text = "FRI";
            this.chkFriday.UseVisualStyleBackColor = true;
            // 
            // chkSaturday
            // 
            this.chkSaturday.AutoSize = true;
            this.chkSaturday.Location = new System.Drawing.Point(408, 21);
            this.chkSaturday.Name = "chkSaturday";
            this.chkSaturday.Size = new System.Drawing.Size(47, 17);
            this.chkSaturday.TabIndex = 1;
            this.chkSaturday.Text = "SAT";
            this.chkSaturday.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.AutoSize = true;
            this.groupBox1.Controls.Add(this.chkSunday);
            this.groupBox1.Controls.Add(this.chkSaturday);
            this.groupBox1.Controls.Add(this.chkFriday);
            this.groupBox1.Controls.Add(this.rdoDaily);
            this.groupBox1.Controls.Add(this.chkMonday);
            this.groupBox1.Controls.Add(this.chkThursday);
            this.groupBox1.Controls.Add(this.chkTuesday);
            this.groupBox1.Controls.Add(this.chkWednesday);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(522, 57);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            // 
            // cmboDayM
            // 
            this.cmboDayM.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmboDayM.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmboDayM.FormattingEnabled = true;
            this.cmboDayM.Location = new System.Drawing.Point(148, 19);
            this.cmboDayM.Name = "cmboDayM";
            this.cmboDayM.Size = new System.Drawing.Size(121, 21);
            this.cmboDayM.TabIndex = 3;
            // 
            // cmboMonth
            // 
            this.cmboMonth.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmboMonth.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmboMonth.FormattingEnabled = true;
            this.cmboMonth.Location = new System.Drawing.Point(148, 21);
            this.cmboMonth.Name = "cmboMonth";
            this.cmboMonth.Size = new System.Drawing.Size(121, 21);
            this.cmboMonth.TabIndex = 3;
            // 
            // chkIsLastM
            // 
            this.chkIsLastM.AutoSize = true;
            this.chkIsLastM.Location = new System.Drawing.Point(280, 20);
            this.chkIsLastM.Name = "chkIsLastM";
            this.chkIsLastM.Size = new System.Drawing.Size(46, 17);
            this.chkIsLastM.TabIndex = 1;
            this.chkIsLastM.Text = "Last";
            this.chkIsLastM.UseVisualStyleBackColor = true;
            this.chkIsLastM.CheckedChanged += new System.EventHandler(this.chkIsLastM_CheckedChanged);
            // 
            // cmboDayY
            // 
            this.cmboDayY.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmboDayY.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmboDayY.FormattingEnabled = true;
            this.cmboDayY.Location = new System.Drawing.Point(339, 22);
            this.cmboDayY.Name = "cmboDayY";
            this.cmboDayY.Size = new System.Drawing.Size(121, 21);
            this.cmboDayY.TabIndex = 3;
            // 
            // chkIsLastY
            // 
            this.chkIsLastY.AutoSize = true;
            this.chkIsLastY.Location = new System.Drawing.Point(467, 26);
            this.chkIsLastY.Name = "chkIsLastY";
            this.chkIsLastY.Size = new System.Drawing.Size(46, 17);
            this.chkIsLastY.TabIndex = 1;
            this.chkIsLastY.Text = "Last";
            this.chkIsLastY.UseVisualStyleBackColor = true;
            this.chkIsLastY.CheckedChanged += new System.EventHandler(this.chkIsLastY_CheckedChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.rdoYearly);
            this.groupBox3.Controls.Add(this.cmboDayY);
            this.groupBox3.Controls.Add(this.chkIsLastY);
            this.groupBox3.Controls.Add(this.cmboMonth);
            this.groupBox3.Location = new System.Drawing.Point(12, 140);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(522, 56);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(272, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Select Day :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(68, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Select Month :";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.cmboDayM);
            this.groupBox2.Controls.Add(this.chkIsLastM);
            this.groupBox2.Controls.Add(this.rdoMonthly);
            this.groupBox2.Location = new System.Drawing.Point(12, 73);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(522, 52);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(68, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Select Day :";
            // 
            // txtDescription
            // 
            this.txtDescription.Location = new System.Drawing.Point(92, 210);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(442, 60);
            this.txtDescription.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 213);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(66, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Description :";
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(459, 293);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(371, 293);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 6;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // frmVoucherRecurring
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(545, 328);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "frmVoucherRecurring";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Recurring Settings";
            this.Load += new System.EventHandler(this.frmRecurring_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton rdoDaily;
        private System.Windows.Forms.RadioButton rdoMonthly;
        private System.Windows.Forms.RadioButton rdoYearly;
        private System.Windows.Forms.CheckBox chkSunday;
        private System.Windows.Forms.CheckBox chkMonday;
        private System.Windows.Forms.CheckBox chkTuesday;
        private System.Windows.Forms.CheckBox chkWednesday;
        private System.Windows.Forms.CheckBox chkThursday;
        private System.Windows.Forms.CheckBox chkFriday;
        private System.Windows.Forms.CheckBox chkSaturday;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cmboDayM;
        private System.Windows.Forms.ComboBox cmboMonth;
        private System.Windows.Forms.CheckBox chkIsLastM;
        private System.Windows.Forms.ComboBox cmboDayY;
        private System.Windows.Forms.CheckBox chkIsLastY;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
    }
}