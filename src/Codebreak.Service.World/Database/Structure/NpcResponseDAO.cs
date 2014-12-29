using Codebreak.Framework.Database;
using Codebreak.Service.World.Game.Spell;
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
    [Table("npcresponse")]
    public sealed class NpcResponseDAO : DataAccessObject<NpcResponseDAO>
    {
        [Key]
        public int Id
        {
            get;
            set;
        }

        public string Conditions
        {
            get;
            set;
        }

        public string Actions
        {
            get;
            set;
        }

        private Dictionary<EffectEnum, Dictionary<string, string>> m_actions;
        public Dictionary<EffectEnum, Dictionary<string, string>> GetActions()
        {
            if(m_actions == null)
            {
                m_actions = new Dictionary<EffectEnum, Dictionary<string, string>>();
                foreach(var action in Actions.Split(';'))
                {
                    var actionData = action.Split(':');
                    var actionId = (EffectEnum)int.Parse(actionData[0]);
                    var actionParams = new Dictionary<string, string>();
                    foreach (var param in actionData[1].Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        var paramData = param.Split('=');
                        actionParams.Add(paramData[0], paramData[1]);
                    }
                    m_actions.Add(actionId, actionParams);
                }
            }
            return m_actions;
        }
    }
}
