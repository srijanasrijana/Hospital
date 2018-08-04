using SComponents;
namespace AccSwift.Forms
{
    partial class frmuserenabledisable
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmuserenabledisable));
            this.lvUser = new System.Windows.Forms.ListView();
            this.btnenable = new System.Windows.Forms.Button();
            this.btndisable = new System.Windows.Forms.Button();
            this.txtUserID = new STextBox();
            this.SuspendLayout();
            // 
            // lvUser
            // 
            this.lvUser.Location = new System.Drawing.Point(4, 2);
            this.lvUser.Name = "lvUser";
            this.lvUser.Size = new System.Drawing.Size(456, 197);
            this.lvUser.TabIndex = 35;
            this.lvUser.UseCompatibleStateImageBehavior = false;
            this.lvUser.View = System.Windows.Forms.View.Details;
            this.lvUser.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.lvUser_ItemSelectionChanged);
            // 
            // btnenable
            // 
            this.btnenable.Location = new System.Drawing.Point(12, 205);
            this.btnenable.Name = "btnenable";
            this.btnenable.Size = new System.Drawing.Size(75, 23);
            this.btnenable.TabIndex = 36;
            this.btnenable.Text = "Enable";
            this.btnenable.UseVisualStyleBackColor = true;
            this.btnenable.Click += new System.EventHandler(this.btnenable_Click);
            // 
            // btndisable
            // 
            this.btndisable.Location = new System.Drawing.Point(109, 205);
            this.btndisable.Name = "btndisable";
            this.btndisable.Size = new System.Drawing.Size(75, 23);
            this.btndisable.TabIndex = 37;
            this.btndisable.Text = "Disable";
            this.btndisable.UseVisualStyleBackColor = true;
            this.btndisable.Click += new System.EventHandler(this.btndisable_Click);
            // 
            // txtUserID
            // 
            this.txtUserID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtUserID.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.txtUserID.FocusLostColor = System.Drawing.Color.White;
            this.txtUserID.Location = new System.Drawing.Point(390, 213);
            this.txtUserID.Name = "txtUserID";
            this.txtUserID.Size = new System.Drawing.Size(58, 20);
            this.txtUserID.TabIndex = 38;
            this.txtUserID.Visible = false;
            // 
            // frmuserenabledisable
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(460, 245);
            this.Controls.Add(this.txtUserID);
            this.Controls.Add(this.btndisable);
            this.Controls.Add(this.btnenable);
            this.Controls.Add(this.lvUser);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmuserenabledisable";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "User Enable/Disable";
            this.Load += new System.EventHandler(this.frmuserenabledisable_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView lvUser;
        private System.Windows.Forms.Button btnenable;
        private System.Windows.Forms.Button btndisable;
        private STextBox txtUserID;
    }
}