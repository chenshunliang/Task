namespace FilesReadTask
{
    partial class ReadResumeToMongoDB
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReadResumeToMongoDB));
            this.btnStart = new System.Windows.Forms.Button();
            this.proBar = new System.Windows.Forms.ProgressBar();
            this.label1 = new System.Windows.Forms.Label();
            this.labNum = new System.Windows.Forms.Label();
            this.labInfo = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(397, 49);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "开始导入";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // proBar
            // 
            this.proBar.Location = new System.Drawing.Point(35, 49);
            this.proBar.Name = "proBar";
            this.proBar.Size = new System.Drawing.Size(345, 23);
            this.proBar.Step = 1;
            this.proBar.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(35, 92);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "文件数量";
            // 
            // labNum
            // 
            this.labNum.AutoSize = true;
            this.labNum.Location = new System.Drawing.Point(125, 92);
            this.labNum.Name = "labNum";
            this.labNum.Size = new System.Drawing.Size(23, 12);
            this.labNum.TabIndex = 3;
            this.labNum.Text = "0/0";
            // 
            // labInfo
            // 
            this.labInfo.AutoSize = true;
            this.labInfo.Location = new System.Drawing.Point(280, 92);
            this.labInfo.Name = "labInfo";
            this.labInfo.Size = new System.Drawing.Size(53, 12);
            this.labInfo.TabIndex = 4;
            this.labInfo.Text = "准备导入";
            // 
            // ReadResumeToMongoDB
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 129);
            this.Controls.Add(this.labInfo);
            this.Controls.Add(this.labNum);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.proBar);
            this.Controls.Add(this.btnStart);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "ReadResumeToMongoDB";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "导入简历到数据库";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ReadResumeToMongoDB_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.ProgressBar proBar;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labNum;
        private System.Windows.Forms.Label labInfo;
    }
}