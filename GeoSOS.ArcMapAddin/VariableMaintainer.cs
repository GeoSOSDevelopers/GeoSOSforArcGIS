using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Resources;
using System.Threading;
using System.ComponentModel;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.Carto;
using GeoSOS.ArcMapAddIn.Utility.CA;
using GeoSOS.CommonLibrary;
using ESRI.ArcGIS.DataSourcesRaster;
using ESRI.ArcGIS.Geodatabase;
using System.Windows.Forms;
using GeoSOS.ArcMapAddIn.UI.Forms;
using GeoSOS.CommonLibrary.Struct;

namespace GeoSOS.ArcMapAddIn
{
    public class VariableMaintainer
    {
        private static FormLogisticCAWizard formLogisticCAWizard;
        private static FormDTCAWizard formDTCAWizard;
        private static FormANNCAWizard formANNCAWizard;
        private static ResourceManager resourceManager;
        private static Thread thread;
        private static EnumCurrentModel enumCurrentModel;
        private static DockableWindowGraphy dockableWindowGraphy;
        private static DockableWindowOutput dockableWindowOutput;
        private static DockableWindowAreaOptimization dockableWindowAreaOptimization;
        private static bool isSimulationFinished = false;
        private static System.Windows.Forms.Timer timer;
        private static bool isNeedRefresh = false;
        private static string defaultOutputFolder = "";
        private static StructACOParameters structACOParameters;
        private static string optimizationExpression;
        private static bool isACOUtilitySet = false;
        private static bool isACOParametersSet = false;
        private static string restrictLayerName = "";
        //20170619添加限制层数据
        private static float[,] restrictImage = null;
        public static float[,] RestrictImage
        {
            get { return restrictImage; }
            set { restrictImage = value; }
        }

        public static string RestrictLayerName
        {
            get { return restrictLayerName; }
            set { restrictLayerName = value; }
        }

        public static IMap CurrentFoucsMap
        {
            get
            {
                return ArcMap.Document.FocusMap;
            }
        }

        public static FormLogisticCAWizard CurrentFormLogisticCAWizard
        {
            get
            {
                if (formLogisticCAWizard == null)
                    formLogisticCAWizard = new FormLogisticCAWizard();
                return formLogisticCAWizard;
            }
            set
            {
                formLogisticCAWizard = value;
            }
        }

        public static FormDTCAWizard CurrentFormDTCAWizard
        {
            get
            {
                if (formDTCAWizard == null)
                    formDTCAWizard = new FormDTCAWizard();
                return formDTCAWizard;
            }
            set
            {
                formDTCAWizard = value;
            }
        }

        public static FormANNCAWizard CurrentFormANNCAWizard
        {
            get
            {
                if (formANNCAWizard == null)
                    formANNCAWizard = new FormANNCAWizard();
                return formANNCAWizard;
            }
            set
            {
                formANNCAWizard = value;
            }
        }

        public static ResourceManager CurrentResourceManager
        {
            get
            {
                if (resourceManager == null)
                {
                    if (System.Threading.Thread.CurrentThread.CurrentCulture.Parent.Name == "zh-CHS")
                        resourceManager = new ResourceManager("GeoSOS.ArcMapAddIn.Properties.Resource_zh_CHS", System.Reflection.Assembly.GetExecutingAssembly());
                    else if (System.Threading.Thread.CurrentThread.CurrentCulture.Parent.Name == "zh-CHT")
                        resourceManager = new System.Resources.ResourceManager("GeoSOS.ArcMapAddIn.Properties.Resource_zh_TW", System.Reflection.Assembly.GetExecutingAssembly());
                    else if (System.Threading.Thread.CurrentThread.CurrentCulture.Parent.Name == "en")
                        resourceManager = new System.Resources.ResourceManager("GeoSOS.ArcMapAddIn.Properties.Resource_en", System.Reflection.Assembly.GetExecutingAssembly());
                }
                return resourceManager;
            }
        }

        public static Thread SimulationThread
        {
            get
            {
                return thread;
            }
            set
            {
                thread = value;
            }
        }

        public static EnumCurrentModel CurrentModel
        {
            get { return enumCurrentModel; }
            set { enumCurrentModel = value; }
        }

