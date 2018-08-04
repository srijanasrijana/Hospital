namespace Attendance_And_Leave
{
    partial class frmLeaveRequest
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmLeaveRequest));
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.lblemployeename = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.chkhalfleave = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtemployeecode = new System.Windows.Forms.TextBox();
            this.cmbleavetype = new System.Windows.Forms.ComboBox();
            this.rtxtreason = new System.Windows.Forms.RichTextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.btnDate = new System.Windows.Forms.Button();
            this.txtFromDate = new System.Windows.Forms.MaskedTextBox();
            this.txtToDate = new System.Windows.Forms.MaskedTextBox();
            this.btnrequestleave = new System.Windows.Forms.Button();
            this.btncancel = new System.Windows.Forms.Button();
            this.grdleaverequest = new SourceGrid.Grid();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.btnDate);
            this.panel1.Controls.Add(this.txtFromDate);
            this.panel1.Controls.Add(this.txtToDate);
            this.panel1.Controls.Add(this.rtxtreason);
            this.panel1.Controls.Add(this.cmbleavetype);
            this.panel1.Controls.Add(this.txtemployeecode);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.chkhalfleave);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.lblemployeename);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(2, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(413, 204);
            this.panel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Employee Code:";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.grdleaverequest);
            this.panel2.Location = new System.Drawing.Point(415, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(265, 234);
            this.panel2.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(207, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Name:";
            // 
            // lblemployeename
            // 
            this.lblemployeename.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.lblemployeename.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblemployeename.Location = new System.Drawing.Point(251, 10);
            this.lblemployeename.Name = "lblemployeename";
            this.lblemployeename.Size = new System.Drawing.Size(159, 13);
            this.lblemployeename.TabIndex = 2;
            this.lblemployeename.Text = "lblemployeename";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 44);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Leave Type:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(207, 72);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(20, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "To";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 72);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(30, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "From";
            // 
            // chkhalfleave
            // 
            this.chkhalfleave.AutoSize = true;
            this.chkhalfleave.Location = new System.Drawing.Point(13, 104);
            this.chkhalfleave.Name = "chkhalfleave";
            this.chkhalfleave.Size = new System.Drawing.Size(78, 17);
            this.chkhalfleave.TabIndex = 6;
            this.chkhalfleave.Text = "Half Leave";
            this.chkhalfleave.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(10, 138);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(91, 13);
            this.label6.TabIndex = 7;
            this.label6.Text = "Reason Of Leave";
            // 
            // txtemployeecode
            // 
            this.txtemployeecode.Location = new System.Drawing.Point(100, 10);
            this.txtemployeecode.Name = "txtemployeecode";
            this.txtemployeecode.Size = new System.Drawing.Size(91, 20);
            this.txtemployeecode.TabIndex = 8;
            this.txtemployeecode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtemployeecode_KeyDown);
            this.txtemployeecode.Leave += new System.EventHandler(this.txtemployeecode_Leave);
            this.txtemployeecode.MouseDown += new System.Windows.Forms.MouseEventHandler(this.txtemployeecode_MouseDown);
            // 
            // cmbleavetype
            // 
            this.cmbleavetype.FormattingEnabled = true;
            this.cmbleavetype.Location = new System.Drawing.Point(100, 44);
            this.cmbleavetype.Name = "cmbleavetype";
            this.cmbleavetype.Size = new System.Drawing.Size(136, 21);
            this.cmbleavetype.TabIndex = 9;
            // 
            // rtxtreason
            // 
            this.rtxtreason.Location = new System.Drawing.Point(127, 141);
            this.rtxtreason.Name = "rtxtreason";
            this.rtxtreason.Size = new System.Drawing.Size(264, 49);
            this.rtxtreason.TabIndex = 10;
            this.rtxtreason.Text = "";
            // 
            // button1
            // 
          //  this.button1.Image = global::Inventory.Properties.Resources.dateIcon;
            this.button1.Location = new System.Drawing.Point(365, 68);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(26, 25);
            this.button1.TabIndex = 14;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnDate
            // 
           // this.btnDate.Image = global::Inventory.Properties.Resources.dateIcon;
            this.btnDate.Location = new System.Drawing.Point(177, 68);
            this.btnDate.Name = "btnDate";
            this.btnDate.Size = new System.Drawing.Size(26, 25);
            this.btnDate.TabIndex = 12;
            this.btnDate.UseVisualStyleBackColor = true;
            this.btnDate.Click += new System.EventHandler(this.btnDate_Click);
            // 
            // txtFromDate
            // 
            this.txtFromDate.Location = new System.Drawing.Point(46, 71);
            this.txtFromDate.Mask = "##/##/####";
            this.txtFromDate.Name = "txtFromDate";
            this.txtFromDate.Size = new System.Drawing.Size(125, 20);
            this.txtFromDate.TabIndex = 11;
            // 
            // txtToDate
            // 
            this.txtToDate.Location = new System.Drawing.Point(233, 69);
            this.txtToDate.Mask = "##/##/####";
            this.txtToDate.Name = "txtToDate";
            this.txtToDate.Size = new System.Drawing.Size(126, 20);
            this.txtToDate.TabIndex = 13;
            // 
            // btnrequestleave
            // 
            this.btnrequestleave.Location = new System.Drawing.Point(4, 213);
            this.btnrequestleave.Name = "btnrequestleave";
            this.btnrequestleave.Size = new System.Drawing.Size(122, 23);
            this.btnrequestleave.TabIndex = 2;
            this.btnrequestleave.Text = "Request Leave";
            this.btnrequestleave.UseVisualStyleBackColor = true;
            this.btnrequestleave.Click += new System.EventHandler(this.btnrequestleave_Click);
            // 
            // btncancel
            // 
            this.btncancel.Location = new System.Drawing.Point(132, 213);
            this.btncancel.Name = "btncancel";
            this.btncancel.Size = new System.Drawing.Size(75, 23);
            this.btncancel.TabIndex = 3;
            this.btncancel.Text = "Cancel";
            this.btncancel.UseVisualStyleBackColor = true;
            this.btncancel.Click += new System.EventHandler(this.btncancel_Click);
            // 
            // grdleaverequest
            // 
            this.grdleaverequest.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grdleaverequest.Location = new System.Drawing.Point(4, 5);
            this.grdleaverequest.Name = "grdleaverequest";
            this.grdleaverequest.OptimizeMode = SourceGrid.CellOptimizeMode.ForRows;
            this.grdleaverequest.SelectionMode = SourceGrid.GridSelectionMode.Cell;
            this.grdleaverequest.Size = new System.Drawing.Size(258, 225);
            this.grdleaverequest.TabIndex = 7;
            this.grdleaverequest.TabStop = true;
            this.grdleaverequest.ToolTipText = "";
            // 
            // frmLeaveRequest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(681, 242);
            this.Controls.Add(this.btncancel);
            this.Controls.Add(this.btnrequestleave);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmLeaveRequest";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "LeaveRequest";
            this.Load += new System.EventHandler(this.frmLeaveRequest_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lblemployeename;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox chkhalfleave;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtemployeecode;
        private System.Windows.Forms.ComboBox cmbleavetype;
        private System.Windows.Forms.RichTextBox rtxtreason;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnDate;
        private System.Windows.Forms.MaskedTextBox txtFromDate;
        private System.Windows.Forms.MaskedTextBox txtToDate;
        private System.Windows.Forms.Button btnrequestleave;
        private System.Windows.Forms.Button btncancel;
        private SourceGrid.Grid grdleaverequest;
    }
}