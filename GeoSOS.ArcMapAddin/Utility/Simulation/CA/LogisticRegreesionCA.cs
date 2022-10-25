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
using System.Data;
using GeoSOS.ArcMapAddIn.UI.Forms;

namespace GeoSOS.ArcMapAddIn.Utility.CA
{
    public class LogisticRegreesionCA : IGeoSimulation
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
            FormLogisticCAWizard formLogisticCAWizard = VariableMaintainer.CurrentFormLogisticCAWizard;
            Random random = new Random();
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            //0.1获得数据
            int rowCount = formLogisticCAWizard.CurrentStructRasterMetaData.RowCount;
            int columnCount = formLogisticCAWizard.CurrentStructRasterMetaData.ColumnCount;
            float[,] simulationStartImage = formLogisticCAWizard.SimulationStartImage;
            float[,] simulationEndImage = formLogisticCAWizard.SimulationEndImage;
            float[,] simulationImage = formLogisticCAWizard.SimulationImage;
            
            //0.2计算初始每种土地利用类型的单元数量
            List<StructLanduseInfoAndCount> listLanduseInfoAndCount;   //记录每种土地利用类型的单元数
            listLanduseInfoAndCount = new List<StructLanduseInfoAndCount>();
            StructLanduseInfoAndCount landuseInfoAndCount;
            foreach (StructLanduseInfo structLanduseInfo in formLogisticCAWizard.LandUseClassificationInfo.AllTypes)
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

