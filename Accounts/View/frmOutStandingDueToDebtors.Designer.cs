using SComponents;
namespace Accounts
{
    partial class frmOutStandingDueToDebtors
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmOutStandingDueToDebtors));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.sMaskedTextBox1 = new SMaskedTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnDateDD = new System.Windows.Forms.Button();
            this.txtDate = new SMaskedTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbParty = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.sMaskedTextBox1);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.btnDateDD);
            this.groupBox1.Controls.Add(this.txtDate);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.cmbParty);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.checkBox1);
            this.groupBox1.Location = new System.Drawing.Point(5, 1);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(266, 155);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // sMaskedTextBox1
            // 
            this.sMaskedTextBox1.BackColor = System.Drawing.Color.White;
            this.sMaskedTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.sMaskedTextBox1.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.sMaskedTextBox1.FocusLostColor = System.Drawing.Color.White;
            this.sMaskedTextBox1.Location = new System.Drawing.Point(93, 117);
            this.sMaskedTextBox1.Name = "sMaskedTextBox1";
            this.sMaskedTextBox1.Size = new System.Drawing.Size(120, 20);
            this.sMaskedTextBox1.TabIndex = 27;
            this.sMaskedTextBox1.Text = "0.0";
            this.sMaskedTextBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(4, 119);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(42, 13);
            this.label3.TabIndex = 26;
            this.label3.Text = "Interest";
            // 
            // btnDateDD
            // 
            this.btnDateDD.Image = global::Accounts.Properties.Resources.dateIcon;
            this.btnDateDD.Location = new System.Drawing.Point(219, 80);
            this.btnDateDD.Name = "btnDateDD";
            this.btnDateDD.Size = new System.Drawing.Size(26, 23);
            this.btnDateDD.TabIndex = 25;
            this.btnDateDD.UseVisualStyleBackColor = true;
            this.btnDateDD.Click += new System.EventHandler(this.btnDate_Click);
            // 
            // txtDate
            // 
            this.txtDate.BackColor = System.Drawing.Color.White;
            this.txtDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDate.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.txtDate.FocusLostColor = System.Drawing.Color.White;
            this.txtDate.Location = new System.Drawing.Point(93, 83);
            this.txtDate.Mask = "0000/00/00";
            this.txtDate.Name = "txtDate";
            this.txtDate.Size = new System.Drawing.Size(120, 20);
            this.txtDate.TabIndex = 24;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 85);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "As On Date";
            // 
            // cmbParty
            // 
            this.cmbParty.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbParty.FormattingEnabled = true;
            this.cmbParty.Location = new System.Drawing.Point(93, 48);
            this.cmbParty.Name = "cmbParty";
            this.cmbParty.Size = new System.Drawing.Size(150, 21);
            this.cmbParty.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 51);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Select Party";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(7, 19);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(72, 17);
            this.checkBox1.TabIndex = 1;
            this.checkBox1.Text = "All Parties";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 162);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Show";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(98, 162);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "Close";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // frmOutStandingDueToDebtors
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(275, 200);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmOutStandingDueToDebtors";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Debtors-Due";
            this.Load += new System.EventHandler(this.frmOutStandingDueToDebtors_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cmbParty;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Label label2;
        private SMaskedTextBox sMaskedTextBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnDateDD;
        private SMaskedTextBox txtDate;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}