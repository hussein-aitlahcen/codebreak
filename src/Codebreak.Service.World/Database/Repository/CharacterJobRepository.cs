using Codebreak.Framework.Database;
using Codebreak.Service.World.Database.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Database.Repository
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class CharacterJobRepository : Repository<CharacterJobRepository, CharacterJobDAO>
    {
        /// <summary>
        /// 
        /// </summary>
        private Dictionary<long, List<CharacterJobDAO>> m_JobByCharacter;

        /// <summary>
        /// 
        /// </summary>
        public CharacterJobRepository()
        {
            m_JobByCharacter = new Dictionary<long, List<CharacterJobDAO>>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jobEntry"></param>
        public override void OnObjectAdded(CharacterJobDAO jobEntry)
        {
            if (!m_JobByCharacter.ContainsKey(jobEntry.Id))
                m_JobByCharacter.Add(jobEntry.Id, new List<CharacterJobDAO>());
            m_JobByCharacter[jobEntry.Id].Add(jobEntry);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="relation"></param>
        public override void OnObjectRemoved(CharacterJobDAO jobEntry)
        {
            m_JobByCharacter[jobEntry.Id].Remove(jobEntry);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public List<CharacterJobDAO> GetByCharacterId(long characterId)
        {
            if (!m_JobByCharacter.ContainsKey(characterId))
                m_JobByCharacter.Add(characterId, new List<CharacterJobDAO>());
            return m_JobByCharacter[characterId];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="characterId"></param>
        /// <param name="jobId"></param>
        /// <param name="level"></param>
        /// <param name="experience"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        public CharacterJobDAO Create(long characterId, int jobId, int level = 1, long experience = 0, int option = 0)
        {
            var job = new CharacterJobDAO()
            {
                Id = characterId,
                JobId = jobId,
                Level = level,
                Experience = experience,
                Options = option,                
            };

            base.Created(job);

            return job;
        }
    }
}
