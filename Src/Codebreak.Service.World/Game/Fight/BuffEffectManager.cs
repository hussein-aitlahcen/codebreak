using Codebreak.Service.World.Game.Fight.Effect;
using Codebreak.Service.World.Game.Spell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Game.Fight
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class BuffEffectManager
    {
        /// <summary>
        /// 
        /// </summary>
        private FighterBase _fighter;

        /// <summary>
        ///
        /// </summary>
        private Dictionary<ActiveType, List<BuffBase>> ActiveBuffs = new Dictionary<ActiveType, List<BuffBase>>()
        {
            { ActiveType.ACTIVE_ATTACKED_AFTER_JET, new List<BuffBase>() },
            { ActiveType.ACTIVE_ATTACKED_POST_JET, new List<BuffBase>() },
            { ActiveType.ACTIVE_ATTACK_AFTER_JET, new List<BuffBase>() },
            { ActiveType.ACTIVE_ATTACK_POST_JET, new List<BuffBase>() },
            { ActiveType.ACTIVE_BEGINTURN, new List<BuffBase>() },
            { ActiveType.ACTIVE_ENDTURN, new List<BuffBase>() },
            { ActiveType.ACTIVE_ENDMOVE, new List<BuffBase>() },
            { ActiveType.ACTIVE_STATS, new List<BuffBase>() },
        };

        /// <summary>
        /// 
        /// </summary>
        private Dictionary<DecrementType, List<BuffBase>> DecrementBuffs = new Dictionary<DecrementType, List<BuffBase>>()
        {
            { DecrementType.TYPE_BEGINTURN, new List<BuffBase>()},
            { DecrementType.TYPE_ENDTURN, new List<BuffBase>()},
            { DecrementType.TYPE_ENDMOVE, new List<BuffBase>()},
        };

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fighter"></param>
        public BuffEffectManager(FighterBase fighter)
        {
            _fighter = fighter;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Buff"></param>
        public void AddBuff(BuffBase Buff)
        {
            ActiveBuffs[Buff.ActiveType].Add(Buff);
            DecrementBuffs[Buff.DecrementType].Add(Buff);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="match"></param>
        public void RemoveState(int state)
        {
            BuffBase stateBuff = null;

            foreach (var buffList in ActiveBuffs.Values)
            {
                stateBuff = buffList.Find(buff => buff.CastInfos.SubEffect == EffectEnum.AddState && buff.CastInfos.Value3 == state);
                if (stateBuff != null)
                {
                    stateBuff.RemoveEffect();

                    RemoveBuff(stateBuff);

                    return;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void RemoveStealth()
        {
            BuffBase stealthBuff = null;

            foreach (var buffList in ActiveBuffs.Values)
            {
                stealthBuff = buffList.Find(buff => buff.CastInfos.EffectType == EffectEnum.Stealth);
                if (stealthBuff != null)
                {
                    stealthBuff.RemoveEffect();

                    RemoveBuff(stealthBuff);

                    return;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="match"></param>
        public void RemoveSkin()
        {
            BuffBase skinBuff = null;

            foreach (var buffList in ActiveBuffs.Values)
            {
                skinBuff = buffList.Find(buff => buff.CastInfos.EffectType == EffectEnum.ChangeSkin);
                if (skinBuff != null)
                {
                    skinBuff.RemoveEffect();

                    RemoveBuff(skinBuff);

                    return;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buff"></param>
        public void RemoveBuff(BuffBase buff)
        {
            ActiveBuffs[buff.ActiveType].Remove(buff);
            DecrementBuffs[buff.DecrementType].Remove(buff);
        }

        /// <summary>
        /// 
        /// </summary>
        public FightActionResultEnum BeginTurn()
        {
            var damage = 0;
            foreach (var buff in ActiveBuffs[ActiveType.ACTIVE_BEGINTURN].ToArray())
            {
                var result = buff.ApplyEffect(ref damage);
                if(result != FightActionResultEnum.RESULT_NOTHING)
                {
                    return result;
                }
            }            

            foreach (var buff in DecrementBuffs[DecrementType.TYPE_BEGINTURN].ToArray())
            {
                if (buff.DecrementDuration() <= 0)
                {
                    var result = buff.RemoveEffect();
                    if(result != FightActionResultEnum.RESULT_NOTHING)
                    {
                        return result;
                    }
                    DecrementBuffs[DecrementType.TYPE_BEGINTURN].Remove(buff);
                }
            }

            foreach (var buffList in ActiveBuffs.Values)
                buffList.RemoveAll(Buff => Buff.DecrementType == DecrementType.TYPE_BEGINTURN && Buff.Duration <= 0);

            return _fighter.Fight.TryKillFighter(_fighter, _fighter.Id);
        }

        /// <summary>
        /// 
        /// </summary>
        public FightActionResultEnum EndTurn()
        {
            var damage = 0;
            foreach (var buff in ActiveBuffs[ActiveType.ACTIVE_ENDTURN].ToArray())
            {
                if (buff.ApplyEffect(ref damage) == FightActionResultEnum.RESULT_END)
                {
                    return FightActionResultEnum.RESULT_END;
                }
            }

            foreach (var buff in DecrementBuffs[DecrementType.TYPE_ENDTURN].ToArray())
            {
                if (buff.DecrementDuration() <= 0)
                {
                    if (buff.RemoveEffect() == FightActionResultEnum.RESULT_END)
                    {
                        return FightActionResultEnum.RESULT_END;
                    }                    
                    DecrementBuffs[DecrementType.TYPE_ENDTURN].Remove(buff);
                }
            }

            foreach (var buffList in ActiveBuffs.Values)
                buffList.RemoveAll(Buff => Buff.DecrementType == DecrementType.TYPE_ENDTURN && Buff.Duration <= 0);

            return _fighter.Fight.TryKillFighter(_fighter, _fighter.Id);
        }

        /// <summary>
        /// 
        /// </summary>
        public FightActionResultEnum EndMove()
        {
            var damage = 0;
            foreach (var buff in ActiveBuffs[ActiveType.ACTIVE_ENDMOVE].ToArray())
            {
                var result = buff.ApplyEffect(ref damage);
                if(result != FightActionResultEnum.RESULT_NOTHING)
                {
                    return result;
                }
            }

            ActiveBuffs[ActiveType.ACTIVE_ENDMOVE].RemoveAll(buff => buff.DecrementType == DecrementType.TYPE_ENDMOVE && buff.Duration <= 0);

            return _fighter.Fight.TryKillFighter(_fighter, _fighter.Id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="castInfos"></param>
        /// <param name="damageValue"></param>
        public FightActionResultEnum OnAttackPostJet(CastInfos castInfos, ref int damageValue)
        {
            foreach (var buff in ActiveBuffs[ActiveType.ACTIVE_ATTACK_POST_JET].ToArray())
            {
                var result = buff.ApplyEffect(ref damageValue, castInfos);
                if(result != FightActionResultEnum.RESULT_NOTHING)
                {
                    return result;
                }
            }

            return _fighter.Fight.TryKillFighter(_fighter, _fighter.Id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CastInfos"></param>
        /// <param name="DamageValue"></param>
        public FightActionResultEnum OnAttackAfterJet(CastInfos castInfos, ref int damageValue)
        {
            foreach (var buff in ActiveBuffs[ActiveType.ACTIVE_ATTACK_AFTER_JET].ToArray())
            {
                var result = buff.ApplyEffect(ref damageValue, castInfos);
                if(result != FightActionResultEnum.RESULT_NOTHING)
                {
                    return result;
                }
            }

            return _fighter.Fight.TryKillFighter(_fighter, _fighter.Id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="castInfos"></param>
        /// <param name="damageValue"></param>
        public FightActionResultEnum OnAttackedPostJet(CastInfos castInfos, ref int damageValue)
        {
            foreach (var buff in ActiveBuffs[ActiveType.ACTIVE_ATTACKED_POST_JET].ToArray())
            {
                var result = buff.ApplyEffect(ref damageValue, castInfos);
                if(result !=  FightActionResultEnum.RESULT_NOTHING)
                    return result;
            }

            return _fighter.Fight.TryKillFighter(_fighter, _fighter.Id);
        }

        /// <summary>
        /// Subit des dommages, activation des buffs de reduction, renvois, anihilation des dommages apres le calcul du jet
        /// </summary>
        /// <param name="castInfos"></param>
        /// <param name="damageValue"></param>
        public FightActionResultEnum OnAttackedAfterJet(CastInfos castInfos, ref int damageValue)
        {
            foreach (var buff in ActiveBuffs[ActiveType.ACTIVE_ATTACKED_AFTER_JET].ToArray())
            {
                var result = buff.ApplyEffect(ref damageValue, castInfos);
                if(result != FightActionResultEnum.RESULT_NOTHING)
                    return result;
            }

            return _fighter.Fight.TryKillFighter(_fighter, _fighter.Id);
        }

        /// <summary>
        /// Debuff le personnage de tous les effets
        /// </summary>
        /// <returns></returns>
        public FightActionResultEnum Debuff()
        {
            foreach (var buff in DecrementBuffs[DecrementType.TYPE_BEGINTURN].ToArray())
                if (buff.IsDebuffable)
                {
                    var result = buff.RemoveEffect();
                    if(result != FightActionResultEnum.RESULT_NOTHING)
                        return result;
                }

            foreach (var buff in DecrementBuffs[DecrementType.TYPE_ENDTURN].ToArray())
                if (buff.IsDebuffable)
                {
                    var result = buff.RemoveEffect();
                    if(result != FightActionResultEnum.RESULT_NOTHING)
                        return result;
                }

            foreach (var buffList in ActiveBuffs.Values)
                buffList.RemoveAll(buff => buff.IsDebuffable);
            foreach (var buffList in DecrementBuffs.Values)
                buffList.RemoveAll(buff => buff.IsDebuffable);

            return _fighter.Fight.TryKillFighter(_fighter, _fighter.Id);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Clear()
        {
            foreach (var buff in DecrementBuffs[DecrementType.TYPE_BEGINTURN].ToArray())
                buff.RemoveEffect();

            foreach (var buff in DecrementBuffs[DecrementType.TYPE_ENDTURN].ToArray())
                buff.RemoveEffect();

            foreach (var buff in DecrementBuffs[DecrementType.TYPE_ENDMOVE].ToArray())
                buff.RemoveEffect();

            foreach (var buffList in ActiveBuffs.Values)
                buffList.Clear();
            foreach (var buffList in DecrementBuffs.Values)
                buffList.Clear();
        }
    }
}
