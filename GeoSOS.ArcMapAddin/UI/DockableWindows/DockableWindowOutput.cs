using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace GeoSOS.ArcMapAddIn
{
    /// <summary>
    /// Designer class of the dockable window add-in. It contains user interfaces that
    /// make up the dockable window.
    /// </summary>
    public partial class DockableWindowOutput : UserControl
    {
        private StringBuilder stringBuilder;

        public StringBuilder StringBuilder
        {
            get
            { return stringBuilder; }
        }

        public DockableWindowOutput(object hook)
        {
            System.Threading.Thread.CurrentThread.CurrentUICulture = System.Threading.Thread.CurrentThread.CurrentCulture;
            InitializeComponent();
            this.Hook = hook;
            stringBuilder = new StringBuilder();
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
            private DockableWindowOutput m_windowUI;

            public DockableWindowOutput DockableWindowOutput
            {
                get { return m_windowUI; }
            }

            public AddinImpl()
            {
            }

            protected override IntPtr OnCreateChild()
            {
                m_windowUI = new DockableWindowOutput(this.Hook);
                return m_windowUI.Handle;
            }

            protected override void Dispose(bool disposing)
            {
                if (m_windowUI != null)
                    m_windowUI.Dispose(disposing);

                base.Dispose(disposing);
            }

        }

        public void AppendText(string text)
        {
            if (textBox.InvokeRequired)
            {
                Action<string> actionDelegate = delegate(string txt) { this.textBox.AppendText(txt); };
                this.textBox.Invoke(actionDelegate, text);
            }
            else
                this.textBox.AppendText(text);
        }

        public void ScrollTextbox()
        {
            if (textBox.InvokeRequired)
            {
                Action<int> actionDelegate = delegate(int i)
                {
                    textBox.SelectionStart = this.textBox.Text.Length;
                    textBox.ScrollToCaret();
                };
                this.textBox.Invoke(actionDelegate, 0);
            }
            else
            {
                textBox.SelectionStart = this.textBox.Text.Length;
                textBox.ScrollToCaret();
            }
        }

        private void toolStripButtonSave_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "文本文件(*.txt)|*.txt";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    TextWriter tw = new StreamWriter(saveFileDialog.FileName);
                    tw.Write(this.textBox.Text);
                    tw.Close();
                    MessageBox.Show("已完全保存文件。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
            }
        }

        private void toolStripButtonClear_Click(object sender, EventArgs e)
        {
            this.textBox.Clear();
        }
    }
}
