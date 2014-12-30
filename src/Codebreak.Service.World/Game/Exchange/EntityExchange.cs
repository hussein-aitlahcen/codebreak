using Codebreak.Service.World.Database.Structure;
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
    /// 
    /// </summary>
    public abstract class EntityExchange : ExchangeBase
    {
         /// <summary>
        /// 
        /// </summary>
        private EntityBase m_local, m_distant;

        /// <summary>
        /// 
        /// </summary>
        private Dictionary<long, Dictionary<long, int>> m_exchangedItems;

        /// <summary>
        /// 
        /// </summary>
        private Dictionary<long, long> m_exchangedKamas;

        /// <summary>
        /// 
        /// </summary>
        private Dictionary<long, bool> m_validated;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="local"></param>
        /// <param name="distant"></param>
        public EntityExchange(ExchangeTypeEnum type, EntityBase local, EntityBase distant)
            : base(type)
        {
            if (local.Inventory == null || distant.Inventory == null)
                throw new InvalidOperationException("EntityExchange : unable to start an exchange with an entity that hasnt an inventory.");
            
            m_local = local;
            m_distant = distant;
            m_exchangedItems = new Dictionary<long, Dictionary<long, int>>()
            {
                { m_local.Id, new Dictionary<long, int>() },
                { m_distant.Id, new Dictionary<long, int>() },
            };
            m_exchangedKamas = new Dictionary<long, long>()
            {
                { m_local.Id, 0},
                { m_distant.Id, 0},
            };
            m_validated = new Dictionary<long, bool>()
            {
                { m_local.Id, false },
                { m_distant.Id, false },
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="guid"></param>
        /// <param name="quantity"></param>
        public override void AddItem(EntityBase entity, long guid, int quantity)
        {
            if (quantity < 1)
                return;

            UnValidateAll();

            var item = entity.Inventory.Items.Find(entry => entry.Id == guid);
            if (quantity > item.Quantity)
                quantity = item.Quantity;

            if (item != null && item.GetSlot() == ItemSlotEnum.SLOT_INVENTORY)
            {
                var alreadyExchangedQuantity = GetQuantity(entity, guid);
                if (alreadyExchangedQuantity > 0)
                {
                    var realQuantity = item.Quantity - alreadyExchangedQuantity;
                    if (quantity > realQuantity)
                        quantity = realQuantity;
                }
                
                m_exchangedItems[entity.Id][guid] += quantity;                

                if (entity.Id == m_local.Id)
                {
                    m_local.Dispatch(WorldMessage.EXCHANGE_LOCAL_MOVEMENT(ExchangeMoveEnum.MOVE_OBJECT, OperatorEnum.OPERATOR_ADD, item.Id.ToString() + '|' + m_exchangedItems[entity.Id][guid]));
                    m_distant.Dispatch(WorldMessage.EXCHANGE_DISTANT_MOVEMENT(ExchangeMoveEnum.MOVE_OBJECT, OperatorEnum.OPERATOR_ADD, item.Id.ToString() + '|' + m_exchangedItems[entity.Id][guid] + '|' + item.TemplateId + '|' + item.StringEffects));
                }
                else
                {
                    m_local.Dispatch(WorldMessage.EXCHANGE_DISTANT_MOVEMENT(ExchangeMoveEnum.MOVE_OBJECT, OperatorEnum.OPERATOR_ADD, item.Id.ToString() + '|' + m_exchangedItems[entity.Id][guid] + '|' + item.TemplateId + '|' + item.StringEffects));
                    m_distant.Dispatch(WorldMessage.EXCHANGE_LOCAL_MOVEMENT(ExchangeMoveEnum.MOVE_OBJECT, OperatorEnum.OPERATOR_ADD, item.Id.ToString() + '|' + m_exchangedItems[entity.Id][guid]));
                 }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="guid"></param>
        /// <returns></returns>
        private int GetQuantity(EntityBase entity, long guid)
        {
            if (m_exchangedItems[entity.Id].ContainsKey(guid))
                return m_exchangedItems[entity.Id][guid];
            else
                m_exchangedItems[entity.Id].Add(guid, 0);
            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="guid"></param>
        /// <param name="quantity"></param>
        public override void RemoveItem(EntityBase entity, long guid, int quantity)
        {
            if (quantity < 1)
                return;

            UnValidateAll();

            if (m_exchangedItems[entity.Id].ContainsKey(guid))
            {
                var item = entity.Inventory.Items.Find(entry => entry.Id == guid);
                if (quantity >= m_exchangedItems[entity.Id][guid])
                {
                    m_exchangedItems[entity.Id].Remove(guid);
                }
                else
                {
                    m_exchangedItems[entity.Id][guid] -= quantity;
                }

                if (entity.Id == m_local.Id)
                {
                    m_local.Dispatch(WorldMessage.EXCHANGE_LOCAL_MOVEMENT(ExchangeMoveEnum.MOVE_OBJECT, OperatorEnum.OPERATOR_REMOVE, item.Id.ToString()));
                    m_distant.Dispatch(WorldMessage.EXCHANGE_DISTANT_MOVEMENT(ExchangeMoveEnum.MOVE_OBJECT, OperatorEnum.OPERATOR_REMOVE, item.Id.ToString()));
                }
                else
                {
                    m_local.Dispatch(WorldMessage.EXCHANGE_DISTANT_MOVEMENT(ExchangeMoveEnum.MOVE_OBJECT, OperatorEnum.OPERATOR_REMOVE, item.Id.ToString()));
                    m_distant.Dispatch(WorldMessage.EXCHANGE_LOCAL_MOVEMENT(ExchangeMoveEnum.MOVE_OBJECT, OperatorEnum.OPERATOR_REMOVE, item.Id.ToString()));
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="quantity"></param>
        public override void MoveKamas(EntityBase entity, long quantity)
        {
            if (quantity < 1)
                return;

            UnValidateAll();

            if (quantity > entity.Inventory.Kamas)
                quantity = entity.Inventory.Kamas;

            m_exchangedKamas[entity.Id] = quantity;
            if (entity.Id == m_local.Id)
            {
                m_local.Dispatch(WorldMessage.EXCHANGE_LOCAL_MOVEMENT(ExchangeMoveEnum.MOVE_GOLD, OperatorEnum.OPERATOR_ADD, quantity.ToString()));
                m_distant.Dispatch(WorldMessage.EXCHANGE_DISTANT_MOVEMENT(ExchangeMoveEnum.MOVE_GOLD, OperatorEnum.OPERATOR_ADD, quantity.ToString()));
            }
            else
            {
                m_local.Dispatch(WorldMessage.EXCHANGE_DISTANT_MOVEMENT(ExchangeMoveEnum.MOVE_GOLD, OperatorEnum.OPERATOR_ADD, quantity.ToString()));
                m_distant.Dispatch(WorldMessage.EXCHANGE_LOCAL_MOVEMENT(ExchangeMoveEnum.MOVE_GOLD, OperatorEnum.OPERATOR_ADD, quantity.ToString()));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void UnValidateAll()
        {
            m_validated[m_local.Id] = false;
            m_validated[m_distant.Id] = false;

            base.Dispatch(WorldMessage.EXCHANGE_VALIDATE(m_local.Id, false));
            base.Dispatch(WorldMessage.EXCHANGE_VALIDATE(m_distant.Id, false));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Validate(EntityBase entity)
        {
            m_validated[entity.Id] = m_validated[entity.Id] == false; // inverse de la valeur actuelle

            base.Dispatch(WorldMessage.EXCHANGE_VALIDATE(entity.Id, m_validated[entity.Id]));

            if (m_validated.All(entry => entry.Value))
            {
                foreach (var kv in m_exchangedItems[m_local.Id])
                {
                    m_distant.Inventory.AddItem(m_local.Inventory.RemoveItem(kv.Key, kv.Value));
                }
                foreach (var kv in m_exchangedItems[m_distant.Id])
                {
                    m_local.Inventory.AddItem(m_distant.Inventory.RemoveItem(kv.Key, kv.Value));
                }

                m_local.Inventory.SubKamas(m_exchangedKamas[m_local.Id]);
                m_distant.Inventory.SubKamas(m_exchangedKamas[m_distant.Id]);
                m_local.Inventory.AddKamas(m_exchangedKamas[m_distant.Id]);
                m_distant.Inventory.AddKamas(m_exchangedKamas[m_local.Id]);

                return true;
            }

            return false;
        }
    }
}
