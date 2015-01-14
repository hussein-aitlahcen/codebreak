using Codebreak.Service.World.Database.Structure;
using Codebreak.Service.World.Game.Action;
using Codebreak.Service.World.Game.Exchange;
using Codebreak.Service.World.Game.Fight;
using Codebreak.Service.World.Game.Fight.AI;
using Codebreak.Service.World.Game.Guild;
using Codebreak.Service.World.Game.Spell;
using Codebreak.Service.World.Game.Stats;
using Codebreak.Service.World.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Game.Entity
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class TaxCollectorEntity : AIFighter, IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        public override int MapId
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public override int CellId
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public override string Name
        {
            get 
            {
                return Util.EncodeBase36(DatabaseRecord.FirstName) + "," + Util.EncodeBase36(DatabaseRecord.Name);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override int Level
        {
            get
            {
                return Guild.Level;
            }
            set
            {
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public override int BaseLife
        {
            get 
            {
                return Statistics.GetTotal(EffectEnum.AddVitality);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override int RealLife
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public override int Restriction
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public override int SkinBase
        {
            get 
            { 
                return DatabaseRecord.Skin;
            }
            set
            {
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override int SkinSizeBase
        {
            get 
            { 
                return DatabaseRecord.SkinSize;
            }
            set
            {
            }
        }
               
        /// <summary>
        /// 
        /// </summary>
        public TaxCollectorDAO DatabaseRecord
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public GuildInstance Guild
        {
            get;
            private set;
        }


        /// <summary>
        /// 
        /// </summary>
        public List<GuildMember> Defenders
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool CanDefend
        {
            get
            {
                if (!HasGameAction(GameActionTypeEnum.FIGHT))
                    return false;
                return (Fight as TaxCollectorFight).CanDefend;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override long Kamas
        {
            get
            {
                return DatabaseRecord.Kamas;
            }
            set
            {
                DatabaseRecord.Kamas = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public long ExperienceGathered
        {
            get
            {
                return DatabaseRecord.Experience;
            }
            set
            {
                DatabaseRecord.Experience = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public TaxCollectorInventory Storage
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public Dictionary<int, int> FarmedItems
        {
            get;
            private set;
        }
        
        /// <summary>
        /// 
        /// </summary>
        public TaxCollectorEntity(GuildInstance guild, TaxCollectorDAO record)
            : base(EntityTypeEnum.TYPE_TAX_COLLECTOR, record.Id)
        {            
            DatabaseRecord = record;
            Guild = guild;

            MapId = DatabaseRecord.MapId;
            CellId = DatabaseRecord.CellId;

            Defenders = new List<GuildMember>();
            FarmedItems = new Dictionary<int, int>();

            Statistics = new GenericStats();
            Statistics.Merge(guild.Statistics.BaseStatistics);
            Spells = SpellBookFactory.Instance.Create(this);
            Storage = new TaxCollectorInventory(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool CanBeMoved()
        {
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Dispose()
        {
            Guild = null;

            Defenders.Clear();
            Defenders = null;

            base.Dispose();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fight"></param>
        /// <param name="team"></param>
        public override void JoinFight(Fight.FightBase fight, Fight.FightTeam team)
        {
            base.JoinFight(fight, team);

            SendCollectorAttacked();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="win"></param>
        public override void EndFight(bool win = false)
        {
            base.EndFight(win);

            if (win)
            {
                StartAction(GameActionTypeEnum.MAP);
                SendCollectorSurvived();
            }
            else
            {
                Guild.AddMessage(() => Guild.RemoveTaxCollector(this));
                SendCollectorDied();
            }

            Defenders.Clear();
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        public void DefenderJoin(GuildMember member)
        {            
            Defenders.Add(member);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="member"></param>
        public void DefenderLeft(GuildMember member)
        {
            Defenders.Remove(member);
        }

        /// <summary>
        /// 
        /// </summary>
        public void SendCollectorAttacked()
        {
            Guild.SafeDispatch(WorldMessage.GUILD_TAXCOLLECTOR_UNDER_ATTACK(Name, Map.X, Map.Y));
        }

        /// <summary>
        /// 
        /// </summary>
        public void SendCollectorDied()
        {
            Guild.SafeDispatch(WorldMessage.GUILD_TAXCOLLECTOR_DIED(Name, Map.X, Map.Y));
        }

        /// <summary>
        /// 
        /// </summary>
        public void SendCollectorSurvived()
        {
            Guild.SafeDispatch(WorldMessage.GUILD_TAXCOLLECTOR_SURVIVED(Name, Map.X, Map.Y));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exchangeType"></param>
        /// <returns></returns>
        public override bool CanBeExchanged(Exchange.ExchangeTypeEnum exchangeType)
        {
            return base.CanBeExchanged(exchangeType) && exchangeType == ExchangeTypeEnum.EXCHANGE_TAXCOLLECTOR;
        }

        /// <summary>
        /// 
        /// </summary>
        private StringBuilder m_serialized;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="message"></param>
        public override void SerializeAs_GameMapInformations(OperatorEnum operation, StringBuilder message)
        {
            switch (operation)
            {
                case OperatorEnum.OPERATOR_REMOVE:
                    message.Append(Id);
                    break;

                case OperatorEnum.OPERATOR_ADD:
                case OperatorEnum.OPERATOR_REFRESH:
                    if (HasGameAction(GameActionTypeEnum.MAP))
                    {
                        message.Append(CellId).Append(';');
                        message.Append(Orientation).Append(';');
                        if (m_serialized == null)
                        {
                            m_serialized = new StringBuilder();
                            m_serialized.Append(0).Append(';');
                            m_serialized.Append(Id).Append(';');
                            m_serialized.Append(Name).Append(';');
                            m_serialized.Append((int)Type).Append(';');
                            m_serialized.Append(SkinBase).Append('^');
                            m_serialized.Append(SkinSizeBase).Append(';');
                            m_serialized.Append(Level).Append(';');
                            m_serialized.Append(Guild.Name).Append(';');
                            m_serialized.Append(Guild.DisplayEmblem);
                        }
                        message.Append(m_serialized.ToString());
                    }
                    else if (HasGameAction(GameActionTypeEnum.FIGHT))
                    {
                        message.Append(Cell.Id).Append(';');
                        message.Append(Orientation).Append(';'); // Direction
                        message.Append('0').Append(';');
                        message.Append(Id).Append(';');
                        message.Append(Name).Append(';');
                        message.Append((int)Type).Append(';');
                        message.Append(Skin).Append('^');
                        message.Append(SkinSize).Append(';');
                        message.Append(Level).Append(';');
                        message.Append(Life).Append(';');
                        message.Append(AP).Append(';');
                        message.Append(MP).Append(';');                        
                        message.Append(Statistics.GetTotal(EffectEnum.AddReduceDamagePercentNeutral)).Append(';');
                        message.Append(Statistics.GetTotal(EffectEnum.AddReduceDamagePercentEarth)).Append(';');
                        message.Append(Statistics.GetTotal(EffectEnum.AddReduceDamagePercentFire)).Append(';');
                        message.Append(Statistics.GetTotal(EffectEnum.AddReduceDamagePercentWater)).Append(';');
                        message.Append(Statistics.GetTotal(EffectEnum.AddReduceDamagePercentAir)).Append(';');
                        message.Append(Statistics.GetTotal(EffectEnum.AddAPDodge)).Append(';');
                        message.Append(Statistics.GetTotal(EffectEnum.AddMPDodge)).Append(';');
                        message.Append(Team.Id);
                    }
                    break;
            }
        }
    }
}
