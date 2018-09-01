using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

namespace GameAssetsManager
{
    public partial class FormScriptView : Form
    {
        private Dictionary<string, string> scripts = new Dictionary<string, string>();
        private BindingList<string> scrnames = new BindingList<string>();
        private string fname = "";

        public FormScriptView()
        {
            InitializeComponent();
            listBoxScr.DataSource = scrnames;
        }

        private void listBoxScr_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxScr.SelectedIndex >= 0)
                textBoxScr.Text = scripts[listBoxScr.SelectedItem.ToString()];
        }

        private void insertToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InputBox input = new InputBox("Add Script");
            if (input.ShowDialog() == DialogResult.OK)
            {
                scripts[input.GetResult()] = "";
                scrnames.Add(input.GetResult());
            }
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listBoxScr.SelectedIndex >= 0)
                scrnames.RemoveAt(listBoxScr.SelectedIndex);
        }

        private void textBoxScr_TextChanged(object sender, EventArgs e)
        {
            if (listBoxScr.SelectedIndex >= 0)
                scripts[listBoxScr.SelectedItem.ToString()] = textBoxScr.Text;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                fname = openFileDialog1.FileName;
                scrnames.Clear();
                FileStream fs = File.OpenRead(openFileDialog1.FileName);
                BinaryReader br = new BinaryReader(fs);
                byte scrCount = br.ReadByte();
                for (int i = 0; i < scrCount; ++i)
                    scrnames.Add(br.ReadString());
                for (int i = 0; i < scrCount; ++i)
                    scripts[scrnames[i]] = br.ReadString();
                br.Close();
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.FileName = fname;
            if (!saveFileDialog1.FileName.Equals(String.Empty) || saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                fname = saveFileDialog1.FileName;
                FileStream fs = File.OpenWrite(saveFileDialog1.FileName);
                BinaryWriter bw = new BinaryWriter(fs);
                bw.Write((Byte)scrnames.Count);
                foreach (string s in scrnames) bw.Write(s);
                foreach (string s in scrnames) bw.Write(scripts[s]);
                bw.Close();
                MessageBox.Show("OK");
            }
        }

        private void renameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InputBox input = new InputBox("Rename Script", scrnames[listBoxScr.SelectedIndex]);
            if (input.ShowDialog() == DialogResult.OK)
            {
                scripts[input.GetResult()] = scripts[scrnames[listBoxScr.SelectedIndex]];
                scrnames[listBoxScr.SelectedIndex] = input.GetResult();
            }
        }
    }
}
