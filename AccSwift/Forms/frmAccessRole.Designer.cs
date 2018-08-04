using SComponents;
namespace AccSwift
{
    partial class frmAccessRole
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
            this.btnNew = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lstAccessRoles = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lblAccessRoleTitle = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cboCopyAccessRole = new SComboBox();
            this.txtRoleName = new STextBox();
            this.txtAccessRoleID = new STextBox();
            this.treeAccessRole = new STreeView(this.components);
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnNew
            // 
            this.btnNew.Image = global::Inventory.Properties.Resources.edit_add;
            this.btnNew.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNew.Location = new System.Drawing.Point(47, 12);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(75, 29);
            this.btnNew.TabIndex = 0;
            this.btnNew.Text = "&New";
            this.btnNew.UseVisualStyleBackColor = true;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Image = global::Inventory.Properties.Resources.document_edit;
            this.btnEdit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEdit.Location = new System.Drawing.Point(126, 12);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(75, 29);
            this.btnEdit.TabIndex = 1;
            this.btnEdit.Text = "&Edit";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnSave
            // 
            this.btnSave.Image = global::Inventory.Properties.Resources.save;
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSave.Location = new System.Drawing.Point(207, 12);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 29);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "&Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Image = global::Inventory.Properties.Resources.document_delete;
            this.btnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDelete.Location = new System.Drawing.Point(287, 12);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 29);
            this.btnDelete.TabIndex = 3;
            this.btnDelete.Text = "&Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Image = global::Inventory.Properties.Resources.gnome_window_close;
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(368, 12);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 29);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 13);
            this.label1.TabIndex = 32;
            this.label1.Text = "Role Name:";
            // 
            // lstAccessRoles
            // 
            this.lstAccessRoles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lstAccessRoles.FormattingEnabled = true;
            this.lstAccessRoles.Location = new System.Drawing.Point(7, 82);
            this.lstAccessRoles.Name = "lstAccessRoles";
            this.lstAccessRoles.Size = new System.Drawing.Size(167, 303);
            this.lstAccessRoles.TabIndex = 0;
            this.lstAccessRoles.SelectedIndexChanged += new System.EventHandler(this.lstAccessRoles_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(114, 13);
            this.label2.TabIndex = 35;
            this.label2.Text = "Existing Access Roles:";
            // 
            // lblAccessRoleTitle
            // 
            this.lblAccessRoleTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblAccessRoleTitle.BackColor = System.Drawing.Color.FloralWhite;
            this.lblAccessRoleTitle.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAccessRoleTitle.Location = new System.Drawing.Point(-1, 0);
            this.lblAccessRoleTitle.Name = "lblAccessRoleTitle";
            this.lblAccessRoleTitle.Size = new System.Drawing.Size(469, 48);
            this.lblAccessRoleTitle.TabIndex = 37;
            this.lblAccessRoleTitle.Text = "The following access role defines the user with the specific previlige. The user " +
                "is later on assigned the created access roll. Create your own access role and pr" +
                "ovide the access role to the user.";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.btnCancel);
            this.groupBox1.Controls.Add(this.btnDelete);
            this.groupBox1.Controls.Add(this.btnSave);
            this.groupBox1.Controls.Add(this.btnEdit);
            this.groupBox1.Controls.Add(this.btnNew);
            this.groupBox1.Location = new System.Drawing.Point(7, 387);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(451, 47);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(148, 11);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(120, 13);
            this.label3.TabIndex = 40;
            this.label3.Text = "Copy Access Role from:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.cboCopyAccessRole);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.txtRoleName);
            this.groupBox2.Location = new System.Drawing.Point(180, 50);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(278, 54);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            // 
            // cboCopyAccessRole
            // 
            this.cboCopyAccessRole.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCopyAccessRole.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.cboCopyAccessRole.FocusLostColor = System.Drawing.Color.White;
            this.cboCopyAccessRole.FormattingEnabled = true;
            this.cboCopyAccessRole.Location = new System.Drawing.Point(151, 27);
            this.cboCopyAccessRole.Name = "cboCopyAccessRole";
            this.cboCopyAccessRole.Size = new System.Drawing.Size(121, 21);
            this.cboCopyAccessRole.TabIndex = 1;
            this.cboCopyAccessRole.SelectedIndexChanged += new System.EventHandler(this.cboCopyAccessRole_SelectedIndexChanged);
            // 
            // txtRoleName
            // 
            this.txtRoleName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtRoleName.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtRoleName.FocusLostColor = System.Drawing.Color.White;
            this.txtRoleName.Location = new System.Drawing.Point(7, 28);
            this.txtRoleName.Name = "txtRoleName";
            this.txtRoleName.Size = new System.Drawing.Size(138, 20);
            this.txtRoleName.TabIndex = 0;
            // 
            // txtAccessRoleID
            // 
            this.txtAccessRoleID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtAccessRoleID.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtAccessRoleID.FocusLostColor = System.Drawing.Color.White;
            this.txtAccessRoleID.Location = new System.Drawing.Point(12, 41);
            this.txtAccessRoleID.Name = "txtAccessRoleID";
            this.txtAccessRoleID.Size = new System.Drawing.Size(20, 20);
            this.txtAccessRoleID.TabIndex = 36;
            this.txtAccessRoleID.Visible = false;
            // 
            // treeAccessRole
            // 
            this.treeAccessRole.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.treeAccessRole.AutoCheckChild = true;
            this.treeAccessRole.CheckBoxes = true;
            this.treeAccessRole.Location = new System.Drawing.Point(180, 110);
            this.treeAccessRole.Name = "treeAccessRole";
            this.treeAccessRole.Size = new System.Drawing.Size(278, 275);
            this.treeAccessRole.TabIndex = 2;
            // 
            // frmAccessRole
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(464, 439);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lblAccessRoleTitle);
            this.Controls.Add(this.txtAccessRoleID);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.treeAccessRole);
            this.Controls.Add(this.lstAccessRoles);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "frmAccessRole";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Access Role";
            this.Load += new System.EventHandler(this.frmAccessRole_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmAccessRole_KeyDown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnCancel;
        private STextBox txtRoleName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox lstAccessRoles;
        private STreeView treeAccessRole;
        private System.Windows.Forms.Label label2;
        private STextBox txtAccessRoleID;
        private System.Windows.Forms.Label lblAccessRoleTitle;
        private System.Windows.Forms.GroupBox groupBox1;
        private SComboBox cboCopyAccessRole;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox2;

    }
}