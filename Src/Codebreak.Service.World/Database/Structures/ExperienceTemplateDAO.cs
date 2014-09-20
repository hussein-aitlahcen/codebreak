using Codebreak.Framework.Database;

namespace Codebreak.Service.World.Database.Structures
{
    /// <summary>
    /// 
    /// </summary>
    [Table("experiencetemplate")]
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
