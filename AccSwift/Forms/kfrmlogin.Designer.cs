namespace AccSwift.Forms
{
    partial class kfrmlogin
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
            this.components = new System.ComponentModel.Container();
            this.kryptonManager = new ComponentFactory.Krypton.Toolkit.KryptonManager(this.components);
            this.kryptonLabel1 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.ktxtusername = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.kryptonLabel2 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.ktxtpassword = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.kbtnlogin = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.kbtncancel = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // kryptonLabel1
            // 
            this.kryptonLabel1.LabelStyle = ComponentFactory.Krypton.Toolkit.LabelStyle.SuperTip;
            this.kryptonLabel1.Location = new System.Drawing.Point(18, 108);
            this.kryptonLabel1.Name = "kryptonLabel1";
            this.kryptonLabel1.Size = new System.Drawing.Size(73, 26);
            this.kryptonLabel1.TabIndex = 1;
            this.kryptonLabel1.Values.Text = "UserName";
            this.kryptonLabel1.Paint += new System.Windows.Forms.PaintEventHandler(this.kryptonLabel1_Paint);
            // 
            // ktxtusername
            // 
            this.ktxtusername.Location = new System.Drawing.Point(127, 114);
            this.ktxtusername.Name = "ktxtusername";
            this.ktxtusername.Size = new System.Drawing.Size(169, 20);
            this.ktxtusername.TabIndex = 2;
            // 
            // kryptonLabel2
            // 
            this.kryptonLabel2.LabelStyle = ComponentFactory.Krypton.Toolkit.LabelStyle.SuperTip;
            this.kryptonLabel2.Location = new System.Drawing.Point(23, 151);
            this.kryptonLabel2.Name = "kryptonLabel2";
            this.kryptonLabel2.Size = new System.Drawing.Size(68, 26);
            this.kryptonLabel2.TabIndex = 3;
            this.kryptonLabel2.Values.Text = "Password";
            // 
            // ktxtpassword
            // 
            this.ktxtpassword.Location = new System.Drawing.Point(127, 151);
            this.ktxtpassword.Name = "ktxtpassword";
            this.ktxtpassword.Size = new System.Drawing.Size(169, 20);
            this.ktxtpassword.TabIndex = 4;
            // 
            // kbtnlogin
            // 
            this.kbtnlogin.ButtonStyle = ComponentFactory.Krypton.Toolkit.ButtonStyle.NavigatorOverflow;
            this.kbtnlogin.Location = new System.Drawing.Point(3, 234);
            this.kbtnlogin.Name = "kbtnlogin";
            this.kbtnlogin.Size = new System.Drawing.Size(90, 25);
            this.kbtnlogin.TabIndex = 5;
            this.kbtnlogin.Values.Text = "Login";
            // 
            // kbtncancel
            // 
            this.kbtncancel.ButtonStyle = ComponentFactory.Krypton.Toolkit.ButtonStyle.NavigatorOverflow;
            this.kbtncancel.Location = new System.Drawing.Point(104, 234);
            this.kbtncancel.Name = "kbtncancel";
            this.kbtncancel.Size = new System.Drawing.Size(90, 25);
            this.kbtncancel.TabIndex = 6;
            this.kbtncancel.Values.Text = "Cancel";
            this.kbtncancel.Click += new System.EventHandler(this.kbtncancel_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Location = new System.Drawing.Point(-1, 214);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(337, 8);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::AccSwift.Properties.Resources.LogInImage;
            this.pictureBox1.Location = new System.Drawing.Point(-1, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(329, 82);
            this.pictureBox1.TabIndex = 7;
            this.pictureBox1.TabStop = false;
            // 
            // kfrmlogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ClientSize = new System.Drawing.Size(328, 266);
            this.ControlBox = false;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.kbtncancel);
            this.Controls.Add(this.kbtnlogin);
            this.Controls.Add(this.ktxtpassword);
            this.Controls.Add(this.kryptonLabel2);
            this.Controls.Add(this.ktxtusername);
            this.Controls.Add(this.kryptonLabel1);
            this.GroupBackStyle = ComponentFactory.Krypton.Toolkit.PaletteBackStyle.TabOneNote;
            this.GroupBorderStyle = ComponentFactory.Krypton.Toolkit.PaletteBorderStyle.ButtonStandalone;
            this.HeaderStyle = ComponentFactory.Krypton.Toolkit.HeaderStyle.Primary;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "kfrmlogin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Login";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ComponentFactory.Krypton.Toolkit.KryptonManager kryptonManager;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel1;
        private ComponentFactory.Krypton.Toolkit.KryptonTextBox ktxtusername;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel2;
        private ComponentFactory.Krypton.Toolkit.KryptonTextBox ktxtpassword;
        private ComponentFactory.Krypton.Toolkit.KryptonButton kbtnlogin;
        private ComponentFactory.Krypton.Toolkit.KryptonButton kbtncancel;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}

