using Codebreak.Framework.Database;
using Codebreak.Service.World.Database.Repository;
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
    public enum ZoneTypeEnum
    {
        TYPE_SUBAREA = 0,
        TYPE_AREA = 1,
        TYPE_SUPERAREA = 2,
        TYPE_MAP = 3,
    }

    /// <summary>
    /// 
    /// </summary>
    [Table("monsterspawn")]
    public sealed class MonsterSpawnDAO : DataAccessObject<MonsterSpawnDAO>
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        public int ZoneType
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [Write(false)]
        public ZoneTypeEnum Type
        {
            get
            {
                return (ZoneTypeEnum)ZoneType;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int ZoneId
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int GradeId
        {
            get;
            set;
        }

        public double Probability
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        private MonsterGradeDAO m_grade;

        /// <summary>
        /// 
        /// </summary>
        [Write(false)]
        public MonsterGradeDAO Grade
        {
            get
            {
                if (m_grade == null)
                    m_grade = MonsterGradeRepository.Instance.GetById(GradeId);
                return m_grade;
            }
        }
    }
}
