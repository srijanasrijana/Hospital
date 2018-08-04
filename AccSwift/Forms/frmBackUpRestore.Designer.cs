using SComponents;
namespace AccSwift
{
    partial class frmBackUpRestore
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmBackUpRestore));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.lblFilePath = new System.Windows.Forms.Label();
            this.cboDBName = new SComboBox();
            this.txtFile = new STextBox();
            this.btnFile = new System.Windows.Forms.Button();
            this.rdRestore = new System.Windows.Forms.RadioButton();
            this.rdBackup = new System.Windows.Forms.RadioButton();
            this.btnBackUpRestore = new System.Windows.Forms.Button();
            this.openFD = new System.Windows.Forms.OpenFileDialog();
            this.saveFD = new System.Windows.Forms.SaveFileDialog();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.lblFilePath);
            this.groupBox1.Controls.Add(this.cboDBName);
            this.groupBox1.Controls.Add(this.txtFile);
            this.groupBox1.Controls.Add(this.btnFile);
            this.groupBox1.Controls.Add(this.rdRestore);
            this.groupBox1.Controls.Add(this.rdBackup);
            this.groupBox1.Location = new System.Drawing.Point(6, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(259, 141);
            this.groupBox1.TabIndex = 28;
            this.groupBox1.TabStop = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 95);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(63, 13);
            this.label5.TabIndex = 39;
            this.label5.Text = "Restore To:";
            // 
            // lblFilePath
            // 
            this.lblFilePath.AutoSize = true;
            this.lblFilePath.Location = new System.Drawing.Point(6, 51);
            this.lblFilePath.Name = "lblFilePath";
            this.lblFilePath.Size = new System.Drawing.Size(63, 13);
            this.lblFilePath.TabIndex = 38;
            this.lblFilePath.Text = "Backup To:";
            // 
            // cboDBName
            // 
            this.cboDBName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDBName.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.cboDBName.FocusLostColor = System.Drawing.Color.White;
            this.cboDBName.FormattingEnabled = true;
            this.cboDBName.Location = new System.Drawing.Point(6, 111);
            this.cboDBName.Name = "cboDBName";
            this.cboDBName.Size = new System.Drawing.Size(197, 21);
            this.cboDBName.TabIndex = 37;
            // 
            // txtFile
            // 
            this.txtFile.BackColor = System.Drawing.Color.White;
            this.txtFile.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtFile.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtFile.FocusLostColor = System.Drawing.Color.White;
            this.txtFile.Location = new System.Drawing.Point(6, 67);
            this.txtFile.Name = "txtFile";
            this.txtFile.Size = new System.Drawing.Size(197, 20);
            this.txtFile.TabIndex = 33;
            // 
            // btnFile
            // 
            this.btnFile.Location = new System.Drawing.Point(209, 65);
            this.btnFile.Name = "btnFile";
            this.btnFile.Size = new System.Drawing.Size(32, 22);
            this.btnFile.TabIndex = 30;
            this.btnFile.Text = "...";
            this.btnFile.Click += new System.EventHandler(this.btnFile_Click);
            // 
            // rdRestore
            // 
            this.rdRestore.Location = new System.Drawing.Point(145, 19);
            this.rdRestore.Name = "rdRestore";
            this.rdRestore.Size = new System.Drawing.Size(96, 16);
            this.rdRestore.TabIndex = 28;
            this.rdRestore.Text = "Restore";
            this.rdRestore.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            // 
            // rdBackup
            // 
            this.rdBackup.Checked = true;
            this.rdBackup.Location = new System.Drawing.Point(6, 19);
            this.rdBackup.Name = "rdBackup";
            this.rdBackup.Size = new System.Drawing.Size(96, 16);
            this.rdBackup.TabIndex = 27;
            this.rdBackup.TabStop = true;
            this.rdBackup.Text = "Back-Up";
            this.rdBackup.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // btnBackUpRestore
            // 
            this.btnBackUpRestore.Location = new System.Drawing.Point(96, 148);
            this.btnBackUpRestore.Name = "btnBackUpRestore";
            this.btnBackUpRestore.Size = new System.Drawing.Size(88, 24);
            this.btnBackUpRestore.TabIndex = 29;
            this.btnBackUpRestore.Text = "Back-Up";
            this.btnBackUpRestore.Click += new System.EventHandler(this.btnBackUpRestore_Click);
            // 
            // saveFD
            // 
            this.saveFD.FileName = "doc1";
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(190, 149);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 35;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // frmBackUpRestore
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(273, 178);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnBackUpRestore);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "frmBackUpRestore";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Backup/Restore";
            this.Load += new System.EventHandler(this.frmBackUpRestore_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmBackUpRestore_KeyDown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnFile;
        private System.Windows.Forms.Button btnBackUpRestore;
        private System.Windows.Forms.RadioButton rdRestore;
        private System.Windows.Forms.RadioButton rdBackup;
        private System.Windows.Forms.OpenFileDialog openFD;
        private System.Windows.Forms.SaveFileDialog saveFD;
        private SComboBox cboServer;
        private STextBox txtFile;
        private SComboBox cboDatabase;
        private SComboBox cboDBName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblFilePath;
        private System.Windows.Forms.Button btnCancel;
    }
}