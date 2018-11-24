namespace GeoSOS.ArcMapAddIn.UI.Forms
{
    partial class FormACOOutput
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
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.radioButtonNotOutput = new System.Windows.Forms.RadioButton();
            this.radioButtonOutput = new System.Windows.Forms.RadioButton();
            this.label16 = new System.Windows.Forms.Label();
            this.buttonOutputFolder = new System.Windows.Forms.Button();
            this.textBoxOutputFolder = new System.Windows.Forms.TextBox();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.label14 = new System.Windows.Forms.Label();
            this.numericUpDownRefreshInvterval = new System.Windows.Forms.NumericUpDown();
            this.label15 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox9.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRefreshInvterval)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox9
            // 
            this.groupBox9.Controls.Add(this.radioButtonNotOutput);
            this.groupBox9.Controls.Add(this.radioButtonOutput);
            this.groupBox9.Controls.Add(this.label16);
            this.groupBox9.Controls.Add(this.buttonOutputFolder);
            this.groupBox9.Controls.Add(this.textBoxOutputFolder);
            this.groupBox9.Location = new System.Drawing.Point(12, 118);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(757, 144);
            this.groupBox9.TabIndex = 7;
            this.groupBox9.TabStop = false;
            this.groupBox9.Text = "优化过程结果输出";
            // 
            // radioButtonNotOutput
            // 
            this.radioButtonNotOutput.AutoSize = true;
            this.radioButtonNotOutput.Checked = true;
            this.radioButtonNotOutput.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.radioButtonNotOutput.Location = new System.Drawing.Point(18, 104);
            this.radioButtonNotOutput.Name = "radioButtonNotOutput";
            this.radioButtonNotOutput.Size = new System.Drawing.Size(178, 19);
            this.radioButtonNotOutput.TabIndex = 11;
            this.radioButtonNotOutput.TabStop = true;
            this.radioButtonNotOutput.Text = "优化过程中不输出结果";
            this.radioButtonNotOutput.UseVisualStyleBackColor = true;
            // 
            // radioButtonOutput
            // 
            this.radioButtonOutput.AutoSize = true;
            this.radioButtonOutput.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.radioButtonOutput.Location = new System.Drawing.Point(18, 27);
            this.radioButtonOutput.Name = "radioButtonOutput";
            this.radioButtonOutput.Size = new System.Drawing.Size(163, 19);
            this.radioButtonOutput.TabIndex = 10;
            this.radioButtonOutput.Text = "优化过程中输出结果";
            this.radioButtonOutput.UseVisualStyleBackColor = true;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label16.Location = new System.Drawing.Point(39, 65);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(82, 15);
            this.label16.TabIndex = 9;
            this.label16.Text = "输出位置：";
            // 
            // buttonOutputFolder
            // 
            this.buttonOutputFolder.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.buttonOutputFolder.Location = new System.Drawing.Point(644, 58);
            this.buttonOutputFolder.Name = "buttonOutputFolder";
            this.buttonOutputFolder.Size = new System.Drawing.Size(100, 28);
            this.buttonOutputFolder.TabIndex = 8;
            this.buttonOutputFolder.Text = "浏览(&R)...";
            this.buttonOutputFolder.UseVisualStyleBackColor = true;
            this.buttonOutputFolder.Click += new System.EventHandler(this.buttonOutputFolder_Click);
            // 
            // textBoxOutputFolder
            // 
            this.textBoxOutputFolder.Location = new System.Drawing.Point(127, 62);
            this.textBoxOutputFolder.Name = "textBoxOutputFolder";
            this.textBoxOutputFolder.ReadOnly = true;
            this.textBoxOutputFolder.Size = new System.Drawing.Size(491, 25);
            this.textBoxOutputFolder.TabIndex = 7;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(656, 280);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(100, 28);
            this.buttonCancel.TabIndex = 71;
            this.buttonCancel.Text = "取消";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(517, 280);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(100, 28);
            this.buttonOK.TabIndex = 70;
            this.buttonOK.Text = "确定";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label14.Location = new System.Drawing.Point(219, 46);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(262, 15);
            this.label14.TabIndex = 74;
            this.label14.Text = "次迭代在地图视图中刷新一次优化结果";
            // 
            // numericUpDownRefreshInvterval
            // 
            this.numericUpDownRefreshInvterval.Location = new System.Drawing.Point(75, 36);
            this.numericUpDownRefreshInvterval.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numericUpDownRefreshInvterval.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownRefreshInvterval.Name = "numericUpDownRefreshInvterval";
            this.numericUpDownRefreshInvterval.Size = new System.Drawing.Size(106, 25);
            this.numericUpDownRefreshInvterval.TabIndex = 73;
            this.numericUpDownRefreshInvterval.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label15.Location = new System.Drawing.Point(15, 46);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(37, 15);
            this.label15.TabIndex = 72;
            this.label15.Text = "每隔";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.label15);
            this.groupBox1.Controls.Add(this.numericUpDownRefreshInvterval);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(757, 89);
            this.groupBox1.TabIndex = 75;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "地图视图刷新设置";
            // 
            // FormACOOutput
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(781, 329);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.groupBox9);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormACOOutput";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "面优化输出设置";
            this.groupBox9.ResumeLayout(false);
            this.groupBox9.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRefreshInvterval)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox9;
        private System.Windows.Forms.RadioButton radioButtonNotOutput;
        private System.Windows.Forms.RadioButton radioButtonOutput;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Button buttonOutputFolder;
        private System.Windows.Forms.TextBox textBoxOutputFolder;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.NumericUpDown numericUpDownRefreshInvterval;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}