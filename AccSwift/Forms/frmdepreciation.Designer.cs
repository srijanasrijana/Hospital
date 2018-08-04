namespace AccSwift.Forms
{
    partial class frmdepreciation
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmdepreciation));
            this.grddepreciation = new SourceGrid.Grid();
            this.label1 = new System.Windows.Forms.Label();
            this.btnsavedepreciation = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // grddepreciation
            // 
            this.grddepreciation.Location = new System.Drawing.Point(12, 32);
            this.grddepreciation.Name = "grddepreciation";
            this.grddepreciation.OptimizeMode = SourceGrid.CellOptimizeMode.ForRows;
            this.grddepreciation.SelectionMode = SourceGrid.GridSelectionMode.Cell;
            this.grddepreciation.Size = new System.Drawing.Size(397, 257);
            this.grddepreciation.TabIndex = 154;
            this.grddepreciation.TabStop = true;
            this.grddepreciation.ToolTipText = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(182, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Depreciation For Fixed Assests\r\n";
            // 
            // btnsavedepreciation
            // 
            this.btnsavedepreciation.Location = new System.Drawing.Point(15, 297);
            this.btnsavedepreciation.Name = "btnsavedepreciation";
            this.btnsavedepreciation.Size = new System.Drawing.Size(125, 23);
            this.btnsavedepreciation.TabIndex = 155;
            this.btnsavedepreciation.Text = "Save Depreciation";
            this.btnsavedepreciation.UseVisualStyleBackColor = true;
            this.btnsavedepreciation.Click += new System.EventHandler(this.btnsavedepreciation_Click);
            // 
            // frmdepreciation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(430, 332);
            this.Controls.Add(this.btnsavedepreciation);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.grddepreciation);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmdepreciation";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Depreciation";
            this.Load += new System.EventHandler(this.frmdepreciation_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private SourceGrid.Grid grddepreciation;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnsavedepreciation;
    }
}