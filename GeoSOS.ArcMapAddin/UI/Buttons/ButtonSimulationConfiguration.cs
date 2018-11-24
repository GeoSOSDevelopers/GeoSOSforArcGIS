using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Resources;
using GeoSOS.ArcMapAddIn.UI.Forms;

namespace GeoSOS.ArcMapAddIn
{
    public class ButtonSimulationConfiguration : ESRI.ArcGIS.Desktop.AddIns.Button
    {
        protected override void OnClick()
        {
            if (VariableMaintainer.CurrentModel == EnumCurrentModel.Null)
                return;
            ResourceManager resourceManager = VariableMaintainer.CurrentResourceManager;
            FormSimulationSetting formSimulationSetting = new FormSimulationSetting();
            if (VariableMaintainer.CurrentModel == EnumCurrentModel.Simulation_CA_LogisticRegression)
            {
                formSimulationSetting.LabelTask.Text = resourceManager.GetString("String56");
                formSimulationSetting.TextBoxSummary.Text = VariableMaintainer.CurrentFormLogisticCAWizard.WriteSummay();
          }
            else if(VariableMaintainer.CurrentModel== EnumCurrentModel.Simulation_CA_ANN)
            {
                formSimulationSetting.LabelTask.Text = resourceManager.GetString("String80");
                formSimulationSetting.TextBoxSummary.Text = VariableMaintainer.CurrentFormANNCAWizard.WriteSummay();
            }
            else if(VariableMaintainer.CurrentModel== EnumCurrentModel.Simulation_CA_DT)
            {
                formSimulationSetting.LabelTask.Text = resourceManager.GetString("String81");
                formSimulationSetting.TextBoxSummary.Text = VariableMaintainer.CurrentFormDTCAWizard.WriteSummay();
            }
            formSimulationSetting.Show();
            formSimulationSetting.LabelTask.Focus();

            ArcMap.Application.CurrentTool = null;
        }

        protected override void OnUpdate()
        {
            Enabled = ArcMap.Application != null;
        }
    }
}
