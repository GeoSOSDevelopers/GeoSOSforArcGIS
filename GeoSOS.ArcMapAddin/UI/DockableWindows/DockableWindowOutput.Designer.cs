namespace GeoSOS.ArcMapAddIn
{
    partial class DockableWindowOutput
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DockableWindowOutput));
            this.toolStripOutputWindow = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonSave = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonClear = new System.Windows.Forms.ToolStripButton();
            this.textBox = new System.Windows.Forms.TextBox();
            this.toolStripOutputWindow.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripOutputWindow
            // 
            this.toolStripOutputWindow.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonSave,
            this.toolStripSeparator1,
            this.toolStripButtonClear});
            resources.ApplyResources(this.toolStripOutputWindow, "toolStripOutputWindow");
            this.toolStripOutputWindow.Name = "toolStripOutputWindow";
            // 
            // toolStripButtonSave
            // 
            this.toolStripButtonSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.toolStripButtonSave, "toolStripButtonSave");
            this.toolStripButtonSave.Name = "toolStripButtonSave";
            this.toolStripButtonSave.Click += new System.EventHandler(this.toolStripButtonSave_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // toolStripButtonClear
            // 
            this.toolStripButtonClear.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.toolStripButtonClear, "toolStripButtonClear");
            this.toolStripButtonClear.Name = "toolStripButtonClear";
            this.toolStripButtonClear.Click += new System.EventHandler(this.toolStripButtonClear_Click);
            // 
            // textBox
            // 
            this.textBox.BackColor = System.Drawing.SystemColors.Window;
            resources.ApplyResources(this.textBox, "textBox");
            this.textBox.Name = "textBox";
            this.textBox.ReadOnly = true;
            // 
            // DockableWindowOutput
            // 
            this.Controls.Add(this.textBox);
            this.Controls.Add(this.toolStripOutputWindow);
            this.Name = "DockableWindowOutput";
            resources.ApplyResources(this, "$this");
            this.toolStripOutputWindow.ResumeLayout(false);
            this.toolStripOutputWindow.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStripOutputWindow;
        private System.Windows.Forms.ToolStripButton toolStripButtonClear;
        private System.Windows.Forms.ToolStripButton toolStripButtonSave;
        private System.Windows.Forms.TextBox textBox;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;

    }
}
