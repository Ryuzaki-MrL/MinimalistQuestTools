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
        private int statTableSize, entityCount;

        public static string[] GetEntityNames()
        {
            return Properties.Resources.EntityNames.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
        }

        public static string[] GetSpriteNames()
        {
            return Properties.Resources.SpriteNames.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
        }

        public FormEntityView()
        {
            InitializeComponent();
            entityData = new GameEntityData[256];
            statData = new BaseStatData[256];
            for (int i = 0; i < 256; ++i) statData[i] = new BaseStatData();
            comboBoxObj.DataSource = GetEntityNames();
            comboBoxSpr.DataSource = GetSpriteNames();
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
                RZDBReader br = new RZDBReader(File.OpenRead(openFileDialog1.FileName));
                entityCount = br.ReadSize();
                for (int i = 0; i < entityCount; ++i)
                    entityData[i] = new GameEntityData(br);
                statTableSize = br.ReadSize();
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
                RZDBWriter bw = new RZDBWriter(File.Open(saveFileDialog1.FileName, FileMode.Truncate));
                bw.WriteSize(comboBoxObj.Items.Count);
                for (int i = 0; i < comboBoxObj.Items.Count; ++i)
                    entityData[i].ToStream(bw);
                bw.WriteSize(statTableSize);
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
            int idx = (int)numericStats.Value;
            if (numericStats.Value > statTableSize) statTableSize = idx;
            BaseExp.Value = statData[idx].BaseExp;
            BaseAtk.Value = statData[idx].BaseAtk;
            BaseDef.Value = statData[idx].BaseDef;
            BaseSpd.Value = statData[idx].BaseSpd;
            BaseHP.Value  = statData[idx].BaseHP;
            BaseRad.Value = statData[idx].BaseRad;
            BaseImm.Value = statData[idx].BaseImm;
            GrowAtk.Value = statData[idx].GrowAtk;
            GrowDef.Value = statData[idx].GrowDef;
            GrowSpd.Value = statData[idx].GrowSpd;
            GrowRad.Value = statData[idx].GrowRad;
            GrowHP.Value  = statData[idx].GrowHP;
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
