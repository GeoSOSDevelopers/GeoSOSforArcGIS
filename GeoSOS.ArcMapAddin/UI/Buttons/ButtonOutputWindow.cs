using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Framework;

namespace GeoSOS.ArcMapAddIn
{
    public class ButtonOutputWindow : ESRI.ArcGIS.Desktop.AddIns.Button
    {
        public ButtonOutputWindow()
        {
        }

        protected override void OnClick()
        {
            //
            //  TODO: Sample code showing how to access button host
            //
            UID theUid = new UIDClass();
            theUid.Value = ThisAddIn.IDs.DockableWindowOutput;
            IDockableWindow mapUnitForm = ArcMap.DockableWindowManager.GetDockableWindow(theUid);
            mapUnitForm.Show(true);
            DockableWindowOutput.AddinImpl addin = ESRI.ArcGIS.Desktop.AddIns.AddIn.FromID<DockableWindowOutput.AddinImpl>(theUid.Value.ToString());
            VariableMaintainer.CurrentDockableWindowOutput = addin.DockableWindowOutput;

            ArcMap.Application.CurrentTool = null;
        }
        protected override void OnUpdate()
        {
            Enabled = ArcMap.Application != null;
        }
    }
}
