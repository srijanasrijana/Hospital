using SComponents;
namespace AccSwift
{
    partial class frmFreeze
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmFreeze));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnFreezeStartDate = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.btnFreezeEndDate = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.btnFreeze = new System.Windows.Forms.Button();
            this.chkAllowFreeze = new System.Windows.Forms.CheckBox();
            this.btnDefreeze = new System.Windows.Forms.Button();
            this.txtFreezeEndDate = new SMaskedTextBox();
            this.txtFreezeStartDate = new SMaskedTextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Fiscal Year :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(98, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "2013-04-01";
            // 
            // btnFreezeStartDate
            // 
            this.btnFreezeStartDate.Image = global::Inventory.Properties.Resources.dateIcon;
            this.btnFreezeStartDate.Location = new System.Drawing.Point(170, 71);
            this.btnFreezeStartDate.Name = "btnFreezeStartDate";
            this.btnFreezeStartDate.Size = new System.Drawing.Size(26, 23);
            this.btnFreezeStartDate.TabIndex = 9;
            this.btnFreezeStartDate.UseVisualStyleBackColor = true;
            this.btnFreezeStartDate.Click += new System.EventHandler(this.btnFreezeStartDate_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(31, 75);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Start Date:";
            // 
            // btnFreezeEndDate
            // 
            this.btnFreezeEndDate.Image = global::Inventory.Properties.Resources.dateIcon;
            this.btnFreezeEndDate.Location = new System.Drawing.Point(170, 108);
            this.btnFreezeEndDate.Name = "btnFreezeEndDate";
            this.btnFreezeEndDate.Size = new System.Drawing.Size(26, 23);
            this.btnFreezeEndDate.TabIndex = 12;
            this.btnFreezeEndDate.UseVisualStyleBackColor = true;
            this.btnFreezeEndDate.Click += new System.EventHandler(this.btnFreezeEndDate_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(31, 112);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(55, 13);
            this.label4.TabIndex = 13;
            this.label4.Text = "End Date:";
            // 
            // btnFreeze
            // 
            this.btnFreeze.Location = new System.Drawing.Point(34, 148);
            this.btnFreeze.Name = "btnFreeze";
            this.btnFreeze.Size = new System.Drawing.Size(75, 23);
            this.btnFreeze.TabIndex = 14;
            this.btnFreeze.Text = "&Freeze";
            this.btnFreeze.UseVisualStyleBackColor = true;
            this.btnFreeze.Click += new System.EventHandler(this.btnFreezeDefreeze_Click);
            // 
            // chkAllowFreeze
            // 
            this.chkAllowFreeze.AutoSize = true;
            this.chkAllowFreeze.Location = new System.Drawing.Point(34, 50);
            this.chkAllowFreeze.Name = "chkAllowFreeze";
            this.chkAllowFreeze.Size = new System.Drawing.Size(89, 17);
            this.chkAllowFreeze.TabIndex = 16;
            this.chkAllowFreeze.Text = "Allow Freeze ";
            this.chkAllowFreeze.UseVisualStyleBackColor = true;
            this.chkAllowFreeze.CheckedChanged += new System.EventHandler(this.chkAllowFreeze_CheckedChanged);
            // 
            // btnDefreeze
            // 
            this.btnDefreeze.Location = new System.Drawing.Point(121, 148);
            this.btnDefreeze.Name = "btnDefreeze";
            this.btnDefreeze.Size = new System.Drawing.Size(75, 23);
            this.btnDefreeze.TabIndex = 17;
            this.btnDefreeze.Text = "&Defreeze";
            this.btnDefreeze.UseVisualStyleBackColor = true;
            this.btnDefreeze.Click += new System.EventHandler(this.btnDefreeze_Click);
            // 
            // txtFreezeEndDate
            // 
            this.txtFreezeEndDate.BackColor = System.Drawing.Color.White;
            this.txtFreezeEndDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtFreezeEndDate.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.txtFreezeEndDate.FocusLostColor = System.Drawing.Color.White;
            this.txtFreezeEndDate.Location = new System.Drawing.Point(94, 110);
            this.txtFreezeEndDate.Mask = "0000/00/00";
            this.txtFreezeEndDate.Name = "txtFreezeEndDate";
            this.txtFreezeEndDate.Size = new System.Drawing.Size(75, 20);
            this.txtFreezeEndDate.TabIndex = 11;
            // 
            // txtFreezeStartDate
            // 
            this.txtFreezeStartDate.BackColor = System.Drawing.Color.White;
            this.txtFreezeStartDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtFreezeStartDate.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.txtFreezeStartDate.FocusLostColor = System.Drawing.Color.White;
            this.txtFreezeStartDate.Location = new System.Drawing.Point(94, 73);
            this.txtFreezeStartDate.Mask = "0000/00/00";
            this.txtFreezeStartDate.Name = "txtFreezeStartDate";
            this.txtFreezeStartDate.Size = new System.Drawing.Size(75, 20);
            this.txtFreezeStartDate.TabIndex = 8;
            // 
            // frmFreeze
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(218, 205);
            this.Controls.Add(this.btnDefreeze);
            this.Controls.Add(this.chkAllowFreeze);
            this.Controls.Add(this.btnFreeze);
            this.Controls.Add(this.btnFreezeEndDate);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtFreezeEndDate);
            this.Controls.Add(this.btnFreezeStartDate);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtFreezeStartDate);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmFreeze";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Freeze";
            this.Load += new System.EventHandler(this.frmFreeze_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnFreezeStartDate;
        private System.Windows.Forms.Label label3;
        private SMaskedTextBox txtFreezeStartDate;
        private System.Windows.Forms.Button btnFreezeEndDate;
        private System.Windows.Forms.Label label4;
        private SMaskedTextBox txtFreezeEndDate;
        private System.Windows.Forms.Button btnFreeze;
        private System.Windows.Forms.CheckBox chkAllowFreeze;
        private System.Windows.Forms.Button btnDefreeze;
    }
}