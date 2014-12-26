using Codebreak.Service.World.Game.Action;
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
        private FighterBase _fighter;
        private Dictionary<FighterStateEnum, BuffBase> _state = new Dictionary<FighterStateEnum, BuffBase>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Fighter"></param>
        public FighterStateManager(FighterBase Fighter)
        {
            _fighter = Fighter;
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
            return _state.ContainsKey(State);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Buff"></param>
        public void AddState(BuffBase Buff)        
        {
            Buff.CastInfos.SubEffect = EffectEnum.AddState;

            if (Buff.Caster.Fight.State == FightStateEnum.STATE_FIGHTING)
            {
                switch (Buff.CastInfos.EffectType)
                {
                    case EffectEnum.Stealth:

                        if (HasState(FighterStateEnum.STATE_STEALTH))
                            return;

                        _fighter.Fight.Dispatch(WorldMessage.GAME_ACTION(EffectEnum.Stealth, _fighter.Id, _fighter.Id + "," + Buff.Duration));

                        _state.Add(FighterStateEnum.STATE_STEALTH, Buff);

                        return;

                    default:

                        _fighter.Fight.Dispatch(WorldMessage.GAME_ACTION(EffectEnum.AddState, _fighter.Id, _fighter.Id + "," + Buff.CastInfos.Value3 + ",1"));

                        break;
                }

                if (HasState((FighterStateEnum)Buff.CastInfos.Value3))
                    return;

                _state.Add((FighterStateEnum)Buff.CastInfos.Value3, Buff);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Buff"></param>
        public void RemoveState(BuffBase Buff)
        {
            if (Buff.Caster.Fight.State == FightStateEnum.STATE_FIGHTING)
            {
                switch (Buff.CastInfos.EffectType)
                {
                    case EffectEnum.Stealth:

                        _fighter.Fight.Dispatch(WorldMessage.GAME_ACTION(EffectEnum.Stealth, _fighter.Id, _fighter.Id.ToString()));
                        _fighter.Fight.Dispatch(WorldMessage.GAME_ACTION(GameActionTypeEnum.MAP_TELEPORT, _fighter.Id, _fighter.Id + "," + _fighter.Cell.Id));

                        _state.Remove(FighterStateEnum.STATE_STEALTH);

                        return;

                    default:

                        _fighter.Fight.Dispatch(WorldMessage.GAME_ACTION(EffectEnum.AddState, _fighter.Id, _fighter.Id + "," + Buff.CastInfos.Value3 + ",0"));

                        break;
                }
            }

            _state.Remove((FighterStateEnum)Buff.CastInfos.Value3);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public BuffBase FindState(FighterStateEnum state)
        {
            if (HasState(state))
                return _state[state];
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Clear()
        {
            foreach (var state in _state.Values)
                state.RemoveEffect();

            _state.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            _state.Clear();
            _state = null;
            _fighter = null;
        }
    }
}
