using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeoSOS.CommonLibrary.Struct;
using ESRI.ArcGIS.DataSourcesRaster;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geodatabase;
using System.Windows.Forms;

namespace GeoSOS.CommonLibrary
{
    /// <summary>
    /// 进行逻辑回归中的抽样操作。
    /// </summary>
    public class RasterSampling
    {

        /// <summary>
        /// 生成随机数。
        /// </summary>
        private Random random;

        /// <summary>
        /// 抽样数据的个数。
        /// </summary>
        private int samplingCellsCount;

        /// <summary>
        /// 抽样数据的个数。
        /// </summary>
        public int SamplingCellsCount
        {
            get
            {
                return samplingCellsCount;
            }
        }

        public RasterSampling()
        {
            random = new Random();
        }

        /// <summary>
        /// 根据定义的抽取真值的比例来抽取随机样本。
        /// </summary>
        /// <param name="independentVariables">进行抽样的自变量的数值集合。</param>
        /// <param name="dependentVariable">因变量的值。</param>
        /// <param name="samplePrecent">抽样比例。乘以总样本数为抽样样本总数。</param>
        /// <param name="trueDataPercent">变化为真(值为1，未变化值为0)的值占整个抽样样本数的比例。</param>
        /// /// <param name="structRasterMetaData">记录当前影像数据元数据(包括非空值)的结构体。</param>
        /// <returns></returns>
        public float[,] SamplingData(List<float[,]> independentVariables, float[,] dependentVariable, double samplePrecent,
            float trueDataPercent, StructRasterMetaData structRasterMetaData, List<int> notNullRows, List<int> notNullColumns)
        {
            samplingCellsCount = Convert.ToInt32(notNullRows.Count * samplePrecent / 100);    //要抽取的样本总数。
            int trueDataCount = Convert.ToInt32(samplingCellsCount * trueDataPercent / 100);      //要抽取的真值的样本总数。
            int falseDataCount = samplingCellsCount - trueDataCount;      //要抽取的非真值的样本总数。

            float[,] result = new float[samplingCellsCount, independentVariables.Count + 1];
            int sum = 0;            //当前抽取的样本总数。
            int trueSum = 0;        //当前抽取的真值的样本总数。
            int falseSum = 0;       //当前抽取的非真值的样本总数。
            int randomIndex = 0;    //随机抽取的行列号的索引。
            int randomRow = 0;      //由随机抽取的行列号的索引得到的非空数据的行号。
            int randomColumn = 0;   //由随机抽取的行列号的索引得到的非空数据的列号。
            bool isExistNullValue = false;  //记录当前选择的行列号在空间因子图层中是否存在非空值。

            try
            {
                while (sum < samplingCellsCount)
                {
                    randomIndex = random.Next(0, notNullRows.Count);
                    randomRow = notNullRows[randomIndex];
                    randomColumn = notNullColumns[randomIndex];
                    if ((randomRow >= structRasterMetaData.RowCount) || (randomColumn >= structRasterMetaData.ColumnCount))
                        continue;
                    isExistNullValue = false;

                    //要判断变量如果某点该变量的值为空值，则不抽样该点。
                    //(该原因的造成是数据问题，可能开始年份的数据非空，但其他数据该点为空值。)
                    for (int j = 0; j < independentVariables.Count; j++)
                    {
                        if (independentVariables[j][randomRow, randomColumn] == structRasterMetaData.NoDataValue)
                        {
                            isExistNullValue = true;
                            break;
                        }
                    }
                    if (isExistNullValue)
                        continue;

                    //如果其因变量为真值
                    if (dependentVariable[randomRow, randomColumn] == 1f)
                    {
                        //如果真值数据还未抽样完毕，添加一条抽样结果。
                        if (trueSum < trueDataCount)
                        {
                            for (int j = 0; j < independentVariables.Count; j++)
                            {
                                result[sum, j] = independentVariables[j][randomRow, randomColumn];
                            }
                            result[sum, independentVariables.Count] = dependentVariable[randomRow, randomColumn];
                            trueSum++;
                            sum++;
                        }
                    }
                    //如果其因变量为非真值
                    else if (dependentVariable[randomRow, randomColumn] == 0f)
                    {
                        //如果非真值数据还未抽样完毕，添加一条抽样结果。
                        if (falseSum < falseDataCount)
                        {
                            for (int j = 0; j < independentVariables.Count; j++)
                            {
                                result[sum, j] = independentVariables[j][randomRow, randomColumn];
                            }
                            result[sum, independentVariables.Count] = dependentVariable[randomRow, randomColumn];
                            falseSum++;
                            sum++;
                        }
                    }
                    else
                        //如果因变量为数据空值，则跳过该点。(处理数据有问题时的情况，应该不会出现。)
                        continue;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return result;
        }

        /// <summary>
        /// 进行基于ANN的数据抽样。
        /// </summary>
        /// <param name="listSpatialVariables"></param>
        /// <param name="trainningStartImage"></param>
        /// <param name="trainningEndImage"></param>
        /// <param name="landuseClassificationInfo"></param>
        /// <param name="structRasterMetaData"></param>
        /// <param name="neiWindowSize"></param>
        /// <param name="listNotNullRows"></param>
        /// <param name="listNotNullColumns"></param>
        /// <param name="inputs"></param>
        /// <param name="outputs"></param>
        /// <param name="samplingCellsCount"></param>
        /// <param name="inputNeuronsCount"></param>
        /// <param name="outputNeuronsCount"></param>
        public void ANNSamplingData(List<float[,]> listSpatialVariables, float[,] trainningStartImage, float[,] trainningEndImage,
            LandUseClassificationInfo landuseClassificationInfo, StructRasterMetaData structRasterMetaData, int neiWindowSize,
            List<int> listNotNullRows, List<int> listNotNullColumns, ref float[][] inputs, ref float[][] outputs,
            int samplingCellsCount, int inputNeuronsCount, int outputNeuronsCount)
        {
            float[] tempInputsArray = null;
            float[] tempOutputsArray = null;
            List<int> listChoosen = new List<int>();
            for (int z = 0; z < listNotNullRows.Count; z++)
                listChoosen.Add(0);
            int index, rowIndex, columnIndex;
            //在抽样时，目前是随机采样。可以考虑分层抽样，即发生变化的元胞取一半，未变化的取一半。
            try
            {
                for (int s = 0; s < samplingCellsCount; s++)
                {
                    index = random.Next(0, listNotNullRows.Count);
                    if (listChoosen[index] == 1)
                    {
                        s--;
                        continue;
                    }
                    rowIndex = listNotNullRows[index];
                    columnIndex = listNotNullColumns[index];
                    if ((rowIndex >= structRasterMetaData.RowCount) || (columnIndex >= structRasterMetaData.ColumnCount))
                    {
                        s--;
                        continue;
                    }
                    if ((trainningStartImage[rowIndex, columnIndex] == -9999f) || (trainningEndImage[rowIndex, columnIndex] == -9999f))
                    {
                        s--;
                        continue;
                    }
                    tempInputsArray = new float[inputNeuronsCount];
                    tempOutputsArray = new float[outputNeuronsCount];
                    //1.输入的列
                    //1.1先去各个空间变量获取
                    bool isNull=false;
                    for (int i = 0; i < listSpatialVariables.Count; i++)
                    {
                        tempInputsArray[i] = listSpatialVariables[i][rowIndex, columnIndex];
                        if (tempInputsArray[i] == -9999f)
                        {
                            isNull = true;
                            break;
                        }
                    }
                    if (isNull)
                    {
                        s--;
                        continue;
                    }
                    //1.2再去获取邻域信息
                    NeighbourOperator neighbourOperator = new NeighbourOperator();
                    List<float> listLanduseValues = new List<float>();
                    for (int j = 0; j < landuseClassificationInfo.AllTypesCount; j++)
                        listLanduseValues.Add(landuseClassificationInfo.AllTypes[j].LanduseTypeValue);
                    float[] counts = neighbourOperator.GetNeighbourCount(trainningStartImage, rowIndex, columnIndex, neiWindowSize,
                       structRasterMetaData.RowCount, structRasterMetaData.ColumnCount, landuseClassificationInfo.AllTypesCount, listLanduseValues);
                    for (int z = 0; z < counts.Length; z++)
                        tempInputsArray[listSpatialVariables.Count + z] = counts[z] / (neiWindowSize * neiWindowSize-1);
                    //1.3最后获取土地利用类型
                    tempInputsArray[inputNeuronsCount - 1] = trainningStartImage[rowIndex, columnIndex];
                    inputs[s] = tempInputsArray;
                    //2.输出的列。是根据当前元胞在终止土地利用数据中是哪种土地利用类型值，从而形成0,1序列。0代表这种土地利用类型转换的概率为0。
                    for (int k = 0; k < landuseClassificationInfo.AllTypesCount; k++)
                    {
                        if (trainningEndImage[rowIndex, columnIndex] == landuseClassificationInfo.AllTypes[k].LanduseTypeValue)
                        {
                            tempOutputsArray[k] = 1.0f;
                            break;
                        }
                    }
                    outputs[s] = tempOutputsArray;

                    listChoosen[index] = 1;
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("ANNSamplingData: " + ex.Message);
            }
            ////输出抽样结果数据
            //GeneralOpertor.WriteDataFloat(@"C:\DanLi\Download\annSamplingData.txt", inputs, outputs, samplingCellsCount,
            //    inputNeuronsCount, outputNeuronsCount);
        }
    }

}
