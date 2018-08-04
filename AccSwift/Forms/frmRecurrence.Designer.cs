using SComponents;
namespace AccSwift
{
    partial class frmReoccurance
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
            this.pnlReoccurance = new System.Windows.Forms.Panel();
            this.btnEndDate = new System.Windows.Forms.Button();
            this.btnStartDate = new System.Windows.Forms.Button();
            this.pnlFrequency = new System.Windows.Forms.Panel();
            this.rdbYearly = new System.Windows.Forms.RadioButton();
            this.rdbMonthly = new System.Windows.Forms.RadioButton();
            this.rdbWeekly = new System.Windows.Forms.RadioButton();
            this.rdbDaily = new System.Windows.Forms.RadioButton();
            this.label7 = new System.Windows.Forms.Label();
            this.chkNoEndDate = new System.Windows.Forms.CheckBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnClearRecurrence = new System.Windows.Forms.Button();
            this.txtEndDate = new SMaskedTextBox();
            this.txtStartDate = new SMaskedTextBox();
            this.pnlReoccurance.SuspendLayout();
            this.pnlFrequency.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlReoccurance
            // 
            this.pnlReoccurance.Controls.Add(this.btnEndDate);
            this.pnlReoccurance.Controls.Add(this.txtEndDate);
            this.pnlReoccurance.Controls.Add(this.btnStartDate);
            this.pnlReoccurance.Controls.Add(this.txtStartDate);
            this.pnlReoccurance.Controls.Add(this.pnlFrequency);
            this.pnlReoccurance.Controls.Add(this.chkNoEndDate);
            this.pnlReoccurance.Controls.Add(this.label8);
            this.pnlReoccurance.Controls.Add(this.label9);
            this.pnlReoccurance.Location = new System.Drawing.Point(26, 28);
            this.pnlReoccurance.Name = "pnlReoccurance";
            this.pnlReoccurance.Size = new System.Drawing.Size(280, 165);
            this.pnlReoccurance.TabIndex = 33;
            // 
            // btnEndDate
            // 
            this.btnEndDate.Image = global::Inventory.Properties.Resources.dateIcon;
            this.btnEndDate.Location = new System.Drawing.Point(158, 197);
            this.btnEndDate.Name = "btnEndDate";
            this.btnEndDate.Size = new System.Drawing.Size(26, 23);
            this.btnEndDate.TabIndex = 61;
            this.btnEndDate.UseVisualStyleBackColor = true;
            this.btnEndDate.Visible = false;
            this.btnEndDate.Click += new System.EventHandler(this.btnEndDate_Click);
            // 
            // btnStartDate
            // 
            this.btnStartDate.Image = global::Inventory.Properties.Resources.dateIcon;
            this.btnStartDate.Location = new System.Drawing.Point(158, 169);
            this.btnStartDate.Name = "btnStartDate";
            this.btnStartDate.Size = new System.Drawing.Size(26, 23);
            this.btnStartDate.TabIndex = 59;
            this.btnStartDate.UseVisualStyleBackColor = true;
            this.btnStartDate.Visible = false;
            this.btnStartDate.Click += new System.EventHandler(this.btnStartDate_Click);
            // 
            // pnlFrequency
            // 
            this.pnlFrequency.Controls.Add(this.rdbYearly);
            this.pnlFrequency.Controls.Add(this.rdbMonthly);
            this.pnlFrequency.Controls.Add(this.rdbWeekly);
            this.pnlFrequency.Controls.Add(this.rdbDaily);
            this.pnlFrequency.Controls.Add(this.label7);
            this.pnlFrequency.Location = new System.Drawing.Point(3, 10);
            this.pnlFrequency.Name = "pnlFrequency";
            this.pnlFrequency.Size = new System.Drawing.Size(274, 143);
            this.pnlFrequency.TabIndex = 34;
            // 
            // rdbYearly
            // 
            this.rdbYearly.AutoSize = true;
            this.rdbYearly.Location = new System.Drawing.Point(103, 86);
            this.rdbYearly.Name = "rdbYearly";
            this.rdbYearly.Size = new System.Drawing.Size(54, 17);
            this.rdbYearly.TabIndex = 13;
            this.rdbYearly.Text = "Yearly";
            this.rdbYearly.UseVisualStyleBackColor = true;
            // 
            // rdbMonthly
            // 
            this.rdbMonthly.AutoSize = true;
            this.rdbMonthly.Location = new System.Drawing.Point(103, 63);
            this.rdbMonthly.Name = "rdbMonthly";
            this.rdbMonthly.Size = new System.Drawing.Size(62, 17);
            this.rdbMonthly.TabIndex = 12;
            this.rdbMonthly.Text = "Monthly";
            this.rdbMonthly.UseVisualStyleBackColor = true;
            // 
            // rdbWeekly
            // 
            this.rdbWeekly.AutoSize = true;
            this.rdbWeekly.Location = new System.Drawing.Point(103, 40);
            this.rdbWeekly.Name = "rdbWeekly";
            this.rdbWeekly.Size = new System.Drawing.Size(61, 17);
            this.rdbWeekly.TabIndex = 11;
            this.rdbWeekly.Text = "Weekly";
            this.rdbWeekly.UseVisualStyleBackColor = true;
            // 
            // rdbDaily
            // 
            this.rdbDaily.AutoSize = true;
            this.rdbDaily.Checked = true;
            this.rdbDaily.Location = new System.Drawing.Point(103, 17);
            this.rdbDaily.Name = "rdbDaily";
            this.rdbDaily.Size = new System.Drawing.Size(48, 17);
            this.rdbDaily.TabIndex = 10;
            this.rdbDaily.TabStop = true;
            this.rdbDaily.Text = "Daily";
            this.rdbDaily.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(22, 17);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(63, 13);
            this.label7.TabIndex = 9;
            this.label7.Text = "Frequency :";
            // 
            // chkNoEndDate
            // 
            this.chkNoEndDate.AutoSize = true;
            this.chkNoEndDate.Location = new System.Drawing.Point(189, 200);
            this.chkNoEndDate.Name = "chkNoEndDate";
            this.chkNoEndDate.Size = new System.Drawing.Size(88, 17);
            this.chkNoEndDate.TabIndex = 16;
            this.chkNoEndDate.Text = "No End Date";
            this.chkNoEndDate.UseVisualStyleBackColor = true;
            this.chkNoEndDate.Visible = false;
            this.chkNoEndDate.CheckedChanged += new System.EventHandler(this.chkNoEndDate_CheckedChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(15, 201);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(61, 13);
            this.label8.TabIndex = 15;
            this.label8.Text = "End Date : ";
            this.label8.Visible = false;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(12, 171);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(64, 13);
            this.label9.TabIndex = 14;
            this.label9.Text = "Start Date : ";
            this.label9.Visible = false;
            // 
            // btnCancel
            // 
            this.btnCancel.Image = global::Inventory.Properties.Resources.gnome_window_close;
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(231, 199);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 29);
            this.btnCancel.TabIndex = 35;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Image = global::Inventory.Properties.Resources.save;
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSave.Location = new System.Drawing.Point(148, 199);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 29);
            this.btnSave.TabIndex = 34;
            this.btnSave.Text = "&Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnClearRecurrence
            // 
            this.btnClearRecurrence.Image = global::Inventory.Properties.Resources.document_delete;
            this.btnClearRecurrence.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClearRecurrence.Location = new System.Drawing.Point(28, 199);
            this.btnClearRecurrence.Name = "btnClearRecurrence";
            this.btnClearRecurrence.Size = new System.Drawing.Size(114, 29);
            this.btnClearRecurrence.TabIndex = 65;
            this.btnClearRecurrence.Text = "&Clear Recurrence";
            this.btnClearRecurrence.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnClearRecurrence.UseVisualStyleBackColor = true;
            this.btnClearRecurrence.Click += new System.EventHandler(this.btnClearReminder_Click);
            // 
            // txtEndDate
            // 
            this.txtEndDate.BackColor = System.Drawing.Color.White;
            this.txtEndDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtEndDate.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.txtEndDate.FocusLostColor = System.Drawing.Color.White;
            this.txtEndDate.Location = new System.Drawing.Point(82, 198);
            this.txtEndDate.Mask = "0000/00/00";
            this.txtEndDate.Name = "txtEndDate";
            this.txtEndDate.Size = new System.Drawing.Size(75, 20);
            this.txtEndDate.TabIndex = 60;
            this.txtEndDate.Visible = false;
            // 
            // txtStartDate
            // 
            this.txtStartDate.BackColor = System.Drawing.Color.White;
            this.txtStartDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtStartDate.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.txtStartDate.FocusLostColor = System.Drawing.Color.White;
            this.txtStartDate.Location = new System.Drawing.Point(82, 171);
            this.txtStartDate.Mask = "0000/00/00";
            this.txtStartDate.Name = "txtStartDate";
            this.txtStartDate.Size = new System.Drawing.Size(75, 20);
            this.txtStartDate.TabIndex = 58;
            this.txtStartDate.Visible = false;
            // 
            // frmReoccurance
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(325, 241);
            this.Controls.Add(this.btnClearRecurrence);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.pnlReoccurance);
            this.Name = "frmReoccurance";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Reoccurance";
            this.Load += new System.EventHandler(this.frmReoccurance_Load);
            this.pnlReoccurance.ResumeLayout(false);
            this.pnlReoccurance.PerformLayout();
            this.pnlFrequency.ResumeLayout(false);
            this.pnlFrequency.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlReoccurance;
        private System.Windows.Forms.Button btnEndDate;
        private SMaskedTextBox txtEndDate;
        private System.Windows.Forms.Button btnStartDate;
        private SMaskedTextBox txtStartDate;
        private System.Windows.Forms.Panel pnlFrequency;
        private System.Windows.Forms.RadioButton rdbYearly;
        private System.Windows.Forms.RadioButton rdbMonthly;
        private System.Windows.Forms.RadioButton rdbWeekly;
        private System.Windows.Forms.RadioButton rdbDaily;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox chkNoEndDate;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnClearRecurrence;
    }
}