using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeoSOS.CommonLibrary.Struct
{
    /// <summary>
    /// 记录ASCII码格式的影像文件的元数据结构体。
    /// </summary>
    public struct StructRasterMetaData
    {
        /// <summary>
        /// 栅格的总行数。
        /// </summary>
        public int RowCount;
        /// <summary>
        /// 栅格的总列数。
        /// </summary>
        public int ColumnCount;
        /// <summary>
        /// X轴最小值。
        /// </summary>
        public double XMin;
        /// <summary>
        /// Y轴最小值。
        /// </summary>
        public double YMin;
        /// <summary>
        /// X轴最大值。
        /// </summary>
        public double XMax;
        /// <summary>
        /// Y轴最大值。
        /// </summary>
        public double YMax;
        /// <summary>
        /// 空值的代表值。
        /// </summary>
        public float NoDataValue;
     }
}
