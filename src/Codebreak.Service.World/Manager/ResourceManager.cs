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
        public const string RESOURCES_PATH = "data/";
        public const string RESOURCES_XML_PATH = RESOURCES_PATH + "xml/";
        public const string SPELLS_XML_PATH = RESOURCES_XML_PATH + "spells.xml";
    }
}
