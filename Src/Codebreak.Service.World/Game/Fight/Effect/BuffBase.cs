using Codebreak.Service.World.Game.Spell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Game.Fight.Effect
{
    /// <summary>
    /// 
    /// </summary>
    public enum ActiveType
    {
        ACTIVE_STATS,
        ACTIVE_BEGINTURN,
        ACTIVE_ENDTURN,
        ACTIVE_ENDMOVE,
        ACTIVE_ATTACK_POST_JET,
        ACTIVE_ATTACK_AFTER_JET,
        ACTIVE_ATTACKED_POST_JET,
        ACTIVE_ATTACKED_AFTER_JET,
    }

    /// <summary>
    /// 
    /// </summary>
    public enum DecrementType
    {
        TYPE_BEGINTURN,
        TYPE_ENDTURN,
        TYPE_ENDMOVE,
    }

    /// <summary>
    /// 
    /// </summary>
    public abstract class BuffBase
    {
        /// <summary>
        ///
        /// </summary>
        public DecrementType DecrementType
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public ActiveType ActiveType
        {
            get;
            set;
        }

        /// <summary>
        //
        /// </summary>
        public CastInfos CastInfos
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public FighterBase Caster
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public FighterBase Target
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int Duration
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public bool IsDebuffable
        {
            get
            {
                switch (CastInfos.SubEffect)
                {
                    case EffectEnum.AddState:
                    case EffectEnum.ChangeSkin:
                    case EffectEnum.AddChatiment:
                        return false;
                }

                return true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Effect"></param>
        public BuffBase(CastInfos castInfos, FighterBase target, ActiveType activeType, DecrementType decrementType)
        {
            CastInfos = castInfos;
            Duration = target.Fight.CurrentFighter == target ? castInfos.Duration + 1 : castInfos.Duration;
            Caster = castInfos.Caster;
            Target = target;

            ActiveType = activeType;
            DecrementType = decrementType;

            switch (castInfos.EffectType)
            {
                case EffectEnum.ReflectSpell:
                    Target.Fight.Dispatch(WorldMessage.FIGHT_EFFECT_INFORMATION(CastInfos.EffectType,
                                                                               Target.Id,
                                                                               "-1",
                                                                               CastInfos.Value1.ToString(),
                                                                               "10",
                                                                               "",
                                                                               CastInfos.Duration.ToString(),
                                                                               CastInfos.SpellId.ToString()));
                    break;

                case EffectEnum.ChanceEcaflip:
                case EffectEnum.AddChatiment:
                    Target.Fight.Dispatch(WorldMessage.FIGHT_EFFECT_INFORMATION(CastInfos.EffectType,
                                                                           Target.Id,
                                                                           CastInfos.Value1.ToString(),
                                                                           CastInfos.Value2.ToString(),
                                                                           CastInfos.Value3.ToString(),
                                                                           "",
                                                                           CastInfos.Duration.ToString(),
                                                                           CastInfos.SpellId.ToString()));
                    break;

                case EffectEnum.PandaCarrier:
                    Caster.Fight.Dispatch(WorldMessage.FIGHT_EFFECT_INFORMATION(CastInfos.EffectType,
                                                                          Caster.Id,
                                                                          CastInfos.Value1.ToString(),
                                                                          "",
                                                                          "",
                                                                          "",
                                                                          CastInfos.Duration.ToString(),
                                                                          CastInfos.SpellId.ToString()));
                    break;

                default:
                    Target.Fight.Dispatch(WorldMessage.FIGHT_EFFECT_INFORMATION(CastInfos.EffectType,
                                                                               Target.Id,
                                                                               CastInfos.Value1.ToString(),
                                                                               "",
                                                                               "",
                                                                               "",
                                                                               CastInfos.Duration.ToString(),
                                                                               CastInfos.SpellId.ToString()));
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ActiveType"></param>
        public virtual FightActionResultEnum ApplyEffect(ref int damageValue, CastInfos damageInfos = null)
        {
            return Caster.Fight.TryKillFighter(Target, Caster.Id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual FightActionResultEnum RemoveEffect()
        {
            return Caster.Fight.TryKillFighter(Target, Caster.Id);
        }

        /// <summary>
        /// 
        /// </summary>
        public int DecrementDuration()
        {
            Duration--;

            CastInfos.FakeValue = 0;

            return Duration;
        }
    }
}
