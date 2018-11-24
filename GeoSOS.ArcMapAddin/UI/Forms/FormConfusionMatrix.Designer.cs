namespace GeoSOS.ArcMapAddIn.UI.Forms
{
    partial class FormConfusionMatrix
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormConfusionMatrix));
            this.dataGridViewConfusionMatrix = new System.Windows.Forms.DataGridView();
            this.buttonPercent = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonOK = new System.Windows.Forms.Button();
            this.labelAccuracy = new System.Windows.Forms.Label();
            this.labelKappa = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.labelPA = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.labelUA = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.labelFoMValues = new System.Windows.Forms.Label();
            this.labelFoM = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewConfusionMatrix)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridViewConfusionMatrix
            // 
            resources.ApplyResources(this.dataGridViewConfusionMatrix, "dataGridViewConfusionMatrix");
            this.dataGridViewConfusionMatrix.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewConfusionMatrix.Name = "dataGridViewConfusionMatrix";
            this.dataGridViewConfusionMatrix.ReadOnly = true;
            this.dataGridViewConfusionMatrix.RowTemplate.Height = 23;
            // 
            // buttonPercent
            // 
            resources.ApplyResources(this.buttonPercent, "buttonPercent");
            this.buttonPercent.Name = "buttonPercent";
            this.buttonPercent.UseVisualStyleBackColor = true;
            this.buttonPercent.Click += new System.EventHandler(this.buttonPercent_Click);
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.buttonPercent);
            this.groupBox1.Controls.Add(this.dataGridViewConfusionMatrix);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // buttonOK
            // 
            resources.ApplyResources(this.buttonOK, "buttonOK");
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // labelAccuracy
            // 
            resources.ApplyResources(this.labelAccuracy, "labelAccuracy");
            this.labelAccuracy.Name = "labelAccuracy";
            // 
            // labelKappa
            // 
            resources.ApplyResources(this.labelKappa, "labelKappa");
            this.labelKappa.Name = "labelKappa";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // groupBox2
            // 
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.labelPA);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.labelUA);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.labelFoMValues);
            this.groupBox2.Controls.Add(this.labelFoM);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.labelAccuracy);
            this.groupBox2.Controls.Add(this.labelKappa);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // labelPA
            // 
            resources.ApplyResources(this.labelPA, "labelPA");
            this.labelPA.Name = "labelPA";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // labelUA
            // 
            resources.ApplyResources(this.labelUA, "labelUA");
            this.labelUA.Name = "labelUA";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // labelFoMValues
            // 
            resources.ApplyResources(this.labelFoMValues, "labelFoMValues");
            this.labelFoMValues.Name = "labelFoMValues";
            // 
            // labelFoM
            // 
            resources.ApplyResources(this.labelFoM, "labelFoM");
            this.labelFoM.Name = "labelFoM";
            // 
            // FormConfusionMatrix
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.groupBox1);
            this.MinimizeBox = false;
            this.Name = "FormConfusionMatrix";
            this.ShowIcon = false;
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewConfusionMatrix)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewConfusionMatrix;
        private System.Windows.Forms.Button buttonPercent;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Label labelAccuracy;
        private System.Windows.Forms.Label labelKappa;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label labelFoMValues;
        private System.Windows.Forms.Label labelFoM;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label labelPA;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label labelUA;

    }
}