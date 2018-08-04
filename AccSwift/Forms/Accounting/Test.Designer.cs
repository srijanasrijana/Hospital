namespace Inventory.Forms.Accounting
{
    partial class Test
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
            this.button1 = new System.Windows.Forms.Button();
            this.grdJournal = new SourceGrid.Grid();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(323, 239);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // grdJournal
            // 
            this.grdJournal.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grdJournal.Location = new System.Drawing.Point(12, 68);
            this.grdJournal.Name = "grdJournal";
            this.grdJournal.OptimizeMode = SourceGrid.CellOptimizeMode.ForRows;
            this.grdJournal.SelectionMode = SourceGrid.GridSelectionMode.Cell;
            this.grdJournal.Size = new System.Drawing.Size(453, 165);
            this.grdJournal.TabIndex = 1;
            this.grdJournal.TabStop = true;
            this.grdJournal.ToolTipText = "";
            // 
            // Test
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(644, 416);
            this.Controls.Add(this.grdJournal);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Test";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Test";
            this.Load += new System.EventHandler(this.Test_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private SourceGrid.Grid grdJournal;
    }
}