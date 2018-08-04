using SComponents;
namespace Accounts.Reports
{
    partial class frmMBalanceSheetSettings
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
            this.panelleft = new System.Windows.Forms.Panel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.chkShowSecLevGrpDet = new System.Windows.Forms.CheckBox();
            this.chkShowZeroBal = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cboProjectName = new SComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnSelectAccClass = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.btnToday = new System.Windows.Forms.Button();
            this.btnDate = new System.Windows.Forms.Button();
            this.cboMonths = new SComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.chkDate = new System.Windows.Forms.CheckBox();
            this.txtToDate = new System.Windows.Forms.MaskedTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.panelright = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnShow = new System.Windows.Forms.Button();
            this.panelleft.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.panelright.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelleft
            // 
            this.panelleft.BackColor = System.Drawing.Color.LightGray;
            this.panelleft.Controls.Add(this.groupBox3);
            this.panelleft.Controls.Add(this.groupBox2);
            this.panelleft.Controls.Add(this.groupBox1);
            this.panelleft.Controls.Add(this.groupBox5);
            this.panelleft.Location = new System.Drawing.Point(0, 2);
            this.panelleft.Name = "panelleft";
            this.panelleft.Size = new System.Drawing.Size(410, 280);
            this.panelleft.TabIndex = 0;
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.Color.Gainsboro;
            this.groupBox3.Controls.Add(this.chkShowSecLevGrpDet);
            this.groupBox3.Controls.Add(this.chkShowZeroBal);
            this.groupBox3.Location = new System.Drawing.Point(4, 206);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(398, 56);
            this.groupBox3.TabIndex = 65;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Options";
            // 
            // chkShowSecLevGrpDet
            // 
            this.chkShowSecLevGrpDet.AutoSize = true;
            this.chkShowSecLevGrpDet.Location = new System.Drawing.Point(162, 20);
            this.chkShowSecLevGrpDet.Name = "chkShowSecLevGrpDet";
            this.chkShowSecLevGrpDet.Size = new System.Drawing.Size(189, 17);
            this.chkShowSecLevGrpDet.TabIndex = 35;
            this.chkShowSecLevGrpDet.Text = "Show Second Level Group Details";
            this.chkShowSecLevGrpDet.UseVisualStyleBackColor = true;
            // 
            // chkShowZeroBal
            // 
            this.chkShowZeroBal.AutoSize = true;
            this.chkShowZeroBal.Location = new System.Drawing.Point(10, 20);
            this.chkShowZeroBal.Name = "chkShowZeroBal";
            this.chkShowZeroBal.Size = new System.Drawing.Size(120, 17);
            this.chkShowZeroBal.TabIndex = 34;
            this.chkShowZeroBal.Text = "Show Zero Balance";
            this.chkShowZeroBal.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.Gainsboro;
            this.groupBox2.Controls.Add(this.cboProjectName);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Location = new System.Drawing.Point(4, 142);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(398, 56);
            this.groupBox2.TabIndex = 63;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Project";
            // 
            // cboProjectName
            // 
            this.cboProjectName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboProjectName.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.cboProjectName.FocusLostColor = System.Drawing.Color.White;
            this.cboProjectName.FormattingEnabled = true;
            this.cboProjectName.Location = new System.Drawing.Point(73, 23);
            this.cboProjectName.Name = "cboProjectName";
            this.cboProjectName.Size = new System.Drawing.Size(148, 21);
            this.cboProjectName.TabIndex = 55;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(6, 26);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(55, 13);
            this.label5.TabIndex = 54;
            this.label5.Text = "Project: ";
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Gainsboro;
            this.groupBox1.Controls.Add(this.btnSelectAccClass);
            this.groupBox1.Location = new System.Drawing.Point(3, 77);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(398, 56);
            this.groupBox1.TabIndex = 61;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Accounting Class";
            // 
            // btnSelectAccClass
            // 
            this.btnSelectAccClass.Location = new System.Drawing.Point(6, 19);
            this.btnSelectAccClass.Name = "btnSelectAccClass";
            this.btnSelectAccClass.Size = new System.Drawing.Size(124, 25);
            this.btnSelectAccClass.TabIndex = 35;
            this.btnSelectAccClass.Text = "Select Account Class";
            this.btnSelectAccClass.UseVisualStyleBackColor = true;
            this.btnSelectAccClass.Click += new System.EventHandler(this.btnSelectAccClass_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.BackColor = System.Drawing.Color.Gainsboro;
            this.groupBox5.Controls.Add(this.btnToday);
            this.groupBox5.Controls.Add(this.btnDate);
            this.groupBox5.Controls.Add(this.cboMonths);
            this.groupBox5.Controls.Add(this.label1);
            this.groupBox5.Controls.Add(this.chkDate);
            this.groupBox5.Controls.Add(this.txtToDate);
            this.groupBox5.Controls.Add(this.label2);
            this.groupBox5.Location = new System.Drawing.Point(3, 4);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(400, 67);
            this.groupBox5.TabIndex = 60;
            this.groupBox5.TabStop = false;
            // 
            // btnToday
            // 
            this.btnToday.Location = new System.Drawing.Point(329, 30);
            this.btnToday.Name = "btnToday";
            this.btnToday.Size = new System.Drawing.Size(56, 25);
            this.btnToday.TabIndex = 63;
            this.btnToday.Text = "&Today";
            this.btnToday.UseVisualStyleBackColor = true;
            this.btnToday.Click += new System.EventHandler(this.btnToday_Click);
            // 
            // btnDate
            // 
            this.btnDate.Image = global::Accounts.Properties.Resources.dateIcon;
            this.btnDate.Location = new System.Drawing.Point(303, 30);
            this.btnDate.Name = "btnDate";
            this.btnDate.Size = new System.Drawing.Size(26, 25);
            this.btnDate.TabIndex = 62;
            this.btnDate.UseVisualStyleBackColor = true;
            this.btnDate.Click += new System.EventHandler(this.btnDate_Click);
            // 
            // cboMonths
            // 
            this.cboMonths.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMonths.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.cboMonths.FocusLostColor = System.Drawing.Color.White;
            this.cboMonths.FormattingEnabled = true;
            this.cboMonths.Location = new System.Drawing.Point(3, 34);
            this.cboMonths.Name = "cboMonths";
            this.cboMonths.Size = new System.Drawing.Size(139, 21);
            this.cboMonths.TabIndex = 61;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(31, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 13);
            this.label1.TabIndex = 60;
            this.label1.Text = "End of Month";
            // 
            // chkDate
            // 
            this.chkDate.AutoSize = true;
            this.chkDate.Checked = true;
            this.chkDate.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDate.Location = new System.Drawing.Point(0, 0);
            this.chkDate.Name = "chkDate";
            this.chkDate.Size = new System.Drawing.Size(93, 17);
            this.chkDate.TabIndex = 54;
            this.chkDate.Text = "Click For Date";
            this.chkDate.UseVisualStyleBackColor = true;
            // 
            // txtToDate
            // 
            this.txtToDate.Location = new System.Drawing.Point(162, 34);
            this.txtToDate.Mask = "##/##/####";
            this.txtToDate.Name = "txtToDate";
            this.txtToDate.Size = new System.Drawing.Size(126, 20);
            this.txtToDate.TabIndex = 7;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(175, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "End of Date:";
            // 
            // panelright
            // 
            this.panelright.BackColor = System.Drawing.Color.Linen;
            this.panelright.Controls.Add(this.btnCancel);
            this.panelright.Controls.Add(this.btnShow);
            this.panelright.Location = new System.Drawing.Point(410, 2);
            this.panelright.Name = "panelright";
            this.panelright.Size = new System.Drawing.Size(83, 279);
            this.panelright.TabIndex = 1;
            // 
            // btnCancel
            // 
            this.btnCancel.Image = global::Accounts.Properties.Resources.gnome_window_close;
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(3, 48);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 63;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnShow
            // 
            this.btnShow.Location = new System.Drawing.Point(2, 10);
            this.btnShow.Name = "btnShow";
            this.btnShow.Size = new System.Drawing.Size(75, 23);
            this.btnShow.TabIndex = 62;
            this.btnShow.Text = "Show";
            this.btnShow.UseVisualStyleBackColor = true;
            this.btnShow.Click += new System.EventHandler(this.btnShow_Click);
            // 
            // frmMBalanceSheetSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(493, 282);
            this.Controls.Add(this.panelright);
            this.Controls.Add(this.panelleft);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.Name = "frmMBalanceSheetSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Balance Sheet Settings";
            this.Load += new System.EventHandler(this.frmMBalanceSheetSettings_Load);
            this.panelleft.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.panelright.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelleft;
        private System.Windows.Forms.Panel panelright;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Button btnToday;
        private System.Windows.Forms.Button btnDate;
        private SComboBox cboMonths;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkDate;
        private System.Windows.Forms.MaskedTextBox txtToDate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnSelectAccClass;
        private System.Windows.Forms.GroupBox groupBox2;
        private SComboBox cboProjectName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnShow;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox chkShowSecLevGrpDet;
        private System.Windows.Forms.CheckBox chkShowZeroBal;
    }
}