using Codebreak.Service.World.Database.Structure;
using Codebreak.Service.World.Game.Entity;
using Codebreak.Service.World.Game.Spell;
using Codebreak.Service.World.Game.Stats;
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
    public sealed class AddStatsEffect : ActionEffectBase<AddStatsEffect>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="item"></param>
        /// <param name="effect"></param>
        /// <param name="targetId"></param>
        /// <param name="targetCell"></param>
        /// <returns></returns>
        public override bool ProcessItem(CharacterEntity character, InventoryItemDAO item, GenericStats.VariableEffect effect, long targetId, int targetCell)
        {
            return Process(character, new Dictionary<string, string>() { { "statsId", effect.EffectId.ToString() }, { "value", effect.RandomJet.ToString() } });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public override bool Process(CharacterEntity character, Dictionary<string, string> parameters)
        {
            var addEffect = EffectEnum.None;
            var effectType = (EffectEnum)int.Parse(parameters["statsId"]);
            var value = int.Parse(parameters["value"]);

            switch(effectType)
            {
                case EffectEnum.AddVitality:
                case EffectEnum.AddCaractVitality:
                    addEffect = EffectEnum.AddVitality;
                    character.DatabaseRecord.Vitality += value;
                    break;

                case EffectEnum.AddWisdom:
                case EffectEnum.AddCaractWisdom:
                    addEffect = EffectEnum.AddWisdom;
                    character.DatabaseRecord.Wisdom += value;
                    break;

                case EffectEnum.AddIntelligence:
                case EffectEnum.AddCaractIntelligence:
                    addEffect = EffectEnum.AddIntelligence;
                    character.DatabaseRecord.Intelligence += value;
                    break;

                case  EffectEnum.AddStrength:
                case EffectEnum.AddCaractStrength:
                    addEffect = EffectEnum.AddStrength;
                    character.DatabaseRecord.Strength += value;
                    break;

                case EffectEnum.AddAgility:
                case EffectEnum.AddCaractAgility:
                    addEffect = EffectEnum.AddAgility;
                    character.DatabaseRecord.Agility += value;
                    break;

                case EffectEnum.AddChance:
                case EffectEnum.AddCaractChance:
                    addEffect = EffectEnum.AddChance;
                    character.DatabaseRecord.Chance += value;
                    break;
            }

            character.Statistics.AddBase(addEffect, value);

            character.CachedBuffer = true;
            character.SendAccountStats();
            character.Dispatch(WorldMessage.INFORMATION_MESSAGE(InformationTypeEnum.INFO, InformationEnum.INFO_CARACTERISTIC_UPGRADED, value));
            character.CachedBuffer = false;

            return true;
        }
    }
}
