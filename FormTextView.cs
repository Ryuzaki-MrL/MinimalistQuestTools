using System;
using System.Windows.Forms;
using GameAssetsManager.Properties;

namespace GameAssetsManager
{
    public partial class FormTextView : Form
    {
        private TextContainer txt = new TextContainer();

        public FormTextView()
        {
            InitializeComponent();
            comboBoxLang.DataSource = txt.langs;
            listBoxMsg.DataSource = txt.entries;
            if (!Settings.Default.rootpath.Equals(String.Empty))
                txt.Load(textBoxFile.Text = Settings.Default.rootpath + "messages.rzdb");
        }

        private void LoadTextLang()
        {
            if (listBoxMsg.SelectedIndex >= 0 && comboBoxLang.SelectedIndex >= 0)
                textBoxMsg.Text = txt.messages[comboBoxLang.SelectedItem.ToString()][listBoxMsg.SelectedItem.ToString()];
        }

        private void textBoxMsg_TextChanged(object sender, EventArgs e)
        {
            try
            {
                textBox1.Text = textBox2.Text = txt.messages[comboBoxLang.SelectedItem.ToString()][listBoxMsg.SelectedItem.ToString()] =
                    System.Text.RegularExpressions.Regex.Unescape(textBoxMsg.Text);
            } catch(Exception) { }
        }

        private void comboBoxLang_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadTextLang();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listBoxMsg.SelectedIndex >= 0)
                txt.entries.RemoveAt(listBoxMsg.SelectedIndex);
        }

        private void textBoxEntry_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                txt.AddMessageEntry(textBoxEntry.Text);
                textBoxEntry.Text = "";
            }
        }

        private void buttonAddLang_Click(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                txt.AddLanguage(textBoxLang.Text);
                textBoxLang.Text = "";
            }
        }

        private void removeToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (comboBoxLang.SelectedIndex >= 0)
            {
                txt.messages.Remove(comboBoxLang.SelectedItem.ToString());
                txt.langs.RemoveAt(comboBoxLang.SelectedIndex);
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.FileName = textBoxFile.Text;
            if (!saveFileDialog1.FileName.Equals(String.Empty) || saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBoxFile.Text = saveFileDialog1.FileName;
                txt.Save(saveFileDialog1.FileName);
                MessageBox.Show("OK");
            }
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
                txt.Load(textBoxFile.Text = openFileDialog1.FileName);
        }

        private void insertToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InputBox input = new InputBox("Insert Entry");
            if (input.ShowDialog() == DialogResult.OK)
                txt.AddMessageEntry(input.GetResult(), listBoxMsg.SelectedIndex);
        }

        private void renameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InputBox input = new InputBox("Rename Entry", txt.entries[listBoxMsg.SelectedIndex]);
            if (input.ShowDialog() == DialogResult.OK)
                txt.SetEntry(listBoxMsg.SelectedIndex, input.GetResult());
        }

        private void insertToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            InputBox input = new InputBox("Insert Language");
            if (input.ShowDialog() == DialogResult.OK)
                txt.AddLanguage(input.GetResult(), comboBoxLang.SelectedIndex);
        }

        private void renameToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            InputBox input = new InputBox("Rename Language", txt.langs[comboBoxLang.SelectedIndex]);
            if (input.ShowDialog() == DialogResult.OK)
                txt.SetLanguage(comboBoxLang.SelectedIndex, input.GetResult());
        }
    }
}
