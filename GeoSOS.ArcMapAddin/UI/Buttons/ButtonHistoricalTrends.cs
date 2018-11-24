using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoSOS.ArcMapAddIn.UI.Forms;

namespace GeoSOS.ArcMapAddIn
{
    public class ButtonHistoricalTrends : ESRI.ArcGIS.Desktop.AddIns.Button
    {
        protected override void OnClick()
        {
            FormLandUseHistory formLandUseHistory = new FormLandUseHistory();
            formLandUseHistory.ShowDialog();

            ArcMap.Application.CurrentTool = null;
        }

        protected override void OnUpdate()
        {
            Enabled = ArcMap.Application != null;
        }
    }
}
