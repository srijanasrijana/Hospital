namespace AccSwift
{
    partial class frmReminderList
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tpGeneral = new System.Windows.Forms.TabPage();
            this.txtReminderID = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSnooze = new System.Windows.Forms.Button();
            this.btnDismiss = new System.Windows.Forms.Button();
            this.lvwReminder = new System.Windows.Forms.ListView();
            this.headerSN = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.headerSubject = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.headerStatus = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.headerPriority = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.headerDate = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.headerDescription = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.headerReminderID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tpVoucher = new System.Windows.Forms.TabPage();
            this.txtDate = new SComponents.SMaskedTextBox();
            this.btnDate = new System.Windows.Forms.Button();
            this.btnSetup = new System.Windows.Forms.Button();
            this.grdVoucher = new SourceGrid.Grid();
            this.tabControl1.SuspendLayout();
            this.tpGeneral.SuspendLayout();
            this.tpVoucher.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tpGeneral);
            this.tabControl1.Controls.Add(this.tpVoucher);
            this.tabControl1.Location = new System.Drawing.Point(13, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(765, 397);
            this.tabControl1.TabIndex = 0;
            // 
            // tpGeneral
            // 
            this.tpGeneral.Controls.Add(this.txtReminderID);
            this.tpGeneral.Controls.Add(this.btnCancel);
            this.tpGeneral.Controls.Add(this.btnSnooze);
            this.tpGeneral.Controls.Add(this.btnDismiss);
            this.tpGeneral.Controls.Add(this.lvwReminder);
            this.tpGeneral.Location = new System.Drawing.Point(4, 22);
            this.tpGeneral.Name = "tpGeneral";
            this.tpGeneral.Padding = new System.Windows.Forms.Padding(3);
            this.tpGeneral.Size = new System.Drawing.Size(757, 371);
            this.tpGeneral.TabIndex = 0;
            this.tpGeneral.Text = "General";
            this.tpGeneral.UseVisualStyleBackColor = true;
            // 
            // txtReminderID
            // 
            this.txtReminderID.Location = new System.Drawing.Point(13, 326);
            this.txtReminderID.Name = "txtReminderID";
            this.txtReminderID.Size = new System.Drawing.Size(100, 20);
            this.txtReminderID.TabIndex = 77;
            this.txtReminderID.Text = "k";
            this.txtReminderID.Visible = false;
            // 
            // btnCancel
            // 
            this.btnCancel.Image = global::Inventory.Properties.Resources.gnome_window_close;
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(669, 332);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 29);
            this.btnCancel.TabIndex = 76;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSnooze
            // 
            this.btnSnooze.Image = global::Inventory.Properties.Resources.save;
            this.btnSnooze.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSnooze.Location = new System.Drawing.Point(512, 332);
            this.btnSnooze.Name = "btnSnooze";
            this.btnSnooze.Size = new System.Drawing.Size(70, 29);
            this.btnSnooze.TabIndex = 74;
            this.btnSnooze.Text = "&Snooze";
            this.btnSnooze.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSnooze.UseVisualStyleBackColor = true;
            this.btnSnooze.Click += new System.EventHandler(this.btnSnooze_Click);
            // 
            // btnDismiss
            // 
            this.btnDismiss.Image = global::Inventory.Properties.Resources.document_delete;
            this.btnDismiss.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDismiss.Location = new System.Drawing.Point(588, 332);
            this.btnDismiss.Name = "btnDismiss";
            this.btnDismiss.Size = new System.Drawing.Size(75, 29);
            this.btnDismiss.TabIndex = 75;
            this.btnDismiss.Text = "   &Dismiss";
            this.btnDismiss.UseVisualStyleBackColor = true;
            this.btnDismiss.Click += new System.EventHandler(this.btnDismiss_Click);
            // 
            // lvwReminder
            // 
            this.lvwReminder.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.headerSN,
            this.headerSubject,
            this.headerStatus,
            this.headerPriority,
            this.headerDate,
            this.headerDescription,
            this.headerReminderID});
            this.lvwReminder.FullRowSelect = true;
            this.lvwReminder.GridLines = true;
            this.lvwReminder.Location = new System.Drawing.Point(3, 6);
            this.lvwReminder.MultiSelect = false;
            this.lvwReminder.Name = "lvwReminder";
            this.lvwReminder.Size = new System.Drawing.Size(750, 310);
            this.lvwReminder.TabIndex = 73;
            this.lvwReminder.UseCompatibleStateImageBehavior = false;
            this.lvwReminder.View = System.Windows.Forms.View.Details;
            this.lvwReminder.SelectedIndexChanged += new System.EventHandler(this.lvwReminder_SelectedIndexChanged);
            // 
            // headerSN
            // 
            this.headerSN.Text = "S.N.";
            this.headerSN.Width = 50;
            // 
            // headerSubject
            // 
            this.headerSubject.Text = "Subject";
            this.headerSubject.Width = 150;
            // 
            // headerStatus
            // 
            this.headerStatus.Text = "Status";
            this.headerStatus.Width = 75;
            // 
            // headerPriority
            // 
            this.headerPriority.Text = "Priority";
            this.headerPriority.Width = 75;
            // 
            // headerDate
            // 
            this.headerDate.Text = "Date";
            this.headerDate.Width = 100;
            // 
            // headerDescription
            // 
            this.headerDescription.Text = "Description";
            this.headerDescription.Width = 276;
            // 
            // headerReminderID
            // 
            this.headerReminderID.Text = "ReminderID";
            this.headerReminderID.Width = 0;
            // 
            // tpVoucher
            // 
            this.tpVoucher.Controls.Add(this.txtDate);
            this.tpVoucher.Controls.Add(this.btnDate);
            this.tpVoucher.Controls.Add(this.btnSetup);
            this.tpVoucher.Controls.Add(this.grdVoucher);
            this.tpVoucher.Location = new System.Drawing.Point(4, 22);
            this.tpVoucher.Name = "tpVoucher";
            this.tpVoucher.Padding = new System.Windows.Forms.Padding(3);
            this.tpVoucher.Size = new System.Drawing.Size(757, 371);
            this.tpVoucher.TabIndex = 1;
            this.tpVoucher.Text = "Voucher";
            this.tpVoucher.UseVisualStyleBackColor = true;
            // 
            // txtDate
            // 
            this.txtDate.BackColor = System.Drawing.Color.FloralWhite;
            this.txtDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDate.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.txtDate.FocusLostColor = System.Drawing.Color.White;
            this.txtDate.Location = new System.Drawing.Point(542, 337);
            this.txtDate.Mask = "0000/00/00";
            this.txtDate.Name = "txtDate";
            this.txtDate.Size = new System.Drawing.Size(108, 20);
            this.txtDate.TabIndex = 219;
            this.txtDate.TextChanged += new System.EventHandler(this.txtDate_TextChanged);
            // 
            // btnDate
            // 
            this.btnDate.BackColor = System.Drawing.Color.FloralWhite;
            this.btnDate.Image = global::Inventory.Properties.Resources.dateIcon;
            this.btnDate.Location = new System.Drawing.Point(656, 336);
            this.btnDate.Name = "btnDate";
            this.btnDate.Size = new System.Drawing.Size(26, 23);
            this.btnDate.TabIndex = 218;
            this.btnDate.UseVisualStyleBackColor = false;
            this.btnDate.Click += new System.EventHandler(this.btnDate_Click);
            // 
            // btnSetup
            // 
            this.btnSetup.Location = new System.Drawing.Point(695, 335);
            this.btnSetup.Name = "btnSetup";
            this.btnSetup.Size = new System.Drawing.Size(56, 23);
            this.btnSetup.TabIndex = 217;
            this.btnSetup.Text = "Setup";
            this.btnSetup.UseVisualStyleBackColor = true;
            this.btnSetup.Click += new System.EventHandler(this.btnSetup_Click);
            // 
            // grdVoucher
            // 
            this.grdVoucher.EnableSort = true;
            this.grdVoucher.Location = new System.Drawing.Point(6, 6);
            this.grdVoucher.Name = "grdVoucher";
            this.grdVoucher.OptimizeMode = SourceGrid.CellOptimizeMode.ForRows;
            this.grdVoucher.SelectionMode = SourceGrid.GridSelectionMode.Row;
            this.grdVoucher.Size = new System.Drawing.Size(745, 321);
            this.grdVoucher.TabIndex = 8;
            this.grdVoucher.TabStop = true;
            this.grdVoucher.ToolTipText = "";
            // 
            // frmReminderList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(786, 407);
            this.Controls.Add(this.tabControl1);
            this.KeyPreview = true;
            this.Name = "frmReminderList";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Your Reminder List";
            this.Load += new System.EventHandler(this.frmReminderList_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmReminderList_KeyDown);
            this.tabControl1.ResumeLayout(false);
            this.tpGeneral.ResumeLayout(false);
            this.tpGeneral.PerformLayout();
            this.tpVoucher.ResumeLayout(false);
            this.tpVoucher.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tpGeneral;
        private System.Windows.Forms.TabPage tpVoucher;
        private System.Windows.Forms.TextBox txtReminderID;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSnooze;
        private System.Windows.Forms.Button btnDismiss;
        private System.Windows.Forms.ListView lvwReminder;
        private System.Windows.Forms.ColumnHeader headerSN;
        private System.Windows.Forms.ColumnHeader headerSubject;
        private System.Windows.Forms.ColumnHeader headerStatus;
        private System.Windows.Forms.ColumnHeader headerPriority;
        private System.Windows.Forms.ColumnHeader headerDate;
        private System.Windows.Forms.ColumnHeader headerDescription;
        private System.Windows.Forms.ColumnHeader headerReminderID;
        private SourceGrid.Grid grdVoucher;
        private SComponents.SMaskedTextBox txtDate;
        private System.Windows.Forms.Button btnDate;
        private System.Windows.Forms.Button btnSetup;


    }
}