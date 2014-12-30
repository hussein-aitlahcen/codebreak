using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Codebreak.Framework.IO
{
    /// <summary>
    /// 
    /// </summary>
    public class BinaryQueue : Queue<byte>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int ReadInt()
        {
            return BitConverter.ToInt32(ReadBytes(4), 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public long ReadLong()
        {
            return BitConverter.ToInt64(ReadBytes(8), 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int ReadShort()
        {
            return BitConverter.ToInt16(ReadBytes(2), 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string ReadString()
        {
            var length = ReadInt();
            var data = ReadBytes(length);
            return Encoding.Default.GetString(data, 0, length);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void WriteInt(int value)
        {
            WriteBytes(BitConverter.GetBytes(value));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void WriteLong(long value)
        {
            WriteBytes(BitConverter.GetBytes(value));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void WriteShort(short value)
        {
            WriteBytes(BitConverter.GetBytes(value));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void WriteString(string value)
        {
            WriteInt(value.Length);
            WriteBytes(Encoding.Default.GetBytes(value));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public void WriteBytes(byte[] data)
        {
            for (int i = 0; i < data.Length; i++)
                WriteByte(data[i]);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public void WriteByte(byte data)
        {
            base.Enqueue(data);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public byte[] ReadBytes(int length)
        {
            if (base.Count < length)
                throw new OutOfMemoryException("BinaryStack::ReadBytes end of stream.");

            byte[] data = new byte[length];
            for (int i = 0; i < length; i++)
                data[i] = ReadByte();

            return data;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public byte ReadByte()
        {
            if (base.Count == 0)
                throw new OutOfMemoryException("BinaryStack::ReadBytes end of stream.");

            return base.Dequeue();
        }
    }
}
