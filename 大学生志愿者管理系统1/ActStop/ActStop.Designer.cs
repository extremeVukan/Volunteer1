namespace 大学生志愿者管理系统1.ActStop
{
    partial class ActStop
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
            this.dgvactivity = new System.Windows.Forms.DataGridView();
            this.活动ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.活动名称 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewButtonColumn();
            this.dgvShowVol = new System.Windows.Forms.DataGridView();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtVolTime = new System.Windows.Forms.TextBox();
            this.btnAddtime = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvactivity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvShowVol)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvactivity
            // 
            this.dgvactivity.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvactivity.BackgroundColor = System.Drawing.Color.White;
            this.dgvactivity.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvactivity.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.活动ID,
            this.活动名称,
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column5,
            this.Column4});
            this.dgvactivity.GridColor = System.Drawing.SystemColors.Control;
            this.dgvactivity.Location = new System.Drawing.Point(1, 0);
            this.dgvactivity.Name = "dgvactivity";
            this.dgvactivity.RowTemplate.Height = 23;
            this.dgvactivity.Size = new System.Drawing.Size(914, 548);
            this.dgvactivity.TabIndex = 0;
            this.dgvactivity.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvactivity_CellContentClick);
            // 
            // 活动ID
            // 
            this.活动ID.HeaderText = "活动ID";
            this.活动ID.Name = "活动ID";
            // 
            // 活动名称
            // 
            this.活动名称.HeaderText = "活动名称";
            this.活动名称.Name = "活动名称";
            // 
            // Column1
            // 
            this.Column1.HeaderText = "活动类型";
            this.Column1.Name = "Column1";
            // 
            // Column2
            // 
            this.Column2.HeaderText = "开始时间";
            this.Column2.Name = "Column2";
            // 
            // Column3
            // 
            this.Column3.HeaderText = "结束时间";
            this.Column3.Name = "Column3";
            // 
            // Column5
            // 
            this.Column5.HeaderText = "状态";
            this.Column5.Name = "Column5";
            // 
            // Column4
            // 
            this.Column4.HeaderText = "活动结束";
            this.Column4.Name = "Column4";
            this.Column4.Text = "结束";
            this.Column4.UseColumnTextForButtonValue = true;
            // 
            // dgvShowVol
            // 
            this.dgvShowVol.BackgroundColor = System.Drawing.Color.White;
            this.dgvShowVol.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvShowVol.Location = new System.Drawing.Point(921, 0);
            this.dgvShowVol.Name = "dgvShowVol";
            this.dgvShowVol.RowTemplate.Height = 23;
            this.dgvShowVol.Size = new System.Drawing.Size(406, 427);
            this.dgvShowVol.TabIndex = 1;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(1176, 517);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "返回";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(1063, 474);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "志愿时长：";
            // 
            // txtVolTime
            // 
            this.txtVolTime.BackColor = System.Drawing.SystemColors.Control;
            this.txtVolTime.Location = new System.Drawing.Point(1167, 471);
            this.txtVolTime.Name = "txtVolTime";
            this.txtVolTime.Size = new System.Drawing.Size(100, 21);
            this.txtVolTime.TabIndex = 5;
            // 
            // btnAddtime
            // 
            this.btnAddtime.Location = new System.Drawing.Point(1065, 517);
            this.btnAddtime.Name = "btnAddtime";
            this.btnAddtime.Size = new System.Drawing.Size(75, 23);
            this.btnAddtime.TabIndex = 2;
            this.btnAddtime.Text = "添加时长";
            this.btnAddtime.UseVisualStyleBackColor = true;
            this.btnAddtime.Click += new System.EventHandler(this.btnAddtime_Click);
            // 
            // ActStop
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.PeachPuff;
            this.ClientSize = new System.Drawing.Size(1328, 552);
            this.Controls.Add(this.txtVolTime);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnAddtime);
            this.Controls.Add(this.dgvShowVol);
            this.Controls.Add(this.dgvactivity);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "ActStop";
            this.Text = "ActStop";
            this.Load += new System.EventHandler(this.ActStop_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvactivity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvShowVol)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvactivity;
        private System.Windows.Forms.DataGridView dgvShowVol;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtVolTime;
        private System.Windows.Forms.Button btnAddtime;
        private System.Windows.Forms.DataGridViewTextBoxColumn 活动ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn 活动名称;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewButtonColumn Column4;
    }
}