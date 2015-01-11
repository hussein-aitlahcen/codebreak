using Codebreak.Framework.Database;
using Codebreak.Service.World.Game.Spell;
using Codebreak.Service.World.Game.Stats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Database.Structure
{
    /// <summary>
    /// 
    /// </summary>
    [Table("itemset")]
    public sealed class ItemSetDAO : DataAccessObject<ItemSetDAO>
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        public int Id
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Effects2
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Effects3
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Effects4
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Effects5
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Effects6
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Effects7
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Effects8
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        private List<GenericStats> m_statistics;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="itemCount"></param>
        /// <returns></returns>
        public GenericStats GetStats(int itemCount)
        {
            if(m_statistics == null)
            {
                m_statistics = new List<GenericStats>();
                m_statistics.Add(new GenericStats());
                m_statistics.Add(new GenericStats());
                AddStats(Effects2);
                AddStats(Effects3);
                AddStats(Effects4);
                AddStats(Effects5);
                AddStats(Effects6);
                AddStats(Effects7);
                AddStats(Effects8);                
            }

            return m_statistics[itemCount];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="effects"></param>
        private void AddStats(string effects)
        {
            var stats = new GenericStats();
            if (effects != string.Empty)
            {
                foreach (var effect in effects.Split(';'))
                {
                    var data = effect.Split(',');
                    var effectId = int.Parse(data[0]);
                    var value = int.Parse(data[1]);
                    stats.AddItem((EffectEnum)effectId, value);
                }
            }
            m_statistics.Add(stats);
        }
    }
}
