namespace GeoSOS.ArcMapAddIn.UI.Forms
{
    partial class FormAbout
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAbout));
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPageGeoSOS = new System.Windows.Forms.TabPage();
            this.richTextBoxTheory = new System.Windows.Forms.RichTextBox();
            this.tabPageCredits = new System.Windows.Forms.TabPage();
            this.richTextBoxCredits = new System.Windows.Forms.RichTextBox();
            this.tabPageUpdate = new System.Windows.Forms.TabPage();
            this.richTextBoxUpdate = new System.Windows.Forms.RichTextBox();
            this.tabPageLicense = new System.Windows.Forms.TabPage();
            this.richTextBoxLicense = new System.Windows.Forms.RichTextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonOk = new System.Windows.Forms.Button();
            this.tabControl.SuspendLayout();
            this.tabPageGeoSOS.SuspendLayout();
            this.tabPageCredits.SuspendLayout();
            this.tabPageUpdate.SuspendLayout();
            this.tabPageLicense.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPageGeoSOS);
            this.tabControl.Controls.Add(this.tabPageCredits);
            this.tabControl.Controls.Add(this.tabPageUpdate);
            this.tabControl.Controls.Add(this.tabPageLicense);
            resources.ApplyResources(this.tabControl, "tabControl");
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            // 
            // tabPageGeoSOS
            // 
            this.tabPageGeoSOS.Controls.Add(this.richTextBoxTheory);
            resources.ApplyResources(this.tabPageGeoSOS, "tabPageGeoSOS");
            this.tabPageGeoSOS.Name = "tabPageGeoSOS";
            this.tabPageGeoSOS.UseVisualStyleBackColor = true;
            // 
            // richTextBoxTheory
            // 
            resources.ApplyResources(this.richTextBoxTheory, "richTextBoxTheory");
            this.richTextBoxTheory.Name = "richTextBoxTheory";
            this.richTextBoxTheory.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.richTextBoxTheory_LinkClicked);
            // 
            // tabPageCredits
            // 
            this.tabPageCredits.Controls.Add(this.richTextBoxCredits);
            resources.ApplyResources(this.tabPageCredits, "tabPageCredits");
            this.tabPageCredits.Name = "tabPageCredits";
            this.tabPageCredits.UseVisualStyleBackColor = true;
            // 
            // richTextBoxCredits
            // 
            resources.ApplyResources(this.richTextBoxCredits, "richTextBoxCredits");
            this.richTextBoxCredits.Name = "richTextBoxCredits";
            this.richTextBoxCredits.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.richTextBoxCredits_LinkClicked);
            // 
            // tabPageUpdate
            // 
            this.tabPageUpdate.Controls.Add(this.richTextBoxUpdate);
            resources.ApplyResources(this.tabPageUpdate, "tabPageUpdate");
            this.tabPageUpdate.Name = "tabPageUpdate";
            this.tabPageUpdate.UseVisualStyleBackColor = true;
            // 
            // richTextBoxUpdate
            // 
            resources.ApplyResources(this.richTextBoxUpdate, "richTextBoxUpdate");
            this.richTextBoxUpdate.Name = "richTextBoxUpdate";
            // 
            // tabPageLicense
            // 
            this.tabPageLicense.Controls.Add(this.richTextBoxLicense);
            resources.ApplyResources(this.tabPageLicense, "tabPageLicense");
            this.tabPageLicense.Name = "tabPageLicense";
            this.tabPageLicense.UseVisualStyleBackColor = true;
            // 
            // richTextBoxLicense
            // 
            resources.ApplyResources(this.richTextBoxLicense, "richTextBoxLicense");
            this.richTextBoxLicense.Name = "richTextBoxLicense";
            this.richTextBoxLicense.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.richTextBoxLicense_LinkClicked);
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // pictureBox1
            // 
            resources.ApplyResources(this.pictureBox1, "pictureBox1");
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.TabStop = false;
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
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
            // buttonOk
            // 
            resources.ApplyResources(this.buttonOk, "buttonOk");
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // FormAbout
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonOk);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormAbout";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.tabControl.ResumeLayout(false);
            this.tabPageGeoSOS.ResumeLayout(false);
            this.tabPageCredits.ResumeLayout(false);
            this.tabPageUpdate.ResumeLayout(false);
            this.tabPageLicense.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPageCredits;
        private System.Windows.Forms.RichTextBox richTextBoxCredits;
        private System.Windows.Forms.TabPage tabPageUpdate;
        private System.Windows.Forms.RichTextBox richTextBoxUpdate;
        private System.Windows.Forms.TabPage tabPageLicense;
        private System.Windows.Forms.RichTextBox richTextBoxLicense;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.TabPage tabPageGeoSOS;
        private System.Windows.Forms.RichTextBox richTextBoxTheory;
    }
}