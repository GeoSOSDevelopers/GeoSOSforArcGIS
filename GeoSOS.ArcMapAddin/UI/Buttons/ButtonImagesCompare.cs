using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoSOS.ArcMapAddIn.UI.Forms;

namespace GeoSOS.ArcMapAddIn
{
    public class ButtonImagesCompare : ESRI.ArcGIS.Desktop.AddIns.Button
    {
        protected override void OnClick()
        {
            FormImagesCompare formImagesCompare = new FormImagesCompare();
            formImagesCompare.ShowDialog();

            ArcMap.Application.CurrentTool = null;
        }

        protected override void OnUpdate()
        {
            Enabled = ArcMap.Application != null;
        }
    }
}
