using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GeoSOS.ArcMapAddIn.UI.Forms
{
    public partial class FormSimulationSetting : Form
    {
        public FormSimulationSetting()
        {
            InitializeComponent();
        }

        public TextBox TextBoxSummary
        {
            get
            {
                return this.textBoxSummary;
            }
        }

        public Label LabelTask
        {
            get
            {
                return this.labelTask;
            }
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void Clear()
        {
            this.labelTask.Text = "";
            this.textBoxSummary.Text = "";
        }
    }
}
