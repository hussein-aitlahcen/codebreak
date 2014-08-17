using System.Collections.Generic;
using Codebreak.Framework.Generic;
using Codebreak.Service.World.Database.Repository;
using Codebreak.Service.World.Game.Area;
using Codebreak.Service.World.Game.Database.Repository;
using System.Threading;

namespace Codebreak.Service.World.Manager
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class AreaManager : Singleton<AreaManager>
    {
        private Dictionary<int, SuperAreaInstance> _superAreaById;
        private Dictionary<int, AreaInstance> _areaById;
        private Dictionary<int, SubAreaInstance> _subAreaById;

        /// <summary>
        /// 
        /// </summary>
        public AreaManager()
        {
            _superAreaById = new Dictionary<int, SuperAreaInstance>();
            _areaById = new Dictionary<int, AreaInstance>();
            _subAreaById = new Dictionary<int, SubAreaInstance>();
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
                WorldService.Instance.Dispatcher.AddHandler(instance.Dispatch);

                _superAreaById.Add(superAreaDAO.Id, instance);
            }

            foreach(var areaDAO in AreaRepository.Instance.GetAll())
            {
                var instance =  new AreaInstance(areaDAO);
                instance.SuperArea.AddHandler(instance.Dispatch);

                _areaById.Add(areaDAO.Id, instance);
            }

            foreach(var subAreaDAO in SubAreaRepository.Instance.GetAll())
            {
                var instance = new SubAreaInstance(subAreaDAO);
                instance.Area.AddUpdatable(instance);
                instance.Area.AddHandler(instance.Dispatch);

                _subAreaById.Add(subAreaDAO.Id, instance);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SuperAreaInstance GetSuperArea(int id)
        {
            return _superAreaById[id];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public AreaInstance GetArea(int id)
        {
            return _areaById[id];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SubAreaInstance GetSubArea(int id)
        {
            return _subAreaById[id];
        }

        /// <summary>
        /// 
        /// </summary>
        public void BlockQueues()
        {
            foreach (var area in _areaById.Values)
            {
                area.IOQueue.Blocked = true;

                while (!area.IOQueue.IsPaused)
                    Thread.Sleep(1);
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public void ResumeQueues()
        {
            foreach (var area in _areaById.Values)
            {
                area.IOQueue.Blocked = false;
            }
        }
    }
}
