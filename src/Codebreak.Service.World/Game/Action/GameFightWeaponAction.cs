using Codebreak.Service.World.Game.Fight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Game.Action
{
    public sealed class GameFightWeaponAction : AbstractGameFightAction
    {
        /// <summary>
        /// 
        /// </summary>
        public System.Action Callback
        {
            get;
            private set;
        }
        
        /// <summary>
        /// 
        /// </summary>
        public int CellId
        {
            get;
            private set;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fighter"></param>
        /// <param name="callback"></param>
        public GameFightWeaponAction(AbstractFighter fighter, int cellId, long duration, System.Action callback)
            : base(GameActionTypeEnum.FIGHT_WEAPON_USE, fighter, duration)
        {
            Callback = callback;
            CellId = cellId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public override void Stop(params object[] args)
        {
            Callback();

            base.Stop(args);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string SerializeAs_GameAction()
        {
            return CellId.ToString();
        }
    }
}
