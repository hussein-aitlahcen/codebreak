using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Codebreak.Framework.Generic
{
    /// <summary>
    /// Interface for object that need a clean before being pooled again.
    /// </summary>
    public interface IPoolable
    {
        /// <summary>
        /// Clean the object before he goes back to the pool.
        /// </summary>
        void Cleanup();
    }
}
