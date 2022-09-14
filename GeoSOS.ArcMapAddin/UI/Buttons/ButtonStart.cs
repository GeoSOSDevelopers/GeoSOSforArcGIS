using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Resources;
using GeoSOS.ArcMapAddIn.Utility.CA;
using GeoSOS.CommonLibrary.Struct;
using GeoSOS.CommonLibrary;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesRaster;
using GeoSOS.ArcMapAddIn.Utility.Optimization;

namespace GeoSOS.ArcMapAddIn
{
    public class ButtonStart : ESRI.ArcGIS.Desktop.AddIns.Button
    {
        public ButtonStart()
        {
        }

        protected override void OnClick()
        {
            try
            {
                ResourceManager resourceManager = VariableMaintainer.CurrentResourceManager;
                if (VariableMaintainer.CurrentModel == EnumCurrentModel.Null)
                {
                    MessageBox.Show(resourceManager.GetString("String168"), resourceManager.GetString("String2"),
                           MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                
                Thread thread = VariableMaintainer.SimulationThread;
                if (thread != null)
                {
                    if (thread.ThreadState != ThreadState.Stopped)
                    {
                        MessageBox.Show(resourceManager.GetString("String39"), resourceManager.GetString("String2"),
                            MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                }

                StructRasterMetaData structRasterMetaData = new StructRasterMetaData();
                string model = "";
                if (VariableMaintainer.CurrentModel == EnumCurrentModel.Simulation_CA_LogisticRegression)
                    model = resourceManager.GetString("String104");
                else if (VariableMaintainer.CurrentModel == EnumCurrentModel.Simulation_CA_ANN)
                    model = resourceManager.GetString("String102");
                else if (VariableMaintainer.CurrentModel == EnumCurrentModel.Simulation_CA_DT)
                    model = resourceManager.GetString("String103");
                else if (VariableMaintainer.CurrentModel == EnumCurrentModel.Optimization_Area)
                    model = resourceManager.GetString("String167");
                VariableMaintainer.CurrentDockableWindowOutput.AppendText(resourceManager.GetString("String82")
                    + model + resourceManager.GetString("String101") + DateTime.Now.ToShortTimeString());
                VariableMaintainer.CurrentDockableWindowOutput.AppendText("\n");
                VariableMaintainer.CurrentDockableWindowOutput.AppendText("\n");
                ArcMap.Application.StatusBar.set_Message(0, resourceManager.GetString("String58"));
                VariableMaintainer.CurrentDockableWindowOutput.AppendText(resourceManager.GetString("String58"));
                VariableMaintainer.CurrentDockableWindowOutput.AppendText("\n");

                if (VariableMaintainer.CurrentModel == EnumCurrentModel.Simulation_CA_LogisticRegression)
                {
                    //ArcGIS操作在另一线程中执行效率降低很多，因此把数据读取及刷新操作在线程外执行
                    //首先读取模拟起始时刻和/或终止时刻影像       
                    ArcMap.Application.StatusBar.set_Message(0, resourceManager.GetString("String12"));
                    VariableMaintainer.CurrentDockableWindowOutput.AppendText(resourceManager.GetString("String12") +
                                VariableMaintainer.CurrentFormLogisticCAWizard.SimulationStartImageName + ".....\n");
                    VariableMaintainer.CurrentFormLogisticCAWizard.SimulationStartImage =
                        ArcGISOperator.ReadRasterAndGetMetaData(ArcGISOperator.GetRasterLayerByName(
                        VariableMaintainer.CurrentFormLogisticCAWizard.SimulationStartImageName), out structRasterMetaData);
                    if (VariableMaintainer.CurrentFormLogisticCAWizard.SimulationEndImageName != "")
                    {
                        ArcMap.Application.StatusBar.set_Message(0, resourceManager.GetString("String13"));
                        VariableMaintainer.CurrentDockableWindowOutput.AppendText(resourceManager.GetString("String13") +
                            VariableMaintainer.CurrentFormLogisticCAWizard.SimulationEndImageName + ".....\n");
                        VariableMaintainer.CurrentFormLogisticCAWizard.SimulationEndImage = ArcGISOperator.ReadRaster(ArcGISOperator.GetRasterLayerByName(
                                VariableMaintainer.CurrentFormLogisticCAWizard.SimulationEndImageName), -9999f);
                    }
                    //然后读取各变量影像
                    if (VariableMaintainer.CurrentFormLogisticCAWizard.IsUsingDefault)
                    {
                        foreach (string layerName in VariableMaintainer.CurrentFormLogisticCAWizard.ListVariableLayersName)
                        {
                            ArcMap.Application.StatusBar.set_Message(0, resourceManager.GetString("String14") + layerName + ".....");
                            VariableMaintainer.CurrentDockableWindowOutput.AppendText(resourceManager.GetString("String14") + layerName + ".....\n");
                            VariableMaintainer.CurrentFormLogisticCAWizard.VaribaleImages.Add(
                                ArcGISOperator.ReadRaster(ArcGISOperator.GetRasterLayerByName(layerName), -9999f));
                        }
                        //获取最小空间范围
                        IRasterLayer rasterLayerStartImage = ArcGISOperator.GetRasterLayerByName(VariableMaintainer.CurrentFormLogisticCAWizard.SimulationStartImageName);
                        IRasterLayer rasterLayerEndImage = ArcGISOperator.GetRasterLayerByName(VariableMaintainer.CurrentFormLogisticCAWizard.SimulationEndImageName);

                        List<IRasterLayer> listVariablesLayers = new List<IRasterLayer>();
                        for (int i = 0; i < VariableMaintainer.CurrentFormLogisticCAWizard.ListVariableLayersName.Count; i++)
                            listVariablesLayers.Add(ArcGISOperator.GetRasterLayerByName(VariableMaintainer.CurrentFormLogisticCAWizard.ListVariableLayersName[i]));
                        ArcGISOperator.GetSmallestBound(rasterLayerStartImage, rasterLayerEndImage, listVariablesLayers, ref structRasterMetaData);
                        VariableMaintainer.CurrentFormLogisticCAWizard.CurrentStructRasterMetaData = structRasterMetaData;
                    }
                    //添加限制层数据
                    VariableMaintainer.CurrentDockableWindowOutput.AppendText(resourceManager.GetString("String171") +
                            VariableMaintainer.RestrictLayerName + ".....\n");
                    VariableMaintainer.RestrictImage = ArcGISOperator.ReadRaster(ArcGISOperator.GetRasterLayerByName(
                            VariableMaintainer.RestrictLayerName), -9999f);

                    //最后用模拟起始时刻影像创建模拟影像
                    string dateTime = GeneralOpertor.GetDataTimeFullString(DateTime.Now);
                    string rasterName = "sim" + dateTime + ".img";
                    ArcMap.Application.StatusBar.set_Message(0, resourceManager.GetString("String37"));
                    VariableMaintainer.CurrentDockableWindowOutput.AppendText(resourceManager.GetString("String37") + rasterName + ".....\n");

                    IRasterLayer simulationStartImageLayer = ArcGISOperator.GetRasterLayerByName(
                        VariableMaintainer.CurrentFormLogisticCAWizard.SimulationStartImageName);
                    IWorkspaceFactory workspaceFactory = new RasterWorkspaceFactoryClass();
                    IWorkspace workspace = workspaceFactory.OpenFromFile(VariableMaintainer.CurrentFormLogisticCAWizard.OutputFolder, 0);
                    ISaveAs2 saveAs2 = (ISaveAs2)simulationStartImageLayer.Raster;
                    saveAs2.SaveAs(rasterName, workspace, "IMAGINE Image");
                    IRasterLayer simulationImageLayer = new RasterLayerClass();
                    simulationImageLayer.CreateFromFilePath(VariableMaintainer.CurrentFormLogisticCAWizard.OutputFolder + @"\" + rasterName);
                    simulationImageLayer.Renderer = simulationStartImageLayer.Renderer;
                    ArcGISOperator.FoucsMap.AddLayer((ILayer)simulationImageLayer);
                    VariableMaintainer.CurrentFormLogisticCAWizard.SimulationImage = ArcGISOperator.ReadRaster(simulationImageLayer, -9999f);
                    VariableMaintainer.CurrentFormLogisticCAWizard.SimulationLayerName = rasterName;

                    LogisticRegreesionCA lrCA = new LogisticRegreesionCA();
                    lrCA.DockableWindowGraphy = VariableMaintainer.CurrentDockableWindowGraphy;
                    lrCA.DockableWindowOutput = VariableMaintainer.CurrentDockableWindowOutput;

                    thread = new Thread(new ThreadStart(lrCA.DoSimulation));
                }
                else if (VariableMaintainer.CurrentModel == EnumCurrentModel.Simulation_CA_ANN)
                {
                    //今后应使用父类和接口进行重构
                    //首先读取模拟起始时刻和/或终止时刻影像         
                    ArcMap.Application.StatusBar.set_Message(0, resourceManager.GetString("String12"));
                    VariableMaintainer.CurrentDockableWindowOutput.AppendText(resourceManager.GetString("String12") +
                                VariableMaintainer.CurrentFormANNCAWizard.SimulationStartImageName + ".....\n");
                    VariableMaintainer.CurrentFormANNCAWizard.SimulationStartImage =
                        ArcGISOperator.ReadRasterAndGetMetaData(ArcGISOperator.GetRasterLayerByName(
                        VariableMaintainer.CurrentFormANNCAWizard.SimulationStartImageName), out structRasterMetaData);
                    //VariableMaintainer.CurrentANNCAWizard.CurrentStructRasterMetaData = structRasterMetaData;
                    if (VariableMaintainer.CurrentFormANNCAWizard.SimulationEndImageName != "")
                    {
                        ArcMap.Application.StatusBar.set_Message(0, resourceManager.GetString("String13"));
                        VariableMaintainer.CurrentDockableWindowOutput.AppendText(resourceManager.GetString("String13") +
                            VariableMaintainer.CurrentFormLogisticCAWizard.SimulationEndImageName + ".....\n");
                        VariableMaintainer.CurrentFormANNCAWizard.SimulationEndImage = ArcGISOperator.ReadRaster(ArcGISOperator.GetRasterLayerByName(
                                VariableMaintainer.CurrentFormANNCAWizard.SimulationEndImageName), -9999f);
                    }
                    //添加限制层数据
                    VariableMaintainer.CurrentDockableWindowOutput.AppendText(resourceManager.GetString("String171") +
                            VariableMaintainer.RestrictLayerName + ".....\n");
                    VariableMaintainer.RestrictImage = ArcGISOperator.ReadRaster(ArcGISOperator.GetRasterLayerByName(
                            VariableMaintainer.RestrictLayerName), -9999f);

                    //最后用模拟起始时刻影像创建模拟影像
                    string dateTime = GeneralOpertor.GetDataTimeFullString(DateTime.Now);
                    string rasterName = "sim" + dateTime + ".img";
                    ArcMap.Application.StatusBar.set_Message(0, resourceManager.GetString("String37"));
                    VariableMaintainer.CurrentDockableWindowOutput.AppendText(resourceManager.GetString("String37") + rasterName + ".....\n");

                    IRasterLayer simulationStartImageLayer = ArcGISOperator.GetRasterLayerByName(
                        VariableMaintainer.CurrentFormANNCAWizard.SimulationStartImageName);
                    IWorkspaceFactory workspaceFactory = new RasterWorkspaceFactoryClass();
                    IWorkspace workspace = workspaceFactory.OpenFromFile(VariableMaintainer.CurrentFormANNCAWizard.OutputFolder, 0);
                    ISaveAs2 saveAs2 = (ISaveAs2)simulationStartImageLayer.Raster;
                    saveAs2.SaveAs(rasterName, workspace, "IMAGINE Image");
                    IRasterLayer simulationImageLayer = new RasterLayerClass();
                    simulationImageLayer.CreateFromFilePath(VariableMaintainer.CurrentFormANNCAWizard.OutputFolder + @"\" + rasterName);
                    simulationImageLayer.Renderer = simulationStartImageLayer.Renderer;
                    ArcGISOperator.FoucsMap.AddLayer((ILayer)simulationImageLayer);
                    VariableMaintainer.CurrentFormANNCAWizard.SimulationImage = ArcGISOperator.ReadRaster(simulationImageLayer, -9999f);
                    VariableMaintainer.CurrentFormANNCAWizard.SimulationLayerName = rasterName;

                    ANNCA annCA = new ANNCA();
                    annCA.DockableWindowGraphy = VariableMaintainer.CurrentDockableWindowGraphy;
                    annCA.DockableWindowOutput = VariableMaintainer.CurrentDockableWindowOutput;

                    thread = new Thread(new ThreadStart(annCA.DoSimulation));
                }
                else if (VariableMaintainer.CurrentModel == EnumCurrentModel.Simulation_CA_DT)
                {
                    //今后应使用父类和接口进行重构
                    //首先读取模拟起始时刻和/或终止时刻影像    
                    ArcMap.Application.StatusBar.set_Message(0, resourceManager.GetString("String12"));
                    VariableMaintainer.CurrentDockableWindowOutput.AppendText(resourceManager.GetString("String12") +
                                VariableMaintainer.CurrentFormDTCAWizard.SimulationStartImageName + ".....\n");
                    VariableMaintainer.CurrentFormDTCAWizard.SimulationStartImage =
                        ArcGISOperator.ReadRasterAndGetMetaData(ArcGISOperator.GetRasterLayerByName(
                        VariableMaintainer.CurrentFormDTCAWizard.SimulationStartImageName), out structRasterMetaData);
                    //VariableMaintainer.CurrentDTCAWizard.CurrentStructRasterMetaData = structRasterMetaData;
                    if (VariableMaintainer.CurrentFormDTCAWizard.SimulationEndImageName != "")
                    {
                        ArcMap.Application.StatusBar.set_Message(0, resourceManager.GetString("String13"));
                        VariableMaintainer.CurrentDockableWindowOutput.AppendText(resourceManager.GetString("String13") +
                            VariableMaintainer.CurrentFormLogisticCAWizard.SimulationEndImageName + ".....\n");
                        VariableMaintainer.CurrentFormDTCAWizard.SimulationEndImage = ArcGISOperator.ReadRaster(ArcGISOperator.GetRasterLayerByName(
                                VariableMaintainer.CurrentFormDTCAWizard.SimulationEndImageName), -9999f);
                    }
                    //添加限制层数据
                    VariableMaintainer.CurrentDockableWindowOutput.AppendText(resourceManager.GetString("String171") +
                            VariableMaintainer.RestrictLayerName + ".....\n");
                    VariableMaintainer.RestrictImage = ArcGISOperator.ReadRaster(ArcGISOperator.GetRasterLayerByName(
                            VariableMaintainer.RestrictLayerName), -9999f);

                    //最后用模拟起始时刻影像创建模拟影像
                    string dateTime = GeneralOpertor.GetDataTimeFullString(DateTime.Now);
                    string rasterName = "sim" + dateTime + ".img";
                    ArcMap.Application.StatusBar.set_Message(0, resourceManager.GetString("String37"));
                    VariableMaintainer.CurrentDockableWindowOutput.AppendText(resourceManager.GetString("String37") + rasterName + ".....\n");

                    IRasterLayer simulationStartImageLayer = ArcGISOperator.GetRasterLayerByName(
                        VariableMaintainer.CurrentFormDTCAWizard.SimulationStartImageName);
                    IWorkspaceFactory workspaceFactory = new RasterWorkspaceFactoryClass();
                    IWorkspace workspace = workspaceFactory.OpenFromFile(VariableMaintainer.CurrentFormDTCAWizard.OutputFolder, 0);
                    ISaveAs2 saveAs2 = (ISaveAs2)simulationStartImageLayer.Raster;
                    saveAs2.SaveAs(rasterName, workspace, "IMAGINE Image");
                    IRasterLayer simulationImageLayer = new RasterLayerClass();
                    simulationImageLayer.CreateFromFilePath(VariableMaintainer.CurrentFormDTCAWizard.OutputFolder + @"\" + rasterName);
                    simulationImageLayer.Renderer = simulationStartImageLayer.Renderer;
                    ArcGISOperator.FoucsMap.AddLayer((ILayer)simulationImageLayer);
                    VariableMaintainer.CurrentFormDTCAWizard.SimulationImage = ArcGISOperator.ReadRaster(simulationImageLayer, -9999f);
                    VariableMaintainer.CurrentFormDTCAWizard.SimulationLayerName = rasterName;

                    DecisionTreeCA dtCA = new DecisionTreeCA();
                    dtCA.DockableWindowGraphy = VariableMaintainer.CurrentDockableWindowGraphy;
                    dtCA.DockableWindowOutput = VariableMaintainer.CurrentDockableWindowOutput;

                    thread = new Thread(new ThreadStart(dtCA.DoSimulation));
                }
                else if (VariableMaintainer.CurrentModel == EnumCurrentModel.Optimization_Area)
                {
                    if (!VariableMaintainer.IsACOUtilitySet || !VariableMaintainer.IsACOParametersSet)
                    {
                        MessageBox.Show(resourceManager.GetString("String166"), resourceManager.GetString("String2"),
                            MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    AreaOptimizationACO areaOptimizationACO = new AreaOptimizationACO();
                    areaOptimizationACO.Initialize(VariableMaintainer.CurrentStructACOParameters, 1, false,
                        VariableMaintainer.CurrentDockableWindowOutput, VariableMaintainer.CurrentDockableWindowGraphy, true);

                    thread = new Thread(new ThreadStart(areaOptimizationACO.Run));
                }
                else
                {
                    MessageBox.Show(resourceManager.GetString("String168"), resourceManager.GetString("String2"),
                            MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                VariableMaintainer.SimulationThread = thread;
                thread.SetApartmentState(ApartmentState.STA);
                thread.Start();

                VariableMaintainer.CurrentTimer.Interval = 500;
                VariableMaintainer.CurrentTimer.Tick += new EventHandler(t_Tick);
                VariableMaintainer.CurrentTimer.Enabled = true;
                VariableMaintainer.CurrentTimer.Start();

                ArcMap.Application.CurrentTool = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void t_Tick(object sender, EventArgs e)
        {
            try
            {
                if (VariableMaintainer.IsNeedRefresh)
                {
                    string simulationLayerName = VariableMaintainer.CurrentFoucsMap.get_Layer(0).Name;
                    IRasterLayer simulationImageLayer = new RasterLayerClass();
                    if (VariableMaintainer.CurrentModel == EnumCurrentModel.Simulation_CA_LogisticRegression)
                    {
                        simulationImageLayer.CreateFromFilePath(VariableMaintainer.CurrentFormLogisticCAWizard.OutputFolder + @"\" + simulationLayerName);
                        ArcGISOperator.WriteRaster(simulationImageLayer, VariableMaintainer.CurrentFormLogisticCAWizard.SimulationImage);
                    }
                    else if (VariableMaintainer.CurrentModel == EnumCurrentModel.Simulation_CA_ANN)
                    {
                        simulationImageLayer.CreateFromFilePath(VariableMaintainer.CurrentFormANNCAWizard.OutputFolder + @"\" + simulationLayerName);
                        ArcGISOperator.WriteRaster(simulationImageLayer, VariableMaintainer.CurrentFormANNCAWizard.SimulationImage);
                    }
                    else if (VariableMaintainer.CurrentModel == EnumCurrentModel.Simulation_CA_DT)
                    {
                        simulationImageLayer.CreateFromFilePath(VariableMaintainer.CurrentFormDTCAWizard.OutputFolder + @"\" + simulationLayerName);
                        ArcGISOperator.WriteRaster(simulationImageLayer, VariableMaintainer.CurrentFormDTCAWizard.SimulationImage);
                    }

                    IRasterLayer l = ArcGISOperator.GetRasterLayerByName(simulationLayerName);
                    IRaster2 raster = simulationImageLayer.Raster as IRaster2;
                    IPnt fromPnt = new PntClass();
                    fromPnt.SetCoords(0, 0);
                    IPnt blockSize = new PntClass();
                    blockSize.SetCoords(simulationImageLayer.ColumnCount, simulationImageLayer.RowCount);
                    IPixelBlock pixelBlock = ((IRaster)raster).CreatePixelBlock(blockSize);
                    l.Raster.Read(fromPnt, pixelBlock);
                    ((IRasterEdit)l.Raster).Refresh();

                    IActiveView activeView = VariableMaintainer.CurrentFoucsMap as IActiveView;
                    activeView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, null);
                    VariableMaintainer.IsNeedRefresh = false;
                }

                if (VariableMaintainer.IsSimulationFinished)
                {
                    VariableMaintainer.CurrentTimer.Stop();
                    VariableMaintainer.IsSimulationFinished = false;
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
        }

        protected override void OnUpdate()
        {
            Enabled = ArcMap.Application != null;
        }
    }
}
