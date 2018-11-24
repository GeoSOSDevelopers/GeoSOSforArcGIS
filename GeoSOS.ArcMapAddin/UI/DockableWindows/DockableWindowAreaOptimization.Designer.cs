namespace GeoSOS.ArcMapAddIn
{
    partial class DockableWindowAreaOptimization
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DockableWindowAreaOptimization));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonSetACOParas = new System.Windows.Forms.Button();
            this.buttonSetOutput = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.buttonSetUtilityFunction = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.checkBoxUseDefault = new System.Windows.Forms.CheckBox();
            this.linkLabelPaper = new System.Windows.Forms.LinkLabel();
            this.label5 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.buttonSetACOParas);
            this.groupBox1.Controls.Add(this.buttonSetOutput);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.buttonSetUtilityFunction);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(10, 10);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(305, 242);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "面优化设置";
            // 
            // buttonSetACOParas
            // 
            this.buttonSetACOParas.Location = new System.Drawing.Point(218, 86);
            this.buttonSetACOParas.Name = "buttonSetACOParas";
            this.buttonSetACOParas.Size = new System.Drawing.Size(75, 28);
            this.buttonSetACOParas.TabIndex = 7;
            this.buttonSetACOParas.Text = "设置";
            this.buttonSetACOParas.UseVisualStyleBackColor = true;
            this.buttonSetACOParas.Click += new System.EventHandler(this.buttonSetACOParas_Click);
            // 
            // buttonSetOutput
            // 
            this.buttonSetOutput.Location = new System.Drawing.Point(218, 139);
            this.buttonSetOutput.Name = "buttonSetOutput";
            this.buttonSetOutput.Size = new System.Drawing.Size(75, 28);
            this.buttonSetOutput.TabIndex = 12;
            this.buttonSetOutput.Text = "设置";
            this.buttonSetOutput.UseVisualStyleBackColor = true;
            this.buttonSetOutput.Click += new System.EventHandler(this.buttonSetOutput_Click);
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(7, 194);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(278, 35);
            this.label9.TabIndex = 11;
            this.label9.Text = "完成上述步骤后可以点击工具条中的“开始”按钮运行优化过程。";
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(114, 146);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(82, 15);
            this.label8.TabIndex = 10;
            this.label8.Text = "输出设置.";
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(67, 93);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(142, 15);
            this.label7.TabIndex = 9;
            this.label7.Text = "蚁群算法参数设置.";
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(67, 40);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(112, 15);
            this.label6.TabIndex = 8;
            this.label6.Text = "设置效用函数.";
            // 
            // buttonSetUtilityFunction
            // 
            this.buttonSetUtilityFunction.Location = new System.Drawing.Point(218, 33);
            this.buttonSetUtilityFunction.Name = "buttonSetUtilityFunction";
            this.buttonSetUtilityFunction.Size = new System.Drawing.Size(75, 28);
            this.buttonSetUtilityFunction.TabIndex = 6;
            this.buttonSetUtilityFunction.Text = "设置";
            this.buttonSetUtilityFunction.UseVisualStyleBackColor = true;
            this.buttonSetUtilityFunction.Click += new System.EventHandler(this.buttonSetUtilityFunction_Click);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(7, 146);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(113, 15);
            this.label3.TabIndex = 4;
            this.label3.Text = "第三步(可选):";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(7, 93);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "第二步:";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(7, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "第一步:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.checkBoxUseDefault);
            this.groupBox2.Controls.Add(this.linkLabelPaper);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.pictureBox1);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Location = new System.Drawing.Point(10, 260);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(305, 530);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "面优化介绍";
            // 
            // checkBoxUseDefault
            // 
            this.checkBoxUseDefault.Location = new System.Drawing.Point(18, 465);
            this.checkBoxUseDefault.Name = "checkBoxUseDefault";
            this.checkBoxUseDefault.Size = new System.Drawing.Size(275, 63);
            this.checkBoxUseDefault.TabIndex = 4;
            this.checkBoxUseDefault.Text = "使用示例数据。(请打开插件安装目录下示例的Optimization_DongGuan\r\n.mxd)";
            this.checkBoxUseDefault.UseVisualStyleBackColor = true;
            this.checkBoxUseDefault.CheckedChanged += new System.EventHandler(this.checkBoxUseDefault_CheckedChanged);
            // 
            // linkLabelPaper
            // 
            this.linkLabelPaper.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.linkLabelPaper.LinkColor = System.Drawing.Color.Black;
            this.linkLabelPaper.Location = new System.Drawing.Point(15, 310);
            this.linkLabelPaper.Name = "linkLabelPaper";
            this.linkLabelPaper.Size = new System.Drawing.Size(278, 135);
            this.linkLabelPaper.TabIndex = 3;
            this.linkLabelPaper.TabStop = true;
            this.linkLabelPaper.Text = resources.GetString("linkLabelPaper.Text");
            this.linkLabelPaper.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelPaper_LinkClicked);
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(15, 289);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(172, 15);
            this.label5.TabIndex = 2;
            this.label5.Text = "参考论文(点击获取):";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.InitialImage = null;
            this.pictureBox1.Location = new System.Drawing.Point(33, 75);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(234, 177);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(15, 27);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(278, 34);
            this.label4.TabIndex = 0;
            this.label4.Text = "基于蚁群算法进行涉及空间及非空间多目标面状优化问题的求解。";
            // 
            // DockableWindowAreaOptimization
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "DockableWindowAreaOptimization";
            this.Size = new System.Drawing.Size(330, 800);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox checkBoxUseDefault;
        private System.Windows.Forms.LinkLabel linkLabelPaper;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button buttonSetACOParas;
        private System.Windows.Forms.Button buttonSetUtilityFunction;
        private System.Windows.Forms.Button buttonSetOutput;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
    }
}
