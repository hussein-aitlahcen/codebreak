using Codebreak.Service.World.Game.Entity;
using Codebreak.Service.World.Game.Job;
using Codebreak.Service.World.Game.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Game.Interactive.Type
{
    /// <summary>
    /// 
    /// </summary>
    public class CraftPlan : InteractiveObject
    {
        /// <summary>
        /// 
        /// </summary>
        public CraftPlan(MapInstance map, int cellId)
            : base(map, cellId)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="skill"></param>
        public override void UseWithSkill(CharacterEntity character, JobSkill skill)
        {
            switch(skill.SkillId)
            {
                case SkillIdEnum.SKILL_SCIER:
                case SkillIdEnum.SKILL_FORGER_UNE_EPEE:
                    Craft(character, skill);
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="skill"></param>
        private void Craft(CharacterEntity character, JobSkill skill)
        {
            character.CraftStart(this, skill);
        }
    }
}
