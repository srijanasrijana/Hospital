using SComponents;
namespace Inventory
{
    partial class frmProject
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmProject));
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.txtProjectID = new STextBox();
            this.cboParentProjectName = new SComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtProjectName = new STextBox();
            this.txtDescription = new STextBox();
            this.tabDisplay = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tvProject = new System.Windows.Forms.TreeView();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.grid1 = new SourceGrid.Grid();
            this.grdListView = new SourceGrid.Grid();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnNew = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtSearchProjectName = new SMaskedTextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtSearchDescription = new STextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.cboUnder = new SComboBox();
            this.groupBox3.SuspendLayout();
            this.tabDisplay.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.grid1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.txtProjectID);
            this.groupBox3.Controls.Add(this.cboParentProjectName);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.txtProjectName);
            this.groupBox3.Controls.Add(this.txtDescription);
            this.groupBox3.Location = new System.Drawing.Point(17, 96);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(272, 181);
            this.groupBox3.TabIndex = 23;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Project";
            // 
            // txtProjectID
            // 
            this.txtProjectID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtProjectID.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtProjectID.FocusLostColor = System.Drawing.Color.White;
            this.txtProjectID.Location = new System.Drawing.Point(22, 140);
            this.txtProjectID.Name = "txtProjectID";
            this.txtProjectID.Size = new System.Drawing.Size(26, 20);
            this.txtProjectID.TabIndex = 8;
            // 
            // cboParentProjectName
            // 
            this.cboParentProjectName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboParentProjectName.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.cboParentProjectName.FocusLostColor = System.Drawing.Color.White;
            this.cboParentProjectName.FormattingEnabled = true;
            this.cboParentProjectName.Location = new System.Drawing.Point(86, 42);
            this.cboParentProjectName.Name = "cboParentProjectName";
            this.cboParentProjectName.Size = new System.Drawing.Size(170, 21);
            this.cboParentProjectName.TabIndex = 2;
            this.cboParentProjectName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cboParentProjectName_KeyDown);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 23);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(39, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "*Name";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 51);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(43, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "*Under:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 99);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(67, 13);
            this.label7.TabIndex = 4;
            this.label7.Text = "*Description:";
            // 
            // txtProjectName
            // 
            this.txtProjectName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtProjectName.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtProjectName.FocusLostColor = System.Drawing.Color.White;
            this.txtProjectName.Location = new System.Drawing.Point(86, 16);
            this.txtProjectName.Name = "txtProjectName";
            this.txtProjectName.Size = new System.Drawing.Size(170, 20);
            this.txtProjectName.TabIndex = 0;
            this.txtProjectName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtProjectName_KeyDown);
            // 
            // txtDescription
            // 
            this.txtDescription.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDescription.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtDescription.FocusLostColor = System.Drawing.Color.White;
            this.txtDescription.Location = new System.Drawing.Point(86, 69);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(170, 88);
            this.txtDescription.TabIndex = 3;
            // 
            // tabDisplay
            // 
            this.tabDisplay.Controls.Add(this.tabPage1);
            this.tabDisplay.Controls.Add(this.tabPage2);
            this.tabDisplay.Location = new System.Drawing.Point(295, 96);
            this.tabDisplay.Name = "tabDisplay";
            this.tabDisplay.SelectedIndex = 0;
            this.tabDisplay.Size = new System.Drawing.Size(315, 181);
            this.tabDisplay.TabIndex = 24;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.tvProject);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(307, 155);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "TreeView";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tvProject
            // 
            this.tvProject.Location = new System.Drawing.Point(6, 0);
            this.tvProject.Name = "tvProject";
            this.tvProject.Size = new System.Drawing.Size(295, 132);
            this.tvProject.TabIndex = 0;
            this.tvProject.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvProject_AfterSelect);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.grid1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.tabPage2.Size = new System.Drawing.Size(307, 155);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "ListView";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // grid1
            // 
            this.grid1.Controls.Add(this.grdListView);
            this.grid1.Location = new System.Drawing.Point(-4, 3);
            this.grid1.Name = "grid1";
            this.grid1.OptimizeMode = SourceGrid.CellOptimizeMode.ForRows;
            this.grid1.SelectionMode = SourceGrid.GridSelectionMode.Cell;
            this.grid1.Size = new System.Drawing.Size(305, 129);
            this.grid1.TabIndex = 0;
            this.grid1.TabStop = true;
            this.grid1.ToolTipText = "";
            // 
            // grdListView
            // 
            this.grdListView.Location = new System.Drawing.Point(3, 3);
            this.grdListView.Name = "grdListView";
            this.grdListView.OptimizeMode = SourceGrid.CellOptimizeMode.ForRows;
            this.grdListView.SelectionMode = SourceGrid.GridSelectionMode.Cell;
            this.grdListView.Size = new System.Drawing.Size(299, 116);
            this.grdListView.TabIndex = 4;
            this.grdListView.TabStop = true;
            this.grdListView.ToolTipText = "";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnNew);
            this.groupBox2.Controls.Add(this.btnCancel);
            this.groupBox2.Controls.Add(this.btnSave);
            this.groupBox2.Controls.Add(this.btnDelete);
            this.groupBox2.Controls.Add(this.btnEdit);
            this.groupBox2.Location = new System.Drawing.Point(17, 284);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(600, 59);
            this.groupBox2.TabIndex = 25;
            this.groupBox2.TabStop = false;
            // 
            // btnNew
            // 
            this.btnNew.Image = global::Inventory.Properties.Resources.edit_add;
            this.btnNew.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNew.Location = new System.Drawing.Point(86, 19);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(75, 23);
            this.btnNew.TabIndex = 0;
            this.btnNew.Text = "&New";
            this.btnNew.UseVisualStyleBackColor = true;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Image = global::Inventory.Properties.Resources.gnome_window_close;
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(410, 19);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(77, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Image = global::Inventory.Properties.Resources.save;
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSave.Location = new System.Drawing.Point(248, 19);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "&Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Image = global::Inventory.Properties.Resources.document_delete;
            this.btnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDelete.Location = new System.Drawing.Point(329, 19);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 3;
            this.btnDelete.Text = "&Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Image = global::Inventory.Properties.Resources.document_edit;
            this.btnEdit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEdit.Location = new System.Drawing.Point(167, 19);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(75, 23);
            this.btnEdit.TabIndex = 2;
            this.btnEdit.Text = "&Edit";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtSearchProjectName);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.txtSearchDescription);
            this.groupBox1.Controls.Add(this.btnSearch);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.cboUnder);
            this.groupBox1.Location = new System.Drawing.Point(17, 9);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(593, 76);
            this.groupBox1.TabIndex = 26;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Search Box";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 13);
            this.label2.TabIndex = 22;
            this.label2.Text = "Under:";
            // 
            // txtSearchProjectName
            // 
            this.txtSearchProjectName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSearchProjectName.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.txtSearchProjectName.FocusLostColor = System.Drawing.Color.White;
            this.txtSearchProjectName.Location = new System.Drawing.Point(86, 19);
            this.txtSearchProjectName.Name = "txtSearchProjectName";
            this.txtSearchProjectName.Size = new System.Drawing.Size(170, 20);
            this.txtSearchProjectName.TabIndex = 21;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(275, 35);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(63, 13);
            this.label8.TabIndex = 20;
            this.label8.Text = "Description:";
            // 
            // txtSearchDescription
            // 
            this.txtSearchDescription.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSearchDescription.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtSearchDescription.FocusLostColor = System.Drawing.Color.White;
            this.txtSearchDescription.Location = new System.Drawing.Point(350, 19);
            this.txtSearchDescription.Multiline = true;
            this.txtSearchDescription.Name = "txtSearchDescription";
            this.txtSearchDescription.Size = new System.Drawing.Size(152, 47);
            this.txtSearchDescription.TabIndex = 2;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(508, 30);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 3;
            this.btnSearch.Text = "&Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click_1);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Project Name:";
            // 
            // cboUnder
            // 
            this.cboUnder.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboUnder.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.cboUnder.FocusLostColor = System.Drawing.Color.White;
            this.cboUnder.FormattingEnabled = true;
            this.cboUnder.Location = new System.Drawing.Point(86, 49);
            this.cboUnder.Name = "cboUnder";
            this.cboUnder.Size = new System.Drawing.Size(170, 21);
            this.cboUnder.TabIndex = 0;
            // 
            // frmProject
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(636, 354);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.tabDisplay);
            this.Controls.Add(this.groupBox3);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "frmProject";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Project";
            this.Load += new System.EventHandler(this.frmProject_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmProject_KeyDown);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.tabDisplay.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.grid1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox3;
        private STextBox txtProjectID;
        private SComboBox cboParentProjectName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label7;
        private STextBox txtProjectName;
        private STextBox txtDescription;
        private System.Windows.Forms.TabControl tabDisplay;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TreeView tvProject;
        private System.Windows.Forms.TabPage tabPage2;
        private SourceGrid.Grid grid1;
        private SourceGrid.Grid grdListView;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label8;
        private STextBox txtSearchDescription;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Label label1;
        private SComboBox cboUnder;
        private SMaskedTextBox txtSearchProjectName;
        private System.Windows.Forms.Label label2;
    }
}