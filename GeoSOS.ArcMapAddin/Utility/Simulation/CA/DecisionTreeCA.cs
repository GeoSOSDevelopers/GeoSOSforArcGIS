using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Resources;
using System.Windows.Forms;
using System.Diagnostics;
using GeoSOS.CommonLibrary.Struct;
using GeoSOS.ArcMapAddIn;
using GeoSOS.CommonLibrary;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesRaster;
using Accord.MachineLearning.DecisionTrees;
using System.Data;
using GeoSOS.ArcMapAddIn.UI.Forms;

namespace GeoSOS.ArcMapAddIn.Utility.CA
{
    public class DecisionTreeCA : IGeoSimulation
    {
        private DockableWindowGraphy dockableWindowGraphy;
        private DockableWindowOutput dockableWindowOutput;

        public DockableWindowGraphy DockableWindowGraphy
        {
            set { dockableWindowGraphy = value; }
        }

        public DockableWindowOutput DockableWindowOutput
        {
            set { dockableWindowOutput = value; }
        }

        public void DoSimulation()
        {
            //0准备开始模拟
            ResourceManager resourceManager = VariableMaintainer.CurrentResourceManager;
            FormDTCAWizard formDTCAWizard = VariableMaintainer.CurrentFormDTCAWizard;
            Random random = new Random();
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            //0.1获得数据
            int rowCount = formDTCAWizard.CurrentStructRasterMetaData.RowCount;
            int columnCount = formDTCAWizard.CurrentStructRasterMetaData.ColumnCount;
            float[,] simulationStartImage = formDTCAWizard.SimulationStartImage;
            float[,] simulationEndImage = formDTCAWizard.SimulationEndImage;
            float[,] simulationImage = formDTCAWizard.SimulationImage;           

            //0.2计算初始每种土地利用类型的单元数量
            List<StructLanduseInfoAndCount> listLanduseInfoAndCount;   //记录每种土地利用类型的单元数
            listLanduseInfoAndCount = new List<StructLanduseInfoAndCount>();
            StructLanduseInfoAndCount landuseInfoAndCount;
            foreach (StructLanduseInfo structLanduseInfo in formDTCAWizard.LandUseClassificationInfo.AllTypes)
            {
                landuseInfoAndCount = new StructLanduseInfoAndCount();
                landuseInfoAndCount.structLanduseInfo = structLanduseInfo;
                landuseInfoAndCount.LanduseTypeCount = 0;
                listLanduseInfoAndCount.Add(landuseInfoAndCount);
            }
            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < columnCount; j++)
                    CommonLibrary.GeneralOpertor.ChangeLandUseCount(simulationStartImage[i, j], -10000, listLanduseInfoAndCount);
            }

            //0.3显示输出结果窗体和图表窗体
            dockableWindowGraphy.GraphTitle = resourceManager.GetString("String40");
            dockableWindowGraphy.XAxisTitle = resourceManager.GetString("String41");
            dockableWindowGraphy.YAxisTitle = resourceManager.GetString("String42");
            dockableWindowOutput.AppendText("\n");
            dockableWindowOutput.AppendText(resourceManager.GetString("String43") );
            Application.DoEvents();
            //0.4绘制初始的图表
            List<string> listPointPairListName = new List<string>();
            List<string> notToDrawList = new List<string>();
            notToDrawList.Add(resourceManager.GetString("String44"));
            dockableWindowGraphy.CreateGraph(listLanduseInfoAndCount, notToDrawList, out listPointPairListName);
            dockableWindowGraphy.RefreshGraph();

            int convertedCellCount = 0; //模拟中总共转换的元胞数量
            int randomRow, randomColumn;    //Monte Carlo方法选取的随机行和随机列
            int convertCountInOneIteration = formDTCAWizard.ConvertCount / formDTCAWizard.Iterations;   //每次迭代应转换的数量
            int convertCountOnce = 0;   //每次迭代已经转换的数量
            float oldValue, newValue;   //每次转换前土地利用类型的新值和旧值
            int iteration = 0;    //迭代的次数

