using Codebreak.Service.World.Database.Structure;
using Codebreak.Service.World.Game.Entity;
using Codebreak.Service.World.Game.Stats;
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
    public sealed class AddSpellpointEffect : ActionEffectBase<AddSpellpointEffect>
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
        public override bool ProcessItem(CharacterEntity character, InventoryItemDAO item, GenericStats.VariableEffect effect, long targetId, int targetCell)
        {
            return Process(character, new Dictionary<string, string>() { { "value", effect.RandomJet.ToString() } });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public override bool Process(CharacterEntity character, Dictionary<string, string> parameters)
        {
            var value = int.Parse(parameters["value"]);

            character.CachedBuffer = true;
            character.SpellPoint += value;
            character.SendAccountStats();
            character.CachedBuffer = false;

            return true;
        }
    }
}
