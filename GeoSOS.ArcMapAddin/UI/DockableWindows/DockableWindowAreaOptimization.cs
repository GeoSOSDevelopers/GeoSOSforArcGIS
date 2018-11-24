using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Resources;
using GeoSOS.ArcMapAddIn.UI.Forms;

namespace GeoSOS.ArcMapAddIn
{
    /// <summary>
    /// Designer class of the dockable window add-in. It contains user interfaces that
    /// make up the dockable window.
    /// </summary>
    public partial class DockableWindowAreaOptimization : UserControl
    {
        private ResourceManager resourceManager;
        private static FormACOUtility formACOUtility;
        private static FormACOParameters formACOParameters;
        private static FormACOOutput formACOOutput;

        public DockableWindowAreaOptimization(object hook)
        {
            resourceManager = VariableMaintainer.CurrentResourceManager;
            InitializeComponent();
            this.Hook = hook;
            if (formACOUtility == null)
                formACOUtility = new FormACOUtility();
            if (formACOParameters == null)
                formACOParameters = new FormACOParameters();
            if (formACOOutput == null)
                formACOOutput = new FormACOOutput();
        }

        /// <summary>
        /// Host object of the dockable window
        /// </summary>
        private object Hook
        {
            get;
            set;
        }

        /// <summary>
        /// Implementation class of the dockable window add-in. It is responsible for 
        /// creating and disposing the user interface class of the dockable window.
        /// </summary>
        public class AddinImpl : ESRI.ArcGIS.Desktop.AddIns.DockableWindow
        {
            private DockableWindowAreaOptimization m_windowUI;

            public DockableWindowAreaOptimization DockableWindowAreaOptimization
            {
                get { return m_windowUI; }
            }

            public AddinImpl()
            {
            }

            protected override IntPtr OnCreateChild()
            {
                m_windowUI = new DockableWindowAreaOptimization(this.Hook);
                return m_windowUI.Handle;
            }

            protected override void Dispose(bool disposing)
            {
                if (m_windowUI != null)
                    m_windowUI.Dispose(disposing);

                base.Dispose(disposing);
            }
        }

        private void linkLabelPaper_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://geosimulation.cn/Publications.html#Optimization-Area");
        }

        private void checkBoxUseDefault_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxUseDefault.Checked)
            {
                if (!ArcMap.Application.Caption.Contains("Optimization_DongGuan"))
                {
                    MessageBox.Show(resourceManager.GetString("String165"), resourceManager.GetString("String2"), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    checkBoxUseDefault.Checked = false;
                }
                else
                {
                    MessageBox.Show(resourceManager.GetString("String163"), resourceManager.GetString("String2"), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    //设置默认值                
                    formACOUtility.Initial();
                    string defaultExpressionString = "0.159*(1-[UrbanSuitable.tif])+0.214*[NDVI.tif]+0.214*[MNDWI.tif]+0.138*[Relief.tif]+0.138*[NDVIStd.tif]+0.137*(1-[UrbanDensity.tif])";
                    formACOUtility.TextBoxUtilityFormula.Text = defaultExpressionString;
                    VariableMaintainer.OptimizationExpression = defaultExpressionString;

                    formACOParameters.Initial();
                    formACOOutput.Initial();
                    VariableMaintainer.IsACOUtilitySet = true;
                    VariableMaintainer.IsACOParametersSet = true;
                    VariableMaintainer.CurrentModel = EnumCurrentModel.Optimization_Area;
                }
            }
        }

        private void buttonSetUtilityFunction_Click(object sender, EventArgs e)
        {
            formACOUtility.Initial();
            formACOUtility.ShowDialog();
        }

        private void buttonSetACOParas_Click(object sender, EventArgs e)
        {
            formACOParameters.Initial();
            formACOParameters.ShowDialog();
        }

        private void buttonSetOutput_Click(object sender, EventArgs e)
        {
            formACOOutput.Initial();
            formACOOutput.ShowDialog();
        }

        public static FormACOUtility CurrentFormACOUtility
        {
            get
            {
                if (formACOUtility == null)
                    formACOUtility = new FormACOUtility();
                return formACOUtility;
            }
            set
            {
                formACOUtility = value;
            }
        }

        public static FormACOParameters CurrentFormACOParameters
        {
            get
            {
                if (formACOParameters == null)
                    formACOParameters = new FormACOParameters();
                return formACOParameters;
            }
            set
            {
                formACOParameters = value;
            }
        }
    }
}
