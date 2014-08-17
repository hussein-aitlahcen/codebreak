using Codebreak.Framework.Generic;
using Codebreak.WorldService.World.Database.Repository;
using Codebreak.WorldService.World.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.WorldService.World.Manager
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class MapManager : Singleton<MapManager>
    {
        private Dictionary<int, MapInstance> _mapById;
        
        /// <summary>
        /// 
        /// </summary>
        public MapManager()
        {
            _mapById = new Dictionary<int, MapInstance>();
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
                instance.SubArea.AddHandler(instance.Dispatch);

                _mapById.Add(instance.Id, instance);
            }

            Logger.Info("MapManager : " + _mapById.Count + " MapInstance loaded.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public MapInstance GetById(int id)
        {
            if (_mapById.ContainsKey(id))
                return _mapById[id];
            return null;
        }
    }
}
