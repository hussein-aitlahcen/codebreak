using Codebreak.Framework.Generic;
using Codebreak.Service.World.Database.Structure;
using Codebreak.Service.World.Game.ActionEffect;
using Codebreak.Service.World.Game.Entity;
using Codebreak.Service.World.Game.Spell;
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
            m_effectById.Add(EffectEnum.AddLife, AddLifeEffect.Instance);
            m_effectById.Add(EffectEnum.NpcDialogLeave, NpcDialogLeaveEffect.Instance);
            m_effectById.Add(EffectEnum.NpcDialogReply, NpcDialogReplyEffect.Instance);
            m_effectById.Add(EffectEnum.NpcOpenBank, OpenBankEffect.Instance);

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
        public void ApplyEffects(EntityBase entity, long itemId, long targetId = -1, int targetCell = -1, Dictionary<string, string> parameters = null)
        {
            var item = entity.Inventory.RemoveItem(itemId);
            if(item == null)            
                return;

            if (item.StringEffects != string.Empty)
            {
                foreach (var effect in item.Statistics.GetEffects())
                {
                    if (m_effectById.ContainsKey(effect.Key))
                    {
                        m_effectById[effect.Key].ProcessItem(entity, item, effect.Value, targetId, targetCell);
                    }
                }
            }
            else
            {
                if (m_effectByType.ContainsKey((ItemTypeEnum)item.Template.Type))
                {
                    foreach(var effect in m_effectByType[(ItemTypeEnum)item.Template.Type])
                    {
                        effect.ProcessItem(entity, item, null, targetId, targetCell);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="effect"></param>
        /// <param name="parameters"></param>
        public void ApplyEffect(EntityBase entity, EffectEnum effect, Dictionary<string, string> parameters)
        {
            if(m_effectById.ContainsKey(effect))
            {
                m_effectById[effect].Process(entity, parameters);
            }
        }
    }
}
