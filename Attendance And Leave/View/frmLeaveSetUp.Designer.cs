using SComponents;
namespace Attendance_And_Leave
{
    partial class frmLeaveSetUp
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmLeaveSetUp));
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.grdleavesetup = new SourceGrid.Grid();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnNew = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.chktransfernextyear = new System.Windows.Forms.CheckBox();
            this.chkisaccumulated = new System.Windows.Forms.CheckBox();
            this.cmblimittype = new System.Windows.Forms.ComboBox();
            this.txtcode = new STextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtlimit = new STextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cmbleavetype = new System.Windows.Forms.ComboBox();
            this.txtleavename = new STextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.grdleavesetup);
            this.groupBox3.Location = new System.Drawing.Point(3, 4);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(417, 322);
            this.groupBox3.TabIndex = 6;
            this.groupBox3.TabStop = false;
            // 
            // grdleavesetup
            // 
            this.grdleavesetup.Location = new System.Drawing.Point(5, 9);
            this.grdleavesetup.Name = "grdleavesetup";
            this.grdleavesetup.OptimizeMode = SourceGrid.CellOptimizeMode.ForRows;
            this.grdleavesetup.SelectionMode = SourceGrid.GridSelectionMode.Cell;
            this.grdleavesetup.Size = new System.Drawing.Size(406, 305);
            this.grdleavesetup.TabIndex = 1;
            this.grdleavesetup.TabStop = true;
            this.grdleavesetup.ToolTipText = "";
            this.grdleavesetup.Click += new System.EventHandler(this.grdleavesetup_Click_1);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnEdit);
            this.groupBox2.Controls.Add(this.btnDelete);
            this.groupBox2.Controls.Add(this.btnNew);
            this.groupBox2.Controls.Add(this.btnSave);
            this.groupBox2.Controls.Add(this.chktransfernextyear);
            this.groupBox2.Controls.Add(this.chkisaccumulated);
            this.groupBox2.Controls.Add(this.cmblimittype);
            this.groupBox2.Controls.Add(this.txtcode);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.txtlimit);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.cmbleavetype);
            this.groupBox2.Controls.Add(this.txtleavename);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(430, 4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(379, 321);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            // 
            // btnEdit
            // 
           // this.btnEdit.Image = global::Inventory.Properties.Resources.document_edit;
            this.btnEdit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEdit.Location = new System.Drawing.Point(100, 269);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(75, 23);
            this.btnEdit.TabIndex = 39;
            this.btnEdit.Text = "&Edit";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnDelete
            // 
           // this.btnDelete.Image = global::Inventory.Properties.Resources.document_delete;
            this.btnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDelete.Location = new System.Drawing.Point(265, 267);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 37;
            this.btnDelete.Text = "&Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnNew
            // 
            this.btnNew.Image = ((System.Drawing.Image)(resources.GetObject("btnNew.Image")));
            this.btnNew.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNew.Location = new System.Drawing.Point(19, 268);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(75, 24);
            this.btnNew.TabIndex = 36;
            this.btnNew.Text = "&New";
            this.btnNew.UseVisualStyleBackColor = true;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // btnSave
            // 
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSave.Location = new System.Drawing.Point(184, 268);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 24);
            this.btnSave.TabIndex = 38;
            this.btnSave.Text = "&Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // chktransfernextyear
            // 
            this.chktransfernextyear.AutoSize = true;
            this.chktransfernextyear.Location = new System.Drawing.Point(174, 206);
            this.chktransfernextyear.Name = "chktransfernextyear";
            this.chktransfernextyear.Size = new System.Drawing.Size(115, 17);
            this.chktransfernextyear.TabIndex = 35;
            this.chktransfernextyear.Text = "Transfer Next Year";
            this.chktransfernextyear.UseVisualStyleBackColor = true;
            // 
            // chkisaccumulated
            // 
            this.chkisaccumulated.AutoSize = true;
            this.chkisaccumulated.Location = new System.Drawing.Point(31, 207);
            this.chkisaccumulated.Name = "chkisaccumulated";
            this.chkisaccumulated.Size = new System.Drawing.Size(88, 17);
            this.chkisaccumulated.TabIndex = 34;
            this.chkisaccumulated.Text = "Accumulated";
            this.chkisaccumulated.UseVisualStyleBackColor = true;
            // 
            // cmblimittype
            // 
            this.cmblimittype.FormattingEnabled = true;
            this.cmblimittype.Items.AddRange(new object[] {
            "Monthly",
            "Yearly"});
            this.cmblimittype.Location = new System.Drawing.Point(199, 160);
            this.cmblimittype.Name = "cmblimittype";
            this.cmblimittype.Size = new System.Drawing.Size(90, 21);
            this.cmblimittype.TabIndex = 33;
            // 
            // txtcode
            // 
            this.txtcode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtcode.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtcode.FocusLostColor = System.Drawing.Color.White;
            this.txtcode.Location = new System.Drawing.Point(138, 67);
            this.txtcode.Name = "txtcode";
            this.txtcode.Size = new System.Drawing.Size(151, 20);
            this.txtcode.TabIndex = 32;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(32, 74);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(32, 13);
            this.label6.TabIndex = 31;
            this.label6.Text = "Code";
            // 
            // txtlimit
            // 
            this.txtlimit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtlimit.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtlimit.FocusLostColor = System.Drawing.Color.White;
            this.txtlimit.Location = new System.Drawing.Point(138, 161);
            this.txtlimit.Name = "txtlimit";
            this.txtlimit.Size = new System.Drawing.Size(55, 20);
            this.txtlimit.TabIndex = 24;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(32, 163);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(28, 13);
            this.label5.TabIndex = 25;
            this.label5.Text = "Limit";
            // 
            // cmbleavetype
            // 
            this.cmbleavetype.FormattingEnabled = true;
            this.cmbleavetype.Items.AddRange(new object[] {
            "Paid",
            "UnPaid"});
            this.cmbleavetype.Location = new System.Drawing.Point(138, 112);
            this.cmbleavetype.Name = "cmbleavetype";
            this.cmbleavetype.Size = new System.Drawing.Size(151, 21);
            this.cmbleavetype.TabIndex = 23;
            // 
            // txtleavename
            // 
            this.txtleavename.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtleavename.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtleavename.FocusLostColor = System.Drawing.Color.White;
            this.txtleavename.Location = new System.Drawing.Point(138, 26);
            this.txtleavename.Name = "txtleavename";
            this.txtleavename.Size = new System.Drawing.Size(151, 20);
            this.txtleavename.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(28, 120);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 13);
            this.label2.TabIndex = 20;
            this.label2.Text = "Leave Type";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(28, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 13);
            this.label1.TabIndex = 19;
            this.label1.Text = "Leave Name";
            // 
            // frmLeaveSetUp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(811, 332);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox3);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmLeaveSetUp";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "LeaveSetUp";
            this.Load += new System.EventHandler(this.frmLeaveSetUp_Load);
            this.groupBox3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox3;
        private SourceGrid.Grid grdleavesetup;
        private System.Windows.Forms.GroupBox groupBox2;
        private STextBox txtleavename;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbleavetype;
        private STextBox txtlimit;
        private System.Windows.Forms.Label label5;
        private STextBox txtcode;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox chktransfernextyear;
        private System.Windows.Forms.CheckBox chkisaccumulated;
        private System.Windows.Forms.ComboBox cmblimittype;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.Button btnSave;
    }
}