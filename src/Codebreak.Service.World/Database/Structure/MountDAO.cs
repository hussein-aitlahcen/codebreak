using Codebreak.Framework.Database;
using Codebreak.Service.World.Database.Repository;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Database.Structure
{
    [Table("mountinstance")]
    [ImplementPropertyChanged]
    public sealed class MountDAO : DataAccessObject<MountDAO>
    {
        [Key]
        public long Id { get; set; }
        public int TemplateId { get; set; }
        public long OwnerId { get; set; }
        public int PaddockId { get; set; }
        public bool Wild { get; set; }
        public bool Castrated { get; set; }
        public int Tired { get; set; }
        public bool Sex { get; set; }
        public int Capacity { get; set; }
        public string Name { get; set; }
        public long Experience { get; set; }
        public long Stamina { get; set; }
        public long Maturity { get; set; }
        public long Energy { get; set; }
        public long Serenity { get; set; }
        public long Love { get; set; }
        public int Reproduction { get; set; }
        public int XPSharePercent { get; set; }

        private MountTemplateDAO m_template;

        [Write(false)]
        public MountTemplateDAO Template
        {
            get
            {
                if(m_template == null)
                    m_template = MountTemplateRepository.Instance.GetById(TemplateId);
                return m_template;
            }
        }
    }
}
