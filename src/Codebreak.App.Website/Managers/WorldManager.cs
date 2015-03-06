using Codebreak.Framework.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Codebreak.App.Website.Managers
{
    public sealed class WorldManager : Singleton<WorldManager>
    {
        public long PlayersConnected;
    }
}