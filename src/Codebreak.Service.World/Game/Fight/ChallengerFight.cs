using Codebreak.Service.World.Game.Entity;
using Codebreak.Service.World.Game.Fight.Challenges;
using Codebreak.Service.World.Game.Map;
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
    public sealed class ChallengerFight : FightBase, IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        public CharacterEntity Attacker
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public CharacterEntity Defender
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
        public ChallengerFight(MapInstance map, long id, CharacterEntity attacker, CharacterEntity defender)
            : base(FightTypeEnum.TYPE_CHALLENGE, map, id, attacker.Id, 0, attacker.CellId, defender.Id, 0, defender.CellId, WorldConfig.PVP_START_TIMEOUT, WorldConfig.PVP_TURN_TIME, true)
        {
            Attacker = attacker;
            Defender = defender;

            JoinFight(Attacker, Team0);
            JoinFight(Defender, Team1);

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
        /// <param name="character"></param>
        /// <param name="kick"></param>
        /// <returns></returns>
        public override FightActionResultEnum FightQuit(CharacterEntity character, bool kick = false)
        {
            if (LoopState == FightLoopStateEnum.STATE_WAIT_END || LoopState == FightLoopStateEnum.STATE_ENDED)
                return FightActionResultEnum.RESULT_NOTHING;

            switch (State)
            {
                case FightStateEnum.STATE_PLACEMENT:
                    if (character.IsLeader)
                    {
                        foreach (var teamFighter in character.Team.Fighters)
                        {
                            if (base.TryKillFighter(teamFighter, teamFighter.Id, true, true) == FightActionResultEnum.RESULT_END)
                            {
                                return FightActionResultEnum.RESULT_END;
                            }
                        }

                        return FightActionResultEnum.RESULT_END;
                    }
                    else
                    {
                        character.Fight.Dispatch(WorldMessage.FIGHT_FLAG_UPDATE(OperatorEnum.OPERATOR_REMOVE, character.Team.LeaderId, character));
                        character.Fight.Dispatch(WorldMessage.GAME_MAP_INFORMATIONS(OperatorEnum.OPERATOR_REMOVE, character));
                        character.EndFight(true);
                        character.Dispatch(WorldMessage.FIGHT_LEAVE());

                        return FightActionResultEnum.RESULT_NOTHING;
                    }

                case FightStateEnum.STATE_FIGHTING:
                    if (character.IsSpectating)
                    {
                        character.EndFight(true);
                        character.Dispatch(WorldMessage.FIGHT_LEAVE());

                        return FightActionResultEnum.RESULT_NOTHING;
                    }

                    if (TryKillFighter(character, character.Id, true, true) != FightActionResultEnum.RESULT_END)
                    {
                        character.EndFight();
                        character.Dispatch(WorldMessage.FIGHT_LEAVE());

                        return FightActionResultEnum.RESULT_DEATH;
                    }

                    return FightActionResultEnum.RESULT_END;
            }

            return FightActionResultEnum.RESULT_NOTHING;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void InitEndCalculation()
        {
            foreach (var fighter in m_winnersTeam.Fighters)
            {
                Result.AddResult(fighter, FightEndTypeEnum.END_WINNER);
            }
            foreach (var fighter in m_losersTeam.Fighters)
            {
                Result.AddResult(fighter);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void ApplyEndCalculation()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public override void SerializeAs_FightList(StringBuilder message)
        {
            message.Append(Id.ToString()).Append(';');
            message.Append(UpdateTime).Append(';'); // TODO : Time;
            message.Append("0,-1,"); // TODO : Alignement etc
            message.Append(Team0.AliveFighters.Count()).Append(';');
            message.Append("0,-1,"); // TODO : Valeur monster "," Alignement monstre
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
                m_serializedFlag.Append('2').Append(';');
                m_serializedFlag.Append("-1").Append('|');
                m_serializedFlag.Append(Team1.LeaderId).Append(';');
                m_serializedFlag.Append(Team1.FlagCellId).Append(';');
                m_serializedFlag.Append('2').Append(';');
                m_serializedFlag.Append("-1");
            }

            message.Append(m_serializedFlag.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Dispose()
        {
            Attacker = null;
            Defender = null;
            
            m_serializedFlag.Clear();
            m_serializedFlag = null;

            base.Dispose();
        }
    }
}
