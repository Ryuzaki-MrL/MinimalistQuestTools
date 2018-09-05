using System.IO;

namespace GameAssetsManager
{
    class RZDBWriter : BinaryWriter
    {
        public RZDBWriter(Stream output) : base(output)
        {
            base.Write(0x42445a52); // RZDB
        }

        public void WriteSize(int value) => Write7BitEncodedInt(value);
    }

    class RZDBReader : BinaryReader
    {
        public RZDBReader(Stream input) : base(input)
        {
            if (base.ReadInt32() != 0x42445a52)
                throw new InvalidDataException();
        }

        public int ReadSize() => Read7BitEncodedInt();
    }
}
