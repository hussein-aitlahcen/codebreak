using Codebreak.Framework.Database;
using Codebreak.Service.World.Database.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Database.Repositories
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class CharacterGuildRepository : Repository<CharacterGuildRepository, CharacterGuildDAO>
    {
        private Dictionary<long, CharacterGuildDAO> _characterGuildById;

        /// <summary>
        /// 
        /// </summary>
        public CharacterGuildRepository()
        {
            _characterGuildById = new Dictionary<long, CharacterGuildDAO>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CharacterGuildDAO GetById(long id)
        {
            if (_characterGuildById.ContainsKey(id))
                return _characterGuildById[id];
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="characterGuild"></param>
        public override void OnObjectAdded(CharacterGuildDAO characterGuild)
        {
            _characterGuildById.Add(characterGuild.Id, characterGuild);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="characterGuild"></param>
        public override void OnObjectRemoved(CharacterGuildDAO characterGuild)
        {
            _characterGuildById.Remove(characterGuild.Id);
        }
    }
}
