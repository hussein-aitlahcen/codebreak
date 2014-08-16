using Codebreak.Framework.Generic;
using Codebreak.WorldService.World.Action;
using Codebreak.WorldService.World.Database.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.WorldService.World.Manager
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
