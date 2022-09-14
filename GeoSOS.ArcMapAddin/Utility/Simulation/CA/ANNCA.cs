using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using GeoSOS.CommonLibrary.Struct;
using GeoSOS.ArcMapAddIn;
using System.Resources;
using System.Windows.Forms;
using GeoSOS.CommonLibrary;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesRaster;
using Accord.Neuro;
using System.Data;
using GeoSOS.ArcMapAddIn.UI.Forms;

namespace GeoSOS.ArcMapAddIn.Utility.CA
{
    public class ANNCA : IGeoSimulation
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
            try
            {
                //0准备开始模拟
                ResourceManager resourceManager = VariableMaintainer.CurrentResourceManager;
                FormANNCAWizard formANNCAWizard = VariableMaintainer.CurrentFormANNCAWizard;
                Random random = new Random();
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();
                //0.1获得数据
                int rowCount = formANNCAWizard.CurrentStructRasterMetaData.RowCount;
                int columnCount = formANNCAWizard.CurrentStructRasterMetaData.ColumnCount;
                float[,] simulationStartImage = formANNCAWizard.SimulationStartImage;
                float[,] simulationEndImage = formANNCAWizard.SimulationEndImage;
                float[,] simulationImage = formANNCAWizard.SimulationImage;

                //0.2计算初始每种土地利用类型的单元数量
                List<StructLanduseInfoAndCount> listLanduseInfoAndCount;   //记录每种土地利用类型的单元数
                listLanduseInfoAndCount = new List<StructLanduseInfoAndCount>();
                StructLanduseInfoAndCount landuseInfoAndCount;
                foreach (StructLanduseInfo structLanduseInfo in formANNCAWizard.LandUseClassificationInfo.AllTypes)
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
                dockableWindowOutput.AppendText(resourceManager.GetString("String43"));
                Application.DoEvents();
                //0.4绘制初始的图表
                List<string> listPointPairListName = new List<string>();
                List<string> notToDrawList = new List<string>();
                notToDrawList.Add(resourceManager.GetString("String44"));
                dockableWindowGraphy.CreateGraph(listLanduseInfoAndCount, notToDrawList, out listPointPairListName);
                dockableWindowGraphy.RefreshGraph();

                int convertedCellCount = 0; //模拟中总共转换的元胞数量
                int randomRow, randomColumn;    //Monte Carlo方法选取的随机行和随机列
                int convertCountInOneIteration = formANNCAWizard.ConvertCount / formANNCAWizard.Iterations;   //每次迭代应转换的数量
                int convertCountOnce = 0;   //每次迭代已经转换的数量
                float oldValue, newValue;   //每次转换前土地利用类型的新值和旧值
                int iteration = 0;    //迭代的次数
                List<float> listLanduseValues = new List<float>();  //记录土地利用类型值的List
                for (int j = 0; j < formANNCAWizard.LandUseClassificationInfo.AllTypesCount; j++)
                    listLanduseValues.Add(formANNCAWizard.LandUseClassificationInfo.AllTypes[j].LanduseTypeValue);
                int neiWindowSize = formANNCAWizard.NeighbourWindowSize;
                int inputNeuronsCount = formANNCAWizard.InputNeuronsCount;
                int outputNeuronsCount = formANNCAWizard.OutputNeuronsCount;
                //2.开始进行转换 
                while (convertedCellCount < formANNCAWizard.ConvertCount)
                {
                    convertCountOnce = 0;

                    //2.3选择元胞与随机比较进行转换               
                    //完成一次迭代
                    while (convertCountOnce < convertCountInOneIteration)
                    {
                        //随机选择一个栅格进行计算
                        randomRow = random.Next(0, rowCount);
                        randomColumn = random.Next(0, columnCount);

                        //计算逻辑为：
                        //这里需要首先获取当前元胞的输入值
                        //然后通过神经网络计算每种土地利用类型的概率，乘以随机量
                        //然后选出最高的概率
                        //再查看是否是同样的土地利用类型，如果不是则转变
                        //如果不是并且概率大于一个值，则进行转变（目前先不用）  

                        //如果是空值，则不进行计算
                        if (simulationImage[randomRow, randomColumn] == -9999f)
                            continue;

                        //添加限制层数据
                        if (VariableMaintainer.RestrictImage[randomRow, randomColumn] == 1)
                            continue;

                        double[] tempInputsArray = new double[inputNeuronsCount];
                        double[] tempOutputsArray = new double[outputNeuronsCount];
                        //获取该栅格的空间变量值
                        for (int i = 0; i < formANNCAWizard.ListVaribaleImages.Count; i++)
                            tempInputsArray[i] = formANNCAWizard.ListVaribaleImages[i][randomRow, randomColumn];
                        //获取该栅格的邻域值
                        NeighbourOperator neighbourOperator = new NeighbourOperator();
                        float[] counts = neighbourOperator.GetNeighbourCount(simulationImage, randomRow, randomColumn, neiWindowSize,
                           rowCount, columnCount, formANNCAWizard.LandUseClassificationInfo.AllTypesCount, listLanduseValues);
                        for (int z = 0; z < counts.Length; z++)
                            tempInputsArray[formANNCAWizard.ListVaribaleImages.Count + z] = counts[z] / (neiWindowSize * neiWindowSize-1);
                        //获取该栅格的土地利用类型
                        tempInputsArray[inputNeuronsCount - 1] = simulationImage[randomRow, randomColumn];

                        //计算神经网络输出的概率值
                        double[] output = formANNCAWizard.ANN.ANNActivationNetwork.Compute(tempInputsArray);
                        double maxOutput = 0;
                        int maxIndex = -1;
                        Random r = new Random(1);
                        double gamma;
                        double disturbance;
                        double probability;
                        double[] probabilityArray = new double[output.Length];
                        for (int k = 0; k < output.Length; k++)
                        {
                            gamma = random.NextDouble();
                            disturbance = 1 + Math.Pow((-Math.Log(gamma)), formANNCAWizard.Delta);
                            probabilityArray[k] = disturbance * output[k];
                            if (maxOutput < probabilityArray[k])
                            {
                                maxOutput = probabilityArray[k];
                                maxIndex = k;
                            }
                        }
                        probability = maxOutput;  //计算最高值的最终概率       

                        //未使用邻域因素模拟较为散，所以加入邻域因素。但可能导致难以达到需要的转换数量，模拟时间过长。
                        double neiValue = 0f;
                        for (int t = 0; t < listLanduseValues.Count; t++)
                        {
                            if (listLanduseValues[t] == formANNCAWizard.LandUseClassificationInfo.UrbanValues[0].LanduseTypeValue)
                            {
                                neiValue = Convert.ToDouble(counts[t]) / (neiWindowSize* neiWindowSize-1);
                                break;
                            }
                        }
                        probability = probability * neiValue;

                        //得到应转变为的土地利用类型值，以及当前土地利用类型值
                        newValue = formANNCAWizard.LandUseClassificationInfo.AllTypes[maxIndex].LanduseTypeValue;
                        oldValue = simulationImage[randomRow, randomColumn];
                        //oldValue有时取到0值等不正确的值
                        bool isOldValueCorrect = false;
                        for (int w = 0; w < formANNCAWizard.LandUseClassificationInfo.AllTypes.Count; w++)
                        {
                            if (oldValue == formANNCAWizard.LandUseClassificationInfo.AllTypes[w].LanduseTypeValue)
                                isOldValueCorrect=true;
                        }
                        if (!isOldValueCorrect)
                            continue;
                        //根据转换矩阵进行判断是否可以转换
                        DataTable dtMatrix = formANNCAWizard.DTMatrix;
                        int oldValueIndex = -1;
                        for (int k = 0; k < formANNCAWizard.LandUseClassificationInfo.AllTypesCount; k++)
                        {
                            if (formANNCAWizard.LandUseClassificationInfo.AllTypes[k].LanduseTypeValue == oldValue)
                            {
                                oldValueIndex = k;
                                break;
                            }
                        }
                        string canConvert = dtMatrix.Rows[oldValueIndex][maxIndex].ToString();
                        if (canConvert == "0")
                            continue;

                        //与设定的阈值进行比较
                        double convertThreshold = formANNCAWizard.ConvertThreshold;
                        if ((oldValue != newValue) && (probability >= convertThreshold))
                        {
                            simulationImage[randomRow, randomColumn] = newValue;
                            CommonLibrary.GeneralOpertor.ChangeLandUseCount(newValue, oldValue, listLanduseInfoAndCount);
                            //以城市用地的转变量为总转变量的标准
                            for (int w = 0; w < formANNCAWizard.LandUseClassificationInfo.UrbanValues.Count; w++)
                            {
                                if (newValue == formANNCAWizard.LandUseClassificationInfo.UrbanValues[w].LanduseTypeValue)
                                {
                                    convertCountOnce++;
                                    convertedCellCount++;
                                }
                            }
                            //System.Diagnostics.Debug.WriteLine(convertedCellCount + " - Old: " + oldValue + " New: " + newValue);
                        }
                    }
                    iteration++;
                    System.Diagnostics.Debug.WriteLine("iteration: " + iteration);

                    //2.4.刷新外部界面并输出中间结果数据
                    if (convertedCellCount == 1 || (iteration % formANNCAWizard.RefreshInterval == 0 && convertedCellCount != 0))
                    {
                        //刷新图像
                        formANNCAWizard.SimulationImage = simulationImage;
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
                    if (formANNCAWizard.IsOutput && (iteration % formANNCAWizard.OutputImageInterval == 0))
                    {
                        GeneralOpertor.WriteDataFloat(formANNCAWizard.OutputFolder + @"\" + GeneralOpertor.GetNowString()
                        + "_ann_iterate_" + iteration.ToString() + @".txt", simulationImage, rowCount, columnCount);
                    }
                }

                //3.完成模拟，输出结果。
                stopWatch.Stop();
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
                        VariableMaintainer.CurrentFormANNCAWizard.OutputFolder, 0) as IRasterWorkspace;
                IRasterDataset rasterDataset = rasterWorkspace.OpenRasterDataset(VariableMaintainer.CurrentFoucsMap.get_Layer(0).Name);
                IRasterDatasetEdit3 rasterDatasetEdit3 = rasterDataset as IRasterDatasetEdit3;
                rasterDatasetEdit3.BuildAttributeTable();
                IRasterDataset3 rasterDataset3 = rasterDataset as IRasterDataset3;
                rasterDataset3.Refresh();

                if (formANNCAWizard.SimulationEndImageName != "")
                {
                    //GeneralOpertor.WriteDataFloat(formLogisticCAWizard.OutputFolder + @"\CA_ANN_Reslut" + GeneralOpertor.GetNowString() + ".txt", 
                    //    simulationImage, rowCount,columnCount);

                    DataTable dtMatrixNumber = GeneralOpertor.GetMultiTypesMatrix(
                        simulationImage, simulationEndImage, rowCount, columnCount, formANNCAWizard.LandUseClassificationInfo);
                    double overallAccuracy = 0d;
                    double kappa = 0d;
                    GeneralOpertor.GetMultiTypesAccuracy(dtMatrixNumber, ref overallAccuracy, ref  kappa, formANNCAWizard.LandUseClassificationInfo);
                    FormConfusionMatrix formConfusionMatrix = new FormConfusionMatrix();
                    formConfusionMatrix.DataTableValues = dtMatrixNumber;
                    DataTable dtMatrixPercent = dtMatrixNumber.Clone();
                    GeneralOpertor.CopyDataTableValues(dtMatrixNumber, dtMatrixPercent);
                    formConfusionMatrix.DataTablePercents = dtMatrixPercent;
                    formConfusionMatrix.DataGridViewConfusionMatrix.DataSource = dtMatrixNumber;
                    formConfusionMatrix.LabelOverallAccuracy.Text = (overallAccuracy * 100).ToString("0.00") + " %";
                    formConfusionMatrix.LabelKappa.Text = kappa.ToString("0.000");

                    float[] fomValues = GeneralOpertor.GetMultiFoMAccuracy(simulationStartImage, simulationEndImage, simulationImage, rowCount, columnCount);
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
                    formConfusionMatrix.Text = resourceManager.GetString("String105") + " - " + formANNCAWizard.SimulationLayerName;
                    formConfusionMatrix.ShowDialog();
                }
                dockableWindowOutput.AppendText("-------------------------------------------");
                dockableWindowOutput.AppendText("\n");
                dockableWindowOutput.AppendText("\n");
                dockableWindowOutput.ScrollTextbox();
                Application.DoEvents();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
