using System;
using System.IO;

namespace GameAssetsManager
{
    public struct GameEntityData
    {
        public UInt16 Components;
        public UInt16 Properties;
        public byte Sprite;
        public sbyte Anim;
        public byte StatTableEntry;
        public byte PathType;

        public GameEntityData(BinaryReader data)
        {
            Components = data.ReadUInt16();
            Properties = data.ReadUInt16();
            Sprite = data.ReadByte();
            Anim = data.ReadSByte();
            StatTableEntry = data.ReadByte();
            PathType = data.ReadByte();
        }

        public void ToStream(BinaryWriter bw)
        {
            bw.Write(Components);
            bw.Write(Properties);
            bw.Write(Sprite);
            bw.Write(Anim);
            bw.Write(StatTableEntry);
            bw.Write(PathType);
        }
    }
}
