using Codebreak.Framework.Generic;
using Codebreak.Service.World.Database.Structure;
using Codebreak.Service.World.Game.ActionEffect;
using Codebreak.Service.World.Game.Entity;
using Codebreak.Service.World.Game.Spell;
using Codebreak.Service.World.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Manager
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ActionEffectManager : Singleton<ActionEffectManager>
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly Dictionary<EffectEnum, IActionEffect> m_effectById;
        private readonly Dictionary<ItemTypeEnum, List<IActionEffect>> m_effectByType;

        /// <summary>
        /// 
        /// </summary>
        public ActionEffectManager()
        {
            m_effectById = new Dictionary<EffectEnum, IActionEffect>
            {
                {EffectEnum.BddDialogLeave, DialogLeaveEffect.Instance},
                {EffectEnum.BddDialogReply, DialogReplyEffect.Instance},
                {EffectEnum.BddOpenBank, OpenBankEffect.Instance},
                {EffectEnum.BddAddStatistic, AddStatsEffect.Instance},
                {EffectEnum.BddAddItem, AddItemEffect.Instance},
                {EffectEnum.BddTeleport, TeleportEffect.Instance},
                {EffectEnum.BddResetStats, ResetStatsEffect.Instance},
                {EffectEnum.BddResetSpells, ResetSpellEffect.Instance},
                {EffectEnum.BddAddJob, AddJobEffect.Instance},
                {EffectEnum.BddRemoveItem, RemoveItemEffect.Instance},
                {EffectEnum.BddCreateGuild, GuildCreationEffect.Instance},
                {EffectEnum.BddLaunchFight, StartFightEffect.Instance},
                {EffectEnum.LaunchFight, StartFightEffect.Instance},
                {EffectEnum.AddJob, AddJobEffect.Instance},
                {EffectEnum.AlignmentChange, ChangeAlignmentEffect.Instance},
                {EffectEnum.TeleportSavedZaap, RecallEffect.Instance},
                {EffectEnum.AddLife, AddLifeEffect.Instance},
                {EffectEnum.AddKamas, AddKamasEffect.Instance},
                {EffectEnum.AddBoost, AddBoostEffect.Instance},
                {EffectEnum.AddEnergy, AddEnergyEffect.Instance},
                {EffectEnum.AddExperience, AddExperienceEffect.Instance},
                {EffectEnum.AddSpell, AddSpellEffect.Instance},
                {EffectEnum.AddSpellPoint, AddSpellpointEffect.Instance},
                {EffectEnum.AddCaractVitality, AddStatsEffect.Instance},
                {EffectEnum.AddCaractWisdom, AddStatsEffect.Instance},
                {EffectEnum.AddCaractStrength, AddStatsEffect.Instance},
                {EffectEnum.AddCaractIntelligence, AddStatsEffect.Instance},
                {EffectEnum.AddCaractAgility, AddStatsEffect.Instance},
                {EffectEnum.AddCaractChance, AddStatsEffect.Instance},
                {EffectEnum.AddVitality, AddStatsEffect.Instance},
                {EffectEnum.AddWisdom, AddStatsEffect.Instance},
                {EffectEnum.AddStrength, AddStatsEffect.Instance},
                {EffectEnum.AddIntelligence, AddStatsEffect.Instance},
                {EffectEnum.AddAgility, AddStatsEffect.Instance},
                {EffectEnum.AddChance, AddStatsEffect.Instance}
            };
            m_effectByType = new Dictionary<ItemTypeEnum, List<IActionEffect>>
            {
                {
                    ItemTypeEnum.TYPE_FEE_ARTIFICE, new List<IActionEffect>()
                    {
                    }
                }
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="itemId"></param>
        /// <param name="targetId"></param>
        /// <param name="targetCell"></param>
        /// <param name="parameters"></param>
        public void ApplyEffects(CharacterEntity character, long itemId, long targetId = -1, int targetCell = -1, Dictionary<string, string> parameters = null)
        {
            var item = character.Inventory.GetItem(itemId);
            if(item == null)
                return;

            if (!item.Template.Usable && !item.Template.Buff && !item.Template.Targetable && targetId != -1 && targetCell != -1)
            {
                Logger.Info("ActionEffectManager::Apply non usable/buff/targetable item=" + item.Template.Name + " character=" + character.Name);                
                character.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }
            
            if(!item.SatisfyConditions(character))
            {
                character.Dispatch(WorldMessage.INFORMATION_MESSAGE(InformationTypeEnum.ERROR, InformationEnum.ERROR_CONDITIONS_UNSATISFIED));
                return;
            }
            
            var used = false;
            if (item.StringEffects != string.Empty)
            {
                foreach (var effect in item.Statistics.Effects.Values)
                {
                    if (m_effectById.ContainsKey(effect.EffectType))
                    {
                        used = m_effectById[effect.EffectType].ProcessItem(character, item, effect, targetId, targetCell) 
                            || used;
                    }
                }
            }
            else
            {
                if (m_effectByType.ContainsKey((ItemTypeEnum)item.Template.Type))
                {
                    foreach(var effect in m_effectByType[(ItemTypeEnum)item.Template.Type])
                    {
                        used = effect.ProcessItem(character, item, null, targetId, targetCell) 
                            || used;
                    }
                }
            }

            if (used)
                character.Inventory.RemoveItem(itemId);
            else
                character.Dispatch(WorldMessage.BASIC_NO_OPERATION());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="effect"></param>
        /// <param name="parameters"></param>
        public void ApplyEffect(CharacterEntity character, EffectEnum effect, Dictionary<string, string> parameters)
        {
            if(m_effectById.ContainsKey(effect))
                m_effectById[effect].Process(character, parameters);
        }
    }
}
