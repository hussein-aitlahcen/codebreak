﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Game.Fight.Effect.Type
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class PandaCarriedBuff : AbstractSpellBuff
    {
         /// <summary>
        /// 
        /// </summary>
        /// <param name="castInfos"></param>
        /// <param name="target"></param>
        public PandaCarriedBuff(CastInfos castInfos, AbstractFighter target)
            : base(castInfos, target, ActiveType.ACTIVE_ENDMOVE, DecrementType.TYPE_ENDMOVE)
        {
            Target.StateManager.AddState(this);

            Target.SetCell(Caster.Cell);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="damageValue"></param>
        /// <param name="damageInfos"></param>
        /// <returns></returns>
        public override FightActionResultEnum ApplyEffect(ref int damageValue, CastInfos damageInfos = null)
        {
            if (Caster.Cell.Id != Target.Cell.Id)
            {
                Target.BuffManager.RemoveState((int)FighterStateEnum.STATE_CARRIED);
                Caster.BuffManager.RemoveState((int)FighterStateEnum.STATE_CARRIER);

                Duration = 0;
            }

            return FightActionResultEnum.RESULT_NOTHING;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override FightActionResultEnum RemoveEffect()
        {
            Target.StateManager.RemoveState(this);

            return base.RemoveEffect();
        }
    }
}
