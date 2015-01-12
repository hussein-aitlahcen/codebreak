using Codebreak.Service.World.Database.Structure;
using Codebreak.Service.World.Game.Entity;
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
        private long m_winnersMaxLevel;
        private long m_winnersTotalLevel;
        private long m_winnersTotalPP;
        private long m_losersTotalLevel;
        private long m_kamasLoot;
        private Dictionary<CharacterEntity, List<InventoryItemDAO>> m_distributedDrops;
        private List<InventoryItemDAO> m_itemLoot;
        
        /// <summary>
        /// 
        /// </summary>
        public override void InitEndCalculation()
        {            
            // Player win
            if (m_winnersTeam == Team0)
            {
                m_winnersMaxLevel = m_winnersFighter.Max(fighter => fighter.Level);
                m_winnersTotalLevel = m_winnersFighter.Sum(fighter => fighter.Level);
                m_winnersTotalPP = m_winnersFighter.Sum(fighter => fighter.Prospection);
                m_losersTotalLevel = m_losersFighter.Sum(fighter => fighter.Level);
                m_itemLoot = new List<InventoryItemDAO>();
               
                foreach (var monster in m_losersFighter.OfType<MonsterEntity>())
                {
                    m_kamasLoot += (long)Math.Round(Util.Next(monster.Grade.Template.MinKamas, monster.Grade.Template.MaxKamas) * WorldConfig.RATE_KAMAS);
                    m_itemLoot.AddRange(DropManager.Instance.GetDrops(m_winnersTotalPP, monster, WorldConfig.RATE_DROP));
                }

                m_distributedDrops = DropManager.Instance.Distribute(m_winnersFighter.OfType<CharacterEntity>(), m_winnersTotalPP, m_itemLoot);           
            }
            else // Monsters win
            {
                foreach (var player in m_losersFighter.OfType<CharacterEntity>())
                {
                    player.MapId = player.SavedMapId;
                    player.CellId = player.SavedCellId;
                    player.Life = 1;
                }
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
                foreach (var player in m_winnersFighter.OfType<CharacterEntity>())
                {
                    var exp = Util.CalculPVMExperience(m_losersFighter.OfType<MonsterEntity>(), m_winnersFighter.OfType<CharacterEntity>(), player.Level, player.Statistics.GetTotal(EffectEnum.AddWisdom));
                    var kamas = Util.CalculPVMKamas(m_kamasLoot, player.Prospection, m_winnersTotalPP);
                    var items = m_distributedDrops[player];
                    Dictionary<int, int> itemCount = new Dictionary<int, int>();

                    player.CachedBuffer = true;
                    foreach (var item in items)
                    {
                        player.Inventory.AddItem(item);
                        if (!itemCount.ContainsKey(item.TemplateId))
                            itemCount.Add(item.TemplateId, 0);
                        itemCount[item.TemplateId]++;
                    }
                    player.Inventory.AddKamas(kamas);
                    player.AddExperience(exp);
                    player.CachedBuffer = false;

                    Result.AddResult(player, true, false, kamas, exp, 0, 0, 0, 0, itemCount);
                }
            }
            else
            {
                foreach (var player in m_winnersFighter)
                {
                    Result.AddResult(player, true);
                }
            }

            foreach (var fighter in m_losersFighter)
            {
                Result.AddResult(fighter, false);
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
