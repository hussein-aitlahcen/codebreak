using Codebreak.Framework.Generic;
using Codebreak.Service.World.Database.Repository;
using Codebreak.Service.World.Game.Mount;
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
    public sealed class PaddockManager : Singleton<PaddockManager>
    {
        private readonly Dictionary<int, Paddock> m_paddockByMap;

        public PaddockManager()
        {
            m_paddockByMap = new Dictionary<int, Paddock>();
        }

        public void Initialize()
        {
            var count = 0;
            foreach(var paddock in PaddockRepository.Instance.All)
            {
                m_paddockByMap.Add(paddock.MapId, new Paddock(paddock));
                count++;
            }
            Logger.Info("PaddockManager : " + count + " Paddocks loaded.");
        }

        public Paddock GetByMapId(int mapId)
        {
            if (m_paddockByMap.ContainsKey(mapId))
                return m_paddockByMap[mapId];
            return null;
        }
    }
}
