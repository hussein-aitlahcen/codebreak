using Codebreak.Service.World.Game.Fight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Game.Action
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class AbstractGameFightAction : AbstractGameAction
    {
        /// <summary>
        /// 
        /// </summary>
        public override bool CanAbort => false;

        /// <summary>
        /// 
        /// </summary>
        public long Timeout
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public AbstractFighter Fighter
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="fighter"></param>
        /// <param name="timeout"></param>
        public AbstractGameFightAction(GameActionTypeEnum type, AbstractFighter fighter, long duration)
            : base(type, fighter, duration)
        {
            Fighter = fighter;
            Timeout = Fighter.Fight.UpdateTime + duration;
        }
    }
}
