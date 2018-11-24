namespace GeoSOS.ArcMapAddIn.UI.Forms
{
    partial class FormLandUseHistory
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormLandUseHistory));
            this.buttonOk = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dataGridViewResult = new System.Windows.Forms.DataGridView();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonStatictics = new System.Windows.Forms.Button();
            this.buttonDelLayer = new System.Windows.Forms.Button();
            this.buttonAddLayer = new System.Windows.Forms.Button();
            this.dataGridViewLandUse = new System.Windows.Forms.DataGridView();
            this.ColumnLUDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.landuseColumnIsUrban = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dataGridViewLayers = new System.Windows.Forms.DataGridView();
            this.DataGridViewTextBoxColumnLayers = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.buttonExport = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewResult)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewLandUse)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewLayers)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonOk
            // 
            resources.ApplyResources(this.buttonOk, "buttonOk");
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dataGridViewResult);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // dataGridViewResult
            // 
            this.dataGridViewResult.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            resources.ApplyResources(this.dataGridViewResult, "dataGridViewResult");
            this.dataGridViewResult.Name = "dataGridViewResult";
            this.dataGridViewResult.RowTemplate.Height = 23;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.buttonStatictics);
            this.groupBox2.Controls.Add(this.buttonDelLayer);
            this.groupBox2.Controls.Add(this.buttonAddLayer);
            this.groupBox2.Controls.Add(this.dataGridViewLandUse);
            this.groupBox2.Controls.Add(this.dataGridViewLayers);
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // buttonStatictics
            // 
            resources.ApplyResources(this.buttonStatictics, "buttonStatictics");
            this.buttonStatictics.Name = "buttonStatictics";
            this.buttonStatictics.UseVisualStyleBackColor = true;
            this.buttonStatictics.Click += new System.EventHandler(this.buttonStatictics_Click);
            // 
            // buttonDelLayer
            // 
            resources.ApplyResources(this.buttonDelLayer, "buttonDelLayer");
            this.buttonDelLayer.Name = "buttonDelLayer";
            this.buttonDelLayer.UseVisualStyleBackColor = true;
            this.buttonDelLayer.Click += new System.EventHandler(this.buttonDelLayer_Click);
            // 
            // buttonAddLayer
            // 
            resources.ApplyResources(this.buttonAddLayer, "buttonAddLayer");
            this.buttonAddLayer.Name = "buttonAddLayer";
            this.buttonAddLayer.UseVisualStyleBackColor = true;
            this.buttonAddLayer.Click += new System.EventHandler(this.buttonAddLayer_Click);
            // 
            // dataGridViewLandUse
            // 
            this.dataGridViewLandUse.AllowUserToAddRows = false;
            this.dataGridViewLandUse.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewLandUse.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnLUDescription,
            this.landuseColumnIsUrban});
            resources.ApplyResources(this.dataGridViewLandUse, "dataGridViewLandUse");
            this.dataGridViewLandUse.Name = "dataGridViewLandUse";
            this.dataGridViewLandUse.RowTemplate.Height = 23;
            // 
            // ColumnLUDescription
            // 
            resources.ApplyResources(this.ColumnLUDescription, "ColumnLUDescription");
            this.ColumnLUDescription.Name = "ColumnLUDescription";
            this.ColumnLUDescription.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ColumnLUDescription.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // landuseColumnIsUrban
            // 
            this.landuseColumnIsUrban.FalseValue = "false";
            resources.ApplyResources(this.landuseColumnIsUrban, "landuseColumnIsUrban");
            this.landuseColumnIsUrban.Name = "landuseColumnIsUrban";
            this.landuseColumnIsUrban.TrueValue = "true";
            // 
            // dataGridViewLayers
            // 
            this.dataGridViewLayers.AllowUserToAddRows = false;
            this.dataGridViewLayers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewLayers.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.DataGridViewTextBoxColumnLayers});
            resources.ApplyResources(this.dataGridViewLayers, "dataGridViewLayers");
            this.dataGridViewLayers.Name = "dataGridViewLayers";
            this.dataGridViewLayers.RowTemplate.Height = 23;
            // 
            // DataGridViewTextBoxColumnLayers
            // 
            resources.ApplyResources(this.DataGridViewTextBoxColumnLayers, "DataGridViewTextBoxColumnLayers");
            this.DataGridViewTextBoxColumnLayers.Name = "DataGridViewTextBoxColumnLayers";
            this.DataGridViewTextBoxColumnLayers.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // buttonExport
            // 
            resources.ApplyResources(this.buttonExport, "buttonExport");
            this.buttonExport.Name = "buttonExport";
            this.buttonExport.UseVisualStyleBackColor = true;
            this.buttonExport.Click += new System.EventHandler(this.buttonExport_Click);
            // 
            // FormLandUseHistory
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.buttonExport);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonOk);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormLandUseHistory";
            this.ShowIcon = false;
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewResult)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewLandUse)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewLayers)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView dataGridViewResult;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView dataGridViewLayers;
        private System.Windows.Forms.DataGridViewTextBoxColumn DataGridViewTextBoxColumnLayers;
        private System.Windows.Forms.DataGridView dataGridViewLandUse;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnLUDescription;
        private System.Windows.Forms.DataGridViewCheckBoxColumn landuseColumnIsUrban;
        private System.Windows.Forms.Button buttonAddLayer;
        private System.Windows.Forms.Button buttonStatictics;
        private System.Windows.Forms.Button buttonDelLayer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonExport;
        private System.Windows.Forms.Label label2;
    }
}