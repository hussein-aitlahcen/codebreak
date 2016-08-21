using Codebreak.Service.World.Game.Spell;
using Codebreak.Service.World.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Game.Fight.Effect.Type
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class BuffRemoveEffect : AbstractSpellEffect
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="CastInfos"></param>
        /// <returns></returns>
        public override FightActionResultEnum ApplyEffect(CastInfos CastInfos)
        {
            if (CastInfos.Target == null)
                return FightActionResultEnum.RESULT_NOTHING;

            if (CastInfos.Target.BuffManager.Debuff() == FightActionResultEnum.RESULT_END)
                return FightActionResultEnum.RESULT_END;

            CastInfos.Target.Fight.Dispatch(WorldMessage.GAME_ACTION(EffectEnum.DeleteAllBonus, CastInfos.Target.Id, CastInfos.Target.Id.ToString()));

            return FightActionResultEnum.RESULT_NOTHING;
        }
    }
}
