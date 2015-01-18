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
    public sealed class StatsBuff : BuffBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="castInfos"></param>
        /// <param name="target"></param>
        public StatsBuff(CastInfos castInfos, FighterBase target)
            : base(castInfos, target, ActiveType.ACTIVE_STATS, DecrementType.TYPE_ENDTURN)
        {
        }

        /// <summary>
        /// Ajout le boost
        /// </summary>
        /// <param name="damageValue"></param>
        /// <param name="damageInfos"></param>
        /// <returns></returns>
        public override FightActionResultEnum ApplyEffect(ref int damageValue, CastInfos damageInfos = null)
        {
            int showValue;

            switch (CastInfos.EffectType)
            {
                case EffectEnum.SubAP:
                case EffectEnum.SubMP:
                case EffectEnum.SubAPDodgeable:
                case EffectEnum.SubMPDodgeable:
                    showValue = -CastInfos.Value1;
                    break;

                default:
                    showValue = CastInfos.Value1;
                    break;
            }

            if (CastInfos.EffectType != EffectEnum.ReflectSpell)
                Target.Fight.Dispatch(WorldMessage.GAME_ACTION(CastInfos.EffectType, Target.Id, Target.Id + "," + showValue + "," + Duration));

            Target.Statistics.AddDon(CastInfos.EffectType, CastInfos.Value1);

            return base.ApplyEffect(ref damageValue, damageInfos);
        }

        /// <summary>
        /// Retire le boost
        /// </summary>
        /// <returns></returns>
        public override FightActionResultEnum RemoveEffect()
        {
            Target.Statistics.GetEffect(CastInfos.EffectType).Boosts -= CastInfos.Value1;

            return base.RemoveEffect();
        }
    }
}
