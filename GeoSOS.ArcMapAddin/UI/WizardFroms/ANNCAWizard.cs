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


namespace GeoSOS.ArcMapAddIn
{
    public partial class FormANNCAWizard : Form
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
        private float[,] trainningEndImage;
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
        /// 训练起始影像的非空行索引列表。与非空列一一对应。
        /// </summary>
        private List<int> listNotNullRows;
        /// <summary>
        /// 训练起始影像的非空列索引列表。与非空行一一对应。
        /// </summary>
        private List<int> listNotNullColumns;
        /// <summary>
        /// 输入层神经元数量
        /// </summary>
        private int inputNeuronsCount = 0;
        /// <summary>
        /// 输出层神经元数量
        /// </summary>
        private int outputNeuronsCount = 0;
        /// <summary>
        /// 记录输入层数据的数组
        /// </summary>
        float[][] inputs = null;
        /// <summary>
        /// 记录输出层数据的数组
        /// </summary>
        float[][] outputs = null;
        /// <summary>
        /// 抽样数据的总数。
        /// </summary>
        private int samplingCellsCount;
        /// <summary>
        /// 执行人工神经网络算法的类。
        /// </summary>
        private ArtificalNeuralNetwork ann;
        /// <summary>
        /// 扩散系数。
        /// </summary>
        private int delta;
        /// <summary>
        /// 记录土地利用类型之间相互转换关系的矩阵
        /// </summary>
        private DataTable dtMatrix;
        /// <summary>
        /// 记录转换阈值
        /// </summary>
        private float convertThreshold;
        /// <summary>
        /// 标识是否已经完成抽样
        /// </summary>
        private bool isSampled = false;
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

