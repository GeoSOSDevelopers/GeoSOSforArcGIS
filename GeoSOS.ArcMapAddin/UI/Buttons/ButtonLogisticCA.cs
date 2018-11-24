using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Resources;
using GeoSOS.CommonLibrary;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesRaster;
using ESRI.ArcGIS.ArcMapUI;
using GeoSOS.CommonLibrary.Struct;
using System.Windows.Forms;

namespace GeoSOS.ArcMapAddIn
{
    public class ButtonLogisticCA : ESRI.ArcGIS.Desktop.AddIns.Button
    {
        protected override void OnClick()
        {
            FormLogisticCAWizard formLogisticCAWizard = VariableMaintainer.CurrentFormLogisticCAWizard;
            formLogisticCAWizard.Clear();    //每次打开向导时清空各配置信息
            formLogisticCAWizard.ShowDialog();
            if (formLogisticCAWizard.IsFinish == true)
            {
                ResourceManager resourceManager = VariableMaintainer.CurrentResourceManager;
                MessageBox.Show(resourceManager.GetString("String38"), resourceManager.GetString("String2"), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                ArcMap.Application.StatusBar.set_Message(0, VariableMaintainer.CurrentResourceManager.GetString("String38"));

                VariableMaintainer.CurrentModel = EnumCurrentModel.Simulation_CA_LogisticRegression;
            }
            ArcMap.Application.CurrentTool = null;
        }

        protected override void OnUpdate()
        {
            Enabled = ArcMap.Application != null;
        }
    }

}
