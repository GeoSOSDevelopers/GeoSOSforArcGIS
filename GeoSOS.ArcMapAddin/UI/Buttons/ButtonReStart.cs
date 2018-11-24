using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeoSOS.ArcMapAddIn
{
    public class ButtonReStart : ESRI.ArcGIS.Desktop.AddIns.Button
    {
        protected override void OnClick()
        {
            ArcMap.Application.StatusBar.set_Message(0, VariableMaintainer.CurrentResourceManager.GetString("String57"));
            ArcMap.Application.CurrentTool = null;
        }

        protected override void OnUpdate()
        {
            Enabled = ArcMap.Application != null;
        }
    }
}
