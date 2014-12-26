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
        /// 
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
        ///
        /// </summary>
        /// <param name="CastInfos"></param>
        /// <param name="damageJet"></param>
        public static FightActionResultEnum ApplyDamages(CastInfos castInfos, FighterBase target, ref int damageJet)
        {
            var caster = castInfos.Caster;

            // caster stealth goes out of dealing direct damages
            if (!castInfos.IsPoison && !castInfos.IsTrap && !castInfos.IsReflect && caster.StateManager.HasState(FighterStateEnum.STATE_STEALTH))
                caster.BuffManager.RemoveStealth();

            // poison and reflected damages cannot triggers on hit effect
            if (!castInfos.IsPoison && !castInfos.IsReflect)
            {
                if (caster.BuffManager.OnAttackPostJet(castInfos, ref damageJet) == FightActionResultEnum.RESULT_END)
                    return FightActionResultEnum.RESULT_END; // check out end of fight

                if (target.BuffManager.OnAttackedPostJet(castInfos, ref damageJet) == FightActionResultEnum.RESULT_END)
                    return FightActionResultEnum.RESULT_END; // check out end of fight
            }

            // caster statistics increase damages
            caster.CalculDamages(castInfos.EffectType, ref damageJet);

            // target statistics reduce incomming damages
            target.CalculReduceDamages(castInfos.EffectType, ref damageJet);

            // target armor reduce damages
            if (damageJet > 0)
            {
                // target armor cannot reduce poison or reflect
                if (!castInfos.IsPoison && !castInfos.IsReflect)
                {
                    // Calcul de l'armure par rapport a l'effet
                    var armor = target.CalculArmor(castInfos.EffectType);

                    // if the target has some armor
                    if (armor != 0)
                    {
                        castInfos.Fight.Dispatch(WorldMessage.GAME_ACTION(GameActionTypeEnum.FIGHT_ARMOR, target.Id, target.Id + "," + armor));

                        damageJet -= armor;

                        if (damageJet < 0)
                            damageJet = 0;
                    }
                }
            }

            // poison and reflected damages cannot triggers on hit effect after jets and armor
            if (!castInfos.IsPoison && !castInfos.IsReflect)
            {
                if (caster.BuffManager.OnAttackAfterJet(castInfos, ref damageJet) == FightActionResultEnum.RESULT_END)
                    return FightActionResultEnum.RESULT_END; // check out end of fight
                if (target.BuffManager.OnAttackedAfterJet(castInfos, ref damageJet) == FightActionResultEnum.RESULT_END)
                    return FightActionResultEnum.RESULT_END; // check out end of fight
            }

            // check out damages after all calculations
            if (damageJet > 0)
            {
                // poison and reflected damages cannot be reflected
                if (!castInfos.IsPoison && !castInfos.IsReflect && castInfos.EffectType != EffectEnum.DamageBrut)
                {
                    var reflectDamage = target.ReflectDamage;

                    // check out if he has some reflection
                    if (reflectDamage > 0)
                    {
                        target.Fight.Dispatch(WorldMessage.GAME_ACTION(EffectEnum.AddReflectDamage, target.Id, target.Id + "," + reflectDamage));

                        // too much reflect
                        if (reflectDamage > damageJet)
                            reflectDamage = damageJet;

                        var subInfos = new CastInfos(castInfos.EffectType, 0, 0, 0, 0, 0, 0, 0, target, null);
                        subInfos.IsReflect = true;

                        // Si le renvoi de dommage entraine la fin de combat on stop
                        if (DamageEffect.ApplyDamages(subInfos, caster, ref reflectDamage) == FightActionResultEnum.RESULT_END)
                            return FightActionResultEnum.RESULT_END;

                        // Dommage renvoyé
                        damageJet -= reflectDamage;
                    }
                }
            }

            // cannot be negative
            if (damageJet < 0)
                damageJet = 0;

            // more damages than target life
            if (damageJet > target.Life)
                damageJet = target.Life;

            // recude life
            target.Life -= damageJet;

            // display damages
            castInfos.Fight.Dispatch(WorldMessage.GAME_ACTION(GameActionTypeEnum.FIGHT_DAMAGE, caster.Id, target.Id + "," + (-damageJet).ToString()));

            // check out the death
            return castInfos.Fight.TryKillFighter(target, caster.Id);
        }
    }
}
