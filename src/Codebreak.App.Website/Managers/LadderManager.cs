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
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class LadderManager : Singleton<LadderManager>
    {
        /// <summary>
        /// 
        /// </summary>
        public const long UPDATE_INTERVAL = 1000 * 60 * 5;

        /// <summary>
        /// 
        /// </summary>
        private static object m_syncLock = new object();

        /// <summary>
        /// 
        /// </summary>
        private Dictionary<long, LadderEntry> m_entryById;

        /// <summary>
        /// 
        /// </summary>
        private long m_nextUpdate;

        /// <summary>
        /// 
        /// </summary>
        public bool UpdateNeeded
        {
            get
            {
                return m_nextUpdate < Environment.TickCount;
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
                lock (m_syncLock)
                {
                    if (UpdateNeeded)
                        Update();
                    return m_entryById.Values;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public LadderManager()
        {
            m_entryById = new Dictionary<long, LadderEntry>();
            m_nextUpdate = 1;
        }
        
        /// <summary>
        /// Update the ladder
        /// </summary>
        private void Update()
        {
            if(!UpdateNeeded)
                return;

            m_nextUpdate = Environment.TickCount + UPDATE_INTERVAL;

            var index = 1;
            var newEntries = new Dictionary<long, LadderEntry>();
            foreach(var character in CharacterRepository.Instance.SqlMgr.Query<Character>("select * from characterinstance order by level desc limit 100"))
            {
                var entry = GetEntry(character, index);
                entry.LastIndex = entry.Index;
                entry.Index = index++;
                entry.Level = character.Level;
                entry.Experience = character.Experience;
                newEntries.Add(character.Id, entry);
            }

            m_entryById.Clear();
            m_entryById = newEntries;
        }

        /// <summary>
        /// Get or generate the entry
        /// </summary>
        /// <param name="character"></param>
        /// <returns></returns>
        private LadderEntry GetEntry(Character character, int index)
        {
            if (m_entryById.ContainsKey(character.Id))
                return m_entryById[character.Id];
            var entry = new LadderEntry()
            {
                Index = index,
                Name = character.Name,
                Level = character.Level,
                Experience = character.Experience,
                LastIndex = index,
            };
            return entry;
        }
    }
}