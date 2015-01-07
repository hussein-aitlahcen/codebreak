using Codebreak.Service.World.Database.Structure;
using Codebreak.Service.World.Game.Action;
using Codebreak.Service.World.Game.Entity;
using Codebreak.Service.World.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Game.Exchange
{
    /// <summary>
    /// type:kamas:quantity,type:itemId:quantity;type:kamas:quantity|
    /// </summary>
    public sealed class NpcExchange : EntityExchange
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
        public NonPlayerCharacterEntity Npc
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        private Dictionary<int, long> m_templateQuantity;
        private RewardEntry m_reward;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="npc"></param>
        public NpcExchange(CharacterEntity character, NonPlayerCharacterEntity npc)
            : base(ExchangeTypeEnum.EXCHANGE_NPC, character, npc)
        {
            m_templateQuantity = new Dictionary<int, long>();

            Character = character;
            Npc = npc;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override bool Validate(EntityBase entity)
        {
            if(m_reward != null)
            {
                Character.CachedBuffer = true;
                Character.Inventory.SubKamas(m_exchangedKamas[Character.Id]);
                foreach (var item in m_exchangedItems[Character.Id])
                    Character.Inventory.RemoveItem(item.Key, item.Value);
                Character.Inventory.AddKamas(m_reward.RewardedKamas);
                foreach(var item in m_reward.RewardedItems)
                    Character.Inventory.AddItem(item.Template.Create(item.Quantity));
                Character.CachedBuffer = false;
                return true;
            }

            Character.AddMessage(() => Character.AbortAction(GameActionTypeEnum.EXCHANGE));
            return false;
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
            var added = base.AddItem(entity, guid, quantity, price);
            if(added > 0)
            {
                var invItem = Character.Inventory.GetItem(guid);
                if (invItem == null)
                    return 0;
                if (!m_templateQuantity.ContainsKey(invItem.TemplateId))
                    m_templateQuantity.Add(invItem.TemplateId, 0);
                m_templateQuantity[invItem.TemplateId] += quantity;

                CheckRewards();

                return added;
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
            var removed = base.RemoveItem(entity, guid, quantity);
            if (removed > 0)
            {
                var invItem = Character.Inventory.GetItem(guid);
                if (invItem == null)
                    return 0;
                m_templateQuantity[invItem.TemplateId] -= quantity;

                CheckRewards();

                return removed;
            }
            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public override long MoveKamas(EntityBase entity, long quantity)
        {
            var moved = base.MoveKamas(entity, quantity);

            CheckRewards();

            return moved;
        }

        /// <summary>
        /// 
        /// </summary>
        private void CheckRewards()
        {
            if(m_reward != null)
                foreach(var item in m_reward.RewardedItems)            
                    Character.Dispatch(WorldMessage.EXCHANGE_DISTANT_MOVEMENT(ExchangeMoveEnum.MOVE_OBJECT, OperatorEnum.OPERATOR_REMOVE, item.TemplateId.ToString()));

            m_reward = null;

            Character.Dispatch(WorldMessage.EXCHANGE_DISTANT_MOVEMENT(ExchangeMoveEnum.MOVE_GOLD, OperatorEnum.OPERATOR_ADD, "0"));

            m_reward = Npc.Rewards.Find(entry => entry.Match(m_templateQuantity, base.m_exchangedKamas[Character.Id]));
            if (m_reward != null)
            {
                foreach (var item in m_reward.RewardedItems)
                {
                    Character.Dispatch(WorldMessage.EXCHANGE_DISTANT_MOVEMENT(ExchangeMoveEnum.MOVE_OBJECT, OperatorEnum.OPERATOR_ADD, item.TemplateId + "|" + item.Quantity + '|' + item.TemplateId + '|' + item.Template.Effects));   
                }
                Character.Dispatch(WorldMessage.EXCHANGE_DISTANT_MOVEMENT(ExchangeMoveEnum.MOVE_GOLD, OperatorEnum.OPERATOR_ADD, m_reward.RewardedKamas.ToString()));
            }
        }
    }
}
