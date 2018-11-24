using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GeoSOS.CommonLibrary.Struct;

namespace GeoSOS.ArcMapAddIn.UI.Forms
{
    public partial class FormACOOutput : Form
    {
        private StructACOParameters structACOParameters;

        public FormACOOutput()
        {
            InitializeComponent();
            Initial();
        }

        public void Initial()
        {
            structACOParameters = VariableMaintainer.CurrentStructACOParameters;
            if (string.IsNullOrEmpty(structACOParameters.OutputFolder))
                structACOParameters.OutputFolder = "";
            if (structACOParameters.OutputFolder == "")
                structACOParameters.OutputFolder = VariableMaintainer.GetOutputFolder();
            this.textBoxOutputFolder.Text = structACOParameters.OutputFolder;
            radioButtonOutput.Checked = structACOParameters.IsOutput;
            if (structACOParameters.MapRefreshInterval == 0)
                structACOParameters.MapRefreshInterval = 5;
            this.numericUpDownRefreshInvterval.Value = Convert.ToDecimal(structACOParameters.MapRefreshInterval);
        }

        private void buttonOutputFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                textBoxOutputFolder.Text = folderBrowserDialog.SelectedPath;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            structACOParameters.OutputFolder = textBoxOutputFolder.Text;
            structACOParameters.IsOutput = radioButtonOutput.Checked;
            structACOParameters.MapRefreshInterval = Convert.ToInt32(this.numericUpDownRefreshInvterval.Value);
            VariableMaintainer.CurrentStructACOParameters = structACOParameters;
            this.Close();
        }
    }
}
