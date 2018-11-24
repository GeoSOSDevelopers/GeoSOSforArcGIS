using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeoSOS.CommonLibrary.Struct
{
    /// <summary>
    /// 记录混淆矩阵及精度信息(以观测数据为基准比较)。
    /// </summary>
    public struct StructBinaryConfusionMatrix
    {
        /// <summary>
        /// 城市用地模拟为城市用地的单元数量。
        /// </summary>
        public int Urban2UrbanCount;
        /// <summary>
        /// 城市用地中模拟为非城市用地的单元数量。
        /// </summary>
        public int Urban2NotUrbanCount;
        /// <summary>
        /// 非城市用地中模拟为城市用地的单元数量。
        /// </summary>
        public int NotUrban2UrbanCount;
        /// <summary>
        /// 非城市用地中模拟为非城市用地的单元数量。
        /// </summary>
        public int NotUrban2NotUrbanCount;

        /// <summary>
        /// 观测数据中总的城市用地单元总数。
        /// </summary>
        public int OverallObseredDataUrbanCount;
        /// <summary>
        /// 观测数据中总的非城市用地单元总数。
        /// </summary>
        public int OverallObseredDataNotUrbanCount;
        /// <summary>
        /// 模拟结果中总的城市用地单元总数。
        /// </summary>
        public int OverallSimulatedDataUrbanCount;
        /// <summary>
        /// 模拟结果中总的非城市用地单元总数。
        /// </summary>
        public int OverallSimulatedDataNotUrbanCount;
        /// <summary>
        /// 数据总的单元数。
        /// </summary>
        public int OverallDataCout;

        /// <summary>
        /// 城市用地单元模拟的正确率。
        /// </summary>
        public double UrabanCorrectPrecent;
        /// <summary>
        /// 非城市用地单元模拟的正确率。
        /// </summary>
        public double NotUrbanCorrectPrecent;
        /// <summary>
        /// 总的模拟正确率。
        /// </summary>
        public double OverallCorrectPrecent;
    }
}
