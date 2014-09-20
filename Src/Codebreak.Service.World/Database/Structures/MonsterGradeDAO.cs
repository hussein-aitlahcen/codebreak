using Codebreak.Framework.Database;
using Codebreak.Service.World.Database.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Database.Structures
{
    /// <summary>
    /// 
    /// </summary>
    [Table("MonsterGrade")]
    public sealed class MonsterGradeDAO : DataAccessObject<MonsterGradeDAO>
    {
        public int Id
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
        { get; set; }
        public int EarthResistance
        { get; set; }
        public int FireResistance
        { get; set; }
        public int WaterResistance
        { get; set; }
        public int AirResistance
        { get; set; }
        public int APDodgePercent
        { get; set; }
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
        public string Spells
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
        private MonsterDAO _template;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public MonsterDAO GetTemplate()
        {
            if (_template == null)
                _template = MonsterRepository.Instance.GetById(MonsterId);
            return _template;
        }
    }
}
