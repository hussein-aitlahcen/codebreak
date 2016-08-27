using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Codebreak.Service.World.Database.Structure;

namespace Codebreak.Service.World.Game.Quest.Impl
{
    public sealed class KillMonsterObjective : AbstractQuestObjective
    {
        public int MonsterTemplateId { get; }
        public int Count { get; }

        public KillMonsterObjective(QuestObjectiveDAO record) : base(record)
        {
            try
            {
                MonsterTemplateId = int.Parse(record.Parameters.Split(',')[0]);
                Count = int.Parse(record.Parameters.Split(',')[1]);
            }
            catch (Exception e)
            {
                Logger.Warn("Quest::KillMonsterObjective wrong parameter type, param=" + record.Parameters);
            }
        }

        public override bool Done(string value)
        {
            return int.Parse(value) >= Count;
        }
    }
}
