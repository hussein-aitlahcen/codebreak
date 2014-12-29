using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Game.Dialog
{
    public sealed class NpcQuestion
    {
        public int QuestionId
        {
            get;
            private set;
        }

        private List<string> m_parameters;
        private Dictionary<int, NpcResponse> m_responses;
    }
}
