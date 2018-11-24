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
    public partial class FormACOParameters : Form
    {
        private StructACOParameters structACOParameters;

        public FormACOParameters()
        {
            InitializeComponent();
            structACOParameters.Q = 10;
            structACOParameters.Alpha = 3;
            structACOParameters.Beta = 2;
            structACOParameters.Rho = 0.05f;
            structACOParameters.WeightSuitable = 1f;
            structACOParameters.WeightCompact = 1f;
            structACOParameters.AntCount = 5500;
            structACOParameters.InterationCount = 1000;
            VariableMaintainer.CurrentStructACOParameters = structACOParameters;
        }

        public void Initial()
        {
            structACOParameters = VariableMaintainer.CurrentStructACOParameters;
            textBoxQ.Text = structACOParameters.Q.ToString();
            textBoxAlpha.Text = structACOParameters.Alpha.ToString();
            textBoxBeta.Text = structACOParameters.Beta.ToString();
            textBoxRho.Text = structACOParameters.Rho.ToString();
            textBoxWeightSuitable.Text = structACOParameters.WeightSuitable.ToString();
            textBoxWeightCompact.Text = structACOParameters.WeightCompact.ToString();
            textBoxAntCount.Text = structACOParameters.AntCount.ToString();
            textBoxIterationCount.Text = structACOParameters.InterationCount.ToString();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            structACOParameters.Q = Convert.ToInt32(textBoxQ.Text);
            structACOParameters.Alpha = Convert.ToInt32(textBoxAlpha.Text);
            structACOParameters.Beta = Convert.ToInt32(textBoxBeta.Text);
            structACOParameters.Rho = Convert.ToSingle(textBoxRho.Text);
            structACOParameters.WeightSuitable = Convert.ToSingle(textBoxWeightSuitable.Text);
            structACOParameters.WeightCompact = Convert.ToSingle(textBoxWeightCompact.Text);
            structACOParameters.AntCount = Convert.ToInt32(textBoxAntCount.Text);
            structACOParameters.InterationCount = Convert.ToInt32(textBoxIterationCount.Text);
            VariableMaintainer.CurrentStructACOParameters = structACOParameters;
            VariableMaintainer.IsACOParametersSet = true;
            this.Close();
        }
    }
}
