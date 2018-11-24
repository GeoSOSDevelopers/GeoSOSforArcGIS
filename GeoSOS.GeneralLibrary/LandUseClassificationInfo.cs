using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using GeoSOS.CommonLibrary.Struct;

namespace GeoSOS.CommonLibrary
{
    /// <summary>
    /// 记录当前使用的土地利用分类图所使用具体数值与表示含义之间的对应关系。
    /// </summary>
    public class LandUseClassificationInfo
    {
        # region 变量

        /// <summary>
        /// 表示城市用地的数值。
        /// </summary>
        private List<StructLanduseInfo> urbanValues;
        /// <summary>
        /// 可以被转换为城市用地的土地利用类型值。
        /// </summary>
        private List<StructLanduseInfo> convertValues;
        /// <summary>
        /// 不能被转换为城市用地的土地利用类型值。
        /// </summary>
        private List<StructLanduseInfo> notToConvertValues;


        /// <summary>
        /// 空值数据类型。
        /// </summary>
        private StructLanduseInfo nullValue;


        /// <summary>
        /// 所有土地利用类型的总类别数。
        /// </summary>
        private int allTypesCount;
        /// <summary>
        /// 所有的土地利用类型及其对应类型值。
        /// </summary>
        private List<StructLanduseInfo> allTypes;

        #endregion

        #region 属性

        /// <summary>
        /// 可以被转换为城市用地的土地利用类型值。
        /// </summary>
        public List<StructLanduseInfo> ConvertValues
        {
            get
            {
                if (convertValues == null)
                    convertValues = new List<StructLanduseInfo>();
                return convertValues;
            }
            set
            {
                convertValues = value;
            }
        }

        /// <summary>
        /// 不能被转换为城市用地的土地利用类型值。
        /// </summary>
        public List<StructLanduseInfo> NotToConvertValues
        {
            get
            {
                if (notToConvertValues == null)
                    notToConvertValues = new List<StructLanduseInfo>();
                return notToConvertValues;
            }
            set
            {
                notToConvertValues = value;
            }
        }

        /// <summary>
        /// 表示城市用地的数值。
        /// </summary>
        public List<StructLanduseInfo> UrbanValues
        {
            get
            {
                if (urbanValues == null)
                    urbanValues = new List<StructLanduseInfo>();
                return urbanValues;
            }
            set
            {
                urbanValues = value;
            }
        }


        /// <summary>
        /// 所有土地利用类型的总类别数。
        /// </summary>
        public int AllTypesCount
        {
            get
            {
                return allTypesCount;
            }
            set
            {
                allTypesCount = value;
            }
        }

        /// <summary>
        /// 所有的土地利用类型及其对应类型值。
        /// </summary>
        public List<StructLanduseInfo> AllTypes
        {
            get
            {
                if (allTypes == null)
                    allTypes = new List<StructLanduseInfo>();
                return allTypes;
            }
            set
            {
                allTypes = value;
            }
        }


        /// <summary>
        /// 空值数据类型。
        /// </summary>
        public StructLanduseInfo NullValue
        {
            get
            {
                if (nullValue.LanduseTypeChsName == null)
                {
                    nullValue = new StructLanduseInfo();
                    nullValue.LanduseTypeChsName = "数据空值";
                    nullValue.LanduseTypeEnName = "Null Data";
                    nullValue.LanduseTypeChtName = "空值數據";
                    nullValue.LanduseTypeValue = -9999f;
                    nullValue.LanduseTypeColorIntValue = Color.White.ToArgb();
                }
                return nullValue;
            }
            set
            {
                nullValue = value;
            }
        }

        #endregion

        #region 方法

        /// <summary>
        /// 将当前记录的土地利用清空，不包括所有类型的总数。
        /// </summary>
        public void ClearLists()
        {
            nullValue.LanduseTypeValue = -9999f;
            nullValue.LanduseTypeColorIntValue = Color.White.ToArgb();
            AllTypes.Clear();
            ConvertValues.Clear();
            NotToConvertValues.Clear();
            UrbanValues.Clear();
        }

        /// <summary>
        /// 将当前所有的数据，包括所有类型的总数。
        /// </summary>
        public void ClearAll()
        {
            nullValue.LanduseTypeValue = -9999f;
            nullValue.LanduseTypeColorIntValue = Color.White.ToArgb();
            AllTypesCount = 0;
            AllTypes.Clear();
            ConvertValues.Clear();
            NotToConvertValues.Clear();
            UrbanValues.Clear();
        }

