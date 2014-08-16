using Codebreak.WorldService.World.Fight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.WorldService.World.Action
{
    public abstract class GameFightActionBase : GameActionBase
    {
        /// <summary>
        /// 
        /// </summary>
        public override bool CanAbort
        {
            get 
            {
                return false;
            }
        }

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
        public FighterBase Fighter
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
        public GameFightActionBase(GameActionTypeEnum type, FighterBase fighter, long duration)
            : base(type, fighter)
        {
            Fighter = fighter;
            Timeout = Fighter.Fight.UpdateTime + duration;
        }
    }
}
