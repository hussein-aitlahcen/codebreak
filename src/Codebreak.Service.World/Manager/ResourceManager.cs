using Codebreak.Framework.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Manager
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ResourceManager : Singleton<ResourceManager>
    {
        public const string RESOURCES_PATH = "resources/";
        public const string SPELLS_BINARY = "spells.bin";
        public const string SPELLS_BINARY_PATH = RESOURCES_PATH + SPELLS_BINARY;
    }
}
