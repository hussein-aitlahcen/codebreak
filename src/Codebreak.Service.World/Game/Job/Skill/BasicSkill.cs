using Codebreak.Service.World.Game.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Game.Job.Skill
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class BasicSkill : JobSkill
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="skillId"></param>
        public BasicSkill(SkillIdEnum skillId)
            : base(skillId)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="level"></param>
        /// <param name="tool"></param>
        /// <returns></returns>
        public override bool Usable(CharacterEntity character, int level)
        {
            return true;
        }
    }
}
