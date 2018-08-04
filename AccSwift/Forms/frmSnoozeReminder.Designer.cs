using SComponents;
namespace AccSwift
{
    partial class frmSnoozeReminder
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
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnDate = new System.Windows.Forms.Button();
            this.txtToDate = new System.Windows.Forms.MaskedTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cboSnoozeDay = new SComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.ReminderIdTextBox = new System.Windows.Forms.TextBox();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnSave.Location = new System.Drawing.Point(218, 128);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(80, 26);
            this.btnSave.TabIndex = 7;
            this.btnSave.Text = "&Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(304, 128);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(71, 26);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnDate);
            this.groupBox2.Controls.Add(this.txtToDate);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.cboSnoozeDay);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Location = new System.Drawing.Point(12, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(381, 98);
            this.groupBox2.TabIndex = 52;
            this.groupBox2.TabStop = false;
            // 
            // btnDate
            // 
            this.btnDate.Image = global::Inventory.Properties.Resources.dateIcon;
            this.btnDate.Location = new System.Drawing.Point(278, 32);
            this.btnDate.Name = "btnDate";
            this.btnDate.Size = new System.Drawing.Size(26, 23);
            this.btnDate.TabIndex = 70;
            this.btnDate.UseVisualStyleBackColor = true;
            this.btnDate.Click += new System.EventHandler(this.btnDate_Click);
            // 
            // txtToDate
            // 
            this.txtToDate.Location = new System.Drawing.Point(174, 34);
            this.txtToDate.Mask = "0000/00/00";
            this.txtToDate.Name = "txtToDate";
            this.txtToDate.Size = new System.Drawing.Size(98, 20);
            this.txtToDate.TabIndex = 69;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(27, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 13);
            this.label1.TabIndex = 60;
            this.label1.Text = " Snooze";
            // 
            // cboSnoozeDay
            // 
            this.cboSnoozeDay.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboSnoozeDay.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboSnoozeDay.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSnoozeDay.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.cboSnoozeDay.FocusLostColor = System.Drawing.Color.White;
            this.cboSnoozeDay.FormattingEnabled = true;
            this.cboSnoozeDay.Location = new System.Drawing.Point(27, 34);
            this.cboSnoozeDay.Name = "cboSnoozeDay";
            this.cboSnoozeDay.Size = new System.Drawing.Size(129, 21);
            this.cboSnoozeDay.TabIndex = 59;
            this.cboSnoozeDay.SelectedIndexChanged += new System.EventHandler(this.cboSnoozeDay_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(171, 16);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(78, 13);
            this.label6.TabIndex = 56;
            this.label6.Text = "Reminder Date";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 3);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(0, 13);
            this.label5.TabIndex = 58;
            // 
            // ReminderIdTextBox
            // 
            this.ReminderIdTextBox.Location = new System.Drawing.Point(67, 152);
            this.ReminderIdTextBox.Name = "ReminderIdTextBox";
            this.ReminderIdTextBox.Size = new System.Drawing.Size(46, 20);
            this.ReminderIdTextBox.TabIndex = 53;
            this.ReminderIdTextBox.Visible = false;
            // 
            // frmSnoozeReminder
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(399, 184);
            this.Controls.Add(this.ReminderIdTextBox);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.KeyPreview = true;
            this.Name = "frmSnoozeReminder";
            this.Text = "Snooze Reminder";
            this.Load += new System.EventHandler(this.frmSnoozeReminder_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmSnoozeReminder_KeyDown);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox ReminderIdTextBox;
        private System.Windows.Forms.Label label1;
        private SComboBox cboSnoozeDay;
        private System.Windows.Forms.MaskedTextBox txtToDate;
        private System.Windows.Forms.Button btnDate;
    }
}