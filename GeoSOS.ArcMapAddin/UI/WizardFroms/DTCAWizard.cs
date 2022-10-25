using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Resources;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.DataSourcesRaster;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geodatabase;
using GeoSOS.CommonLibrary;
using GeoSOS.CommonLibrary.Struct;
using GeoSOS.Algorithms;
using GeoSOS.ArcMapAddIn.UI.Forms;
using Accord.Statistics.Analysis;
using Accord.MachineLearning.DecisionTrees;

namespace GeoSOS.ArcMapAddIn
{
    public partial class FormDTCAWizard : Form
    {
        #region 内部变量

        IApplication application;
        IMxDocument document;
        IMap map;
        /// <summary>
        /// 当前地图所有图层列表
        /// </summary>
        List<string> listAllLayersInMapName;
        /// <summary>
        /// 资源类
        /// </summary>
        private ResourceManager resourceManager;
        /// <summary>
        /// 存储各空间变量图层名称的列表
        /// </summary>
        private List<string> listVariableLayersName;
        /// <summary>
        /// 提取规则时使用的开始时刻遥感图像。
        /// </summary>
        private string trainingStartImageName;
        /// <summary>
        /// 提取规则时使用的终止时刻遥感图像。
        /// </summary>
        private string trainingEndImageName;
        /// <summary>
        /// 模拟时使用的开始时刻遥感图像。
        /// </summary>
        private string simulationStartImageName;
        /// <summary>
        /// 模拟时使用的终止时刻遥感图像。
        /// </summary>
        private string simulationEndImageName = "";
        /// <summary>
        /// 是否在模拟结束时生成模拟结果和真实数据的二值对比影像
        /// </summary>
        private bool isGenerateCompareImage = false;
        /// <summary>
        /// 要模拟的总的单元数。
        /// </summary>
        private int convertCount;
        /// <summary>
        /// 模拟要迭代的次数。
        /// </summary>
        private int simulationIterations;
        /// <summary>
        /// 模拟过程中每多少次迭代刷新一次界面。
        /// </summary>
        private int refreshInterval;
        /// <summary>
        /// 模拟过程中每多少次输出一次图像。
        /// </summary>
        private int outputImageInterval;
        /// <summary>
        /// 模拟过程中输出图像的位置。
        /// </summary>
        private string outputFolder;
        /// <summary>
        /// 是否在模拟过程中输出图像。
        /// </summary>
        private bool isOutput = false;
        /// <summary>
        /// 标识目前正使用默认数据状态。
        /// </summary>
        private bool isUsingDefault = false;
        /// <summary>
        /// 整个向导是否已经完成。
        /// </summary>
        private bool isFinish = false;
        /// <summary>
        /// 进行规则提取的初始时刻土地利用分类影像
        /// </summary>
        private float[,] trainingStartImage;
        /// <summary>
        /// 进行规则提取的终止时刻土地利用分类影像
        /// </summary>
        private float[,] trainingEndImage;
        /// <summary>
        /// 所有的空间变量影像数据列表
        /// </summary>
        private List<float[,]> listVaribaleImages;
        /// <summary>
        /// 本次模拟使用的土地利用类型信息。
        /// </summary>
        private LandUseClassificationInfo landUseClassificationInfo;
        /// <summary>
        /// 模拟使用的初始时刻影像
        /// </summary>
        private float[,] simulationStartImage;
        /// <summary>
        /// 模拟使用的终止时刻影像
        /// </summary>
        private float[,] simulationEndImage;
        /// <summary>
        /// 模拟结果使用的图像
        /// </summary>
        private float[,] simulationImage;
        /// <summary>
        /// 关于栅格图层的元数据信息
        /// </summary>
        private StructRasterMetaData structRasterMetaData;
        /// <summary>
        /// 邻域窗口的大小
        /// </summary>
        private int neighbourWindowSize;
        /// <summary>
        /// 决策树
        /// </summary>
        private DecisionTree decisionTree;
        /// <summary>
        /// 标识是否已经完成决策树训练
        /// </summary>
        private bool isGenerated = false;
        /// <summary>
        /// 模拟结果图层的名称
        /// </summary>
        private string simulationLayerName;

        #endregion

        #region 属性

        /// <summary>
        /// 整个向导是否已经完成。
        /// </summary>
        public bool IsFinish
        {
            get { return isFinish; }
        }

        /// <summary>
        /// 存储各空间变量图层名称的列表
        /// </summary>
        public List<string> ListVariableLayersName
        {
            get { return listVariableLayersName; }
        }

        /// <summary>
        /// 模拟时使用的开始时刻遥感图像。
        /// </summary>
        public string SimulationStartImageName
        {
            get
            {
                return simulationStartImageName;
            }
        }

