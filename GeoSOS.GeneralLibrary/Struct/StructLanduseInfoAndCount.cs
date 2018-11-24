using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeoSOS.CommonLibrary.Struct
{
    /// <summary>
    /// 记录土地利用类型的信息和单元数量。
    /// </summary>
    public struct StructLanduseInfoAndCount
    {
        /// <summary>
        /// 该土地利用类型使用的信息。
        /// </summary>
        public StructLanduseInfo structLanduseInfo;
        /// <summary>
        /// 该土地利用类型当前的元胞数量。
        /// </summary>
        public int LanduseTypeCount;
    }
}
