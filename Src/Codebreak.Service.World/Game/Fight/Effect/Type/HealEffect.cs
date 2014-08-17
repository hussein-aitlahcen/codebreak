using Codebreak.Service.World.Game.Action;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Game.Fight.Effect.Type
{
    /// <summary>
    /// Classe de gestion des soins
    /// </summary>
    public sealed class HealEffect : EffectBase
    {
        /// <summary>
        /// Application de l'effet, buff ou directe
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
                castInfos.Target.BuffManager.AddBuff(new DamageBuff(castInfos, castInfos.Target));
            }
            else // Heal direct
            {
                var healValue = castInfos.RandomJet;
                return HealEffect.ApplyHeal(castInfos, castInfos.Target, ref healValue);
            }

            return FightActionResultEnum.RESULT_NOTHING;
        }

        /// <summary>
        /// Application du heal
        /// </summary>
        /// <param name="CastInfos"></param>
        /// <param name="Heal"></param>
        /// <returns></returns>
        public static FightActionResultEnum ApplyHeal(CastInfos castInfos, FighterBase target, ref int Heal)
        {
            var caster = castInfos.Caster;

            // Boost soin etc
            caster.CalculHeal(ref Heal);

            // Si le soin est superieur a sa vie actuelle
            if (target.Life + Heal > target.MaxLife)
                Heal = target.MaxLife - target.Life;

            // Affectation
            target.Life += Heal;

            // Envoi du packet
            target.Fight.Dispatch(WorldMessage.GAME_ACTION(GameActionTypeEnum.FIGHT_HEAL, caster.Id, target.Id + "," + Heal));

            // Le soin entraine la fin du combat ? lol on test quand même :D
            return target.Fight.TryKillFighter(target, caster.Id);
        }
    }
}
