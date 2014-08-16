using Codebreak.Framework.Generic;
using Codebreak.WorldService.RPC;
using Codebreak.WorldService.World.Database;
using Codebreak.WorldService.World.Database.Repository;
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
    public sealed class WorldManager : Singleton<WorldManager>
    {
        /// <summary>
        /// 
        /// </summary>
        public void Initialize()
        {
            WorldDbMgr.Instance.Initialize();            
            AccountManager.Instance.Initialize();
            SpellManager.Instance.Initialize();
            AreaManager.Instance.Initialize();
            MapManager.Instance.Initialize();
            NpcManager.Instance.Initialize();
            RPCManager.Instance.Initialize();

            WorldService.Instance.AddTimer(WorldConfig.WORLD_SAVE_INTERVAL, UpdateWorld);

            WorldService.Instance.Start(WorldConfig.GAME_BIND_IP, WorldConfig.GAME_BIND_PORT);
        }

        private void UpdateWorld()
        {
            WorldService.Instance.AddMessage(() =>
            {
                WorldService.Instance.Dispatcher.Dispatch(WorldMessage.INFORMATION_MESSAGE(InformationTypeEnum.ERROR, InformationEnum.ERROR_WORLD_SAVING));

                WorldService.Instance.AddMessage(() =>
                    {
                        CharacterRepository.Instance.UpdateAll();

                        WorldService.Instance.AddMessage(() =>
                        {
                            WorldService.Instance.Dispatcher.Dispatch(WorldMessage.INFORMATION_MESSAGE(InformationTypeEnum.ERROR, InformationEnum.ERROR_WORLD_SAVING_FINISHED));
                        });
                    });
            });
        }
    }
}