        /// <summary>
        /// 
        /// </summary>
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
        public ArtificalNeuralNetwork ANN
        {
            get { return ann; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int Delta
        {
            get { return delta; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int InputNeuronsCount
        {
            get { return inputNeuronsCount; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int OutputNeuronsCount
        {
            get { return outputNeuronsCount; }
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
        public DataTable DTMatrix
        {
            get { return dtMatrix; }
        }
        /// <summary>
        /// 
        /// </summary>
        public float ConvertThreshold
        {
            get { return convertThreshold; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool IsInTheSameConfig
        {
            get { return isSampled; }
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

        public FormANNCAWizard()
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
            listNotNullRows = new List<int>();
            listNotNullColumns = new List<int>();
        }

        #region 向导页切换

        /// <summary>
        /// 对当前页进行操作
        ///1. 当向导页要切换到下一页前可以做的当前页的检查工作，决定是否可以切换到下一页。
        ///2. 对后续的页面需要进行的操作。
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
            //抽样数据页切换前的检查，确保有土地利用类型被设置为城市用地
            else if (e.OldIndex == 3 && e.NewIndex == 4)
            {
                bool isChecked = false;
                for (int i = 0; i < dataGridViewLandUse.Rows.Count; i++)
                {
                    DataGridViewCheckBoxCell chk = dataGridViewLandUse.Rows[i].Cells[2] as DataGridViewCheckBoxCell;
                    if (chk.Value == chk.TrueValue)
                    {
                        isChecked = true;
                        //如果不是使用默认配置，则设置城市用地类型
                        if (!isUsingDefault)
                        {
                            int urbanValue = Convert.ToInt32(dataGridViewLandUse.Rows[i].Cells[0].Value);
                            for (int j = 0; j < landUseClassificationInfo.AllTypesCount; j++)
                            {
                                if (landUseClassificationInfo.AllTypes[j].LanduseTypeValue == urbanValue)
                                {
                                    if (!landUseClassificationInfo.UrbanValues.Contains(landUseClassificationInfo.AllTypes[j]))
                                        landUseClassificationInfo.UrbanValues.Add(landUseClassificationInfo.AllTypes[j]);
                                    break;
                                }
                            }

                        }
                    }
                }
                if (!isChecked)
                {
                    MessageBox.Show(resourceManager.GetString("String66"),
                                resourceManager.GetString("String2"), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    e.Cancel = true;
                }
            }
            //ANN训练页可以继续的条件
            else if (e.OldIndex == 4 && e.NewIndex == 5)
            {
                if (ann == null)
                {
                    MessageBox.Show(resourceManager.GetString("String67"),
                            resourceManager.GetString("String2"), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    e.Cancel = true;
                }
            }
            //模拟数据设置页可以继续的条件
            else if (e.OldIndex == 5 && e.NewIndex == 6)
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
            else if (e.OldIndex == 6 && e.NewIndex == 7)
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
            //如果欢迎页选择使用默认，则填充已知的信息，基本上用户只需要点击下一步即可完成配置
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

                        numericUpDownSamplingPrecent.Value = 5;
                        numericUpDownNeiWindow.Value = 7;
                        numericUpDownTrainningIterations.Value = 300;
                        numericUpDownTrainningPercent.Value = 80;
                        numericUpDownValidationPercent.Value = 20;
                        numericUpDownHiddenLayerNeurons.Value = 10;
                        numericUpDownLearningRate.Value = Convert.ToDecimal(0.05);
                        numericUpDownConvertCount.Value = 19260;
                        numericUpDownIterations.Value = 100;
                        numericUpDownDelta.Value = 1;
                        numericUpDownThreshold.Value = Convert.ToDecimal(0.8);
                        numericUpDownRefresh.Value = 10;
                        isOutput = false;
                    }
                }
            }
        }

        /// <summary>
        /// 对新的一页进行操作，并对上一页的配置信息进行保存
        /// 1. 当向导页的新一页出现前可以做的该页初始化工作。
        /// 2. 同时可以保存上一页的部分配置信息。
        /// （后续需要改进为将界面与数据分离，将数据保存到某个类中）
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
            //切换至抽样数据页时首先获取图层信息，接着填充土地利用类型。然后进行数据抽样。
            else if (e.OldIndex == 2 && e.NewIndex == 3)
            {
                if (isSampled)
                    return;
                //如果不使用默认配置，则读取图层信息
                if (!isUsingDefault)
                {
                    //保存信息
                    listVariableLayersName.Clear();
                    trainingStartImageName = comboBoxTrainingStartImage.SelectedItem.ToString();
                    trainingEndImageName = comboBoxTrainingEndImage.SelectedItem.ToString();
                    for (int i = 0; i < dataGridViewVariableDatas.Rows.Count; i++)
                        listVariableLayersName.Add(dataGridViewVariableDatas.Rows[i].Cells[0].Value.ToString());

                    //读取并保存土地利用类型信息
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
                    landUseClassificationInfo.ClearAll();
                    landUseClassificationInfo.AllTypesCount = classCount;
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

                        //将土地利用类型信息保存到landUseClassificationInfo中
                        StructLanduseInfo structLanduseInfo = new StructLanduseInfo();
                        structLanduseInfo.LanduseTypeValue = Convert.ToSingle(listUniqueValues[i]);
                        structLanduseInfo.LanduseTypeColorIntValue = color.ToArgb();
                        if (System.Threading.Thread.CurrentThread.CurrentCulture.Parent.Name == "zh-CHS")
                            structLanduseInfo.LanduseTypeChsName = rasterUniqueValueRenderer.get_Label(0, i);
                        else if (System.Threading.Thread.CurrentThread.CurrentCulture.Parent.Name == "zh-CHT")
                            structLanduseInfo.LanduseTypeChtName = rasterUniqueValueRenderer.get_Label(0, i);
                        else if (System.Threading.Thread.CurrentThread.CurrentCulture.Parent.Name == "en")
                            structLanduseInfo.LanduseTypeEnName = rasterUniqueValueRenderer.get_Label(0, i);
                        landUseClassificationInfo.AllTypes.Add(structLanduseInfo);
                    }
                }
                else
                {
                    //读取已经设置的土地利用类型信息
                    dataGridViewLandUse.Rows.Clear();
                    System.IO.StreamReader streamReader = new System.IO.StreamReader(GetMxdDocumentFolder() + @"\Config Files\DefaultLanduseInfo.xml");
                    System.Xml.Serialization.XmlSerializer xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(LandUseClassificationInfo));
                    landUseClassificationInfo = (LandUseClassificationInfo)xmlSerializer.Deserialize(streamReader);
                    streamReader.Close();
                    for (int i = 0; i < landUseClassificationInfo.AllTypesCount; i++)
                    {
                        dataGridViewLandUse.Rows.Add();
                        if (System.Threading.Thread.CurrentThread.CurrentCulture.Parent.Name == "zh-CHS")
                            dataGridViewLandUse.Rows[i].Cells[0].Value = landUseClassificationInfo.AllTypes[i].LanduseTypeChsName;
                        else if (System.Threading.Thread.CurrentThread.CurrentCulture.Parent.Name == "zh-CHT")
                            dataGridViewLandUse.Rows[i].Cells[0].Value = landUseClassificationInfo.AllTypes[i].LanduseTypeChtName;
                        else if (System.Threading.Thread.CurrentThread.CurrentCulture.Parent.Name == "en")
                            dataGridViewLandUse.Rows[i].Cells[0].Value = landUseClassificationInfo.AllTypes[i].LanduseTypeEnName;
                        dataGridViewLandUse.Rows[i].Cells[1].Value = landUseClassificationInfo.AllTypes[i].LanduseTypeValue;
                        if (landUseClassificationInfo.UrbanValues.Contains(landUseClassificationInfo.AllTypes[i]))
                        {
                            DataGridViewCheckBoxCell chk = dataGridViewLandUse.Rows[i].Cells[2] as DataGridViewCheckBoxCell;
                            chk.Value = chk.TrueValue;
                        }
                        dataGridViewLandUse.Rows[i].Cells[3].Value =
                                GeneralOpertor.GetBitmap(15, 15, Color.FromArgb(landUseClassificationInfo.AllTypes[i].LanduseTypeColorIntValue));
                    }
                }

                //进行抽样
                labelSamplingPrecent.Text = "";
                labelSamplingCount.Text = "";
                labelInputsCount.Text = "";
                labelVariablesCount.Text = "";
                labelOutputsCount.Text = "";
                labelLanduseTypesCount.Text = "";
                labelLanduseTypesCount2.Text = "";
                labelCalculte.Text = resourceManager.GetString("String11");
                Application.DoEvents();
                labelCalculte.Visible = true;
                progressBarCalculate.Visible = true;
                Application.DoEvents();
                ReadTrainningDatas();
                //要抽取的样本总数。
                samplingCellsCount = Convert.ToInt32(listNotNullRows.Count * Convert.ToDouble(numericUpDownSamplingPrecent.Value) / 100);
                //抽样数据的列包括：
                //输入[各空间变量，邻域窗口中各土地利用类型的数量，当前元胞在初始数据中的土地利用类型]
                inputNeuronsCount = listVaribaleImages.Count + landUseClassificationInfo.AllTypesCount + 1;
                //输出[当前元胞在终止数据中是否是某种土地利用类型]
                outputNeuronsCount = landUseClassificationInfo.AllTypesCount;
                inputs = new float[samplingCellsCount][];
                outputs = new float[samplingCellsCount][];
                neighbourWindowSize = Convert.ToInt32(numericUpDownNeiWindow.Value);
                RasterSampling rasterSampling = new RasterSampling();
                rasterSampling.ANNSamplingData(listVaribaleImages, trainingStartImage, trainningEndImage, landUseClassificationInfo,
                    structRasterMetaData, neighbourWindowSize, listNotNullRows, listNotNullColumns,
                    ref inputs, ref outputs, samplingCellsCount, inputNeuronsCount, outputNeuronsCount);

                labelSamplingPrecent.Text = numericUpDownSamplingPrecent.Value.ToString();
                labelSamplingCount.Text = samplingCellsCount.ToString();
                labelInputsCount.Text = inputNeuronsCount.ToString();
                labelVariablesCount.Text = listVaribaleImages.Count.ToString();
                labelOutputsCount.Text = outputNeuronsCount.ToString();
                labelLanduseTypesCount.Text = landUseClassificationInfo.AllTypesCount.ToString();
                labelLanduseTypesCount2.Text = landUseClassificationInfo.AllTypesCount.ToString();
                labelCalculte.Text = resourceManager.GetString("String68");
                progressBarCalculate.Visible = false;
                Application.DoEvents();
                isSampled = true;
            }
            //切换到人工神经网络训练页面时，进行清空操作
            else if (e.OldIndex == 3 && e.NewIndex == 4)
            {
                labelTrainingCount.Text = (samplingCellsCount *
                    Convert.ToInt32(numericUpDownTrainningPercent.Value) / 100).ToString();
                if (ann == null)
                {
                    labelTrainingProcess.Text = "";
                    labelTrainningAccuracy.Text = "";
                    labelValidationAccuracy.Text = "";
                    zedGraphControl.GraphPane.CurveList.Clear();
                }
            }
            //切换到模拟数据设置页时
            else if (e.OldIndex == 4 && e.NewIndex == 5)
            {
                if (comboBoxSimEndImage.SelectedIndex == -1)
                    buttonCalConvertCells.Enabled = false;
            }
            //切换到模拟参数设置页时打开适宜性设置表进行适宜性设置。
            else if (e.OldIndex == 5 && e.NewIndex == 6)
            {
                if (dataGridViewMatrix.Rows.Count > 0)
                    return;
                //如果不使用默认，则根据数据填充矩阵。否则自动加载配置文件。
                if (!isUsingDefault)
                {
                    dataGridViewMatrix.Columns.Clear();
                    dataGridViewMatrix.Rows.Clear();
                    dtMatrix = new DataTable();
                    for (int i = 0; i < landUseClassificationInfo.AllTypesCount; i++)
                    {
                        if (System.Threading.Thread.CurrentThread.CurrentCulture.Parent.Name == "zh-CHS")
                        {
                            dataGridViewMatrix.Columns.Add(landUseClassificationInfo.AllTypes[i].LanduseTypeChsName, landUseClassificationInfo.AllTypes[i].LanduseTypeChsName);
                            dtMatrix.Columns.Add(landUseClassificationInfo.AllTypes[i].LanduseTypeChsName);
                        }
                        else if (System.Threading.Thread.CurrentThread.CurrentCulture.Parent.Name == "zh-CHT")
                        {
                            dataGridViewMatrix.Columns.Add(landUseClassificationInfo.AllTypes[i].LanduseTypeChtName, landUseClassificationInfo.AllTypes[i].LanduseTypeChtName);
                            dtMatrix.Columns.Add(landUseClassificationInfo.AllTypes[i].LanduseTypeChtName);
                        }
                        else if (System.Threading.Thread.CurrentThread.CurrentCulture.Parent.Name == "en")
                        {
                            dataGridViewMatrix.Columns.Add(landUseClassificationInfo.AllTypes[i].LanduseTypeEnName, landUseClassificationInfo.AllTypes[i].LanduseTypeEnName);
                            dtMatrix.Columns.Add(landUseClassificationInfo.AllTypes[i].LanduseTypeEnName);
                        }
                    }
                    for (int i = 0; i < landUseClassificationInfo.AllTypesCount; i++)
                    {
                        dataGridViewMatrix.Rows.Add();
                        dataGridViewMatrix.RowHeadersWidth = 110;
                        dataGridViewMatrix.Rows[i].Height = 30;
                        if (System.Threading.Thread.CurrentThread.CurrentCulture.Parent.Name == "zh-CHS")
                            dataGridViewMatrix.Rows[i].HeaderCell.Value = landUseClassificationInfo.AllTypes[i].LanduseTypeChsName;
                        else if (System.Threading.Thread.CurrentThread.CurrentCulture.Parent.Name == "zh-CHT")
                            dataGridViewMatrix.Rows[i].HeaderCell.Value = landUseClassificationInfo.AllTypes[i].LanduseTypeChtName;
                        else if (System.Threading.Thread.CurrentThread.CurrentCulture.Parent.Name == "en")
                            dataGridViewMatrix.Rows[i].HeaderCell.Value = landUseClassificationInfo.AllTypes[i].LanduseTypeEnName;

                        DataRow row = dtMatrix.NewRow();
                        for (int j = 0; j < landUseClassificationInfo.AllTypesCount; j++)
                        {
                            dataGridViewMatrix.Rows[i].Cells[j].Value = 1;
                            row[j] = 1;
                        }
                        dtMatrix.Rows.Add(row);
                    }
                }
                else
                {
                    DataSet ds = new DataSet();
                    ds.ReadXml(GetMxdDocumentFolder() + @"\Config Files\SuitableMatrix.xml");
                    dtMatrix = ds.Tables[0];

                    dataGridViewMatrix.Columns.Clear();
                    dataGridViewMatrix.Rows.Clear();
                    for (int i = 0; i < dtMatrix.Rows.Count; i++)
                    {
                        if (System.Threading.Thread.CurrentThread.CurrentCulture.Parent.Name == "zh-CHS")
                            dataGridViewMatrix.Columns.Add(landUseClassificationInfo.AllTypes[i].LanduseTypeChsName, landUseClassificationInfo.AllTypes[i].LanduseTypeChsName);
                        else if (System.Threading.Thread.CurrentThread.CurrentCulture.Parent.Name == "zh-CHT")
                            dataGridViewMatrix.Columns.Add(landUseClassificationInfo.AllTypes[i].LanduseTypeChtName, landUseClassificationInfo.AllTypes[i].LanduseTypeChtName);
                        else if (System.Threading.Thread.CurrentThread.CurrentCulture.Parent.Name == "en")
                            dataGridViewMatrix.Columns.Add(landUseClassificationInfo.AllTypes[i].LanduseTypeEnName, landUseClassificationInfo.AllTypes[i].LanduseTypeEnName);
                    }
                    for (int i = 0; i < dtMatrix.Rows.Count; i++)
                    {
                        dataGridViewMatrix.Rows.Add();
                        dataGridViewMatrix.RowHeadersWidth = 110;
                        dataGridViewMatrix.Rows[i].Height = 30;
                        if (System.Threading.Thread.CurrentThread.CurrentCulture.Parent.Name == "zh-CHS")
                            dataGridViewMatrix.Rows[i].HeaderCell.Value = landUseClassificationInfo.AllTypes[i].LanduseTypeChsName;
                        else if (System.Threading.Thread.CurrentThread.CurrentCulture.Parent.Name == "zh-CHT")
                            dataGridViewMatrix.Rows[i].HeaderCell.Value = landUseClassificationInfo.AllTypes[i].LanduseTypeChtName;
                        else if (System.Threading.Thread.CurrentThread.CurrentCulture.Parent.Name == "en")
                            dataGridViewMatrix.Rows[i].HeaderCell.Value = landUseClassificationInfo.AllTypes[i].LanduseTypeEnName;

                        for (int j = 0; j < dtMatrix.Columns.Count; j++)
                            dataGridViewMatrix.Rows[i].Cells[j].Value = dtMatrix.Rows[i][j];
                    }
                }
            }
            //切换到模拟过程参数设置页时给定控件初始值，同时保存模拟数据及参数信息
            else if (e.OldIndex == 6 && e.NewIndex == 7)
            {
                simulationStartImageName = comboBoxSimStartImage.SelectedItem.ToString();
                if (comboBoxSimEndImage.SelectedIndex != -1)
                    simulationEndImageName = comboBoxSimEndImage.SelectedItem.ToString();
                convertCount = Convert.ToInt32(numericUpDownConvertCount.Value);
                simulationIterations = Convert.ToInt32(numericUpDownIterations.Value);
                delta = Convert.ToInt32(numericUpDownDelta.Value);
                convertThreshold = Convert.ToSingle(numericUpDownThreshold.Value);

                numericUpDownRefresh.Value = 10;
                numericUpDownOutputImage.Value = 10;
                outputFolder = GetOutputFolder();
                textBoxOutputFolder.Text = outputFolder;
            }
            //切换到完成页时填充摘要，同时保存模拟过程输出参数的信息
            else if (e.OldIndex == 7 && e.NewIndex == 8)
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
                int startUrbanConuts = ArcGISOperator.GetUrbanCount(rasterLayer,landUseClassificationInfo);
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
            System.Diagnostics.Process.Start("http://geosimulation.cn/Publications.html#ANN-CA");
        }

        private void buttonStartTrainning_Click(object sender, EventArgs e)
        {
            labelTrainningAccuracy.Text = "";
            labelValidationAccuracy.Text = "";
            zedGraphControl.GraphPane.Title.Text = "ANN Training Process";
            zedGraphControl.GraphPane.XAxis.Title.Text = "Iterations";
            zedGraphControl.GraphPane.YAxis.Title.Text = "Error (Mean Squared Error)";
            ann = new ArtificalNeuralNetwork();
            double error = ann.TarinNetwork(inputs, outputs, samplingCellsCount, inputNeuronsCount, outputNeuronsCount,
                Convert.ToInt32(numericUpDownHiddenLayerNeurons.Value), Convert.ToDouble(numericUpDownLearningRate.Value),
                Convert.ToInt32(numericUpDownTrainningIterations.Value), labelTrainingProcess,
                labelTrainningAccuracy, labelValidationAccuracy, zedGraphControl);
            labelTrainingProcess.Text = resourceManager.GetString("String69") + "!  Error：" + error.ToString("0.00000");
        }

        private void buttonRetrain_Click(object sender, EventArgs e)
        {
            buttonStartTrainning_Click(sender, e);
        }

        private void numericUpDownDelta_ValueChanged(object sender, EventArgs e)
        {
            delta = Convert.ToInt32(numericUpDownDelta.Value);
        }

        private void numericUpDownThreshold_ValueChanged(object sender, EventArgs e)
        {
            convertThreshold = Convert.ToSingle(numericUpDownThreshold.Value);
        }

        private void buttonLoadMatrix_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = resourceManager.GetString("String52");
                ofd.Title = resourceManager.GetString("String70");
                if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    DataSet ds = new DataSet();
                    ds.ReadXml(ofd.FileName);
                    dtMatrix = ds.Tables[0];

                    dataGridViewMatrix.Columns.Clear();
                    dataGridViewMatrix.Rows.Clear();
                    for (int i = 0; i < dtMatrix.Rows.Count; i++)
                    {
                        if (System.Threading.Thread.CurrentThread.CurrentCulture.Parent.Name == "zh-CHS")
                            dataGridViewMatrix.Columns.Add(landUseClassificationInfo.AllTypes[i].LanduseTypeChsName, landUseClassificationInfo.AllTypes[i].LanduseTypeChsName);
                        else if (System.Threading.Thread.CurrentThread.CurrentCulture.Parent.Name == "zh-CHT")
                            dataGridViewMatrix.Columns.Add(landUseClassificationInfo.AllTypes[i].LanduseTypeChtName, landUseClassificationInfo.AllTypes[i].LanduseTypeChtName);
                        else if (System.Threading.Thread.CurrentThread.CurrentCulture.Parent.Name == "en")
                            dataGridViewMatrix.Columns.Add(landUseClassificationInfo.AllTypes[i].LanduseTypeEnName, landUseClassificationInfo.AllTypes[i].LanduseTypeEnName);
                    }
                    for (int i = 0; i < dtMatrix.Rows.Count; i++)
                    {
                        dataGridViewMatrix.Rows.Add();
                        dataGridViewMatrix.RowHeadersWidth = 110;
                        dataGridViewMatrix.Rows[i].Height = 30;
                        if (System.Threading.Thread.CurrentThread.CurrentCulture.Parent.Name == "zh-CHS")
                            dataGridViewMatrix.Rows[i].HeaderCell.Value = landUseClassificationInfo.AllTypes[i].LanduseTypeChsName;
                        else if (System.Threading.Thread.CurrentThread.CurrentCulture.Parent.Name == "zh-CHT")
                            dataGridViewMatrix.Rows[i].HeaderCell.Value = landUseClassificationInfo.AllTypes[i].LanduseTypeChtName;
                        else if (System.Threading.Thread.CurrentThread.CurrentCulture.Parent.Name == "en")
                            dataGridViewMatrix.Rows[i].HeaderCell.Value = landUseClassificationInfo.AllTypes[i].LanduseTypeEnName;

                        for (int j = 0; j < dtMatrix.Columns.Count; j++)
                            dataGridViewMatrix.Rows[i].Cells[j].Value = dtMatrix.Rows[i][j];
                    }
                }
            }
        }

        private void buttonSaveMatrix_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = resourceManager.GetString("String52");
                sfd.Title = resourceManager.GetString("String71");
                if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    if (dtMatrix.DataSet == null)
                    {
                        DataSet ds = new DataSet();
                        ds.Tables.Add(dtMatrix);
                        ds.WriteXml(sfd.FileName);
                    }
                    else
                        dtMatrix.DataSet.WriteXml(sfd.FileName);
                    System.Windows.Forms.MessageBox.Show(resourceManager.GetString("String55"),
                        resourceManager.GetString("String2"), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
            }

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

        private void pictureBoxLearningRate_Click(object sender, EventArgs e)
        {
            if (labelLearningRate.Visible)
                labelLearningRate.Visible = false;
            else
                labelLearningRate.Visible = true;
        }

        private void pictureBoxInfoTrainningIteration_Click(object sender, EventArgs e)
        {
            if (this.labelTrainingIterations.Visible)
                labelTrainingIterations.Visible = false;
            else
                labelTrainingIterations.Visible = true;
        }

        private void pictureBoxProcessTraining_Click(object sender, EventArgs e)
        {
            if (this.labelExecuteTraining.Visible)
                labelExecuteTraining.Visible = false;
            else
                labelExecuteTraining.Visible = true;
        }

        private void pictureBoxCalCovertAmount_Click(object sender, EventArgs e)
        {
            if (this.labelCalCovertAmount.Visible)
                labelCalCovertAmount.Visible = false;
            else
                labelCalCovertAmount.Visible = true;
        }

        private void dataGridViewMatrix_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex != e.RowIndex)
            {
                if (dataGridViewMatrix.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "1")
                {
                    dataGridViewMatrix.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = 0;
                    dtMatrix.Rows[e.RowIndex][e.ColumnIndex] = 0;
                }
                else
                {
                    dataGridViewMatrix.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = 1;
                    dtMatrix.Rows[e.RowIndex][e.ColumnIndex] = 1;
                }
            }
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

