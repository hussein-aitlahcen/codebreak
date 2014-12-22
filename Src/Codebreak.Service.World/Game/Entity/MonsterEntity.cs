using Codebreak.Service.World.Database.Structures;
using Codebreak.Service.World.Game.Fight.AI;
using Codebreak.Service.World.Game.Spell;
using Codebreak.Service.World.Game.Stats;
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
    public sealed class MonsterEntity : AIFighter
    {
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
        public override int BaseLife
        {
            get
            {
                return Grade.MaxLife;
            }
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
        public override string Name
        {
            get
            {
                return Grade.MonsterId.ToString();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override int Level
        {
            get
            {
                return Grade.Level;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override int SkinBase
        {
            get
            {
                return Grade.GetTemplate().GfxId;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override int SkinSizeBase
        {
            get
            {
                return 100;
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public override int RealLife
        {
            get;
            set;
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
        public MonsterGradeDAO Grade
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="monsterGrade"></param>
        public MonsterEntity(long id, MonsterGradeDAO monsterGrade)
            : base(EntityTypEnum.TYPE_MONSTER_FIGHTER, id)
        {
            Grade = monsterGrade;

            Statistics = new GenericStats(monsterGrade);
           
            RealLife = MaxLife;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="message"></param>
        public override void SerializeAs_GameMapInformations(OperatorEnum operation, StringBuilder message)
        {
            message.Append(Cell.Id).Append(';');
            message.Append(Orientation).Append(';');
            message.Append('0').Append(';');
            message.Append(Id).Append(';');
            message.Append(Name).Append(';');
            message.Append((int)EntityTypEnum.TYPE_MONSTER_FIGHTER).Append(';');
            message.Append(Skin).Append('^').Append(SkinSize).Append(';');
            message.Append(Grade.Grade).Append(';');
            message.Append(Grade.GetTemplate().Colors.Replace(",", ";"));
            message.Append(";0,0,0,0;");
            message.Append(Life).Append(';');
            message.Append(AP).Append(';');
            message.Append(MP).Append(';');
            message.Append(Statistics.GetTotal(EffectEnum.AddReduceDamagePercentNeutral)).Append(';');
            message.Append(Statistics.GetTotal(EffectEnum.AddReduceDamagePercentEarth)).Append(';');
            message.Append(Statistics.GetTotal(EffectEnum.AddReduceDamagePercentFire)).Append(';');
            message.Append(Statistics.GetTotal(EffectEnum.AddReduceDamagePercentWater)).Append(';');
            message.Append(Statistics.GetTotal(EffectEnum.AddReduceDamagePercentAir)).Append(';');
            message.Append(Statistics.GetTotal(EffectEnum.AddAPDodge)).Append(';');
            message.Append(Statistics.GetTotal(EffectEnum.AddMPDodge)).Append(';');
            message.Append(Team.Id);
        }
    }
}
