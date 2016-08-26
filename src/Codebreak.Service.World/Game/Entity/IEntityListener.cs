using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Game.Entity
{
    public enum EntityEventType
    {
        FIGHT_KILL,
    }

    public interface IEntityListener
    {
        void OnEntityEvent(EntityEventType ev, AbstractEntity entity, object parameters);
    }
}
