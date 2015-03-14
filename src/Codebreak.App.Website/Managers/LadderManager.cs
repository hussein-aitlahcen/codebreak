using Codebreak.App.Website.Models.Worldservice;
using Codebreak.Framework.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Codebreak.App.Website.Managers
{
    /// <summary>
    /// 
    /// </summary>
    public class LadderEntry
    {
        public int LastIndex
        {
            get;
            set;
        }
        public int Index
        {
            get;
            set;
        } 
        public string Name
        {
            get;
            set;
        }
        public int Level
        {
            get;
            set;
        }
        public long Experience
        {
            get;
            set;
        }
        public int Breed
        {
            get;
            set;
        }
        public bool Sex
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class LadderManager : Singleton<LadderManager>
    {
        /// <summary>
        /// 
        /// </summary>
        public const int UPDATE_INTERVAL = 1000 * 60 * 60 * 12;

        /// <summary>
        /// 
        /// </summary>
        public const int TOP_LADDER = 100;
        
        /// <summary>
        /// 
        /// </summary>
        private Dictionary<long, LadderEntry> m_entryById;

        /// <summary>
        /// 
        /// </summary>
        private DateTime m_nextUpdate;

        /// <summary>
        /// 
        /// </summary>
        private DateTime m_lastUpdate;

        /// <summary>
        /// 
        /// </summary>
        public DateTime NextUpdate
        {
            get
            {
                return m_nextUpdate;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public DateTime LastUpdate
        {
            get
            {
                return m_lastUpdate;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool UpdateNeeded
        {
            get
            {
                return m_nextUpdate < DateTime.Now;
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<LadderEntry> Entries
        {
            get
            {
                return m_entryById.Values;                
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public LadderManager()
        {
            m_entryById = new Dictionary<long, LadderEntry>();
            m_nextUpdate = DateTime.Now.Subtract(TimeSpan.FromSeconds(1));
        }
        
        /// <summary>
        /// Update the ladder
        /// </summary>
        public void TryUpdate()
        {
            if (!UpdateNeeded)
                return;

            m_nextUpdate = DateTime.Now.AddMilliseconds(UPDATE_INTERVAL);
            m_lastUpdate = DateTime.Now;

            var index = 1;
            var newEntries = new Dictionary<long, LadderEntry>();
            foreach (var character in CharacterRepository.Instance.SqlMgr.Query<Character>("select * from characterinstance where dead = 0 order by level desc, experience desc, name asc limit " + TOP_LADDER))
            {
                var entry = GetEntry(character);
                entry.LastIndex = entry.Index;
                entry.Index = index++;
                entry.Level = character.Level;
                entry.Experience = character.Experience;
                entry.Breed = character.Breed;
                entry.Sex = character.Sex;
                entry.Name = character.Name;
                newEntries.Add(character.Id, entry);
            }

            m_entryById.Clear();
            m_entryById = newEntries;

            Logger.Info("LadderManager performing scheduled update : " + DateTime.Now.ToString());
        }

        /// <summary>
        /// Get or generate the entry
        /// </summary>
        /// <param name="character"></param>
        /// <returns></returns>
        private LadderEntry GetEntry(Character character)
        {
            if (m_entryById.ContainsKey(character.Id))
                return m_entryById[character.Id];
            var entry = new LadderEntry()
            {
                Index = m_entryById.Count,
                Name = character.Name,
                Level = character.Level,
                Experience = character.Experience,
                Breed = character.Breed,
                Sex = character.Sex,
                LastIndex = m_entryById.Count,
            };
            return entry;
        }
    }
}