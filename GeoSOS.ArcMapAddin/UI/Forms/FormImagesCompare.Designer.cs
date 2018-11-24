namespace GeoSOS.ArcMapAddIn.UI.Forms
{
    partial class FormImagesCompare
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonPercent = new System.Windows.Forms.Button();
            this.dataGridViewConfusionMatrix = new System.Windows.Forms.DataGridView();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.labelSimulationData = new System.Windows.Forms.Label();
            this.comboBoxSimulation = new System.Windows.Forms.ComboBox();
            this.labelEndTime = new System.Windows.Forms.Label();
            this.comboBoxLayerEnd = new System.Windows.Forms.ComboBox();
            this.radioButtonThreeImages = new System.Windows.Forms.RadioButton();
            this.radioButtonActualSimualtion = new System.Windows.Forms.RadioButton();
            this.radioButtonTwoImages = new System.Windows.Forms.RadioButton();
            this.labelStartTime = new System.Windows.Forms.Label();
            this.comboBoxLayerStart = new System.Windows.Forms.ComboBox();
            this.labelDescription = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonCompare = new System.Windows.Forms.Button();
            this.dataGridViewLandUse = new System.Windows.Forms.DataGridView();
            this.ColumnLUDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.landuseColumnIsUrban = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.textBoxResult = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewConfusionMatrix)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewLandUse)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.groupBox1.Controls.Add(this.buttonPercent);
            this.groupBox1.Controls.Add(this.dataGridViewConfusionMatrix);
            this.groupBox1.Location = new System.Drawing.Point(5, 321);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(717, 290);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "混淆矩阵";
            // 
            // buttonPercent
            // 
            this.buttonPercent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonPercent.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.buttonPercent.Location = new System.Drawing.Point(556, 255);
            this.buttonPercent.Name = "buttonPercent";
            this.buttonPercent.Size = new System.Drawing.Size(100, 26);
            this.buttonPercent.TabIndex = 13;
            this.buttonPercent.Text = "百分比数值";
            this.buttonPercent.UseVisualStyleBackColor = true;
            this.buttonPercent.Click += new System.EventHandler(this.buttonPercent_Click);
            // 
            // dataGridViewConfusionMatrix
            // 
            this.dataGridViewConfusionMatrix.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewConfusionMatrix.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewConfusionMatrix.Location = new System.Drawing.Point(15, 20);
            this.dataGridViewConfusionMatrix.Name = "dataGridViewConfusionMatrix";
            this.dataGridViewConfusionMatrix.ReadOnly = true;
            this.dataGridViewConfusionMatrix.RowTemplate.Height = 23;
            this.dataGridViewConfusionMatrix.Size = new System.Drawing.Size(696, 229);
            this.dataGridViewConfusionMatrix.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.labelSimulationData);
            this.groupBox2.Controls.Add(this.comboBoxSimulation);
            this.groupBox2.Controls.Add(this.labelEndTime);
            this.groupBox2.Controls.Add(this.comboBoxLayerEnd);
            this.groupBox2.Controls.Add(this.radioButtonThreeImages);
            this.groupBox2.Controls.Add(this.radioButtonActualSimualtion);
            this.groupBox2.Controls.Add(this.radioButtonTwoImages);
            this.groupBox2.Controls.Add(this.labelStartTime);
            this.groupBox2.Controls.Add(this.comboBoxLayerStart);
            this.groupBox2.Controls.Add(this.labelDescription);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.buttonCompare);
            this.groupBox2.Controls.Add(this.dataGridViewLandUse);
            this.groupBox2.Location = new System.Drawing.Point(5, 4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(717, 312);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "对比设置";
            // 
            // labelSimulationData
            // 
            this.labelSimulationData.AutoSize = true;
            this.labelSimulationData.Location = new System.Drawing.Point(31, 219);
            this.labelSimulationData.Name = "labelSimulationData";
            this.labelSimulationData.Size = new System.Drawing.Size(89, 12);
            this.labelSimulationData.TabIndex = 17;
            this.labelSimulationData.Text = "模拟结果数据：";
            this.labelSimulationData.Visible = false;
            // 
            // comboBoxSimulation
            // 
            this.comboBoxSimulation.FormattingEnabled = true;
            this.comboBoxSimulation.Location = new System.Drawing.Point(31, 238);
            this.comboBoxSimulation.Name = "comboBoxSimulation";
            this.comboBoxSimulation.Size = new System.Drawing.Size(265, 20);
            this.comboBoxSimulation.TabIndex = 16;
            this.comboBoxSimulation.Visible = false;
            // 
            // labelEndTime
            // 
            this.labelEndTime.AutoSize = true;
            this.labelEndTime.Location = new System.Drawing.Point(31, 167);
            this.labelEndTime.Name = "labelEndTime";
            this.labelEndTime.Size = new System.Drawing.Size(161, 12);
            this.labelEndTime.TabIndex = 15;
            this.labelEndTime.Text = "终止时期土地利用分类数据：";
            // 
            // comboBoxLayerEnd
            // 
            this.comboBoxLayerEnd.FormattingEnabled = true;
            this.comboBoxLayerEnd.Location = new System.Drawing.Point(31, 186);
            this.comboBoxLayerEnd.Name = "comboBoxLayerEnd";
            this.comboBoxLayerEnd.Size = new System.Drawing.Size(265, 20);
            this.comboBoxLayerEnd.TabIndex = 14;
            // 
            // radioButtonThreeImages
            // 
            this.radioButtonThreeImages.AutoSize = true;
            this.radioButtonThreeImages.Location = new System.Drawing.Point(427, 46);
            this.radioButtonThreeImages.Name = "radioButtonThreeImages";
            this.radioButtonThreeImages.Size = new System.Drawing.Size(263, 16);
            this.radioButtonThreeImages.TabIndex = 13;
            this.radioButtonThreeImages.Text = "模拟起始、终止时段数据、模拟结果三者对比";
            this.radioButtonThreeImages.UseVisualStyleBackColor = true;
            this.radioButtonThreeImages.CheckedChanged += new System.EventHandler(this.radioButtonThreeImages_CheckedChanged);
            // 
            // radioButtonActualSimualtion
            // 
            this.radioButtonActualSimualtion.AutoSize = true;
            this.radioButtonActualSimualtion.Location = new System.Drawing.Point(231, 46);
            this.radioButtonActualSimualtion.Name = "radioButtonActualSimualtion";
            this.radioButtonActualSimualtion.Size = new System.Drawing.Size(155, 16);
            this.radioButtonActualSimualtion.TabIndex = 12;
            this.radioButtonActualSimualtion.Text = "模拟结果与真实数据对比";
            this.radioButtonActualSimualtion.UseVisualStyleBackColor = true;
            this.radioButtonActualSimualtion.CheckedChanged += new System.EventHandler(this.radioButtonActualSimualtion_CheckedChanged);
            // 
            // radioButtonTwoImages
            // 
            this.radioButtonTwoImages.AutoSize = true;
            this.radioButtonTwoImages.Checked = true;
            this.radioButtonTwoImages.Location = new System.Drawing.Point(30, 46);
            this.radioButtonTwoImages.Name = "radioButtonTwoImages";
            this.radioButtonTwoImages.Size = new System.Drawing.Size(167, 16);
            this.radioButtonTwoImages.TabIndex = 11;
            this.radioButtonTwoImages.TabStop = true;
            this.radioButtonTwoImages.Text = "两幅土地利用分类数据对比";
            this.radioButtonTwoImages.UseVisualStyleBackColor = true;
            this.radioButtonTwoImages.CheckedChanged += new System.EventHandler(this.radioButtonTwoImages_CheckedChanged);
            // 
            // labelStartTime
            // 
            this.labelStartTime.AutoSize = true;
            this.labelStartTime.Location = new System.Drawing.Point(30, 115);
            this.labelStartTime.Name = "labelStartTime";
            this.labelStartTime.Size = new System.Drawing.Size(161, 12);
            this.labelStartTime.TabIndex = 10;
            this.labelStartTime.Text = "起始时期土地利用分类数据：";
            // 
            // comboBoxLayerStart
            // 
            this.comboBoxLayerStart.FormattingEnabled = true;
            this.comboBoxLayerStart.Location = new System.Drawing.Point(31, 134);
            this.comboBoxLayerStart.Name = "comboBoxLayerStart";
            this.comboBoxLayerStart.Size = new System.Drawing.Size(265, 20);
            this.comboBoxLayerStart.TabIndex = 9;
            this.comboBoxLayerStart.SelectedIndexChanged += new System.EventHandler(this.comboBoxLayerStart_SelectedIndexChanged);
            // 
            // labelDescription
            // 
            this.labelDescription.AutoSize = true;
            this.labelDescription.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labelDescription.Location = new System.Drawing.Point(29, 77);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(443, 12);
            this.labelDescription.TabIndex = 8;
            this.labelDescription.Text = "说明：进行同一区域两个时段土地利用分类数据对比，可得到混淆矩阵及Kappa值。";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label1.Location = new System.Drawing.Point(18, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(221, 12);
            this.label1.TabIndex = 7;
            this.label1.Text = "首先选择需要进行影像对比分析的类型：";
            // 
            // buttonCompare
            // 
            this.buttonCompare.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.buttonCompare.Location = new System.Drawing.Point(117, 272);
            this.buttonCompare.Name = "buttonCompare";
            this.buttonCompare.Size = new System.Drawing.Size(80, 26);
            this.buttonCompare.TabIndex = 6;
            this.buttonCompare.Text = "对比";
            this.buttonCompare.UseVisualStyleBackColor = true;
            this.buttonCompare.Click += new System.EventHandler(this.buttonCompare_Click);
            // 
            // dataGridViewLandUse
            // 
            this.dataGridViewLandUse.AllowUserToAddRows = false;
            this.dataGridViewLandUse.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewLandUse.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnLUDescription,
            this.landuseColumnIsUrban});
            this.dataGridViewLandUse.Location = new System.Drawing.Point(360, 104);
            this.dataGridViewLandUse.Name = "dataGridViewLandUse";
            this.dataGridViewLandUse.RowTemplate.Height = 23;
            this.dataGridViewLandUse.Size = new System.Drawing.Size(351, 200);
            this.dataGridViewLandUse.TabIndex = 2;
            // 
            // ColumnLUDescription
            // 
            this.ColumnLUDescription.HeaderText = "土地利用类型说明";
            this.ColumnLUDescription.Name = "ColumnLUDescription";
            this.ColumnLUDescription.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ColumnLUDescription.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ColumnLUDescription.Width = 150;
            // 
            // landuseColumnIsUrban
            // 
            this.landuseColumnIsUrban.FalseValue = "false";
            this.landuseColumnIsUrban.HeaderText = "是否是城市用地";
            this.landuseColumnIsUrban.Name = "landuseColumnIsUrban";
            this.landuseColumnIsUrban.TrueValue = "true";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.textBoxResult);
            this.groupBox3.Location = new System.Drawing.Point(5, 617);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(717, 120);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "对比结果";
            // 
            // textBoxResult
            // 
            this.textBoxResult.Location = new System.Drawing.Point(14, 20);
            this.textBoxResult.Multiline = true;
            this.textBoxResult.Name = "textBoxResult";
            this.textBoxResult.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxResult.Size = new System.Drawing.Size(697, 92);
            this.textBoxResult.TabIndex = 0;
            // 
            // FormImagesCompare
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(734, 741);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormImagesCompare";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "影像对比分析";
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewConfusionMatrix)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewLandUse)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button buttonPercent;
        private System.Windows.Forms.DataGridView dataGridViewConfusionMatrix;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label labelSimulationData;
        private System.Windows.Forms.ComboBox comboBoxSimulation;
        private System.Windows.Forms.Label labelEndTime;
        private System.Windows.Forms.ComboBox comboBoxLayerEnd;
        private System.Windows.Forms.RadioButton radioButtonThreeImages;
        private System.Windows.Forms.RadioButton radioButtonActualSimualtion;
        private System.Windows.Forms.RadioButton radioButtonTwoImages;
        private System.Windows.Forms.Label labelStartTime;
        private System.Windows.Forms.ComboBox comboBoxLayerStart;
        private System.Windows.Forms.Label labelDescription;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonCompare;
        private System.Windows.Forms.DataGridView dataGridViewLandUse;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnLUDescription;
        private System.Windows.Forms.DataGridViewCheckBoxColumn landuseColumnIsUrban;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox textBoxResult;
    }
}