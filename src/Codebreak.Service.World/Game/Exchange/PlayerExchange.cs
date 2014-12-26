using Codebreak.Service.World.Database.Structure;
using Codebreak.Service.World.Game.Entity;
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
    public sealed class PlayerExchange : ExchangeBase
    {
        private CharacterEntity _local, _distant;
        private Dictionary<long, Dictionary<long, int>> _exchangedItems;
        private Dictionary<long, long> _exchangedKamas;
        private Dictionary<long, bool> _validated;

        public PlayerExchange(CharacterEntity local, CharacterEntity distant)
            : base(ExchangeTypeEnum.EXCHANGE_PLAYER)
        {
            _local = local;
            _distant = distant;
            _exchangedItems = new Dictionary<long, Dictionary<long, int>>()
            {
                { _local.Id, new Dictionary<long, int>() },
                { _distant.Id, new Dictionary<long, int>() },
            };
            _exchangedKamas = new Dictionary<long, long>()
            {
                { _local.Id, 0},
                { _distant.Id, 0},
            };
            _validated = new Dictionary<long, bool>()
            {
                { _local.Id, false },
                { _distant.Id, false },
            };
        }

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
                
                _exchangedItems[entity.Id][guid] += quantity;                

                if (entity.Id == _local.Id)
                {
                    _local.Dispatch(WorldMessage.EXCHANGE_LOCAL_MOVEMENT(ExchangeMoveEnum.MOVE_OBJECT, OperatorEnum.OPERATOR_ADD, item.Id.ToString() + '|' + _exchangedItems[entity.Id][guid]));
                    _distant.Dispatch(WorldMessage.EXCHANGE_DISTANT_MOVEMENT(ExchangeMoveEnum.MOVE_OBJECT, OperatorEnum.OPERATOR_ADD, item.Id.ToString() + '|' + _exchangedItems[entity.Id][guid] + '|' + item.TemplateId + '|' + item.StringEffects));
                }
                else
                {
                    _local.Dispatch(WorldMessage.EXCHANGE_DISTANT_MOVEMENT(ExchangeMoveEnum.MOVE_OBJECT, OperatorEnum.OPERATOR_ADD, item.Id.ToString() + '|' + _exchangedItems[entity.Id][guid] + '|' + item.TemplateId + '|' + item.StringEffects));
                    _distant.Dispatch(WorldMessage.EXCHANGE_LOCAL_MOVEMENT(ExchangeMoveEnum.MOVE_OBJECT, OperatorEnum.OPERATOR_ADD, item.Id.ToString() + '|' + _exchangedItems[entity.Id][guid]));
                 }
            }
        }

        private int GetQuantity(EntityBase entity, long guid)
        {
            if (_exchangedItems[entity.Id].ContainsKey(guid))
                return _exchangedItems[entity.Id][guid];
            else
                _exchangedItems[entity.Id].Add(guid, 0);
            return 0;
        }

        public override void RemoveItem(EntityBase entity, long guid, int quantity)
        {
            if (quantity < 1)
                return;

            UnValidateAll();

            if (_exchangedItems[entity.Id].ContainsKey(guid))
            {
                var item = entity.Inventory.Items.Find(entry => entry.Id == guid);
                if (quantity >= _exchangedItems[entity.Id][guid])
                {
                    _exchangedItems[entity.Id].Remove(guid);
                }
                else
                {
                    _exchangedItems[entity.Id][guid] -= quantity;
                }

                if (entity.Id == _local.Id)
                {
                    _local.Dispatch(WorldMessage.EXCHANGE_LOCAL_MOVEMENT(ExchangeMoveEnum.MOVE_OBJECT, OperatorEnum.OPERATOR_REMOVE, item.Id.ToString()));
                    _distant.Dispatch(WorldMessage.EXCHANGE_DISTANT_MOVEMENT(ExchangeMoveEnum.MOVE_OBJECT, OperatorEnum.OPERATOR_REMOVE, item.Id.ToString()));
                }
                else
                {
                    _local.Dispatch(WorldMessage.EXCHANGE_DISTANT_MOVEMENT(ExchangeMoveEnum.MOVE_OBJECT, OperatorEnum.OPERATOR_REMOVE, item.Id.ToString()));
                    _distant.Dispatch(WorldMessage.EXCHANGE_LOCAL_MOVEMENT(ExchangeMoveEnum.MOVE_OBJECT, OperatorEnum.OPERATOR_REMOVE, item.Id.ToString()));
                }
            }
        }

        public override void MoveKamas(EntityBase entity, long quantity)
        {
            if (quantity < 1)
                return;

            UnValidateAll();

            if (quantity > entity.Inventory.Kamas)
                quantity = entity.Inventory.Kamas;

            _exchangedKamas[entity.Id] = quantity;
            if (entity.Id == _local.Id)
            {
                _local.Dispatch(WorldMessage.EXCHANGE_LOCAL_MOVEMENT(ExchangeMoveEnum.MOVE_GOLD, OperatorEnum.OPERATOR_ADD, quantity.ToString()));
                _distant.Dispatch(WorldMessage.EXCHANGE_DISTANT_MOVEMENT(ExchangeMoveEnum.MOVE_GOLD, OperatorEnum.OPERATOR_ADD, quantity.ToString()));
            }
            else
            {
                _local.Dispatch(WorldMessage.EXCHANGE_DISTANT_MOVEMENT(ExchangeMoveEnum.MOVE_GOLD, OperatorEnum.OPERATOR_ADD, quantity.ToString()));
                _distant.Dispatch(WorldMessage.EXCHANGE_LOCAL_MOVEMENT(ExchangeMoveEnum.MOVE_GOLD, OperatorEnum.OPERATOR_ADD, quantity.ToString()));
            }
        }

        private void UnValidateAll()
        {
            _validated[_local.Id] = false;
            _validated[_distant.Id] = false;

            base.Dispatch(WorldMessage.EXCHANGE_VALIDATE(_local.Id, false));
            base.Dispatch(WorldMessage.EXCHANGE_VALIDATE(_distant.Id, false));
        }

        public bool Validate(EntityBase entity)
        {
            _validated[entity.Id] = _validated[entity.Id] == false; // inverse de la valeur actuelle

            base.Dispatch(WorldMessage.EXCHANGE_VALIDATE(entity.Id, _validated[entity.Id]));

            if (_validated.All(entry => entry.Value))
            {
                foreach (var kv in _exchangedItems[_local.Id])
                {
                    _distant.Inventory.AddItem(_local.Inventory.RemoveItem(kv.Key, kv.Value));
                }
                foreach (var kv in _exchangedItems[_distant.Id])
                {
                    _local.Inventory.AddItem(_distant.Inventory.RemoveItem(kv.Key, kv.Value));
                }

                _local.Inventory.SubKamas(_exchangedKamas[_local.Id]);
                _distant.Inventory.SubKamas(_exchangedKamas[_distant.Id]);
                _local.Inventory.AddKamas(_exchangedKamas[_distant.Id]);
                _distant.Inventory.AddKamas(_exchangedKamas[_local.Id]);

                return true;
            }

            return false;
        }
    }
}
