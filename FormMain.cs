using System;
using System.Windows.Forms;

namespace GameAssetsManager
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
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
    }
}
