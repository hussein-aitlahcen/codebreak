using Codebreak.Service.World.Database.Structure;
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
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public virtual void SerializeAs_SkillListMessage(CharacterJobDAO job, StringBuilder message)
        {
            message.Append((int)SkillId).Append('~');
            message.Append("").Append('~'); // param1
            message.Append("").Append('~'); // param2
            message.Append("").Append('~'); // param3
            message.Append(""); // param4
        }
    }
}
