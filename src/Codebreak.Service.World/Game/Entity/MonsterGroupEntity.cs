using Codebreak.Service.World.Database.Repository;
using Codebreak.Service.World.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Game.Entity
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class MonsterGroupEntity : EntityBase
    {
        /// <summary>
        /// 
        /// </summary>
        public override string Name
        {
            get
            {
                return "MonsterGroup_" + Id;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override int MapId
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public override int CellId
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public override int Level
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override int RealLife
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override int BaseLife
        {
            get 
            { 
                throw new NotImplementedException(); 
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override int Restriction
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int AggressionRange
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<MonsterEntity> Monsters
        {
            get
            {
                return _monsters;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private StringBuilder _serializedMapInformations;
        private List<MonsterEntity> _monsters;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public MonsterGroupEntity(long id, int mapId, int cellId)
            : base(EntityTypeEnum.TYPE_MONSTER_GROUP, id)
        {
            _monsters = new List<MonsterEntity>();
            long monsterId = -1;
            while(_monsters.Count < 5)
            {
                var random = Util.Next(0, 2000);
                var template = MonsterRepository.Instance.GetById(random);
                if (template != null)
                {
                    if(template.GetGrades().Count() > 0)
                    {                        
                        _monsters.Add(new MonsterEntity(monsterId--, template.GetGrades().Last()));
                    }
                }
            }

            AggressionRange = _monsters.Max(monster => monster.Grade.GetTemplate().AggressionRange);
            MapId = mapId;
            CellId = cellId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="message"></param>
        public override void SerializeAs_GameMapInformations(OperatorEnum operation, StringBuilder message)
        {
            switch (operation)
            {
                case OperatorEnum.OPERATOR_REMOVE:
                    message.Append(Id);
                    break;

                case OperatorEnum.OPERATOR_ADD:
                case OperatorEnum.OPERATOR_REFRESH:
                    if (_serializedMapInformations == null)
                    {
                        string mobIds = string.Join(",", _monsters.Select(monster => monster.Grade.MonsterId.ToString()));
                        string mobGfxs = string.Join(",", _monsters.Select(monster => monster.Grade.GetTemplate().GfxId + "^100"));
                        string mobLevels = string.Join(",", _monsters.Select(monster => monster.Grade.Level.ToString()));
                        string mobColors = string.Join("", _monsters.Select(monster => monster.Grade.GetTemplate().Colors + ";0,0,0,0;"));

                        _serializedMapInformations = new StringBuilder();
                        _serializedMapInformations.Append(Id).Append(";");
                        _serializedMapInformations.Append(mobIds).Append(";");
                        _serializedMapInformations.Append((int)EntityTypeEnum.TYPE_MONSTER_GROUP).Append(';');
                        _serializedMapInformations.Append(mobGfxs).Append(";");
                        _serializedMapInformations.Append(mobLevels).Append(";");
                        _serializedMapInformations.Append(mobColors);
                    }

                    // cell/orientation/bonus may change
                    message.Append(CellId).Append(";");
                    message.Append(Orientation).Append(';');
                    message.Append('0').Append(';');
                    message.Append(_serializedMapInformations.ToString());
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public override void SerializeAs_ShopItemsListInformations(StringBuilder message)
        {
            throw new NotImplementedException();
        }
    }
}
