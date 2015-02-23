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
        private Dictionary<EffectEnum, IActionEffect> m_effectById;
        private Dictionary<ItemTypeEnum, List<IActionEffect>> m_effectByType;

        /// <summary>
        /// 
        /// </summary>
        public ActionEffectManager()
        {
            m_effectById = new Dictionary<EffectEnum, IActionEffect>();

            m_effectById.Add(EffectEnum.BddDialogLeave, DialogLeaveEffect.Instance);
            m_effectById.Add(EffectEnum.BddDialogReply, DialogReplyEffect.Instance);
            m_effectById.Add(EffectEnum.BddOpenBank, OpenBankEffect.Instance);
            m_effectById.Add(EffectEnum.BddAddStatistic, AddStatsEffect.Instance);
            m_effectById.Add(EffectEnum.BddAddItem, AddItemEffect.Instance);
            m_effectById.Add(EffectEnum.BddTeleport, TeleportEffect.Instance);
            m_effectById.Add(EffectEnum.BddResetStats, ResetStatsEffect.Instance);
            m_effectById.Add(EffectEnum.BddResetSpells, ResetSpellEffect.Instance);
            m_effectById.Add(EffectEnum.BddAddJob, AddJobEffect.Instance);
            m_effectById.Add(EffectEnum.BddRemoveItem, RemoveItemEffect.Instance);
            m_effectById.Add(EffectEnum.BddCreateGuild, GuildCreationEffect.Instance);

            m_effectById.Add(EffectEnum.AddJob, AddJobEffect.Instance);

            m_effectById.Add(EffectEnum.AlignmentChange, ChangeAlignmentEffect.Instance);

            m_effectById.Add(EffectEnum.TeleportSavedZaap, RecallEffect.Instance);

            m_effectById.Add(EffectEnum.AddLife, AddLifeEffect.Instance);

            m_effectById.Add(EffectEnum.AddKamas, AddKamasEffect.Instance);

            m_effectById.Add(EffectEnum.AddBoost, AddBoostEffect.Instance);

            m_effectById.Add(EffectEnum.AddEnergy, AddEnergyEffect.Instance);
            m_effectById.Add(EffectEnum.AddExperience, AddExperienceEffect.Instance);

            m_effectById.Add(EffectEnum.AddSpell, AddSpellEffect.Instance);
            m_effectById.Add(EffectEnum.AddSpellPoint, AddSpellpointEffect.Instance);

            m_effectById.Add(EffectEnum.AddCaractVitality, AddStatsEffect.Instance);
            m_effectById.Add(EffectEnum.AddCaractWisdom, AddStatsEffect.Instance);
            m_effectById.Add(EffectEnum.AddCaractStrength, AddStatsEffect.Instance);
            m_effectById.Add(EffectEnum.AddCaractIntelligence, AddStatsEffect.Instance);
            m_effectById.Add(EffectEnum.AddCaractAgility, AddStatsEffect.Instance);
            m_effectById.Add(EffectEnum.AddCaractChance, AddStatsEffect.Instance);

            m_effectById.Add(EffectEnum.AddVitality, AddStatsEffect.Instance);
            m_effectById.Add(EffectEnum.AddWisdom, AddStatsEffect.Instance);
            m_effectById.Add(EffectEnum.AddStrength, AddStatsEffect.Instance);
            m_effectById.Add(EffectEnum.AddIntelligence, AddStatsEffect.Instance);
            m_effectById.Add(EffectEnum.AddAgility, AddStatsEffect.Instance);
            m_effectById.Add(EffectEnum.AddChance, AddStatsEffect.Instance);
            
            m_effectByType = new Dictionary<ItemTypeEnum, List<IActionEffect>>();
            m_effectByType.Add(ItemTypeEnum.TYPE_FEE_ARTIFICE, new List<IActionEffect>()
            {
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="?"></param>
        /// <param name="item"></param>
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
            
            bool used = false;

            if (item.StringEffects != string.Empty)
            {
                foreach (var effect in item.Statistics.Effects.Values)
                {
                    if (m_effectById.ContainsKey(effect.EffectType))
                    {
                        used = m_effectById[effect.EffectType].ProcessItem(character, item, effect, targetId, targetCell) || used;
                    }
                }
            }
            else
            {
                if (m_effectByType.ContainsKey((ItemTypeEnum)item.Template.Type))
                {
                    foreach(var effect in m_effectByType[(ItemTypeEnum)item.Template.Type])
                    {
                        used = effect.ProcessItem(character, item, null, targetId, targetCell) || used;
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
        /// <param name="entity"></param>
        /// <param name="effect"></param>
        /// <param name="parameters"></param>
        public void ApplyEffect(CharacterEntity character, EffectEnum effect, Dictionary<string, string> parameters)
        {
            if(m_effectById.ContainsKey(effect))
            {
                m_effectById[effect].Process(character, parameters);
            }
        }
    }
}
