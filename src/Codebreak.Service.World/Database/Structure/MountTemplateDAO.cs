using Codebreak.Framework.Database;
using Codebreak.Service.World.Game.Stats;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Database.Structure
{
    [Table("mounttemplate")]
    public sealed class MountTemplateDAO : DataAccessObject<MountTemplateDAO>
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Effects { get; set; }
        public int DefaultPods { get; set; }
        public int PodsPerLevel { get; set; }
        public int DefaultEnergy { get; set; }
        public int EnergyPerLevel { get; set; }
        public int MaxMaturity { get; set; }
        public int GestationTime { get; set; }
        public int LearningTime { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        private RandomStatistics m_effects;

        /// <summary>
        /// 
        /// </summary>
        [Write(false)]
        public RandomStatistics RandomEffects
        {
            get
            {
                if (m_effects == null)
                    m_effects = RandomStatistics.Deserialize(Effects);
                return m_effects;
            }
        }
    }
}