            sb.AppendLine(resourceManager.GetString("String72"));
            sb.AppendLine();

            sb.AppendLine(resourceManager.GetString("String73"));
            foreach (string variableName in listVariableLayersName)
                sb.AppendLine(variableName);

            sb.AppendLine();
            sb.AppendLine(resourceManager.GetString("String74"));
            foreach (StructLanduseInfo structLanduseInfo in landUseClassificationInfo.AllTypes)
            {
                if (System.Threading.Thread.CurrentThread.CurrentCulture.Parent.Name == "zh-CHS")
                    sb.AppendLine(structLanduseInfo.LanduseTypeChsName
                    + resourceManager.GetString("String28") + structLanduseInfo.LanduseTypeValue.ToString());
                else if (System.Threading.Thread.CurrentThread.CurrentCulture.Parent.Name == "zh-CHT")
                    sb.AppendLine(structLanduseInfo.LanduseTypeChtName
                    + resourceManager.GetString("String28") + structLanduseInfo.LanduseTypeValue.ToString());
                else if (System.Threading.Thread.CurrentThread.CurrentCulture.Parent.Name == "en")
                sb.AppendLine(structLanduseInfo.LanduseTypeEnName 
                    + resourceManager.GetString("String28") + structLanduseInfo.LanduseTypeValue.ToString());
            }
            sb.AppendLine();
            sb.AppendLine(resourceManager.GetString("String27"));
            foreach (StructLanduseInfo structLanduseInfo in landUseClassificationInfo.UrbanValues)
            {
                if (System.Threading.Thread.CurrentThread.CurrentCulture.Parent.Name == "zh-CHS")
                    sb.AppendLine(structLanduseInfo.LanduseTypeChsName
                    + resourceManager.GetString("String28") + structLanduseInfo.LanduseTypeValue.ToString());
                else if (System.Threading.Thread.CurrentThread.CurrentCulture.Parent.Name == "zh-CHT")
                    sb.AppendLine(structLanduseInfo.LanduseTypeChtName
                    + resourceManager.GetString("String28") + structLanduseInfo.LanduseTypeValue.ToString());
                else if (System.Threading.Thread.CurrentThread.CurrentCulture.Parent.Name == "en")
                sb.AppendLine(structLanduseInfo.LanduseTypeEnName
                    + resourceManager.GetString("String28") + structLanduseInfo.LanduseTypeValue.ToString());
            }
            sb.AppendLine();

