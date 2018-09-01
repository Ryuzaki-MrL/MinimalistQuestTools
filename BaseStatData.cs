using System.IO;

namespace GameAssetsManager
{
    public sealed class BaseStatData
    {
        public byte BaseExp { get; set; }
        public byte BaseAtk { get; set; }
        public byte BaseDef { get; set; }
        public byte BaseSpd { get; set; }
        public byte BaseHP  { get; set; }
        public byte BaseRad { get; set; }
        public byte BaseImm { get; set; }
        public byte GrowAtk { get; set; }
        public byte GrowDef { get; set; }
        public byte GrowSpd { get; set; }
        public byte GrowHP  { get; set; }
        public byte GrowRad { get; set; }

        public BaseStatData() { }

        public BaseStatData(BinaryReader data)
        {
            BaseExp = data.ReadByte();
            BaseAtk = data.ReadByte();
            BaseDef = data.ReadByte();
            BaseSpd = data.ReadByte();
            BaseHP = data.ReadByte();
            BaseRad = data.ReadByte();
            BaseImm = data.ReadByte();
            GrowAtk = data.ReadByte();
            GrowDef = data.ReadByte();
            GrowSpd = data.ReadByte();
            GrowHP = data.ReadByte();
            GrowRad = data.ReadByte();
        }

        public void ToStream(BinaryWriter bw)
        {
            bw.Write(BaseExp);
            bw.Write(BaseAtk);
            bw.Write(BaseDef);
            bw.Write(BaseSpd);
            bw.Write(BaseHP);
            bw.Write(BaseRad);
            bw.Write(BaseImm);
            bw.Write(GrowAtk);
            bw.Write(GrowDef);
            bw.Write(GrowSpd);
            bw.Write(GrowHP);
            bw.Write(GrowRad);
        }

        public object this[string propertyName]
        {
            get { return this.GetType().GetProperty(propertyName).GetValue(this, null); }
            set { this.GetType().GetProperty(propertyName).SetValue(this, value, null); }
        }
    }
}
