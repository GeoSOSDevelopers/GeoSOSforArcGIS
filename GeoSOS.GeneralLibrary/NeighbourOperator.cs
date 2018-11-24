using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeoSOS.CommonLibrary
{
    public class NeighbourOperator
    {
        /// <summary>
        /// 对每一个栅格统计其邻域中各种土地利用类型值的数量。
        /// </summary>
        /// <param name="data"></param>
        /// <param name="rowIndex"></param>
        /// <param name="columnIndex"></param>
        /// <param name="windowSize"></param>
        /// <param name="rowCount"></param>
        /// <param name="columnCount"></param>
        /// <param name="landuseTypesCount"></param>
        /// <param name="landuseTypeValue"></param>
        /// <returns></returns>
        public float[] GetNeighbourCount(float[,] data, int rowIndex, int columnIndex, int windowSize,
             int rowCount, int columnCount, int landuseTypesCount, List<float> landuseTypeValue)
        {
            int neighbourLength = (windowSize - 1) / 2;
            float[,] tempValue = new float[windowSize, windowSize];
            float[] counts = new float[landuseTypesCount];
            try
            {
                for (int i = rowIndex - neighbourLength; i <= rowIndex + neighbourLength; i++)
                {
                    for (int j = columnIndex - neighbourLength; j <= columnIndex + neighbourLength; j++)
                    {
                        if (i < 0 || j < 0 || i >= rowCount || j >= columnCount)
                        {
                            tempValue[i - rowIndex + neighbourLength, j - columnIndex + neighbourLength] = -1f;
                            continue;
                        }
                        //System.Diagnostics.Debug.WriteLine(data[i, j]);
                        tempValue[i - rowIndex + neighbourLength, j - columnIndex + neighbourLength] = data[i, j];
                    }
                }
                for (int k = 0; k < windowSize; k++)
                {
                    for (int z = 0; z < windowSize; z++)
                    {
                        int index = landuseTypeValue.IndexOf(tempValue[k, z]);
                        if (index != -1)
                            counts[index]++;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
            return counts;
        }

        /// <summary>
        /// 对给定图像的每一个栅格统计其邻域中城市用地的数量。
        /// </summary>
        /// <param name="data"></param>
        /// <param name="windowSize"></param>
        /// <param name="rowCount"></param>
        /// <param name="columnCount"></param>
        /// <param name="urbanTypeValue"></param>
        /// <returns></returns>
        public float[,] GetNeighbourData(float[,] data, int windowSize,int rowCount, int columnCount,List<float> urbanTypeValue)
        {
            float[,] neighbourData = new float[rowCount, columnCount];
            int neighbourLength = (windowSize - 1) / 2;
            float[,] tempValue = new float[windowSize, windowSize];
            int urbanCount = 0;
            for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < columnCount; columnIndex++)
                {
                    for (int i = rowIndex - neighbourLength; i <= rowIndex + neighbourLength; i++)
                    {
                        for (int j = columnIndex - neighbourLength; j <= columnIndex + neighbourLength; j++)
                        {
                            if (i < 0 || j < 0 || i >= rowCount || j >= columnCount)
                            {
                                tempValue[i - rowIndex + neighbourLength, j - columnIndex + neighbourLength] = -1f;
                                continue;
                            }
                            tempValue[i - rowIndex + neighbourLength, j - columnIndex + neighbourLength] = data[i, j];
                            //System.Diagnostics.Debug.WriteLine(data[i, j]);
                        }
                    }
                    urbanCount = 0;
                    for (int k = 0; k < windowSize; k++)
                    {
                        for (int z = 0; z < windowSize; z++)
                        {
                            int index = urbanTypeValue.IndexOf(tempValue[k, z]);
                            if (index != -1)
                                urbanCount++;
                        }
                    }
                    neighbourData[rowIndex, columnIndex] = urbanCount;
                }
            }
            return neighbourData;
        }
    }
}
