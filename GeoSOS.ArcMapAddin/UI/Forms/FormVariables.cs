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
    public partial class FormVariables : Form
    {
        public FormVariables()
        {
            InitializeComponent();
        }

        public ListView ListViewControl
        {
            get { return this.listView; }
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }
    }
}
