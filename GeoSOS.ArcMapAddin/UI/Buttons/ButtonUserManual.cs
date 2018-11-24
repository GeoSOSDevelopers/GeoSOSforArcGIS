using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Windows.Forms;
using GeoSOS.ArcMapAddIn.UI.Forms;

namespace GeoSOS.ArcMapAddIn
{
   public class ButtonUserManual : ESRI.ArcGIS.Desktop.AddIns.Button
    {
       public ButtonUserManual()
        {
        }

        protected override void OnClick()
        {
            //MessageBox.Show(VariableMaintainer.CurrentResourceManager.GetString("String62"),
            //    VariableMaintainer.CurrentResourceManager.GetString("String2"),
            //            MessageBoxButtons.OK, MessageBoxIcon.Information);
            FormManual formManual = new FormManual();
            formManual.ShowDialog();

            ArcMap.Application.CurrentTool = null;
        }
        protected override void OnUpdate()
        {
            Enabled = ArcMap.Application != null;
        }
    }
}
