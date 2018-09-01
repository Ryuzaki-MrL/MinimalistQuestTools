using System;
using System.Windows.Forms;

namespace GameAssetsManager
{
    public partial class InputBox : Form
    {
        public InputBox(string caption, string def = "")
        {
            InitializeComponent();
            Text = caption;
            textBox1.Text = def;
        }

        public string GetResult()
        {
            return textBox1.Text;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
