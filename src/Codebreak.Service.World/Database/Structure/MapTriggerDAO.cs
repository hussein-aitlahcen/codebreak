using Codebreak.Framework.Database;
using Codebreak.Service.World.Game.ActionEffect;
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
        private ActionList m_actions;

        /// <summary>
        /// 
        /// </summary>
        [Write(false)]
        public ActionList ActionsList
        {
            get
            {
                if (m_actions == null)
                {
                    m_actions = ActionList.Deserialize(Actions);
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
