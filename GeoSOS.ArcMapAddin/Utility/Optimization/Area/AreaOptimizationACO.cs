using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using GeoSOS.ArcMapAddIn.UI.Forms;
using GeoSOS.CommonLibrary.Struct;
using GeoSOS.CommonLibrary;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Carto;
using System.Windows.Forms;
using ESRI.ArcGIS.DataSourcesRaster;
using System.Data;
using ESRI.ArcGIS.Display;
using Evaluant.Calculator;

namespace GeoSOS.ArcMapAddIn.Utility.Optimization
{
    public class AreaOptimizationACO
    {
        //-----------------------------------------------------成员-----------------------------------------------------
        /// <summary>
        /// 成员
        /// </summary>
        #region 成员

        private double q;                       //信息素强度
        private double rho;                      //挥发因子
        private double alpha;                     //信息素权重
        private double beta;                   //启发函数权重
        private double weightSutiable;                      //适宜性权重
        private double weightCompact;                      //紧凑性权重

        private double cellSize;               //网格边长
        private double cellArea;                    //网格面积      
        private float randomBound;             //生成随机数范围

        private int microSearchIterationCount;                 //小搜索次数
        private int currentIterationInMicroSearch;          //小搜索内，当前迭代次数
        private double goalUtilityInMicroSearch;          //小搜索内，最优目标函数值
        private Ant[] goalUtilityAntsStatusInMicroSearch;            //小搜索内，最优状态

        private int currentIteration;                //当前迭代次数
        private int currentRefreshIteration;               //当前更新次数
        private int totalItearationCount;           //总迭代次数
        private double needFinishIterationCount;                 //当连续迭代kFinish次不需要更新时，停止寻址       

        private string outputPath;                 //结果输出路径
        private string outInitName;             //结果输出初始状态文件名
        private string outFileName;                 //结果输出文件名
        private string outPreFileName;              //结果输出文件名前面部分
        private string outGoalName;             //结果输出目标函数名

        /// <summary>
        /// 所有迭代次数中的最优值
        /// </summary>
        private double goalUtilityInAllIterations;
        /// <summary>
        /// 所有迭代次数中的最优状态
        /// </summary>
        private int[,] goalUtilityAntsStatusInAllIterations;

        private Boolean isNeedUpdate;             //判断是否需要更新最优状态
        private Boolean isNeedSave;             //判断是否需要保存结果  
        private Boolean isInitialStatusSave;           //判断是否已经保存初始状态数据
        private Boolean isUseCompactness;       //判断是否使用紧凑性

        private float[,] suitabilityData;          //适宜性数据
        private float[,] pheromoneData;         //信息素数据
        private float[,] heuristicData;              //启发函数数据
        private int[,] occupiedStatus;                  //判断当前位置是否有智能体，有则为1 ,没有则为0,没数据为-9999
        private string[] goalUtilityArray;              //用于保存目标函数值

        //private int rows;                       //网格行数
        //private int cols;                       //网格列数
        //private double xllcorner;               //输出文件左上角横坐标
        //private double yllcorner;               //输出文件左上角纵坐标
        //private double cellsize;                //栅格大小
        //private double noDataValue;            //无数据值

        private int antsCount;                //智能体个数
        private Ant[] arrayAnts;                 //智能体数组

        private Random random;                 //随机数产生器

        private DockableWindowOutput dockableWindowOutput;          //消息输出窗口
        private DockableWindowGraphy dockableWindowGraphy;      //图表窗口
        private StringBuilder stringBuilderMessage;    //消息字符串
        private StructRasterMetaData structRasterMetaData;  //当前栅格的基本信息

        #endregion
        //--------------------------------------------------------------------------------------------------------------

        //-------------------------------------------------属性---------------------------------------------------------
        /// <summary>
        /// 属性
        /// </summary>
        #region 属性

        /// <summary>
        /// 启发函数权重
        /// </summary>
        public double Beita
        {
            get
            {
                return beta;
            }
            set
            {
                beta = value;
            }
        }

        /// <summary>
        /// 信息素强度
        /// </summary>
        public double Q
        {
            get
            {
                return q;
            }
            set
            {
                q = value;
            }
        }

        /// <summary>
        /// 信息素权重
        /// </summary>
        public double Rfa
        {
            get
            {
                return alpha;
            }
            set
            {
                alpha = value;
            }
        }

        /// <summary>
        /// 挥发因子
        /// </summary>
        public double Ro
        {
            get
            {
                return rho;
            }
            set
            {
                rho = value;
            }
        }

        /// <summary>
        /// 紧凑性权重
        /// </summary>
        public double Wc
        {
            get
            {
                return weightCompact;
            }
            set
            {
                weightCompact = value;
            }
        }

        /// <summary>
        /// 阻抗性权重
        /// </summary>
        public double Wk
        {
            get
            {
                return weightSutiable;
            }
            set
            {
                weightSutiable = value;
            }
        }

        /// <summary>
        /// 网格边长
        /// </summary>
        public double BianChang
        {
            get
            {
                return cellSize;
            }
            set
            {
                cellSize = value;
            }
        }

