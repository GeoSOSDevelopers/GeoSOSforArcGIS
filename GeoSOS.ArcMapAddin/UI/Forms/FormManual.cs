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
    public partial class FormManual : Form
    {
        public FormManual()
        {
            InitializeComponent();
        }

        private void linkLabelhelp_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.geosimulation.cn/geososforarcgis_usermanual.html");
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
