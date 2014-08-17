using Codebreak.Framework.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.WorldService.World.Database.Structure
{
    /// <summary>
    /// 
    /// </summary>
    [Table("ExperienceTemplate")]
    public sealed class ExperienceTemplateDAO : DataAccessObject<ExperienceTemplateDAO>
    {
        public int Level
        {
            get;
            set;
        }
        public long Character
        {
            get;
            set;
        }
        public long Job
        {
            get;
            set;
        }
        public long Mount
        {
            get;
            set;
        }
        public long Aggression
        {
            get;
            set;
        }
    }
}
