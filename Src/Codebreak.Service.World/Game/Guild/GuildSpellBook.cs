using Codebreak.Service.World.Game.Spell;
using Codebreak.Service.World.Manager;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Game.Guild
{
    /// <summary>
    /// 
    /// </summary>
    [ProtoContract(ImplicitFields = ImplicitFields.AllFields)]
    public sealed class GuildSpellBook
    {
        /// <summary>
        /// 
        /// </summary>
        [ProtoContract(ImplicitFields = ImplicitFields.AllFields)]
        public class Entry
        {
            /// <summary>
            /// 
            /// </summary>
            public int SpellId
            {
                get;
                set;
            }

            /// <summary>
            /// 
            /// </summary>
            public int Level
            {
                get;
                set;
            }

            /// <summary>
            /// 
            /// </summary>
            [ProtoIgnore]
            public SpellLevel SpellLevel
            {
                get
                {
                    if ((_spellLevel == null && Level > 0) || _spellLevel.Level != Level)
                        _spellLevel = SpellManager.Instance.GetSpellLevel(SpellId, Level);
                    return _spellLevel;
                }
            }

            /// <summary>
            /// 
            /// </summary>
            [ProtoIgnore]
            private SpellLevel _spellLevel;

            /// <summary>
            /// 
            /// </summary>
            public Entry()
            {
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="spellId"></param>
            /// <param name="level"></param>
            public Entry(int spellId, int level)
            {
                SpellId = spellId;
                Level = level;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static GuildSpellBook Create()
        {
            var instance = new GuildSpellBook();
            instance._spells.Add(462, new Entry(462, 0));
            instance._spells.Add(461, new Entry(461, 0));
            instance._spells.Add(460, new Entry(460, 0));
            instance._spells.Add(459, new Entry(459, 0));
            instance._spells.Add(458, new Entry(458, 0));
            instance._spells.Add(457, new Entry(457, 0));
            instance._spells.Add(456, new Entry(456, 0));
            instance._spells.Add(455, new Entry(455, 0));
            instance._spells.Add(454, new Entry(454, 0));
            instance._spells.Add(453, new Entry(453, 0));
            instance._spells.Add(452, new Entry(452, 0));
            instance._spells.Add(451, new Entry(451, 0));
            return instance;
        }

        /// <summary>
        /// 
        /// </summary>
        private Dictionary<int, Entry> _spells;

        /// <summary>
        /// 
        /// </summary>
        public GuildSpellBook()
        {
            _spells = new Dictionary<int, Entry>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spellId"></param>
        public void LevelUpSpell(int spellId)
        {
            if(_spells.ContainsKey(spellId))
            {
                _spells[spellId].Level++;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void SerializeAs_SpellsList(StringBuilder message)
        {
            foreach(var spell in _spells.Values)
            {
                message.Append(spell.SpellId).Append(';');
                message.Append(spell.Level).Append('|');
            }
            message.Remove(message.Length - 1, 1);
        }
    }
}
