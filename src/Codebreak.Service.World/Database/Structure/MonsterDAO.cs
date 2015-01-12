using Codebreak.Framework.Database;
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


        private int m_minKamas = -1, m_maxKamas = -1;
        private List<DropTemplateDAO> m_drops = new List<DropTemplateDAO>();
        private Dictionary<int, MonsterGradeDAO> m_monsterGrades = new Dictionary<int, MonsterGradeDAO>();

        /// <summary>
        /// 
        /// </summary>
        public int MinKamas
        {
            get
            {
                if (m_minKamas == -1)
                    InitKamas();
                return m_minKamas;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int MaxKamas
        {
            get
            {
                if (m_maxKamas == -1)
                    InitKamas();
                return m_maxKamas;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<MonsterGradeDAO> Grades
        {
            get
            {
                return m_monsterGrades.Values;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<DropTemplateDAO> Drops
        {
            get
            {
                return m_drops;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitKamas()
        {
            var data = Kamas.Split(';');
            m_minKamas = int.Parse(data[0]);
            m_maxKamas = int.Parse(data[1]);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="grade"></param>
        public void AddGrade(MonsterGradeDAO grade)
        {
            m_monsterGrades.Add(grade.Grade, grade);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="drop"></param>
        public void AddDrop(DropTemplateDAO drop)
        {
            m_drops.Add(drop);
        }        
    }
}
