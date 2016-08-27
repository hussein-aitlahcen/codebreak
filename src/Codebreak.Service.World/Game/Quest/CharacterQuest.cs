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

        public int Id => CurrentStep.QuestId;

        public bool Done
        {
            get { return m_record.Done; }
            private set { m_record.Done = value; }
        }

        public int CurrentStepId
        {
            get { return m_record.CurrentStepId; }
            private set { m_record.CurrentStepId = value; }
        }

        public QuestStep CurrentStep
        {
            get;
            private set;
        }

        public Quest Template => m_template;

        public List<ObjectiveAdvancement> Advancements { get; }

        private readonly CharacterEntity m_character;
        private readonly CharacterQuestDAO m_record;
        private Quest m_template;
        
        public CharacterQuest(CharacterEntity character, CharacterQuestDAO record)
        {
            m_character = character;
            m_record = record;
            Advancements = new List<ObjectiveAdvancement>();
            
            Initialize();
        }

        private void Initialize()
        {
            CurrentStep = QuestManager.Instance.GetStep(CurrentStepId);
            m_template = QuestManager.Instance.GetQuest(Id);
            m_character.AddEventListener(this);
            DeserializeObjectives();
        }
        
        private void DeserializeObjectives()
        {
            Advancements.Clear();
            foreach(var serializeObjective in m_record.SerializedObjectives.Split(new[] { SEPARATOR }, StringSplitOptions.RemoveEmptyEntries))
            {
                var data = serializeObjective.Split(new[] { SUB_SEPARATOR }, StringSplitOptions.RemoveEmptyEntries);
                Advancements.Add(new ObjectiveAdvancement
                {
                    Id = int.Parse(data[0]),
                    Value = data[1],
                });
            }
        }

        private void SerializeObjectives()
        {
            m_record.SerializedObjectives = string.Join
                (
                    SEPARATOR.ToString(), 
                    Advancements.Select(a => a.Id.ToString() + SUB_SEPARATOR + a.Value)
                );
        }

        private void UpdateObjective<T>(int objectiveId, Func<string, T> transformer, Func<T, T> updater)
        {
            var advancement = Advancements.First(a => a.Id == objectiveId);
            advancement.Value = updater(transformer(advancement.Value)).ToString();
            SerializeObjectives();
            CheckEnd();
        }

        public string GetAdvancement(int objectiveId)
        {
            return Advancements.First(a => a.Id == objectiveId).Value;
        }

        private void CheckEnd()
        {
            Done = CurrentStep.Objectives.All(objective => objective.Done(GetAdvancement(objective.Id)));
            if (Done)
            {
                foreach (var action in CurrentStep.ActionsList)
                    ActionEffectManager.Instance.ApplyEffect(m_character, action.Effect, action.Parameters);
                
                var nextStep = m_template.Steps.FirstOrDefault(s => s.Order > CurrentStep.Order);
                if (nextStep != null)
                {
                    CurrentStepId = nextStep.Id;
                    CurrentStep = nextStep;
                    m_character.Dispatch(WorldMessage.IM_INFO_MESSAGE(InformationEnum.INFOS_QUEST_UPDATE, Id));
                }
                else
                {
                    m_character.Dispatch(WorldMessage.IM_INFO_MESSAGE(InformationEnum.INFOS_QUEST_END, Id));
                }
            }
        }

        public void OnEntityEvent(EntityEventType ev, AbstractEntity entity, object parameters)
        {
            if (Done)
                return;

            switch (ev)
            {
                case EntityEventType.FIGHT_KILL:
                    var monster = parameters as MonsterEntity;
                    if (monster != null)
                    {
                        foreach (var killMonster in CurrentStep.Objectives
                            .OfType<KillMonsterObjective>()
                            .Where(killMonster => killMonster.MonsterTemplateId == monster.Grade.Template.Id))
                            UpdateObjective(killMonster.Id, int.Parse, i => i + 1);
                    }
                    break;
            }
        }
    }
}
