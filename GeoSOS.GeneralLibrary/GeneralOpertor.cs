using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Data;
using GeoSOS.CommonLibrary.Struct;

namespace GeoSOS.CommonLibrary
{
    public class GeneralOpertor
    {
        /// <summary>
        /// 根据输入的宽度、高度及颜色得到一个Bitmap。
        /// </summary>
        public static Bitmap GetBitmap(int width, int height, Color color)
        {
            Bitmap bitmap = new Bitmap(width, height);
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                    bitmap.SetPixel(i, j, color);
            }
            return bitmap;
        }

        /// <summary>
        /// 根据两幅图像得到相应的变化二值图像。
        /// </summary>
        /// <param name="startImage"></param>
        /// <param name="endImage"></param>
        /// <param name="rowCount"></param>
        /// <param name="columnCount"></param>
        /// <param name="nullValue"></param>
        /// <returns></returns>
        public static float[,] GetBinaryImageByTwoImages(float[,] startImage, float[,] endImage, int rowCount, int columnCount, float nullValue)
        {
            float[,] result = new float[rowCount, columnCount];
            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < columnCount; j++)
                {
                    if (startImage[i, j] == nullValue || endImage[i, j] == nullValue)
                        result[i, j] = nullValue;
                    else
                    {
                        if (startImage[i, j] == endImage[i, j])
                            result[i, j] = 0;
                        else
                            result[i, j] = 1;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 将数组的数据写入到文本文件中。
        /// </summary>
        /// <param name="path"></param>
        /// <param name="data"></param>
        /// <param name="rowCount"></param>
        /// <param name="columnCount"></param>
        public static void WriteDataFloat(string path, float[,] data, int rowCount, int columnCount)
        {
            if (System.IO.File.Exists(path))
                System.IO.File.Delete(path);
            System.IO.TextWriter textWrite = new System.IO.StreamWriter(path);
            string rowString = "";

            for (int i = 0; i < rowCount; i++)
            {
                rowString = "";
                for (int j = 0; j < columnCount - 1; j++)
                {
                    rowString += data[i, j].ToString() + " ";
                }
                rowString += data[i, columnCount - 1].ToString();
                textWrite.WriteLine(rowString);
            }
            textWrite.Close();
        }

        public static void WriteDataFloat(string path, float[][] inputs, float[][] outputs, int rowCount, int inputsLength, int outputsLength)
        {
            if (System.IO.File.Exists(path))
                System.IO.File.Delete(path);
            System.IO.TextWriter textWrite = new System.IO.StreamWriter(path);
            string rowString = "";
            float[] tempInputArray = null;
            float[] tempOutputArray = null;

            try
            {
                for (int i = 0; i < rowCount; i++)
                {
                    rowString = "";
                    tempInputArray = inputs[i];
                    tempOutputArray = outputs[i];
                    if (tempInputArray == null)
                    {
                        System.Diagnostics.Debug.WriteLine("input - " + i);
                        continue;
                    }
                    if (tempOutputArray == null)
                    {
                        System.Diagnostics.Debug.WriteLine("output - " + i);
                        continue;
                    }
                    for (int j = 0; j < tempInputArray.Length; j++)
                    {
                        rowString += tempInputArray[j].ToString() + " ";
                    }
                    for (int k = 0; k < tempOutputArray.Length - 1; k++)
                    {
                        rowString += tempOutputArray[k].ToString() + " ";
                    }
                    rowString += tempOutputArray[tempOutputArray.Length - 1].ToString();
                    textWrite.WriteLine(rowString);
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
            textWrite.Close();
        }

        /// <summary>
        /// 根据输入的字符格式修改double数据的小数位。
        /// </summary>
        public static double FormatDouble(double number, string formatString)
        {
            if (number.ToString() == "")
                return 0;
            else
            {
                string numberString = number.ToString(formatString);
                if (numberString == "")
                    return 0;
                else
                    return Convert.ToDouble(numberString);
            }
        }

        /// <summary>
        /// 改变相应土地利用类型的单元总数。
        /// </summary>
        /// <param name="newValue"></param>
        /// <param name="oldValue"></param>
        /// <param name="listLanduseInfoAndCount"></param>
        public static void ChangeLandUseCount(float newValue, float oldValue, List<StructLanduseInfoAndCount> listLanduseInfoAndCount)
        {
            StructLanduseInfoAndCount s;
            for (int i = 0; i < listLanduseInfoAndCount.Count; i++)
            {
                s = listLanduseInfoAndCount[i];
                if (s.structLanduseInfo.LanduseTypeValue == newValue)
                {
                    s.LanduseTypeCount++;
                    listLanduseInfoAndCount[i] = s;
                }
                else if (s.structLanduseInfo.LanduseTypeValue == oldValue)
                {
                    s.LanduseTypeCount--;
                    listLanduseInfoAndCount[i] = s;
                }
            }
        }

        /// <summary>
        /// 得到元胞所在位置的所有摩尔领域单元。
        /// </summary>
        public static List<float> GetMooreNeighbors(int x, int y, float[,] cells, int rowCount, int columnCount)
        {
            List<float> neighbors = new List<float>();
            {
                if ((x - 1) > 0 && (y - 1) > 0)
                    neighbors.Add(cells[x - 1, y - 1]);
                if ((y - 1) > 0)
                    neighbors.Add(cells[x, y - 1]);
                if ((x + 1) < rowCount && (y - 1) > 0)
                    neighbors.Add(cells[x + 1, y - 1]);
                if ((x - 1) > 0)
                    neighbors.Add(cells[x - 1, y]);
                if ((x + 1) < rowCount)
                    neighbors.Add(cells[x + 1, y]);
                if ((x - 1) > 0 && (y + 1) < columnCount)
                    neighbors.Add(cells[x - 1, y + 1]);
                if ((y + 1) < columnCount)
                    neighbors.Add(cells[x, y + 1]);
                if ((x + 1) < rowCount && (y + 1) < columnCount)
                    neighbors.Add(cells[x + 1, y + 1]);
            }
            return neighbors;
        }

        /// <summary>
        /// 根据邻域大小获取窗口内的所有数值。
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="cells"></param>
        /// <param name="rowCount"></param>
        /// <param name="columnCount"></param>
        /// <returns></returns>
        public static int GetNeighbors(int rowIndex, int columnIndex, float[,] cells, int rowCount, int columnCount, int neiWindowSize, List<float> urbanTypeValue)
        {
            int neighbourLength = (neiWindowSize - 1) / 2;
            float[,] tempValue = new float[neiWindowSize, neiWindowSize];
            int urbanCount = 0;
            for (int i = rowIndex - neighbourLength; i <= rowIndex + neighbourLength; i++)
            {
                for (int j = columnIndex - neighbourLength; j <= columnIndex + neighbourLength; j++)
                {
                    if (i < 0 || j < 0 || i >= rowCount || j >= columnCount)
                    {
                        tempValue[i - rowIndex + neighbourLength, j - columnIndex + neighbourLength] = -1f;
                        continue;
                    }
                    tempValue[i - rowIndex + neighbourLength, j - columnIndex + neighbourLength] = cells[i, j];
                    //System.Diagnostics.Debug.WriteLine(cells[i, j]);
                    int index = urbanTypeValue.IndexOf(tempValue[i - rowIndex + neighbourLength, j - columnIndex + neighbourLength]);
                    if (index != -1)
                        urbanCount++;
                }
            }
            return urbanCount;
        }

        /// <summary>
        /// 得到所给邻域内城市单元的数量。
        /// </summary>
        public static int GetUrbanCount(List<float> neighbors, float urbanValue)
        {
            int count = 0;
            for (int i = 0; i < neighbors.Count; i++)
            {
                if (neighbors[i] == urbanValue)
                    count++;
            }
            return count;
        }

        /// <summary>
        /// 返回当前时间的自定义字符串。
        /// </summary>
        /// <returns></returns>
        public static string GetNowString()
        {
            return DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() +
                DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
        }

        /// <summary>
        /// 返回计时器的时间字符串。
        /// </summary>
        /// <param name="timeSpan"></param>
        /// <returns></returns>
        public static string GetElapsedTimeString(TimeSpan timeSpan)
        {
            System.Resources.ResourceManager resourceManager = null;
            if (System.Globalization.CultureInfo.CurrentCulture.Parent.Name == "zh-CHS")
                resourceManager = new System.Resources.ResourceManager("GeoSOS.CommonLibrary.Properties.Resource_zh_CHS", System.Reflection.Assembly.GetExecutingAssembly());
            else if (System.Globalization.CultureInfo.CurrentCulture.Parent.Name == "zh-CHT")
                resourceManager = new System.Resources.ResourceManager("GeoSOS.CommonLibrary.Properties.Resource_zh_TW", System.Reflection.Assembly.GetExecutingAssembly());
            else if (System.Globalization.CultureInfo.CurrentCulture.Parent.Name == "en")
                resourceManager = new System.Resources.ResourceManager("GeoSOS.CommonLibrary.Properties.Resource_en", System.Reflection.Assembly.GetExecutingAssembly());

            StringBuilder sb = new StringBuilder();
            if (timeSpan.Days > 0)
                sb.Append(timeSpan.Days.ToString() + resourceManager.GetString("String11"));
            else
            {
                if (timeSpan.Hours > 0)
                    sb.Append(timeSpan.Hours.ToString() + resourceManager.GetString("String12"));
                else
                {
                    sb.Append(timeSpan.Minutes.ToString() + resourceManager.GetString("String13"));
                    sb.Append(timeSpan.Seconds.ToString() + resourceManager.GetString("String14"));
                    sb.Append(timeSpan.Milliseconds.ToString() + resourceManager.GetString("String15"));
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 根据模拟的结果和要进行对比的影像得到相关混淆矩阵信息。
        /// </summary>
        /// <param name="simulatedData"></param>
        /// <param name="observedData"></param>
        /// <param name="rowCount"></param>
        /// <param name="columnCount"></param>
        /// <returns></returns>
        public static StructBinaryConfusionMatrix GetBinaryAccuracy(float[,] simulatedData, float[,] observedData, int rowCount, int columnCount, LandUseClassificationInfo landUseClassificationInfo)
        {
            StructBinaryConfusionMatrix structConfusionMatrix = new StructBinaryConfusionMatrix();

            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < columnCount; j++)
                {
                    //如果真实数据为城市用地。
                    if (landUseClassificationInfo.IsExistInUrbanValue(observedData[i, j]))
                    {
                        //如果模拟为城市用地
                        if (landUseClassificationInfo.IsExistInUrbanValue(simulatedData[i, j]))
                            structConfusionMatrix.Urban2UrbanCount += 1;
                        //如果模拟为非城市用地
                        else if (landUseClassificationInfo.NullValue.LanduseTypeValue != simulatedData[i, j])
                            structConfusionMatrix.Urban2NotUrbanCount += 1;
                    }
                    //如果真实数据为非城市用地。空值不进行统计。
                    else if (landUseClassificationInfo.NullValue.LanduseTypeValue != observedData[i, j])
                    {
                        //如果模拟为城市用地
                        if (landUseClassificationInfo.IsExistInUrbanValue(simulatedData[i, j]))
                            structConfusionMatrix.NotUrban2UrbanCount += 1;
                        //如果模拟为非城市用地
                        else if (landUseClassificationInfo.NullValue.LanduseTypeValue != simulatedData[i, j])
                            structConfusionMatrix.NotUrban2NotUrbanCount += 1;
                    }
                }
            }

            structConfusionMatrix.OverallObseredDataNotUrbanCount = structConfusionMatrix.NotUrban2NotUrbanCount + structConfusionMatrix.NotUrban2UrbanCount;
            structConfusionMatrix.OverallObseredDataUrbanCount = structConfusionMatrix.Urban2NotUrbanCount + structConfusionMatrix.Urban2UrbanCount;
            structConfusionMatrix.OverallSimulatedDataNotUrbanCount = structConfusionMatrix.Urban2NotUrbanCount + structConfusionMatrix.NotUrban2NotUrbanCount;
            structConfusionMatrix.OverallSimulatedDataUrbanCount = structConfusionMatrix.Urban2UrbanCount + structConfusionMatrix.NotUrban2UrbanCount;

            structConfusionMatrix.NotUrbanCorrectPrecent = Convert.ToDouble(structConfusionMatrix.NotUrban2NotUrbanCount) /
                (structConfusionMatrix.NotUrban2NotUrbanCount + structConfusionMatrix.NotUrban2UrbanCount);
            structConfusionMatrix.UrabanCorrectPrecent = Convert.ToDouble(structConfusionMatrix.Urban2UrbanCount) /
                (structConfusionMatrix.Urban2UrbanCount + structConfusionMatrix.Urban2NotUrbanCount);
            structConfusionMatrix.OverallCorrectPrecent = Convert.ToDouble((structConfusionMatrix.Urban2UrbanCount +
                structConfusionMatrix.NotUrban2NotUrbanCount)) / (structConfusionMatrix.NotUrban2NotUrbanCount
                + structConfusionMatrix.NotUrban2UrbanCount + structConfusionMatrix.Urban2UrbanCount
                + structConfusionMatrix.Urban2NotUrbanCount);

            return structConfusionMatrix;
        }

        /// <summary>
        /// 根据输入的精度计算结果得到精度报告字符串。
        /// </summary>
        /// <param name="structBinaryConfusionMatrix"></param>
        /// <param name="convertedCellCount">已转换的元胞数量。</param>
        /// <returns></returns>
        public static string GetBinaryAccuracyReportString(StructBinaryConfusionMatrix structBinaryConfusionMatrix, int convertedCellCount)
        {
            System.Resources.ResourceManager resourceManager = null;
            if (System.Globalization.CultureInfo.CurrentCulture.Parent.Name == "zh-CHS")
                resourceManager = new System.Resources.ResourceManager("GeoSOS.CommonLibrary.Properties.Resource_zh_CHS", System.Reflection.Assembly.GetExecutingAssembly());
            else if (System.Globalization.CultureInfo.CurrentCulture.Parent.Name == "zh-CHT")
                resourceManager = new System.Resources.ResourceManager("GeoSOS.CommonLibrary.Properties.Resource_zh_TW", System.Reflection.Assembly.GetExecutingAssembly());
            else if (System.Globalization.CultureInfo.CurrentCulture.Parent.Name == "en")
                resourceManager = new System.Resources.ResourceManager("GeoSOS.CommonLibrary.Properties.Resource_en", System.Reflection.Assembly.GetExecutingAssembly());

            StringBuilder sb = new StringBuilder();
            sb.AppendLine(resourceManager.GetString("String1"));
            sb.AppendLine(resourceManager.GetString("String2") + convertedCellCount);
            sb.AppendLine("\n");
            sb.AppendLine(resourceManager.GetString("String3") + structBinaryConfusionMatrix.NotUrban2NotUrbanCount);
            sb.AppendLine(resourceManager.GetString("String4") + structBinaryConfusionMatrix.NotUrban2UrbanCount);
            sb.AppendLine(resourceManager.GetString("String5") + FormatDouble(structBinaryConfusionMatrix.NotUrbanCorrectPrecent, "#.###") * 100 + "%");
            sb.AppendLine("\n");
            sb.AppendLine(resourceManager.GetString("String6") + structBinaryConfusionMatrix.Urban2NotUrbanCount);
            sb.AppendLine(resourceManager.GetString("String7") + structBinaryConfusionMatrix.Urban2UrbanCount);
            sb.AppendLine(resourceManager.GetString("String8") + FormatDouble(structBinaryConfusionMatrix.UrabanCorrectPrecent, "#.###") * 100 + "%");
            sb.AppendLine("\n");
            //sb.AppendLine(resourceManager.GetString("String9") + FormatDouble(structBinaryConfusionMatrix.OverallCorrectPrecent, "#.###") * 100 + "%");
            return sb.ToString();
        }

        public static float[,] GetMooreNeighborInfo(float[,] landuseImage, int rowCount, int columnCount, int windowSize,
            LandUseClassificationInfo landUseClassificationInfo)
        {
            float[,] neighbor = new float[rowCount, columnCount];
            for (int x = 0; x < rowCount; x++)
            {
                for (int y = 0; y < columnCount; y++)
                {
                    int count = 0;
                    float landuseValue = -1;
                    {
                        if ((x - windowSize) > 0 && (y - windowSize) > 0)
                        {
                            landuseValue = landuseImage[x - windowSize, y - windowSize];
                            for (int i = 0; i < landUseClassificationInfo.UrbanValues.Count; i++)
                            {
                                if (landUseClassificationInfo.UrbanValues[i].LanduseTypeValue == landuseValue)
                                    count++;
                            }
                        }
                        if ((y - windowSize) > 0)
                        {
                            landuseValue = landuseImage[x, y - windowSize];
                            for (int i = 0; i < landUseClassificationInfo.UrbanValues.Count; i++)
                            {
                                if (landUseClassificationInfo.UrbanValues[i].LanduseTypeValue == landuseValue)
                                    count++;
                            }
                        }
                        if ((x + windowSize) < rowCount && (y - windowSize) > 0)
                        {
                            landuseValue = landuseImage[x + windowSize, y - windowSize];
                            for (int i = 0; i < landUseClassificationInfo.UrbanValues.Count; i++)
                            {
                                if (landUseClassificationInfo.UrbanValues[i].LanduseTypeValue == landuseValue)
                                    count++;
                            }
                        }
                        if ((x - windowSize) > 0)
                        {
                            landuseValue = landuseImage[x - windowSize, y];
                            for (int i = 0; i < landUseClassificationInfo.UrbanValues.Count; i++)
                            {
                                if (landUseClassificationInfo.UrbanValues[i].LanduseTypeValue == landuseValue)
                                    count++;
                            }
                        }
                        if ((x + windowSize) < rowCount)
                        {
                            landuseValue = landuseImage[x + windowSize, y];
                            for (int i = 0; i < landUseClassificationInfo.UrbanValues.Count; i++)
                            {
                                if (landUseClassificationInfo.UrbanValues[i].LanduseTypeValue == landuseValue)
                                    count++;
                            }
                        }
                        if ((x - windowSize) > 0 && (y + windowSize) < columnCount)
                        {
                            landuseValue = landuseImage[x - windowSize, y + windowSize];
                            for (int i = 0; i < landUseClassificationInfo.UrbanValues.Count; i++)
                            {
                                if (landUseClassificationInfo.UrbanValues[i].LanduseTypeValue == landuseValue)
                                    count++;
                            }
                        }
                        if ((y + windowSize) < columnCount)
                        {
                            landuseValue = landuseImage[x, y + windowSize];
                            for (int i = 0; i < landUseClassificationInfo.UrbanValues.Count; i++)
                            {
                                if (landUseClassificationInfo.UrbanValues[i].LanduseTypeValue == landuseValue)
                                    count++;
                            }
                        }
                        if ((x + windowSize) < rowCount && (y + windowSize) < columnCount)
                        {
                            landuseValue = landuseImage[x + windowSize, y + windowSize];
                            for (int i = 0; i < landUseClassificationInfo.UrbanValues.Count; i++)
                            {
                                if (landUseClassificationInfo.UrbanValues[i].LanduseTypeValue == landuseValue)
                                    count++;
                            }
                        }
                    }
                    neighbor[x, y] = count;
                }
            }
            return neighbor;
        }

        /// <summary>
        /// 对多类土地利用模拟结果进行混淆矩阵计算。
        /// </summary>
        /// <param name="simulatedData"></param>
        /// <param name="observedData"></param>
        /// <param name="rowCount"></param>
        /// <param name="columnCount"></param>
        /// <param name="landUseClassificationInfo"></param>
        /// <returns></returns>
        public static DataTable GetMultiTypesMatrix(float[,] simulatedData, float[,] observedData, int rowCount, int columnCount, LandUseClassificationInfo landUseClassificationInfo)
        {
            int allTypesCount = landUseClassificationInfo.AllTypesCount;
            //建立记录混淆矩阵的DataTable
            DataTable dtMatrix = new DataTable();
            for (int i = 0; i < allTypesCount; i++)
            {
                if (System.Threading.Thread.CurrentThread.CurrentCulture.Parent.Name == "zh-CHS")
                    dtMatrix.Columns.Add(landUseClassificationInfo.AllTypes[i].LanduseTypeChsName);
                else if (System.Threading.Thread.CurrentThread.CurrentCulture.Parent.Name == "zh-CHT")
                    dtMatrix.Columns.Add(landUseClassificationInfo.AllTypes[i].LanduseTypeChtName);
                else if (System.Threading.Thread.CurrentThread.CurrentCulture.Parent.Name == "en")
                    dtMatrix.Columns.Add(landUseClassificationInfo.AllTypes[i].LanduseTypeEnName);
            }
            System.Resources.ResourceManager resourceManager = null;
            if (System.Globalization.CultureInfo.CurrentCulture.Parent.Name == "zh-CHS")
                resourceManager = new System.Resources.ResourceManager("GeoSOS.CommonLibrary.Properties.Resource_zh_CHS", System.Reflection.Assembly.GetExecutingAssembly());
            else if (System.Globalization.CultureInfo.CurrentCulture.Parent.Name == "zh-CHT")
                resourceManager = new System.Resources.ResourceManager("GeoSOS.CommonLibrary.Properties.Resource_zh_TW", System.Reflection.Assembly.GetExecutingAssembly());
            else if (System.Globalization.CultureInfo.CurrentCulture.Parent.Name == "en")
                resourceManager = new System.Resources.ResourceManager("GeoSOS.CommonLibrary.Properties.Resource_en", System.Reflection.Assembly.GetExecutingAssembly());
            dtMatrix.Columns.Add(resourceManager.GetString("String16"));
            for (int i = 0; i < allTypesCount + 1; i++)
            {
                DataRow row = dtMatrix.NewRow();
                for (int j = 0; j < allTypesCount + 1; j++)
                    row[j] = 0;
                dtMatrix.Rows.Add(row);
            }
            //对真实数据的每个栅格统计其每个值，在模拟结果数据中是什么类型
            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < columnCount; j++)
                {
                    int simulatedIndex = GetLandUseTypeIndex(simulatedData[i, j], landUseClassificationInfo);
                    int observedIndex = GetLandUseTypeIndex(observedData[i, j], landUseClassificationInfo);
                    if ((simulatedIndex == -1) || (observedIndex == -1))
                        continue;
                    int oldValue = Convert.ToInt32(dtMatrix.Rows[observedIndex][simulatedIndex]);
                    dtMatrix.Rows[observedIndex][simulatedIndex] = oldValue + 1;
                }
            }
            //对每一行和列进行总值统计
            int sumRow = 0;
            int sumColumn = 0;
            for (int i = 0; i < landUseClassificationInfo.AllTypesCount; i++)
            {
                sumRow = 0;
                for (int j = 0; j < landUseClassificationInfo.AllTypesCount; j++)
                {
                    sumRow += Convert.ToInt32(dtMatrix.Rows[i][j]);
                }
                dtMatrix.Rows[i][allTypesCount] = sumRow;
            }
            for (int i = 0; i < landUseClassificationInfo.AllTypesCount; i++)
            {
                sumColumn = 0;
                for (int j = 0; j < landUseClassificationInfo.AllTypesCount; j++)
                {
                    sumColumn += Convert.ToInt32(dtMatrix.Rows[j][i]);
                }
                dtMatrix.Rows[allTypesCount][i] = sumColumn;
            }
            //对最后一行和列的值进行总计
            int sum = 0;
            for (int i = 0; i < dtMatrix.Columns.Count; i++)
                sum += Convert.ToInt32(dtMatrix.Rows[dtMatrix.Rows.Count - 1][i]);
            dtMatrix.Rows[dtMatrix.Rows.Count - 1][dtMatrix.Columns.Count - 1] = sum;
            return dtMatrix;
        }

        /// <summary>
        /// 获得多类模拟结果的精度和Kappa系数。
        /// </summary>
        /// <param name="dtMatrix"></param>
        /// <param name="overallAccuracy"></param>
        /// <param name="kappa"></param>
        /// <param name="landUseClassificationInfo"></param>
        public static void GetMultiTypesAccuracy(DataTable dtMatrix, ref double overallAccuracy, ref double kappa, LandUseClassificationInfo landUseClassificationInfo)
        {
            int allTypesCount = landUseClassificationInfo.AllTypesCount;
            double sumNiiN = 0;
            double sumNii = 0;
            double n = 0;
            for (int i = 0; i < allTypesCount; i++)
                sumNiiN += Convert.ToDouble(dtMatrix.Rows[i][allTypesCount]) * Convert.ToDouble(dtMatrix.Rows[allTypesCount][i]);
            for (int i = 0; i < allTypesCount; i++)
                sumNii += Convert.ToDouble(dtMatrix.Rows[i][i]);
            for (int i = 0; i < allTypesCount; i++)
                n += Convert.ToDouble(dtMatrix.Rows[i][allTypesCount]);
            overallAccuracy = sumNii / n;
            kappa = (n * sumNii - sumNiiN) / (n * n - sumNiiN);
        }

        /// <summary>
        /// 根据土地利用类型的值获取其所在列表中的索引。
        /// </summary>
        /// <param name="landuseTypeValue"></param>
        /// <param name="landUseClassificationInfo"></param>
        /// <returns></returns>
        public static int GetLandUseTypeIndex(float landuseTypeValue, LandUseClassificationInfo landUseClassificationInfo)
        {
            int index = -1;
            for (int i = 0; i < landUseClassificationInfo.AllTypesCount; i++)
            {
                if (landuseTypeValue == landUseClassificationInfo.AllTypes[i].LanduseTypeValue)
                {
                    index = i;
                    break;
                }
            }
            return index;
        }

        /// <summary>
        /// 将一个表中数值复制到另一个表中。
        /// </summary>
        /// <param name="originDataTable"></param>
        /// <param name="destinationDataTable"></param>
        public static void CopyDataTableValues(DataTable originDataTable, DataTable destinationDataTable)
        {
            for (int i = 0; i < originDataTable.Rows.Count; i++)
            {
                DataRow row = destinationDataTable.NewRow();
                destinationDataTable.Rows.Add(row);
            }
            for (int i = 0; i < originDataTable.Rows.Count; i++)
                for (int j = 0; j < originDataTable.Columns.Count; j++)
                    destinationDataTable.Rows[i][j] = originDataTable.Rows[i][j];
        }

        /// <summary>
        /// 根据原始数据获取部分数据。
        /// </summary>
        /// <param name="orgArray"></param>
        /// <param name="percent"></param>
        /// <param name="isBegin"></param>
        /// <param name="inputsCount"></param>
        /// <returns></returns>
        public static double[][] GetArray(float[][] orgArray, double percent, bool isBegin, int inputsCount)
        {
            int length = Convert.ToInt32(orgArray.Length * percent);
            double[][] array = null;
            double[] tempArray = null;
            if (isBegin)
                array = new double[length][];
            else
                array = new double[orgArray.Length - length][];
            if (isBegin)
            {
                for (int i = 0; i < length; i++)
                {
                    tempArray = new double[inputsCount];
                    for (int j = 0; j < inputsCount; j++)
                        tempArray[j] = Convert.ToDouble(orgArray[i][j]);
                    array[i] = tempArray;
                }
            }
            else
            {
                for (int i = length; i < orgArray.Length; i++)
                {
                    tempArray = new double[inputsCount];
                    for (int j = 0; j < inputsCount; j++)
                        tempArray[j] = Convert.ToDouble(orgArray[i - length][j]);
                    array[i - length] = tempArray;
                }
            }
            return array;
        }

        /// <summary>
        /// 根据原始数据获取部分数据。
        /// </summary>
        /// <param name="orgArray"></param>
        /// <param name="percent"></param>
        /// <param name="isBegin"></param>
        /// <returns></returns>
        public static int[] GetArray(int[] orgArray, double percent, bool isBegin)
        {
            int length = Convert.ToInt32(orgArray.Length * percent);
            int[] array = null;
            if (isBegin)
                array = new int[length];
            else
                array = new int[orgArray.Length - length];
            if (isBegin)
            {
                for (int i = 0; i < length; i++)
                    array[i] = orgArray[i];
            }
            else
            {
                for (int i = length; i < orgArray.Length; i++)
                    array[i - length] = orgArray[i - length];
            }
            return array;
        }

        /// <summary>
        /// 为多类土地利用变化计算FoM精度。
        /// </summary>
        /// <param name="startImage"></param>
        /// <param name="endImage"></param>
        /// <param name="simulatedImage"></param>
        /// <param name="rowCount"></param>
        /// <param name="columnCount"></param>
        /// <returns></returns>
        public static float[] GetMultiFoMAccuracy(float[,] startImage, float[,] endImage, float[,] simulatedImage, int rowCount, int columnCount)
        {
            float[] values = new float[7];
            for (int i = 0; i < rowCount; i++)
                for (int j = 0; j < columnCount; j++)
                {
                    if (startImage[i, j] == -9999f)
                        continue;
                    //如果是应该发生变化的情况
                    if (startImage[i, j] != endImage[i, j])
                    {
                        //如果该发生变化而未发生变化，则A加1
                        if (startImage[i, j] == simulatedImage[i, j])
                            values[0] += 1;
                        //如果发生了变化，则需要判断
                        else
                        {
                            //如果完全一样，则B加1
                            if (simulatedImage[i, j] == endImage[i, j])
                                values[1] += 1;
                            //如果变化不一样，则C加1
                            else
                                values[2] += 1;
                        }
                    }
                    //如果是不应该发生变化的情况
                    else
                    {
                        //如果发生的变化，则D加1
                        if (startImage[i, j] != simulatedImage[i, j])
                            values[3] += 1;
                    }
                }
            //Figure of merit = B/(A + B + C + D)
            values[4] = values[1] / (values[0] + values[1] + values[2] + values[3]);
            //Producer's Accuracy = B/(A + B + C)
            values[5] = values[1] / (values[0] + values[1] + values[2]);
            //User's Accuracy = B/(B + C + D)
            values[6] = values[1] / (values[1] + values[2] + values[3]);

            return values;
        }

        /// <summary>
        /// 为城市-非城市土地利用变化计算FoM精度。
        /// </summary>
        /// <param name="startImage"></param>
        /// <param name="endImage"></param>
        /// <param name="simulatedImage"></param>
        /// <param name="rowCount"></param>
        /// <param name="columnCount"></param>
        /// <returns></returns>
        public static float[] GetBinaryFoMAccuracy(float[,] startImage, float[,] endImage, float[,] simulatedImage,
            int rowCount, int columnCount, float urbanValue)
        {
            float[] values = new float[7];
            for (int i = 0; i < rowCount; i++)
                for (int j = 0; j < columnCount; j++)
                {
                    if (startImage[i, j] == -9999f)
                        continue;
                    if ((startImage[i, j] != urbanValue) && (endImage[i, j] != urbanValue))
                    {
                        if (simulatedImage[i, j] == urbanValue)
                            values[3] += 1;
                    }
                    else if ((startImage[i, j] != urbanValue) && (endImage[i, j] == urbanValue))
                    {
                        if (simulatedImage[i, j] != urbanValue)
                            values[0] += 1;
                        else if (simulatedImage[i, j] == urbanValue)
                            values[1] += 1;
                    }
                    else if ((startImage[i, j] == urbanValue) && (endImage[i, j] != urbanValue))
                    {
                        if (simulatedImage[i, j] != urbanValue)
                            values[1] += 1;
                        else if (simulatedImage[i, j] == urbanValue)
                            values[0] += 1;
                    }
                    else if ((startImage[i, j] == urbanValue) && (endImage[i, j] == urbanValue))
                    {
                        if (simulatedImage[i, j] != urbanValue)
                            values[3] += 1;
                    }
                }
            //Figure of merit = B/(A + B + C + D)
            values[4] = values[1] / (values[0] + values[1] + values[2] + values[3]);
            //Producer's Accuracy = B/(A + B + C)
            values[5] = values[1] / (values[0] + values[1] + values[2]);
            //User's Accuracy = B/(B + C + D)
            values[6] = values[1] / (values[1] + values[2] + values[3]);

            return values;
        }

        /// <summary>
        /// 根据计算出的混淆矩阵输出文本。
        /// </summary>
        /// <param name="dtConfusionMatrix"></param>
        /// <returns></returns>
        public static string WriteCoufusionMatrix(DataTable dtConfusionMatrix)
        {
            StringBuilder sb = new StringBuilder();
            string tempString = "";
            for (int i = 0; i < dtConfusionMatrix.Columns.Count; i++)
                tempString += dtConfusionMatrix.Columns[i].ColumnName + ", ";
            tempString = tempString.TrimEnd(',', ' ');
            sb.AppendLine(tempString);
            for (int i = 0; i < dtConfusionMatrix.Rows.Count; i++)
            {
                tempString = "";
                tempString += dtConfusionMatrix.Columns[i].ColumnName + " ";
                for (int j = 0; j < dtConfusionMatrix.Columns.Count; j++)
                {
                    tempString += dtConfusionMatrix.Rows[i][j].ToString() + ", ";
                }
                tempString = tempString.TrimEnd(',', ' ');
                sb.AppendLine(tempString);
            }
            return sb.ToString();
        }

        /// <summary>
        /// 根据给定的时间返回全字符串
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string GetDataTimeFullString(DateTime dateTime)
        {
            string str = "";
            if (DateTime.Now.Month.ToString().Length == 1)
                str = str + "0";
            str += DateTime.Now.Month.ToString();
            if (DateTime.Now.Day.ToString().Length == 1)
                str = str + "0";
            str += DateTime.Now.Day.ToString();
            if (DateTime.Now.Hour.ToString().Length == 1)
                str = str + "0";
            str += DateTime.Now.Hour.ToString();
            if (DateTime.Now.Minute.ToString().Length == 1)
                str = str + "0";
            str += DateTime.Now.Minute.ToString();
            if (DateTime.Now.Second.ToString().Length == 1)
                str = str + "0";
            str += DateTime.Now.Second.ToString();
            return str;
        }
    }
}
