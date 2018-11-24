namespace GeoSOS.ArcMapAddIn.UI.Forms
{
    partial class FormACOUtility
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormACOUtility));
            this.dataGridViewLayers = new System.Windows.Forms.DataGridView();
            this.DataGridViewTextBoxColumnLayers = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.textBoxUtility = new System.Windows.Forms.TextBox();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.pictureBox11 = new System.Windows.Forms.PictureBox();
            this.btnRight = new System.Windows.Forms.Button();
            this.btnLeft = new System.Windows.Forms.Button();
            this.btnAbs = new System.Windows.Forms.Button();
            this.btnLo = new System.Windows.Forms.Button();
            this.btnLog = new System.Windows.Forms.Button();
            this.btnExp = new System.Windows.Forms.Button();
            this.btnPow = new System.Windows.Forms.Button();
            this.btnSqr = new System.Windows.Forms.Button();
            this.btnSqrt = new System.Windows.Forms.Button();
            this.btnATan = new System.Windows.Forms.Button();
            this.btnACos = new System.Windows.Forms.Button();
            this.btnAsin = new System.Windows.Forms.Button();
            this.btnTan = new System.Windows.Forms.Button();
            this.btnCos = new System.Windows.Forms.Button();
            this.btnSin = new System.Windows.Forms.Button();
            this.btnDian = new System.Windows.Forms.Button();
            this.btn3 = new System.Windows.Forms.Button();
            this.btn6 = new System.Windows.Forms.Button();
            this.ftn9 = new System.Windows.Forms.Button();
            this.btn2 = new System.Windows.Forms.Button();
            this.btn5 = new System.Windows.Forms.Button();
            this.btn8 = new System.Windows.Forms.Button();
            this.btn0 = new System.Windows.Forms.Button();
            this.btn1 = new System.Windows.Forms.Button();
            this.btn4 = new System.Windows.Forms.Button();
            this.btn7 = new System.Windows.Forms.Button();
            this.btnDivide = new System.Windows.Forms.Button();
            this.btnMultiPly = new System.Windows.Forms.Button();
            this.btnSubtract = new System.Windows.Forms.Button();
            this.btnPlus = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewLayers)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox11)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridViewLayers
            // 
            this.dataGridViewLayers.AllowUserToAddRows = false;
            this.dataGridViewLayers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewLayers.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.DataGridViewTextBoxColumnLayers});
            this.dataGridViewLayers.Location = new System.Drawing.Point(19, 32);
            this.dataGridViewLayers.Name = "dataGridViewLayers";
            this.dataGridViewLayers.ReadOnly = true;
            this.dataGridViewLayers.RowHeadersWidth = 50;
            this.dataGridViewLayers.RowTemplate.Height = 23;
            this.dataGridViewLayers.Size = new System.Drawing.Size(314, 208);
            this.dataGridViewLayers.TabIndex = 1;
            this.dataGridViewLayers.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewLayers_CellDoubleClick);
            // 
            // DataGridViewTextBoxColumnLayers
            // 
            this.DataGridViewTextBoxColumnLayers.HeaderText = "空间变量图层名称";
            this.DataGridViewTextBoxColumnLayers.Name = "DataGridViewTextBoxColumnLayers";
            this.DataGridViewTextBoxColumnLayers.ReadOnly = true;
            this.DataGridViewTextBoxColumnLayers.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.DataGridViewTextBoxColumnLayers.Width = 250;
            // 
            // textBoxUtility
            // 
            this.textBoxUtility.Location = new System.Drawing.Point(19, 53);
            this.textBoxUtility.Multiline = true;
            this.textBoxUtility.Name = "textBoxUtility";
            this.textBoxUtility.Size = new System.Drawing.Size(807, 113);
            this.textBoxUtility.TabIndex = 2;
            this.textBoxUtility.MouseDown += new System.Windows.Forms.MouseEventHandler(this.textBoxUtility_MouseDown);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(724, 482);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(100, 28);
            this.buttonCancel.TabIndex = 73;
            this.buttonCancel.Text = "取消";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(589, 482);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(100, 28);
            this.buttonOK.TabIndex = 72;
            this.buttonOK.Text = "确定";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.pictureBox11);
            this.groupBox1.Controls.Add(this.btnRight);
            this.groupBox1.Controls.Add(this.btnLeft);
            this.groupBox1.Controls.Add(this.btnAbs);
            this.groupBox1.Controls.Add(this.btnLo);
            this.groupBox1.Controls.Add(this.btnLog);
            this.groupBox1.Controls.Add(this.btnExp);
            this.groupBox1.Controls.Add(this.btnPow);
            this.groupBox1.Controls.Add(this.btnSqr);
            this.groupBox1.Controls.Add(this.btnSqrt);
            this.groupBox1.Controls.Add(this.btnATan);
            this.groupBox1.Controls.Add(this.btnACos);
            this.groupBox1.Controls.Add(this.btnAsin);
            this.groupBox1.Controls.Add(this.btnTan);
            this.groupBox1.Controls.Add(this.btnCos);
            this.groupBox1.Controls.Add(this.btnSin);
            this.groupBox1.Controls.Add(this.btnDian);
            this.groupBox1.Controls.Add(this.btn3);
            this.groupBox1.Controls.Add(this.btn6);
            this.groupBox1.Controls.Add(this.ftn9);
            this.groupBox1.Controls.Add(this.btn2);
            this.groupBox1.Controls.Add(this.btn5);
            this.groupBox1.Controls.Add(this.btn8);
            this.groupBox1.Controls.Add(this.btn0);
            this.groupBox1.Controls.Add(this.btn1);
            this.groupBox1.Controls.Add(this.btn4);
            this.groupBox1.Controls.Add(this.btn7);
            this.groupBox1.Controls.Add(this.btnDivide);
            this.groupBox1.Controls.Add(this.btnMultiPly);
            this.groupBox1.Controls.Add(this.btnSubtract);
            this.groupBox1.Controls.Add(this.btnPlus);
            this.groupBox1.Location = new System.Drawing.Point(367, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(491, 258);
            this.groupBox1.TabIndex = 74;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "计算符号及函数";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(291, 225);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(157, 15);
            this.label2.TabIndex = 121;
            this.label2.Text = "点击按钮添加计算公式";
            // 
            // pictureBox11
            // 
            this.pictureBox11.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox11.Image")));
            this.pictureBox11.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pictureBox11.Location = new System.Drawing.Point(269, 224);
            this.pictureBox11.Name = "pictureBox11";
            this.pictureBox11.Size = new System.Drawing.Size(16, 16);
            this.pictureBox11.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox11.TabIndex = 120;
            this.pictureBox11.TabStop = false;
            // 
            // btnRight
            // 
            this.btnRight.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnRight.Location = new System.Drawing.Point(164, 205);
            this.btnRight.Margin = new System.Windows.Forms.Padding(4);
            this.btnRight.Name = "btnRight";
            this.btnRight.Size = new System.Drawing.Size(52, 35);
            this.btnRight.TabIndex = 119;
            this.btnRight.Tag = ")";
            this.btnRight.Text = ")";
            this.btnRight.UseVisualStyleBackColor = true;
            this.btnRight.Click += new System.EventHandler(this.operator_Click);
            // 
            // btnLeft
            // 
            this.btnLeft.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnLeft.Location = new System.Drawing.Point(104, 205);
            this.btnLeft.Margin = new System.Windows.Forms.Padding(4);
            this.btnLeft.Name = "btnLeft";
            this.btnLeft.Size = new System.Drawing.Size(52, 35);
            this.btnLeft.TabIndex = 118;
            this.btnLeft.Tag = "(";
            this.btnLeft.Text = "(";
            this.btnLeft.UseVisualStyleBackColor = true;
            this.btnLeft.Click += new System.EventHandler(this.operator_Click);
            // 
            // btnAbs
            // 
            this.btnAbs.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnAbs.Location = new System.Drawing.Point(44, 205);
            this.btnAbs.Margin = new System.Windows.Forms.Padding(4);
            this.btnAbs.Name = "btnAbs";
            this.btnAbs.Size = new System.Drawing.Size(52, 35);
            this.btnAbs.TabIndex = 117;
            this.btnAbs.Tag = "F_Abs()";
            this.btnAbs.Text = "Abs";
            this.btnAbs.UseVisualStyleBackColor = true;
            this.btnAbs.Click += new System.EventHandler(this.operator_Click);
            // 
            // btnLo
            // 
            this.btnLo.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnLo.Location = new System.Drawing.Point(104, 163);
            this.btnLo.Margin = new System.Windows.Forms.Padding(4);
            this.btnLo.Name = "btnLo";
            this.btnLo.Size = new System.Drawing.Size(52, 35);
            this.btnLo.TabIndex = 116;
            this.btnLo.Tag = "F_Log(,2.71828182845905";
            this.btnLo.Text = "Ln";
            this.btnLo.UseVisualStyleBackColor = true;
            this.btnLo.Click += new System.EventHandler(this.operator_Click);
            // 
            // btnLog
            // 
            this.btnLog.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnLog.Location = new System.Drawing.Point(44, 163);
            this.btnLog.Margin = new System.Windows.Forms.Padding(4);
            this.btnLog.Name = "btnLog";
            this.btnLog.Size = new System.Drawing.Size(52, 35);
            this.btnLog.TabIndex = 115;
            this.btnLog.Tag = "F_Log10()";
            this.btnLog.Text = "Log";
            this.btnLog.UseVisualStyleBackColor = true;
            this.btnLog.Click += new System.EventHandler(this.operator_Click);
            // 
            // btnExp
            // 
            this.btnExp.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnExp.Location = new System.Drawing.Point(104, 119);
            this.btnExp.Margin = new System.Windows.Forms.Padding(4);
            this.btnExp.Name = "btnExp";
            this.btnExp.Size = new System.Drawing.Size(52, 35);
            this.btnExp.TabIndex = 114;
            this.btnExp.Tag = "F_Exp()";
            this.btnExp.Text = "Exp";
            this.btnExp.UseVisualStyleBackColor = true;
            this.btnExp.Click += new System.EventHandler(this.operator_Click);
            // 
            // btnPow
            // 
            this.btnPow.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnPow.Location = new System.Drawing.Point(44, 119);
            this.btnPow.Margin = new System.Windows.Forms.Padding(4);
            this.btnPow.Name = "btnPow";
            this.btnPow.Size = new System.Drawing.Size(52, 35);
            this.btnPow.TabIndex = 113;
            this.btnPow.Tag = "F_Pow( , )";
            this.btnPow.Text = "Pow";
            this.btnPow.UseVisualStyleBackColor = true;
            this.btnPow.Click += new System.EventHandler(this.operator_Click);
            // 
            // btnSqr
            // 
            this.btnSqr.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSqr.Location = new System.Drawing.Point(164, 119);
            this.btnSqr.Margin = new System.Windows.Forms.Padding(4);
            this.btnSqr.Name = "btnSqr";
            this.btnSqr.Size = new System.Drawing.Size(52, 35);
            this.btnSqr.TabIndex = 112;
            this.btnSqr.Tag = "F_Pow( ,2)";
            this.btnSqr.Text = "Sqr";
            this.btnSqr.UseVisualStyleBackColor = true;
            this.btnSqr.Click += new System.EventHandler(this.operator_Click);
            // 
            // btnSqrt
            // 
            this.btnSqrt.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSqrt.Location = new System.Drawing.Point(164, 163);
            this.btnSqrt.Margin = new System.Windows.Forms.Padding(4);
            this.btnSqrt.Name = "btnSqrt";
            this.btnSqrt.Size = new System.Drawing.Size(52, 35);
            this.btnSqrt.TabIndex = 111;
            this.btnSqrt.Tag = "F_Sqrt()";
            this.btnSqrt.Text = "Sqrt";
            this.btnSqrt.UseVisualStyleBackColor = true;
            this.btnSqrt.Click += new System.EventHandler(this.operator_Click);
            // 
            // btnATan
            // 
            this.btnATan.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnATan.Location = new System.Drawing.Point(164, 77);
            this.btnATan.Margin = new System.Windows.Forms.Padding(4);
            this.btnATan.Name = "btnATan";
            this.btnATan.Size = new System.Drawing.Size(52, 35);
            this.btnATan.TabIndex = 110;
            this.btnATan.Tag = "F_Atan()";
            this.btnATan.Text = "ATan";
            this.btnATan.UseVisualStyleBackColor = true;
            this.btnATan.Click += new System.EventHandler(this.operator_Click);
            // 
            // btnACos
            // 
            this.btnACos.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnACos.Location = new System.Drawing.Point(104, 77);
            this.btnACos.Margin = new System.Windows.Forms.Padding(4);
            this.btnACos.Name = "btnACos";
            this.btnACos.Size = new System.Drawing.Size(52, 35);
            this.btnACos.TabIndex = 109;
            this.btnACos.Tag = "F_Acos()";
            this.btnACos.Text = "ACos";
            this.btnACos.UseVisualStyleBackColor = true;
            this.btnACos.Click += new System.EventHandler(this.operator_Click);
            // 
            // btnAsin
            // 
            this.btnAsin.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnAsin.Location = new System.Drawing.Point(44, 77);
            this.btnAsin.Margin = new System.Windows.Forms.Padding(4);
            this.btnAsin.Name = "btnAsin";
            this.btnAsin.Size = new System.Drawing.Size(52, 35);
            this.btnAsin.TabIndex = 108;
            this.btnAsin.Tag = "F_Asin()";
            this.btnAsin.Text = "ASin";
            this.btnAsin.UseVisualStyleBackColor = true;
            this.btnAsin.Click += new System.EventHandler(this.operator_Click);
            // 
            // btnTan
            // 
            this.btnTan.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnTan.Location = new System.Drawing.Point(164, 35);
            this.btnTan.Margin = new System.Windows.Forms.Padding(4);
            this.btnTan.Name = "btnTan";
            this.btnTan.Size = new System.Drawing.Size(52, 35);
            this.btnTan.TabIndex = 107;
            this.btnTan.Tag = "F_Tan()";
            this.btnTan.Text = "Tan";
            this.btnTan.UseVisualStyleBackColor = true;
            this.btnTan.Click += new System.EventHandler(this.operator_Click);
            // 
            // btnCos
            // 
            this.btnCos.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnCos.Location = new System.Drawing.Point(104, 35);
            this.btnCos.Margin = new System.Windows.Forms.Padding(4);
            this.btnCos.Name = "btnCos";
            this.btnCos.Size = new System.Drawing.Size(52, 35);
            this.btnCos.TabIndex = 106;
            this.btnCos.Tag = "F_Cos()";
            this.btnCos.Text = "Cos";
            this.btnCos.UseVisualStyleBackColor = true;
            this.btnCos.Click += new System.EventHandler(this.operator_Click);
            // 
            // btnSin
            // 
            this.btnSin.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSin.Location = new System.Drawing.Point(44, 35);
            this.btnSin.Margin = new System.Windows.Forms.Padding(4);
            this.btnSin.Name = "btnSin";
            this.btnSin.Size = new System.Drawing.Size(52, 35);
            this.btnSin.TabIndex = 105;
            this.btnSin.Tag = "F_Sin()";
            this.btnSin.Text = "Sin";
            this.btnSin.UseVisualStyleBackColor = true;
            this.btnSin.Click += new System.EventHandler(this.operator_Click);
            // 
            // btnDian
            // 
            this.btnDian.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnDian.Location = new System.Drawing.Point(401, 161);
            this.btnDian.Margin = new System.Windows.Forms.Padding(4);
            this.btnDian.Name = "btnDian";
            this.btnDian.Size = new System.Drawing.Size(37, 35);
            this.btnDian.TabIndex = 104;
            this.btnDian.Tag = ".";
            this.btnDian.Text = ".";
            this.btnDian.UseVisualStyleBackColor = true;
            this.btnDian.Click += new System.EventHandler(this.operator_Click);
            // 
            // btn3
            // 
            this.btn3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btn3.Location = new System.Drawing.Point(401, 119);
            this.btn3.Margin = new System.Windows.Forms.Padding(4);
            this.btn3.Name = "btn3";
            this.btn3.Size = new System.Drawing.Size(37, 35);
            this.btn3.TabIndex = 103;
            this.btn3.Tag = "3";
            this.btn3.Text = "3";
            this.btn3.UseVisualStyleBackColor = true;
            this.btn3.Click += new System.EventHandler(this.operator_Click);
            // 
            // btn6
            // 
            this.btn6.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btn6.Location = new System.Drawing.Point(401, 77);
            this.btn6.Margin = new System.Windows.Forms.Padding(4);
            this.btn6.Name = "btn6";
            this.btn6.Size = new System.Drawing.Size(37, 35);
            this.btn6.TabIndex = 102;
            this.btn6.Tag = "6";
            this.btn6.Text = "6";
            this.btn6.UseVisualStyleBackColor = true;
            this.btn6.Click += new System.EventHandler(this.operator_Click);
            // 
            // ftn9
            // 
            this.ftn9.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.ftn9.Location = new System.Drawing.Point(401, 34);
            this.ftn9.Margin = new System.Windows.Forms.Padding(4);
            this.ftn9.Name = "ftn9";
            this.ftn9.Size = new System.Drawing.Size(37, 35);
            this.ftn9.TabIndex = 101;
            this.ftn9.Tag = "9";
            this.ftn9.Text = "9";
            this.ftn9.UseVisualStyleBackColor = true;
            this.ftn9.Click += new System.EventHandler(this.operator_Click);
            // 
            // btn2
            // 
            this.btn2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btn2.Location = new System.Drawing.Point(357, 119);
            this.btn2.Margin = new System.Windows.Forms.Padding(4);
            this.btn2.Name = "btn2";
            this.btn2.Size = new System.Drawing.Size(37, 35);
            this.btn2.TabIndex = 100;
            this.btn2.Tag = "2";
            this.btn2.Text = "2";
            this.btn2.UseVisualStyleBackColor = true;
            this.btn2.Click += new System.EventHandler(this.operator_Click);
            // 
            // btn5
            // 
            this.btn5.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btn5.Location = new System.Drawing.Point(357, 77);
            this.btn5.Margin = new System.Windows.Forms.Padding(4);
            this.btn5.Name = "btn5";
            this.btn5.Size = new System.Drawing.Size(37, 35);
            this.btn5.TabIndex = 99;
            this.btn5.Tag = "5";
            this.btn5.Text = "5";
            this.btn5.UseVisualStyleBackColor = true;
            this.btn5.Click += new System.EventHandler(this.operator_Click);
            // 
            // btn8
            // 
            this.btn8.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btn8.Location = new System.Drawing.Point(357, 34);
            this.btn8.Margin = new System.Windows.Forms.Padding(4);
            this.btn8.Name = "btn8";
            this.btn8.Size = new System.Drawing.Size(37, 35);
            this.btn8.TabIndex = 98;
            this.btn8.Tag = "8";
            this.btn8.Text = "8";
            this.btn8.UseVisualStyleBackColor = true;
            this.btn8.Click += new System.EventHandler(this.operator_Click);
            // 
            // btn0
            // 
            this.btn0.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btn0.Location = new System.Drawing.Point(313, 161);
            this.btn0.Margin = new System.Windows.Forms.Padding(4);
            this.btn0.Name = "btn0";
            this.btn0.Size = new System.Drawing.Size(80, 35);
            this.btn0.TabIndex = 97;
            this.btn0.Tag = "0";
            this.btn0.Text = "0";
            this.btn0.UseVisualStyleBackColor = true;
            this.btn0.Click += new System.EventHandler(this.operator_Click);
            // 
            // btn1
            // 
            this.btn1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btn1.Location = new System.Drawing.Point(313, 119);
            this.btn1.Margin = new System.Windows.Forms.Padding(4);
            this.btn1.Name = "btn1";
            this.btn1.Size = new System.Drawing.Size(37, 35);
            this.btn1.TabIndex = 96;
            this.btn1.Tag = "1";
            this.btn1.Text = "1";
            this.btn1.UseVisualStyleBackColor = true;
            this.btn1.Click += new System.EventHandler(this.operator_Click);
            // 
            // btn4
            // 
            this.btn4.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btn4.Location = new System.Drawing.Point(313, 77);
            this.btn4.Margin = new System.Windows.Forms.Padding(4);
            this.btn4.Name = "btn4";
            this.btn4.Size = new System.Drawing.Size(37, 35);
            this.btn4.TabIndex = 95;
            this.btn4.Tag = "4";
            this.btn4.Text = "4";
            this.btn4.UseVisualStyleBackColor = true;
            this.btn4.Click += new System.EventHandler(this.operator_Click);
            // 
            // btn7
            // 
            this.btn7.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btn7.Location = new System.Drawing.Point(313, 34);
            this.btn7.Margin = new System.Windows.Forms.Padding(4);
            this.btn7.Name = "btn7";
            this.btn7.Size = new System.Drawing.Size(37, 35);
            this.btn7.TabIndex = 94;
            this.btn7.Tag = "7";
            this.btn7.Text = "7";
            this.btn7.UseVisualStyleBackColor = true;
            this.btn7.Click += new System.EventHandler(this.operator_Click);
            // 
            // btnDivide
            // 
            this.btnDivide.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnDivide.Location = new System.Drawing.Point(269, 161);
            this.btnDivide.Margin = new System.Windows.Forms.Padding(4);
            this.btnDivide.Name = "btnDivide";
            this.btnDivide.Size = new System.Drawing.Size(37, 35);
            this.btnDivide.TabIndex = 93;
            this.btnDivide.Tag = "/";
            this.btnDivide.Text = "/";
            this.btnDivide.UseVisualStyleBackColor = true;
            this.btnDivide.Click += new System.EventHandler(this.operator_Click);
            // 
            // btnMultiPly
            // 
            this.btnMultiPly.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnMultiPly.Location = new System.Drawing.Point(269, 119);
            this.btnMultiPly.Margin = new System.Windows.Forms.Padding(4);
            this.btnMultiPly.Name = "btnMultiPly";
            this.btnMultiPly.Size = new System.Drawing.Size(37, 35);
            this.btnMultiPly.TabIndex = 92;
            this.btnMultiPly.Tag = "*";
            this.btnMultiPly.Text = "*";
            this.btnMultiPly.UseVisualStyleBackColor = true;
            this.btnMultiPly.Click += new System.EventHandler(this.operator_Click);
            // 
            // btnSubtract
            // 
            this.btnSubtract.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSubtract.Location = new System.Drawing.Point(269, 77);
            this.btnSubtract.Margin = new System.Windows.Forms.Padding(4);
            this.btnSubtract.Name = "btnSubtract";
            this.btnSubtract.Size = new System.Drawing.Size(37, 35);
            this.btnSubtract.TabIndex = 91;
            this.btnSubtract.Tag = "-";
            this.btnSubtract.Text = "-";
            this.btnSubtract.UseVisualStyleBackColor = true;
            this.btnSubtract.Click += new System.EventHandler(this.operator_Click);
            // 
            // btnPlus
            // 
            this.btnPlus.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnPlus.Location = new System.Drawing.Point(269, 34);
            this.btnPlus.Margin = new System.Windows.Forms.Padding(4);
            this.btnPlus.Name = "btnPlus";
            this.btnPlus.Size = new System.Drawing.Size(37, 35);
            this.btnPlus.TabIndex = 90;
            this.btnPlus.Tag = "+";
            this.btnPlus.Text = "+";
            this.btnPlus.UseVisualStyleBackColor = true;
            this.btnPlus.Click += new System.EventHandler(this.operator_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.textBoxUtility);
            this.groupBox2.Location = new System.Drawing.Point(12, 276);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(846, 184);
            this.groupBox2.TabIndex = 75;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "效用函数";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 15);
            this.label1.TabIndex = 3;
            this.label1.Text = "Utility = ";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.dataGridViewLayers);
            this.groupBox3.Location = new System.Drawing.Point(12, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(349, 258);
            this.groupBox3.TabIndex = 76;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "选择数据";
            // 
            // FormACOUtility
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(870, 528);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormACOUtility";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "面优化效用函数设置";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewLayers)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox11)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewLayers;
        private System.Windows.Forms.DataGridViewTextBoxColumn DataGridViewTextBoxColumnLayers;
        private System.Windows.Forms.TextBox textBoxUtility;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnDian;
        private System.Windows.Forms.Button btn3;
        private System.Windows.Forms.Button btn6;
        private System.Windows.Forms.Button ftn9;
        private System.Windows.Forms.Button btn2;
        private System.Windows.Forms.Button btn5;
        private System.Windows.Forms.Button btn8;
        private System.Windows.Forms.Button btn0;
        private System.Windows.Forms.Button btn1;
        private System.Windows.Forms.Button btn4;
        private System.Windows.Forms.Button btn7;
        private System.Windows.Forms.Button btnDivide;
        private System.Windows.Forms.Button btnMultiPly;
        private System.Windows.Forms.Button btnSubtract;
        private System.Windows.Forms.Button btnPlus;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnRight;
        private System.Windows.Forms.Button btnLeft;
        private System.Windows.Forms.Button btnAbs;
        private System.Windows.Forms.Button btnLo;
        private System.Windows.Forms.Button btnLog;
        private System.Windows.Forms.Button btnExp;
        private System.Windows.Forms.Button btnPow;
        private System.Windows.Forms.Button btnSqr;
        private System.Windows.Forms.Button btnSqrt;
        private System.Windows.Forms.Button btnATan;
        private System.Windows.Forms.Button btnACos;
        private System.Windows.Forms.Button btnAsin;
        private System.Windows.Forms.Button btnTan;
        private System.Windows.Forms.Button btnCos;
        private System.Windows.Forms.Button btnSin;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox pictureBox11;
    }
}