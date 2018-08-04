namespace Inventory
{
    partial class frmtenderamount
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txt_tenderamount = new System.Windows.Forms.TextBox();
            this.txt_paidamount = new System.Windows.Forms.TextBox();
            this.txt_returnamount = new System.Windows.Forms.TextBox();
            this.btn_close = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(144, 23);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(161, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "Tender Amount";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(143, 195);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(155, 25);
            this.label2.TabIndex = 1;
            this.label2.Text = "Return Amount";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(149, 107);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(135, 25);
            this.label3.TabIndex = 2;
            this.label3.Text = "Paid Amount";
            // 
            // txt_tenderamount
            // 
            this.txt_tenderamount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txt_tenderamount.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_tenderamount.Location = new System.Drawing.Point(111, 60);
            this.txt_tenderamount.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txt_tenderamount.Name = "txt_tenderamount";
            this.txt_tenderamount.ReadOnly = true;
            this.txt_tenderamount.Size = new System.Drawing.Size(200, 30);
            this.txt_tenderamount.TabIndex = 3;
            this.txt_tenderamount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txt_paidamount
            // 
            this.txt_paidamount.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_paidamount.Location = new System.Drawing.Point(111, 141);
            this.txt_paidamount.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txt_paidamount.Name = "txt_paidamount";
            this.txt_paidamount.Size = new System.Drawing.Size(200, 30);
            this.txt_paidamount.TabIndex = 4;
            this.txt_paidamount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txt_paidamount.TextChanged += new System.EventHandler(this.txt_paidamount_TextChanged);
            this.txt_paidamount.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_paidamount_KeyDown);
            this.txt_paidamount.Leave += new System.EventHandler(this.txt_paidamount_Leave);
            // 
            // txt_returnamount
            // 
            this.txt_returnamount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txt_returnamount.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_returnamount.Location = new System.Drawing.Point(111, 232);
            this.txt_returnamount.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txt_returnamount.Name = "txt_returnamount";
            this.txt_returnamount.ReadOnly = true;
            this.txt_returnamount.Size = new System.Drawing.Size(200, 48);
            this.txt_returnamount.TabIndex = 5;
            this.txt_returnamount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // btn_close
            // 
            this.btn_close.Location = new System.Drawing.Point(148, 312);
            this.btn_close.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btn_close.Name = "btn_close";
            this.btn_close.Size = new System.Drawing.Size(112, 35);
            this.btn_close.TabIndex = 6;
            this.btn_close.Text = "Close";
            this.btn_close.UseVisualStyleBackColor = true;
            this.btn_close.Click += new System.EventHandler(this.btn_close_Click);
            // 
            // frmtenderamount
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(428, 365);
            this.Controls.Add(this.btn_close);
            this.Controls.Add(this.txt_returnamount);
            this.Controls.Add(this.txt_paidamount);
            this.Controls.Add(this.txt_tenderamount);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "frmtenderamount";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Tender Amount";
            this.Load += new System.EventHandler(this.frmtenderamount_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txt_tenderamount;
        private System.Windows.Forms.TextBox txt_paidamount;
        private System.Windows.Forms.TextBox txt_returnamount;
        private System.Windows.Forms.Button btn_close;
    }
}