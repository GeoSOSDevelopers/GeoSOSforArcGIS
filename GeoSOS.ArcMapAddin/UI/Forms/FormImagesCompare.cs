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

namespace GeoSOS.ArcMapAddIn.UI.Forms
{
    public partial class FormImagesCompare : Form
    {
        private IMap map;
        private ResourceManager resourceManager;

        public FormImagesCompare()
        {
            InitializeComponent();
            map = ((IMxDocument)ArcMap.Application.Document).FocusMap;
            ArcGISOperator.FoucsMap = map;
            resourceManager = VariableMaintainer.CurrentResourceManager;
            FillLayers();
        }

        private void radioButtonTwoImages_CheckedChanged(object sender, EventArgs e)
        {
            InitialComboBoxs();
            labelDescription.Text = resourceManager.GetString("String156");
            labelStartTime.Text = resourceManager.GetString("String157");
            labelEndTime.Text = resourceManager.GetString("String158");
            labelSimulationData.Visible = false;
            comboBoxSimulation.Visible = false;
        }

        private void radioButtonActualSimualtion_CheckedChanged(object sender, EventArgs e)
        {
            InitialComboBoxs();
            labelDescription.Text = resourceManager.GetString("String160");
            labelStartTime.Text = resourceManager.GetString("String161");
            labelEndTime.Text = resourceManager.GetString("String159");
            labelSimulationData.Visible = false;
            comboBoxSimulation.Visible = false;
        }

        private void radioButtonThreeImages_CheckedChanged(object sender, EventArgs e)
        {
            InitialComboBoxs();
            labelDescription.Text = resourceManager.GetString("String162");
            labelStartTime.Text = resourceManager.GetString("String157");
            labelEndTime.Text = resourceManager.GetString("String158");
            labelSimulationData.Visible = true;
            comboBoxSimulation.Visible = true;
            labelSimulationData.Text = resourceManager.GetString("String159");
        }

        private void buttonCompare_Click(object sender, EventArgs e)
        {
            //先判断数据是否为空
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
                ExceuteCompre();
        }

        private void buttonPercent_Click(object sender, EventArgs e)
        {

        }

        private void ExceuteCompre()
        {

        }

        private void FillLayers()
        {
            for (int i = 0; i < map.LayerCount; i++)
            {
                string layerName = map.get_Layer(i).Name;
                comboBoxLayerStart.Items.Add(layerName);
                comboBoxLayerEnd.Items.Add(layerName);
                comboBoxSimulation.Items.Add(layerName);
            }
        }

        private void InitialComboBoxs()
        {
            comboBoxLayerStart.SelectedIndex = -1;
            comboBoxLayerEnd.SelectedIndex = -1;
            comboBoxSimulation.SelectedIndex = -1;
        }

        private void comboBoxLayerStart_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxLayerStart.SelectedItem != null)
                FillLanduseInfo(comboBoxLayerStart.SelectedItem.ToString());
            else
                dataGridViewLandUse.Rows.Clear();
        }

        /// <summary>
        /// 读取并填充土地利用类型信息。
        /// </summary>
        private void FillLanduseInfo(string layerName)
        {
            dataGridViewLandUse.Rows.Clear();
            //根据第一个图层的符号化设置读取土地利用类型信息。
            //注意：因为一般来说符号化设置的设置和属性表一致，因为不需要考虑顺序不一致的问题。
            IRasterLayer rasterLayer = ArcGISOperator.GetRasterLayerByName(layerName);
            IRasterUniqueValueRenderer rasterUniqueValueRenderer = (IRasterUniqueValueRenderer)rasterLayer.Renderer;
            IRasterRendererUniqueValues rasterRendererUniqueValues = (IRasterRendererUniqueValues)rasterUniqueValueRenderer;
            IUniqueValues uniqueValues = rasterRendererUniqueValues.UniqueValues;
            List<object> listUniqueValues = new List<object>();
            for (int i = 0; i < uniqueValues.Count; i++)
            {
                listUniqueValues.Add(uniqueValues.get_UniqueValue(i));
            }
            int classCount = rasterUniqueValueRenderer.get_ClassCount(0);
            for (int i = 0; i < classCount; i++)
            {
                dataGridViewLandUse.Rows.Add();
                dataGridViewLandUse.Rows[i].Cells[0].Value = rasterUniqueValueRenderer.get_Label(0, i);
            }
        }
    }
}
