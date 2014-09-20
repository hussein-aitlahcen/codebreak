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
        void SetBuffer(byte[] buffer, int offset, int count);
        int Offset
        {
            get;
        }
    }
}
