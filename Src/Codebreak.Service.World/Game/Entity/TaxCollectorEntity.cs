using Codebreak.Service.World.Database.Structures;
using Codebreak.Service.World.Game.Action;
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
    public sealed class TaxCollectorEntity : AIFighter
    {
        /// <summary>
        /// 
        /// </summary>
        public override int MapId
        {
            get
            {
                return DatabaseRecord.MapId;
            }
            set
            {
                DatabaseRecord.MapId = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override int CellId
        {
            get
            {
                return DatabaseRecord.CellId;
            }
            set
            {
                DatabaseRecord.CellId = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override string Name
        {
            get 
            {
                return Util.EncodeBase36(DatabaseRecord.FirstName) + "," + Util.EncodeBase36(DatabaseRecord.FirstName);
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
        }

        /// <summary>
        /// 
        /// </summary>
        public List<InventoryItemDAO> Items
        {
            get
            {
                return DatabaseRecord.GetItems();
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
        public TaxCollectorEntity(GuildInstance guild, TaxCollectorDAO record)
            : base(EntityTypEnum.TYPE_TAX_COLLECTOR, record.Id)
        {
            DatabaseRecord = record;
            Guild = guild;

            Statistics = new GenericStats();
            Statistics.Merge(guild.Statistics.BaseStatistics);
            Inventory = new TaxCollectorInventory(this);
        }

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
                        message.Append(Orientation).Append(';'); ;
                        message.Append(0).Append(';');
                        message.Append(Id).Append(';');
                        message.Append(Name).Append(';');
                        message.Append((int)Type).Append(';');
                        message.Append(SkinBase).Append('^');
                        message.Append(SkinSizeBase).Append(';');
                        message.Append(Level).Append(';');
                        message.Append(Guild.Name).Append(';');
                        message.Append(Guild.DisplayEmblem);
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
