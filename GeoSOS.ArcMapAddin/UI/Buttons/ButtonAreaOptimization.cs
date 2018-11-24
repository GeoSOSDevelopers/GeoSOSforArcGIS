using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoSOS.ArcMapAddIn.UI.Forms;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Framework;

namespace GeoSOS.ArcMapAddIn
{
     public class ButtonAreaOptimization : ESRI.ArcGIS.Desktop.AddIns.Button
    {
        protected override void OnClick()
        {
            //
            //  TODO: Sample code showing how to access button host
            //
            UID theUid = new UIDClass();
            theUid.Value = ThisAddIn.IDs.DockableWindowAreaOptimization;

            IDockableWindow mapUnitForm = ArcMap.DockableWindowManager.GetDockableWindow(theUid);
            mapUnitForm.Show(true);
            DockableWindowAreaOptimization.AddinImpl addin = ESRI.ArcGIS.Desktop.AddIns.AddIn.FromID<DockableWindowAreaOptimization.AddinImpl>(theUid.Value.ToString());

            ArcMap.Application.StatusBar.set_Message(0, VariableMaintainer.CurrentResourceManager.GetString("String164"));
            VariableMaintainer.CurrentModel = EnumCurrentModel.Optimization_Area;

            ArcMap.Application.CurrentTool = null;
        }

        protected override void OnUpdate()
        {
            Enabled = ArcMap.Application != null;
        }
    }
}
