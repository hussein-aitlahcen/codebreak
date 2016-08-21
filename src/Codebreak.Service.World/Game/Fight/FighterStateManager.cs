using Codebreak.Service.World.Game.Action;
using Codebreak.Service.World.Game.Fight.Effect;
using Codebreak.Service.World.Game.Spell;
using Codebreak.Service.World.Network;
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
    public enum FighterStateEnum
    {
        STATE_DRUNK = 1,
        STATE_CARRIER = 3,
        STATE_ROOTED = 6,
        STATE_GRAVITY = 7,
        STATE_CARRIED = 8,
        STATE_WEAKENED = 42,
        STATE_ALTRUISM = 50,
        STATE_STEALTH = 600,
    }
    
    /// <summary>
    /// 
    /// </summary>
    public sealed class FighterStateManager : IDisposable
    {
        private AbstractFighter m_fighter;
        private Dictionary<FighterStateEnum, AbstractSpellBuff> m_states = new Dictionary<FighterStateEnum, AbstractSpellBuff>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Fighter"></param>
        public FighterStateManager(AbstractFighter Fighter)
        {
            m_fighter = Fighter;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="State"></param>
        /// <returns></returns>
        public bool CanState(FighterStateEnum State)
        {
            switch (State)
            {
                case FighterStateEnum.STATE_CARRIED:
                case FighterStateEnum.STATE_CARRIER:
                    return !HasState(FighterStateEnum.STATE_GRAVITY);
            }

            return !HasState(State);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="State"></param>
        /// <returns></returns>
        public bool HasState(FighterStateEnum State)
        {
            return m_states.ContainsKey(State);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Buff"></param>
        public void AddState(AbstractSpellBuff Buff)        
        {
            Buff.CastInfos.SubEffect = EffectEnum.AddState;

            if (Buff.Caster.Fight.State == FightStateEnum.STATE_FIGHTING)
            {
                switch (Buff.CastInfos.EffectType)
                {
                    case EffectEnum.Stealth:

                        if (HasState(FighterStateEnum.STATE_STEALTH))
                            return;

                        m_fighter.Fight.Dispatch(WorldMessage.GAME_ACTION(EffectEnum.Stealth, m_fighter.Id, m_fighter.Id + "," + Buff.Duration));

                        m_states.Add(FighterStateEnum.STATE_STEALTH, Buff);

                        return;

                    default:

                        m_fighter.Fight.Dispatch(WorldMessage.GAME_ACTION(EffectEnum.AddState, m_fighter.Id, m_fighter.Id + "," + Buff.CastInfos.Value3 + ",1"));

                        break;
                }

                if (HasState((FighterStateEnum)Buff.CastInfos.Value3))
                    return;

                m_states.Add((FighterStateEnum)Buff.CastInfos.Value3, Buff);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Buff"></param>
        public void RemoveState(AbstractSpellBuff Buff)
        {
            if (Buff.Caster.Fight.State == FightStateEnum.STATE_FIGHTING)
            {
                switch (Buff.CastInfos.EffectType)
                {
                    case EffectEnum.Stealth:

                        m_fighter.Fight.Dispatch(WorldMessage.GAME_ACTION(EffectEnum.Stealth, m_fighter.Id, m_fighter.Id.ToString()));
                        m_fighter.Fight.Dispatch(WorldMessage.GAME_ACTION(GameActionTypeEnum.MAP_TELEPORT, m_fighter.Id, m_fighter.Id + "," + m_fighter.Cell.Id));

                        m_states.Remove(FighterStateEnum.STATE_STEALTH);

                        return;

                    default:

                        m_fighter.Fight.Dispatch(WorldMessage.GAME_ACTION(EffectEnum.AddState, m_fighter.Id, m_fighter.Id + "," + Buff.CastInfos.Value3 + ",0"));

                        break;
                }
            }

            m_states.Remove((FighterStateEnum)Buff.CastInfos.Value3);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public AbstractSpellBuff FindState(FighterStateEnum state)
        {
            if (HasState(state))
                return m_states[state];
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Clear()
        {
            foreach (var state in m_states.Values)
                state.RemoveEffect();

            m_states.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            m_states.Clear();
            m_states = null;
            m_fighter = null;
        }
    }
}
