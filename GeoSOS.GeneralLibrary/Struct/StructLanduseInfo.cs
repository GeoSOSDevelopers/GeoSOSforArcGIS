using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeoSOS.CommonLibrary.Struct
{
    /// <summary>
    /// 记录某土地利用类型的值、含义和颜色信息。
    /// </summary>
    public struct StructLanduseInfo
    {
        /// <summary>
        /// 该土地利用类型的简体中文名称。
        /// </summary>
        public string LanduseTypeChsName;
        /// <summary>
        /// 该土地利用类型的英文名称。
        /// </summary>
        public string LanduseTypeEnName;
        /// <summary>
        /// 该土地利用类型的繁体中文名称。
        /// </summary>
        public string LanduseTypeChtName;
        /// <summary>
        /// 该土地利用类型的值。
        /// </summary>
        public float LanduseTypeValue;
        /// <summary>
        /// 该土地利用类型使用的颜色的Int值。(不直接使用Color结构是因为该结构无法直接序列化)
        /// </summary>
        public int LanduseTypeColorIntValue;
    }
}
