namespace 大学生志愿者管理系统1.Identify
{
    partial class ExamineIdentify
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
            this.dgvIdentify = new System.Windows.Forms.DataGridView();
            this.btnOK = new System.Windows.Forms.Button();
            this.btncancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvIdentify)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvIdentify
            // 
            this.dgvIdentify.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvIdentify.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.dgvIdentify.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvIdentify.Location = new System.Drawing.Point(0, 1);
            this.dgvIdentify.Name = "dgvIdentify";
            this.dgvIdentify.RowTemplate.Height = 23;
            this.dgvIdentify.Size = new System.Drawing.Size(1301, 427);
            this.dgvIdentify.TabIndex = 0;
            this.dgvIdentify.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvIdentify_CellContentClick);
            // 
            // btnOK
            // 
            this.btnOK.AutoSize = true;
            this.btnOK.Font = new System.Drawing.Font("华文中宋", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnOK.Location = new System.Drawing.Point(491, 540);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(98, 33);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "审核";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btncancel
            // 
            this.btncancel.AutoSize = true;
            this.btncancel.Font = new System.Drawing.Font("华文中宋", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btncancel.Location = new System.Drawing.Point(716, 540);
            this.btncancel.Name = "btncancel";
            this.btncancel.Size = new System.Drawing.Size(98, 33);
            this.btncancel.TabIndex = 2;
            this.btncancel.Text = "返回";
            this.btncancel.UseVisualStyleBackColor = true;
            this.btncancel.Click += new System.EventHandler(this.btncancel_Click);
            // 
            // ExamineIdentify
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.PeachPuff;
            this.ClientSize = new System.Drawing.Size(1302, 633);
            this.Controls.Add(this.btncancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.dgvIdentify);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "ExamineIdentify";
            this.Text = "ExamineIdentify";
            this.Load += new System.EventHandler(this.ExamineIdentify_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvIdentify)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvIdentify;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btncancel;
    }
}