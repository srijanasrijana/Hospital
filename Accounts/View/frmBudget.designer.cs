namespace Accounts.View
{
    partial class frmBudget
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtBudgetName = new System.Windows.Forms.TextBox();
            this.txtDescription = new SComponents.STextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtEndDate = new SComponents.SMaskedTextBox();
            this.btnEndDate = new System.Windows.Forms.Button();
            this.txtStartDate = new SComponents.SMaskedTextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnStartDate = new System.Windows.Forms.Button();
            this.grdShowBudget = new SourceGrid.Grid();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnNew = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(36, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Budget Name:";
            // 
            // txtBudgetName
            // 
            this.txtBudgetName.Location = new System.Drawing.Point(127, 31);
            this.txtBudgetName.Name = "txtBudgetName";
            this.txtBudgetName.Size = new System.Drawing.Size(193, 20);
            this.txtBudgetName.TabIndex = 1;
            // 
            // txtDescription
            // 
            this.txtDescription.BackColor = System.Drawing.Color.White;
            this.txtDescription.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDescription.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtDescription.FocusLostColor = System.Drawing.Color.White;
            this.txtDescription.Location = new System.Drawing.Point(127, 106);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(298, 20);
            this.txtDescription.TabIndex = 8;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(48, 108);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Description:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(240, 74);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "Ending Date:";
            // 
            // txtEndDate
            // 
            this.txtEndDate.BackColor = System.Drawing.Color.White;
            this.txtEndDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtEndDate.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.txtEndDate.FocusLostColor = System.Drawing.Color.White;
            this.txtEndDate.Location = new System.Drawing.Point(315, 72);
            this.txtEndDate.Mask = "0000/00/00";
            this.txtEndDate.Name = "txtEndDate";
            this.txtEndDate.Size = new System.Drawing.Size(75, 20);
            this.txtEndDate.TabIndex = 9;
            // 
            // btnEndDate
            // 
            this.btnEndDate.Image = global::Accounts.Properties.Resources.dateIcon;
            this.btnEndDate.Location = new System.Drawing.Point(393, 70);
            this.btnEndDate.Name = "btnEndDate";
            this.btnEndDate.Size = new System.Drawing.Size(26, 23);
            this.btnEndDate.TabIndex = 10;
            this.btnEndDate.UseVisualStyleBackColor = true;
            this.btnEndDate.Click += new System.EventHandler(this.btnEndDate_Click);
            // 
            // txtStartDate
            // 
            this.txtStartDate.BackColor = System.Drawing.Color.White;
            this.txtStartDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtStartDate.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.txtStartDate.FocusLostColor = System.Drawing.Color.White;
            this.txtStartDate.Location = new System.Drawing.Point(127, 69);
            this.txtStartDate.Mask = "0000/00/00";
            this.txtStartDate.Name = "txtStartDate";
            this.txtStartDate.Size = new System.Drawing.Size(75, 20);
            this.txtStartDate.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(39, 72);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(72, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "Starting Date:";
            this.label4.Click += new System.EventHandler(this.label4_Click);
            // 
            // btnStartDate
            // 
            this.btnStartDate.Image = global::Accounts.Properties.Resources.dateIcon;
            this.btnStartDate.Location = new System.Drawing.Point(205, 67);
            this.btnStartDate.Name = "btnStartDate";
            this.btnStartDate.Size = new System.Drawing.Size(26, 23);
            this.btnStartDate.TabIndex = 10;
            this.btnStartDate.UseVisualStyleBackColor = true;
            this.btnStartDate.Click += new System.EventHandler(this.btnStartDate_Click);
            // 
            // grdShowBudget
            // 
            this.grdShowBudget.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.grdShowBudget.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdShowBudget.EnableSort = true;
            this.grdShowBudget.Location = new System.Drawing.Point(3, 16);
            this.grdShowBudget.Name = "grdShowBudget";
            this.grdShowBudget.OptimizeMode = SourceGrid.CellOptimizeMode.ForRows;
            this.grdShowBudget.SelectionMode = SourceGrid.GridSelectionMode.Row;
            this.grdShowBudget.Size = new System.Drawing.Size(589, 130);
            this.grdShowBudget.TabIndex = 13;
            this.grdShowBudget.TabStop = true;
            this.grdShowBudget.ToolTipText = "";
            this.grdShowBudget.Paint += new System.Windows.Forms.PaintEventHandler(this.grdShowBudget_Paint);
            // 
            // btnCancel
            // 
            this.btnCancel.Image = global::Accounts.Properties.Resources.gnome_window_close;
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(451, 150);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 12;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Image = global::Accounts.Properties.Resources.document_edit;
            this.btnEdit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEdit.Location = new System.Drawing.Point(208, 150);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(75, 23);
            this.btnEdit.TabIndex = 9;
            this.btnEdit.Text = "&Edit";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Image = global::Accounts.Properties.Resources.document_delete;
            this.btnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDelete.Location = new System.Drawing.Point(370, 150);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 11;
            this.btnDelete.Text = "&Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnSave
            // 
            this.btnSave.Image = global::Accounts.Properties.Resources.save;
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSave.Location = new System.Drawing.Point(289, 150);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 10;
            this.btnSave.Text = "&Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnNew
            // 
            this.btnNew.Image = global::Accounts.Properties.Resources.edit_add;
            this.btnNew.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNew.Location = new System.Drawing.Point(127, 150);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(75, 23);
            this.btnNew.TabIndex = 8;
            this.btnNew.Text = "&New";
            this.btnNew.UseVisualStyleBackColor = true;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtBudgetName);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.btnEndDate);
            this.groupBox1.Controls.Add(this.btnStartDate);
            this.groupBox1.Controls.Add(this.txtDescription);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.txtStartDate);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtEndDate);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(595, 144);
            this.groupBox1.TabIndex = 14;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Add Budget";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.grdShowBudget);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox2.Location = new System.Drawing.Point(0, 179);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(595, 149);
            this.groupBox2.TabIndex = 15;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Current and Upcomming Budget";
            // 
            // frmBudget
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(595, 328);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnNew);
            this.Controls.Add(this.btnCancel);
            this.Name = "frmBudget";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Add Budget";
            this.Load += new System.EventHandler(this.frmBudget_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtBudgetName;
        private System.Windows.Forms.Button btnEndDate;
        private SComponents.STextBox txtDescription;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private SComponents.SMaskedTextBox txtEndDate;
        private SComponents.SMaskedTextBox txtStartDate;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnStartDate;
        private SourceGrid.Grid grdShowBudget;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
    }
}