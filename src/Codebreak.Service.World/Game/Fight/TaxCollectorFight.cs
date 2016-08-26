using Codebreak.Service.World.Game.Action;
using Codebreak.Service.World.Game.Entity;
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
    public sealed class TaxCollectorFight : AbstractFight, IDisposable
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
        public TaxCollectorEntity TaxCollector
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool CanDefend
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
        public TaxCollectorFight(MapInstance map, long id, CharacterEntity attacker, TaxCollectorEntity taxCollector)
            : base(FightTypeEnum.TYPE_PVT, map, id, attacker.Id, 0, attacker.CellId, taxCollector.Id, 0, taxCollector.CellId, WorldConfig.PVT_START_TIMEOUT, WorldConfig.PVT_TURN_TIME)
        {
            CanDefend = true;
            Attacker = attacker;
            TaxCollector = taxCollector;
            
            JoinFight(Attacker, Team0);
            JoinFight(TaxCollector, Team1);

            AddTimer(WorldConfig.PVT_TELEPORT_DEFENDERS_TIMEOUT, TeleportDefenders, true);
        }

        /// <summary>
        /// 
        /// </summary>
        public void TeleportDefenders()
        {
            CanDefend = false;

            foreach(var defender in TaxCollector.Defenders)
            {
                var character = defender.Character;
                if(character != null)
                {
                    character.AddMessage(() =>
                    {
                        character.StopAction(GameActionTypeEnum.TAXCOLLECTOR_AGGRESSION);

                        JoinFight(character, Team1);
                    });
                }
            }

            TaxCollector.Defenders.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="team"></param>
        public override void OnCharacterJoin(CharacterEntity character, FightTeam team)
        {
            TaxCollector.Guild.TaxCollectorAttackerJoin(TaxCollector.Id, character);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
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
                    if (TryKillFighter(character, character, true, true) == FightActionResultEnum.RESULT_END)
                        return FightActionResultEnum.RESULT_END;
                    
                    if (kick)
                    {
                        character.Fight.Dispatch(WorldMessage.FIGHT_FLAG_UPDATE(OperatorEnum.OPERATOR_REMOVE, character.Team.LeaderId, character));
                        character.Fight.Dispatch(WorldMessage.GAME_MAP_INFORMATIONS(OperatorEnum.OPERATOR_REMOVE, character));
                        character.EndFight(true);
                        character.Dispatch(WorldMessage.FIGHT_LEAVE());
                    }

                    TaxCollector.Guild.TaxColectorAttackerLeave(TaxCollector.Id, character);

                    return FightActionResultEnum.RESULT_NOTHING;
                    

                case FightStateEnum.STATE_FIGHTING:
                    if (character.IsSpectating)
                    {
                        character.EndFight(true);
                        character.Dispatch(WorldMessage.FIGHT_LEAVE());

                        return FightActionResultEnum.RESULT_NOTHING;
                    }

                    if (TryKillFighter(character, character, true, true) != FightActionResultEnum.RESULT_END)
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
        /// <param name="message"></param>
        public override void SerializeAs_FightList(StringBuilder message)
        {
            message.Append(Id.ToString()).Append(';');
            message.Append(UpdateTime).Append(';'); // TODO : Time;
            message.Append("0,-1,"); // TODO : Alignement etc
            message.Append(Team0.AliveFighters.Count()).Append(';');
            message.Append("3,-1,"); // TODO : Valeur monster "," Alignement monstre
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
                m_serializedFlag.Append("-1").Append('|'); // neutral
                m_serializedFlag.Append(Team1.LeaderId).Append(';');
                m_serializedFlag.Append(Team1.FlagCellId).Append(';');
                m_serializedFlag.Append('3').Append(';');
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
            TaxCollector = null;

            m_serializedFlag.Clear();
            m_serializedFlag = null;

            base.Dispose();
        }
    }
}
