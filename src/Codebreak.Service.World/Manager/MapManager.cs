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
        
        /// <summary>
        /// 
        /// </summary>
        public MapManager()
        {
            m_mapById = new Dictionary<int, MapInstance>();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Initialize()
        {
            foreach(var mapDAO in MapRepository.Instance.GetAll())
            {
                var instance = new MapInstance(mapDAO.SubAreaId, mapDAO.Id, mapDAO.X, mapDAO.Y, mapDAO.Width, mapDAO.Height, mapDAO.Data, mapDAO.DataKey, mapDAO.CreateTime, mapDAO.FightTeam0Cells, mapDAO.FightTeam1Cells);

                instance.SubArea.AddUpdatable(instance);
                instance.SubArea.SafeAddHandler(instance.Dispatch);

                m_mapById.Add(instance.Id, instance);
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
                return m_mapById[id];
            return null;
        }
    }
}
