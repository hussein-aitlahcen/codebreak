using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Codebreak.Framework.Network
{
    /// <summary>
    /// Interface that represent an object that can handle a buffer
    /// </summary>
    public interface IBufferHandler
    {
        /// <summary>
        /// 
        /// </summary>
        int Offset
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        void SetBuffer(byte[] buffer, int offset, int count);
    }
}
