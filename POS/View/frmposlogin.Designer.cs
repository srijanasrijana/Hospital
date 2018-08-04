namespace POS.POSLogIn
{
    partial class frmposlogin
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
            this.txtpin = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panelpin = new System.Windows.Forms.Panel();
            this.panelkey = new System.Windows.Forms.Panel();
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
            this.btnBS = new System.Windows.Forms.Button();
            this.btnEnter = new System.Windows.Forms.Button();
            this.panelpin.SuspendLayout();
            this.panelkey.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtpin
            // 
            this.txtpin.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtpin.Location = new System.Drawing.Point(2, 32);
            this.txtpin.Name = "txtpin";
            this.txtpin.PasswordChar = '*';
            this.txtpin.Size = new System.Drawing.Size(263, 30);
            this.txtpin.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(5, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "PIN";
            // 
            // panelpin
            // 
            this.panelpin.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.panelpin.Controls.Add(this.txtpin);
            this.panelpin.Controls.Add(this.label1);
            this.panelpin.Location = new System.Drawing.Point(0, 1);
            this.panelpin.Name = "panelpin";
            this.panelpin.Size = new System.Drawing.Size(267, 63);
            this.panelpin.TabIndex = 3;
            // 
            // panelkey
            // 
            this.panelkey.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.panelkey.Controls.Add(this.btn7);
            this.panelkey.Controls.Add(this.btn8);
            this.panelkey.Controls.Add(this.btn9);
            this.panelkey.Controls.Add(this.btn4);
            this.panelkey.Controls.Add(this.btn5);
            this.panelkey.Controls.Add(this.btn6);
            this.panelkey.Controls.Add(this.btn1);
            this.panelkey.Controls.Add(this.btn2);
            this.panelkey.Controls.Add(this.btn3);
            this.panelkey.Controls.Add(this.btn0);
            this.panelkey.Controls.Add(this.btnBS);
            this.panelkey.Controls.Add(this.btnEnter);
            this.panelkey.Location = new System.Drawing.Point(-1, 67);
            this.panelkey.Name = "panelkey";
            this.panelkey.Size = new System.Drawing.Size(268, 400);
            this.panelkey.TabIndex = 4;
            // 
            // btn7
            // 
            this.btn7.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.btn7.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btn7.Font = new System.Drawing.Font("Tahoma", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn7.ForeColor = System.Drawing.Color.MidnightBlue;
            this.btn7.Location = new System.Drawing.Point(2, 3);
            this.btn7.Name = "btn7";
            this.btn7.Padding = new System.Windows.Forms.Padding(7, 0, 0, 0);
            this.btn7.Size = new System.Drawing.Size(83, 91);
            this.btn7.TabIndex = 12;
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
            this.btn8.Location = new System.Drawing.Point(91, 3);
            this.btn8.Name = "btn8";
            this.btn8.Padding = new System.Windows.Forms.Padding(7, 0, 0, 0);
            this.btn8.Size = new System.Drawing.Size(83, 91);
            this.btn8.TabIndex = 13;
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
            this.btn9.Location = new System.Drawing.Point(180, 3);
            this.btn9.Name = "btn9";
            this.btn9.Padding = new System.Windows.Forms.Padding(7, 0, 0, 0);
            this.btn9.Size = new System.Drawing.Size(83, 91);
            this.btn9.TabIndex = 14;
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
            this.btn4.Location = new System.Drawing.Point(2, 100);
            this.btn4.Name = "btn4";
            this.btn4.Padding = new System.Windows.Forms.Padding(7, 0, 0, 0);
            this.btn4.Size = new System.Drawing.Size(83, 91);
            this.btn4.TabIndex = 15;
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
            this.btn5.Location = new System.Drawing.Point(91, 100);
            this.btn5.Name = "btn5";
            this.btn5.Padding = new System.Windows.Forms.Padding(7, 0, 0, 0);
            this.btn5.Size = new System.Drawing.Size(83, 91);
            this.btn5.TabIndex = 16;
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
            this.btn6.Location = new System.Drawing.Point(180, 100);
            this.btn6.Name = "btn6";
            this.btn6.Padding = new System.Windows.Forms.Padding(7, 0, 0, 0);
            this.btn6.Size = new System.Drawing.Size(83, 91);
            this.btn6.TabIndex = 17;
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
            this.btn1.Location = new System.Drawing.Point(2, 197);
            this.btn1.Name = "btn1";
            this.btn1.Padding = new System.Windows.Forms.Padding(7, 0, 0, 0);
            this.btn1.Size = new System.Drawing.Size(83, 91);
            this.btn1.TabIndex = 18;
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
            this.btn2.Location = new System.Drawing.Point(91, 197);
            this.btn2.Name = "btn2";
            this.btn2.Padding = new System.Windows.Forms.Padding(7, 0, 0, 0);
            this.btn2.Size = new System.Drawing.Size(83, 91);
            this.btn2.TabIndex = 19;
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
            this.btn3.Location = new System.Drawing.Point(180, 197);
            this.btn3.Name = "btn3";
            this.btn3.Padding = new System.Windows.Forms.Padding(7, 0, 0, 0);
            this.btn3.Size = new System.Drawing.Size(83, 91);
            this.btn3.TabIndex = 20;
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
            this.btn0.Location = new System.Drawing.Point(2, 294);
            this.btn0.Name = "btn0";
            this.btn0.Padding = new System.Windows.Forms.Padding(7, 0, 0, 0);
            this.btn0.Size = new System.Drawing.Size(83, 91);
            this.btn0.TabIndex = 21;
            this.btn0.Tag = "0";
            this.btn0.Text = "0";
            this.btn0.UseVisualStyleBackColor = false;
            this.btn0.Click += new System.EventHandler(this.btn0_Click);
            // 
            // btnBS
            // 
            this.btnBS.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.btnBS.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnBS.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBS.ForeColor = System.Drawing.Color.MidnightBlue;
            this.btnBS.Location = new System.Drawing.Point(91, 294);
            this.btnBS.Name = "btnBS";
            this.btnBS.Padding = new System.Windows.Forms.Padding(7, 0, 0, 0);
            this.btnBS.Size = new System.Drawing.Size(83, 91);
            this.btnBS.TabIndex = 22;
            this.btnBS.Tag = "Back Space";
            this.btnBS.Text = "Back Space";
            this.btnBS.UseVisualStyleBackColor = false;
            this.btnBS.Click += new System.EventHandler(this.btnBS_Click);
            // 
            // btnEnter
            // 
            this.btnEnter.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.btnEnter.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnEnter.Font = new System.Drawing.Font("Tahoma", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEnter.ForeColor = System.Drawing.Color.MidnightBlue;
            this.btnEnter.Location = new System.Drawing.Point(180, 294);
            this.btnEnter.Name = "btnEnter";
            this.btnEnter.Padding = new System.Windows.Forms.Padding(7, 0, 0, 0);
            this.btnEnter.Size = new System.Drawing.Size(83, 91);
            this.btnEnter.TabIndex = 23;
            this.btnEnter.Tag = "OK";
            this.btnEnter.Text = "OK";
            this.btnEnter.UseVisualStyleBackColor = false;
            this.btnEnter.Click += new System.EventHandler(this.btnEnter_Click);
            // 
            // frmposlogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(267, 465);
            this.Controls.Add(this.panelkey);
            this.Controls.Add(this.panelpin);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmposlogin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "LogIn";
            this.panelpin.ResumeLayout(false);
            this.panelpin.PerformLayout();
            this.panelkey.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtpin;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panelpin;
        private System.Windows.Forms.Panel panelkey;
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
        private System.Windows.Forms.Button btnBS;
        private System.Windows.Forms.Button btnEnter;
    }
}