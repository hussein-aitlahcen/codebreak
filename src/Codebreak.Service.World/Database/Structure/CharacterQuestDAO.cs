using Codebreak.Framework.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Database.Structure
{
    [Table("characterquest")]
    public sealed class CharacterQuestDAO : DataAccessObject<CharacterQuestDAO>
    {
        [Key]
        public int Id { get; set; }
        public int CurrentStepId { get; set; }
        public string SerializedObjectives { get; set; }
    }
}
