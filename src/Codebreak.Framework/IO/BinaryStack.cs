using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Codebreak.Framework.IO
{
    public class BinaryQueue : Queue<byte>
    {
        public int ReadInt()
        {
            return BitConverter.ToInt32(ReadBytes(4), 0);
        }

        public long ReadLong()
        {
            return BitConverter.ToInt64(ReadBytes(8), 0);
        }

        public int ReadShort()
        {
            return BitConverter.ToInt16(ReadBytes(2), 0);
        }

        public string ReadString()
        {
            var length = ReadInt();
            var data = ReadBytes(length);
            return Encoding.Default.GetString(data, 0, length);
        }

        public void WriteInt(int value)
        {
            WriteBytes(BitConverter.GetBytes(value));
        }

        public void WriteLong(long value)
        {
            WriteBytes(BitConverter.GetBytes(value));
        }

        public void WriteShort(short value)
        {
            WriteBytes(BitConverter.GetBytes(value));
        }

        public void WriteString(string value)
        {
            WriteInt(value.Length);
            WriteBytes(Encoding.Default.GetBytes(value));
        }

        public void WriteBytes(byte[] data)
        {
            for (int i = 0; i < data.Length; i++)
                WriteByte(data[i]);
        }

        public void WriteByte(byte data)
        {
            base.Enqueue(data);
        }

        public byte[] ReadBytes(int length)
        {
            if (base.Count < length)
                throw new OutOfMemoryException("BinaryStack::ReadBytes end of stream.");

            byte[] data = new byte[length];
            for (int i = 0; i < length; i++)
                data[i] = ReadByte();

            return data;
        }

        public byte ReadByte()
        {
            if (base.Count == 0)
                throw new OutOfMemoryException("BinaryStack::ReadBytes end of stream.");

            return base.Dequeue();
        }
    }
}