            //1.计算Pg
            float[,] pg = new float[rowCount, columnCount];
            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < columnCount; j++)
                {
                    if (simulationStartImage[i, j] == formLogisticCAWizard.LandUseClassificationInfo.NullValue.LanduseTypeValue)
                        pg[i, j] = formLogisticCAWizard.LandUseClassificationInfo.NullValue.LanduseTypeValue;
                    else
                    {
                        pg[i, j] += Convert.ToSingle(formLogisticCAWizard.Coefficents[0]);
                        for (int k = 0; k < formLogisticCAWizard.ListVariableLayersName.Count; k++)
                        {
                            pg[i, j] += Convert.ToSingle(formLogisticCAWizard.Coefficents[k + 1]) *
                                formLogisticCAWizard.VaribaleImages[k][i, j];
                        }
                    }
                    pg[i, j] = 1 / (1 + Convert.ToSingle(Math.Exp(-1 * pg[i, j])));
                }
            }
            //GeneralOpertor.WriteDataFloat(formLogisticCAWizard.OutputFolder + @"\pg.txt", pg, rowCount, columnCount);

            int convertedCellCount = 0; //模拟中总共转换的元胞数量
            List<float> neighbours; //Moore领域的元胞
            float omega;    //扩散系数
            float conSuitable;  //适应度函数
            float maxPct;  //每次迭代时最大的Pct值
            float[,] pct;
            float[,] pst;
            float sumPtt = 0;   //Ptt的总和
            int randomRow, randomColumn;    //Monte Carlo方法选取的随机行和随机列
            int convertCountInOneIteration = formLogisticCAWizard.ConvertCount / formLogisticCAWizard.Iterations;   //每次迭代应转换的数量
            int convertCountOnce = 0;   //每次迭代已经转换的数量
            float oldValue, newValue;   //每次转换前土地利用类型的新值和旧值
            int[,] changeCells = new int[rowCount, columnCount]; //记录每次转换为城市的单元。
            int iteration = 0;    //迭代的次数

            //2.开始进行转换 
            while (convertedCellCount < formLogisticCAWizard.ConvertCount)
            {
                if (convertedCellCount % formLogisticCAWizard.OutputImageInterval == 0 && convertedCellCount != 0)
                    changeCells = new int[rowCount, columnCount];
                convertCountOnce = 0;

                //2.1每次迭代先得到Pct和MaxPct
                maxPct = float.MinValue;
                pct = new float[rowCount, columnCount];
                for (int i = 0; i < rowCount; i++)
                {
                    for (int j = 0; j < columnCount; j++)
                    {
                        neighbours = CommonLibrary.GeneralOpertor.GetMooreNeighbors(i, j, simulationImage, rowCount, columnCount);
                        omega = Convert.ToSingle(CommonLibrary.GeneralOpertor.GetUrbanCount(neighbours, formLogisticCAWizard.LandUseClassificationInfo.UrbanValues[0].LanduseTypeValue)) / (neighbours.Count - 1);
                        if (formLogisticCAWizard.LandUseClassificationInfo.IsExistInConvertValue(simulationImage[i, j]))
                            conSuitable = 1f;
                        else
                            conSuitable = 0f;
                        pct[i, j] = pg[i, j] * conSuitable * omega;
                        if (pct[i, j] > maxPct)
                            maxPct = pct[i, j];
                    }
                }
                //CommonOperator.WriteData(Application.StartupPath + @"\\OutputData\pct.txt", pct, structAsciiImageFileMetaData);

                //2.2再得到Pst 
                pst = new float[rowCount, columnCount];
                sumPtt = 0;
                //2.2.1先得到ptt和Ptt的总和
                for (int i = 0; i < rowCount; i++)
                {
                    for (int j = 0; j < columnCount; j++)
                    {
                        pst[i, j] = pct[i, j] * Convert.ToSingle(Math.Exp(-1f * formLogisticCAWizard.Delta * (1f - pct[i, j] / maxPct)));
                        sumPtt += pst[i, j];
                    }
                }
                for (int i = 0; i < rowCount; i++)
                {
                    for (int j = 0; j < columnCount; j++)
                    {
                        pst[i, j] = convertCountInOneIteration * pst[i, j] / sumPtt;
                    }
                }
                //CommonOperator.WriteData(Application.StartupPath + @"\\OutputData\pst.txt", pst, structAsciiImageFileMetaData);
 
                //2.3选择元胞与随机比较进行转换               
                //完成一次迭代
                while (convertCountOnce < convertCountInOneIteration)
                {
                    randomRow = random.Next(0, rowCount);
                    randomColumn = random.Next(0, columnCount);
                    if (pst[randomRow, randomColumn] > Convert.ToSingle(random.NextDouble()))
                    {
                        //添加限制层数据
                        if (VariableMaintainer.RestrictImage != null)
                        {
                            if (VariableMaintainer.RestrictImage[randomRow, randomColumn] == 1)
                                continue;
                        }

                        oldValue = simulationImage[randomRow, randomColumn];
                        newValue = formLogisticCAWizard.LandUseClassificationInfo.UrbanValues[0].LanduseTypeValue;
                        simulationImage[randomRow, randomColumn] = newValue;
                        CommonLibrary.GeneralOpertor.ChangeLandUseCount(newValue, oldValue, listLanduseInfoAndCount);
                        convertCountOnce++;
                        convertedCellCount++;
                        changeCells[randomRow, randomColumn] = 1;
                    }
                }
                iteration++;

                //2.4.刷新外部界面并输出中间结果数据
                if (convertedCellCount == 1 || (iteration % formLogisticCAWizard.RefreshInterval == 0 && convertedCellCount != 0))
                {
                    //刷新图像
                    formLogisticCAWizard.SimulationImage = simulationImage;
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
                if (formLogisticCAWizard.IsOutput && (iteration % formLogisticCAWizard.OutputImageInterval == 0))
                {
                    GeneralOpertor.WriteDataFloat(formLogisticCAWizard.OutputFolder + @"\" + GeneralOpertor.GetNowString()
                    + "_lr_iterate_" + iteration.ToString() + @".txt", simulationImage, rowCount, columnCount);
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
                    VariableMaintainer.CurrentFormLogisticCAWizard.OutputFolder, 0) as IRasterWorkspace;
            IRasterDataset rasterDataset = rasterWorkspace.OpenRasterDataset(VariableMaintainer.CurrentFoucsMap.get_Layer(0).Name);
            IRasterDatasetEdit3 rasterDatasetEdit3 = rasterDataset as IRasterDatasetEdit3;
            rasterDatasetEdit3.BuildAttributeTable();
            IRasterDataset3 rasterDataset3 = rasterDataset as IRasterDataset3;
            rasterDataset3.Refresh();

            if (formLogisticCAWizard.SimulationEndImageName != "")
            {
                //GeneralOpertor.WriteDataFloat(formLogisticCAWizard.OutputFolder + @"\CA_LR_Reslut" + GeneralOpertor.GetNowString() + ".txt",
                //    simulationImage, rowCount, columnCount);

                StructBinaryConfusionMatrix structConfusionMatrix = GeneralOpertor.GetBinaryAccuracy(
                    simulationImage, simulationEndImage, rowCount, columnCount, formLogisticCAWizard.LandUseClassificationInfo);
                string accuracyString = GeneralOpertor.GetBinaryAccuracyReportString(structConfusionMatrix, convertedCellCount);
                dockableWindowOutput.AppendText(accuracyString);

                DataTable dtMatrixNumber = GeneralOpertor.GetMultiTypesMatrix(
                    simulationImage, simulationEndImage, rowCount, columnCount, formLogisticCAWizard.LandUseClassificationInfo);
                double overallAccuracy = 0d;
                double kappa = 0d;
                GeneralOpertor.GetMultiTypesAccuracy(dtMatrixNumber, ref overallAccuracy, ref  kappa, formLogisticCAWizard.LandUseClassificationInfo);
                 FormConfusionMatrix formConfusionMatrix = new FormConfusionMatrix();
                formConfusionMatrix.DataTableValues = dtMatrixNumber;
                DataTable dtMatrixPercent = dtMatrixNumber.Clone();
                GeneralOpertor.CopyDataTableValues(dtMatrixNumber, dtMatrixPercent);
                formConfusionMatrix.DataTablePercents = dtMatrixPercent;
                formConfusionMatrix.DataGridViewConfusionMatrix.DataSource = dtMatrixNumber;
                formConfusionMatrix.LabelOverallAccuracy.Text = (overallAccuracy * 100).ToString("0.00") + " %";
                formConfusionMatrix.LabelKappa.Text = kappa.ToString("0.000");

                float[] fomValues = GeneralOpertor.GetBinaryFoMAccuracy(simulationStartImage, simulationEndImage, simulationImage, 
                    rowCount, columnCount,formLogisticCAWizard.LandUseClassificationInfo.UrbanValues[0].LanduseTypeValue);
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
                formConfusionMatrix.Text = resourceManager.GetString("String105") + " - " + formLogisticCAWizard.SimulationLayerName;
                formConfusionMatrix.ShowDialog();

                dockableWindowOutput.ScrollTextbox();
                Application.DoEvents();
            }
            dockableWindowOutput.AppendText("-------------------------------------------");
            dockableWindowOutput.AppendText("\n");
            dockableWindowOutput.AppendText("\n");
            dockableWindowOutput.ScrollTextbox();
            Application.DoEvents();
        }
    }
}
