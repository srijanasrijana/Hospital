using SComponents;
namespace Accounts
{
    partial class frmAccountClass
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAccountClass));
            this.pnlLeft = new System.Windows.Forms.Panel();
            this.treeAccountClass = new System.Windows.Forms.TreeView();
            this.pnlRight = new System.Windows.Forms.Panel();
            this.txtID = new SComponents.STextBox();
            this.txtRemarks = new SComponents.STextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnNew = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.cboParentClass = new SComponents.SComboBox();
            this.txtClassName = new SComponents.STextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.collapsibleSplitter1 = new NJFLib.Controls.CollapsibleSplitter();
            this.pnlLeft.SuspendLayout();
            this.pnlRight.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlLeft
            // 
            this.pnlLeft.Controls.Add(this.treeAccountClass);
            this.pnlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlLeft.Location = new System.Drawing.Point(0, 0);
            this.pnlLeft.Name = "pnlLeft";
            this.pnlLeft.Size = new System.Drawing.Size(120, 262);
            this.pnlLeft.TabIndex = 3;
            // 
            // treeAccountClass
            // 
            this.treeAccountClass.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeAccountClass.Location = new System.Drawing.Point(0, 0);
            this.treeAccountClass.Name = "treeAccountClass";
            this.treeAccountClass.Size = new System.Drawing.Size(120, 262);
            this.treeAccountClass.TabIndex = 0;
            this.treeAccountClass.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeAccountClass_AfterSelect);
            // 
            // pnlRight
            // 
            this.pnlRight.Controls.Add(this.txtID);
            this.pnlRight.Controls.Add(this.txtRemarks);
            this.pnlRight.Controls.Add(this.label3);
            this.pnlRight.Controls.Add(this.btnCancel);
            this.pnlRight.Controls.Add(this.btnEdit);
            this.pnlRight.Controls.Add(this.btnDelete);
            this.pnlRight.Controls.Add(this.btnSave);
            this.pnlRight.Controls.Add(this.btnNew);
            this.pnlRight.Controls.Add(this.label2);
            this.pnlRight.Controls.Add(this.cboParentClass);
            this.pnlRight.Controls.Add(this.txtClassName);
            this.pnlRight.Controls.Add(this.label1);
            this.pnlRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRight.Location = new System.Drawing.Point(128, 0);
            this.pnlRight.Name = "pnlRight";
            this.pnlRight.Size = new System.Drawing.Size(477, 262);
            this.pnlRight.TabIndex = 4;
            // 
            // txtID
            // 
            this.txtID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtID.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtID.FocusLostColor = System.Drawing.Color.White;
            this.txtID.Location = new System.Drawing.Point(403, 12);
            this.txtID.Name = "txtID";
            this.txtID.Size = new System.Drawing.Size(60, 20);
            this.txtID.TabIndex = 21;
            this.txtID.Visible = false;
            // 
            // txtRemarks
            // 
            this.txtRemarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtRemarks.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtRemarks.FocusLostColor = System.Drawing.Color.White;
            this.txtRemarks.Location = new System.Drawing.Point(102, 65);
            this.txtRemarks.Multiline = true;
            this.txtRemarks.Name = "txtRemarks";
            this.txtRemarks.Size = new System.Drawing.Size(188, 112);
            this.txtRemarks.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(45, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 13);
            this.label3.TabIndex = 19;
            this.label3.Text = "Remarks:";
            // 
            // btnCancel
            // 
            this.btnCancel.Image = global::Accounts.Properties.Resources.gnome_window_close;
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(388, 226);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Image = global::Accounts.Properties.Resources.document_edit;
            this.btnEdit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEdit.Location = new System.Drawing.Point(145, 226);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(75, 23);
            this.btnEdit.TabIndex = 4;
            this.btnEdit.Text = "&Edit";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Image = global::Accounts.Properties.Resources.document_delete;
            this.btnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDelete.Location = new System.Drawing.Point(307, 226);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 6;
            this.btnDelete.Text = "&Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnSave
            // 
            this.btnSave.Image = global::Accounts.Properties.Resources.save;
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSave.Location = new System.Drawing.Point(226, 226);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 5;
            this.btnSave.Text = "&Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnNew
            // 
            this.btnNew.Image = global::Accounts.Properties.Resources.edit_add;
            this.btnNew.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNew.Location = new System.Drawing.Point(64, 226);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(75, 23);
            this.btnNew.TabIndex = 3;
            this.btnNew.Text = "&New";
            this.btnNew.UseVisualStyleBackColor = true;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(31, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Parent Class:";
            // 
            // cboParentClass
            // 
            this.cboParentClass.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.cboParentClass.FocusLostColor = System.Drawing.Color.White;
            this.cboParentClass.FormattingEnabled = true;
            this.cboParentClass.Location = new System.Drawing.Point(103, 38);
            this.cboParentClass.Name = "cboParentClass";
            this.cboParentClass.Size = new System.Drawing.Size(188, 21);
            this.cboParentClass.TabIndex = 1;
            this.cboParentClass.SelectedIndexChanged += new System.EventHandler(this.cboParentClass_SelectedIndexChanged);
            this.cboParentClass.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cboParentClass_KeyDown);
            // 
            // txtClassName
            // 
            this.txtClassName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtClassName.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtClassName.FocusLostColor = System.Drawing.Color.White;
            this.txtClassName.Location = new System.Drawing.Point(103, 12);
            this.txtClassName.Name = "txtClassName";
            this.txtClassName.Size = new System.Drawing.Size(188, 20);
            this.txtClassName.TabIndex = 0;
            this.txtClassName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtClassName_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(34, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Class Name:";
            // 
            // collapsibleSplitter1
            // 
            this.collapsibleSplitter1.AnimationDelay = 20;
            this.collapsibleSplitter1.AnimationStep = 20;
            this.collapsibleSplitter1.BorderStyle3D = System.Windows.Forms.Border3DStyle.Flat;
            this.collapsibleSplitter1.ControlToHide = this.pnlLeft;
            this.collapsibleSplitter1.ExpandParentForm = false;
            this.collapsibleSplitter1.Location = new System.Drawing.Point(120, 0);
            this.collapsibleSplitter1.Name = "collapsibleSplitter1";
            this.collapsibleSplitter1.TabIndex = 5;
            this.collapsibleSplitter1.TabStop = false;
            this.collapsibleSplitter1.UseAnimations = false;
            this.collapsibleSplitter1.VisualStyle = NJFLib.Controls.VisualStyles.Mozilla;
            // 
            // frmAccountClass
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(605, 262);
            this.Controls.Add(this.pnlRight);
            this.Controls.Add(this.collapsibleSplitter1);
            this.Controls.Add(this.pnlLeft);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "frmAccountClass";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Account Class Maintainance";
            this.Load += new System.EventHandler(this.frmAccountClass_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmAccountClass_KeyDown);
            this.pnlLeft.ResumeLayout(false);
            this.pnlRight.ResumeLayout(false);
            this.pnlRight.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlLeft;
        private System.Windows.Forms.Panel pnlRight;
        private NJFLib.Controls.CollapsibleSplitter collapsibleSplitter1;
        private SComboBox cboParentClass;
        private STextBox txtClassName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TreeView treeAccountClass;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnNew;
        private STextBox txtRemarks;
        private System.Windows.Forms.Label label3;
        private STextBox txtID;
    }
}