        /// <summary>
        /// 当前迭代次数
        /// </summary>
        public int StickNumber
        {
            get
            {
                return currentIteration;
            }
            set
            {
                currentIteration = value;
            }
        }

        /// <summary>
        /// 小搜索次数
        /// </summary>
        public int XiaoNumber
        {
            get
            {
                return microSearchIterationCount;
            }
            set
            {
                microSearchIterationCount = value;
            }
        }


        /// <summary>
        /// 全局最优状态
        /// </summary>
        public int[,] AllOptStatus
        {
            get
            {
                return goalUtilityAntsStatusInAllIterations;
            }
        }

        /// <summary>
        /// 判断是否需要更新
        /// </summary>
        public Boolean NeedUpdate
        {
            get
            {
                return isNeedUpdate;
            }
        }

        public float[,] SuitabilityData
        {
            get { return suitabilityData; }
        }

        #endregion
        //--------------------------------------------------------------------------------------------------------------
        
        //------------------------------------------------方法----------------------------------------------------------

        /// <summary>
        /// 方法
        /// </summary>
        #region 方法

        /// <summary>
        /// 初始化方法
        /// </summary>
        #region ----------------初始化方法------------------------

        public void Initialize(StructACOParameters structACOParameters, int microSearchIterationCountValue, 
            Boolean isNeedSaveValue, DockableWindowOutput currentDockableWindowOutput, 
            DockableWindowGraphy currentDockableWindowGraphy, Boolean isUseCompactnessValue)
        {

            q = structACOParameters.Q;                     //信息素强度
            rho = structACOParameters.Rho;                   //挥发因子
            alpha = structACOParameters.Alpha;                 //信息素权重
            beta = structACOParameters.Beta;             //启发函数权重
            weightSutiable = structACOParameters.WeightSuitable;                   //阻抗性权重
            weightCompact = structACOParameters.WeightCompact;                   //紧凑性权重

            totalItearationCount = structACOParameters.InterationCount;         //总迭代次数
            currentIteration = 0;                                //当前迭代次数
            currentRefreshIteration = 0;                                //更新次数
            needFinishIterationCount = 0;

            microSearchIterationCount = microSearchIterationCountValue;                     //小搜索次数
            goalUtilityAntsStatusInMicroSearch = new Ant[structACOParameters.AntCount];          //小搜索内最优状态
            currentIterationInMicroSearch = 0;
            goalUtilityInMicroSearch = 0;

            isNeedSave = isNeedSaveValue;                     //判断是否需要保存结果
            isNeedUpdate = false;                           //初始化判断是否需要更新最优状态
            isInitialStatusSave = false;                         //判断是否已经保存初始状态数据

            //rows = rrows;                       //网格行数
            //cols = ccols;                       //网格列数
            //xllcorner = xxllcorner;             //输出文件左上角横坐标
            //yllcorner = yyllcorner;             //输出文件左上角纵坐标
            //cellsize = ccellsize;               //栅格大小
            //noDataValue = nnodata_value;       //无数据值

            outputPath = structACOParameters.OutputFolder;                 //结果输出路径
            outInitName = "_Init.txt";          //结果输出初始状态文件名
            outFileName = "";                       //结果输出文件名
            outPreFileName = "g";                   //结果输出文件名前面部分
            outGoalName = "GoalFunction.txt";   //结果输出目标函数名

            antsCount = structACOParameters.AntCount;                    //智能体个数 
            //cellSize = bbianChang;                        //网格边长
            //cellArea = cellSize * cellSize * antsCount;    //智能体面积
            

            goalUtilityInAllIterations = 0;
            randomBound = 1;

            goalUtilityArray = new string[totalItearationCount + 2];   //保存目标函数值数据

            suitabilityData = GetSuitabilityData(VariableMaintainer.OptimizationExpression);
            InitializeOccupiedAndGoalUtilityAntsStatusInAllIterations();      //初始化挖空
            InitializeAnts();                     //初始化智能体
            InitializePheromone();                  //初始化信息素
            InitializeHeuristicFunction();                       //初始化启发函数    

            string simulationLayerName = VariableMaintainer.CurrentFoucsMap.get_Layer(0).Name;
            IRasterLayer simulationImageLayer = ArcGISOperator.GetRasterLayerByName(simulationLayerName);
            string dateTime = GeneralOpertor.GetDataTimeFullString(DateTime.Now);
            string rasterName = "ACOArea" + dateTime + ".img";
            IRasterDataset rst = ArcGISOperator.CreateRasterDataset(VariableMaintainer.DefaultOutputFolder,
                rasterName, simulationImageLayer, structRasterMetaData, occupiedStatus, 0);
            IRasterLayer n = new RasterLayerClass();
            n.CreateFromDataset(rst);
            IColorRamp colorRamp = new RandomColorRampClass();
            ArcGISOperator.UniqueValueRenderer(colorRamp, n, "Value");
            ArcGISOperator.FoucsMap.AddLayer((ILayer)n);

            double pixelWidth = ((IRasterDefaultProps)n.Raster).DefaultPixelWidth;
            double pixelHeight = ((IRasterDefaultProps)n.Raster).DefaultPixelHeight;
            cellArea = pixelWidth * pixelHeight * antsCount;
            cellSize = pixelWidth;
            random = new Random();

            dockableWindowOutput = currentDockableWindowOutput;
            dockableWindowGraphy = currentDockableWindowGraphy;
            stringBuilderMessage = new StringBuilder();
            isUseCompactness = isUseCompactnessValue;

            dockableWindowGraphy.GraphTitle = "Goal Utility";
            dockableWindowGraphy.XAxisTitle = "Iteration";
            dockableWindowGraphy.YAxisTitle = "Goal Utility Value";
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void InitializeOccupiedAndGoalUtilityAntsStatusInAllIterations()
        {
            occupiedStatus = new int[structRasterMetaData.RowCount, structRasterMetaData.ColumnCount];
            goalUtilityAntsStatusInAllIterations = new int[structRasterMetaData.RowCount, structRasterMetaData.ColumnCount];
            for (int i = 0; i < structRasterMetaData.RowCount; i++)
            {
                for (int j = 0; j < structRasterMetaData.ColumnCount; j++)
                {
                    if (suitabilityData[i, j] == structRasterMetaData.NoDataValue)
                    {
                        occupiedStatus[i, j] = Convert.ToInt32(structRasterMetaData.NoDataValue);
                        goalUtilityAntsStatusInAllIterations[i, j] = Convert.ToInt32(structRasterMetaData.NoDataValue);
                    }
                    else
                    {
                        occupiedStatus[i, j] = 0;
                        goalUtilityAntsStatusInAllIterations[i, j] = 0;
                    }
                }
            }
        }

        /// <summary>
        /// 初始化智能体
        /// </summary>
        private void InitializeAnts()
        {
            arrayAnts = new Ant[antsCount];
            for (int i = 0; i < antsCount; i++)
            {
                arrayAnts[i] = new Ant();
                Boolean isOccupied = false;
                int count = 0;
                //int countLimit = 10 * cols * rows;
                Random random = new Random();
                //while ((isOccupied == false) && (count < countLimit))
                while ((isOccupied == false) && (count < antsCount))
                {
                    int x = (int)(random.Next(structRasterMetaData.RowCount));
                    int y = (int)(random.Next(structRasterMetaData.ColumnCount));
                    if (occupiedStatus[x, y] == 0)
                    {
                        arrayAnts[i].SetXY(x, y);
                        goalUtilityAntsStatusInAllIterations[x, y] = 1;
                        occupiedStatus[x, y] = 1;
                        isOccupied = true;
                    }
                    count++;
                }
            }
        }

        /// <summary>
        /// 初始化信息素
        /// </summary>
        private void InitializePheromone()
        {
            pheromoneData = new float[structRasterMetaData.RowCount, structRasterMetaData.ColumnCount];
            for (int i = 0; i < structRasterMetaData.RowCount; i++)
            {
                for (int j = 0; j < structRasterMetaData.ColumnCount; j++)
                {
                    if (suitabilityData[i, j] != structRasterMetaData.NoDataValue)
                    {
                        //pheromoneData[i, j] = 10;
                        pheromoneData[i, j] = Convert.ToSingle(q);
                    }
                    else
                    {
                        pheromoneData[i, j] = 0;
                    }
                }
            }
        }

        /// <summary>
        /// 初始化启发函数
        /// </summary>
        private void InitializeHeuristicFunction()
        {
            heuristicData = new float[structRasterMetaData.RowCount, structRasterMetaData.ColumnCount];
            float suitabilitySum = 0;
            for (int i = 0; i < structRasterMetaData.RowCount; i++)
            {
                for (int j = 0; j < structRasterMetaData.ColumnCount; j++)
                {
                    if (suitabilityData[i, j] != structRasterMetaData.NoDataValue)
                        suitabilitySum += suitabilityData[i, j];
                }
            }
            for (int i = 0; i < structRasterMetaData.RowCount; i++)
            {
                for (int j = 0; j < structRasterMetaData.ColumnCount; j++)
                {
                    if (suitabilityData[i, j] != structRasterMetaData.NoDataValue)
                    {
                        heuristicData[i, j] = suitabilityData[i, j] / suitabilitySum * 10;
                    }
                    else
                    {
                        heuristicData[i, j] = 0;
                    }
                }
            }
        }

        #endregion  -------------------------------------------------------------------------------------


        /// <summary>
        /// 求评价函数方法
        /// </summary>
        #region ---------------求评价函数方法----------------------

        /// <summary>
        /// 判断网格是否被占用
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private Boolean IsOccupied(int x, int y)
        {
            Boolean retVal = false;
            if (occupiedStatus[x, y] == 1) retVal = true;
            return retVal;
        }

        /// <summary>
        /// 计算周长
        /// </summary>
        /// <param name="Ants"></param>
        /// <returns></returns>
        private double GetPerimeter(Ant[] Ants)
        {
            double perimeter = 0;
            int count = Ants.Length;
            for (int i = 0; i < count; i++)
            {
                int x = Ants[i].X;
                int y = Ants[i].Y;
                if (x - 1 >= 0)
                {
                    if (IsOccupied(x - 1, y) == false) perimeter += 1;
                }
                if (x + 1 < structRasterMetaData.RowCount)
                {
                    if (IsOccupied(x + 1, y) == false) perimeter += 1;
                }
                if (y - 1 >= 0)
                {
                    if (IsOccupied(x, y - 1) == false) perimeter += 1;
                }
                if (y + 1 < structRasterMetaData.ColumnCount)
                {
                    if (IsOccupied(x, y + 1) == false) perimeter += 1;
                }
            }
            perimeter = perimeter * cellSize;
            return perimeter;
        }

        /// <summary>
        /// 求紧凑函数,面积除以周长,越大越好
        /// </summary>
        /// <param name="Ants"></param>
        /// <returns></returns>
        private double GetCompactness(Ant[] Ants)
        {
            return Math.Sqrt(cellArea) / GetPerimeter(Ants);
        }

        /// <summary>
        /// 求智能体所在网格适宜性平均值，越大越好
        /// </summary>
        /// <param name="Ants"></param>
        /// <returns></returns>
        private double GetSuitableMean(Ant[] Ants)
        {
            double goalSum = 0;
            int count = Ants.Length;
            for (int i = 0; i < count; i++)
            {
                int x = Ants[i].X;
                int y = Ants[i].Y;
                goalSum += suitabilityData[x, y];
            }
            return goalSum / count;
        }

        /// <summary>
        /// 求评价函数，越大越好
        /// </summary>
        /// <param name="Ants"></param>
        /// <returns></returns>
        private double GetGoalFun(Ant[] Ants)
        {
            double goalFun = 0;
            //goalFun=0.4*getGoalMean(Ants)+ 0.6*getCompactness(Ants);
            //goalFun=getGoalMean(Ants)*getCompactness(Ants);
            if (isUseCompactness == true)
            {
                goalFun = Math.Pow(GetSuitableMean(Ants), weightSutiable) * Math.Pow(GetCompactness(Ants), weightCompact);
                //goalFun = 0.4 * getSuitableMean(Ants) + 0.6 * getCompactness(Ants);
            }
            else
            {
                goalFun = GetSuitableMean(Ants);
            }
            return goalFun;
        }

        #endregion ----------------------------------------------------------------------------


        /// <summary>
        /// 选址方法
        /// </summary>
        #region ----------------------选址方法--------------------------------

        /// <summary>
        /// 每迭代一次的概率
        /// </summary>
        /// <returns></returns>
        private float[,] GetP()
        {
            float[,] P = new float[structRasterMetaData.RowCount, structRasterMetaData.ColumnCount];
            float pSum = 0;
            for (int i = 0; i < structRasterMetaData.RowCount; i++)
            {
                for (int j = 0; j < structRasterMetaData.ColumnCount; j++)
                {
                    if (suitabilityData[i, j] != structRasterMetaData.NoDataValue)
                    {
                        pSum += Convert.ToSingle(Math.Pow(pheromoneData[i, j], alpha) * Math.Pow(heuristicData[i, j], beta));
                    }
                }
            }
            for (int i = 0; i < structRasterMetaData.RowCount; i++)
            {
                for (int j = 0; j < structRasterMetaData.ColumnCount; j++)
                {
                    if (suitabilityData[i, j] != structRasterMetaData.NoDataValue)
                    {
                        P[i, j] = Convert.ToSingle(Math.Pow(pheromoneData[i, j], alpha) * Math.Pow(heuristicData[i, j], beta) / pSum);
                    }
                    else
                    {
                        P[i, j] = 0;
                    }
                }
            }
            return P;
        }

        /// <summary>
        /// 设置小搜索内的最优状态，并获得小搜索内的最优目标函数
        /// </summary>
        /// <param name="Ants"></param>
        /// <param name="k"></param>
        private void SetOptAnts(Ant[] Ants, int k)
        {
            if (microSearchIterationCount == 1)
            {
                goalUtilityAntsStatusInMicroSearch = Ant.DuplicateAgents(Ants);
                goalUtilityInMicroSearch = GetGoalFun(Ants);
            }
            else
            {
                if (k % microSearchIterationCount == 1)
                {
                    goalUtilityAntsStatusInMicroSearch = Ant.DuplicateAgents(Ants);
                    goalUtilityInMicroSearch = GetGoalFun(Ants);
                }
                else
                {
                    double currGoalFucn = GetGoalFun(Ants);
                    if (goalUtilityInMicroSearch < currGoalFucn)
                    {
                        goalUtilityAntsStatusInMicroSearch = Ant.DuplicateAgents(Ants);
                        goalUtilityInMicroSearch = currGoalFucn;
                    }
                }
            }
        }

        /// <summary>
        /// 更新信息素，同时更新最优状态
        /// </summary>
        /// <param name="Ants"></param>
        private void UpdatePheromoneData(Ant[] Ants)
        {
            for (int i = 0; i < structRasterMetaData.RowCount; i++)
            {
                for (int j = 0; j < structRasterMetaData.ColumnCount; j++)
                {
                    if (suitabilityData[i, j] != structRasterMetaData.NoDataValue)
                    {
                        //计算剩下的信息素
                        pheromoneData[i, j] = (float)(pheromoneData[i, j] * (1 - rho));
                        if (goalUtilityInMicroSearch > goalUtilityInAllIterations) goalUtilityAntsStatusInAllIterations[i, j] = 0;
                    }
                }
            }

            double onePheromon = q * goalUtilityInMicroSearch;

            int conNN = 5;
            if (currentIteration > 200 && currentIteration <= 700) { conNN = 5; /*Beita=3;*/}
            if (currentIteration > 700) { conNN = 3; /*Beita=3;*/}

            for (int i = 0; i < Ants.Length; i++)
            {

                int x = Ants[i].X;
                int y = Ants[i].Y;

                //-------------计算增加的信息素-----------------------------------------------
                pheromoneData[x, y] += (float)onePheromon;                              //更新信息素
                UpdateNeighbour(pheromoneData, conNN, onePheromon, structRasterMetaData.ColumnCount, structRasterMetaData.RowCount, y, x);      //更新邻域信息素；
                //-------------------------------------------------------------------------       	

                //判断是否需要更新最优状态
                if (goalUtilityInMicroSearch > goalUtilityInAllIterations) goalUtilityAntsStatusInAllIterations[x, y] = 1;
            }

            if (goalUtilityInMicroSearch > goalUtilityInAllIterations)
            {
                goalUtilityInAllIterations = goalUtilityInMicroSearch;
                isNeedUpdate = true;
                needFinishIterationCount = 1;
                currentRefreshIteration += 1;
            }
            else
            {
                needFinishIterationCount += 1;
            }

            currentIteration += 1;
            dockableWindowOutput.AppendText("CurrentInteration=" + currentIteration + "     CurrentRefreshIteration=" + currentRefreshIteration + "     NeedUpdate=" + isNeedUpdate + "     NeedFinishIterationCount=" + needFinishIterationCount + "       GoalUtilityValueInAllIterations=" + goalUtilityInAllIterations + "\r\n");
            dockableWindowOutput.AppendText(("\n"));
            dockableWindowOutput.ScrollTextbox();

            if (currentIteration == 1)
            {
                dockableWindowGraphy.CreateGraph(currentIteration, goalUtilityInAllIterations, "");
                dockableWindowGraphy.RefreshGraph();
            }
            else
            {
                dockableWindowGraphy.UpdateData(currentIteration, goalUtilityInAllIterations, 0);
                dockableWindowGraphy.RefreshGraph();
            }

            float[,] currentOccpied = new float[structRasterMetaData.RowCount, structRasterMetaData.ColumnCount];
            for (int i = 0; i < structRasterMetaData.RowCount; i++)
                for (int j = 0; j < structRasterMetaData.ColumnCount; j++)
                {
                    if (suitabilityData[i, j] == -9999f)
                        currentOccpied[i, j] = 255;
                    else
                    {
                        if (occupiedStatus[i, j] == 1)
                            currentOccpied[i, j] = Convert.ToSingle(occupiedStatus[i, j]);
                        else
                            currentOccpied[i, j] = 0;
                    }
                }

            int mapRefreshInterval = VariableMaintainer.CurrentStructACOParameters.MapRefreshInterval;
            if (mapRefreshInterval == 0)
                mapRefreshInterval = 5;
            if (currentIteration % mapRefreshInterval == 0)
            {
                string simulationLayerName = VariableMaintainer.CurrentFoucsMap.get_Layer(0).Name;
                IRasterLayer simulationImageLayer = new RasterLayerClass();
                simulationImageLayer.CreateFromFilePath(VariableMaintainer.DefaultOutputFolder + @"\" + simulationLayerName);
                ArcGISOperator.WriteRaster(simulationImageLayer, currentOccpied);
                IRasterLayer l = ArcGISOperator.GetRasterLayerByName(simulationLayerName);
                ((IRasterEdit)l.Raster).Refresh();

                IActiveView activeView = VariableMaintainer.CurrentFoucsMap as IActiveView;
                activeView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, null);
            }

            //保存每次迭代的目标值，如果需要保存		
            if (isNeedSave == true)
            {
                goalUtilityArray[currentIteration + 1] = currentIteration + "," + currentRefreshIteration + "," + isNeedUpdate + "," + needFinishIterationCount + "," + goalUtilityInAllIterations;
                if (currentIteration == totalItearationCount)
                {
                    string outResult = outputPath + outGoalName;
                    //CommonOperator.writeGoalData(outResult, goalSave);
                }
            }

            //判断是否需要把结果保存成文件		
            if (isNeedSave == true && needFinishIterationCount == 1)
            {
                outFileName = outPreFileName + currentRefreshIteration + "_s" + currentIteration + ".txt";
                String outResult = outputPath + outFileName;

                dockableWindowOutput.AppendText("Writing file：" + outResult + "\r\n");
                dockableWindowOutput.ScrollTextbox();

                //CommonOperator.WriteData(outResult, allOptStatus, rows, cols, xllcorner, yllcorner, cellsize, NODATA_value);

                dockableWindowOutput.AppendText("Writing file finished\r\n");
                dockableWindowOutput.ScrollTextbox();
            }
        }

        /// <summary>
        /// 在n*n邻域内更新信息素
        /// </summary>
        /// <param name="pheromone"></param>
        /// <param name="n"></param>
        /// <param name="onePheromon"></param>
        /// <param name="cols"></param>
        /// <param name="rows"></param>
        /// <param name="col"></param>
        /// <param name="row"></param>
        private void UpdateNeighbour(float[,] pheromone, int n, double onePheromon, int cols, int rows, int col, int row)
        {
            int leftRightCount;             //左和右有多少列，或者上和下有多少行
            leftRightCount = (n - 1) / 2;

            for (int i = row - leftRightCount; i <= row + leftRightCount; i++)
            {
                //如果i列没有超出范围
                if (i >= 0 && i < rows)
                {
                    for (int j = col - leftRightCount; j <= col + leftRightCount; j++)
                    {
                        //如果j行没有超出范围
                        if (j >= 0 && j < cols)
                        {
                            if (i == row && j == col) continue;
                            if (pheromone[i, j] != structRasterMetaData.NoDataValue)
                            {
                                pheromone[i, j] += (float)(onePheromon / (n * n - 1));
                                //pheromone[j][i]+=onePheromon/n;
                            }
                        }
                    }
                }
            }
        }
        
        /// <summary>
        /// update wakong[,]数组
        /// </summary>
        private void UpdateNoDataValue()
        {
            for (int i = 0; i < structRasterMetaData.ColumnCount; i++)
            {
                for (int j = 0; j < structRasterMetaData.RowCount; j++)
                {
                    if (occupiedStatus[j, i] != structRasterMetaData.NoDataValue)
                    {
                        occupiedStatus[j, i] = 0;
                    }
                }
            }
        }

        /// <summary>
        /// 按概率选择下一个位置
        /// </summary>
        /// <param name="pCur"></param>
        /// <returns></returns>
        private Point SelectPosition(float[,] pCur)
        {
            Boolean found = false;
            Boolean foundHalf = false;
            //Random rdm = new Random();    
            //float rdmNumber = (float)(rdm.NextDouble() * randomFanwei);
            float rdmNumber = (float)(random.NextDouble() * randomBound);
            float pSum = 0;
            int x = 0, y = 0;
            //---------------------------------------------------------------            
            while (found == false)
            {
                for (int i = 0; i < structRasterMetaData.RowCount; i++)
                {
                    for (int j = 0; j < structRasterMetaData.ColumnCount; j++)
                    {
                        if (occupiedStatus[i, j] == 0)
                        {
                            pSum += pCur[i, j];
                            if (pSum >= rdmNumber)
                            {
                                x = i;
                                y = j;
                                occupiedStatus[i, j] = 1;
                                randomBound = randomBound - pCur[i, j];
                                foundHalf = true;
                                break;
                            }
                        }
                    }
                    if (foundHalf == true) break;
                }
                found = true;
            }
            //System.out.println("selectPosition is ok!");
            Point foundP = new Point();
            foundP.X = x;
            foundP.Y = y;
            return foundP;
        }

        /// <summary>
        /// 按概率选择位置，选择所有位置
        /// </summary>
        /// <param name="pCur"></param>
        /// <returns></returns>
        private Point[] SelectPositions(float[,] pCur)
        {
            int selectedNumber;                                   //选择的位置数
            float[] rdms;                                           //随机数数组
            Point[] points;                                       //选择的位置
            float pSum;                                           //概率和
            float totalSum;                                       //所有概率和

            totalSum = 1;
            selectedNumber = 0;
            points = new Point[antsCount];

            //int k=0;

            while (selectedNumber < antsCount)
            {
                //k++;
                //MessageString.AppendLine("k=" + Convert.ToString(k));
                //frmMessage.TextBox.Text = MessageString.ToString();
                //frmMessage.TextBox.SelectionStart = frmMessage.TextBox.Text.Length;
                //frmMessage.TextBox.ScrollToCaret();

                pSum = 0;
                int unSelectNumber = antsCount - selectedNumber;     //未选择的位置数目
                int rdmK = 0;                                          //随机数当前位置
                Boolean isRdmKFinished = false;                        //判断rdmK是否超出上限，超出为真，未超出为假

                //产生随机数
                rdms = new float[unSelectNumber];
                for (int i = 0; i < unSelectNumber; i++) { rdms[i] = (float)(random.NextDouble() * totalSum); }
                //对随机数进行排序，从小到大排列
                float temp;
                for (int i = 0; i < unSelectNumber - 1; i++)
                {
                    for (int j = i + 1; j < unSelectNumber; j++)
                    {
                        if (rdms[i] > rdms[j])
                        {
                            temp = rdms[i];
                            rdms[i] = rdms[j];
                            rdms[j] = temp;
                        }
                    }
                }

                //开始选择
                for (int i = 0; i < structRasterMetaData.RowCount; i++)
                {
                    for (int j = 0; j < structRasterMetaData.ColumnCount; j++)
                    {
                        if (occupiedStatus[i, j] == 0)
                        {
                            pSum += pCur[i, j];
                            if (pSum >= rdms[rdmK])
                            {
                                points[selectedNumber] = new Point();
                                points[selectedNumber].X = i;
                                points[selectedNumber].Y = j;
                                selectedNumber++;
                                occupiedStatus[i, j] = 1;
                                totalSum = totalSum - pCur[i, j];
                                rdmK++;
                                if (rdmK < unSelectNumber)
                                {
                                    while (pSum >= rdms[rdmK])
                                    {
                                        rdmK++;
                                        if (rdmK == unSelectNumber)
                                        {
                                            isRdmKFinished = true;
                                            break;
                                        }
                                    }
                                }
                            }
                            if (rdmK == unSelectNumber)
                            {
                                isRdmKFinished = true;
                                break;
                            }
                        }
                    }
                    if (isRdmKFinished == true) break;
                }
            }
            return points;
        }

        /// <summary>
        /// 对于每一个智能体，下一步的操作
        /// </summary>
        /// <param name="ant"></param>
        /// <param name="pCur"></param>
        private void AntAction(Ant ant, float[,] pCur)
        {
            Point points;
            points = SelectPosition(pCur);
            ant.SetXY(points.X, points.Y);
        }

        /// <summary>
        /// 对于全部智能体，下一步的操作
        /// </summary>
        /// <param name="ant"></param>
        /// <param name="pCur"></param>
        private void AntActions(Ant[] ants, float[,] pCur)
        {
            Point[] points;
            points = SelectPositions(pCur);
            for (int i = 0; i < antsCount; i++)
            {
                ants[i].SetXY(points[i].X, points[i].Y);
            }
        }

        /// <summary>
        /// 智能体每步执行的操作方法
        /// </summary>
        /// <param name="Ants"></param>
        private void Step(Ant[] Ants)
        {
            //===============是否需要保存初始状态=========================================================
            if (isNeedSave == true && currentIteration == 0 && isInitialStatusSave == false)
            {
                string outResult = outputPath + outInitName;
                //CommonOperator.WriteData(outResult, allOptStatus, rows, cols, yllcorner, yllcorner, cellsize, NODATA_value);
                goalUtilityArray[0] = "stepNum,genXinNum,needUpdate,kFinish,allOptGoalFun";
                goalUtilityArray[1] = 0 + "," + 0 + "," + true + "," + 0 + "," + GetGoalFun(Ants);
                isInitialStatusSave = true;
            }
            //==========================================================================================

            currentIterationInMicroSearch += 1;          //当前小搜索次数+1
            float[,] pCur = GetP();          //获取全局搜索概率
            randomBound = 1;               //范围为1
            UpdateNoDataValue();                //更新挖空图层		

            ////每一个智能体进行选址
            //for (int i = 0; i < Ants.Length; i++)
            //{
            //    antAction(Ants[i], pCur);
            //}

            //对全部智能体进行选址
            AntActions(Ants, pCur);

            //更新小搜索内的最优状态
            SetOptAnts(Ants, currentIterationInMicroSearch);

            if (microSearchIterationCount > 1)
            {
                if (currentIterationInMicroSearch % microSearchIterationCount == 0)
                {
                    UpdatePheromoneData(goalUtilityAntsStatusInMicroSearch);    //更新信息素的同时应该更新全局最优状态
                    currentIterationInMicroSearch = 0;
                }
            }
            else
            {
                UpdatePheromoneData(goalUtilityAntsStatusInMicroSearch);        //更新信息素的同时应该更新全局最优状态
                currentIterationInMicroSearch = 0;
            }
        }

        /// <summary>
        /// 启动蚁群选址方法
        /// </summary>
        public void Run()
        {
            //Agents = ACO_Ant.BreakOrder(Agents);
            isNeedUpdate = false;
            while (this.currentIteration < totalItearationCount)
                Step(arrayAnts);
            //ArcMap.Application.StatusBar.set_Message(0, VariableMaintainer.CurrentResourceManager.GetString("String170"));
            VariableMaintainer.CurrentDockableWindowOutput.AppendText(VariableMaintainer.CurrentResourceManager.GetString("String170") + goalUtilityInAllIterations + "\n");
            VariableMaintainer.CurrentDockableWindowOutput.AppendText("\n");
        }

        #endregion ----------------------------------------------------------------


        #region 获取适宜性图层
        public float[,] GetSuitabilityData(string expression)
        {
            string[] splitedExpressionArray;
            bool isSplited = SplitExpression(expression, out splitedExpressionArray);
            if (isSplited == false)
            {
                MessageBox.Show("Suitibility Function Express Error", VariableMaintainer.CurrentResourceManager.GetString("String2"),
                            MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return null;
            }
            //根据获取的文件名字符串去读取相应的栅格文件
            List<float[,]> listLayers = new List<float[,]>();
            structRasterMetaData = new StructRasterMetaData();
            for (int i = 0; i < splitedExpressionArray.Length; i++)
            {
                if (i > 0 && i % 2 > 0)
                {
                    string layerName = splitedExpressionArray[i];
                    if (i == 1)
                        listLayers.Add(ArcGISOperator.ReadRasterAndGetMetaData(
                            ArcGISOperator.GetRasterLayerByName(layerName), out structRasterMetaData));
                    else
                        listLayers.Add(ArcGISOperator.ReadRaster(ArcGISOperator.GetRasterLayerByName(layerName),
                            structRasterMetaData.NoDataValue));
                }
            }
            if (listLayers.Count == 0)
            {
                MessageBox.Show("Please select layers for calculation!", VariableMaintainer.CurrentResourceManager.GetString("String2"),
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return null;
            }
            //读取全部文件后进行计算，生成适宜性图层数据，并进行归一化
            float[,] suitabilityData = new float[structRasterMetaData.RowCount, structRasterMetaData.ColumnCount];
            float min = 1, max = 0;
            Expression ex = new Expression("1+1");
            //MathParser mathParser = new MathParser();
            for (int i = 0; i < structRasterMetaData.RowCount; i++)
            {
                for (int j = 0; j < structRasterMetaData.ColumnCount; j++)
                {
                    bool isNullData = false;
                    for (int k = 0; k < listLayers.Count; k++)
                        if (listLayers[k][i, j] == structRasterMetaData.NoDataValue)
                        {
                            isNullData = true;
                            break;
                        }
                    if (isNullData)
                        suitabilityData[i, j] = structRasterMetaData.NoDataValue;
                    else
                    {
                        string calculateExpression = "";
                        for (int w = 0; w < splitedExpressionArray.Length; w++)
                        {
                            if (w > 0 && w % 2 == 1)
                                calculateExpression += listLayers[w / 2][i, j];
                            else
                                calculateExpression += splitedExpressionArray[w];
                        }
                        ex = new Expression(calculateExpression);
                        suitabilityData[i, j] = Convert.ToSingle(ex.Evaluate());
                        //suitabilityData[i, j] = Convert.ToSingle(mathParser.Parse(calculateExpression, false));
                        if (suitabilityData[i, j] < min)
                            min = suitabilityData[i, j];
                        if (suitabilityData[i, j] > max)
                            max = suitabilityData[i, j];
                    }
                }
            }
            float[,] suitabilityData2 = new float[structRasterMetaData.RowCount, structRasterMetaData.ColumnCount];
            for (int i = 0; i < structRasterMetaData.RowCount; i++)
                for (int j = 0; j < structRasterMetaData.ColumnCount; j++)
                {
                    if (suitabilityData[i, j] == structRasterMetaData.NoDataValue)
                        suitabilityData2[i, j] = structRasterMetaData.NoDataValue;
                    else
                        suitabilityData2[i, j] = (suitabilityData[i, j] - min) / (max - min);
                }

            //将其他图层关闭，以免影响蚁群优化的刷新效果
            for (int i = 0; i < VariableMaintainer.CurrentFoucsMap.LayerCount; i++)
            {
                if (VariableMaintainer.CurrentFoucsMap.get_Layer(i).Visible)
                    VariableMaintainer.CurrentFoucsMap.get_Layer(i).Visible = false;
            }
            //显示适宜性图层
            string dateTime = GeneralOpertor.GetDataTimeFullString(DateTime.Now);
            string rasterName = "suitability" + dateTime + ".img";
            ArcMap.Application.StatusBar.set_Message(0, VariableMaintainer.CurrentResourceManager.GetString("String169"));
            VariableMaintainer.CurrentDockableWindowOutput.AppendText(VariableMaintainer.CurrentResourceManager.GetString("String169") + rasterName + ".....\n");
            VariableMaintainer.CurrentDockableWindowOutput.AppendText("\n");

            IRasterLayer templateImageLayer = ArcGISOperator.GetRasterLayerByName(
               splitedExpressionArray[1].ToString());
            IWorkspaceFactory workspaceFactory = new RasterWorkspaceFactoryClass();
            IWorkspace workspace = workspaceFactory.OpenFromFile(VariableMaintainer.DefaultOutputFolder, 0);
            ISaveAs2 saveAs2 = (ISaveAs2)templateImageLayer.Raster;
            saveAs2.SaveAs(rasterName, workspace, "IMAGINE Image");
            IRasterLayer sutiabilityImageLayer = new RasterLayerClass();
            sutiabilityImageLayer.CreateFromFilePath(VariableMaintainer.DefaultOutputFolder + @"\" + rasterName);
            //sutiabilityImageLayer.Renderer = templateImageLayer.Renderer;
            ArcGISOperator.FoucsMap.AddLayer((ILayer)sutiabilityImageLayer);

            IRasterLayer simulationImageLayer = new RasterLayerClass();
            simulationImageLayer.CreateFromFilePath(VariableMaintainer.DefaultOutputFolder + @"\" + rasterName);
            ArcGISOperator.WriteRaster(simulationImageLayer, suitabilityData2);
            IRasterLayer l = ArcGISOperator.GetRasterLayerByName(rasterName);
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
            return suitabilityData2;
        }

        private bool SplitExpression(string expression, out string[] splitExpression)
        {
            try
            {
                splitExpression = expression.Split(new char[] { '[', ']' });
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                splitExpression = null;
                return false;
            }
        }
        #endregion

        #endregion
        //--------------------------------------------------------------------------------------------------------------
    }
}
