using Codebreak.Framework.Database;
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
    /// 
    [Table("monster")]
    public sealed class MonsterDAO : DataAccessObject<MonsterDAO>
    {
        [Key]
        public int Id
        {
            get;
            set;
        }
        public string Name
        {
            get;
            set;
        }
        public int GfxId
        {
            get;
            set;
        }
        public int Alignment
        {
            get;
            set;
        }
        public string Colors
        {
            get;
            set;
        }
        public string Kamas
        {
            get;
            set;
        }
        public int AggressionRange
        {
            get;
            set;
        }
        public int Race
        {
            get;
            set;
        }

        private Dictionary<int, MonsterGradeDAO> _monsterGrades = new Dictionary<int, MonsterGradeDAO>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="grade"></param>
        public void AddGrade(MonsterGradeDAO grade)
        {
            _monsterGrades.Add(grade.Grade, grade);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<MonsterGradeDAO> GetGrades()
        {
            return _monsterGrades.Values;
        }
    }
}
