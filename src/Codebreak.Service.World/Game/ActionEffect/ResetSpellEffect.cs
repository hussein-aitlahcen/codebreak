using Codebreak.Service.World.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Game.ActionEffect
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ResetSpellEffect : ActionEffectBase<ResetSpellEffect>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="item"></param>
        /// <param name="effect"></param>
        /// <param name="targetId"></param>
        /// <param name="targetCell"></param>
        /// <returns></returns>
        public override bool ProcessItem(Entity.CharacterEntity character, Database.Structure.InventoryItemDAO item, Stats.GenericEffect effect, long targetId, int targetCell)
        {
            return Process(character, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public override bool Process(Entity.CharacterEntity character, Dictionary<string, string> parameters)
        {
            character.SpellBook.Spells.ForEach(spell => spell.Level = 1);
            character.SpellPoint = character.Level - 1;

            character.CachedBuffer = true;
            character.SendAccountStats();
            character.Dispatch(WorldMessage.SPELLS_LIST(character.SpellBook));
            character.CachedBuffer = false;

            return true;
        }
    }
}
