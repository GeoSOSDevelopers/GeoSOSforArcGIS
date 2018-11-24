using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Resources;

namespace GeoSOS.ArcMapAddIn.UI.Forms
{
    public partial class FormConfusionMatrix : Form
    {
        private DataTable dtValues;
        private DataTable dtPercents;
        private bool isInValue = false;
        private bool isPercentReady = false;
        private ResourceManager resourceManager = null;

        public DataTable DataTableValues
        {
            set { dtValues = value; }
        }

        public DataTable DataTablePercents
        {
            set { dtPercents = value; }
        }

        public FormConfusionMatrix()
        {
            System.Threading.Thread.CurrentThread.CurrentUICulture = System.Threading.Thread.CurrentThread.CurrentCulture;
            InitializeComponent();
            
            if (System.Globalization.CultureInfo.CurrentCulture.Parent.Name == "zh-CHS")
                resourceManager = new System.Resources.ResourceManager("GeoSOS.ArcMapAddIn.Properties.Resource_zh_CHS", System.Reflection.Assembly.GetExecutingAssembly());
            else if (System.Globalization.CultureInfo.CurrentCulture.Parent.Name == "zh-CHT")
                resourceManager = new System.Resources.ResourceManager("GeoSOS.ArcMapAddIn.Properties.Resource_zh_TW", System.Reflection.Assembly.GetExecutingAssembly());
            else if (System.Globalization.CultureInfo.CurrentCulture.Parent.Name == "en")
                resourceManager = new System.Resources.ResourceManager("GeoSOS.ArcMapAddIn.Properties.Resource_en", System.Reflection.Assembly.GetExecutingAssembly());
        }

        public DataGridView DataGridViewConfusionMatrix
        {
            get { return this.dataGridViewConfusionMatrix; }
        }

        public Label LabelOverallAccuracy
        {
            get { return this.labelAccuracy; }
        }

        public Label LabelKappa
        {
            get { return this.labelKappa; }
        }

        public Label LabelFoM
        {
            get { return this.labelFoM; }
        }

        public Label LabelFoMValues
        {
            get { return this.labelFoMValues; }
        }

        public Label LabelPA
        {
            get { return this.labelPA; }
        }

        public Label LabelUA
        {
            get { return this.labelUA; }
        }

        private void buttonPercent_Click(object sender, EventArgs e)
        {
            if (isInValue)
            {
                dataGridViewConfusionMatrix.DataSource = dtValues;
                dataGridViewConfusionMatrix.Refresh();
                dataGridViewConfusionMatrix.RowHeadersWidth = 90;
                for (int i = 0; i < dtValues.Rows.Count; i++)
                {
                    dataGridViewConfusionMatrix.Rows[i].Height = 30;
                    dataGridViewConfusionMatrix.Rows[i].HeaderCell.Value = dtValues.Columns[i].ColumnName;
                }
                buttonPercent.Text = resourceManager.GetString("String92");
                isInValue = false;
            }
            else
            {
                if (!isPercentReady)
                    FillPercentData();
                dataGridViewConfusionMatrix.DataSource = dtPercents;
                dataGridViewConfusionMatrix.Refresh();
                dataGridViewConfusionMatrix.RowHeadersWidth = 90;
                for (int i = 0; i < dtValues.Rows.Count; i++)
                {
                    dataGridViewConfusionMatrix.Rows[i].Height = 30;
                    dataGridViewConfusionMatrix.Rows[i].HeaderCell.Value = dtValues.Columns[i].ColumnName;
                }
                buttonPercent.Text = resourceManager.GetString("String91");
                isInValue = true;
            }            
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FillPercentData()
        {
            for (int i = 0; i < dtPercents.Rows.Count; i++)
            {
                for (int j = 0; j < dtPercents.Columns.Count; j++)
                {
                    dtPercents.Rows[i][j] = (100 * (Convert.ToDouble(dtPercents.Rows[i][j])
                        / Convert.ToDouble(dtPercents.Rows[i][dtPercents.Columns.Count - 1]))).ToString("0.00") + " %";
                    if (i == dtPercents.Rows.Count - 1)
                    {
                        dtPercents.Rows[i][j] = "100.00 %";
                        if (j == dtPercents.Columns.Count - 1)
                            dtPercents.Rows[i][j] = "";
                    }
                }
            }
            isPercentReady = true;
        }
    }
}
