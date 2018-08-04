using SComponents;
namespace AccSwift
{
    partial class frmCompanyFiscalYear
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
            this.label12 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnDateStartFY = new System.Windows.Forms.Button();
            this.txtDateFYStart = new SMaskedTextBox();
            this.btnDateBookBegin = new System.Windows.Forms.Button();
            this.txtDateBookBegin = new SMaskedTextBox();
            this.SuspendLayout();
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(67, 58);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(88, 13);
            this.label12.TabIndex = 77;
            this.label12.Text = "Fiscal Year From:";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(45, 85);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(110, 13);
            this.label14.TabIndex = 76;
            this.label14.Text = "Books Begining From:";
            // 
            // btnSave
            // 
            this.btnSave.Image = global::Inventory.Properties.Resources.save;
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSave.Location = new System.Drawing.Point(161, 122);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 85;
            this.btnSave.Text = "&Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnDateStartFY
            // 
            this.btnDateStartFY.Image = global::Inventory.Properties.Resources.dateIcon;
            this.btnDateStartFY.Location = new System.Drawing.Point(237, 55);
            this.btnDateStartFY.Name = "btnDateStartFY";
            this.btnDateStartFY.Size = new System.Drawing.Size(26, 23);
            this.btnDateStartFY.TabIndex = 87;
            this.btnDateStartFY.UseVisualStyleBackColor = true;
            this.btnDateStartFY.Click += new System.EventHandler(this.btnDateStartFY_Click);
            // 
            // txtDateFYStart
            // 
            this.txtDateFYStart.BackColor = System.Drawing.Color.White;
            this.txtDateFYStart.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDateFYStart.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.txtDateFYStart.FocusLostColor = System.Drawing.Color.White;
            this.txtDateFYStart.Location = new System.Drawing.Point(161, 56);
            this.txtDateFYStart.Mask = "0000/00/00";
            this.txtDateFYStart.Name = "txtDateFYStart";
            this.txtDateFYStart.Size = new System.Drawing.Size(75, 20);
            this.txtDateFYStart.TabIndex = 86;
            // 
            // btnDateBookBegin
            // 
            this.btnDateBookBegin.Image = global::Inventory.Properties.Resources.dateIcon;
            this.btnDateBookBegin.Location = new System.Drawing.Point(237, 80);
            this.btnDateBookBegin.Name = "btnDateBookBegin";
            this.btnDateBookBegin.Size = new System.Drawing.Size(26, 23);
            this.btnDateBookBegin.TabIndex = 89;
            this.btnDateBookBegin.UseVisualStyleBackColor = true;
            this.btnDateBookBegin.Click += new System.EventHandler(this.btnDateBookBegin_Click);
            // 
            // txtDateBookBegin
            // 
            this.txtDateBookBegin.BackColor = System.Drawing.Color.White;
            this.txtDateBookBegin.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDateBookBegin.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.txtDateBookBegin.FocusLostColor = System.Drawing.Color.White;
            this.txtDateBookBegin.Location = new System.Drawing.Point(161, 82);
            this.txtDateBookBegin.Mask = "0000/00/00";
            this.txtDateBookBegin.Name = "txtDateBookBegin";
            this.txtDateBookBegin.Size = new System.Drawing.Size(75, 20);
            this.txtDateBookBegin.TabIndex = 88;
            // 
            // frmCompanyFiscalYear
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(294, 157);
            this.Controls.Add(this.btnDateBookBegin);
            this.Controls.Add(this.txtDateBookBegin);
            this.Controls.Add(this.btnDateStartFY);
            this.Controls.Add(this.txtDateFYStart);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label14);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmCompanyFiscalYear";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Company Fiscal Year";
            this.Load += new System.EventHandler(this.frmCompanyFiscalYear_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnDateStartFY;
        private SMaskedTextBox txtDateFYStart;
        private System.Windows.Forms.Button btnDateBookBegin;
        private SMaskedTextBox txtDateBookBegin;
    }
}