using Codebreak.Service.World.Game.Action;
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
    public sealed class HealEffect : EffectBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="CastInfos"></param>
        /// <returns></returns>
        public override FightActionResultEnum ApplyEffect(CastInfos castInfos)
        {
            if (castInfos.Target == null)
                return FightActionResultEnum.RESULT_NOTHING;

            // Si > 0 alors c'est un buff
            if (castInfos.Duration > 0)
            {
                castInfos.Target.BuffManager.AddBuff(new HealBuff(castInfos, castInfos.Target));
            }
            else // Heal direct
            {
                var healValue = castInfos.RandomJet;
                return HealEffect.ApplyHeal(castInfos, castInfos.Target, ref healValue);
            }

            return FightActionResultEnum.RESULT_NOTHING;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="CastInfos"></param>
        /// <param name="Heal"></param>
        /// <returns></returns>
        public static FightActionResultEnum ApplyHeal(CastInfos castInfos, FighterBase target, ref int heal)
        {
            var caster = castInfos.Caster;

            if(castInfos.EffectType != EffectEnum.DamageBrut)
                caster.CalculHeal(ref heal);

            if (target.Life + heal > target.MaxLife)
                heal = target.MaxLife - target.Life;

            target.Life += heal;

            castInfos.Fight.Dispatch(WorldMessage.GAME_ACTION(GameActionTypeEnum.FIGHT_HEAL, caster.Id, target.Id + "," + heal));

            return castInfos.Fight.TryKillFighter(target, caster.Id);
        }
    }
}
