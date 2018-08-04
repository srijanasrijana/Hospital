using SComponents;
namespace AccSwift
{
    partial class frmReminder
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmReminder));
            this.grdReminderList = new SourceGrid.Grid();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.chkReoccurance = new System.Windows.Forms.CheckBox();
            this.pnlReminderDetail = new System.Windows.Forms.Panel();
            this.chkUserList = new System.Windows.Forms.CheckedListBox();
            this.cboAsignTo = new SComboBox();
            this.btnEdit = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.txtReminderID = new STextBox();
            this.btnNew = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.txtDescription = new STextBox();
            this.cboPriority = new SComboBox();
            this.cboStatus = new SComboBox();
            this.btnStartDate = new System.Windows.Forms.Button();
            this.txtDate = new SMaskedTextBox();
            this.txtSubject = new STextBox();
            this.pnlReminderDetail.SuspendLayout();
            this.SuspendLayout();
            // 
            // grdReminderList
            // 
            this.grdReminderList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.grdReminderList.Location = new System.Drawing.Point(3, 268);
            this.grdReminderList.Name = "grdReminderList";
            this.grdReminderList.OptimizeMode = SourceGrid.CellOptimizeMode.ForRows;
            this.grdReminderList.SelectionMode = SourceGrid.GridSelectionMode.Cell;
            this.grdReminderList.Size = new System.Drawing.Size(715, 148);
            this.grdReminderList.TabIndex = 1;
            this.grdReminderList.TabStop = true;
            this.grdReminderList.ToolTipText = "";
            this.grdReminderList.Click += new System.EventHandler(this.grdReminderList_Click);
            this.grdReminderList.Paint += new System.Windows.Forms.PaintEventHandler(this.grdReminderList_Paint);
            this.grdReminderList.DoubleClick += new System.EventHandler(this.grdReminderList_DoubleClick_1);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Subject : ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(265, 81);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Date : ";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(490, 56);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(43, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Status :";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(489, 86);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(44, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "Priority :";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(22, 162);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(66, 13);
            this.label10.TabIndex = 17;
            this.label10.Text = "Description :";
            // 
            // chkReoccurance
            // 
            this.chkReoccurance.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkReoccurance.AutoSize = true;
            this.chkReoccurance.Location = new System.Drawing.Point(593, 136);
            this.chkReoccurance.Name = "chkReoccurance";
            this.chkReoccurance.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.chkReoccurance.Size = new System.Drawing.Size(73, 23);
            this.chkReoccurance.TabIndex = 33;
            this.chkReoccurance.Text = "Recurrence";
            this.chkReoccurance.UseVisualStyleBackColor = true;
            this.chkReoccurance.CheckedChanged += new System.EventHandler(this.chkReoccurance_CheckedChanged);
            // 
            // pnlReminderDetail
            // 
            this.pnlReminderDetail.Controls.Add(this.chkUserList);
            this.pnlReminderDetail.Controls.Add(this.cboAsignTo);
            this.pnlReminderDetail.Controls.Add(this.btnEdit);
            this.pnlReminderDetail.Controls.Add(this.label6);
            this.pnlReminderDetail.Controls.Add(this.btnCancel);
            this.pnlReminderDetail.Controls.Add(this.txtReminderID);
            this.pnlReminderDetail.Controls.Add(this.btnNew);
            this.pnlReminderDetail.Controls.Add(this.chkReoccurance);
            this.pnlReminderDetail.Controls.Add(this.btnSave);
            this.pnlReminderDetail.Controls.Add(this.btnDelete);
            this.pnlReminderDetail.Controls.Add(this.txtDescription);
            this.pnlReminderDetail.Controls.Add(this.cboPriority);
            this.pnlReminderDetail.Controls.Add(this.cboStatus);
            this.pnlReminderDetail.Controls.Add(this.btnStartDate);
            this.pnlReminderDetail.Controls.Add(this.txtDate);
            this.pnlReminderDetail.Controls.Add(this.txtSubject);
            this.pnlReminderDetail.Controls.Add(this.label4);
            this.pnlReminderDetail.Controls.Add(this.label1);
            this.pnlReminderDetail.Controls.Add(this.label10);
            this.pnlReminderDetail.Controls.Add(this.label2);
            this.pnlReminderDetail.Controls.Add(this.label5);
            this.pnlReminderDetail.Location = new System.Drawing.Point(3, 3);
            this.pnlReminderDetail.Name = "pnlReminderDetail";
            this.pnlReminderDetail.Size = new System.Drawing.Size(715, 259);
            this.pnlReminderDetail.TabIndex = 33;
            // 
            // chkUserList
            // 
            this.chkUserList.CheckOnClick = true;
            this.chkUserList.FormattingEnabled = true;
            this.chkUserList.Location = new System.Drawing.Point(92, 65);
            this.chkUserList.Name = "chkUserList";
            this.chkUserList.Size = new System.Drawing.Size(145, 94);
            this.chkUserList.TabIndex = 73;
            // 
            // cboAsignTo
            // 
            this.cboAsignTo.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboAsignTo.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboAsignTo.BackColor = System.Drawing.Color.White;
            this.cboAsignTo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboAsignTo.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.cboAsignTo.FocusLostColor = System.Drawing.Color.White;
            this.cboAsignTo.FormattingEnabled = true;
            this.cboAsignTo.Location = new System.Drawing.Point(92, 38);
            this.cboAsignTo.Name = "cboAsignTo";
            this.cboAsignTo.Size = new System.Drawing.Size(145, 21);
            this.cboAsignTo.TabIndex = 70;
            this.cboAsignTo.SelectedIndexChanged += new System.EventHandler(this.cboAsignTo_SelectedIndexChanged);
            // 
            // btnEdit
            // 
            this.btnEdit.Image = global::Inventory.Properties.Resources.document_edit;
            this.btnEdit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEdit.Location = new System.Drawing.Point(361, 215);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(75, 29);
            this.btnEdit.TabIndex = 62;
            this.btnEdit.Text = "&Edit";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(22, 41);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(55, 13);
            this.label6.TabIndex = 69;
            this.label6.Text = "Asign To :";
            // 
            // btnCancel
            // 
            this.btnCancel.Image = global::Inventory.Properties.Resources.gnome_window_close;
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(604, 215);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 29);
            this.btnCancel.TabIndex = 65;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // txtReminderID
            // 
            this.txtReminderID.BackColor = System.Drawing.Color.White;
            this.txtReminderID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtReminderID.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtReminderID.FocusLostColor = System.Drawing.Color.White;
            this.txtReminderID.Location = new System.Drawing.Point(267, 41);
            this.txtReminderID.Multiline = true;
            this.txtReminderID.Name = "txtReminderID";
            this.txtReminderID.Size = new System.Drawing.Size(46, 20);
            this.txtReminderID.TabIndex = 66;
            this.txtReminderID.Text = "8";
            this.txtReminderID.Visible = false;
            // 
            // btnNew
            // 
            this.btnNew.Image = global::Inventory.Properties.Resources.edit_add;
            this.btnNew.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNew.Location = new System.Drawing.Point(280, 215);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(75, 29);
            this.btnNew.TabIndex = 61;
            this.btnNew.Text = "&New";
            this.btnNew.UseVisualStyleBackColor = true;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // btnSave
            // 
            this.btnSave.Image = global::Inventory.Properties.Resources.save;
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSave.Location = new System.Drawing.Point(442, 215);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 29);
            this.btnSave.TabIndex = 63;
            this.btnSave.Text = "&Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Image = global::Inventory.Properties.Resources.document_delete;
            this.btnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDelete.Location = new System.Drawing.Point(523, 215);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 29);
            this.btnDelete.TabIndex = 64;
            this.btnDelete.Text = "&Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // txtDescription
            // 
            this.txtDescription.BackColor = System.Drawing.Color.White;
            this.txtDescription.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDescription.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtDescription.FocusLostColor = System.Drawing.Color.White;
            this.txtDescription.Location = new System.Drawing.Point(92, 165);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(585, 44);
            this.txtDescription.TabIndex = 60;
            // 
            // cboPriority
            // 
            this.cboPriority.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboPriority.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboPriority.BackColor = System.Drawing.Color.White;
            this.cboPriority.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.cboPriority.FocusLostColor = System.Drawing.Color.White;
            this.cboPriority.FormattingEnabled = true;
            this.cboPriority.Location = new System.Drawing.Point(532, 83);
            this.cboPriority.Name = "cboPriority";
            this.cboPriority.Size = new System.Drawing.Size(145, 21);
            this.cboPriority.TabIndex = 59;
            // 
            // cboStatus
            // 
            this.cboStatus.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboStatus.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboStatus.BackColor = System.Drawing.Color.White;
            this.cboStatus.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.cboStatus.FocusLostColor = System.Drawing.Color.White;
            this.cboStatus.FormattingEnabled = true;
            this.cboStatus.Location = new System.Drawing.Point(532, 54);
            this.cboStatus.Name = "cboStatus";
            this.cboStatus.Size = new System.Drawing.Size(145, 21);
            this.cboStatus.TabIndex = 58;
            // 
            // btnStartDate
            // 
            this.btnStartDate.Image = global::Inventory.Properties.Resources.dateIcon;
            this.btnStartDate.Location = new System.Drawing.Point(401, 77);
            this.btnStartDate.Name = "btnStartDate";
            this.btnStartDate.Size = new System.Drawing.Size(26, 23);
            this.btnStartDate.TabIndex = 55;
            this.btnStartDate.UseVisualStyleBackColor = true;
            this.btnStartDate.Click += new System.EventHandler(this.btnStartDate_Click);
            // 
            // txtDate
            // 
            this.txtDate.BackColor = System.Drawing.Color.White;
            this.txtDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDate.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.txtDate.FocusLostColor = System.Drawing.Color.White;
            this.txtDate.Location = new System.Drawing.Point(320, 79);
            this.txtDate.Mask = "0000/00/00";
            this.txtDate.Name = "txtDate";
            this.txtDate.Size = new System.Drawing.Size(75, 20);
            this.txtDate.TabIndex = 54;
            // 
            // txtSubject
            // 
            this.txtSubject.BackColor = System.Drawing.Color.White;
            this.txtSubject.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSubject.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtSubject.FocusLostColor = System.Drawing.Color.White;
            this.txtSubject.Location = new System.Drawing.Point(92, 12);
            this.txtSubject.Multiline = true;
            this.txtSubject.Name = "txtSubject";
            this.txtSubject.Size = new System.Drawing.Size(585, 20);
            this.txtSubject.TabIndex = 18;
            // 
            // frmReminder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(725, 420);
            this.Controls.Add(this.pnlReminderDetail);
            this.Controls.Add(this.grdReminderList);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmReminder";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Reminder";
            this.Load += new System.EventHandler(this.frmReminder_Load);
            this.pnlReminderDetail.ResumeLayout(false);
            this.pnlReminderDetail.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private SourceGrid.Grid grdReminderList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.CheckBox chkReoccurance;
        private System.Windows.Forms.Panel pnlReminderDetail;
        private STextBox txtSubject;
        private System.Windows.Forms.Button btnStartDate;
        private SMaskedTextBox txtDate;
        private STextBox txtDescription;
        private SComboBox cboPriority;
        private SComboBox cboStatus;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnNew;
        private STextBox txtReminderID;
        private SComboBox cboAsignTo;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckedListBox chkUserList;
    }
}