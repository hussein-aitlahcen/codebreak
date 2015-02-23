using Codebreak.Framework.Database;
using Codebreak.Service.World.Database.Repository;
using Codebreak.Service.World.Game.Entity;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Database.Structure
{
    /// <summary>
    /// 
    /// </summary>
    [Table("monstergrade")]
    public sealed class MonsterGradeDAO : DataAccessObject<MonsterGradeDAO>
    {
        [Key]
        public long Id
        {
            get;
            set;
        }

        public int MonsterId
        {
            get;
            set;
        }

        public int Grade
        {
            get;
            set;
        }
        
        public int Level
        {
            get;
            set;
        }
        public int AP
        {
            get;
            set;
        }
        public int MP
        {
            get;
            set;
        }
        public int MaxLife
        {
            get;
            set;
        }
        public int NeutralResistance
        { 
            get; set; 
        }
        public int EarthResistance
        { 
            get; set; 
        }
        public int FireResistance
        { 
            get; set; 
        }
        public int WaterResistance
        { 
            get; set; 
        }
        public int AirResistance
        { 
            get; set; 
        }
        public int APDodgePercent
        {
            get; set; 
        }
        public int MPDodgePercent
        {
            get;
            set;
        }
        public int Wisdom
        {
            get;
            set;
        }
        public int Strenght
        {
            get;
            set;
        }
        public int Intelligence
        {
            get;
            set;
        }
        public int Chance
        {
            get;
            set;
        }
        public int Agility
        {
            get;
            set;
        }
        public int Initiative
        {
            get;
            set;
        }
        public int MaxInvocation
        {
            get;
            set;
        }
        public int Experience
        {
            get;
            set;
        }        
        /// <summary>
        /// 
        /// </summary>
        private MonsterDAO m_template;
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Write(false)]
        [DoNotNotify]
        public MonsterDAO Template
        {
            get
            {
                if (m_template == null)
                    m_template = MonsterRepository.Instance.GetById(MonsterId);
                return m_template;
            }
        }
    }
}
