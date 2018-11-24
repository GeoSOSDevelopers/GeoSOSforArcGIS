using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Resources;
using GeoSOS.ArcMapAddIn.UI.Forms;

namespace GeoSOS.ArcMapAddIn
{
    public class ButtonAbout : ESRI.ArcGIS.Desktop.AddIns.Button
    {
        protected override void OnClick()
        {
           FormAbout formAbout = new FormAbout();
           formAbout.ShowDialog();

            ArcMap.Application.CurrentTool = null;
        }

        protected override void OnUpdate()
        {
            Enabled = ArcMap.Application != null;
        }
    }
}
