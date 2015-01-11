using Codebreak.Framework.Database;
using Codebreak.Service.World.Database.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Database.Structure
{
    /// <summary>
    /// 
    /// </summary>
    [Table("npcquestion")]
    public sealed class NpcQuestionDAO : DataAccessObject<NpcQuestionDAO>
    {
        [Key]
        public int Id
        {
            get;
            set;
        }

        public string Params
        {
            get;
            set;
        }

        public string Responses
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        private List<NpcResponseDAO> m_responses;

        /// <summary>
        /// 
        /// </summary>
        [Write(false)]
        public List<NpcResponseDAO> ResponseList
        {
            get
            {
                if (m_responses == null)
                {
                    m_responses = new List<NpcResponseDAO>();
                    if (Responses != string.Empty)
                    {
                        foreach (var response in Responses.Split(';'))
                        {
                            m_responses.Add(NpcResponseRepository.Instance.GetById(int.Parse(response)));
                        }
                    }
                }
                return m_responses;
            }
        }
    }
}
