using Codebreak.Service.World.Database.Structure;
using Codebreak.Service.World.Game.Entity;
using Codebreak.Service.World.Game.Fight.Challenges;
using Codebreak.Service.World.Game.Map;
using Codebreak.Service.World.Game.Spell;
using Codebreak.Service.World.Manager;
using Codebreak.Service.World.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Game.Fight
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class MonsterFight : FightBase, IDisposable
    {        
        /// <summary>
        /// 
        /// </summary>
        public CharacterEntity Character
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public MonsterGroupEntity MonsterGroup
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        private StringBuilder m_serializedFlag;

        /// <summary>
        /// 
        /// </summary>
        public MonsterFight(MapInstance map, long id, CharacterEntity character, MonsterGroupEntity monsterGroup)
            : base(FightTypeEnum.TYPE_PVM, map, id, character.Id, 0, character.CellId, monsterGroup.Id, monsterGroup.Monsters.First().Grade.Template.Alignment, monsterGroup.CellId, WorldConfig.PVM_START_TIMEOUT, WorldConfig.PVM_TURN_TIME)
        {
            Character = character;
            MonsterGroup = monsterGroup;
            
            JoinFight(Character, Team0);
            foreach(var monster in monsterGroup.Monsters)            
                JoinFight(monster, Team1);

            foreach (var challenge in ChallengeManager.Instance.Generate(WorldConfig.PVM_CHALLENGE_COUNT))
                Team0.AddChallenge(challenge);
            
            base.Start();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fighter"></param>
        /// <returns></returns>
        public override bool CanJoin(CharacterEntity character)
        {
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fighter"></param>
        /// <param name="kick"></param>
        /// <returns></returns>
        public override FightActionResultEnum FightQuit(CharacterEntity character, bool kick = false)
        {
            if (LoopState == FightLoopStateEnum.STATE_WAIT_END || LoopState == FightLoopStateEnum.STATE_ENDED)
                return FightActionResultEnum.RESULT_NOTHING;

            switch (State)
            {
                case FightStateEnum.STATE_PLACEMENT:
                    if (base.TryKillFighter(character, character.Id, true, true) == FightActionResultEnum.RESULT_END)
                    {
                        return FightActionResultEnum.RESULT_END;
                    }
                    else
                    {
                        if (kick)
                        {
                            character.Fight.Dispatch(WorldMessage.FIGHT_FLAG_UPDATE(OperatorEnum.OPERATOR_REMOVE, character.Team.LeaderId, character));
                            character.Fight.Dispatch(WorldMessage.GAME_MAP_INFORMATIONS(OperatorEnum.OPERATOR_REMOVE, character));
                            character.LeaveFight(true);
                            character.Dispatch(WorldMessage.FIGHT_LEAVE());
                        }

                        return FightActionResultEnum.RESULT_NOTHING;
                    }

                case FightStateEnum.STATE_FIGHTING:
                    if (character.IsSpectating)
                    {
                        character.LeaveFight(kick);
                        character.Dispatch(WorldMessage.FIGHT_LEAVE());

                        return FightActionResultEnum.RESULT_NOTHING;
                    }

                    if (TryKillFighter(character, character.Id, true, true) != FightActionResultEnum.RESULT_END)
                    {
                        character.LeaveFight();
                        character.Dispatch(WorldMessage.FIGHT_LEAVE());

                        return FightActionResultEnum.RESULT_DEATH;
                    }

                    return FightActionResultEnum.RESULT_END;
            }

            return FightActionResultEnum.RESULT_NOTHING;
        }

        // end fight calculation
        private int m_challengeXpBonus = 1;
        private int m_challengeDropBonus = 1;
        private long m_winnersMaxLevel;
        private long m_winnersTotalLevel;
        private long m_droppersTotalPP;
        private long m_losersTotalLevel;
        private long m_kamasLoot;
        private Dictionary<FighterBase, List<InventoryItemDAO>> m_distributedDrops;
        private List<FighterBase> m_droppers;
        private List<InventoryItemDAO> m_itemLoot;

        /// <summary>
        /// 
        /// </summary>
        private void InitCalculs()
        {
            m_winnersMaxLevel = m_winnersFighter.Max(fighter => fighter.Level);
            m_winnersTotalLevel = m_winnersFighter.Sum(fighter => fighter.Level);
            m_losersTotalLevel = m_losersFighter.Sum(fighter => fighter.Level);
            var winChallenges = m_winnersTeam.SucceededChallenges;
            m_challengeXpBonus = (int)Math.Round((double)(100 + winChallenges.Sum(challenge => challenge.BasicXpBonus + challenge.TeamXpBonus)) / 100);
            m_challengeDropBonus = (int)Math.Round((double)(100 + winChallenges.Sum(challenge => challenge.BasicDropBonus + challenge.TeamDropBonus)) / 100);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private void InitDroppers()
        {
            m_droppers = new List<FighterBase>();
            m_droppers.AddRange(m_winnersTeam.Fighters.Where
                (
                    fighter => fighter.Type == EntityTypeEnum.TYPE_CHARACTER ||
                    (fighter.Invocator != null && fighter.Invocator.Type == EntityTypeEnum.TYPE_CHARACTER && ((MonsterEntity)fighter).Grade.MonsterId == WorldConfig.LIVING_CHEST_ID)
                ));
            if (Map.SubArea.TaxCollector != null)
                m_droppers.Add(Map.SubArea.TaxCollector); // Perco
            m_droppersTotalPP = m_droppers.Sum(fighter => fighter.Prospection); // Total PP
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitLoots()
        {
            m_itemLoot = new List<InventoryItemDAO>();
            foreach (var monster in m_losersFighter.OfType<MonsterEntity>())
            {
                m_kamasLoot += (long)Math.Round(Util.Next(monster.Grade.Template.MinKamas, monster.Grade.Template.MaxKamas) * WorldConfig.RATE_KAMAS * m_challengeDropBonus);
                m_itemLoot.AddRange(DropManager.Instance.GetDrops(m_droppersTotalPP, monster, WorldConfig.RATE_DROP * m_challengeDropBonus));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitDistribution()
        {
            m_distributedDrops = DropManager.Instance.Distribute(m_droppers, m_droppersTotalPP, m_itemLoot);  
        }

        /// <summary>
        /// 
        /// </summary>
        public override void InitEndCalculation()
        {            
            // Player win
            if (m_winnersTeam == Team0)
            {
                InitCalculs();
                InitDroppers();
                InitLoots();
                InitDistribution();        
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public override void ApplyEndCalculation()
        {
            // Player win
            if (m_winnersTeam == Team0)
            {
                foreach (var fighter in m_droppers)
                {
                    long exp = 0;
                    long kamas = 0;
                    var items = m_distributedDrops[fighter];
                    Dictionary<int, int> itemCount = new Dictionary<int, int>();

                    fighter.CachedBuffer = true;
                                        
                    switch(fighter.Type)
                    {
                        case EntityTypeEnum.TYPE_CHARACTER:
                            var character = fighter as CharacterEntity;
                            exp = Util.CalculPVMExperience(m_losersFighter.OfType<MonsterEntity>(), m_droppers, fighter.Level, fighter.Statistics.GetTotal(EffectEnum.AddWisdom), m_challengeXpBonus, MonsterGroup.AgeBonus);
                            kamas = Util.CalculPVMKamas(m_kamasLoot, fighter.Prospection, m_droppersTotalPP);
                            character.Inventory.AddKamas(kamas);
                            character.AddExperience(exp);
                            foreach (var item in items)
                            {
                                character.Inventory.AddItem(item);
                                if (!itemCount.ContainsKey(item.TemplateId))
                                    itemCount.Add(item.TemplateId, 0);
                                itemCount[item.TemplateId]++;
                            }
                            Result.AddResult(fighter, FightEndTypeEnum.END_WINNER, false, kamas, exp, 0, 0, 0, 0, itemCount);
                            break;

                        case EntityTypeEnum.TYPE_MONSTER_FIGHTER:
                            foreach (var item in items)
                            {
                                if(fighter.Invocator != null && fighter.Invocator.Inventory != null)
                                    fighter.Invocator.Inventory.AddItem(item);
                                if (!itemCount.ContainsKey(item.TemplateId))
                                    itemCount.Add(item.TemplateId, 0);
                                itemCount[item.TemplateId]++;
                            }
                            Result.AddResult(fighter, FightEndTypeEnum.END_WINNER, false, kamas, exp, 0, 0, 0, 0, itemCount);
                            break;

                        case EntityTypeEnum.TYPE_TAX_COLLECTOR:
                            var taxCollector = fighter as TaxCollectorEntity;
                            exp = Util.CalculPVMExperienceTaxCollector(m_losersFighter.OfType<MonsterEntity>(), m_winnersFighter, fighter.Level, fighter.Statistics.GetTotal(EffectEnum.AddWisdom), m_challengeXpBonus, MonsterGroup.AgeBonus);          
                            kamas = Util.CalculPVMKamas(m_kamasLoot, fighter.Prospection, m_droppersTotalPP);
                            taxCollector.Storage.AddKamas(kamas);
                            taxCollector.ExperienceGathered += exp;
                            foreach (var item in items)
                            {
                                taxCollector.Storage.AddItem(item);
                                if (!itemCount.ContainsKey(item.TemplateId))
                                    itemCount.Add(item.TemplateId, 0);
                                itemCount[item.TemplateId]++;
                            }
                            Result.AddResult(fighter, FightEndTypeEnum.END_TAXCOLLECTOR, false, kamas, 0, 0, 0, exp, 0, itemCount);
                            break;
                    }

                    fighter.CachedBuffer = false;
                }
            }
            else
            {
                foreach (var player in m_winnersFighter)
                {
                    Result.AddResult(player, FightEndTypeEnum.END_WINNER);
                }
            }

            foreach (var fighter in m_losersFighter)
            {
                Result.AddResult(fighter);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public override void SerializeAs_FightList(StringBuilder message)
        {
            message.Append(Id.ToString()).Append(';');
            message.Append(UpdateTime).Append(';');
            message.Append("0,-1,");
            message.Append(Team0.AliveFighters.Count()).Append(';');
            message.Append("1,");
            message.Append(MonsterGroup.Monsters.ElementAt(0).Grade.Template.Alignment).Append(',');
            message.Append(Team1.AliveFighters.Count()).Append(';');
            message.Append('|');
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public override void SerializeAs_FightFlag(StringBuilder message)
        {
            if (m_serializedFlag == null)
            {
                m_serializedFlag = new StringBuilder();
                m_serializedFlag.Append(Id).Append(';');
                m_serializedFlag.Append((int)Type).Append('|');
                m_serializedFlag.Append(Team0.LeaderId).Append(';');
                m_serializedFlag.Append(Team0.FlagCellId).Append(';');
                m_serializedFlag.Append('0').Append(';');
                m_serializedFlag.Append("-1").Append('|');
                m_serializedFlag.Append(Team1.LeaderId).Append(';');
                m_serializedFlag.Append(Team1.FlagCellId).Append(';');
                m_serializedFlag.Append('1').Append(';');
                m_serializedFlag.Append(MonsterGroup.Monsters.ElementAt(0).Grade.Template.Alignment);
            }

            message.Append(m_serializedFlag.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Dispose()
        {
            Character = null;
            MonsterGroup = null;
            
            m_serializedFlag.Clear();
            m_serializedFlag = null;

            base.Dispose();
        }
    }
}
