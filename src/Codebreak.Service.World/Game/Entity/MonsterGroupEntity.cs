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
                return m_monsters;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private StringBuilder m_serializedMapInformations;

        /// <summary>
        /// 
        /// </summary>
        private List<MonsterEntity> m_monsters;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public MonsterGroupEntity(long id, int mapId, int cellId)
            : base(EntityTypeEnum.TYPE_MONSTER_GROUP, id)
        {
            m_monsters = new List<MonsterEntity>();
            long monsterId = -1;
            while(m_monsters.Count < 5)
            {
                var random = Util.Next(0, 2000);
                var template = MonsterRepository.Instance.GetById(random);
                if (template != null)
                {
                    if(template.Grades.Count() > 0)
                    {                        
                        m_monsters.Add(new MonsterEntity(monsterId--, template.Grades.Last()));
                    }
                }
            }

            AggressionRange = m_monsters.Max(monster => monster.Grade.Template.AggressionRange);
            MapId = mapId;
            CellId = cellId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool CanBeMoved()
        {
            return true;
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
           
                    // cell/orientation/bonus may change
                    message.Append(CellId).Append(";");
                    message.Append(Orientation).Append(';');
                    message.Append('0').Append(';');
                    if (m_serializedMapInformations == null)
                    {
                        string mobIds = string.Join(",", m_monsters.Select(monster => monster.Grade.MonsterId.ToString()));
                        string mobGfxs = string.Join(",", m_monsters.Select(monster => monster.Grade.Template.GfxId + "^100"));
                        string mobLevels = string.Join(",", m_monsters.Select(monster => monster.Grade.Level.ToString()));
                        string mobColors = string.Join("", m_monsters.Select(monster => monster.Grade.Template.Colors + ";0,0,0,0;"));

                        m_serializedMapInformations = new StringBuilder();
                        m_serializedMapInformations.Append(Id).Append(";");
                        m_serializedMapInformations.Append(mobIds).Append(";");
                        m_serializedMapInformations.Append((int)EntityTypeEnum.TYPE_MONSTER_GROUP).Append(';');
                        m_serializedMapInformations.Append(mobGfxs).Append(";");
                        m_serializedMapInformations.Append(mobLevels).Append(";");
                        m_serializedMapInformations.Append(mobColors);
                    }
                    message.Append(m_serializedMapInformations.ToString());
                    break;
            }
        }
    }
}
