namespace Accounts.View.Reports
{
    partial class frmBudgetReportSetting
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmBudgetReportSetting));
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnSelectAccClass = new System.Windows.Forms.Button();
            this.grpDate = new System.Windows.Forms.GroupBox();
            this.cboMonths = new SComponents.SComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.btnDate = new System.Windows.Forms.Button();
            this.txtFromDate = new System.Windows.Forms.MaskedTextBox();
            this.txtToDate = new System.Windows.Forms.MaskedTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbChooseBudget = new SComponents.SComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnsavestate = new System.Windows.Forms.Button();
            this.btnShow = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.cboProjectName = new SComponents.SComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox2.SuspendLayout();
            this.grpDate.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox2.Controls.Add(this.btnSelectAccClass);
            this.groupBox2.Location = new System.Drawing.Point(5, 162);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(251, 48);
            this.groupBox2.TabIndex = 66;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Accounting Class";
            this.groupBox2.Enter += new System.EventHandler(this.groupBox2_Enter);
            // 
            // btnSelectAccClass
            // 
            this.btnSelectAccClass.Location = new System.Drawing.Point(71, 19);
            this.btnSelectAccClass.Name = "btnSelectAccClass";
            this.btnSelectAccClass.Size = new System.Drawing.Size(148, 23);
            this.btnSelectAccClass.TabIndex = 3;
            this.btnSelectAccClass.Text = "Select Account Class";
            this.btnSelectAccClass.UseVisualStyleBackColor = true;
            this.btnSelectAccClass.Click += new System.EventHandler(this.btnSelectAccClass_Click);
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
            this.grpDate.Location = new System.Drawing.Point(5, 77);
            this.grpDate.Name = "grpDate";
            this.grpDate.Size = new System.Drawing.Size(250, 79);
            this.grpDate.TabIndex = 65;
            this.grpDate.TabStop = false;
            // 
            // cboMonths
            // 
            this.cboMonths.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMonths.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.cboMonths.FocusLostColor = System.Drawing.Color.White;
            this.cboMonths.FormattingEnabled = true;
            this.cboMonths.Location = new System.Drawing.Point(97, 75);
            this.cboMonths.Name = "cboMonths";
            this.cboMonths.Size = new System.Drawing.Size(154, 21);
            this.cboMonths.TabIndex = 63;
            this.cboMonths.Visible = false;
            this.cboMonths.SelectedIndexChanged += new System.EventHandler(this.cboMonths_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 78);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 13);
            this.label1.TabIndex = 62;
            this.label1.Text = "End of Month";
            this.label1.Visible = false;
            // 
            // button1
            // 
            this.button1.Image = ((System.Drawing.Image)(resources.GetObject("button1.Image")));
            this.button1.Location = new System.Drawing.Point(225, 43);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(26, 24);
            this.button1.TabIndex = 5;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnDate
            // 
            this.btnDate.Image = ((System.Drawing.Image)(resources.GetObject("btnDate.Image")));
            this.btnDate.Location = new System.Drawing.Point(224, 11);
            this.btnDate.Name = "btnDate";
            this.btnDate.Size = new System.Drawing.Size(26, 24);
            this.btnDate.TabIndex = 2;
            this.btnDate.UseVisualStyleBackColor = true;
            this.btnDate.Visible = false;
            this.btnDate.Click += new System.EventHandler(this.btnDate_Click);
            // 
            // txtFromDate
            // 
            this.txtFromDate.Location = new System.Drawing.Point(59, 14);
            this.txtFromDate.Mask = "##/##/####";
            this.txtFromDate.Name = "txtFromDate";
            this.txtFromDate.Size = new System.Drawing.Size(159, 20);
            this.txtFromDate.TabIndex = 1;
            this.txtFromDate.MaskInputRejected += new System.Windows.Forms.MaskInputRejectedEventHandler(this.txtFromDate_MaskInputRejected);
            // 
            // txtToDate
            // 
            this.txtToDate.Location = new System.Drawing.Point(59, 47);
            this.txtToDate.Mask = "##/##/####";
            this.txtToDate.Name = "txtToDate";
            this.txtToDate.Size = new System.Drawing.Size(160, 20);
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
            this.label6.Location = new System.Drawing.Point(20, 19);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(33, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "From:";
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.cmbChooseBudget);
            this.groupBox1.Location = new System.Drawing.Point(5, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(250, 65);
            this.groupBox1.TabIndex = 64;
            this.groupBox1.TabStop = false;
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(22, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(117, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Choose Budget Name :";
            // 
            // cmbChooseBudget
            // 
            this.cmbChooseBudget.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cmbChooseBudget.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbChooseBudget.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.cmbChooseBudget.FocusLostColor = System.Drawing.Color.White;
            this.cmbChooseBudget.FormattingEnabled = true;
            this.cmbChooseBudget.Location = new System.Drawing.Point(22, 35);
            this.cmbChooseBudget.Name = "cmbChooseBudget";
            this.cmbChooseBudget.Size = new System.Drawing.Size(197, 21);
            this.cmbChooseBudget.TabIndex = 2;
            this.cmbChooseBudget.SelectedIndexChanged += new System.EventHandler(this.cmbChooseBudget_SelectedIndexChanged);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.LightSteelBlue;
            this.panel1.Controls.Add(this.btnsavestate);
            this.panel1.Controls.Add(this.btnShow);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Location = new System.Drawing.Point(262, 6);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(94, 262);
            this.panel1.TabIndex = 67;
            // 
            // btnsavestate
            // 
            this.btnsavestate.Location = new System.Drawing.Point(12, 72);
            this.btnsavestate.Name = "btnsavestate";
            this.btnsavestate.Size = new System.Drawing.Size(77, 23);
            this.btnsavestate.TabIndex = 65;
            this.btnsavestate.Text = "Save State";
            this.btnsavestate.UseVisualStyleBackColor = true;
            // 
            // btnShow
            // 
            this.btnShow.Location = new System.Drawing.Point(12, 13);
            this.btnShow.Name = "btnShow";
            this.btnShow.Size = new System.Drawing.Size(75, 23);
            this.btnShow.TabIndex = 3;
            this.btnShow.Text = "Show";
            this.btnShow.UseVisualStyleBackColor = true;
            this.btnShow.Click += new System.EventHandler(this.btnShow_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Image = ((System.Drawing.Image)(resources.GetObject("btnCancel.Image")));
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(12, 42);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(76, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox3.Controls.Add(this.cboProjectName);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Location = new System.Drawing.Point(5, 216);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(251, 52);
            this.groupBox3.TabIndex = 68;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Project";
            this.groupBox3.Enter += new System.EventHandler(this.groupBox3_Enter);
            // 
            // cboProjectName
            // 
            this.cboProjectName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboProjectName.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.cboProjectName.FocusLostColor = System.Drawing.Color.White;
            this.cboProjectName.FormattingEnabled = true;
            this.cboProjectName.Location = new System.Drawing.Point(71, 17);
            this.cboProjectName.Name = "cboProjectName";
            this.cboProjectName.Size = new System.Drawing.Size(148, 21);
            this.cboProjectName.TabIndex = 59;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(22, 20);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(46, 13);
            this.label5.TabIndex = 58;
            this.label5.Text = "Project: ";
            // 
            // frmBudgetReportSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(360, 275);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.grpDate);
            this.Controls.Add(this.groupBox1);
            this.Name = "frmBudgetReportSetting";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Budget Report Setting";
            this.Load += new System.EventHandler(this.frmBudgetReportSetting_Load);
            this.groupBox2.ResumeLayout(false);
            this.grpDate.ResumeLayout(false);
            this.grpDate.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnSelectAccClass;
        private System.Windows.Forms.GroupBox grpDate;
        private SComponents.SComboBox cboMonths;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnDate;
        private System.Windows.Forms.MaskedTextBox txtFromDate;
        private System.Windows.Forms.MaskedTextBox txtToDate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupBox1;
        private SComponents.SComboBox cmbChooseBudget;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnsavestate;
        private System.Windows.Forms.Button btnShow;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox3;
        private SComponents.SComboBox cboProjectName;
        private System.Windows.Forms.Label label5;
    }
}