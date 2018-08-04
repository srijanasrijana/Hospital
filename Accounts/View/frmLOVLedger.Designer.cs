using SComponents;
namespace Accounts
{
    partial class frmLOVLedger
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkDisplayZeroBalance = new System.Windows.Forms.CheckBox();
            this.txtLedgerCode = new SComponents.STextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtGroupName = new SComponents.STextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtLedgerName = new SComponents.STextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnCreate = new System.Windows.Forms.Button();
            this.dgListOfLedger = new SComponents.SDataGrid(this.components);
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.chkDisplayZeroBalance);
            this.groupBox1.Controls.Add(this.txtLedgerCode);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.txtGroupName);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtLedgerName);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 443);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(465, 70);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Filter By:";
            // 
            // chkDisplayZeroBalance
            // 
            this.chkDisplayZeroBalance.AutoSize = true;
            this.chkDisplayZeroBalance.Checked = true;
            this.chkDisplayZeroBalance.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDisplayZeroBalance.Location = new System.Drawing.Point(187, 10);
            this.chkDisplayZeroBalance.Name = "chkDisplayZeroBalance";
            this.chkDisplayZeroBalance.Size = new System.Drawing.Size(90, 17);
            this.chkDisplayZeroBalance.TabIndex = 9;
            this.chkDisplayZeroBalance.Text = "Zero Balance";
            this.chkDisplayZeroBalance.UseVisualStyleBackColor = true;
            this.chkDisplayZeroBalance.Visible = false;
            this.chkDisplayZeroBalance.CheckedChanged += new System.EventHandler(this.chkDisplayZeroBalance_CheckedChanged);
            // 
            // txtLedgerCode
            // 
            this.txtLedgerCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtLedgerCode.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtLedgerCode.FocusLostColor = System.Drawing.Color.White;
            this.txtLedgerCode.Location = new System.Drawing.Point(25, 28);
            this.txtLedgerCode.Name = "txtLedgerCode";
            this.txtLedgerCode.Size = new System.Drawing.Size(86, 20);
            this.txtLedgerCode.TabIndex = 2;
            this.txtLedgerCode.TextChanged += new System.EventHandler(this.txtLedgerCode_TextChanged);
            this.txtLedgerCode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtLedgerCode_KeyDown);
            this.txtLedgerCode.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtLedgerCode_KeyPress);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(22, 12);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Code:";
            // 
            // txtGroupName
            // 
            this.txtGroupName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtGroupName.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtGroupName.FocusLostColor = System.Drawing.Color.White;
            this.txtGroupName.Location = new System.Drawing.Point(317, 28);
            this.txtGroupName.Name = "txtGroupName";
            this.txtGroupName.Size = new System.Drawing.Size(142, 20);
            this.txtGroupName.TabIndex = 1;
            this.txtGroupName.TextChanged += new System.EventHandler(this.txtGroupID_TextChanged);
            this.txtGroupName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtGroupName_KeyDown);
            this.txtGroupName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtGroupName_KeyPress);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(314, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Under Group:";
            // 
            // txtLedgerName
            // 
            this.txtLedgerName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtLedgerName.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtLedgerName.FocusLostColor = System.Drawing.Color.White;
            this.txtLedgerName.Location = new System.Drawing.Point(146, 28);
            this.txtLedgerName.Name = "txtLedgerName";
            this.txtLedgerName.Size = new System.Drawing.Size(148, 20);
            this.txtLedgerName.TabIndex = 0;
            this.txtLedgerName.TextChanged += new System.EventHandler(this.txtLedgerName_TextChanged);
            this.txtLedgerName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtLedgerName_KeyDown);
            this.txtLedgerName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtLedgerName_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(143, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Name:";
            // 
            // btnCreate
            // 
            this.btnCreate.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCreate.Location = new System.Drawing.Point(382, 415);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(97, 23);
            this.btnCreate.TabIndex = 26;
            this.btnCreate.Text = "&Create New Ledger";
            this.btnCreate.UseVisualStyleBackColor = true;
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            // 
            // dgListOfLedger
            // 
            this.dgListOfLedger.AlternateBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.dgListOfLedger.AlternateForeColor = System.Drawing.Color.Black;
            this.dgListOfLedger.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgListOfLedger.DeleteQuestionMessage = "Are you sure to delete all the selected rows?";
            this.dgListOfLedger.EnableSort = false;
            this.dgListOfLedger.FixedRows = 1;
            this.dgListOfLedger.Location = new System.Drawing.Point(1, 4);
            this.dgListOfLedger.Name = "dgListOfLedger";
            this.dgListOfLedger.SelectionMode = SourceGrid.GridSelectionMode.Row;
            this.dgListOfLedger.Size = new System.Drawing.Size(486, 405);
            this.dgListOfLedger.TabIndex = 45;
            this.dgListOfLedger.TabStop = true;
            this.dgListOfLedger.ToolTipText = "";
            this.dgListOfLedger.Paint += new System.Windows.Forms.PaintEventHandler(this.dgListOfLedger_Paint);
            this.dgListOfLedger.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.dgListOfLedger_PreviewKeyDown);
            // 
            // frmLOVLedger
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(489, 516);
            this.Controls.Add(this.dgListOfLedger);
            this.Controls.Add(this.btnCreate);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "frmLOVLedger";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Ledger - List Of Values";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmLOVLedger_FormClosing);
            this.Load += new System.EventHandler(this.frmLOVLedger_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmLOVLedger_KeyDown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private STextBox txtLedgerCode;
        private System.Windows.Forms.Label label4;
        private STextBox txtGroupName;
        private System.Windows.Forms.Label label2;
        private STextBox txtLedgerName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkDisplayZeroBalance;
        private System.Windows.Forms.Button btnCreate;
        private SDataGrid dgListOfLedger;
    }
}