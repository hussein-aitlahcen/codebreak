using Codebreak.Service.World.Database.Structure;
using Codebreak.Service.World.Game.Entity;
using Codebreak.Service.World.Game.Fight;
using Codebreak.Service.World.Game.Quest.Impl;
using Codebreak.Service.World.Manager;
using Codebreak.Service.World.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Game.Quest
{
    public sealed class ObjectiveAdvancement
    {
        public int Id { get; set; }
        public string Value { get; set; }
    }

    public sealed class CharacterQuest : MessageDispatcher, IEntityListener
    {
        private static char SEPARATOR = ',';
        private static char SUB_SEPARATOR = ':';

        public int CurrentStepId => m_record.CurrentStepId;

        private CharacterQuestDAO m_record;
        private QuestStep m_currentStep;

        private List<ObjectiveAdvancement> m_advancement;

        public CharacterQuest(CharacterQuestDAO record)
        {
            m_record = record;
            m_advancement = new List<ObjectiveAdvancement>();
        }

        private void LoadCurrentStep()
        {
            m_currentStep = QuestManager.Instance.GetStep(CurrentStepId);
        }

        private void DeserializeObjectives()
        {
            m_advancement.Clear();
            foreach(var serializeObjective in m_record.SerializedObjectives.Split(new[] { SEPARATOR }, StringSplitOptions.RemoveEmptyEntries))
            {
                var data = serializeObjective.Split(new[] { SUB_SEPARATOR }, StringSplitOptions.RemoveEmptyEntries);
                m_advancement.Add(new ObjectiveAdvancement
                {
                    Id = int.Parse(data[0]),
                    Value = data[1],
                });
            }
        }

        private void SerializeObjectives()
        {
            m_record.SerializedObjectives = string.Join(SEPARATOR.ToString(), m_advancement.Select(a => a.Id + SUB_SEPARATOR + a.Value));
        }

        private void UpdateObjective<T>(int objectiveId, Func<string, T> transformer, Func<T, T> updater)
        {
            var advancement = m_advancement.First(a => a.Id == objectiveId);
            advancement.Value = updater(transformer(advancement.Value)).ToString();
            SerializeObjectives();
        }

        public void OnEntityEvent(EntityEventType ev, AbstractEntity entity, object parameters)
        {
            switch (ev)
            {
                case EntityEventType.FIGHT_KILL:
                    var monster = parameters as MonsterEntity;
                    if (monster != null)
                    {
                        foreach (var killMonster in m_currentStep.Objectives.OfType<KillMonsterObjective>())
                            if (killMonster.MonsterTemplateId == monster.Grade.Template.Id)
                                UpdateObjective(killMonster.Id, int.Parse, i => i + 1);
                    }
                    break;
            }
        }
    }
}
