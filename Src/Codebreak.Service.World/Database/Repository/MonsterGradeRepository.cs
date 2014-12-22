using Codebreak.Framework.Database;
using Codebreak.Service.World.Database.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Database.Repositories
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class MonsterGradeRepository : Repository<MonsterGradeRepository, MonsterGradeDAO>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="grade"></param>
        public override void OnObjectAdded(MonsterGradeDAO grade)
        {
            MonsterRepository.Instance.GetById(grade.MonsterId).AddGrade(grade);
        }
    }
}
