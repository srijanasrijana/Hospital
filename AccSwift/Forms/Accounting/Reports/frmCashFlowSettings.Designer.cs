using SComponents;
namespace Inventory
{
    partial class frmCashFlowSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCashFlowSettings));
            this.rdbtnDetails = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.btnDate = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtToDate = new System.Windows.Forms.MaskedTextBox();
            this.txtFromDate = new System.Windows.Forms.MaskedTextBox();
            this.btnShow = new System.Windows.Forms.Button();
            this.rdbtnSummary = new System.Windows.Forms.RadioButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.rdbtnGroupwise = new System.Windows.Forms.RadioButton();
            this.rdbtnAccountwise = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSelectAccClass = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.cboProjectName = new SComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnsavestate = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.cboMonths = new SComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // rdbtnDetails
            // 
            this.rdbtnDetails.AutoSize = true;
            this.rdbtnDetails.Location = new System.Drawing.Point(114, 19);
            this.rdbtnDetails.Name = "rdbtnDetails";
            this.rdbtnDetails.Size = new System.Drawing.Size(57, 17);
            this.rdbtnDetails.TabIndex = 1;
            this.rdbtnDetails.TabStop = true;
            this.rdbtnDetails.Text = "Details";
            this.rdbtnDetails.UseVisualStyleBackColor = true;
            this.rdbtnDetails.CheckedChanged += new System.EventHandler(this.rdbtnDetails_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox1.Controls.Add(this.cboMonths);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.btnDate);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtToDate);
            this.groupBox1.Controls.Add(this.txtFromDate);
            this.groupBox1.Location = new System.Drawing.Point(2, 92);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(348, 110);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Date Selection:";
            // 
            // button1
            // 
            this.button1.Image = global::Inventory.Properties.Resources.dateIcon;
            this.button1.Location = new System.Drawing.Point(231, 44);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(26, 25);
            this.button1.TabIndex = 3;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnDate
            // 
            this.btnDate.Image = global::Inventory.Properties.Resources.dateIcon;
            this.btnDate.Location = new System.Drawing.Point(231, 18);
            this.btnDate.Name = "btnDate";
            this.btnDate.Size = new System.Drawing.Size(26, 25);
            this.btnDate.TabIndex = 1;
            this.btnDate.UseVisualStyleBackColor = true;
            this.btnDate.Click += new System.EventHandler(this.btnDate_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(72, 51);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "To Date";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(72, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "From Date";
            // 
            // txtToDate
            // 
            this.txtToDate.Location = new System.Drawing.Point(134, 44);
            this.txtToDate.Mask = "##/##/####";
            this.txtToDate.Name = "txtToDate";
            this.txtToDate.Size = new System.Drawing.Size(79, 20);
            this.txtToDate.TabIndex = 2;
            this.txtToDate.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtToDate_KeyDown);
            // 
            // txtFromDate
            // 
            this.txtFromDate.Location = new System.Drawing.Point(134, 18);
            this.txtFromDate.Mask = "##/##/####";
            this.txtFromDate.Name = "txtFromDate";
            this.txtFromDate.Size = new System.Drawing.Size(79, 20);
            this.txtFromDate.TabIndex = 0;
            this.txtFromDate.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtFromDate_KeyDown);
            // 
            // btnShow
            // 
            this.btnShow.Location = new System.Drawing.Point(9, 10);
            this.btnShow.Name = "btnShow";
            this.btnShow.Size = new System.Drawing.Size(75, 23);
            this.btnShow.TabIndex = 3;
            this.btnShow.Text = "Show";
            this.btnShow.UseVisualStyleBackColor = true;
            this.btnShow.Click += new System.EventHandler(this.btnShow_Click);
            // 
            // rdbtnSummary
            // 
            this.rdbtnSummary.AutoSize = true;
            this.rdbtnSummary.Location = new System.Drawing.Point(9, 19);
            this.rdbtnSummary.Name = "rdbtnSummary";
            this.rdbtnSummary.Size = new System.Drawing.Size(68, 17);
            this.rdbtnSummary.TabIndex = 0;
            this.rdbtnSummary.TabStop = true;
            this.rdbtnSummary.Text = "Summary";
            this.rdbtnSummary.UseVisualStyleBackColor = true;
            this.rdbtnSummary.CheckedChanged += new System.EventHandler(this.rdbtnSummary_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.rdbtnGroupwise);
            this.panel1.Controls.Add(this.rdbtnAccountwise);
            this.panel1.Location = new System.Drawing.Point(114, 39);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(220, 26);
            this.panel1.TabIndex = 6;
            // 
            // rdbtnGroupwise
            // 
            this.rdbtnGroupwise.AutoSize = true;
            this.rdbtnGroupwise.Location = new System.Drawing.Point(128, 5);
            this.rdbtnGroupwise.Name = "rdbtnGroupwise";
            this.rdbtnGroupwise.Size = new System.Drawing.Size(81, 17);
            this.rdbtnGroupwise.TabIndex = 1;
            this.rdbtnGroupwise.TabStop = true;
            this.rdbtnGroupwise.Text = "Group-Wise";
            this.rdbtnGroupwise.UseVisualStyleBackColor = true;
            // 
            // rdbtnAccountwise
            // 
            this.rdbtnAccountwise.AutoSize = true;
            this.rdbtnAccountwise.Location = new System.Drawing.Point(3, 5);
            this.rdbtnAccountwise.Name = "rdbtnAccountwise";
            this.rdbtnAccountwise.Size = new System.Drawing.Size(121, 17);
            this.rdbtnAccountwise.TabIndex = 0;
            this.rdbtnAccountwise.TabStop = true;
            this.rdbtnAccountwise.Text = "Account Head-Wise";
            this.rdbtnAccountwise.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox2.Controls.Add(this.panel1);
            this.groupBox2.Controls.Add(this.rdbtnSummary);
            this.groupBox2.Controls.Add(this.rdbtnDetails);
            this.groupBox2.Controls.Add(this.groupBox3);
            this.groupBox2.Location = new System.Drawing.Point(0, 4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(350, 82);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            // 
            // groupBox3
            // 
            this.groupBox3.Location = new System.Drawing.Point(104, 11);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(238, 64);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            // 
            // btnCancel
            // 
            this.btnCancel.Image = global::Inventory.Properties.Resources.gnome_window_close;
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(9, 43);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSelectAccClass
            // 
            this.btnSelectAccClass.Location = new System.Drawing.Point(178, 18);
            this.btnSelectAccClass.Name = "btnSelectAccClass";
            this.btnSelectAccClass.Size = new System.Drawing.Size(162, 23);
            this.btnSelectAccClass.TabIndex = 2;
            this.btnSelectAccClass.Text = "Select Account Class";
            this.btnSelectAccClass.UseVisualStyleBackColor = true;
            this.btnSelectAccClass.Click += new System.EventHandler(this.btnSelectAccClass_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.BackColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox4.Controls.Add(this.cboProjectName);
            this.groupBox4.Controls.Add(this.label5);
            this.groupBox4.Location = new System.Drawing.Point(3, 267);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(347, 60);
            this.groupBox4.TabIndex = 62;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Project";
            // 
            // cboProjectName
            // 
            this.cboProjectName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboProjectName.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.cboProjectName.FocusLostColor = System.Drawing.Color.White;
            this.cboProjectName.FormattingEnabled = true;
            this.cboProjectName.Location = new System.Drawing.Point(177, 19);
            this.cboProjectName.Name = "cboProjectName";
            this.cboProjectName.Size = new System.Drawing.Size(162, 21);
            this.cboProjectName.TabIndex = 59;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(115, 22);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(46, 13);
            this.label5.TabIndex = 58;
            this.label5.Text = "Project: ";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.LightSteelBlue;
            this.panel2.Controls.Add(this.btnsavestate);
            this.panel2.Controls.Add(this.btnShow);
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Location = new System.Drawing.Point(351, -1);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(93, 328);
            this.panel2.TabIndex = 63;
            // 
            // btnsavestate
            // 
            this.btnsavestate.Location = new System.Drawing.Point(9, 75);
            this.btnsavestate.Name = "btnsavestate";
            this.btnsavestate.Size = new System.Drawing.Size(75, 23);
            this.btnsavestate.TabIndex = 67;
            this.btnsavestate.Text = "Save State";
            this.btnsavestate.UseVisualStyleBackColor = true;
            this.btnsavestate.Click += new System.EventHandler(this.btnsavestate_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.BackColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox5.Controls.Add(this.btnSelectAccClass);
            this.groupBox5.Location = new System.Drawing.Point(2, 208);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(347, 51);
            this.groupBox5.TabIndex = 64;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Accounting Class";
            // 
            // cboMonths
            // 
            this.cboMonths.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMonths.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.cboMonths.FocusLostColor = System.Drawing.Color.White;
            this.cboMonths.FormattingEnabled = true;
            this.cboMonths.Location = new System.Drawing.Point(147, 73);
            this.cboMonths.Name = "cboMonths";
            this.cboMonths.Size = new System.Drawing.Size(110, 21);
            this.cboMonths.TabIndex = 65;
            this.cboMonths.SelectedIndexChanged += new System.EventHandler(this.cboMonths_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(72, 76);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 13);
            this.label1.TabIndex = 64;
            this.label1.Text = "End of Month";
            // 
            // frmCashFlowSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(442, 334);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "frmCashFlowSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "CashFlow Settings";
            this.Load += new System.EventHandler(this.frmCashFlowSettings_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmCashFlowSettings_KeyDown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RadioButton rdbtnDetails;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.MaskedTextBox txtFromDate;
        private System.Windows.Forms.MaskedTextBox txtToDate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnShow;
        private System.Windows.Forms.RadioButton rdbtnSummary;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton rdbtnGroupwise;
        private System.Windows.Forms.RadioButton rdbtnAccountwise;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnDate;
        private System.Windows.Forms.Button btnSelectAccClass;
        private System.Windows.Forms.GroupBox groupBox4;
        private SComboBox cboProjectName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Button btnsavestate;
        private SComboBox cboMonths;
        private System.Windows.Forms.Label label1;
    }
}