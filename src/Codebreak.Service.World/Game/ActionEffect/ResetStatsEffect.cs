using Codebreak.Service.World.Game.Spell;
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
    public sealed class ResetStatsEffect : ActionEffectBase<ResetStatsEffect>
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
        public override bool ProcessItem(Entity.CharacterEntity character, Database.Structure.ItemDAO item, Stats.GenericEffect effect, long targetId, int targetCell)
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
            character.CachedBuffer = true;
            character.CaractPoint = (character.Level - 1) * 5;
            character.Statistics.AddBase(EffectEnum.AddVitality, -character.DatabaseRecord.Vitality);
            character.Statistics.AddBase(EffectEnum.AddWisdom, -character.DatabaseRecord.Wisdom);
            character.Statistics.AddBase(EffectEnum.AddIntelligence, -character.DatabaseRecord.Intelligence);
            character.Statistics.AddBase(EffectEnum.AddStrength, -character.DatabaseRecord.Strength);
            character.Statistics.AddBase(EffectEnum.AddAgility, -character.DatabaseRecord.Agility);
            character.Statistics.AddBase(EffectEnum.AddChance, -character.DatabaseRecord.Chance);
            character.DatabaseRecord.Vitality = 0;
            character.DatabaseRecord.Wisdom = 0;
            character.DatabaseRecord.Intelligence = 0;
            character.DatabaseRecord.Strength = 0;
            character.DatabaseRecord.Agility = 0;
            character.DatabaseRecord.Chance = 0;
            character.SendAccountStats();
            character.CachedBuffer = false;

            return true;
        }
    }
}
