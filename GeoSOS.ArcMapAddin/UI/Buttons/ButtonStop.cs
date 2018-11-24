using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace GeoSOS.ArcMapAddIn
{
    public class ButtonStop : ESRI.ArcGIS.Desktop.AddIns.Button
    {
        protected override void OnClick()
        {
            if (VariableMaintainer.CurrentModel == EnumCurrentModel.Null)
                return;
            Thread thread = VariableMaintainer.SimulationThread;
            if (thread != null)
            {
                if (thread.ThreadState == ThreadState.Stopped)
                    return;
                if (thread.ThreadState == ThreadState.Suspended)
                    thread.Resume();
                thread.Abort();
                ArcMap.Application.StatusBar.set_Message(0, VariableMaintainer.CurrentResourceManager.GetString("String61"));
                VariableMaintainer.CurrentDockableWindowOutput.AppendText("\n");
                VariableMaintainer.CurrentDockableWindowOutput.AppendText(
                    VariableMaintainer.CurrentResourceManager.GetString("String61"));
                VariableMaintainer.CurrentDockableWindowOutput.AppendText("\n");
            }
            ArcMap.Application.CurrentTool = null;
        }

        protected override void OnUpdate()
        {
            Enabled = ArcMap.Application != null;
        }
    }
}