        public static DockableWindowGraphy CurrentDockableWindowGraphy
        {
            get
            {
                if (dockableWindowGraphy == null)
                {
                    UID theUid = new UIDClass();
                    theUid.Value = ThisAddIn.IDs.DockableWindowGraphy;
                    IDockableWindow mapUnitForm = ArcMap.DockableWindowManager.GetDockableWindow(theUid);
                    DockableWindowGraphy.AddinImpl addin = ESRI.ArcGIS.Desktop.AddIns.AddIn.FromID<DockableWindowGraphy.AddinImpl>(theUid.Value.ToString());
                    dockableWindowGraphy = addin.DockableWindowGraphy; ;
                }
                return dockableWindowGraphy;
            }
            set { dockableWindowGraphy = value; }
        }

        public static DockableWindowOutput CurrentDockableWindowOutput
        {
            get
            {
                if (dockableWindowOutput == null)
                {
                    UID theUid = new UIDClass();
                    theUid.Value = ThisAddIn.IDs.DockableWindowOutput;
                    IDockableWindow mapUnitForm = ArcMap.DockableWindowManager.GetDockableWindow(theUid);
                    mapUnitForm.Show(true);
                    DockableWindowOutput.AddinImpl addin = ESRI.ArcGIS.Desktop.AddIns.AddIn.FromID<DockableWindowOutput.AddinImpl>(theUid.Value.ToString());
                    dockableWindowOutput = addin.DockableWindowOutput;
                }
                return dockableWindowOutput;
            }
            set { dockableWindowOutput = value; }
        }

        public static DockableWindowAreaOptimization CurrentDockableWindowAreaOptimization
        {
            get
            {
                if (dockableWindowAreaOptimization == null)
                {
                    UID theUid = new UIDClass();
                    theUid.Value = ThisAddIn.IDs.DockableWindowAreaOptimization;
                    IDockableWindow mapUnitForm = ArcMap.DockableWindowManager.GetDockableWindow(theUid);
                    mapUnitForm.Show(true);
                    DockableWindowAreaOptimization.AddinImpl addin = ESRI.ArcGIS.Desktop.AddIns.AddIn.FromID<DockableWindowAreaOptimization.AddinImpl>(theUid.Value.ToString());
                    dockableWindowAreaOptimization = addin.DockableWindowAreaOptimization;
                }
                return dockableWindowAreaOptimization;
            }
            set { dockableWindowAreaOptimization = value; }
        }

        public static StructACOParameters CurrentStructACOParameters
        {
            get
            {
                return structACOParameters;
            }
            set
            { structACOParameters = value; }
        }

        public static bool IsSimulationFinished
        {
            get { return isSimulationFinished; }
            set { isSimulationFinished = value; }
        }

        public static bool IsNeedRefresh
        {
            get { return isNeedRefresh; }
            set { isNeedRefresh = value; }
        }

        public static System.Windows.Forms.Timer CurrentTimer
        {
            get
            {
                if (timer == null)
                {
                    timer = new System.Windows.Forms.Timer();
                }
                return timer;
            }
            set
            { timer = value; }
        }

        public static string DefaultOutputFolder
        {
            get
            {
                if (defaultOutputFolder == "")
                    defaultOutputFolder = GetOutputFolder();
                return defaultOutputFolder;
            }
        }

        public static string OptimizationExpression
        {
            get { return optimizationExpression; }
            set { optimizationExpression = value; }
        }

        public static bool IsACOUtilitySet
        {
            get { return isACOUtilitySet; }
            set { isACOUtilitySet=value; }
        }

        public static bool IsACOParametersSet
        {
            get { return isACOParametersSet; }
            set { isACOParametersSet = value; }
        }

        /// <summary>
        /// 获取当前Mxd的文件夹位置
        /// </summary>
        /// <returns></returns>
        public static string GetMxdDocumentFolder()
        {
            string mxdFileFullPath = ArcMap.Application.Templates.get_Item(ArcMap.Application.Templates.Count - 1);
            return mxdFileFullPath.Substring(0, mxdFileFullPath.LastIndexOf(@"\"));
        }

        /// <summary>
        /// 获取输出目录位置
        /// </summary>
        /// <returns></returns>
        public static string GetOutputFolder()
        {
            string outputFolder = GetMxdDocumentFolder() + @"\Output Data";
            if (!System.IO.Directory.Exists(outputFolder))
                System.IO.Directory.CreateDirectory(outputFolder);
            return outputFolder;
        }
    }
}
