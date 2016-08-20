using Codebreak.Framework.Database;
using Codebreak.Service.World.Game.ActionEffect;
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
    }
}
