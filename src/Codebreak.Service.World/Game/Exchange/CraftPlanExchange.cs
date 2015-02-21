using Codebreak.Service.World.Database.Structure;
using Codebreak.Service.World.Game.Entity;
using Codebreak.Service.World.Game.Job;
using Codebreak.Service.World.Game.Job.Skill;
using Codebreak.Service.World.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Game.Exchange
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class CraftPlanExchange : ExchangeBase, IValidableExchange
    {
        /// <summary>
        /// 
        /// </summary>
        public CharacterEntity Character
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public CharacterJobDAO Job
        {
            get;
            private set;
        }
        
        /// <summary>
        /// 
        /// </summary>
        public CraftSkill Skill
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int MaxCase
        {
            get;
            set;
        }
        
        /// <summary>
        /// 
        /// </summary>
        private Dictionary<long, int> m_caseItems;
        private Dictionary<int, long> m_templateQuantity;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="skill"></param>
        /// <param name="type"></param>
        public CraftPlanExchange(CharacterEntity character, JobSkill skill, ExchangeTypeEnum type = ExchangeTypeEnum.EXCHANGE_CRAFTPLAN)
            : base(type)
        {
            m_caseItems = new Dictionary<long, int>();
            m_templateQuantity = new Dictionary<int, long>();
            Character = character;
            Skill = (CraftSkill)skill;
            Job = Character.CharacterJobs.GetJob(skill.SkillId);
            MaxCase = Job.CraftMaxCase;
        }

        /// <summary>
        /// 
        /// </summary>
        protected override string SerializeAs_ExchangeCreate()
        {
            return MaxCase + ";" + (int)Skill.SkillId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="guid"></param>
        /// <param name="quantity"></param>
        /// <param name="price"></param>
        public override int AddItem(EntityBase entity, long guid, int quantity, long price = -1)
        {
            var item = Character.Inventory.GetItem(guid);
            if (item == null)
                return 0;

            if (quantity > item.Quantity)
                quantity = item.Quantity;

            if (item != null && item.Slot == ItemSlotEnum.SLOT_INVENTORY)
            {
                var alreadyExchangedQuantity = GetQuantity(guid);
                if (alreadyExchangedQuantity > 0)
                {
                    var realQuantity = item.Quantity - alreadyExchangedQuantity;
                    if (quantity > realQuantity)
                        quantity = realQuantity;
                }

                if (!m_templateQuantity.ContainsKey(item.TemplateId))
                    m_templateQuantity.Add(item.TemplateId, 0);
                m_templateQuantity[item.TemplateId] += quantity;
                m_caseItems[guid] += quantity;

                Character.Dispatch(WorldMessage.EXCHANGE_LOCAL_MOVEMENT(ExchangeMoveEnum.MOVE_OBJECT, OperatorEnum.OPERATOR_ADD, item.Id.ToString() + '|' + m_caseItems[guid]));



                return quantity;
            }

            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="guid"></param>
        /// <param name="quantity"></param>
        public override int RemoveItem(EntityBase entity, long guid, int quantity)
        {
            if (m_caseItems.ContainsKey(guid))
            {
                var item = entity.Inventory.Items.Find(entry => entry.Id == guid);
                if (quantity >= m_caseItems[guid])
                {
                    quantity = m_caseItems[guid];
                    m_caseItems.Remove(guid);
                }
                else
                {
                    m_caseItems[guid] -= quantity;
                }
                m_templateQuantity[item.TemplateId] -= quantity;
                if (m_templateQuantity[item.TemplateId] == 0)
                    m_templateQuantity.Remove(item.TemplateId);

                var exists = m_caseItems.ContainsKey(guid);
                Character.Dispatch(WorldMessage.EXCHANGE_LOCAL_MOVEMENT(ExchangeMoveEnum.MOVE_OBJECT, OperatorEnum.OPERATOR_REMOVE, item.Id.ToString()));
                if (exists)
                    Character.Dispatch(WorldMessage.EXCHANGE_LOCAL_MOVEMENT(ExchangeMoveEnum.MOVE_OBJECT, OperatorEnum.OPERATOR_ADD, item.Id.ToString() + '|' + m_caseItems[guid]));
                
                return quantity;
            }
            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="guid"></param>
        /// <returns></returns>
        private int GetQuantity(long guid)
        {
            if (m_caseItems.ContainsKey(guid))
                return m_caseItems[guid];
            else
                m_caseItems.Add(guid, 0);
            return 0;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Validate(EntityBase entity)
        {
            Character.CachedBuffer = true;
            
            var craftableItem = (m_caseItems.Count > 0) ? Skill.Craftables.Find(entry => entry.MatchCraft(m_templateQuantity)) : null;
            if (craftableItem != null)
            {
                // TODO : CRAFTTTT
            }

            Character.CachedBuffer = false;

            return false;
        }
    }
}
