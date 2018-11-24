using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ZedGraph;
using GeoSOS.CommonLibrary.Struct;

namespace GeoSOS.ArcMapAddIn
{
    /// <summary>
    /// Designer class of the dockable window add-in. It contains user interfaces that
    /// make up the dockable window.
    /// </summary>
    public partial class DockableWindowGraphy : UserControl
    {
        private ZedGraphControl zgc;
        private GraphPane myPane;

        public string GraphTitle
        {
            set
            {
                myPane.Title.Text = value;
            }
        }

        public string XAxisTitle
        {
            set
            {
                myPane.XAxis.Title.Text = value;
            }
        }

        public string YAxisTitle
        {
            set
            {
                myPane.YAxis.Title.Text = value;
            }
        }

        public DockableWindowGraphy(object hook)
        {
            System.Threading.Thread.CurrentThread.CurrentUICulture = System.Threading.Thread.CurrentThread.CurrentCulture;
            InitializeComponent();
            this.Hook = hook;
            zgc = this.zedGraphControl;
            myPane = zgc.GraphPane;
        }

        /// <summary>
        /// Host object of the dockable window
        /// </summary>
        private object Hook
        {
            get;
            set;
        }

        /// <summary>
        /// Implementation class of the dockable window add-in. It is responsible for 
        /// creating and disposing the user interface class of the dockable window.
        /// </summary>
        public class AddinImpl : ESRI.ArcGIS.Desktop.AddIns.DockableWindow
        {
            private DockableWindowGraphy m_windowUI;

            public DockableWindowGraphy DockableWindowGraphy
            {
                get { return m_windowUI; }
            }

            public AddinImpl()
            {
            }

            protected override IntPtr OnCreateChild()
            {
                m_windowUI = new DockableWindowGraphy(this.Hook);
                return m_windowUI.Handle;
            }

            protected override void Dispose(bool disposing)
            {
                if (m_windowUI != null)
                    m_windowUI.Dispose(disposing);

                base.Dispose(disposing);
            }

        }

        /// <summary>
        /// 根据土地利用类型的值，含义及数量信息创建图表。
        /// </summary>
        /// <param name="x">迭代次数。</param>
        /// <param name="listLanduseNameValueCount">土地利用类型的值，含义及数量信息列表。</param>
        /// <param name="notToDrawList">不需要绘制的土地利用类型的含义列表。</param>
        public void CreateGraph(List<StructLanduseInfoAndCount> listLanduseInfoAndCount, List<string> notToDrawList, out List<string> pointPairListName)
        {
            myPane.CurveList.Clear();
            pointPairListName = new List<string>();
            string landuseTypeName = "";

            foreach (StructLanduseInfoAndCount landuseInfoAndCount in listLanduseInfoAndCount)
            {
                if (System.Globalization.CultureInfo.CurrentCulture.Parent.Name == "zh-CHS")
                    landuseTypeName = landuseInfoAndCount.structLanduseInfo.LanduseTypeChsName;
                else if (System.Globalization.CultureInfo.CurrentCulture.Parent.Name == "zh-CHT")
                    landuseTypeName = landuseInfoAndCount.structLanduseInfo.LanduseTypeChtName;
                else
                    landuseTypeName = landuseInfoAndCount.structLanduseInfo.LanduseTypeEnName;

                if (notToDrawList != null)
                {
                    if (!notToDrawList.Contains(landuseTypeName))
                    {
                        PointPairList list = new PointPairList();
                        list.Add(0, Convert.ToDouble(landuseInfoAndCount.LanduseTypeCount));
                        pointPairListName.Add(landuseTypeName);
                        myPane.AddCurve(landuseTypeName, list, Color.FromArgb(landuseInfoAndCount.structLanduseInfo.LanduseTypeColorIntValue), SymbolType.None);
                    }
                }
                else
                {
                    PointPairList list = new PointPairList();
                    list.Add(0, Convert.ToDouble(landuseInfoAndCount.LanduseTypeCount));
                    pointPairListName.Add(landuseTypeName);
                    myPane.AddCurve(landuseTypeName, list, Color.FromArgb(landuseInfoAndCount.structLanduseInfo.LanduseTypeColorIntValue), SymbolType.None);
                }
            }

            myPane.Chart.Fill = new Fill(Color.White, Color.LightGoldenrodYellow, 45F);
            myPane.Fill = new Fill(Color.White, Color.FromArgb(220, 220, 255), 45F);

            zgc.AxisChange();
        }

        /// <summary>
        /// 使用新的X，Y坐标值更新Index为输入值的CurveList。
        /// </summary>
        /// <param name="newX"></param>
        /// <param name="newY"></param>
        /// <param name="pointPairListIndex"></param>
        public void UpdateData(double newX, double newY, int pointPairListIndex)
        {
            myPane.CurveList[pointPairListIndex].AddPoint(newX, newY);
            if (zgc.InvokeRequired)
            {
                Action<int> actionDelegate = delegate(int i) { this.zgc.AxisChange(); };
                this.zgc.Invoke(actionDelegate, 0);
            }
            else
                zgc.AxisChange();
        }

        public void RefreshGraph()
        {
            if (zgc.InvokeRequired)
            {
                Action<int> actionDelegate = delegate(int i) { this.zgc.Refresh(); };
                this.zgc.Invoke(actionDelegate, 0);
            }
            else
                zgc.Refresh();
        }

        /// <summary>
        ///  建立一个基本的曲线。初始值为0。
        /// </summary>
        /// <param name="label">曲线的名称。</param>
        /// <param name="initialY">初始化时的纵坐标值。</param>
        public void CreateGraph(string label, double initialY)
        {
            myPane.CurveList.Clear();
            PointPairList list = new PointPairList();
            list.Add(0, initialY);
            myPane.AddCurve(label, list, Color.Red, SymbolType.Square);

            myPane.Chart.Fill = new Fill(Color.White, Color.LightGoldenrodYellow, 45F);
            myPane.Fill = new Fill(Color.White, Color.FromArgb(220, 220, 255), 45F);

            zgc.AxisChange();
        }

        public void CreateGraph(int interation, double goalUtilityValue, string label)
        {
            myPane.CurveList.Clear();

            PointPairList list = new PointPairList();
            list.Add(interation, goalUtilityValue);
            myPane.AddCurve(label, list, Color.Red, SymbolType.None);

            myPane.Chart.Fill = new Fill(Color.White, Color.LightGoldenrodYellow, 45F);
            myPane.Fill = new Fill(Color.White, Color.FromArgb(220, 220, 255), 45F);

            zgc.AxisChange();
        }
    }
}
