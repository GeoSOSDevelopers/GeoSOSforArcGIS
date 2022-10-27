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

        private void buttonOk_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void labelWeb_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.geosimulation.cn/geososforarcgis_usermanual.html");
        }

        private void labelWeb_MouseHover(object sender, EventArgs e)
        {
            Cursor = Cursors.Hand;
        }
    }
}
