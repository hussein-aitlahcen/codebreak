using Codebreak.Framework.Database;
using Codebreak.Service.World.Game.Condition;
using Codebreak.Service.World.Game.Entity;
using Codebreak.Service.World.Game.Spell;
using System;
using System.Collections.Generic;

namespace Codebreak.Service.World.Database.Structure
{
    /// <summary>
    /// 
    /// </summary>
    [Table("maptrigger")]
    public sealed class MapTriggerDAO : DataAccessObject<MapTriggerDAO>
    {
        public int MapId
        {
            get;
            set;
        }
        public int CellId
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <returns></returns>
        public bool SatisfyConditions(CharacterEntity character)
        {
            return ConditionParser.Instance.Check(Conditions, character);
        }
    }
}
