using Codebreak.Framework.Generic;
using Codebreak.Service.World.Game.Action;
using Codebreak.Service.World.Game.Database.Repository;

namespace Codebreak.Service.World.Manager
{
    public sealed class NpcManager : Singleton<NpcManager>
    {
        public void Initialize()
        {
            long currentId = 1;
            foreach(var npcInstance in NpcInstanceRepository.Instance.GetAll())
            {
                var npc = EntityManager.Instance.CreateNpc(npcInstance, currentId++);
                if(npc.Map != null)
                    npc.StartAction(GameActionTypeEnum.MAP);
            }

            Logger.Info("NpcManager : " + currentId + " NpcInstance loaded.");
        }
    }
}
