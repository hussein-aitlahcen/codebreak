using Codebreak.Framework.Database;
using Codebreak.Service.World.Game.Fight;
using Codebreak.Service.World.Game.Spell;
using PropertyChanged;
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
    [Table("fightaction")]
    public sealed class FightActionDAO : DataAccessObject<FightActionDAO>
    {
        [Key]
        public int ZoneType
        {
            get;
            set;
        }

        [Write(false)]
        public ZoneTypeEnum Zone
        {
            get
            {
                return (ZoneTypeEnum)ZoneType;
            }
        }

        [Key]
        public int ZoneId
        {
            get;
            set;
        }

        [Key]
        public int FightType
        {
            get;
            set;
        }

        [Write(false)]
        public FightTypeEnum Fight
        {
            get
            {
                return (FightTypeEnum)FightType;
            }
        }

        [Key]
        public int FightState
        {
            get;
            set;
        }

        [Write(false)]
        public FightStateEnum State
        {
            get
            {
                return (FightStateEnum)FightState;
            }
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

        /// <summary>
        /// 
        /// </summary>
        private Dictionary<EffectEnum, Dictionary<string, string>> m_actions;

        /// <summary>
        /// 
        /// </summary>
        [Write(false)]
        public Dictionary<EffectEnum, Dictionary<string, string>> ActionsList
        {
            get
            {
                if (m_actions == null)
                {
                    m_actions = new Dictionary<EffectEnum, Dictionary<string, string>>();
                    foreach (var action in Actions.Split('|'))
                    {
                        var actionData = action.Split(':');
                        var actionId = (EffectEnum)int.Parse(actionData[0]);
                        var actionParams = new Dictionary<string, string>();
                        foreach (var param in actionData[1].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
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
}
