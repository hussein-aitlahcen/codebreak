using Codebreak.Service.World.Game.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Game.Job
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class JobSkill
    {
        /// <summary>
        /// 
        /// </summary>
        public SkillIdEnum SkillId
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int ObtainLevel
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public List<int> Tools
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="skillId"></param>
        public JobSkill(SkillIdEnum skillId, int obtainLevel = 1, params int[] tools)
        {
            SkillId = skillId;
            ObtainLevel = obtainLevel;
            Tools = new List<int>(tools);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="level"></param>
        /// <param name="tool"></param>
        /// <returns></returns>
        public virtual bool Usable(CharacterEntity character, int level)
        {
            var weapon = character.Inventory.Items.Find(item => item.Slot == Database.Structure.ItemSlotEnum.SLOT_WEAPON);
            var weaponId = -1;
            if(weapon != null)
                weaponId = weapon.TemplateId;
            return ObtainLevel >= level && (Tools.Count == 0 || Tools.Contains(weaponId));
        }
    }
}
