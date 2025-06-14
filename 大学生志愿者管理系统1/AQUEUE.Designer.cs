
namespace 大学生志愿者管理系统1
{
    partial class AQUEUE
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
            this.dgvAqueue = new System.Windows.Forms.DataGridView();
            this.btnClean = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnReturn = new System.Windows.Forms.Button();
            this.btnApply = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAqueue)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvAqueue
            // 
            this.dgvAqueue.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.dgvAqueue.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAqueue.Location = new System.Drawing.Point(36, 82);
            this.dgvAqueue.Name = "dgvAqueue";
            this.dgvAqueue.RowTemplate.Height = 23;
            this.dgvAqueue.Size = new System.Drawing.Size(814, 417);
            this.dgvAqueue.TabIndex = 0;
            this.dgvAqueue.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvAqueue_CellContentClick);
            // 
            // btnClean
            // 
            this.btnClean.Location = new System.Drawing.Point(914, 313);
            this.btnClean.Name = "btnClean";
            this.btnClean.Size = new System.Drawing.Size(75, 36);
            this.btnClean.TabIndex = 1;
            this.btnClean.Text = "清空队列";
            this.btnClean.UseVisualStyleBackColor = true;
            this.btnClean.Click += new System.EventHandler(this.btnClean_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(914, 214);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 35);
            this.btnDelete.TabIndex = 2;
            this.btnDelete.Text = "删除志愿";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnReturn
            // 
            this.btnReturn.Location = new System.Drawing.Point(914, 413);
            this.btnReturn.Name = "btnReturn";
            this.btnReturn.Size = new System.Drawing.Size(75, 35);
            this.btnReturn.TabIndex = 3;
            this.btnReturn.Text = "返回";
            this.btnReturn.UseVisualStyleBackColor = true;
            this.btnReturn.Click += new System.EventHandler(this.btnReturn_Click);
            // 
            // btnApply
            // 
            this.btnApply.Location = new System.Drawing.Point(914, 116);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(75, 34);
            this.btnApply.TabIndex = 4;
            this.btnApply.Text = "申请加入";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // AQUEUE
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.PeachPuff;
            this.ClientSize = new System.Drawing.Size(1024, 613);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.btnReturn);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnClean);
            this.Controls.Add(this.dgvAqueue);
            this.Name = "AQUEUE";
            this.Text = "AQUEUE";
            this.Load += new System.EventHandler(this.AQUEUE_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvAqueue)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvAqueue;
        private System.Windows.Forms.Button btnClean;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnReturn;
        private System.Windows.Forms.Button btnApply;
    }
}