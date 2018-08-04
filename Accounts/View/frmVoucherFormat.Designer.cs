using SComponents;
namespace Accounts
{
    partial class frmVoucherFormat
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmVoucherFormat));
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnDown = new System.Windows.Forms.Button();
            this.btnUp = new System.Windows.Forms.Button();
            this.lvNum = new System.Windows.Forms.ListView();
            this.Sno = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Particulars = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Values = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lblSample = new System.Windows.Forms.Label();
            this.btnDate = new System.Windows.Forms.Button();
            this.btnSymbol = new System.Windows.Forms.Button();
            this.btnAutoNum = new System.Windows.Forms.Button();
            this.label13 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.txtSymbol = new System.Windows.Forms.TextBox();
            this.lblAutoNum = new System.Windows.Forms.Label();
            this.cboDate = new SComboBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnDelete
            // 
            this.btnDelete.Image = ((System.Drawing.Image)(resources.GetObject("btnDelete.Image")));
            this.btnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDelete.Location = new System.Drawing.Point(764, 39);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(74, 23);
            this.btnDelete.TabIndex = 63;
            this.btnDelete.Text = "&Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnSave
            // 
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSave.Location = new System.Drawing.Point(418, 285);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 61;
            this.btnSave.Text = "&Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnDown
            // 
            this.btnDown.Location = new System.Drawing.Point(764, 99);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(74, 23);
            this.btnDown.TabIndex = 57;
            this.btnDown.Text = "Down";
            this.btnDown.UseVisualStyleBackColor = true;
            this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
            // 
            // btnUp
            // 
            this.btnUp.Location = new System.Drawing.Point(764, 70);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(74, 23);
            this.btnUp.TabIndex = 56;
            this.btnUp.Text = "Up";
            this.btnUp.UseVisualStyleBackColor = true;
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // lvNum
            // 
            this.lvNum.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Sno,
            this.Particulars,
            this.Values});
            this.lvNum.FullRowSelect = true;
            this.lvNum.GridLines = true;
            this.lvNum.Location = new System.Drawing.Point(418, 39);
            this.lvNum.Name = "lvNum";
            this.lvNum.Size = new System.Drawing.Size(328, 189);
            this.lvNum.TabIndex = 55;
            this.lvNum.UseCompatibleStateImageBehavior = false;
            this.lvNum.View = System.Windows.Forms.View.Details;
            // 
            // Sno
            // 
            this.Sno.Text = "S.No.";
            this.Sno.Width = 40;
            // 
            // Particulars
            // 
            this.Particulars.Text = "Particulars";
            this.Particulars.Width = 100;
            // 
            // Values
            // 
            this.Values.Text = "Values";
            this.Values.Width = 185;
            // 
            // lblSample
            // 
            this.lblSample.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblSample.Location = new System.Drawing.Point(418, 245);
            this.lblSample.Name = "lblSample";
            this.lblSample.Size = new System.Drawing.Size(328, 28);
            this.lblSample.TabIndex = 53;
            // 
            // btnDate
            // 
            this.btnDate.Location = new System.Drawing.Point(354, 147);
            this.btnDate.Name = "btnDate";
            this.btnDate.Size = new System.Drawing.Size(32, 21);
            this.btnDate.TabIndex = 52;
            this.btnDate.Text = "=>";
            this.btnDate.UseVisualStyleBackColor = true;
            this.btnDate.Click += new System.EventHandler(this.btnDate_Click);
            // 
            // btnSymbol
            // 
            this.btnSymbol.Location = new System.Drawing.Point(354, 114);
            this.btnSymbol.Name = "btnSymbol";
            this.btnSymbol.Size = new System.Drawing.Size(32, 21);
            this.btnSymbol.TabIndex = 51;
            this.btnSymbol.Text = "=>";
            this.btnSymbol.UseVisualStyleBackColor = true;
            this.btnSymbol.Click += new System.EventHandler(this.btnSymbol_Click);
            // 
            // btnAutoNum
            // 
            this.btnAutoNum.Location = new System.Drawing.Point(354, 62);
            this.btnAutoNum.Name = "btnAutoNum";
            this.btnAutoNum.Size = new System.Drawing.Size(32, 21);
            this.btnAutoNum.TabIndex = 50;
            this.btnAutoNum.Text = "=>";
            this.btnAutoNum.UseVisualStyleBackColor = true;
            this.btnAutoNum.Click += new System.EventHandler(this.btnAutoNum_Click);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(9, 147);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(104, 13);
            this.label13.TabIndex = 49;
            this.label13.Text = "Choose Date Format";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(9, 121);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(103, 13);
            this.label15.TabIndex = 47;
            this.label15.Text = "Symbol/Prefix/Suffix";
            // 
            // txtSymbol
            // 
            this.txtSymbol.Location = new System.Drawing.Point(130, 114);
            this.txtSymbol.Name = "txtSymbol";
            this.txtSymbol.Size = new System.Drawing.Size(206, 20);
            this.txtSymbol.TabIndex = 45;
            this.txtSymbol.TextChanged += new System.EventHandler(this.txtSymbol_TextChanged);
            // 
            // lblAutoNum
            // 
            this.lblAutoNum.AutoSize = true;
            this.lblAutoNum.Location = new System.Drawing.Point(270, 66);
            this.lblAutoNum.Name = "lblAutoNum";
            this.lblAutoNum.Size = new System.Drawing.Size(66, 13);
            this.lblAutoNum.TabIndex = 65;
            this.lblAutoNum.Text = "AutoNumber";
            // 
            // cboDate
            // 
            this.cboDate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDate.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.cboDate.FocusLostColor = System.Drawing.Color.White;
            this.cboDate.FormattingEnabled = true;
            this.cboDate.Location = new System.Drawing.Point(130, 144);
            this.cboDate.Name = "cboDate";
            this.cboDate.Size = new System.Drawing.Size(206, 21);
            this.cboDate.TabIndex = 64;
            // 
            // btnCancel
            // 
            this.btnCancel.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Image = global::Accounts.Properties.Resources.gnome_window_close;
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(520, 285);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(82, 23);
            this.btnCancel.TabIndex = 66;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // frmVoucherFormat
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(913, 371);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lblAutoNum);
            this.Controls.Add(this.cboDate);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnDown);
            this.Controls.Add(this.btnUp);
            this.Controls.Add(this.lvNum);
            this.Controls.Add(this.lblSample);
            this.Controls.Add(this.btnDate);
            this.Controls.Add(this.btnSymbol);
            this.Controls.Add(this.btnAutoNum);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.txtSymbol);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "frmVoucherFormat";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Numbering Format";
            this.Load += new System.EventHandler(this.frmVoucherFormat_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmVoucherFormat_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnDown;
        private System.Windows.Forms.Button btnUp;
        private System.Windows.Forms.ListView lvNum;
        private System.Windows.Forms.ColumnHeader Sno;
        private System.Windows.Forms.ColumnHeader Particulars;
        private System.Windows.Forms.ColumnHeader Values;
        private System.Windows.Forms.Label lblSample;
        private System.Windows.Forms.Button btnDate;
        private System.Windows.Forms.Button btnSymbol;
        private System.Windows.Forms.Button btnAutoNum;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox txtSymbol;
        private SComboBox cboDate;
        private System.Windows.Forms.Label lblAutoNum;
        private System.Windows.Forms.Button btnCancel;
    }
}