using Codebreak.Framework.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Database.Structure
{
    [Table("questobjective")]
    public sealed class QuestObjectiveDAO : DataAccessObject<QuestObjectiveDAO>
    {
        [Key]
        public int Id { get; set; }
        public int StepId { get; set; }
        public int Type { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public string Parameters { get; set; }
    }
}
