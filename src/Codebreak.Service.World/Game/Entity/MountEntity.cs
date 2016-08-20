using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Codebreak.Service.World.Network;
using Codebreak.Service.World.Database.Structure;
using System.Threading;
using Codebreak.Service.World.Manager;
using Codebreak.Service.World.Game.Mount;
using Codebreak.Service.World.Game.Stats;

namespace Codebreak.Service.World.Game.Entity
{
    [Flags]
    public enum MountCapacityEnum
    {
        TIRELESS = 1,
        CARRIER = 2,
        REPRODUCTIVE = 4,
        WISE = 8,
        TOUGH = 16,
        INLOVE = 32,
        PRECOCIOUS = 64,
        GENETIC_PRONE = 128,
        CHAMELEON = 256,
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class MountEntity : AbstractEntity
    {
        public const int MAX_REPRODUCTION = 10;
        public const int MAX_TIRED = 250;
        public const int MIN_SERENITY = -10000;
        public const int MAX_SERENITY = 10000;
        public const int MAX_STAMINA = 10000;
        public const int MAX_LOVE = 10000;
        public const int MAX_ENERGY = 2000;

        private static long NextId = -10000;
        
        /// <summary>
        /// 
        /// </summary>
        public override int BaseLife
        {
            get;
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
        public override int Level
        {
            get
            {
                return ExperienceManager.Instance.GetLevel(ExperienceTypeEnum.MOUNT, Experience);
            }
            set
            {
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override int MapId
        {
            get
            {
                return m_record.PaddockId;
            }

            set
            {
                m_record.PaddockId = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override string Name
        {
            get
            {
                return m_record.Name;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override int RealLife
        {
            get; set;
        }

        /// <summary>
        /// 
        /// </summary>
        public override int Restriction
        {
            get; set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Capacities
        {
            get
            {
                return ",";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int MapEnergyCost
        {
            get
            {
                if (Tired >= 220 && Tired < 230)
                    return 1;
                if (Tired >= 230 && Tired < 240)
                    return 2;
                return 0;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int RideEnergyCost
        {
            get
            {
                if (Tired <= 170)
                    return 4;
                if (Tired <= 180)
                    return 5;
                if (Tired <= 200)
                    return 6;
                if (Tired <= 210)
                    return 7;
                if (Tired <= 220)
                    return 8;
                if (Tired <= 230)
                    return 10;
                if (Tired <= 240)
                    return 12;
                return 0;
            }
        }

        public int MaturityPercent => (int)((Maturity / (double)Template.MaxMaturity) * 100);
        public int Size => (int)Math.Round((double)(45 * (1 / (MaturityPercent + 1)) + MaturityPercent));

        // TODO: fecondation.time <= template.gestationtime
        public bool PregnancyTerminated => false;

        // TODO: fecondation.hours + 1
        public string SerializedPregnancyTime =>
            "-1";

        public bool Fecondable =>
            !Pregnant &&
            Love >= 7500 &&
            Stamina >= 7500 &&
            Reproduction < MAX_REPRODUCTION &&
            Level >= 5;
        
        public bool Pregnant => m_fecondation == null;

        public bool Ridable => Maturity == Template.MaxMaturity && !Wild;

        public int XPSharePercent => m_record.XPSharePercent;
        public long UniqueId => m_record.Id;
        public long OwnerId => m_record.OwnerId;
        public int Reproduction => m_record.Reproduction;
        public bool Castrated => m_record.Castrated;
        public int MaxEnergy => Template.DefaultEnergy + Template.EnergyPerLevel * Level;
        public int MaxPods => Template.DefaultPods + Template.PodsPerLevel * Level;
        public bool Wild => m_record.Wild;
        public int Tired => m_record.Tired;
        public long Energy => m_record.Energy;
        public long Stamina => m_record.Stamina;
        public long Maturity => m_record.Maturity;
        public long Love => m_record.Love;
        public long Serenity => m_record.Serenity;
        public MountCapacityEnum Capacity => (MountCapacityEnum)m_record.Capacity;

        public long ExperienceFloorNext => ExperienceManager.Instance.GetFloorNext(ExperienceTypeEnum.MOUNT, Experience);
        public long ExperienceFloorCurrent => ExperienceManager.Instance.GetFlootCurrent(ExperienceTypeEnum.MOUNT, Experience);
        public long Experience
        {
            get
            {
                return m_record.Experience;
            }
            set
            {
                m_record.Experience = value;
            }
        }

        public bool Sex => m_record.Sex;
        public int TemplateId => m_record.TemplateId;        
        public MountTemplateDAO Template => m_record.Template;

        /// <summary>
        /// 
        /// </summary>
        private MountDAO m_record;

        // TODO FECONDATION
        private Fecondation m_fecondation;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="record"></param>
        public MountEntity(MountDAO record)
            : base(EntityTypeEnum.TYPE_MOUNT, Interlocked.Decrement(ref NextId))
        {
            m_record = record;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public GenericStats GetStatistics()
        {
            var statistics = new GenericStats();
            foreach (var effect in Template.RandomEffects)            
                statistics.AddEffect(effect.Type, effect.Random * Level);            
            return statistics;         
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="capacity"></param>
        /// <returns></returns>
        public bool HasCapacity(MountCapacityEnum capacity) 
            => (Capacity & capacity) == capacity;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string SerializeAs_MountLightInfos()
        {
            if (HasCapacity(MountCapacityEnum.CHAMELEON))            
                return TemplateId.ToString() + ",-1,-1,-1";            
            else            
                return TemplateId.ToString();            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public string SerializeAs_MountInfos()
        {
            var message = new StringBuilder();
            message.Append(Id).Append(':');
            message.Append(TemplateId).Append(':');

            // TODO : ANCESTORS
            message.Append(string.Empty).Append(':');

            // TODO : CAPACITIES
            message.Append(Capacities).Append(':');

            message.Append(Name).Append(':');
            message.Append(Sex ? "1" : "0").Append(':');

            message.Append(Experience).Append(',')
                .Append(ExperienceFloorCurrent).Append(',')
                .Append(ExperienceFloorNext).Append(':');

            message.Append(Level).Append(':');
            message.Append(Ridable ? "1" : "0").Append(':');
            message.Append(MaxPods).Append(':');
            message.Append(Wild ? "1" : "0").Append(':');

            message.Append(Stamina).Append(',')
                .Append(MAX_STAMINA).Append(':');

            message.Append(Maturity).Append(',')
                .Append(Template.MaxMaturity).Append(':');

            message.Append(Energy).Append(',')
                .Append(MAX_ENERGY).Append(':');

            message.Append(Serenity)
                .Append(',')
                .Append(MIN_SERENITY)
                .Append(',')
                .Append(MAX_SERENITY).Append(':');

            message.Append(Love).Append(',')
                .Append(MAX_LOVE).Append(':');

            message.Append(SerializedPregnancyTime).Append(':');
            message.Append(Fecondable ? "1" : "0").Append(':');
            message.Append(GetStatistics().ToItemStats()).Append(':');

            message.Append(Tired).Append(',')
                .Append(MAX_TIRED).Append(':');

            message.Append(Castrated ? "-1" : Reproduction.ToString()).Append(',')
                .Append(MAX_REPRODUCTION).Append(':');

            return message.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="message"></param>
        public override void SerializeAs_GameMapInformations(OperatorEnum operation, StringBuilder message)
        {
            message.Append(CellId).Append(';');
            message.Append(Orientation).Append(';');
            message.Append(0).Append(';');
            message.Append(Id).Append(';');
            message.Append(Name).Append(';');
            message.Append("-9").Append(";");
            message.Append("7002").Append('^').Append(Size);
        }
    }
}
