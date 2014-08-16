using Codebreak.AuthService.RPC;
using Codebreak.Framework.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.AuthService.Auth.Manager
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class WorldManager : Singleton<WorldManager>
    {
        private Dictionary<int, AuthServiceRPCClient> _worldById;

        public WorldManager()
        {
            _worldById = new Dictionary<int, AuthServiceRPCClient>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="worlId"></param>
        /// <returns></returns>
        public AuthServiceRPCClient GetById(int worldId)
        {
            if (_worldById.ContainsKey(worldId))
                return _worldById[worldId];
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="worldId"></param>
        /// <param name="client"></param>
        public void RegisterWorld(int worldId, AuthServiceRPCClient client)
        {
            AuthService.Instance.AddMessage(() =>
            {
                if (!_worldById.ContainsKey(worldId))
                    _worldById.Add(worldId, client);
            });

            RefreshWorldList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="worldId"></param>
        public void DeleteWorld(int worldId)
        {
            AuthService.Instance.AddMessage(() =>
            {
                if (_worldById.ContainsKey(worldId))
                    _worldById.Remove(worldId);
            });

            RefreshWorldList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        public void SendWorldList(AuthClient client)
        {
            AuthService.Instance.AddMessage(() =>
                {
                    client.Send(AuthMessage.WORLD_HOST_LIST(_worldById.Values));
                });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        public void SendWorldCharacterList(AuthClient client)
        {
            AuthService.Instance.AddMessage(() =>
            {
                client.Send(AuthMessage.WORLD_CHARACTER_LIST(_worldById.Values));
            });
        }

        public void RefreshWorldList()
        {
            AuthService.Instance.AddMessage(() =>
            {
                AuthService.Instance.SendToAll(AuthMessage.WORLD_HOST_LIST(_worldById.Values));
            });
        }
    }
}
