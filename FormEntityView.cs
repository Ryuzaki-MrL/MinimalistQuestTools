using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;

namespace GameAssetsManager
{
    public partial class FormEntityView : Form
    {
        private GameEntityData[] entityData;
        private BaseStatData[] statData;
        private int sel;
        private Byte statTableSize, entityCount;

        public FormEntityView()
        {
            InitializeComponent();
            entityData = new GameEntityData[256];
            statData = new BaseStatData[256];
            for (int i = 0; i < 256; ++i) statData[i] = new BaseStatData();
            comboBoxObj.DataSource = Properties.Resources.EntityNames.Split('\n');
        }

        private void SetSelected(int idx)
        {
            this.sel = idx;
            comboBoxSpr.SelectedIndex = entityData[idx].Sprite;
            numericBoxAnim.Value = entityData[idx].Anim;
            UInt16 comps = entityData[idx].Components;
            UInt16 props = entityData[idx].Properties;
            for (int i = 0; i < checkedListBoxComps.Items.Count; ++i)
                checkedListBoxComps.SetItemChecked(i, ((comps >> i) & 1) == 1);
            for (int i = 0; i < checkedListBoxProps.Items.Count; ++i)
                checkedListBoxProps.SetItemChecked(i, ((props >> i) & 1) == 1);
            comboBoxPath.SelectedIndex = entityData[idx].PathType;
            numericStats.Value = entityData[idx].StatTableEntry;
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBoxFile.Text = openFileDialog1.FileName;
                FileStream fs = File.OpenRead(openFileDialog1.FileName);
                BinaryReader br = new BinaryReader(fs);
                br.ReadInt32();
                entityCount = br.ReadByte();
                for (int i = 0; i < entityCount; ++i)
                    entityData[i] = new GameEntityData(br);
                statTableSize = br.ReadByte();
                for (int i = 0; i < statTableSize; ++i)
                    statData[i] = new BaseStatData(br);
                br.Close();
                comboBoxObj.SelectedIndex = 0;
            }
        }

        private void comboBoxSpr_SelectedIndexChanged(object sender, EventArgs e)
        {
            pictureBoxSprite.Image = (Bitmap)Properties.Resources.ResourceManager.GetObject(comboBoxSpr.SelectedItem.ToString());
            entityData[sel].Sprite = (Byte)comboBoxSpr.SelectedIndex;
        }

        private void numericBoxAnim_ValueChanged(object sender, EventArgs e)
        {
            entityData[sel].Anim = (SByte)numericBoxAnim.Value;
        }

        private void checkedListBoxComps_SelectedIndexChanged(object sender, EventArgs e)
        {
            entityData[sel].Components = 0;
            foreach (int idx in checkedListBoxComps.CheckedIndices)
                entityData[sel].Components |= (UInt16)(1 << idx);
        }

        private void checkedListBoxProps_SelectedIndexChanged(object sender, EventArgs e)
        {
            entityData[sel].Properties = 0;
            foreach (int idx in checkedListBoxProps.CheckedIndices)
                entityData[sel].Properties |= (UInt16)(1 << idx);
        }

        private void comboBoxObj_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetSelected(comboBoxObj.SelectedIndex);
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.FileName = textBoxFile.Text;
            if (!saveFileDialog1.FileName.Equals(String.Empty) || saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBoxFile.Text = saveFileDialog1.FileName;
                FileStream fs = File.OpenWrite(saveFileDialog1.FileName);
                BinaryWriter bw = new BinaryWriter(fs);
                bw.Write(0x42445a52); // RZDB
                bw.Write((Byte)comboBoxObj.Items.Count);
                for (int i = 0; i < comboBoxObj.Items.Count; ++i)
                    entityData[i].ToStream(bw);
                bw.Write(statTableSize);
                for (int i = 0; i < statTableSize; ++i)
                    statData[i].ToStream(bw);
                bw.Close();
                MessageBox.Show("OK");
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void numericStats_ValueChanged(object sender, EventArgs e)
        {
            entityData[sel].StatTableEntry = (Byte)numericStats.Value;
            if (numericStats.Value > statTableSize) statTableSize = (Byte)numericStats.Value;
            BaseExp.Value = statData[(int)numericStats.Value].BaseExp;
            BaseAtk.Value = statData[(int)numericStats.Value].BaseAtk;
            BaseDef.Value = statData[(int)numericStats.Value].BaseDef;
            BaseSpd.Value = statData[(int)numericStats.Value].BaseSpd;
            BaseHP.Value = statData[(int)numericStats.Value].BaseHP;
            BaseRad.Value = statData[(int)numericStats.Value].BaseRad;
            BaseImm.Value = statData[(int)numericStats.Value].BaseImm;
            GrowAtk.Value = statData[(int)numericStats.Value].GrowAtk;
            GrowDef.Value = statData[(int)numericStats.Value].GrowDef;
            GrowSpd.Value = statData[(int)numericStats.Value].GrowSpd;
            GrowRad.Value = statData[(int)numericStats.Value].GrowRad;
            GrowHP.Value = statData[(int)numericStats.Value].GrowHP;
        }

        private void comboBoxPath_SelectedIndexChanged(object sender, EventArgs e)
        {
            entityData[sel].PathType = (Byte)comboBoxPath.SelectedIndex;
        }

        private void FormEntityView_FormClosing(object sender, FormClosingEventArgs e)
        {
            //DialogResult result = MessageBox.Show("Save changes?", "GameAssetsManager", MessageBoxButtons.YesNo);
            //if (result == DialogResult.Yes)
            //    saveToolStripMenuItem_Click(saveToolStripMenuItem, EventArgs.Empty);
        }

        private void editBaseStatTable(object sender, EventArgs e)
        {
            statData[(int)numericStats.Value][(sender as NumericUpDown).Name] = (Byte)((sender as NumericUpDown).Value);
        }
    }
}
