using Codebreak.Service.World.Game.Action;
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
    public sealed class MPDodgeSubstractBuff : BuffBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="CastInfos"></param>
        /// <param name="Target"></param>
        public MPDodgeSubstractBuff(CastInfos CastInfos, FighterBase Target)
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
            var mpLost = CastInfos.Value1 > Target.MP ? Target.MP : CastInfos.Value1;
            CastInfos.Value1 = Target.CalculDodgeAPMP(CastInfos.Caster, mpLost, true);

            if (CastInfos.Value1 != mpLost)
            {
                Target.Fight.Dispatch(WorldMessage.GAME_ACTION(GameActionTypeEnum.FIGHT_DODGE_SUBPM, Target.Id, Target.Id + "," + (mpLost - CastInfos.Value1)));
            }

            if (CastInfos.Value1 > 0)
            {
                var buff = new StatsBuff(new CastInfos(CastInfos.EffectType, CastInfos.SpellId, CastInfos.SpellId, CastInfos.Value1, 0, 0, 0, Duration, CastInfos.Caster, null), Target);
                buff.ApplyEffect(ref mpLost);
                Target.BuffManager.AddBuff(buff);
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
