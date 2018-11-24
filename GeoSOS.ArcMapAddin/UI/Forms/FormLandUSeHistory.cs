using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GeoSOS.CommonLibrary;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.DataSourcesRaster;
using System.Resources;
using System.IO;

namespace GeoSOS.ArcMapAddIn.UI.Forms
{
    public partial class FormLandUseHistory : Form
    {
        private IMap map;
        /// <summary>
        /// 当前地图所有图层列表
        /// </summary>
        List<string> listAllLayersInMapName;
        /// <summary>
        /// 资源类
        /// </summary>
        private ResourceManager resourceManager;
        /// <summary>
        /// 当前土地利用分类数据的类型数量
        /// </summary>
        int landUseTypesCount = 0;

        public FormLandUseHistory()
        {
            InitializeComponent();
            map = ((IMxDocument)ArcMap.Application.Document).FocusMap;
            ArcGISOperator.FoucsMap = map;
            listAllLayersInMapName = new List<string>();
            resourceManager = VariableMaintainer.CurrentResourceManager;
            landUseTypesCount = 0;
        }

        private void buttonAddLayer_Click(object sender, EventArgs e)
        {
            DataGridViewRow row = new DataGridViewRow();
            FormVariables formVaiables = new FormVariables();
            for (int i = 0; i < map.LayerCount; i++)
            {
                string layerName = map.get_Layer(i).Name;
                formVaiables.ListViewControl.Items.Add(layerName);
            }
            if (formVaiables.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                for (int i = 0; i < formVaiables.ListViewControl.SelectedItems.Count; i++)
                {
                    dataGridViewLayers.Rows.Add(formVaiables.ListViewControl.SelectedItems[i].Text);
                    listAllLayersInMapName.Add(formVaiables.ListViewControl.SelectedItems[i].Text);
                }
            }
            if (listAllLayersInMapName.Count > 0)
                FillLanduseInfo();
        }

        private void buttonDelLayer_Click(object sender, EventArgs e)
        {
            if (dataGridViewLayers.SelectedRows.Count > 0)
            {
                while (dataGridViewLayers.SelectedRows.Count > 0)
                {
                    listAllLayersInMapName.Remove(dataGridViewLayers.SelectedRows[0].Cells[0].Value.ToString());
                    dataGridViewLayers.Rows.Remove(dataGridViewLayers.SelectedRows[0]);
                }
            }
            if (listAllLayersInMapName.Count == 0)
                dataGridViewLandUse.Rows.Clear();
        }

