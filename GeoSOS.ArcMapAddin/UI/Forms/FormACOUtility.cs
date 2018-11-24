using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using ESRI.ArcGIS.Carto;
using GeoSOS.CommonLibrary;
using GeoSOS.CommonLibrary.Struct;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesRaster;

namespace GeoSOS.ArcMapAddIn.UI.Forms
{
    public partial class FormACOUtility : Form
    {
        private int insertTextIndex = 0;

        public TextBox TextBoxUtilityFormula
        {
            get { return this.textBoxUtility; }
            }

        public FormACOUtility()
        {
            InitializeComponent();
            Initial();
        }

        public void Initial()
        {
            ArcGISOperator.FoucsMap = VariableMaintainer.CurrentFoucsMap;
            dataGridViewLayers.Rows.Clear();
            AddMapLayers();
        }

        private void AddMapLayers()
        {
            for (int i = 0; i < VariableMaintainer.CurrentFoucsMap.LayerCount; i++)
            {
                string layerName = VariableMaintainer.CurrentFoucsMap.get_Layer(i).Name;
                dataGridViewLayers.Rows.Add(layerName);
            }
        }

        private void dataGridViewLayers_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            string layerName = dataGridViewLayers.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
            string appendText = "[" + layerName + "]";
            string text = textBoxUtility.Text;
            textBoxUtility.SelectionStart = insertTextIndex;
            text = text.Insert(insertTextIndex, appendText);
            textBoxUtility.Text = text;
            insertTextIndex += appendText.Length;
            textBoxUtility.Focus();
            textBoxUtility.Select(insertTextIndex, 0);
        }
   
        /// <summary>
        /// 单击操作数和操作符执行的操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void operator_Click(object sender, EventArgs e)
        {
            textBoxUtility.Focus();
            Button buttonOperator = (Button)sender;
            string operatorString = buttonOperator.Tag.ToString();
            string text = textBoxUtility.Text;
            if (text.Length == 0)
                insertTextIndex = 0;
            textBoxUtility.SelectionStart = insertTextIndex;
            if (operatorString.Contains("F_"))
            {
                string realOperatorString = operatorString.Substring(operatorString.IndexOf('_')+1);
                text = text.Insert(insertTextIndex, realOperatorString);
                insertTextIndex += realOperatorString.Length - 1;
            }
            else
            {               
                text = text.Insert(insertTextIndex, operatorString);                
                insertTextIndex += operatorString.Length;                
            }
            textBoxUtility.Text = text;
            textBoxUtility.Select(insertTextIndex, 0);
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (this.dataGridViewLayers.Rows.Count == 0)
            {
                MessageBox.Show("Please select layers for calculation!", VariableMaintainer.CurrentResourceManager.GetString("String2"),
                            MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            string expression = this.textBoxUtility.Text.Trim();
            if (expression.Length == 0)
            {
                MessageBox.Show("Please enter calculation formula!", VariableMaintainer.CurrentResourceManager.GetString("String2"),
                            MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            VariableMaintainer.OptimizationExpression = expression;
            VariableMaintainer.IsACOUtilitySet = true;
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBoxUtility_MouseDown(object sender, MouseEventArgs e)
        {
            insertTextIndex = textBoxUtility.SelectionStart;
        }       
    }
}
