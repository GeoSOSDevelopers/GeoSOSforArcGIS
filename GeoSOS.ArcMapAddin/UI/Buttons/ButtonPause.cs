using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Resources;

namespace GeoSOS.ArcMapAddIn
{
    public class ButtonPause : ESRI.ArcGIS.Desktop.AddIns.Button
    {
        protected override void OnClick()
        {
            if (VariableMaintainer.CurrentModel == EnumCurrentModel.Null)
                return;
            ResourceManager resourceManager = VariableMaintainer.CurrentResourceManager;
            Thread thread = VariableMaintainer.SimulationThread;
            if (thread != null)
            {
                if (thread.ThreadState == ThreadState.Running)
                {
                    thread.Suspend();
                    VariableMaintainer.CurrentDockableWindowOutput.AppendText("\n");
                    VariableMaintainer.CurrentDockableWindowOutput.AppendText(resourceManager.GetString("String59"));   
                    ArcMap.Application.StatusBar.set_Message(0, VariableMaintainer.CurrentResourceManager.GetString("String59"));
                }
                else if (thread.ThreadState == ThreadState.Suspended)
                {
                    thread.Resume();
                    VariableMaintainer.CurrentDockableWindowOutput.AppendText("\n");
                    VariableMaintainer.CurrentDockableWindowOutput.AppendText(resourceManager.GetString("String60"));
                    VariableMaintainer.CurrentDockableWindowOutput.AppendText("\n");
                    ArcMap.Application.StatusBar.set_Message(0, VariableMaintainer.CurrentResourceManager.GetString("String60"));
                }
            }
            ArcMap.Application.CurrentTool = null;
        }

        protected override void OnUpdate()
        {
            Enabled = ArcMap.Application != null;
        }
    }
}