        private void buttonStatictics_Click(object sender, EventArgs e)
        {
            if (dataGridViewLandUse.Rows.Count == 0)
            {
                MessageBox.Show(resourceManager.GetString("String153"),
                            resourceManager.GetString("String2"), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }
            //判断是否有类型被选择为城市用地
            bool isChecked = false;
            for (int i = 0; i < dataGridViewLandUse.Rows.Count; i++)
            {
                DataGridViewCheckBoxCell chk = dataGridViewLandUse.Rows[i].Cells[1] as DataGridViewCheckBoxCell;
                if (chk.Value == chk.TrueValue)
                    isChecked = true;
            }
            if (!isChecked)
            {
                MessageBox.Show(resourceManager.GetString("String66"),
                            resourceManager.GetString("String2"), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
                CalculateHistoryTrends(listAllLayersInMapName);
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 读取并填充土地利用类型信息。
        /// </summary>
        private void FillLanduseInfo()
        {
            dataGridViewLandUse.Rows.Clear();
            //根据第一个图层的符号化设置读取土地利用类型信息。
            //注意：因为一般来说符号化设置的设置和属性表一致，因为不需要考虑顺序不一致的问题。
            IRasterLayer rasterLayer = ArcGISOperator.GetRasterLayerByName(listAllLayersInMapName[0]);
            IRasterUniqueValueRenderer rasterUniqueValueRenderer = (IRasterUniqueValueRenderer)rasterLayer.Renderer;
            IRasterRendererUniqueValues rasterRendererUniqueValues = (IRasterRendererUniqueValues)rasterUniqueValueRenderer;
            IUniqueValues uniqueValues = rasterRendererUniqueValues.UniqueValues;
            List<object> listUniqueValues = new List<object>();
            for (int i = 0; i < uniqueValues.Count; i++)
            {
                listUniqueValues.Add(uniqueValues.get_UniqueValue(i));
            }
            int classCount = rasterUniqueValueRenderer.get_ClassCount(0);
            landUseTypesCount = classCount;
            for (int i = 0; i < classCount; i++)
            {
                dataGridViewLandUse.Rows.Add();
                dataGridViewLandUse.Rows[i].Cells[0].Value = rasterUniqueValueRenderer.get_Label(0, i);
            }
        }

        /// <summary>
        /// 根据图层的列表计算土地利用变化历史趋势。
        /// </summary>
        /// <param name="listLayers"></param>
        private void CalculateHistoryTrends(List<string> listLayers)
        {
            //写入列。列名为图层名。
            DataTable dtResults = new DataTable();
            for (int i = 0; i < listLayers.Count; i++)
                dtResults.Columns.Add(listLayers[i]);
            //写入行。除了所有的土地利用类型的栅格数量，还有数量总计，土地开发强度，城市用地增长率。
            for (int i = 0; i < landUseTypesCount + 3; i++)
            {
                DataRow row = dtResults.NewRow();
                dtResults.Rows.Add(row);
            }
            //计算并写入数据
            int urbanIndex = GetUrbanIndex();
            int urbanCount = 0;
            IRasterLayer rasterLayer;
            IRasterUniqueValueRenderer rasterUniqueValueRenderer;
            for (int i = 0; i < listLayers.Count; i++)
            {
                rasterLayer = ArcGISOperator.GetRasterLayerByName(listAllLayersInMapName[i]);
                rasterUniqueValueRenderer = (IRasterUniqueValueRenderer)rasterLayer.Renderer;
                int[] counts = ArcGISOperator.GetLandUseTypesCount(rasterLayer);
                int sum = 0;
                for (int j = 0; j < landUseTypesCount; j++)
                {
                    dtResults.Rows[j][i] = counts[j];
                    sum += counts[j];
                    if (urbanIndex == j)
                        urbanCount = counts[j];
                }
                dtResults.Rows[landUseTypesCount][i] = sum;
                dtResults.Rows[landUseTypesCount + 1][i] = (Convert.ToDecimal(urbanCount) * 100 / sum).ToString("0.00");
                if (i != 0)
                {
                    int oldUrbanCount = Convert.ToInt32(dtResults.Rows[urbanIndex][i - 1]);
                    int newUrbanCount = Convert.ToInt32(dtResults.Rows[urbanIndex][i]);
                    dtResults.Rows[landUseTypesCount + 2][i] = (Convert.ToDecimal(newUrbanCount - oldUrbanCount) * 100
                        / oldUrbanCount).ToString("0.00");
                }
            }
            dataGridViewResult.DataSource = dtResults;
            //设置行名
            rasterLayer = ArcGISOperator.GetRasterLayerByName(listAllLayersInMapName[0]);
            rasterUniqueValueRenderer = (IRasterUniqueValueRenderer)rasterLayer.Renderer;
            for (int i = 0; i < landUseTypesCount; i++)
            {
                dataGridViewResult.RowHeadersWidth = 130;
                dataGridViewResult.Rows[i].HeaderCell.Value = rasterUniqueValueRenderer.get_Label(0, i);
            }
            dataGridViewResult.Rows[landUseTypesCount].HeaderCell.Value = resourceManager.GetString("String150");
            dataGridViewResult.Rows[landUseTypesCount + 1].HeaderCell.Value = resourceManager.GetString("String151");
            dataGridViewResult.Rows[landUseTypesCount + 2].HeaderCell.Value = resourceManager.GetString("String152");
        }

        private void buttonExport_Click(object sender, EventArgs e)
        {
            if (dataGridViewResult.Rows.Count == 0)
                return;

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Execl Files (*.xls)|*.xls";
            sfd.FilterIndex = 0;
            sfd.RestoreDirectory = true;
            sfd.Title = resourceManager.GetString("String154");

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                Stream myStream;
                myStream = sfd.OpenFile();
                StreamWriter sw = new StreamWriter(myStream, System.Text.Encoding.GetEncoding(-0));
                string columnTitle = "";
                try
                {
                    //写入列标题   
                    for (int i = -1; i < dataGridViewResult.ColumnCount; i++)
                    {
                        if (i > 0)
                        {
                            columnTitle += "\t";
                        }
                        if (i == -1)
                            columnTitle = " " + "\t";
                        else
                            columnTitle += dataGridViewResult.Columns[i].HeaderText;
                    }
                    sw.WriteLine(columnTitle);

                    //写入列内容   
                    for (int j = 0; j < dataGridViewResult.Rows.Count; j++)
                    {
                        string columnValue = "";
                        //添加行标题
                        if (dataGridViewResult.Rows[j].HeaderCell.Value != null)
                            columnValue = dataGridViewResult.Rows[j].HeaderCell.Value.ToString() + "\t";
                        for (int k = 0; k < dataGridViewResult.Columns.Count; k++)
                        {
                            if (k > 0)
                            {
                                columnValue += "\t";
                            }
                            if (dataGridViewResult.Rows[j].Cells[k].Value == null)
                                columnValue += "";
                            else
                                columnValue += dataGridViewResult.Rows[j].Cells[k].Value.ToString().Trim();
                        }
                        sw.WriteLine(columnValue);
                    }
                    sw.Close();
                    myStream.Close();
                    System.Windows.Forms.MessageBox.Show(resourceManager.GetString("String155"),
                            resourceManager.GetString("String2"), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                finally
                {
                    sw.Close();
                    myStream.Close();
                }
            }
        }

        /// <summary>
        /// 获取城市用地的索引。
        /// </summary>
        /// <returns></returns>
        private int GetUrbanIndex()
        {
            int index = -1;
            for (int i = 0; i < dataGridViewLandUse.Rows.Count; i++)
            {
                DataGridViewCheckBoxCell chk = dataGridViewLandUse.Rows[i].Cells[1] as DataGridViewCheckBoxCell;
                if (chk.Value == chk.TrueValue)
                {
                    index = i;
                    break;
                }
            }
            return index;
        }
    }
}