            sb.AppendLine(resourceManager.GetString("String75") + numericUpDownNeiWindow.Value.ToString());
            sb.AppendLine();

            sb.AppendLine(resourceManager.GetString("String76")+simulationStartImageName);
            sb.AppendLine(resourceManager.GetString("String77") + simulationEndImageName);
            sb.AppendLine();

            sb.AppendLine(resourceManager.GetString("String22") + convertCount.ToString() + resourceManager.GetString("String23"));
            sb.AppendLine(resourceManager.GetString("String31") + this.simulationIterations.ToString() + resourceManager.GetString("String24"));
            sb.AppendLine(resourceManager.GetString("String25") + (convertCount / simulationIterations).ToString() + resourceManager.GetString("String23"));
            sb.AppendLine();

            sb.AppendLine(resourceManager.GetString("String79"));
            sb.AppendLine(WriteMatrix(dataGridViewMatrix));

            sb.AppendLine(resourceManager.GetString("String26") + this.delta.ToString());
            sb.AppendLine(resourceManager.GetString("String78") + this.convertThreshold.ToString());
            sb.AppendLine(resourceManager.GetString("String171") + ": " + VariableMaintainer.RestrictLayerName);
            sb.AppendLine();

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
        /// 输出适宜性矩阵为文本。
        /// </summary>
        /// <param name="dataGridView"></param>
        /// <returns></returns>
        private string WriteMatrix(DataGridView dataGridView)
        {
            StringBuilder sb = new StringBuilder();
            string tempString = "";
            for (int i = 0; i < dataGridView.ColumnCount; i++)
                tempString+= dataGridView.Columns[i].HeaderText + ", ";
            tempString = tempString.TrimEnd(',', ' ');
            sb.AppendLine(tempString);
            for (int i = 0; i < dtMatrix.Rows.Count; i++)
            {
                tempString = "";
                for (int j = 0; j < dtMatrix.Columns.Count; j++)
                {
                    tempString += dtMatrix.Rows[i][j].ToString() + ", ";
                }
                tempString=tempString.TrimEnd(',',' ');
                sb.AppendLine(tempString);
            }
            return sb.ToString();
        }

