using System;
using System.Windows.Forms;
using GameAssetsManager.Properties;

namespace GameAssetsManager
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
            if (Settings.Default.rootpath.Equals(String.Empty) && folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                Settings.Default.rootpath = folderBrowserDialog1.SelectedPath + "\\";
                Settings.Default.Save();
            }
        }

        private void buttonEntityView_Click(object sender, EventArgs e)
        {
            FormEntityView entityView = new FormEntityView();
            entityView.ShowDialog();
        }

        private void buttonTextView_Click(object sender, EventArgs e)
        {
            FormTextView textView = new FormTextView();
            textView.ShowDialog();
        }

        private void buttonScriptView_Click(object sender, EventArgs e)
        {
            FormScriptView scriptView = new FormScriptView();
            scriptView.ShowDialog();
        }

        private void buttonMapView_Click(object sender, EventArgs e)
        {

        }
    }
}
