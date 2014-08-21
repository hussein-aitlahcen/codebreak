using Codebreak.Service.World.Game.Action;
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
    public sealed class APDodgeSubstractBuff : BuffBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="CastInfos"></param>
        /// <param name="Target"></param>
        public APDodgeSubstractBuff(CastInfos CastInfos, FighterBase Target)
            : base(CastInfos, Target, ActiveType.ACTIVE_STATS, DecrementType.TYPE_ENDTURN)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="damageValue"></param>
        /// <param name="damageInfos"></param>
        /// <returns></returns>
        public override FightActionResultEnum ApplyEffect(ref int damageValue, CastInfos damageInfos = null)
        {
            var apLost = CastInfos.Value1 > Target.AP ? Target.AP : CastInfos.Value1;
            CastInfos.Value1 = Target.CalculDodgeAPMP(CastInfos.Caster, apLost);

            if (CastInfos.Value1 != apLost)
            {
                Target.Fight.Dispatch(WorldMessage.GAME_ACTION(GameActionTypeEnum.FIGHT_DODGE_SUBPA, Target.Id, Target.Id + "," + (apLost - CastInfos.Value1)));
            }

            if (CastInfos.Value1 > 0)
            {
                var buffStats = new StatsBuff(new CastInfos(CastInfos.EffectType, CastInfos.SpellId, CastInfos.SpellId, CastInfos.Value1, 0, 0, 0, Duration, CastInfos.Caster, null), Target);
                buffStats.ApplyEffect(ref apLost);
                Target.BuffManager.AddBuff(buffStats);
            }

            return base.ApplyEffect(ref damageValue, damageInfos);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override FightActionResultEnum RemoveEffect()
        {
            return base.RemoveEffect();
        }
    }
}
