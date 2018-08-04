namespace Accounts
{
    partial class frmDueDate
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDueDate));
            this.grdDueDate = new SourceGrid.Grid();
            this.btnOk = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // grdDueDate
            // 
            this.grdDueDate.EnableSort = true;
            this.grdDueDate.Location = new System.Drawing.Point(5, 3);
            this.grdDueDate.Name = "grdDueDate";
            this.grdDueDate.OptimizeMode = SourceGrid.CellOptimizeMode.ForRows;
            this.grdDueDate.SelectionMode = SourceGrid.GridSelectionMode.Cell;
            this.grdDueDate.Size = new System.Drawing.Size(356, 180);
            this.grdDueDate.TabIndex = 0;
            this.grdDueDate.TabStop = true;
            this.grdDueDate.ToolTipText = "";
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(280, 189);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // frmDueDate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(367, 216);
            this.ControlBox = false;
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.grdDueDate);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmDueDate";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Debtors DueDate";
            this.Load += new System.EventHandler(this.frmDueDate_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private SourceGrid.Grid grdDueDate;
        private System.Windows.Forms.Button btnOk;
    }
}