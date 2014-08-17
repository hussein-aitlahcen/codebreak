using Codebreak.Service.World.Game.Action;
using Codebreak.Service.World.Game.Spell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Game.Fight.Effect.Type
{
    /// <summary>
    /// Classe de gestion des dommages terre
    /// </summary>
    public sealed class DamageEffect : EffectBase
    {
        /// <summary>
        /// Applique l'effet, buff ou direct
        /// </summary>
        /// <param name="castInfos"></param>
        /// <returns></returns>
        public override FightActionResultEnum ApplyEffect(CastInfos castInfos)
        {
            if (castInfos.Target == null)
                return FightActionResultEnum.RESULT_NOTHING;

            // Si > 0 alors c'est un buff
            if (castInfos.Duration > 0)
            {
                // L'effet est un poison
                castInfos.IsPoison = true;

                castInfos.Target.BuffManager.AddBuff(new DamageBuff(castInfos, castInfos.Target));
            }
            else // Dommage direct
            {
                var damageValue = castInfos.RandomJet;

                return DamageEffect.ApplyDamages(castInfos, castInfos.Target, ref damageValue);
            }

            return FightActionResultEnum.RESULT_NOTHING;
        }


        /// <summary>
        /// Applique les dommages.
        /// </summary>
        /// <param name="CastInfos"></param>
        /// <param name="damageJet"></param>
        public static FightActionResultEnum ApplyDamages(CastInfos castInfos, FighterBase target, ref int damageJet)
        {
            var caster = castInfos.Caster;

            // Perd l'invisibilité s'il inflige des dommages direct
            if (!castInfos.IsPoison && !castInfos.IsTrap && !castInfos.IsReflect && caster.StateManager.HasState(FighterStateEnum.STATE_STEALTH))
                caster.BuffManager.RemoveStealth();

            // Application des buffs avant calcul totaux des dommages, et verification qu'ils n'entrainent pas la fin du combat
            if (!castInfos.IsPoison && !castInfos.IsReflect)
            {
                if (caster.BuffManager.OnAttackPostJet(castInfos, ref damageJet) == FightActionResultEnum.RESULT_END)
                    return FightActionResultEnum.RESULT_END; // Fin du combat

                if (target.BuffManager.OnAttackedPostJet(castInfos, ref damageJet) == FightActionResultEnum.RESULT_END)
                    return FightActionResultEnum.RESULT_END; // Fin du combat
            }

            // Calcul jet
            caster.CalculDamages(castInfos.EffectType, ref damageJet);

            // Calcul resistances
            target.CalculReduceDamages(castInfos.EffectType, ref damageJet);

            // Reduction des dommages grace a l'armure
            if (damageJet > 0)
            {
                // Si ce n'est pas des dommages direct on ne reduit pas
                if (!castInfos.IsPoison && !castInfos.IsReflect)
                {
                    // Calcul de l'armure par rapport a l'effet
                    var Armor = target.CalculArmor(castInfos.EffectType);

                    // Si il reduit un minimum
                    if (Armor != 0)
                    {
                        // XX Reduit les dommages de X
                        target.Fight.Dispatch(WorldMessage.GAME_ACTION(GameActionTypeEnum.FIGHT_ARMOR, target.Id, target.Id + "," + Armor));

                        // On reduit
                        damageJet -= Armor;

                        // Si on suprimme totalement les dommages
                        if (damageJet < 0)
                            damageJet = 0;
                    }
                }
            }

            // Application des buffs apres le calcul totaux et l'armure
            if (!castInfos.IsPoison && !castInfos.IsReflect)
            {
                if (caster.BuffManager.OnAttackAfterJet(castInfos, ref damageJet) == FightActionResultEnum.RESULT_END)
                    return FightActionResultEnum.RESULT_END; // Fin du combat
                if (target.BuffManager.OnAttackedAfterJet(castInfos, ref damageJet) == FightActionResultEnum.RESULT_END)
                    return FightActionResultEnum.RESULT_END; // Fin du combat
            }

            // S'il subit des dommages
            if (damageJet > 0)
            {
                // Si c'est pas un poison ou un renvoi on applique le renvoie
                if (!castInfos.IsPoison && !castInfos.IsReflect)
                {
                    var reflectDamage = target.ReflectDamage;

                    // Si du renvoi
                    if (reflectDamage > 0)
                    {
                        target.Fight.Dispatch(WorldMessage.GAME_ACTION(EffectEnum.AddReflectDamage, target.Id, target.Id + "," + reflectDamage));

                        // Trop de renvois
                        if (reflectDamage > damageJet)
                            reflectDamage = damageJet;

                        var subInfos = new CastInfos(EffectEnum.DamageBrut, 0, 0, 0, 0, 0, 0, 0, target, null);
                        subInfos.IsReflect = true;

                        // Si le renvoi de dommage entraine la fin de combat on stop
                        if (DamageEffect.ApplyDamages(subInfos, caster, ref reflectDamage) == FightActionResultEnum.RESULT_END)
                            return FightActionResultEnum.RESULT_END;

                        // Dommage renvoyé
                        damageJet -= reflectDamage;
                    }
                }
            }

            // Peu pas etre en dessous de 0
            if (damageJet < 0) damageJet = 0;

            // Dommages superieur a la vie de la cible
            if (damageJet > target.Life)
                damageJet = target.Life;

            // Deduit la vie
            target.Life -= damageJet;

            // Enois du packet combat subit des dommages
            target.Fight.Dispatch(WorldMessage.GAME_ACTION(GameActionTypeEnum.FIGHT_DAMAGE, caster.Id, target.Id + "," + (-damageJet).ToString()));

            // Tentative de mort et fin de combat
            return caster.Fight.TryKillFighter(target, caster.Id);
        }
    }
}
