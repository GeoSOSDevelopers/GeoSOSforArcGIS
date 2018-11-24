using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Framework;

namespace GeoSOS.ArcMapAddIn
{
    public class ButtonGraphyWindow : ESRI.ArcGIS.Desktop.AddIns.Button
    {
        public ButtonGraphyWindow()
        {
        }

        protected override void OnClick()
        {
            //
            //  TODO: Sample code showing how to access button host
            //
            UID theUid = new UIDClass();
            theUid.Value = ThisAddIn.IDs.DockableWindowGraphy;

            IDockableWindow mapUnitForm = ArcMap.DockableWindowManager.GetDockableWindow(theUid);
            mapUnitForm.Show(true);
            DockableWindowGraphy.AddinImpl addin = ESRI.ArcGIS.Desktop.AddIns.AddIn.FromID<DockableWindowGraphy.AddinImpl>(theUid.Value.ToString());
            VariableMaintainer.CurrentDockableWindowGraphy = addin.DockableWindowGraphy;

            ArcMap.Application.CurrentTool = null;
        }
        protected override void OnUpdate()
        {
            Enabled = ArcMap.Application != null;
        }
    }
}