        /// <summary>
        /// 模拟时使用的终止时刻遥感图像。
        /// </summary>
        public string SimulationEndImageName
        {
            get
            {
                return simulationEndImageName;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public float[,] SimulationStartImage
        {
            get { return simulationStartImage; }
            set { simulationStartImage = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public float[,] SimulationEndImage
        {
            get { return simulationEndImage; }
            set { simulationEndImage = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public float[,] SimulationImage
        {
            get { return simulationImage; }
            set { simulationImage = value; }
        }

        /// <summary>
        /// 要模拟的总的单元数。
        /// </summary>
        public int ConvertCount
        {
            get
            {
                return convertCount;
            }
        }

        /// <summary>
        /// 模拟要迭代的次数。
        /// </summary>
        public int Iterations
        {
            get
            {
                return simulationIterations;
            }
        }

        /// <summary>
        /// 是否在模拟过程中输出图像。
        /// </summary>
        public bool IsOutput
        {
            get
            {
                return isOutput;
            }
        }

        /// <summary>
        /// 模拟过程中每多少次输出一次图像。
        /// </summary>
        public int OutputImageInterval
        {
            get
            {
                return outputImageInterval;
            }
        }

        /// <summary>
        /// 模拟过程中每多少次迭代刷新一次界面。
        /// </summary>
        public int RefreshInterval
        {
            get
            {
                return refreshInterval;
            }
        }

        /// <summary>
        /// 模拟过程中输出图像的位置。
        /// </summary>
        public string OutputFolder
        {
            get
            {
                return outputFolder;
            }
        }

        /// <summary>
        /// 本次模拟使用的土地利用类型信息。
        /// </summary>
        public LandUseClassificationInfo LandUseClassificationInfo
        {
            get
            {
                return landUseClassificationInfo;
            }
        }

        public List<float[,]> ListVaribaleImages
        {
            get { return listVaribaleImages; }
        }

        /// <summary>
        /// 
        /// </summary>
        public StructRasterMetaData CurrentStructRasterMetaData
        {
            get { return structRasterMetaData; }
            set { structRasterMetaData = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsUsingDefault
        {
            get { return isUsingDefault; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int NeighbourWindowSize
        {
            get { return neighbourWindowSize; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DecisionTree CurrentDecisionTree
        {
            get { return decisionTree; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SimulationLayerName
        {
            get { return simulationLayerName; }
            set { simulationLayerName = value; }
        }

        #endregion

        public FormDTCAWizard()
        {
            resourceManager = VariableMaintainer.CurrentResourceManager;
            InitializeComponent();
            ChangeUI();
            listAllLayersInMapName = new List<string>();
            application = ArcMap.Application;
            document = application.Document as IMxDocument;
            map = document.FocusMap;
            ArcGISOperator.FoucsMap = document.FocusMap;
            listVariableLayersName = new List<string>();
            listVaribaleImages = new List<float[,]>();
        }

        #region 向导页切换

        /// <summary>
        /// 当向导页要切换到下一页前可以做的该页检查工作，决定是否可以切换到下一页。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void wizardLogistcCA_BeforeSwitchPages(object sender, CristiPotlog.Controls.Wizard.BeforeSwitchPagesEventArgs e)
        {
            //训练数据设置页切换前的检查
            if (e.OldIndex == 2 && e.NewIndex == 3)
            {
                StringBuilder sb = new StringBuilder();
                if (comboBoxTrainingStartImage.SelectedIndex == -1)
                {
                    sb.AppendLine(resourceManager.GetString("String3"));
                    sb.AppendLine();
                    e.Cancel = true;
                }
                if (comboBoxTrainingEndImage.SelectedIndex == -1)
                {
                    sb.AppendLine(resourceManager.GetString("String4"));
                    sb.AppendLine();
                    e.Cancel = true;
                }
                if (this.dataGridViewVariableDatas.Rows.Count == 0)
                {
                    sb.AppendLine(resourceManager.GetString("String5"));
                    e.Cancel = true;
                }
                else
                {
                    //还需要判断每行里是否有空值
                    if (dataGridViewVariableDatas.Rows[0].Cells[0].Value == null)
                    {
                        sb.AppendLine(resourceManager.GetString("String5"));
                        e.Cancel = true;
                    }
                }
                if (sb.Length > 0)
                    MessageBox.Show(sb.ToString(), resourceManager.GetString("String2"), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            //土地利用类型设置页切换前的检查，确保已设置土地利用类型性质
            else if (e.OldIndex == 3 && e.NewIndex == 4)
            {
                for (int i = 0; i < dataGridViewLandUse.Rows.Count; i++)
                {
                    if (dataGridViewLandUse.Rows[i].Cells[2].Value == null)
                    {
                        MessageBox.Show(resourceManager.GetString("String10"),
                            resourceManager.GetString("String2"), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        e.Cancel = true;
                        break;
                    }
                }
            }
            //模拟数据设置页可以继续的条件
            else if (e.OldIndex == 6 && e.NewIndex == 7)
            {
                StringBuilder sb = new StringBuilder();
                if (this.comboBoxSimStartImage.Text == "")
                {
                    sb.AppendLine(resourceManager.GetString("String16"));
                    sb.AppendLine();
                    e.Cancel = true;
                }
                if (sb.Length > 0)
                    MessageBox.Show(sb.ToString(), resourceManager.GetString("String2"), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            //限制图层
            else if (e.OldIndex == 5 && e.NewIndex == 6)
            {
                //如果不是第一次打开窗体，则再次读取地图数据
                application = ArcMap.Application;
                document = application.Document as IMxDocument;
                map = document.FocusMap;
                ArcGISOperator.FoucsMap = document.FocusMap;
                for (int i = 0; i < map.LayerCount; i++)
                {
                    comboBoxRestrictLayer.Items.Add(map.get_Layer(i).Name);
                }
            }
            //如果欢迎页选择使用默认
            else if (e.OldIndex == 0)
            {
                isUsingDefault = checkBoxUseDefault.Checked;
                if (isUsingDefault)
                {
                    if (!application.Caption.Contains("LandUseChange_DongGuan"))
                    {
                        MessageBox.Show(resourceManager.GetString("String17"), resourceManager.GetString("String2"), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        e.Cancel = true;
                    }
                    else
                    {
                        //如果不是第一次打开窗体，则再次读取地图数据
                        application = ArcMap.Application;
                        document = application.Document as IMxDocument;
                        map = document.FocusMap;
                        ArcGISOperator.FoucsMap = document.FocusMap;
                        //如果图层选择框为空
                        if (comboBoxTrainingStartImage.Items.Count == 0)
                        {
                            for (int i = 0; i < map.LayerCount; i++)
                            {
                                string layerName = map.get_Layer(i).Name;
                                comboBoxTrainingStartImage.Items.Add(layerName);
                                comboBoxTrainingEndImage.Items.Add(layerName);
                                comboBoxSimStartImage.Items.Add(layerName);
                                comboBoxSimEndImage.Items.Add(layerName);
                            }
                        }
                        for (int i = 0; i < comboBoxTrainingStartImage.Items.Count; i++)
                        {
                            if (comboBoxTrainingStartImage.Items[i].ToString() == "landuse2000")
                            {
                                comboBoxTrainingStartImage.SelectedIndex = i;
                                break;
                            }
                        }
                        for (int i = 0; i < comboBoxTrainingEndImage.Items.Count; i++)
                        {
                            if (comboBoxTrainingEndImage.Items[i].ToString() == "landuse2005")
                            {
                                comboBoxTrainingEndImage.SelectedIndex = i;
                                break;
                            }
                        }
                        for (int i = 0; i < comboBoxSimStartImage.Items.Count; i++)
                        {
                            if (comboBoxSimStartImage.Items[i].ToString() == "landuse2005")
                            {
                                comboBoxSimStartImage.SelectedIndex = i;
                                break;
                            }
                        }
                        for (int i = 0; i < comboBoxSimEndImage.Items.Count; i++)
                        {
                            if (comboBoxSimEndImage.Items[i].ToString() == "landuse2006")
                            {
                                comboBoxSimEndImage.SelectedIndex = i;
                                break;
                            }
                        }
                        //使用默认配置时未读取变量数据，需要在模拟前读取
                        listVariableLayersName.Clear();
                        listVariableLayersName.Add("dtroad");
                        listVariableLayersName.Add("dtrailway");
                        listVariableLayersName.Add("dtfreeway");
                        listVariableLayersName.Add("dtcity");
                        dataGridViewVariableDatas.Rows.Clear();
                        dataGridViewVariableDatas.Rows.Add("dtroad");
                        dataGridViewVariableDatas.Rows.Add("dtrailway");
                        dataGridViewVariableDatas.Rows.Add("dtfreeway");
                        dataGridViewVariableDatas.Rows.Add("dtcity");

                        System.IO.StreamReader streamReader = new System.IO.StreamReader(GetMxdDocumentFolder() + @"\Config Files\DefaultLanduseInfo.xml");
                        System.Xml.Serialization.XmlSerializer xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(LandUseClassificationInfo));
                        landUseClassificationInfo = (LandUseClassificationInfo)xmlSerializer.Deserialize(streamReader);
                        streamReader.Close();

                        ColumnLUCharacter.Items.Clear();
                        ColumnLUCharacter.Items.Add(resourceManager.GetString("String6"));
                        ColumnLUCharacter.Items.Add(resourceManager.GetString("String7"));
                        ColumnLUCharacter.Items.Add(resourceManager.GetString("String8"));

                        numericUpDownSamplingPrecent.Value = 2;
                        numericUpDownNeiWindow.Value = 5;
                        numericUpDownConvertCount.Value = 19260;
                        numericUpDownIterations.Value = 100;
                        numericUpDownRefresh.Value = 10;
                        isOutput = false;
                    }
                }
            }
        }

        /// <summary>
        /// 当向导页的新一页出现前可以做的该页初始化工作。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void wizardLogistcCA_AfterSwitchPages(object sender, CristiPotlog.Controls.Wizard.AfterSwitchPagesEventArgs e)
        {
            //切换至训练数据页时，应首先读取地图图层信息，填写部分控件
            if (e.OldIndex == 1 && e.NewIndex == 2)
            {
                //如果不是第一次打开窗体，则再次读取地图数据
                application = ArcMap.Application;
                document = application.Document as IMxDocument;
                map = document.FocusMap;
                ArcGISOperator.FoucsMap = document.FocusMap;
                //如果图层选择框为空
                if (comboBoxTrainingStartImage.Items.Count == 0)
                {
                    for (int i = 0; i < map.LayerCount; i++)
                    {
                        string layerName = map.get_Layer(i).Name;
                        comboBoxTrainingStartImage.Items.Add(layerName);
                        comboBoxTrainingEndImage.Items.Add(layerName);
                        comboBoxSimStartImage.Items.Add(layerName);
                        comboBoxSimEndImage.Items.Add(layerName);
                    }
                }
            }
            //切换到土地利用类型设置页时首先获取图层信息，然后填充土地利用类型
            else if (e.OldIndex == 2 && e.NewIndex == 3)
            {
                if (isGenerated)
                    return;
                //如果已经设置了土地利用类型信息
                if (dataGridViewLandUse.Rows.Count > 0)
                {
                    //保存信息
                    listVariableLayersName.Clear();
                    trainingStartImageName = comboBoxTrainingStartImage.SelectedItem.ToString();
                    trainingEndImageName = comboBoxTrainingEndImage.SelectedItem.ToString();
                    for (int i = 0; i < dataGridViewVariableDatas.Rows.Count; i++)
                        listVariableLayersName.Add(dataGridViewVariableDatas.Rows[i].Cells[0].Value.ToString());
                }
                //如果不使用默认配置，则读取图层信息
                if (!isUsingDefault)
                {
                    //保存信息
                    listVariableLayersName.Clear();
                    trainingStartImageName = comboBoxTrainingStartImage.SelectedItem.ToString();
                    trainingEndImageName = comboBoxTrainingEndImage.SelectedItem.ToString();
                    for (int i = 0; i < dataGridViewVariableDatas.Rows.Count; i++)
                        listVariableLayersName.Add(dataGridViewVariableDatas.Rows[i].Cells[0].Value.ToString());

                    dataGridViewLandUse.Rows.Clear();
                    IRasterLayer rasterLayer = ArcGISOperator.GetRasterLayerByName(trainingStartImageName);
                    IRasterUniqueValueRenderer rasterUniqueValueRenderer = (IRasterUniqueValueRenderer)rasterLayer.Renderer;
                    IRasterRendererUniqueValues rasterRendererUniqueValues = (IRasterRendererUniqueValues)rasterUniqueValueRenderer;
                    IUniqueValues uniqueValues = rasterRendererUniqueValues.UniqueValues;
                    List<object> listUniqueValues = new List<object>();
                    for (int i = 0; i < uniqueValues.Count; i++)
                    {
                        listUniqueValues.Add(uniqueValues.get_UniqueValue(i));
                    }
                    int classCount = rasterUniqueValueRenderer.get_ClassCount(0);
                    for (int i = 0; i < classCount; i++)
                    {
                        dataGridViewLandUse.Rows.Add();
                        dataGridViewLandUse.Rows[i].Cells[0].Value = listUniqueValues[i];
                        dataGridViewLandUse.Rows[i].Cells[1].Value = rasterUniqueValueRenderer.get_Label(0, i);

                        ISymbol symbol = rasterUniqueValueRenderer.get_Symbol(0, i);
                        IFillSymbol fillSymbol = symbol as IFillSymbol;
                        //ISimpleFillSymbol simpleFillSymbol = (ISimpleFillSymbol)symbol;
                        //IColor esriColor = simpleFillSymbol.Color;
                        IColor esriColor = fillSymbol.Color;
                        IRgbColor rgbColor = new RgbColorClass();
                        rgbColor.CMYK = esriColor.CMYK;
                        Color color = Color.FromArgb(rgbColor.Red, rgbColor.Green, rgbColor.Blue);
                        dataGridViewLandUse.Rows[i].Cells[3].Value =
                            GeneralOpertor.GetBitmap(15, 15, color);
                    }
                    ColumnLUCharacter.Items.Clear();
                    ColumnLUCharacter.Items.Add(resourceManager.GetString("String6"));
                    ColumnLUCharacter.Items.Add(resourceManager.GetString("String7"));
                    ColumnLUCharacter.Items.Add(resourceManager.GetString("String8"));
                }
                else
                {
                    dataGridViewLandUse.Rows.Clear();
                    System.IO.StreamReader streamReader = new System.IO.StreamReader(GetMxdDocumentFolder() + @"\Config Files\DefaultLanduseInfo.xml");
                    System.Xml.Serialization.XmlSerializer xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(LandUseClassificationInfo));
                    landUseClassificationInfo = (LandUseClassificationInfo)xmlSerializer.Deserialize(streamReader);
                    streamReader.Close();
                    for (int i = 0; i < landUseClassificationInfo.AllTypesCount; i++)
                    {
                        StructLanduseInfo strucLanduseInfo = landUseClassificationInfo.AllTypes[i];
                        dataGridViewLandUse.Rows.Add();
                        if (System.Globalization.CultureInfo.CurrentCulture.Parent.Name == "zh-CHS")
                            dataGridViewLandUse.Rows[i].Cells[1].Value = strucLanduseInfo.LanduseTypeChsName;
                        else if (System.Globalization.CultureInfo.CurrentCulture.Parent.Name == "zh-CHT")
                            dataGridViewLandUse.Rows[i].Cells[1].Value = strucLanduseInfo.LanduseTypeChtName;
                        else
                            dataGridViewLandUse.Rows[i].Cells[1].Value = strucLanduseInfo.LanduseTypeEnName;
                        dataGridViewLandUse.Rows[i].Cells[0].Value = strucLanduseInfo.LanduseTypeValue;
                        dataGridViewLandUse.Rows[i].Cells[3].Value =
                                GeneralOpertor.GetBitmap(15, 15, Color.FromArgb(strucLanduseInfo.LanduseTypeColorIntValue));
                        if (landUseClassificationInfo.UrbanValues.Contains(strucLanduseInfo))
                            dataGridViewLandUse.Rows[i].Cells[2].Value = resourceManager.GetString("String8");
                        else if (landUseClassificationInfo.ConvertValues.Contains(strucLanduseInfo))
                            dataGridViewLandUse.Rows[i].Cells[2].Value = resourceManager.GetString("String6");
                        else
                            dataGridViewLandUse.Rows[i].Cells[2].Value = resourceManager.GetString("String7");
                    }
                }
            }
            //切换到决策树构建页面时先进行清空操作，然后进行计算
            else if (e.OldIndex == 3 && e.NewIndex == 4)
            {
                //如果已经完成决策树构建
                if (isGenerated)
                    return;
                if (decisionTree == null)
                {
                    decisionTreeView.TreeSource = null;
                    decisionTreeView.Refresh();
                    dataGridViewConfusionMatrix.DataSource = null;
                    dataGridViewConfusionMatrix.Refresh();
                }
                labelSamplingData.Text = resourceManager.GetString("String11");
                labelCalculte.Text = resourceManager.GetString("String9");
                labelCalculte.Visible = true;
                progressBarCalculate.Visible = true;
                labelTrainningAccuracy.Text = "";
                labelValidationAccuracy.Text = "";
                Application.DoEvents();

                //1.读取起始图像、终止图像、各变量图像为数组
                IRasterLayer rasterLayerTrainingStart = ArcGISOperator.GetRasterLayerByName(comboBoxTrainingStartImage.SelectedItem.ToString());
                IRasterLayer rasterLayerTrainingEnd = ArcGISOperator.GetRasterLayerByName(comboBoxTrainingEndImage.SelectedItem.ToString());
                RasterSampling rasterSampling = new RasterSampling();
                labelCalculte.Text = resourceManager.GetString("String12");
                Application.DoEvents();
                List<int> notNullRows;
                List<int> notNullColumns;
                trainingStartImage = ArcGISOperator.ReadRasterAndGetNotNullRowColumn(rasterLayerTrainingStart, out structRasterMetaData,
                   out notNullRows, out notNullColumns);
                //获取最小空间范围
                List<IRasterLayer> listVariablesLayers = new List<IRasterLayer>();
                for (int i = 0; i < listVariableLayersName.Count; i++)
                    listVariablesLayers.Add(ArcGISOperator.GetRasterLayerByName(listVariableLayersName[i]));
                ArcGISOperator.GetSmallestBound(rasterLayerTrainingStart, rasterLayerTrainingEnd, listVariablesLayers, ref structRasterMetaData);

                labelCalculte.Text = resourceManager.GetString("String13");
                Application.DoEvents();
                trainingEndImage = ArcGISOperator.ReadRaster(rasterLayerTrainingEnd, structRasterMetaData.NoDataValue);
                float[,] changeImage = GeneralOpertor.GetBinaryImageByTwoImages(trainingStartImage, trainingEndImage, structRasterMetaData.RowCount,
                    structRasterMetaData.ColumnCount, structRasterMetaData.NoDataValue);
                //GeneralOpertor.WriteDataFloat(GetOutputFolder() + @"\lr_change.txt", changeImage, structRasterMetaData.RowCount, structRasterMetaData.ColumnCount);

                foreach (string layerName in listVariableLayersName)
                {
                    labelCalculte.Text = resourceManager.GetString("String14") + layerName + ".....";
                    Application.DoEvents();
                    listVaribaleImages.Add(ArcGISOperator.ReadRaster(ArcGISOperator.GetRasterLayerByName(layerName), structRasterMetaData.NoDataValue));
                }

                //2.按设置的比例进行随机抽样，得到抽样结果，然后计算
                //1.首先获取起始时刻栅格的邻域信息，作为决策树的其中一个影响因子
                NeighbourOperator neighbourOperator = new NeighbourOperator();
                neighbourWindowSize = Convert.ToInt32(numericUpDownNeiWindow.Value);
                List<float> listUrbanValues = new List<float>();
                for (int j = 0; j < landUseClassificationInfo.UrbanValues.Count; j++)
                {
                    listUrbanValues.Add(landUseClassificationInfo.UrbanValues[j].LanduseTypeValue);
                }
                float[,] neighbourData = neighbourOperator.GetNeighbourData(trainingStartImage, neighbourWindowSize,
                    structRasterMetaData.RowCount, structRasterMetaData.ColumnCount, listUrbanValues);
                listVaribaleImages.Add(neighbourData);
                //2.再将起始时刻的土地利用类型作为一个影响因子
                listVaribaleImages.Add(trainingStartImage);
                //3.进行抽样
                labelCalculte.Text = resourceManager.GetString("String11");
                Application.DoEvents();
                int samplingCellsCount = (int)(structRasterMetaData.RowCount * structRasterMetaData.ColumnCount * numericUpDownSamplingPrecent.Value / 100);
                float[,] datas = new float[samplingCellsCount, listVaribaleImages.Count + 1];
                //抽取真值的比例默认设置为一半,进行层次采样
                datas = rasterSampling.SamplingData(listVaribaleImages, changeImage, (double)numericUpDownSamplingPrecent.Value,
                    50, structRasterMetaData, notNullRows, notNullColumns);
                //2.进行计算
                labelCalculte.Text = resourceManager.GetString("String9");
                Application.DoEvents();
                //输出抽样结果数据
                //GeneralOpertor.WriteDataFloat(GetOutputFolder() + @"\dtSamplingData.txt", datas,
                //    rasterSampling.SamplingCellsCount, listVaribaleImages.Count + 1);

                //3.使用Accord.NET进行决策树计算
                DecisionTreeGenerator decisionTreeGenerator = new DecisionTreeGenerator();
                int datasLength = datas.Length / (listVaribaleImages.Count + 1);
                labelSamplingData.Text = resourceManager.GetString("String64") + datasLength + resourceManager.GetString("String65");
                labelCalculte.Text = resourceManager.GetString("String63");
                Application.DoEvents();

                float[][] inputs = new float[datasLength][];
                int[] outputs = new int[datasLength];
                decisionTreeGenerator.DataPrepare(datas, datasLength, listVaribaleImages.Count + 1, ref inputs, ref outputs);
                List<string> listIndependentVariablesName = new List<string>();
                for (int w = 0; w < listVariableLayersName.Count; w++)
                    listIndependentVariablesName.Add(listVariableLayersName[w]);
                listIndependentVariablesName.Add("Neighbour");
                listIndependentVariablesName.Add("LanduseType");
                Application.DoEvents();

                double trainingScale = 0.8;
                double[][] trainInput = GeneralOpertor.GetArray(inputs, trainingScale, true, listVaribaleImages.Count);
                double[][] testInput = GeneralOpertor.GetArray(inputs, trainingScale, false, listVaribaleImages.Count);
                int[] trainOutput = GeneralOpertor.GetArray(outputs, trainingScale, true);
                int[] testOutput = GeneralOpertor.GetArray(outputs, trainingScale, false);
                int neiWindowSize = Convert.ToInt32(numericUpDownNeiWindow.Value);
                decisionTree = decisionTreeGenerator.GenerateDecisionTree(listVaribaleImages.Count, ref trainInput, ref trainOutput,
                    2, listIndependentVariablesName, neiWindowSize, listVaribaleImages.Count + 1);
                decisionTreeView.TreeSource = decisionTree;
                Application.DoEvents();

                ConfusionMatrix confusionMatrix = decisionTreeGenerator.GetDecisionTreeAccuracy(decisionTree, ref trainInput, ref trainOutput);
                DataTable dtAccuracy = new DataTable();
                dtAccuracy.Columns.Add(resourceManager.GetString("String94"));
                dtAccuracy.Columns.Add(resourceManager.GetString("String95"));
                DataRow row = dtAccuracy.NewRow();
                row[0] = resourceManager.GetString("String96");
                row[1] = confusionMatrix.Accuracy.ToString("0.000");
                dtAccuracy.Rows.Add(row);
                row = dtAccuracy.NewRow();
                row[0] = resourceManager.GetString("String97");
                row[1] = confusionMatrix.Kappa.ToString("0.000");
                dtAccuracy.Rows.Add(row);
                row = dtAccuracy.NewRow();
                row[0] = resourceManager.GetString("String98");
                row[1] = (trainingScale * 100).ToString("0.0") + " %";
                dtAccuracy.Rows.Add(row);
                row = dtAccuracy.NewRow();
                row[0] = resourceManager.GetString("String99");
                row[1] = trainOutput.Length.ToString();
                dtAccuracy.Rows.Add(row);
                this.dataGridViewConfusionMatrix.DataSource = dtAccuracy;
                labelTrainningAccuracy.Text = (confusionMatrix.Accuracy * 100).ToString("0.00") + " %";
                confusionMatrix = decisionTreeGenerator.GetDecisionTreeAccuracy(decisionTree, ref testInput, ref testOutput);
                labelValidationAccuracy.Text = (confusionMatrix.Accuracy * 100).ToString("0.00") + " %";

                labelCalculte.Text = resourceManager.GetString("String93");
                progressBarCalculate.Visible = false;
                Application.DoEvents();
                isGenerated = true;
            }
            //切换到模拟数据设置页时
            else if (e.OldIndex == 4 && e.NewIndex == 5)
            {
                if (comboBoxSimEndImage.SelectedIndex == -1)
                    buttonCalConvertCells.Enabled = false;
                else
                    buttonCalConvertCells.Enabled = true;
            }
            //切换到模拟过程参数设置页时给定控件初始值，同时保存模拟数据信息
            else if (e.OldIndex == 5 && e.NewIndex == 6)
            {
                simulationStartImageName = comboBoxSimStartImage.SelectedItem.ToString();
                if (comboBoxSimEndImage.SelectedIndex != -1)
                    simulationEndImageName = comboBoxSimEndImage.SelectedItem.ToString();

                convertCount = Convert.ToInt32(numericUpDownConvertCount.Value);
                simulationIterations = Convert.ToInt32(numericUpDownIterations.Value);

                numericUpDownRefresh.Value = 10;
                numericUpDownOutputImage.Value = 10;
                outputFolder = GetOutputFolder();
                textBoxOutputFolder.Text = outputFolder;
            }
            //切换到完成页时填充摘要，同时保存模拟过程输出参数的信息
            else if (e.OldIndex == 6 && e.NewIndex == 7)
            {
                refreshInterval = Convert.ToInt32(numericUpDownRefresh.Value);
                outputImageInterval = Convert.ToInt32(numericUpDownOutputImage.Value);
                if (radioButtonOutput.Checked)
                    isOutput = true;
                else
                    isOutput = false;
                outputFolder = textBoxOutputFolder.Text;

                textBoxSummay.Text = WriteSummay();
            }
        }

        #endregion

        #region 控件事件

        private void buttonAddRuleImageFile_Click(object sender, EventArgs e)
        {
            DataGridViewRow row = new DataGridViewRow();
            FormVariables formVaiables = new FormVariables();
            for (int i = 0; i < map.LayerCount; i++)
            {
                string layerName = map.get_Layer(i).Name;
                if ((layerName != trainingStartImageName) && (layerName != trainingEndImageName))
                    formVaiables.ListViewControl.Items.Add(layerName);
            }
            if (formVaiables.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                for (int i = 0; i < formVaiables.ListViewControl.SelectedItems.Count; i++)
                    dataGridViewVariableDatas.Rows.Add(formVaiables.ListViewControl.SelectedItems[i].Text);
            }
        }

        private void buttonDeleteRuleImageImage_Click(object sender, EventArgs e)
        {
            if (dataGridViewVariableDatas.SelectedRows.Count > 0)
            {
                while (dataGridViewVariableDatas.SelectedRows.Count > 0)
                {
                    listAllLayersInMapName.Remove(dataGridViewVariableDatas.SelectedRows[0].Cells[0].Value.ToString());
                    dataGridViewVariableDatas.Rows.Remove(dataGridViewVariableDatas.SelectedRows[0]);
                }
            }
        }

        private void comboBoxSimEndImage_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxSimEndImage.SelectedIndex != -1)
            {
                buttonCalConvertCells.Enabled = true;
                simulationEndImageName = comboBoxSimEndImage.SelectedItem.ToString();
            }
        }

        private void comboBoxSimStartImage_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxSimStartImage.SelectedIndex != -1)
            {
                simulationStartImageName = comboBoxSimStartImage.SelectedItem.ToString();
            }
        }

        private void pictureBoxConvertedNum_Click(object sender, EventArgs e)
        {
            labelConvertedNum.Visible = true;
        }

        private void buttonOutputFolder_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    outputFolder = fbd.SelectedPath;
                    textBoxOutputFolder.Text = outputFolder;
                }
            }
        }

        private void numericUpDownConvertCount_ValueChanged(object sender, EventArgs e)
        {
            convertCount = Convert.ToInt32(numericUpDownConvertCount.Value);
        }

        private void buttonCalConvertCells_Click(object sender, EventArgs e)
        {
            if (SimulationEndImageName != "")
            {
                IRasterLayer rasterLayer = ArcGISOperator.GetRasterLayerByName(simulationStartImageName);
                int startUrbanConuts = ArcGISOperator.GetUrbanCount(rasterLayer, landUseClassificationInfo);
                rasterLayer = ArcGISOperator.GetRasterLayerByName(simulationEndImageName);
                int endUrbanCounts = ArcGISOperator.GetUrbanCount(rasterLayer, landUseClassificationInfo);
                numericUpDownConvertCount.Value = Convert.ToDecimal(endUrbanCounts - startUrbanConuts);
            }
        }

        private void numericUpDownIterations_ValueChanged(object sender, EventArgs e)
        {
            simulationIterations = Convert.ToInt32(numericUpDownIterations.Value);
        }

        private void numericUpDownRefresh_ValueChanged(object sender, EventArgs e)
        {
            refreshInterval = Convert.ToInt32(numericUpDownRefresh.Value);
        }

        private void radioButtonOutput_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonNotOutput.Checked)
            {
                numericUpDownOutputImage.Enabled = false;
                buttonOutputFolder.Enabled = false;
                isOutput = false;
            }
            else
            {
                numericUpDownOutputImage.Enabled = true;
                buttonOutputFolder.Enabled = true;
                isOutput = true;
            }
        }

        private void buttonLoadLanduseConfig_Click(object sender, EventArgs e)
        {
            dataGridViewLandUse.Rows.Clear();
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = resourceManager.GetString("String52");
                ofd.Title = resourceManager.GetString("String53");
                if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    System.IO.StreamReader streamReader = new System.IO.StreamReader(ofd.FileName);
                    System.Xml.Serialization.XmlSerializer xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(LandUseClassificationInfo));
                    landUseClassificationInfo = (LandUseClassificationInfo)xmlSerializer.Deserialize(streamReader);
                    streamReader.Close();
                    for (int i = 0; i < landUseClassificationInfo.AllTypesCount; i++)
                    {
                        StructLanduseInfo strucLanduseInfo = landUseClassificationInfo.AllTypes[i];
                        dataGridViewLandUse.Rows.Add();
                        if (System.Globalization.CultureInfo.CurrentCulture.Parent.Name == "zh-CHS")
                            dataGridViewLandUse.Rows[i].Cells[1].Value = strucLanduseInfo.LanduseTypeChsName;
                        else if (System.Globalization.CultureInfo.CurrentCulture.Parent.Name == "zh-CHT")
                            dataGridViewLandUse.Rows[i].Cells[1].Value = strucLanduseInfo.LanduseTypeChtName;
                        else
                            dataGridViewLandUse.Rows[i].Cells[1].Value = strucLanduseInfo.LanduseTypeEnName;
                        dataGridViewLandUse.Rows[i].Cells[0].Value = strucLanduseInfo.LanduseTypeValue;
                        dataGridViewLandUse.Rows[i].Cells[3].Value =
                                GeneralOpertor.GetBitmap(15, 15, Color.FromArgb(strucLanduseInfo.LanduseTypeColorIntValue));
                        if (landUseClassificationInfo.UrbanValues.Contains(strucLanduseInfo))
                            dataGridViewLandUse.Rows[i].Cells[2].Value = resourceManager.GetString("String8");
                        else if (landUseClassificationInfo.ConvertValues.Contains(strucLanduseInfo))
                            dataGridViewLandUse.Rows[i].Cells[2].Value = resourceManager.GetString("String6");
                        else
                            dataGridViewLandUse.Rows[i].Cells[2].Value = resourceManager.GetString("String7");
                    }
                }
            }
        }

        private void buttonSaveLanduseConfig_Click(object sender, EventArgs e)
        {
            landUseClassificationInfo.ClearAll();
            for (int i = 0; i < dataGridViewLandUse.Rows.Count; i++)
            {
                StructLanduseInfo strucLanduseInfo = new StructLanduseInfo();
                if (System.Globalization.CultureInfo.CurrentCulture.Parent.Name == "zh-CHS")
                    strucLanduseInfo.LanduseTypeChsName = dataGridViewLandUse.Rows[i].Cells[1].Value.ToString();
                else if (System.Globalization.CultureInfo.CurrentCulture.Parent.Name == "zh-CHT")
                    strucLanduseInfo.LanduseTypeChtName = dataGridViewLandUse.Rows[i].Cells[1].Value.ToString();
                else
                    strucLanduseInfo.LanduseTypeEnName = dataGridViewLandUse.Rows[i].Cells[1].Value.ToString();
                strucLanduseInfo.LanduseTypeValue = Convert.ToSingle(dataGridViewLandUse.Rows[i].Cells[0].Value);
                Bitmap bitmap = (Bitmap)dataGridViewLandUse.Rows[i].Cells[3].Value;
                strucLanduseInfo.LanduseTypeColorIntValue = bitmap.GetPixel(0, 0).ToArgb();

                if (dataGridViewLandUse.Rows[i].Cells[2].Value.ToString() == resourceManager.GetString("String8"))
                    landUseClassificationInfo.AddUrbanLanduseType(strucLanduseInfo);
                else if (dataGridViewLandUse.Rows[i].Cells[2].Value.ToString() == resourceManager.GetString("String6"))
                    landUseClassificationInfo.AddConvertLanduseType(strucLanduseInfo);
                else
                    landUseClassificationInfo.AddNotToConvertLanduseType(strucLanduseInfo);
            }
            landUseClassificationInfo.AddNullValue();
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = resourceManager.GetString("String52");
                sfd.Title = resourceManager.GetString("String54");
                if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    System.IO.StreamWriter steamWriter = new System.IO.StreamWriter(sfd.FileName);
                    System.Xml.Serialization.XmlSerializer xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(LandUseClassificationInfo));
                    xmlSerializer.Serialize(steamWriter, landUseClassificationInfo);
                    steamWriter.Close();
                    System.Windows.Forms.MessageBox.Show(resourceManager.GetString("String55"), resourceManager.GetString("String2"), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
            }
        }

        private void pictureBoxIterations_Click(object sender, EventArgs e)
        {
            labelIterations.Visible = true;
        }

        private void numericUpDownOutputImage_ValueChanged(object sender, EventArgs e)
        {
            outputImageInterval = Convert.ToInt32(numericUpDownOutputImage.Value);
        }

        private void wizardLogistcCA_Finish(object sender, EventArgs e)
        {
            isFinish = true;
        }

        private void comboBoxStartImage_SelectedIndexChanged(object sender, EventArgs e)
        {
            trainingStartImageName = comboBoxTrainingStartImage.SelectedItem.ToString();
        }

        private void comboBoxEndImage_SelectedIndexChanged(object sender, EventArgs e)
        {
            trainingEndImageName = comboBoxTrainingEndImage.SelectedItem.ToString();
        }

        private void linkLabelPaper_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://geosimulation.cn/Publications.html#DT-CA");
        }

        private void pictureBoxShowTrainingInfo_Click(object sender, EventArgs e)
        {
            if (labeltrainingInfoInput.Visible)
                labeltrainingInfoInput.Visible = false;
            else
                labeltrainingInfoInput.Visible = true;
            if (labeltrainingInfoOutput.Visible)
                labeltrainingInfoOutput.Visible = false;
            else
                labeltrainingInfoOutput.Visible = true;
        }

        private void pictureBoxShowSimulationInfo_Click(object sender, EventArgs e)
        {
            if (labelSimulationInfo.Visible)
                labelSimulationInfo.Visible = false;
            else
                labelSimulationInfo.Visible = true;
        }

        private void checkBoxUseDefault_CheckedChanged(object sender, EventArgs e)
        {
            isUsingDefault = checkBoxUseDefault.Checked;
            if (!isUsingDefault)
                Clear();
        }

        private void pictureBoxCalCovertAmount_Click(object sender, EventArgs e)
        {
            if (this.labelCalCovertAmount.Visible)
                labelCalCovertAmount.Visible = false;
            else
                labelCalCovertAmount.Visible = true;
        }

        #endregion

        #region Utility

        /// <summary>
        /// 获取当前Mxd的文件夹位置
        /// </summary>
        /// <returns></returns>
        public string GetMxdDocumentFolder()
        {
            string mxdFileFullPath = application.Templates.get_Item(application.Templates.Count - 1);
            return mxdFileFullPath.Substring(0, mxdFileFullPath.LastIndexOf(@"\"));
        }

        /// <summary>
        /// 获取输出目录位置
        /// </summary>
        /// <returns></returns>
        public string GetOutputFolder()
        {
            string outputFolder = GetMxdDocumentFolder() + @"\Output Data";
            if (!System.IO.Directory.Exists(outputFolder))
                System.IO.Directory.CreateDirectory(outputFolder);
            return outputFolder;
        }

        /// <summary>
        /// 输出摘要信息。
        /// </summary>
        public string WriteSummay()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(resourceManager.GetString("String18"));
            sb.AppendLine();

            sb.AppendLine(resourceManager.GetString("String73"));
            foreach (string variableName in listVariableLayersName)
                sb.AppendLine(variableName);

            sb.AppendLine();
            sb.AppendLine(resourceManager.GetString("String74"));
            sb.AppendLine(resourceManager.GetString("String27"));
            foreach (StructLanduseInfo structLanduseInfo in landUseClassificationInfo.UrbanValues)
                sb.AppendLine(structLanduseInfo.LanduseTypeChsName + resourceManager.GetString("String28") + structLanduseInfo.LanduseTypeValue.ToString());
            sb.AppendLine();
            sb.AppendLine(resourceManager.GetString("String29"));
            foreach (StructLanduseInfo structLanduseInfo in landUseClassificationInfo.ConvertValues)
                sb.AppendLine(structLanduseInfo.LanduseTypeChsName + resourceManager.GetString("String28") + structLanduseInfo.LanduseTypeValue.ToString());
            sb.AppendLine();
            sb.AppendLine(resourceManager.GetString("String30"));
            foreach (StructLanduseInfo structLanduseInfo in landUseClassificationInfo.NotToConvertValues)
                sb.AppendLine(structLanduseInfo.LanduseTypeChsName + resourceManager.GetString("String28") + structLanduseInfo.LanduseTypeValue.ToString());
            sb.AppendLine();

            sb.AppendLine(resourceManager.GetString("String75") + numericUpDownNeiWindow.Value.ToString());
            sb.AppendLine();

            sb.AppendLine(resourceManager.GetString("String76") + simulationStartImageName);
            sb.AppendLine(resourceManager.GetString("String77") + simulationEndImageName);
            sb.AppendLine();

            sb.AppendLine(resourceManager.GetString("String22") + convertCount.ToString() + resourceManager.GetString("String23"));
            sb.AppendLine(resourceManager.GetString("String31") + simulationIterations.ToString() + resourceManager.GetString("String24"));
            sb.AppendLine(resourceManager.GetString("String25") + (convertCount / simulationIterations).ToString() + resourceManager.GetString("String23"));
            sb.AppendLine();

            sb.AppendLine(resourceManager.GetString("String171") + ": " + VariableMaintainer.RestrictLayerName);
            sb.AppendLine(resourceManager.GetString("String32"));
            sb.AppendLine(resourceManager.GetString("String33") + refreshInterval + resourceManager.GetString("String34"));
            if (isOutput)
            {
                sb.AppendLine(resourceManager.GetString("String33") + outputImageInterval + resourceManager.GetString("String35"));
                sb.AppendLine(resourceManager.GetString("String36") + outputFolder);
            }
            sb.AppendLine();
            sb.AppendLine();

            return sb.ToString();
        }

        /// <summary>
        /// 改变UI界面中的语言。
        /// </summary>
        private void ChangeUI()
        {
            this.DataGridViewTextBoxColumnLayers.HeaderText = resourceManager.GetString("String109");
            this.ColumnLUCharacter.HeaderText = resourceManager.GetString("String128");
            this.ColumnLUValue.HeaderText = resourceManager.GetString("String110");
            this.ColumnLUDescription.HeaderText = resourceManager.GetString("String111");
            this.ColumnLUColor.HeaderText = resourceManager.GetString("String113");

            this.wizardPageWelcome.Title = resourceManager.GetString("String143");
            this.wizardPageWelcome.Description = resourceManager.GetString("String144");
            this.wizardPageIntroduction.Title = resourceManager.GetString("String145");
            this.wizardPageIntroduction.Description = resourceManager.GetString("String146");
            this.wizardPageTrainingData.Title = resourceManager.GetString("String114");
            this.wizardPageTrainingData.Description = resourceManager.GetString("String147");
            this.wizardPageLandUseType.Title = resourceManager.GetString("String139");
            this.wizardPageLandUseType.Description = resourceManager.GetString("String140");
            this.wizardPageDecisionTree.Title = resourceManager.GetString("String148");
            this.wizardPageDecisionTree.Description = resourceManager.GetString("String149");
            this.wizardPageSimulatedData.Title = resourceManager.GetString("String120");
            this.wizardPageSimulatedData.Description = resourceManager.GetString("String121");
            this.wizardPageOutputSettings.Title = resourceManager.GetString("String124");
            this.wizardPageOutputSettings.Description = resourceManager.GetString("String125");
            this.wizardPageFinish.Title = resourceManager.GetString("String126");
            this.wizardPageFinish.Description = resourceManager.GetString("String127");
            this.labelCalculte.Text = resourceManager.GetString("String9");
        }

        /// <summary>
        /// 执行清空操作。
        /// </summary>
        public void Clear()
        {
            comboBoxTrainingStartImage.Items.Clear();
            comboBoxTrainingEndImage.Items.Clear();
            comboBoxSimStartImage.Items.Clear();
            comboBoxSimEndImage.Items.Clear();
            listAllLayersInMapName.Clear();
            listVariableLayersName.Clear();
            trainingStartImageName = "";
            trainingEndImageName = "";
            simulationStartImageName = "";
            simulationEndImageName = "";
            convertCount = 0;
            simulationIterations = 0;
            refreshInterval = 1;
            outputImageInterval = 1;
            outputFolder = "";
            isOutput = false;
            isUsingDefault = false;
            isFinish = false;
            listVaribaleImages.Clear();
            landUseClassificationInfo = new LandUseClassificationInfo();
            structRasterMetaData = new StructRasterMetaData();
            this.dataGridViewVariableDatas.Rows.Clear();
            this.comboBoxTrainingStartImage.SelectedIndex = -1;
            this.comboBoxTrainingEndImage.SelectedIndex = -1;
            this.comboBoxSimStartImage.SelectedIndex = -1;
            this.comboBoxSimEndImage.SelectedIndex = -1;
            this.comboBoxTrainingStartImage.Text = "";
            this.comboBoxTrainingEndImage.Text = "";
            this.comboBoxSimStartImage.Text = "";
            this.comboBoxSimEndImage.Text = "";
            this.comboBoxRestrictLayer.SelectedIndex = -1;
            VariableMaintainer.IsSimulationFinished = false;
            this.numericUpDownConvertCount.Value = 100;
            this.numericUpDownIterations.Value = 10;
            isGenerated = false;
            decisionTree = null;
            this.comboBoxRestrictLayer.SelectedIndex = -1;
        }

        #endregion

        private void comboBoxRestrictLayer_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxRestrictLayer.SelectedIndex != -1)
                VariableMaintainer.RestrictLayerName = comboBoxRestrictLayer.Items[comboBoxRestrictLayer.SelectedIndex].ToString();
        }
    }
}
