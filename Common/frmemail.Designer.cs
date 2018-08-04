namespace Common
{
    partial class frmemail
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmemail));
            this.txtcontents = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtto = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtsubject = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnsave = new System.Windows.Forms.Button();
            this.btnclear = new System.Windows.Forms.Button();
            this.lstfiles = new System.Windows.Forms.ListBox();
            this.btnshowfile = new System.Windows.Forms.Button();
            this.txtAttachment = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtcontents
            // 
            this.txtcontents.AcceptsReturn = true;
            this.txtcontents.AcceptsTab = true;
            this.txtcontents.Location = new System.Drawing.Point(92, 165);
            this.txtcontents.Multiline = true;
            this.txtcontents.Name = "txtcontents";
            this.txtcontents.Size = new System.Drawing.Size(330, 140);
            this.txtcontents.TabIndex = 3;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(17, 168);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(52, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "Contents:";
            // 
            // txtto
            // 
            this.txtto.Location = new System.Drawing.Point(105, 16);
            this.txtto.Name = "txtto";
            this.txtto.Size = new System.Drawing.Size(318, 20);
            this.txtto.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(17, 19);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(51, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "Send To:";
            // 
            // txtsubject
            // 
            this.txtsubject.Location = new System.Drawing.Point(105, 53);
            this.txtsubject.Name = "txtsubject";
            this.txtsubject.Size = new System.Drawing.Size(318, 20);
            this.txtsubject.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(17, 53);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Subject:";
            // 
            // btnsave
            // 
            this.btnsave.Location = new System.Drawing.Point(12, 314);
            this.btnsave.Name = "btnsave";
            this.btnsave.Size = new System.Drawing.Size(75, 23);
            this.btnsave.TabIndex = 4;
            this.btnsave.Text = "Send";
            this.btnsave.UseVisualStyleBackColor = true;
            this.btnsave.Click += new System.EventHandler(this.btnsave_Click);
            // 
            // btnclear
            // 
            this.btnclear.Location = new System.Drawing.Point(106, 314);
            this.btnclear.Name = "btnclear";
            this.btnclear.Size = new System.Drawing.Size(75, 23);
            this.btnclear.TabIndex = 5;
            this.btnclear.Text = "Clear";
            this.btnclear.UseVisualStyleBackColor = true;
            this.btnclear.Click += new System.EventHandler(this.btnclear_Click);
            // 
            // lstfiles
            // 
            this.lstfiles.Enabled = false;
            this.lstfiles.Location = new System.Drawing.Point(105, 121);
            this.lstfiles.Name = "lstfiles";
            this.lstfiles.Size = new System.Drawing.Size(316, 30);
            this.lstfiles.TabIndex = 21;
            // 
            // btnshowfile
            // 
            this.btnshowfile.Location = new System.Drawing.Point(430, 80);
            this.btnshowfile.Name = "btnshowfile";
            this.btnshowfile.Size = new System.Drawing.Size(24, 24);
            this.btnshowfile.TabIndex = 2;
            this.btnshowfile.Text = "...";
            this.btnshowfile.Click += new System.EventHandler(this.btnshowfile_Click);
            // 
            // txtAttachment
            // 
            this.txtAttachment.Enabled = false;
            this.txtAttachment.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAttachment.Location = new System.Drawing.Point(105, 81);
            this.txtAttachment.Name = "txtAttachment";
            this.txtAttachment.Size = new System.Drawing.Size(317, 23);
            this.txtAttachment.TabIndex = 19;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 81);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 13);
            this.label1.TabIndex = 22;
            this.label1.Text = "Attachment:";
            // 
            // frmemail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(486, 362);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lstfiles);
            this.Controls.Add(this.btnshowfile);
            this.Controls.Add(this.txtAttachment);
            this.Controls.Add(this.btnclear);
            this.Controls.Add(this.btnsave);
            this.Controls.Add(this.txtcontents);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtto);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtsubject);
            this.Controls.Add(this.label3);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmemail";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Email";
            this.Load += new System.EventHandler(this.frmemail_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtcontents;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtto;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtsubject;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnsave;
        private System.Windows.Forms.Button btnclear;
        private System.Windows.Forms.ListBox lstfiles;
        private System.Windows.Forms.Button btnshowfile;
        private System.Windows.Forms.TextBox txtAttachment;
        private System.Windows.Forms.Label label1;
    }
}