using Codebreak.WorldService.World.Spell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.WorldService.World.Fight.Effect.Type
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class SkinChangeEffect : EffectBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="castInfos"></param>
        /// <returns></returns>
        public override FightActionResultEnum ApplyEffect(CastInfos castInfos)
        {
            if (castInfos.Target == null)
                return FightActionResultEnum.RESULT_NOTHING;

            if (castInfos.Duration > 0)
            {
                castInfos.Target.BuffManager.AddBuff(new SkinChangeBuff(castInfos, castInfos.Target));
            }
            else
            {
                return ApplySkinChange(castInfos);
            }

            return FightActionResultEnum.RESULT_NOTHING;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="castInfos"></param>
        /// <returns></returns>
        public static FightActionResultEnum ApplySkinChange(CastInfos castInfos)
        {
            if (castInfos.Value1 == -1 && castInfos.Value2 == -1 && castInfos.Value3 == -1)
            {
                castInfos.Target.BuffManager.RemoveSkin();
            }
            else
            {
                var currentSkin = castInfos.Target.Skin;
                var newSkin = castInfos.Value3 == -1 ? currentSkin : castInfos.Value3;

                castInfos.Value3 = castInfos.Target.Skin;
                castInfos.Target.Skin = newSkin;
                castInfos.Target.Fight.Dispatch(WorldMessage.GAME_ACTION(EffectEnum.ChangeSkin, castInfos.Caster.Id, castInfos.Target.Id + "," + currentSkin + "," + newSkin + "," + castInfos.Duration + 1));
            }

            return FightActionResultEnum.RESULT_NOTHING;
        }
    }
}
