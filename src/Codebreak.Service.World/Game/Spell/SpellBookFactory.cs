using Codebreak.Framework.Generic;
using Codebreak.Service.World.Game.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Game.Spell
{
    public sealed class SpellBookFactory : Singleton<SpellBookFactory>
    {
        public SpellBook Create(AbstractEntity entity)
        {
            switch (entity.Type)
            {
                case EntityTypeEnum.TYPE_CHARACTER:
                case EntityTypeEnum.TYPE_TAX_COLLECTOR:
                    return new SpellBook((int)entity.Type, entity.Id);

                case EntityTypeEnum.TYPE_MONSTER_FIGHTER:
                    return new SpellBook((int)entity.Type, ((MonsterEntity)entity).Grade.Id);
            }

            return null;
        }
    }
}
