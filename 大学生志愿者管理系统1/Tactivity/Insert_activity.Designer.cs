
namespace 大学生志愿者管理系统1.Tactivity
{
    partial class Insert_activity
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtActName = new System.Windows.Forms.TextBox();
            this.cboCategory = new System.Windows.Forms.ComboBox();
            this.activityTypeTBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.vOLUNTEERDataSet = new 大学生志愿者管理系统1.VOLUNTEERDataSet();
            this.txtActID = new System.Windows.Forms.TextBox();
            this.txtActResume = new System.Windows.Forms.TextBox();
            this.txtActNeed = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.dtpBegin = new System.Windows.Forms.DateTimePicker();
            this.dtpStop = new System.Windows.Forms.DateTimePicker();
            this.btnOK = new System.Windows.Forms.Button();
            this.btncancel = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.dgvactivity = new System.Windows.Forms.DataGridView();
            this.activityTypeTTableAdapter = new 大学生志愿者管理系统1.VOLUNTEERDataSetTableAdapters.activityTypeTTableAdapter();
            this.label15 = new System.Windows.Forms.Label();
            this.txtplace = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.btnpid = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.activityTypeTBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.vOLUNTEERDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvactivity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(63, 118);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "活动名称：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(63, 186);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "活动类别：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(63, 50);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "活动编号：";
            // 
            // txtActName
            // 
            this.txtActName.Location = new System.Drawing.Point(199, 114);
            this.txtActName.Name = "txtActName";
            this.txtActName.Size = new System.Drawing.Size(109, 21);
            this.txtActName.TabIndex = 3;
            // 
            // cboCategory
            // 
            this.cboCategory.DataSource = this.activityTypeTBindingSource;
            this.cboCategory.DisplayMember = "Name";
            this.cboCategory.FormattingEnabled = true;
            this.cboCategory.Location = new System.Drawing.Point(199, 181);
            this.cboCategory.Name = "cboCategory";
            this.cboCategory.Size = new System.Drawing.Size(121, 20);
            this.cboCategory.TabIndex = 4;
            this.cboCategory.ValueMember = "Name";
            // 
            // activityTypeTBindingSource
            // 
            this.activityTypeTBindingSource.DataMember = "activityTypeT";
            this.activityTypeTBindingSource.DataSource = this.vOLUNTEERDataSet;
            // 
            // vOLUNTEERDataSet
            // 
            this.vOLUNTEERDataSet.DataSetName = "VOLUNTEERDataSet";
            this.vOLUNTEERDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // txtActID
            // 
            this.txtActID.Location = new System.Drawing.Point(199, 47);
            this.txtActID.Name = "txtActID";
            this.txtActID.Size = new System.Drawing.Size(109, 21);
            this.txtActID.TabIndex = 5;
            // 
            // txtActResume
            // 
            this.txtActResume.Location = new System.Drawing.Point(199, 515);
            this.txtActResume.Multiline = true;
            this.txtActResume.Name = "txtActResume";
            this.txtActResume.Size = new System.Drawing.Size(190, 40);
            this.txtActResume.TabIndex = 6;
            // 
            // txtActNeed
            // 
            this.txtActNeed.Location = new System.Drawing.Point(199, 381);
            this.txtActNeed.Name = "txtActNeed";
            this.txtActNeed.Size = new System.Drawing.Size(100, 21);
            this.txtActNeed.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(67, 526);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 8;
            this.label4.Text = "活动描述：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(67, 390);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 9;
            this.label5.Text = "所需人数：";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(63, 254);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 12);
            this.label6.TabIndex = 10;
            this.label6.Text = "开始时间：";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(63, 322);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 12);
            this.label7.TabIndex = 11;
            this.label7.Text = "结束时间：";
            // 
            // dtpBegin
            // 
            this.dtpBegin.Location = new System.Drawing.Point(199, 247);
            this.dtpBegin.Name = "dtpBegin";
            this.dtpBegin.Size = new System.Drawing.Size(200, 21);
            this.dtpBegin.TabIndex = 12;
            // 
            // dtpStop
            // 
            this.dtpStop.Location = new System.Drawing.Point(199, 314);
            this.dtpStop.Name = "dtpStop";
            this.dtpStop.Size = new System.Drawing.Size(200, 21);
            this.dtpStop.TabIndex = 13;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(87, 588);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 14;
            this.btnOK.Text = "添加";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btncancel
            // 
            this.btncancel.Location = new System.Drawing.Point(199, 588);
            this.btncancel.Name = "btncancel";
            this.btncancel.Size = new System.Drawing.Size(75, 23);
            this.btncancel.TabIndex = 15;
            this.btncancel.Text = "关闭";
            this.btncancel.UseVisualStyleBackColor = true;
            this.btncancel.Click += new System.EventHandler(this.btncancel_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label8.ForeColor = System.Drawing.Color.Red;
            this.label8.Location = new System.Drawing.Point(36, 42);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(21, 21);
            this.label8.TabIndex = 16;
            this.label8.Text = "*";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.ForeColor = System.Drawing.Color.Red;
            this.label9.Location = new System.Drawing.Point(50, 21);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(95, 12);
            this.label9.TabIndex = 17;
            this.label9.Text = "（带*为必填项）";
            this.label9.Click += new System.EventHandler(this.label9_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label10.ForeColor = System.Drawing.Color.Red;
            this.label10.Location = new System.Drawing.Point(36, 110);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(21, 21);
            this.label10.TabIndex = 18;
            this.label10.Text = "*";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label11.ForeColor = System.Drawing.Color.White;
            this.label11.Location = new System.Drawing.Point(36, 193);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(0, 21);
            this.label11.TabIndex = 19;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label12.ForeColor = System.Drawing.Color.Red;
            this.label12.Location = new System.Drawing.Point(36, 265);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(0, 21);
            this.label12.TabIndex = 20;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label13.ForeColor = System.Drawing.Color.Red;
            this.label13.Location = new System.Drawing.Point(36, 338);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(0, 21);
            this.label13.TabIndex = 21;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label14.ForeColor = System.Drawing.Color.Red;
            this.label14.Location = new System.Drawing.Point(36, 382);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(21, 21);
            this.label14.TabIndex = 22;
            this.label14.Text = "*";
            // 
            // dgvactivity
            // 
            this.dgvactivity.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgvactivity.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvactivity.Location = new System.Drawing.Point(444, 224);
            this.dgvactivity.Name = "dgvactivity";
            this.dgvactivity.RowTemplate.Height = 23;
            this.dgvactivity.Size = new System.Drawing.Size(654, 431);
            this.dgvactivity.TabIndex = 23;
            // 
            // activityTypeTTableAdapter
            // 
            this.activityTypeTTableAdapter.ClearBeforeFill = true;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(67, 458);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(65, 12);
            this.label15.TabIndex = 24;
            this.label15.Text = "活动地点：";
            // 
            // txtplace
            // 
            this.txtplace.Location = new System.Drawing.Point(199, 448);
            this.txtplace.Name = "txtplace";
            this.txtplace.Size = new System.Drawing.Size(100, 21);
            this.txtplace.TabIndex = 25;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label16.ForeColor = System.Drawing.Color.Red;
            this.label16.Location = new System.Drawing.Point(36, 450);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(21, 21);
            this.label16.TabIndex = 26;
            this.label16.Text = "*";
            // 
            // btnpid
            // 
            this.btnpid.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnpid.Location = new System.Drawing.Point(523, 114);
            this.btnpid.Name = "btnpid";
            this.btnpid.Size = new System.Drawing.Size(75, 23);
            this.btnpid.TabIndex = 27;
            this.btnpid.Text = "添加图片";
            this.btnpid.UseVisualStyleBackColor = true;
            this.btnpid.Click += new System.EventHandler(this.btnpid_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(633, 25);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(273, 176);
            this.pictureBox1.TabIndex = 28;
            this.pictureBox1.TabStop = false;
            // 
            // Insert_activity
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.PeachPuff;
            this.ClientSize = new System.Drawing.Size(1100, 655);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.btnpid);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.txtplace);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.dgvactivity);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.btncancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.dtpStop);
            this.Controls.Add(this.dtpBegin);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtActNeed);
            this.Controls.Add(this.txtActResume);
            this.Controls.Add(this.txtActID);
            this.Controls.Add(this.cboCategory);
            this.Controls.Add(this.txtActName);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "Insert_activity";
            this.Text = "Insert_activity";
            this.Load += new System.EventHandler(this.Insert_activity_Load);
            ((System.ComponentModel.ISupportInitialize)(this.activityTypeTBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.vOLUNTEERDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvactivity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtActName;
        private System.Windows.Forms.ComboBox cboCategory;
        private System.Windows.Forms.TextBox txtActID;
        private System.Windows.Forms.TextBox txtActResume;
        private System.Windows.Forms.TextBox txtActNeed;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.DateTimePicker dtpBegin;
        private System.Windows.Forms.DateTimePicker dtpStop;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btncancel;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.DataGridView dgvactivity;
        private VOLUNTEERDataSet vOLUNTEERDataSet;
        private System.Windows.Forms.BindingSource activityTypeTBindingSource;
        private VOLUNTEERDataSetTableAdapters.activityTypeTTableAdapter activityTypeTTableAdapter;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox txtplace;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Button btnpid;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}