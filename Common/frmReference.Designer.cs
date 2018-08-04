namespace Common
{
    partial class frmReference
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
            this.rbNewReference = new System.Windows.Forms.RadioButton();
            this.rbAgainst = new System.Windows.Forms.RadioButton();
            this.rbNone = new System.Windows.Forms.RadioButton();
            this.txtReferenceName = new System.Windows.Forms.TextBox();
            this.btnSaveNewRef = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.grpRefList = new System.Windows.Forms.GroupBox();
            this.grdRefList = new SourceGrid.Grid();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.grpNewRef = new System.Windows.Forms.GroupBox();
            this.btnSubmitNone = new System.Windows.Forms.Button();
            this.btnCancelNone = new System.Windows.Forms.Button();
            this.btnSubmitAgainst = new System.Windows.Forms.Button();
            this.btnCancelAgainst = new System.Windows.Forms.Button();
            this.grpRefList.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.grpNewRef.SuspendLayout();
            this.SuspendLayout();
            // 
            // rbNewReference
            // 
            this.rbNewReference.AutoSize = true;
            this.rbNewReference.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbNewReference.Location = new System.Drawing.Point(49, 14);
            this.rbNewReference.Name = "rbNewReference";
            this.rbNewReference.Size = new System.Drawing.Size(119, 20);
            this.rbNewReference.TabIndex = 1;
            this.rbNewReference.TabStop = true;
            this.rbNewReference.Text = "New Reference";
            this.rbNewReference.UseVisualStyleBackColor = true;
            this.rbNewReference.CheckedChanged += new System.EventHandler(this.rbNewReference_CheckedChanged);
            this.rbNewReference.KeyDown += new System.Windows.Forms.KeyEventHandler(this.rbNewReference_KeyDown);
            // 
            // rbAgainst
            // 
            this.rbAgainst.AutoSize = true;
            this.rbAgainst.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbAgainst.Location = new System.Drawing.Point(70, 12);
            this.rbAgainst.Name = "rbAgainst";
            this.rbAgainst.Size = new System.Drawing.Size(71, 20);
            this.rbAgainst.TabIndex = 5;
            this.rbAgainst.TabStop = true;
            this.rbAgainst.Text = "Against";
            this.rbAgainst.UseVisualStyleBackColor = true;
            this.rbAgainst.CheckedChanged += new System.EventHandler(this.rbAgainst_CheckedChanged);
            this.rbAgainst.KeyDown += new System.Windows.Forms.KeyEventHandler(this.rbAgainst_KeyDown);
            // 
            // rbNone
            // 
            this.rbNone.AutoSize = true;
            this.rbNone.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbNone.Location = new System.Drawing.Point(71, 13);
            this.rbNone.Name = "rbNone";
            this.rbNone.Size = new System.Drawing.Size(59, 20);
            this.rbNone.TabIndex = 0;
            this.rbNone.TabStop = true;
            this.rbNone.Text = "None";
            this.rbNone.UseVisualStyleBackColor = true;
            this.rbNone.CheckedChanged += new System.EventHandler(this.rbNone_CheckedChanged);
            this.rbNone.KeyDown += new System.Windows.Forms.KeyEventHandler(this.rbNone_KeyDown);
            // 
            // txtReferenceName
            // 
            this.txtReferenceName.Location = new System.Drawing.Point(19, 49);
            this.txtReferenceName.Multiline = true;
            this.txtReferenceName.Name = "txtReferenceName";
            this.txtReferenceName.Size = new System.Drawing.Size(222, 29);
            this.txtReferenceName.TabIndex = 2;
            this.txtReferenceName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtReferenceName_KeyDown);
            // 
            // btnSaveNewRef
            // 
            this.btnSaveNewRef.Location = new System.Drawing.Point(54, 91);
            this.btnSaveNewRef.Name = "btnSaveNewRef";
            this.btnSaveNewRef.Size = new System.Drawing.Size(75, 30);
            this.btnSaveNewRef.TabIndex = 3;
            this.btnSaveNewRef.Text = "Save";
            this.btnSaveNewRef.UseVisualStyleBackColor = true;
            this.btnSaveNewRef.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(135, 91);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 30);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(79, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(114, 16);
            this.label1.TabIndex = 4;
            this.label1.Text = "Reference Name ";
            // 
            // grpRefList
            // 
            this.grpRefList.Controls.Add(this.btnCancelAgainst);
            this.grpRefList.Controls.Add(this.grdRefList);
            this.grpRefList.Controls.Add(this.btnSubmitAgainst);
            this.grpRefList.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpRefList.Location = new System.Drawing.Point(224, 4);
            this.grpRefList.Name = "grpRefList";
            this.grpRefList.Size = new System.Drawing.Size(285, 215);
            this.grpRefList.TabIndex = 6;
            this.grpRefList.TabStop = false;
            // 
            // grdRefList
            // 
            this.grdRefList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdRefList.EnableSort = true;
            this.grdRefList.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grdRefList.Location = new System.Drawing.Point(4, 14);
            this.grdRefList.Name = "grdRefList";
            this.grdRefList.OptimizeMode = SourceGrid.CellOptimizeMode.ForRows;
            this.grdRefList.SelectionMode = SourceGrid.GridSelectionMode.Row;
            this.grdRefList.Size = new System.Drawing.Size(277, 161);
            this.grdRefList.TabIndex = 0;
            this.grdRefList.TabStop = true;
            this.grdRefList.ToolTipText = "";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbNone);
            this.groupBox1.Location = new System.Drawing.Point(8, 27);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(208, 38);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rbNewReference);
            this.groupBox2.Location = new System.Drawing.Point(9, 67);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(208, 38);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.rbAgainst);
            this.groupBox3.Location = new System.Drawing.Point(9, 107);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(208, 38);
            this.groupBox3.TabIndex = 9;
            this.groupBox3.TabStop = false;
            // 
            // grpNewRef
            // 
            this.grpNewRef.Controls.Add(this.btnCancel);
            this.grpNewRef.Controls.Add(this.txtReferenceName);
            this.grpNewRef.Controls.Add(this.btnSaveNewRef);
            this.grpNewRef.Controls.Add(this.label1);
            this.grpNewRef.Location = new System.Drawing.Point(240, 17);
            this.grpNewRef.Name = "grpNewRef";
            this.grpNewRef.Size = new System.Drawing.Size(259, 137);
            this.grpNewRef.TabIndex = 10;
            this.grpNewRef.TabStop = false;
            // 
            // btnSubmitNone
            // 
            this.btnSubmitNone.Location = new System.Drawing.Point(32, 171);
            this.btnSubmitNone.Name = "btnSubmitNone";
            this.btnSubmitNone.Size = new System.Drawing.Size(75, 30);
            this.btnSubmitNone.TabIndex = 3;
            this.btnSubmitNone.Text = "Submit";
            this.btnSubmitNone.UseVisualStyleBackColor = true;
            this.btnSubmitNone.Click += new System.EventHandler(this.btnSubmitNone_Click);
            // 
            // btnCancelNone
            // 
            this.btnCancelNone.Location = new System.Drawing.Point(113, 171);
            this.btnCancelNone.Name = "btnCancelNone";
            this.btnCancelNone.Size = new System.Drawing.Size(75, 30);
            this.btnCancelNone.TabIndex = 4;
            this.btnCancelNone.Text = "Cancel";
            this.btnCancelNone.UseVisualStyleBackColor = true;
            this.btnCancelNone.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSubmitAgainst
            // 
            this.btnSubmitAgainst.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSubmitAgainst.Location = new System.Drawing.Point(70, 179);
            this.btnSubmitAgainst.Name = "btnSubmitAgainst";
            this.btnSubmitAgainst.Size = new System.Drawing.Size(75, 30);
            this.btnSubmitAgainst.TabIndex = 3;
            this.btnSubmitAgainst.Text = "Submit";
            this.btnSubmitAgainst.UseVisualStyleBackColor = true;
            this.btnSubmitAgainst.Click += new System.EventHandler(this.btnSubmitAgainst_Click);
            // 
            // btnCancelAgainst
            // 
            this.btnCancelAgainst.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancelAgainst.Location = new System.Drawing.Point(151, 179);
            this.btnCancelAgainst.Name = "btnCancelAgainst";
            this.btnCancelAgainst.Size = new System.Drawing.Size(75, 30);
            this.btnCancelAgainst.TabIndex = 4;
            this.btnCancelAgainst.Text = "Cancel";
            this.btnCancelAgainst.UseVisualStyleBackColor = true;
            this.btnCancelAgainst.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // frmReference
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.ClientSize = new System.Drawing.Size(514, 231);
            this.ControlBox = false;
            this.Controls.Add(this.btnCancelNone);
            this.Controls.Add(this.grpNewRef);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.btnSubmitNone);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.grpRefList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Name = "frmReference";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "frmReference";
            this.Load += new System.EventHandler(this.frmReference_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmReference_KeyDown);
            this.grpRefList.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.grpNewRef.ResumeLayout(false);
            this.grpNewRef.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RadioButton rbNewReference;
        private System.Windows.Forms.RadioButton rbAgainst;
        private System.Windows.Forms.RadioButton rbNone;
        private System.Windows.Forms.TextBox txtReferenceName;
        private System.Windows.Forms.Button btnSaveNewRef;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox grpRefList;
        private SourceGrid.Grid grdRefList;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox grpNewRef;
        private System.Windows.Forms.Button btnSubmitNone;
        private System.Windows.Forms.Button btnCancelNone;
        private System.Windows.Forms.Button btnCancelAgainst;
        private System.Windows.Forms.Button btnSubmitAgainst;

    }
}