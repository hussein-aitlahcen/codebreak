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
    public sealed class HarvestSkill : JobSkill
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="skillId"></param>
        public HarvestSkill(SkillIdEnum skillId, int obtainLevel, params int[] tools)
            : base(skillId, obtainLevel, tools)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public override bool Usable(Entity.CharacterEntity character, int level)
        {
            var weapon = character.Inventory.Items.Find(item => item.Slot == Database.Structure.ItemSlotEnum.SLOT_WEAPON);
            var weaponId = -1;
            if (weapon != null)
                weaponId = weapon.TemplateId;
            return ObtainLevel >= level && (Tools.Count == 0 || Tools.Contains(weaponId));
        }
    }
}
