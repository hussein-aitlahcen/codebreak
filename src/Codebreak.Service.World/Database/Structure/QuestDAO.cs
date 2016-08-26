using Codebreak.Framework.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Database.Structure
{
    [Table("quest")]
    public sealed class QuestDAO : DataAccessObject<QuestDAO>
    {
        [Key]
        public int Id { get; set; }
        public string Description { get; set; }

        [Write(false)]
        public List<QuestStepDAO> Steps { get; } = new List<QuestStepDAO>();
    }
}
