using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Game.Spell
{
    /// <summary>
    /// 
    /// </summary>
    [ProtoContract(ImplicitFields = ImplicitFields.AllFields)]
    public sealed class SpellTemplate
    {
        public int Id;
        public string Name;
        public string Description;
        public int Sprite;
        public string SpriteInfos;
        public List<SpellLevel> Levels;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public SpellLevel GetLevel(int level)
        {
            if (Levels.Count < level)
                return null;
            return Levels[level - 1];
        }
    }
}
