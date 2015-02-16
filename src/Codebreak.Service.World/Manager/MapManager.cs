using System.Linq;
using System.Collections.Generic;
using Codebreak.Framework.Generic;
using Codebreak.Service.World.Database.Repository;
using Codebreak.Service.World.Game.Map;

namespace Codebreak.Service.World.Manager
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class MapManager : Singleton<MapManager>
    {
        /// <summary>
        /// 
        /// </summary>
        private Dictionary<int, MapInstance> m_mapById;
        private Dictionary<int, ObjectPool<MapInstance>> m_multyInstanceById;
        
        /// <summary>
        /// 
        /// </summary>
        public MapManager()
        {
            m_mapById = new Dictionary<int, MapInstance>();
            m_multyInstanceById = new Dictionary<int, ObjectPool<MapInstance>>();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Initialize()
        {
            foreach (var mapDAO in MapTemplateRepository.Instance.All)
            {
                var map = new MapInstance(mapDAO.SubAreaId, mapDAO.Id, mapDAO.X, mapDAO.Y, mapDAO.Width, mapDAO.Height, mapDAO.Data, mapDAO.DataKey, mapDAO.CreateTime, mapDAO.FightTeam0Cells, mapDAO.FightTeam1Cells);
                m_mapById.Add(map.Id, map);
                if(WorldConfig.MULTIPLE_INSTANCE_MAP_ID.Contains(map.Id))
                    m_multyInstanceById.Add(mapDAO.Id, new ObjectPool<MapInstance>(map.Clone));
            }
            Logger.Info("MapManager : " + m_mapById.Count + " MapInstance loaded.");
        }
        


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public MapInstance GetById(int id)
        {
            if (m_mapById.ContainsKey(id))
                if (WorldConfig.MULTIPLE_INSTANCE_MAP_ID.Contains(id))
                    return m_multyInstanceById[id].Pop();
                else
                    return m_mapById[id];
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instance"></param>
        public void ReleaseInstance(MapInstance instance)
        {
            m_multyInstanceById[instance.Id].Push(instance);
        }
    }
}
