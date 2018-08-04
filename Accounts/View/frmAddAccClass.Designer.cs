using SComponents;
namespace Accounts
{
    partial class frmAddAccClass
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
            this.lbLeftAccClass = new System.Windows.Forms.ListBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.lbRightAccClass = new System.Windows.Forms.ListBox();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbLeftAccClass
            // 
            this.lbLeftAccClass.FormattingEnabled = true;
            this.lbLeftAccClass.Location = new System.Drawing.Point(29, 35);
            this.lbLeftAccClass.Name = "lbLeftAccClass";
            this.lbLeftAccClass.Size = new System.Drawing.Size(196, 316);
            this.lbLeftAccClass.TabIndex = 0;
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(235, 69);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(51, 23);
            this.btnAdd.TabIndex = 1;
            this.btnAdd.Text = "=>";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // lbRightAccClass
            // 
            this.lbRightAccClass.FormattingEnabled = true;
            this.lbRightAccClass.Location = new System.Drawing.Point(300, 35);
            this.lbRightAccClass.Name = "lbRightAccClass";
            this.lbRightAccClass.Size = new System.Drawing.Size(184, 316);
            this.lbRightAccClass.TabIndex = 2;
            // 
            // btnRemove
            // 
            this.btnRemove.Location = new System.Drawing.Point(234, 261);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(54, 23);
            this.btnRemove.TabIndex = 3;
            this.btnRemove.Text = "<=";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnSave
            // 
            this.btnSave.Image = global::Accounts.Properties.Resources.save;
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSave.Location = new System.Drawing.Point(300, 365);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 16;
            this.btnSave.Text = "&Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Image = global::Accounts.Properties.Resources.gnome_window_close;
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(409, 365);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 19;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // frmAddAccClass
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(517, 400);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.lbRightAccClass);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.lbLeftAccClass);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "frmAddAccClass";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Add Account Class";
            this.Load += new System.EventHandler(this.frmAddAccClass_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox lbLeftAccClass;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.ListBox lbRightAccClass;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;

    }
}