using Codebreak.Framework.Generic;
using Codebreak.Service.World.Database.Structure;
using Codebreak.Service.World.Game.Entity;
using Codebreak.Service.World.Game.Interactive.Type;
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
    public sealed class CraftPlanExchange : ExchangeBase, IValidableExchange, IRetryableExchange
    {
        private const int LOOP_OK = 1;
        private const int LOOP_INTERUPT = 2;
        private const int LOOP_ERROR = 3;
        private const int LOOP_INVALID = 4;

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
        private Dictionary<long, int> m_lastCaseItems;
        private Dictionary<int, long> m_templateQuantity;
        private ItemTemplateDAO m_craftItem;
        private int m_loopCount;
        private UpdatableTimer m_loopTimer;
        private CraftPlan m_plan;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="skill"></param>
        /// <param name="type"></param>
        public CraftPlanExchange(CharacterEntity character, CraftPlan plan, JobSkill skill, ExchangeTypeEnum type = ExchangeTypeEnum.EXCHANGE_CRAFTPLAN)
            : base(type)
        {
            m_caseItems = new Dictionary<long, int>();
            m_templateQuantity = new Dictionary<int, long>();
            m_plan = plan;
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
        /// <param name="success"></param>
        public override void Leave(bool success = false)
        {
            CancelRetry();

            m_plan.StopCraft();

            base.Leave(success);
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

                CheckCraftable();

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
                
                CheckCraftable();

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
        private void CheckCraftable()
        {
            m_craftItem = (m_caseItems.Count > 0) ? Skill.Craftables.Find(entry => entry.MatchCraft(m_templateQuantity)) : null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Validate(EntityBase entity)
        {
            Character.CachedBuffer = true;

            foreach (var item in m_caseItems)
            {
                var templateId = Character.Inventory.RemoveItem(item.Key, item.Value).TemplateId;
                m_templateQuantity[templateId] -= item.Value;
            }

            if (m_craftItem != null)
            {
                var chance = Job.CraftSuccessPercent(m_caseItems.Count);
                var success = Util.Next(0, 100) < chance;

                if(success)
                {
                    var item = m_craftItem.Create();
                    bool merged = Character.Inventory.AddItem(item);
                    item = Character.Inventory.Items.Find(entry => entry.TemplateId == m_craftItem.Id);

                    Character.Dispatch(WorldMessage.EXCHANGE_DISTANT_MOVEMENT(
                        ExchangeMoveEnum.MOVE_OBJECT,
                        OperatorEnum.OPERATOR_ADD,
                        item.Id + "|1|" + m_craftItem.Id + "|" + item.StringEffects));
                    Character.Dispatch(WorldMessage.CRAFT_TEMPLATE_CREATED(item.TemplateId));
                    if(m_loopTimer == null)
                        Character.Map.Dispatch(WorldMessage.CRAFT_INTERACTIVE_SUCCESS(Character.Id, item.TemplateId));
                }
                else
                {
                    Character.Dispatch(WorldMessage.CRAFT_TEMPLATE_FAILED(m_craftItem.Id));
                    if (m_loopTimer == null)
                        Character.Map.Dispatch(WorldMessage.CRAFT_INTERACTIVE_FAILED(Character.Id, m_craftItem.Id));
                }

                Character.CharacterJobs.AddExperience(Job, (long)(Job.CraftExperience(m_templateQuantity.Count) * WorldConfig.RATE_XP));
            }
            else
            {
                Character.Dispatch(WorldMessage.CRAFT_NO_RESULT());
                if (m_loopTimer == null)
                    Character.Map.Dispatch(WorldMessage.CRAFT_INTERACTIVE_NOTHING(Character.Id));
            }            

            Character.CachedBuffer = false;

            m_lastCaseItems = m_caseItems;
            m_caseItems = new Dictionary<long, int>();

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="count"></param>
        public void Retry(int count)
        {
            if (m_loopTimer != null)
                return;

            m_loopCount = count;
            m_loopTimer = base.AddTimer(700, Loop);
        }

        /// <summary>
        /// 
        /// </summary>
        public void CancelRetry()
        {
            if (m_loopTimer == null)
                return;

            EndLoop(LOOP_INTERUPT);
        }

        /// <summary>
        /// 
        /// </summary>
        private void Loop()
        {
            Character.CachedBuffer = true;

            Character.Dispatch(WorldMessage.CRAFT_LOOP_COUNT(m_loopCount - 1));

            foreach (var ingredient in m_lastCaseItems)
            {
                var item = Character.Inventory.GetItem(ingredient.Key);
                if (item == null || item.Quantity < ingredient.Value)
                {
                    EndLoop(LOOP_ERROR);
                    Character.CachedBuffer = false;
                    return;
                }
                AddItem(Character, ingredient.Key, ingredient.Value);
            }            
                       
            Validate(null);
            
            m_loopCount--;

            if (m_loopCount == 0)
            {
                EndLoop(LOOP_OK);

                m_caseItems = new Dictionary<long, int>();
            }

            Character.CachedBuffer = false;
        }

        /// <summary>
        /// 
        /// </summary>
        private void EndLoop(int reason)
        {
            Character.Dispatch(WorldMessage.CRAFT_LOOP_END(reason));

            base.RemoveTimer(m_loopTimer);
            m_loopTimer = null;
        }
    }
}
