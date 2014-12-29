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
    public sealed class PandaCarrierBuff : BuffBase
    { 
        /// <summary>
        /// 
        /// </summary>
        /// <param name="castInfos"></param>
        /// <param name="target"></param>
        public PandaCarrierBuff(CastInfos castInfos, FighterBase target)
            : base(castInfos, target, ActiveType.ACTIVE_ENDMOVE, DecrementType.TYPE_ENDMOVE)
        {
            Caster.StateManager.AddState(this);

            castInfos.Caster.Fight.Dispatch(WorldMessage.GAME_ACTION(EffectEnum.PandaCarrier, castInfos.Caster.Id, target.Id.ToString()));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="DamageValue"></param>
        /// <param name="DamageInfos"></param>
        /// <returns></returns>
        public override FightActionResultEnum ApplyEffect(ref int DamageValue, CastInfos DamageInfos = null)
        {
            // Si effet finis
            if (!Target.StateManager.HasState(FighterStateEnum.STATE_CARRIED))
            {
                Duration = 0;
                return FightActionResultEnum.RESULT_NOTHING;
            }

            // On affecte la meme cell pour la cible porté
            return Target.SetCell(Caster.Cell);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override FightActionResultEnum RemoveEffect()
        {
            Caster.StateManager.RemoveState(this);

            return base.RemoveEffect();
        }
    }
}
