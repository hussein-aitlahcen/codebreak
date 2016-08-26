using Codebreak.Framework.Database;
using Codebreak.Service.World.Database.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Database.Repository
{
    public sealed class CharacterQuestRepository : Repository<CharacterQuestRepository, CharacterQuestDAO>
    {
        public override void OnObjectAdded(CharacterQuestDAO obj)
        {
            CharacterRepository.Instance.GetById(obj.Id).AddQuest(obj);
        }
    }
}
