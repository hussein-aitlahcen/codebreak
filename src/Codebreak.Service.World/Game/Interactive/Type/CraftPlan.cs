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
                case SkillIdEnum.SKILL_COUDRE_UN_CHAPEAU:
                case SkillIdEnum.SKILL_COUDRE_UNE_CAPE:
                case SkillIdEnum.SKILL_CONFECTIONNER_UNE_CEINTURE:
                case SkillIdEnum.SKILL_CONFECTIONNER_DES_BOTTES:
                case SkillIdEnum.SKILL_CREER_UN_ANNEAU:
                case SkillIdEnum.SKILL_CREER_UNE_AMULETTE:
                case SkillIdEnum.SKILL_FORGER_UN_BOUCLIER:
                case SkillIdEnum.SKILL_FORGER_UNE_DAGUE:
                case SkillIdEnum.SKILL_FORGER_UNE_HACHE:
                case SkillIdEnum.SKILL_SCULPTER_UN_ARC:
                case SkillIdEnum.SKILL_SCULPTER_UNE_BAGUETTE:
                case SkillIdEnum.SKILL_SCULPTER_UN_BATON:
                case SkillIdEnum.SKILL_FORGER_UN_MARTEAU:
                case SkillIdEnum.SKILL_FORGER_UNE_FAUX:
                case SkillIdEnum.SKILL_FORGER_UNE_PIOCHE:
                case SkillIdEnum.SKILL_FORGER_UNE_EPEE:
                case SkillIdEnum.SKILL_COUDRE_UN_SAC:
                case SkillIdEnum.SKILL_FORGER_UNE_PELLE:
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
