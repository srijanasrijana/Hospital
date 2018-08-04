using SComponents;
namespace Accounts
{
    partial class frmPurchaseVoucherConfig
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
            this.tabSalesVouConfig = new System.Windows.Forms.TabControl();
            this.tbNumberingConfig = new System.Windows.Forms.TabPage();
            this.tbNumberingFormat = new System.Windows.Forms.TabPage();
            this.tbVoucherConfig = new System.Windows.Forms.TabPage();
            this.sMaskedTextBox1 = new SMaskedTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabSalesVouConfig.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabSalesVouConfig
            // 
            this.tabSalesVouConfig.Controls.Add(this.tbNumberingConfig);
            this.tabSalesVouConfig.Controls.Add(this.tbNumberingFormat);
            this.tabSalesVouConfig.Controls.Add(this.tbVoucherConfig);
            this.tabSalesVouConfig.Location = new System.Drawing.Point(10, 65);
            this.tabSalesVouConfig.Name = "tabSalesVouConfig";
            this.tabSalesVouConfig.SelectedIndex = 0;
            this.tabSalesVouConfig.Size = new System.Drawing.Size(629, 338);
            this.tabSalesVouConfig.TabIndex = 5;
            // 
            // tbNumberingConfig
            // 
            this.tbNumberingConfig.Location = new System.Drawing.Point(4, 22);
            this.tbNumberingConfig.Name = "tbNumberingConfig";
            this.tbNumberingConfig.Padding = new System.Windows.Forms.Padding(3);
            this.tbNumberingConfig.Size = new System.Drawing.Size(621, 312);
            this.tbNumberingConfig.TabIndex = 0;
            this.tbNumberingConfig.Text = "Numbering Configuration";
            this.tbNumberingConfig.UseVisualStyleBackColor = true;
            // 
            // tbNumberingFormat
            // 
            this.tbNumberingFormat.Location = new System.Drawing.Point(4, 22);
            this.tbNumberingFormat.Name = "tbNumberingFormat";
            this.tbNumberingFormat.Padding = new System.Windows.Forms.Padding(3);
            this.tbNumberingFormat.Size = new System.Drawing.Size(621, 312);
            this.tbNumberingFormat.TabIndex = 1;
            this.tbNumberingFormat.Text = "Numbering Format";
            this.tbNumberingFormat.UseVisualStyleBackColor = true;
            // 
            // tbVoucherConfig
            // 
            this.tbVoucherConfig.Location = new System.Drawing.Point(4, 22);
            this.tbVoucherConfig.Name = "tbVoucherConfig";
            this.tbVoucherConfig.Padding = new System.Windows.Forms.Padding(3);
            this.tbVoucherConfig.Size = new System.Drawing.Size(621, 312);
            this.tbVoucherConfig.TabIndex = 2;
            this.tbVoucherConfig.Text = "Voucher Configuration";
            this.tbVoucherConfig.UseVisualStyleBackColor = true;
            // 
            // sMaskedTextBox1
            // 
            this.sMaskedTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.sMaskedTextBox1.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.sMaskedTextBox1.FocusLostColor = System.Drawing.Color.White;
            this.sMaskedTextBox1.Location = new System.Drawing.Point(94, 24);
            this.sMaskedTextBox1.Name = "sMaskedTextBox1";
            this.sMaskedTextBox1.Size = new System.Drawing.Size(192, 20);
            this.sMaskedTextBox1.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Series Name";
            // 
            // frmPurchaseVoucherConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(665, 426);
            this.Controls.Add(this.tabSalesVouConfig);
            this.Controls.Add(this.sMaskedTextBox1);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "frmPurchaseVoucherConfig";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Purchase Voucher Configuration";
            this.Load += new System.EventHandler(this.frmPurchaseVoucherConfig_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmPurchaseVoucherConfig_KeyDown);
            this.tabSalesVouConfig.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabSalesVouConfig;
        private System.Windows.Forms.TabPage tbNumberingConfig;
        private System.Windows.Forms.TabPage tbNumberingFormat;
        private System.Windows.Forms.TabPage tbVoucherConfig;
        private SMaskedTextBox sMaskedTextBox1;
        private System.Windows.Forms.Label label1;
    }
}