            //float gamma = 1f / formDTCAWizard.Iterations;
            //float gamma = 0.4f;
            DecisionTree decisionTree = formDTCAWizard.CurrentDecisionTree;
            List<float> listUrbanValues = new List<float>();
            for (int j = 0; j < formDTCAWizard.LandUseClassificationInfo.UrbanValues.Count; j++)
            {
                listUrbanValues.Add(formDTCAWizard.LandUseClassificationInfo.UrbanValues[j].LanduseTypeValue);
            }
            int neiWindowSize = formDTCAWizard.NeighbourWindowSize;

            try
            {
                //2.开始进行转换 
                while (convertedCellCount < formDTCAWizard.ConvertCount)
                {
                    convertCountOnce = 0;

                    while (convertCountOnce < convertCountInOneIteration)
                    {
                        //随机选择一个栅格进行计算
                        randomRow = random.Next(0, rowCount);
                        randomColumn = random.Next(0, columnCount);

                        //计算逻辑为：
                        //这里需要首先获取当前元胞的输入值
                        //然后通过决策树计算是否发生转变
                        //然后看随机数是否小于等于预定值，则进行转变  

                        //如果是空值，则不进行计算
                        if (simulationImage[randomRow, randomColumn] == -9999f)
                            continue;
                        //如果模拟影像该栅格是空值，也不进行计算
                        if (formDTCAWizard.ListVaribaleImages[formDTCAWizard.ListVaribaleImages.Count - 1][randomRow, randomColumn] == -9999f)
                            continue;
                        //如果已经是城市用地，则不计算
                        if (simulationImage[randomRow, randomColumn] == formDTCAWizard.LandUseClassificationInfo.UrbanValues[0].LanduseTypeValue)
                            continue;

                        double[] tempInputsArray = new double[formDTCAWizard.ListVaribaleImages.Count];
                        //获取该栅格的各空间变量值和其他输入变量
                        for (int i = 0; i < formDTCAWizard.ListVaribaleImages.Count; i++)
                            tempInputsArray[i] = formDTCAWizard.ListVaribaleImages[i][randomRow, randomColumn];
                        //获取该栅格的邻域中城市用地数量
                        tempInputsArray[formDTCAWizard.ListVaribaleImages.Count - 2] =
                            CommonLibrary.GeneralOpertor.GetNeighbors(randomRow, randomColumn, simulationImage,
                            rowCount, columnCount, neiWindowSize, listUrbanValues);
                        //获取该栅格当前的土地利用类型值
                        tempInputsArray[formDTCAWizard.ListVaribaleImages.Count - 1] = simulationImage[randomRow, randomColumn];

                        //计算决策树输出的转换值
                        int output = formDTCAWizard.CurrentDecisionTree.Compute(tempInputsArray);
                        //如果计算为不转换，则不进行转换
                        if (output == 0)
                            continue;
                        //如果可以转换为城市用地
                        if (!formDTCAWizard.LandUseClassificationInfo.IsExistInConvertValue(simulationImage[randomRow, randomColumn]))
                            continue;

                        Random r = new Random(1);
                        double convertThreshold = random.NextDouble();
                        //得到应转变为的土地利用类型值，以及当前土地利用类型值
                        newValue = formDTCAWizard.LandUseClassificationInfo.UrbanValues[0].LanduseTypeValue;
                        oldValue = simulationImage[randomRow, randomColumn];
                        //未使用邻域因素模拟较为散，所以加入邻域因素
                        double neighbourValue = tempInputsArray[formDTCAWizard.ListVaribaleImages.Count - 2] / (neiWindowSize * neiWindowSize-1);
                        //double probability = convertThreshold * neighbourValue;

                        //if ((oldValue != newValue) && (convertThreshold <= gamma))
                        //if ((oldValue != newValue) && (probability >= gamma))
                        //20170619添加限制层数据
                        // if ((VariableMaintainer.RestrictImage[randomRow, randomColumn] == 2) || (VariableMaintainer.RestrictImage[randomRow, randomColumn] == 1))
                        //if (VariableMaintainer.RestrictImage[randomRow, randomColumn] == 1)
                        //    continue;

                        //引入随机因素，如果邻域因素大于随机数
                        if ((oldValue != newValue) && (neighbourValue > convertThreshold))
                        {
                            simulationImage[randomRow, randomColumn] = newValue;
                            CommonLibrary.GeneralOpertor.ChangeLandUseCount(newValue, oldValue, listLanduseInfoAndCount);
                            convertCountOnce++;
                            convertedCellCount++;
                            //System.Diagnostics.Debug.WriteLine(convertedCellCount + " - Old: " + oldValue + " New: " + newValue);
                        }
                    }
                    iteration++;

                    //2.4.刷新外部界面并输出中间结果数据
                    if (convertedCellCount == 1 || (iteration % formDTCAWizard.RefreshInterval == 0 && convertedCellCount != 0))
                    {
                        //刷新图像
                        formDTCAWizard.SimulationImage = simulationImage;
                        VariableMaintainer.IsNeedRefresh = true;

                        //刷新图表窗体
                        string landuseTypeName = "";
                        for (int k = 0; k < listLanduseInfoAndCount.Count; k++)
                        {
                            for (int l = 0; l < listPointPairListName.Count; l++)
                            {
                                if (System.Globalization.CultureInfo.CurrentCulture.Parent.Name == "zh-CHS")
                                    landuseTypeName = listLanduseInfoAndCount[k].structLanduseInfo.LanduseTypeChsName;
                                else if (System.Globalization.CultureInfo.CurrentCulture.Parent.Name == "zh-CHT")
                                    landuseTypeName = listLanduseInfoAndCount[k].structLanduseInfo.LanduseTypeChtName;
                                else
                                    landuseTypeName = listLanduseInfoAndCount[k].structLanduseInfo.LanduseTypeEnName;
                                if (landuseTypeName == listPointPairListName[l])
                                {
                                    dockableWindowGraphy.UpdateData(iteration, listLanduseInfoAndCount[k].LanduseTypeCount, l);
                                }
                            }
                        }
                        dockableWindowGraphy.RefreshGraph();

                        //刷新输出结果窗体
                        dockableWindowOutput.AppendText("\n");
                        dockableWindowOutput.AppendText(resourceManager.GetString("String45") + iteration.ToString()
                            + resourceManager.GetString("String46"));
                        dockableWindowOutput.AppendText("\n");
                        dockableWindowOutput.AppendText(resourceManager.GetString("String47") + convertedCellCount.ToString());
                        dockableWindowOutput.AppendText("\n");
                        dockableWindowOutput.ScrollTextbox();
                        Application.DoEvents();
                    }
                    //输出中间结果
                    if (formDTCAWizard.IsOutput && (iteration % formDTCAWizard.OutputImageInterval == 0))
                    {
                        GeneralOpertor.WriteDataFloat(formDTCAWizard.OutputFolder + @"\" + GeneralOpertor.GetNowString()
                        + "_ann_iterate_" + iteration.ToString() + @".txt", simulationImage, rowCount, columnCount);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("DTCA: " + ex.Message);
            }

            //3.完成模拟，输出结果。
            stopWatch.Stop();
            //isFinished = true;
            VariableMaintainer.IsSimulationFinished = true;
            dockableWindowOutput.AppendText("\n");
            dockableWindowOutput.AppendText("\n");
            dockableWindowOutput.AppendText(resourceManager.GetString("String48"));
            dockableWindowOutput.AppendText("\n");
            dockableWindowOutput.AppendText(resourceManager.GetString("String49") +
                GeneralOpertor.GetElapsedTimeString(stopWatch.Elapsed));
            dockableWindowOutput.AppendText("\n");
            dockableWindowOutput.AppendText("\n");

            //修改结果栅格的属性表
            IWorkspaceFactory workspaceFactory = new RasterWorkspaceFactoryClass();
            IRasterWorkspace rasterWorkspace = workspaceFactory.OpenFromFile(
                    VariableMaintainer.CurrentFormDTCAWizard.OutputFolder, 0) as IRasterWorkspace;
            IRasterDataset rasterDataset = rasterWorkspace.OpenRasterDataset(VariableMaintainer.CurrentFoucsMap.get_Layer(0).Name);
            IRasterDatasetEdit3 rasterDatasetEdit3 = rasterDataset as IRasterDatasetEdit3;
            rasterDatasetEdit3.BuildAttributeTable();
            IRasterDataset3 rasterDataset3 = rasterDataset as IRasterDataset3;
            rasterDataset3.Refresh();

            if (formDTCAWizard.SimulationEndImageName != "")
            {
                //GeneralOpertor.WriteDataFloat(formLogisticCAWizard.OutputFolder + @"\CA_ANN_Reslut" + GeneralOpertor.GetNowString() + ".txt", 
                //    simulationImage, rowCount,columnCount);
                StructBinaryConfusionMatrix structConfusionMatrix = GeneralOpertor.GetBinaryAccuracy(
                    simulationImage, simulationEndImage, rowCount, columnCount, formDTCAWizard.LandUseClassificationInfo);
                string accuracyString = GeneralOpertor.GetBinaryAccuracyReportString(structConfusionMatrix, convertedCellCount);
                dockableWindowOutput.AppendText(accuracyString);

                DataTable dtMatrixNumber = GeneralOpertor.GetMultiTypesMatrix(
                    simulationImage, simulationEndImage, rowCount, columnCount, formDTCAWizard.LandUseClassificationInfo);
                double overallAccuracy = 0d;
                double kappa = 0d;
                GeneralOpertor.GetMultiTypesAccuracy(dtMatrixNumber, ref overallAccuracy, ref  kappa, formDTCAWizard.LandUseClassificationInfo);
                FormConfusionMatrix formConfusionMatrix = new FormConfusionMatrix();
                formConfusionMatrix.DataTableValues = dtMatrixNumber;
                DataTable dtMatrixPercent = dtMatrixNumber.Clone();
                GeneralOpertor.CopyDataTableValues(dtMatrixNumber, dtMatrixPercent);
                formConfusionMatrix.DataTablePercents = dtMatrixPercent;
                formConfusionMatrix.DataGridViewConfusionMatrix.DataSource = dtMatrixNumber;
                formConfusionMatrix.LabelOverallAccuracy.Text = (overallAccuracy * 100).ToString("0.00") + " %";
                formConfusionMatrix.LabelKappa.Text = kappa.ToString("0.000");

                float[] fomValues = GeneralOpertor.GetBinaryFoMAccuracy(simulationStartImage, simulationEndImage, simulationImage, 
                    rowCount, columnCount,formDTCAWizard.LandUseClassificationInfo.UrbanValues[0].LanduseTypeValue);
                formConfusionMatrix.LabelFoMValues.Text = "A: " + fomValues[0] + "\nB: " + fomValues[1] +
                    "\nC: " + fomValues[2] + "\nD: " + fomValues[3];
                formConfusionMatrix.LabelFoM.Text = fomValues[4].ToString("0.000");
                formConfusionMatrix.LabelPA.Text = fomValues[5].ToString("0.000");
                formConfusionMatrix.LabelUA.Text = fomValues[6].ToString("0.000");

                dockableWindowOutput.AppendText("\n");
                dockableWindowOutput.AppendText(resourceManager.GetString("String84"));
                dockableWindowOutput.AppendText("\n");
                dockableWindowOutput.AppendText(GeneralOpertor.WriteCoufusionMatrix(dtMatrixPercent));
                dockableWindowOutput.AppendText("\n");
                dockableWindowOutput.AppendText(resourceManager.GetString("String83"));
                dockableWindowOutput.AppendText("\n");
                dockableWindowOutput.AppendText(resourceManager.GetString("String85") + (overallAccuracy * 100).ToString("0.00") + " %");
                dockableWindowOutput.AppendText("\n");
                dockableWindowOutput.AppendText(resourceManager.GetString("String86") + kappa.ToString("0.000"));
                dockableWindowOutput.AppendText("\n");
                dockableWindowOutput.AppendText(resourceManager.GetString("String87") + fomValues[4].ToString("0.000"));
                dockableWindowOutput.AppendText("\n");
                dockableWindowOutput.AppendText(resourceManager.GetString("String88") + fomValues[5].ToString("0.000"));
                dockableWindowOutput.AppendText("\n");
                dockableWindowOutput.AppendText(resourceManager.GetString("String89") + fomValues[6].ToString("0.000"));
                dockableWindowOutput.AppendText("\n");
                formConfusionMatrix.Text = resourceManager.GetString("String105") + " - " + formDTCAWizard.SimulationLayerName;
                formConfusionMatrix.ShowDialog();
            }
            dockableWindowOutput.AppendText("-------------------------------------------");
            dockableWindowOutput.AppendText("\n");
            dockableWindowOutput.AppendText("\n");
            dockableWindowOutput.ScrollTextbox();
            Application.DoEvents();
        }
    }
}
