using Codebreak.Service.World.Database.Repository;
using Codebreak.Service.World.Database.Structure;
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
    public sealed class CraftSkill : JobSkill
    {
        /// <summary>
        /// 
        /// </summary>
        public List<ItemTemplateDAO> Craftables
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="skill"></param>
        /// <param name="obtainLevel"></param>
        /// <param name="tools"></param>
        public CraftSkill(SkillIdEnum skill, int obtainLevel, int[] craftables, params int[] tools)
            : base(skill, obtainLevel, tools)
        {
            Craftables = new List<ItemTemplateDAO>();
            foreach (var craftableItem in craftables)
            {
                var template = ItemTemplateRepository.Instance.GetById(craftableItem);
                if(template != null)
                    Craftables.Add(template);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public override void SerializeAs_SkillListMessage(CharacterJobDAO job, StringBuilder message)
        {
            var maxCase = job.CraftMaxCase;            
            if(maxCase > 2)
            {
                message.Append((int)SkillId).Append('~');
                message.Append(maxCase - 2).Append('~'); // param1
                message.Append("").Append('~'); // param2
                message.Append("").Append('~'); // param3
                message.Append("100,"); // param4
            }
            message.Append((int)SkillId).Append('~');
            message.Append(maxCase).Append('~'); // param1
            message.Append("").Append('~'); // param2
            message.Append("").Append('~'); // param3
            message.Append(job.CraftSuccessPercent(maxCase)); // param4
        }
    }
}
