namespace AccSwift
{
    partial class frmDBConnect
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        ////protected override void Dispose(bool disposing)
        ////{
        ////    if (disposing && (components != null))
        ////    {
        ////        components.Dispose();
        ////    }
        ////    base.Dispose(disposing);
        ////}

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.cbDataBase = new System.Windows.Forms.ComboBox();
            this.txtUser = new System.Windows.Forms.TextBox();
            this.rbAuthenticationWin = new System.Windows.Forms.RadioButton();
            this.lbUsuario = new System.Windows.Forms.Label();
            this.lbClave = new System.Windows.Forms.Label();
            this.rbAuthenticationSql = new System.Windows.Forms.RadioButton();
            this.GroupBox1 = new System.Windows.Forms.GroupBox();
            this.lbBase = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.cbServer = new System.Windows.Forms.ComboBox();
            this.lbServidor = new System.Windows.Forms.Label();
            this.GroupBox2 = new System.Windows.Forms.GroupBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.GroupBox1.SuspendLayout();
            this.GroupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(93, 97);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(212, 20);
            this.txtPassword.TabIndex = 3;
            this.txtPassword.TextChanged += new System.EventHandler(this.txtPassword_TextChanged);
            // 
            // cbDataBase
            // 
            this.cbDataBase.FormattingEnabled = true;
            this.cbDataBase.Location = new System.Drawing.Point(9, 38);
            this.cbDataBase.Name = "cbDataBase";
            this.cbDataBase.Size = new System.Drawing.Size(296, 21);
            this.cbDataBase.TabIndex = 0;
            this.cbDataBase.Text = "master";
            this.cbDataBase.SelectedIndexChanged += new System.EventHandler(this.cbDataBase_SelectedIndexChanged);
            // 
            // txtUser
            // 
            this.txtUser.Location = new System.Drawing.Point(93, 71);
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size(212, 20);
            this.txtUser.TabIndex = 2;
            this.txtUser.TextChanged += new System.EventHandler(this.txtUser_TextChanged);
            // 
            // rbAuthenticationWin
            // 
            this.rbAuthenticationWin.AutoSize = true;
            this.rbAuthenticationWin.Enabled = false;
            this.rbAuthenticationWin.Location = new System.Drawing.Point(25, 19);
            this.rbAuthenticationWin.Name = "rbAuthenticationWin";
            this.rbAuthenticationWin.Size = new System.Drawing.Size(162, 17);
            this.rbAuthenticationWin.TabIndex = 0;
            this.rbAuthenticationWin.Text = "Use Windows Authentication";
            this.rbAuthenticationWin.UseVisualStyleBackColor = true;
            this.rbAuthenticationWin.CheckedChanged += new System.EventHandler(this.rbAuthenticationWin_CheckedChanged);
            // 
            // lbUsuario
            // 
            this.lbUsuario.AutoSize = true;
            this.lbUsuario.Location = new System.Drawing.Point(34, 74);
            this.lbUsuario.Name = "lbUsuario";
            this.lbUsuario.Size = new System.Drawing.Size(29, 13);
            this.lbUsuario.TabIndex = 9;
            this.lbUsuario.Text = "User";
            // 
            // lbClave
            // 
            this.lbClave.AutoSize = true;
            this.lbClave.Location = new System.Drawing.Point(34, 100);
            this.lbClave.Name = "lbClave";
            this.lbClave.Size = new System.Drawing.Size(53, 13);
            this.lbClave.TabIndex = 10;
            this.lbClave.Text = "Password";
            // 
            // rbAuthenticationSql
            // 
            this.rbAuthenticationSql.AutoSize = true;
            this.rbAuthenticationSql.Checked = true;
            this.rbAuthenticationSql.Location = new System.Drawing.Point(25, 41);
            this.rbAuthenticationSql.Name = "rbAuthenticationSql";
            this.rbAuthenticationSql.Size = new System.Drawing.Size(173, 17);
            this.rbAuthenticationSql.TabIndex = 1;
            this.rbAuthenticationSql.TabStop = true;
            this.rbAuthenticationSql.Text = "Use SQL Server Authentication";
            this.rbAuthenticationSql.CheckedChanged += new System.EventHandler(this.rbAuthenticationSql_CheckedChanged);
            // 
            // GroupBox1
            // 
            this.GroupBox1.Controls.Add(this.lbBase);
            this.GroupBox1.Controls.Add(this.cbDataBase);
            this.GroupBox1.Location = new System.Drawing.Point(8, 258);
            this.GroupBox1.Name = "GroupBox1";
            this.GroupBox1.Size = new System.Drawing.Size(328, 76);
            this.GroupBox1.TabIndex = 3;
            this.GroupBox1.TabStop = false;
            this.GroupBox1.Text = "Connect to database";
            this.GroupBox1.Visible = false;
            // 
            // lbBase
            // 
            this.lbBase.AutoSize = true;
            this.lbBase.Location = new System.Drawing.Point(6, 22);
            this.lbBase.Name = "lbBase";
            this.lbBase.Size = new System.Drawing.Size(161, 13);
            this.lbBase.TabIndex = 36;
            this.lbBase.Text = "Select or enter a database name";
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(164, 229);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(82, 23);
            this.btnOK.TabIndex = 4;
            this.btnOK.Tag = "";
            this.btnOK.Text = "OK";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(252, 229);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(82, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(254, 63);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(82, 23);
            this.btnRefresh.TabIndex = 1;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // cbServer
            // 
            this.cbServer.FormattingEnabled = true;
            this.cbServer.Location = new System.Drawing.Point(8, 65);
            this.cbServer.Name = "cbServer";
            this.cbServer.Size = new System.Drawing.Size(240, 21);
            this.cbServer.TabIndex = 0;
            this.cbServer.SelectedIndexChanged += new System.EventHandler(this.cbServer_SelectedIndexChanged);
            // 
            // lbServidor
            // 
            this.lbServidor.AutoSize = true;
            this.lbServidor.Location = new System.Drawing.Point(5, 49);
            this.lbServidor.Name = "lbServidor";
            this.lbServidor.Size = new System.Drawing.Size(69, 13);
            this.lbServidor.TabIndex = 35;
            this.lbServidor.Text = "Server Name";
            // 
            // GroupBox2
            // 
            this.GroupBox2.Controls.Add(this.txtPassword);
            this.GroupBox2.Controls.Add(this.txtUser);
            this.GroupBox2.Controls.Add(this.rbAuthenticationWin);
            this.GroupBox2.Controls.Add(this.lbUsuario);
            this.GroupBox2.Controls.Add(this.lbClave);
            this.GroupBox2.Controls.Add(this.rbAuthenticationSql);
            this.GroupBox2.Location = new System.Drawing.Point(8, 92);
            this.GroupBox2.Name = "GroupBox2";
            this.GroupBox2.Size = new System.Drawing.Size(328, 131);
            this.GroupBox2.TabIndex = 2;
            this.GroupBox2.TabStop = false;
            this.GroupBox2.Text = "Log on to the server";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Inventory.Properties.Resources.dbsettings1;
            this.pictureBox1.Location = new System.Drawing.Point(-2, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(347, 46);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 43;
            this.pictureBox1.TabStop = false;
            // 
            // frmDBConnect
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(344, 257);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.GroupBox1);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.cbServer);
            this.Controls.Add(this.lbServidor);
            this.Controls.Add(this.GroupBox2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "frmDBConnect";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Database Connection";
            this.Load += new System.EventHandler(this.frmDBConnect_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmDBConnect_KeyDown);
            this.GroupBox1.ResumeLayout(false);
            this.GroupBox1.PerformLayout();
            this.GroupBox2.ResumeLayout(false);
            this.GroupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.TextBox txtPassword;
        internal System.Windows.Forms.ComboBox cbDataBase;
        internal System.Windows.Forms.TextBox txtUser;
        internal System.Windows.Forms.RadioButton rbAuthenticationWin;
        internal System.Windows.Forms.Label lbUsuario;
        internal System.Windows.Forms.Label lbClave;
        internal System.Windows.Forms.RadioButton rbAuthenticationSql;
        internal System.Windows.Forms.GroupBox GroupBox1;
        internal System.Windows.Forms.Label lbBase;
        internal System.Windows.Forms.Button btnOK;
        internal System.Windows.Forms.Button btnCancel;
        internal System.Windows.Forms.Button btnRefresh;
        internal System.Windows.Forms.ComboBox cbServer;
        internal System.Windows.Forms.Label lbServidor;
        internal System.Windows.Forms.GroupBox GroupBox2;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}