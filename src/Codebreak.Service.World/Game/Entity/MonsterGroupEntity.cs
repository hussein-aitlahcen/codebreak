using Codebreak.Framework.Generic;
using Codebreak.Service.World.Database.Repository;
using Codebreak.Service.World.Database.Structure;
using Codebreak.Service.World.Game.Action;
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
        public int AgeBonus
        {
            get;
            set;
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
        private UpdatableTimer m_ageTimer;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public MonsterGroupEntity(long id, int mapId, int cellId, IEnumerable<MonsterSpawnDAO> monsters, int maxSize = 6)
            : base(EntityTypeEnum.TYPE_MONSTER_GROUP, id)
        {
            m_monsters = new List<MonsterEntity>();

            long monsterId = -1;
            var size = 1;
            var rand = Util.Next(0, 100);
            if (rand < 10) // 10%
                size = 1;
            else if (rand < 25)
                size = 2;
            else if (rand < 50)
                size = 3;
            else if (rand < 75)
                size = 4;
            else if (rand < 90)
                size = 5;
            else
                size = 6;

            if (size > maxSize)
                size = maxSize;

            if (monsters.Count() > 0)
            {
                while (m_monsters.Count < size)
                {
                    foreach(var spawn in monsters)
                    {
                        var chance = Util.Next(0, 100);
                        if (chance < spawn.Probability * 100)
                        {
                            m_monsters.Add(new MonsterEntity(monsterId--, spawn.Grade));

                            if(m_monsters.Count == size)                            
                                break;                            
                        }
                    }
                }

                AggressionRange = m_monsters.Max(monster => monster.Grade.Template.AggressionRange);
            }

            MapId = mapId;
            CellId = cellId;

            Inventory = new EntityInventory(this, (int)EntityTypeEnum.TYPE_MONSTER_GROUP, Id);

            base.AddTimer(m_ageTimer = new UpdatableTimer(1000 * WorldConfig.PVM_STAR_BONUS_PERCENT_SECONDS, UpdateAge));
        }

        /// <summary>
        /// 
        /// </summary>
        public void UpdateAge()
        {
            if (AgeBonus > WorldConfig.PVM_MAX_STAR_BONUS - 2)
                base.RemoveTimer(m_ageTimer);
            AgeBonus++;
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
                    message.Append(AgeBonus).Append(';');
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