        /// <summary>
        /// 改变UI界面中的语言。
        /// </summary>
        private void ChangeUI()
        {
            //TODO:根据当前系统语言改变界面语言
            this.DataGridViewTextBoxColumnLayers.HeaderText = resourceManager.GetString("String109");
            this.landuseColumnIsUrban.HeaderText = resourceManager.GetString("String112");
            this.ColumnLUValue.HeaderText = resourceManager.GetString("String110");
            this.ColumnLUDescription.HeaderText=resourceManager.GetString("String111");
            this.ColumnLUColor.HeaderText = resourceManager.GetString("String113");
            this.wizardPageWelcome.Title = resourceManager.GetString("String1");
            this.wizardPageWelcome.Description = resourceManager.GetString("String106");
            this.wizardPageIntroduction.Title = resourceManager.GetString("String107");
            this.wizardPageIntroduction.Description = resourceManager.GetString("String108");
            this.wizardPageTrainingDataSettings.Title = resourceManager.GetString("String114");
            this.wizardPageTrainingDataSettings.Description = resourceManager.GetString("String115");
            this.wizardPageDataSampling.Title = resourceManager.GetString("String116");
            this.wizardPageDataSampling.Description = resourceManager.GetString("String117");
            this.wizardPageANNTraining.Title = resourceManager.GetString("String118");
            this.wizardPageANNTraining.Description = resourceManager.GetString("String119");
            this.wizardPageSimulatedData.Title = resourceManager.GetString("String120");
            this.wizardPageSimulatedData.Description = resourceManager.GetString("String121");
            this.wizardPageParameters.Title = resourceManager.GetString("String122");
            this.wizardPageParameters.Description = resourceManager.GetString("String123");
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
            delta = 1;
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
            this.numericUpDownDelta.Value = 1;
            this.numericUpDownIterations.Value = 10;
            numericUpDownThreshold.Value = Convert.ToDecimal(0.9);
            numericUpDownTrainningIterations.Value = 500;
            ann = null;
            isSampled = false;
        }

