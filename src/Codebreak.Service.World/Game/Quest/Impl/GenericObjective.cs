using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Codebreak.Service.World.Database.Structure;

namespace Codebreak.Service.World.Game.Quest.Impl
{
    public sealed class GenericObjective : AbstractQuestObjective
    {
        public string Text => m_record.Parameters;

        public GenericObjective(QuestObjectiveDAO record) : base(record)
        {
        }
    }
}
