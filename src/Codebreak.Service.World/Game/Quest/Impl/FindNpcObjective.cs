using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Codebreak.Service.World.Database.Structure;
using Codebreak.Service.World.Manager;
using Codebreak.Service.World.Database.Repository;

namespace Codebreak.Service.World.Game.Quest.Impl
{
    public sealed class FindNpcObjective : AbstractQuestObjective
    {
        public int NpcTemplateId { get; }

        public FindNpcObjective(QuestObjectiveDAO record) : base(record)
        {
            try
            {
                NpcTemplateId = int.Parse(record.Parameters);
            }
            catch(Exception e)
            {
                Logger.Warn("Quest::FindNpcObjective wrong parameter type, param=" + record.Parameters);
            }
        }
    }
}