        /// <summary>
        /// 判断某种土地类型的值是否是城市用地类型。
        /// </summary>
        public bool IsExistInUrbanValue(float value)
        {
            for (int i = 0; i < UrbanValues.Count; i++)
            {
                if (UrbanValues[i].LanduseTypeValue == value)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 判断某种土地类型的值是否是可以转换为城市用地类型。
        /// </summary>
        public bool IsExistInConvertValue(float value)
        {
            for (int i = 0; i < ConvertValues.Count; i++)
            {
                if (ConvertValues[i].LanduseTypeValue == value)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 判断某种土地类型的值是否是不可以转换为城市用地类型。
        /// </summary>
        public bool IsExistInNotConvertValue(float value)
        {
            for (int i = 0; i < NotToConvertValues.Count; i++)
            {
                if (NotToConvertValues[i].LanduseTypeValue == value)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 根据输入的土地利用类型值得到其名称。
        /// </summary>
        /// <param name="value"></param>
        /// <param name="localizationString"></param>
        /// <returns></returns>
        public string GetLanduseTypeNameByValue(float value, string localizationString)
        {
            //for (int i = 0; i < UrbanValues.Count; i++)
            //{
            //    if (UrbanValues[i].LanduseTypeValue == value)
            //        return GetLandTypeNameByLocalString(UrbanValues[i], localizationString);
            //}
            //for (int i = 0; i < ConvertValues.Count; i++)
            //{
            //    if (ConvertValues[i].LanduseTypeValue == value)
            //        return GetLandTypeNameByLocalString(ConvertValues[i], localizationString);
            //}
            //for (int i = 0; i < NotToConvertValues.Count; i++)
            //{
            //    if (NotToConvertValues[i].LanduseTypeValue == value)
            //        return GetLandTypeNameByLocalString(NotToConvertValues[i], localizationString);
            //}
            for (int i = 0; i < AllTypesCount; i++)
            {
                if (AllTypes[i].LanduseTypeValue == value)
                    return GetLandTypeNameByLocalString(AllTypes[i], localizationString);
            }
            return null;
        }

        /// <summary>
        /// 根据输入的区域语言字符串得到相应土地利用类型的名称。
        /// </summary>
        /// <param name="structLanduseInfo"></param>
        /// <param name="localizationString"></param>
        /// <returns></returns>
        public string GetLandTypeNameByLocalString(StructLanduseInfo structLanduseInfo, string localizationString)
        {
            switch (localizationString)
            {
                case "zh-CHS":
                    return structLanduseInfo.LanduseTypeChsName;
                case "en":
                    return structLanduseInfo.LanduseTypeEnName;
                case "zh-CHT":
                    return structLanduseInfo.LanduseTypeChtName;
                default:
                    return "";
            }
        }

        /// <summary>
        /// 根据输入的土地利用类型值得到其颜色。
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public Color GetLanduseTypeColorByValue(float value)
        {
            //for (int i = 0; i < UrbanValues.Count; i++)
            //{
            //    if (UrbanValues[i].LanduseTypeValue == value)
            //        return Color.FromArgb(UrbanValues[i].LanduseTypeColorIntValue);
            //}
            //for (int i = 0; i < ConvertValues.Count; i++)
            //{
            //    if (ConvertValues[i].LanduseTypeValue == value)
            //        return Color.FromArgb(ConvertValues[i].LanduseTypeColorIntValue);
            //}
            //for (int i = 0; i < NotToConvertValues.Count; i++)
            //{
            //    if (NotToConvertValues[i].LanduseTypeValue == value)
            //        return Color.FromArgb(NotToConvertValues[i].LanduseTypeColorIntValue);
            //}
            for (int i = 0; i < AllTypesCount; i++)
            {
                if (AllTypes[i].LanduseTypeValue == value)
                    return Color.FromArgb(AllTypes[i].LanduseTypeColorIntValue);
            }
            return Color.White;
        }

        /// <summary>
        /// 根据输入的土地利用类型的值删除相应的土地利用类型。
        /// </summary>
        /// <param name="value"></param>
        public void DeleteLanduseTypeByValue(float value)
        {
            for (int i = 0; i < AllTypesCount; i++)
            {
                if (AllTypes[i].LanduseTypeValue == value)
                {
                    DeleteLanduseTypeByValueInLists(value);
                    AllTypes.RemoveAt(i);
                    AllTypesCount -= 1;
                    return;
                }
            }
        }

        /// <summary>
        /// 在各种土地利用类型的列表中删除相应的土地利用类型。
        /// </summary>
        /// <param name="value"></param>
        public void DeleteLanduseTypeByValueInLists(float value)
        {
            for (int i = 0; i < UrbanValues.Count; i++)
            {
                if (UrbanValues[i].LanduseTypeValue == value)
                {
                    UrbanValues.RemoveAt(i);
                    return;
                }
            }
            for (int i = 0; i < ConvertValues.Count; i++)
            {
                if (ConvertValues[i].LanduseTypeValue == value)
                {
                    ConvertValues.RemoveAt(i);
                    return;
                }
            }
            for (int i = 0; i < NotToConvertValues.Count; i++)
            {
                if (NotToConvertValues[i].LanduseTypeValue == value)
                {
                    NotToConvertValues.RemoveAt(i);
                    return;
                }
            }
        }

        public void AddUrbanLanduseType(StructLanduseInfo structLanduseInfo)
        {
            urbanValues.Add(structLanduseInfo);
            allTypes.Add(structLanduseInfo);
            allTypesCount += 1;
        }

        public void AddConvertLanduseType(StructLanduseInfo structLanduseInfo)
        {
            convertValues .Add(structLanduseInfo);
            allTypes.Add(structLanduseInfo);
            allTypesCount += 1;
        }

        public void AddNotToConvertLanduseType(StructLanduseInfo structLanduseInfo)
        {
            this.notToConvertValues.Add(structLanduseInfo);
            allTypes.Add(structLanduseInfo);
            allTypesCount += 1;
        }

        public void AddNullValue()
        {
            StructLanduseInfo newNullValue = new StructLanduseInfo();
            newNullValue.LanduseTypeChsName = "数据空值";
            newNullValue.LanduseTypeEnName = "Null Data";
            newNullValue.LanduseTypeChtName = "空值數據";
            newNullValue.LanduseTypeValue = -9999f;
            newNullValue.LanduseTypeColorIntValue = Color.White.ToArgb();
            this.nullValue = newNullValue;
        }

        #endregion
    }
}
