namespace AccSwift.Forms
{
    partial class frmbulksms
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmbulksms));
            this.btnsend = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtto = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtmessage = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnsend
            // 
            this.btnsend.Location = new System.Drawing.Point(313, 159);
            this.btnsend.Name = "btnsend";
            this.btnsend.Size = new System.Drawing.Size(75, 33);
            this.btnsend.TabIndex = 0;
            this.btnsend.Text = "Send";
            this.btnsend.UseVisualStyleBackColor = true;
            this.btnsend.Click += new System.EventHandler(this.btnsend_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(20, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "To";
            // 
            // txtto
            // 
            this.txtto.Location = new System.Drawing.Point(87, 13);
            this.txtto.Name = "txtto";
            this.txtto.Size = new System.Drawing.Size(304, 20);
            this.txtto.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Message";
            // 
            // txtmessage
            // 
            this.txtmessage.Location = new System.Drawing.Point(87, 46);
            this.txtmessage.Multiline = true;
            this.txtmessage.Name = "txtmessage";
            this.txtmessage.Size = new System.Drawing.Size(304, 105);
            this.txtmessage.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(10, 207);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(299, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Note: Enter Phone Number in the Format like +9779841xxxxxx";
            // 
            // frmbulksms
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(400, 226);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtmessage);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtto);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnsend);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmbulksms";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Bulk SMS";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnsend;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtto;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtmessage;
        private System.Windows.Forms.Label label3;
    }
}