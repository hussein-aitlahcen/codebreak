using Codebreak.Service.World.Game.Map;
using Codebreak.Service.World.Game.Spell;
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
    public sealed class DamageDodgeBuff : BuffBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="castInfos"></param>
        /// <param name="target"></param>
        public DamageDodgeBuff(CastInfos castInfos, FighterBase target)
            : base(castInfos, target, ActiveType.ACTIVE_ATTACKED_AFTER_JET, DecrementType.TYPE_ENDTURN)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="DamageValue"></param>
        /// <param name="damageInfos"></param>
        /// <returns></returns>
        public override FightActionResultEnum ApplyEffect(ref int damageValue, CastInfos damageInfos = null)
        {
            if (!damageInfos.IsMelee)
                return FightActionResultEnum.RESULT_NOTHING;

            damageValue = 0; // Annihilation des dommages;

            // cannot apply push if target move with last effects
            if (Target.Cell.Id != damageInfos.TargetKnownCellId)
                return FightActionResultEnum.RESULT_NOTHING;

            var subInfos = new CastInfos(EffectEnum.PushBack, 0, 0, 0, 0, 0, 0, 0, damageInfos.Caster, null);
            var direction = Pathfinding.GetDirection(Target.Fight.Map, damageInfos.Caster.Cell.Id, Target.Cell.Id);

            // Application du push
            return PushEffect.ApplyPush(subInfos, Target, direction, 1);
        }
    }
}
