using System.Collections.Generic;
using Codebreak.Framework.Generic;
using Codebreak.Service.World.Database.Repository;
using Codebreak.Service.World.Game.Area;
using System.Threading;

namespace Codebreak.Service.World.Manager
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class AreaManager : Singleton<AreaManager>
    {
        private Dictionary<int, SuperAreaInstance> m_superAreaById;
        private Dictionary<int, AreaInstance> m_areaById;
        private Dictionary<int, SubAreaInstance> m_subAreaById;

        /// <summary>
        /// 
        /// </summary>
        public AreaManager()
        {
            m_superAreaById = new Dictionary<int, SuperAreaInstance>();
            m_areaById = new Dictionary<int, AreaInstance>();
            m_subAreaById = new Dictionary<int, SubAreaInstance>();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Initialize()
        {
            foreach(var superAreaDAO in SuperAreaRepository.Instance.GetAll())
            {
                var instance = new SuperAreaInstance(superAreaDAO);
                WorldService.Instance.AddUpdatable(instance);
                WorldService.Instance.Dispatcher.SafeAddHandler(instance.Dispatch);

                m_superAreaById.Add(superAreaDAO.Id, instance);
            }

            foreach(var areaDAO in AreaRepository.Instance.GetAll())
            {
                var instance =  new AreaInstance(areaDAO);
                instance.SuperArea.AddUpdatable(instance);
                instance.SuperArea.AddHandler(instance.SafeDispatch);

                m_areaById.Add(areaDAO.Id, instance);
            }

            foreach(var subAreaDAO in SubAreaRepository.Instance.GetAll())
            {
                var instance = new SubAreaInstance(subAreaDAO);
                instance.Area.AddHandler(instance.SafeDispatch);
                m_subAreaById.Add(subAreaDAO.Id, instance);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SuperAreaInstance GetSuperArea(int id)
        {
            return m_superAreaById[id];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public AreaInstance GetArea(int id)
        {
            return m_areaById[id];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SubAreaInstance GetSubArea(int id)
        {
            return m_subAreaById[id];
        }
    }
}