        /// <summary>
        /// 读取训练起始图像、终止图像、各变量图像为数组
        /// </summary>
        private void ReadTrainningDatas()
        {
            IRasterLayer rasterLayerStartImage = ArcGISOperator.GetRasterLayerByName(comboBoxTrainingStartImage.SelectedItem.ToString());
            IRasterLayer rasterLayerEndImage = ArcGISOperator.GetRasterLayerByName(comboBoxTrainingEndImage.SelectedItem.ToString());
            labelCalculte.Text = resourceManager.GetString("String12");
            Application.DoEvents();
            trainingStartImage = ArcGISOperator.ReadRasterAndGetNotNullRowColumn(rasterLayerStartImage, out structRasterMetaData,
               out listNotNullRows, out listNotNullColumns);
            //获取最小空间范围
            List<IRasterLayer> listVariablesLayers = new List<IRasterLayer>();
            for (int i = 0; i < listVariableLayersName.Count; i++)
                listVariablesLayers.Add(ArcGISOperator.GetRasterLayerByName(listVariableLayersName[i]));
            ArcGISOperator.GetSmallestBound(rasterLayerStartImage, rasterLayerEndImage, listVariablesLayers, ref structRasterMetaData);
            labelCalculte.Text = resourceManager.GetString("String13");
            Application.DoEvents();
            trainningEndImage = ArcGISOperator.ReadRaster(rasterLayerEndImage, structRasterMetaData.NoDataValue);

            listVaribaleImages.Clear();
            foreach (string layerName in listVariableLayersName)
            {
                labelCalculte.Text = resourceManager.GetString("String14") + layerName + ".....";
                Application.DoEvents();
                listVaribaleImages.Add(ArcGISOperator.ReadRaster(ArcGISOperator.GetRasterLayerByName(layerName), structRasterMetaData.NoDataValue));
            }
        }

        #endregion

        private void comboBoxRestrictLayer_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxRestrictLayer.SelectedIndex != -1)
                VariableMaintainer.RestrictLayerName = comboBoxRestrictLayer.Items[comboBoxRestrictLayer.SelectedIndex].ToString();
        }

    }
}
