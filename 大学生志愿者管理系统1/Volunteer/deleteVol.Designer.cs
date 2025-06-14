namespace 大学生志愿者管理系统1.Volunteer
{
    partial class deleteVol
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
            this.dgvvol = new System.Windows.Forms.DataGridView();
            this.btndelete = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvvol)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvvol
            // 
            this.dgvvol.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvvol.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.dgvvol.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvvol.Location = new System.Drawing.Point(2, 1);
            this.dgvvol.Name = "dgvvol";
            this.dgvvol.RowTemplate.Height = 23;
            this.dgvvol.Size = new System.Drawing.Size(1319, 511);
            this.dgvvol.TabIndex = 0;
            this.dgvvol.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvvol_CellContentClick);
            // 
            // btndelete
            // 
            this.btndelete.AutoSize = true;
            this.btndelete.Font = new System.Drawing.Font("华文中宋", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btndelete.Location = new System.Drawing.Point(522, 563);
            this.btndelete.Name = "btndelete";
            this.btndelete.Size = new System.Drawing.Size(75, 29);
            this.btndelete.TabIndex = 1;
            this.btndelete.Text = "删除";
            this.btndelete.UseVisualStyleBackColor = true;
            this.btndelete.Click += new System.EventHandler(this.btndelete_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.AutoSize = true;
            this.btnCancel.Font = new System.Drawing.Font("华文中宋", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnCancel.Location = new System.Drawing.Point(756, 563);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 29);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "返回";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // deleteVol
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.PeachPuff;
            this.ClientSize = new System.Drawing.Size(1324, 634);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btndelete);
            this.Controls.Add(this.dgvvol);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "deleteVol";
            this.Text = "deleteVol";
            this.Load += new System.EventHandler(this.deleteVol_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvvol)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvvol;
        private System.Windows.Forms.Button btndelete;
        private System.Windows.Forms.Button btnCancel;
    }
}