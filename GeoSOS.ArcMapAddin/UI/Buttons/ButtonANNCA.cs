using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Resources;
using System.Windows.Forms;

namespace GeoSOS.ArcMapAddIn
{
    public class ButtonANNCA : ESRI.ArcGIS.Desktop.AddIns.Button
    {
        protected override void OnClick()
        {
            FormANNCAWizard formANNCAWizard = VariableMaintainer.CurrentFormANNCAWizard;
            formANNCAWizard.Clear();    //每次打开向导时清空各配置信息
            formANNCAWizard.ShowDialog();
            if (formANNCAWizard.IsFinish)
            {
                ResourceManager resourceManager = VariableMaintainer.CurrentResourceManager;
                MessageBox.Show(resourceManager.GetString("String38"), resourceManager.GetString("String2"), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                ArcMap.Application.StatusBar.set_Message(0, VariableMaintainer.CurrentResourceManager.GetString("String38"));
                VariableMaintainer.CurrentModel = EnumCurrentModel.Simulation_CA_ANN;
            }
            ArcMap.Application.CurrentTool = null;
        }

        protected override void OnUpdate()
        {
            Enabled = ArcMap.Application != null;
        }
    }
}
