
namespace 大学生志愿者管理系统1.Tactivity
{
    partial class Update_activity
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
            this.dgvactivity = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.txtId = new System.Windows.Forms.TextBox();
            this.txtName = new System.Windows.Forms.TextBox();
            this.txtPlace = new System.Windows.Forms.TextBox();
            this.txtResume = new System.Windows.Forms.TextBox();
            this.cboCategory = new System.Windows.Forms.ComboBox();
            this.activityTypeTBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.vOLUNTEERDataSet1 = new 大学生志愿者管理系统1.VOLUNTEERDataSet1();
            this.dtpBegin = new System.Windows.Forms.DateTimePicker();
            this.dtpStop = new System.Windows.Forms.DateTimePicker();
            this.txtNeed = new System.Windows.Forms.TextBox();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnCancle = new System.Windows.Forms.Button();
            this.btnPic = new System.Windows.Forms.Button();
            this.pic_act = new System.Windows.Forms.PictureBox();
            this.activityTypeTTableAdapter = new 大学生志愿者管理系统1.VOLUNTEERDataSet1TableAdapters.activityTypeTTableAdapter();
            this.label9 = new System.Windows.Forms.Label();
            this.txtHolder = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvactivity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.activityTypeTBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.vOLUNTEERDataSet1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic_act)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvactivity
            // 
            this.dgvactivity.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgvactivity.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvactivity.Location = new System.Drawing.Point(-1, 1);
            this.dgvactivity.Name = "dgvactivity";
            this.dgvactivity.RowTemplate.Height = 23;
            this.dgvactivity.Size = new System.Drawing.Size(1096, 312);
            this.dgvactivity.TabIndex = 0;
            this.dgvactivity.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvactivity_CellContentClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(39, 357);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "活动编号：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(283, 357);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "活动名称：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(527, 357);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 3;
            this.label3.Text = "活动类别：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(41, 421);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 4;
            this.label4.Text = "开始时间：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(41, 485);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 5;
            this.label5.Text = "结束时间：";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(283, 421);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 12);
            this.label6.TabIndex = 6;
            this.label6.Text = "所需人数：";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(529, 421);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 12);
            this.label7.TabIndex = 7;
            this.label7.Text = "活动地点：";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(285, 485);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(65, 12);
            this.label8.TabIndex = 8;
            this.label8.Text = "活动描述：";
            // 
            // txtId
            // 
            this.txtId.Location = new System.Drawing.Point(110, 354);
            this.txtId.Name = "txtId";
            this.txtId.ReadOnly = true;
            this.txtId.Size = new System.Drawing.Size(100, 21);
            this.txtId.TabIndex = 9;
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(354, 354);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(100, 21);
            this.txtName.TabIndex = 10;
            // 
            // txtPlace
            // 
            this.txtPlace.Location = new System.Drawing.Point(600, 418);
            this.txtPlace.Name = "txtPlace";
            this.txtPlace.Size = new System.Drawing.Size(100, 21);
            this.txtPlace.TabIndex = 11;
            // 
            // txtResume
            // 
            this.txtResume.Location = new System.Drawing.Point(356, 482);
            this.txtResume.Name = "txtResume";
            this.txtResume.Size = new System.Drawing.Size(344, 21);
            this.txtResume.TabIndex = 12;
            // 
            // cboCategory
            // 
            this.cboCategory.DataSource = this.activityTypeTBindingSource;
            this.cboCategory.DisplayMember = "Name";
            this.cboCategory.FormattingEnabled = true;
            this.cboCategory.Location = new System.Drawing.Point(598, 354);
            this.cboCategory.Name = "cboCategory";
            this.cboCategory.Size = new System.Drawing.Size(102, 20);
            this.cboCategory.TabIndex = 13;
            this.cboCategory.ValueMember = "Name";
            // 
            // activityTypeTBindingSource
            // 
            this.activityTypeTBindingSource.DataMember = "activityTypeT";
            this.activityTypeTBindingSource.DataSource = this.vOLUNTEERDataSet1;
            // 
            // vOLUNTEERDataSet1
            // 
            this.vOLUNTEERDataSet1.DataSetName = "VOLUNTEERDataSet1";
            this.vOLUNTEERDataSet1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // dtpBegin
            // 
            this.dtpBegin.Location = new System.Drawing.Point(110, 415);
            this.dtpBegin.Name = "dtpBegin";
            this.dtpBegin.Size = new System.Drawing.Size(147, 21);
            this.dtpBegin.TabIndex = 14;
            // 
            // dtpStop
            // 
            this.dtpStop.Location = new System.Drawing.Point(110, 479);
            this.dtpStop.Name = "dtpStop";
            this.dtpStop.Size = new System.Drawing.Size(147, 21);
            this.dtpStop.TabIndex = 15;
            // 
            // txtNeed
            // 
            this.txtNeed.Location = new System.Drawing.Point(354, 418);
            this.txtNeed.Name = "txtNeed";
            this.txtNeed.Size = new System.Drawing.Size(100, 21);
            this.txtNeed.TabIndex = 16;
            // 
            // btnUpdate
            // 
            this.btnUpdate.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnUpdate.Location = new System.Drawing.Point(354, 540);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 40);
            this.btnUpdate.TabIndex = 17;
            this.btnUpdate.Text = "修改";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnCancle
            // 
            this.btnCancle.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnCancle.Location = new System.Drawing.Point(598, 540);
            this.btnCancle.Name = "btnCancle";
            this.btnCancle.Size = new System.Drawing.Size(75, 40);
            this.btnCancle.TabIndex = 18;
            this.btnCancle.Text = "取消";
            this.btnCancle.UseVisualStyleBackColor = true;
            this.btnCancle.Click += new System.EventHandler(this.btnCancle_Click);
            // 
            // btnPic
            // 
            this.btnPic.Location = new System.Drawing.Point(875, 519);
            this.btnPic.Name = "btnPic";
            this.btnPic.Size = new System.Drawing.Size(102, 30);
            this.btnPic.TabIndex = 19;
            this.btnPic.Text = "选择图片";
            this.btnPic.UseVisualStyleBackColor = true;
            this.btnPic.Click += new System.EventHandler(this.btnPic_Click);
            // 
            // pic_act
            // 
            this.pic_act.Location = new System.Drawing.Point(813, 324);
            this.pic_act.Name = "pic_act";
            this.pic_act.Size = new System.Drawing.Size(218, 179);
            this.pic_act.TabIndex = 20;
            this.pic_act.TabStop = false;
            // 
            // activityTypeTTableAdapter
            // 
            this.activityTypeTTableAdapter.ClearBeforeFill = true;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(41, 536);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(53, 12);
            this.label9.TabIndex = 21;
            this.label9.Text = "发起人：";
            // 
            // txtHolder
            // 
            this.txtHolder.Location = new System.Drawing.Point(110, 533);
            this.txtHolder.Name = "txtHolder";
            this.txtHolder.ReadOnly = true;
            this.txtHolder.Size = new System.Drawing.Size(100, 21);
            this.txtHolder.TabIndex = 22;
            // 
            // Update_activity
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.PeachPuff;
            this.ClientSize = new System.Drawing.Size(1095, 603);
            this.Controls.Add(this.txtHolder);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.pic_act);
            this.Controls.Add(this.btnPic);
            this.Controls.Add(this.btnCancle);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.txtNeed);
            this.Controls.Add(this.dtpStop);
            this.Controls.Add(this.dtpBegin);
            this.Controls.Add(this.cboCategory);
            this.Controls.Add(this.txtResume);
            this.Controls.Add(this.txtPlace);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.txtId);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dgvactivity);
            this.Name = "Update_activity";
            this.Text = "Update_activity";
            this.Load += new System.EventHandler(this.Update_activity_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvactivity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.activityTypeTBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.vOLUNTEERDataSet1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic_act)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvactivity;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtId;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.TextBox txtPlace;
        private System.Windows.Forms.TextBox txtResume;
        private System.Windows.Forms.ComboBox cboCategory;
        private System.Windows.Forms.DateTimePicker dtpBegin;
        private System.Windows.Forms.DateTimePicker dtpStop;
        private System.Windows.Forms.TextBox txtNeed;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnCancle;
        private System.Windows.Forms.Button btnPic;
        private System.Windows.Forms.PictureBox pic_act;
        private VOLUNTEERDataSet1 vOLUNTEERDataSet1;
        private System.Windows.Forms.BindingSource activityTypeTBindingSource;
        private VOLUNTEERDataSet1TableAdapters.activityTypeTTableAdapter activityTypeTTableAdapter;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtHolder;
    }
}