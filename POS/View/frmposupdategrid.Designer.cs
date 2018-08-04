namespace POS.POSInventory
{
    partial class frmposupdategrid
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
            this.lblproductname = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnupdate = new System.Windows.Forms.Button();
            this.btndelete = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btncancel = new System.Windows.Forms.Button();
            this.txtquantity = new System.Windows.Forms.TextBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.btn7 = new System.Windows.Forms.Button();
            this.btn8 = new System.Windows.Forms.Button();
            this.btn9 = new System.Windows.Forms.Button();
            this.btn4 = new System.Windows.Forms.Button();
            this.btn5 = new System.Windows.Forms.Button();
            this.btn6 = new System.Windows.Forms.Button();
            this.btn1 = new System.Windows.Forms.Button();
            this.btn2 = new System.Windows.Forms.Button();
            this.btn3 = new System.Windows.Forms.Button();
            this.btn0 = new System.Windows.Forms.Button();
            this.btnDot = new System.Windows.Forms.Button();
            this.btnClr = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblproductname
            // 
            this.lblproductname.AutoSize = true;
            this.lblproductname.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblproductname.Location = new System.Drawing.Point(3, 5);
            this.lblproductname.Name = "lblproductname";
            this.lblproductname.Size = new System.Drawing.Size(136, 22);
            this.lblproductname.TabIndex = 0;
            this.lblproductname.Text = "Product Name";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(8, 86);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(128, 20);
            this.label2.TabIndex = 1;
            this.label2.Text = "Change Quantity";
            // 
            // btnupdate
            // 
            this.btnupdate.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnupdate.Location = new System.Drawing.Point(14, 134);
            this.btnupdate.Name = "btnupdate";
            this.btnupdate.Size = new System.Drawing.Size(75, 54);
            this.btnupdate.TabIndex = 2;
            this.btnupdate.Text = "Update";
            this.btnupdate.UseVisualStyleBackColor = true;
            this.btnupdate.Click += new System.EventHandler(this.btnupdate_Click);
            this.btnupdate.Enter += new System.EventHandler(this.btnupdate_Enter);
            // 
            // btndelete
            // 
            this.btndelete.Enabled = false;
            this.btndelete.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btndelete.Location = new System.Drawing.Point(111, 134);
            this.btndelete.Name = "btndelete";
            this.btndelete.Size = new System.Drawing.Size(75, 54);
            this.btndelete.TabIndex = 3;
            this.btndelete.Text = "Delete";
            this.btndelete.UseVisualStyleBackColor = true;
            this.btndelete.Visible = false;
            this.btndelete.Click += new System.EventHandler(this.btndelete_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.panel1.Controls.Add(this.btncancel);
            this.panel1.Controls.Add(this.txtquantity);
            this.panel1.Controls.Add(this.lblproductname);
            this.panel1.Controls.Add(this.btndelete);
            this.panel1.Controls.Add(this.btnupdate);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Location = new System.Drawing.Point(273, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(294, 196);
            this.panel1.TabIndex = 5;
            // 
            // btncancel
            // 
            this.btncancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btncancel.Location = new System.Drawing.Point(212, 134);
            this.btncancel.Name = "btncancel";
            this.btncancel.Size = new System.Drawing.Size(75, 54);
            this.btncancel.TabIndex = 6;
            this.btncancel.Text = "Cancel";
            this.btncancel.UseVisualStyleBackColor = true;
            this.btncancel.Click += new System.EventHandler(this.btncancel_Click);
            // 
            // txtquantity
            // 
            this.txtquantity.BackColor = System.Drawing.Color.White;
            this.txtquantity.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtquantity.Location = new System.Drawing.Point(145, 80);
            this.txtquantity.Name = "txtquantity";
            this.txtquantity.Size = new System.Drawing.Size(144, 33);
            this.txtquantity.TabIndex = 5;
            this.txtquantity.Enter += new System.EventHandler(this.txtquantity_Enter);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.flowLayoutPanel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.flowLayoutPanel1.Controls.Add(this.btn7);
            this.flowLayoutPanel1.Controls.Add(this.btn8);
            this.flowLayoutPanel1.Controls.Add(this.btn9);
            this.flowLayoutPanel1.Controls.Add(this.btn4);
            this.flowLayoutPanel1.Controls.Add(this.btn5);
            this.flowLayoutPanel1.Controls.Add(this.btn6);
            this.flowLayoutPanel1.Controls.Add(this.btn1);
            this.flowLayoutPanel1.Controls.Add(this.btn2);
            this.flowLayoutPanel1.Controls.Add(this.btn3);
            this.flowLayoutPanel1.Controls.Add(this.btn0);
            this.flowLayoutPanel1.Controls.Add(this.btnDot);
            this.flowLayoutPanel1.Controls.Add(this.btnClr);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(270, 393);
            this.flowLayoutPanel1.TabIndex = 6;
            // 
            // btn7
            // 
            this.btn7.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.btn7.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btn7.Font = new System.Drawing.Font("Tahoma", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn7.ForeColor = System.Drawing.Color.MidnightBlue;
            this.btn7.Location = new System.Drawing.Point(3, 3);
            this.btn7.Name = "btn7";
            this.btn7.Padding = new System.Windows.Forms.Padding(7, 0, 0, 0);
            this.btn7.Size = new System.Drawing.Size(83, 91);
            this.btn7.TabIndex = 0;
            this.btn7.Tag = "7";
            this.btn7.Text = "7";
            this.btn7.UseVisualStyleBackColor = false;
            this.btn7.Click += new System.EventHandler(this.btn7_Click);
            // 
            // btn8
            // 
            this.btn8.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.btn8.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btn8.Font = new System.Drawing.Font("Tahoma", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn8.ForeColor = System.Drawing.Color.MidnightBlue;
            this.btn8.Location = new System.Drawing.Point(92, 3);
            this.btn8.Name = "btn8";
            this.btn8.Padding = new System.Windows.Forms.Padding(7, 0, 0, 0);
            this.btn8.Size = new System.Drawing.Size(83, 91);
            this.btn8.TabIndex = 1;
            this.btn8.Tag = "8";
            this.btn8.Text = "8";
            this.btn8.UseVisualStyleBackColor = false;
            this.btn8.Click += new System.EventHandler(this.btn8_Click);
            // 
            // btn9
            // 
            this.btn9.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.btn9.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btn9.Font = new System.Drawing.Font("Tahoma", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn9.ForeColor = System.Drawing.Color.MidnightBlue;
            this.btn9.Location = new System.Drawing.Point(181, 3);
            this.btn9.Name = "btn9";
            this.btn9.Padding = new System.Windows.Forms.Padding(7, 0, 0, 0);
            this.btn9.Size = new System.Drawing.Size(83, 91);
            this.btn9.TabIndex = 2;
            this.btn9.Tag = "9";
            this.btn9.Text = "9";
            this.btn9.UseVisualStyleBackColor = false;
            this.btn9.Click += new System.EventHandler(this.btn9_Click);
            // 
            // btn4
            // 
            this.btn4.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.btn4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btn4.Font = new System.Drawing.Font("Tahoma", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn4.ForeColor = System.Drawing.Color.MidnightBlue;
            this.btn4.Location = new System.Drawing.Point(3, 100);
            this.btn4.Name = "btn4";
            this.btn4.Padding = new System.Windows.Forms.Padding(7, 0, 0, 0);
            this.btn4.Size = new System.Drawing.Size(83, 91);
            this.btn4.TabIndex = 3;
            this.btn4.Tag = "4";
            this.btn4.Text = "4";
            this.btn4.UseVisualStyleBackColor = false;
            this.btn4.Click += new System.EventHandler(this.btn4_Click);
            // 
            // btn5
            // 
            this.btn5.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.btn5.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btn5.Font = new System.Drawing.Font("Tahoma", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn5.ForeColor = System.Drawing.Color.MidnightBlue;
            this.btn5.Location = new System.Drawing.Point(92, 100);
            this.btn5.Name = "btn5";
            this.btn5.Padding = new System.Windows.Forms.Padding(7, 0, 0, 0);
            this.btn5.Size = new System.Drawing.Size(83, 91);
            this.btn5.TabIndex = 4;
            this.btn5.Tag = "5";
            this.btn5.Text = "5";
            this.btn5.UseVisualStyleBackColor = false;
            this.btn5.Click += new System.EventHandler(this.btn5_Click);
            // 
            // btn6
            // 
            this.btn6.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.btn6.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btn6.Font = new System.Drawing.Font("Tahoma", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn6.ForeColor = System.Drawing.Color.MidnightBlue;
            this.btn6.Location = new System.Drawing.Point(181, 100);
            this.btn6.Name = "btn6";
            this.btn6.Padding = new System.Windows.Forms.Padding(7, 0, 0, 0);
            this.btn6.Size = new System.Drawing.Size(83, 91);
            this.btn6.TabIndex = 5;
            this.btn6.Tag = "6";
            this.btn6.Text = "6";
            this.btn6.UseVisualStyleBackColor = false;
            this.btn6.Click += new System.EventHandler(this.btn6_Click);
            // 
            // btn1
            // 
            this.btn1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.btn1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btn1.Font = new System.Drawing.Font("Tahoma", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn1.ForeColor = System.Drawing.Color.MidnightBlue;
            this.btn1.Location = new System.Drawing.Point(3, 197);
            this.btn1.Name = "btn1";
            this.btn1.Padding = new System.Windows.Forms.Padding(7, 0, 0, 0);
            this.btn1.Size = new System.Drawing.Size(83, 91);
            this.btn1.TabIndex = 6;
            this.btn1.Tag = "1";
            this.btn1.Text = "1";
            this.btn1.UseVisualStyleBackColor = false;
            this.btn1.Click += new System.EventHandler(this.btn1_Click);
            // 
            // btn2
            // 
            this.btn2.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.btn2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btn2.Font = new System.Drawing.Font("Tahoma", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn2.ForeColor = System.Drawing.Color.MidnightBlue;
            this.btn2.Location = new System.Drawing.Point(92, 197);
            this.btn2.Name = "btn2";
            this.btn2.Padding = new System.Windows.Forms.Padding(7, 0, 0, 0);
            this.btn2.Size = new System.Drawing.Size(83, 91);
            this.btn2.TabIndex = 7;
            this.btn2.Tag = "2";
            this.btn2.Text = "2";
            this.btn2.UseVisualStyleBackColor = false;
            this.btn2.Click += new System.EventHandler(this.btn2_Click);
            // 
            // btn3
            // 
            this.btn3.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.btn3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btn3.Font = new System.Drawing.Font("Tahoma", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn3.ForeColor = System.Drawing.Color.MidnightBlue;
            this.btn3.Location = new System.Drawing.Point(181, 197);
            this.btn3.Name = "btn3";
            this.btn3.Padding = new System.Windows.Forms.Padding(7, 0, 0, 0);
            this.btn3.Size = new System.Drawing.Size(83, 91);
            this.btn3.TabIndex = 8;
            this.btn3.Tag = "3";
            this.btn3.Text = "3";
            this.btn3.UseVisualStyleBackColor = false;
            this.btn3.Click += new System.EventHandler(this.btn3_Click);
            // 
            // btn0
            // 
            this.btn0.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.btn0.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btn0.Font = new System.Drawing.Font("Tahoma", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn0.ForeColor = System.Drawing.Color.MidnightBlue;
            this.btn0.Location = new System.Drawing.Point(3, 294);
            this.btn0.Name = "btn0";
            this.btn0.Padding = new System.Windows.Forms.Padding(7, 0, 0, 0);
            this.btn0.Size = new System.Drawing.Size(83, 91);
            this.btn0.TabIndex = 9;
            this.btn0.Tag = "0";
            this.btn0.Text = "0";
            this.btn0.UseVisualStyleBackColor = false;
            this.btn0.Click += new System.EventHandler(this.btn0_Click);
            // 
            // btnDot
            // 
            this.btnDot.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.btnDot.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnDot.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDot.ForeColor = System.Drawing.Color.MidnightBlue;
            this.btnDot.Location = new System.Drawing.Point(92, 294);
            this.btnDot.Name = "btnDot";
            this.btnDot.Padding = new System.Windows.Forms.Padding(7, 0, 0, 0);
            this.btnDot.Size = new System.Drawing.Size(83, 91);
            this.btnDot.TabIndex = 10;
            this.btnDot.Tag = ".";
            this.btnDot.Text = "Back Space";
            this.btnDot.UseVisualStyleBackColor = false;
            this.btnDot.Click += new System.EventHandler(this.btnDot_Click);
            // 
            // btnClr
            // 
            this.btnClr.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.btnClr.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnClr.Font = new System.Drawing.Font("Tahoma", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClr.ForeColor = System.Drawing.Color.MidnightBlue;
            this.btnClr.Location = new System.Drawing.Point(181, 294);
            this.btnClr.Name = "btnClr";
            this.btnClr.Padding = new System.Windows.Forms.Padding(7, 0, 0, 0);
            this.btnClr.Size = new System.Drawing.Size(83, 91);
            this.btnClr.TabIndex = 11;
            this.btnClr.Tag = "Clr";
            this.btnClr.Text = "Clr";
            this.btnClr.UseVisualStyleBackColor = false;
            this.btnClr.Click += new System.EventHandler(this.btnClr_Click);
            // 
            // frmposupdategrid
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(572, 393);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.panel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmposupdategrid";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Update Quantity";
            this.Load += new System.EventHandler(this.frmposupdategrid_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblproductname;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnupdate;
        private System.Windows.Forms.Button btndelete;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button btn7;
        private System.Windows.Forms.Button btn8;
        private System.Windows.Forms.Button btn9;
        private System.Windows.Forms.Button btn4;
        private System.Windows.Forms.Button btn5;
        private System.Windows.Forms.Button btn6;
        private System.Windows.Forms.Button btn1;
        private System.Windows.Forms.Button btn2;
        private System.Windows.Forms.Button btn3;
        private System.Windows.Forms.Button btn0;
        private System.Windows.Forms.Button btnDot;
        private System.Windows.Forms.Button btnClr;
        private System.Windows.Forms.TextBox txtquantity;
        private System.Windows.Forms.Button btncancel;
    }
}