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
    public sealed class TransposeEffect : EffectBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="CastInfos"></param>
        /// <returns></returns>
        public override FightActionResultEnum ApplyEffect(CastInfos CastInfos)
        {
            if (CastInfos.Target == null)
                return FightActionResultEnum.RESULT_NOTHING;

            if (CastInfos.SpellId == 445)
            {
                if (CastInfos.Target.Team == CastInfos.Caster.Team)
                    return FightActionResultEnum.RESULT_NOTHING;
            }
            else if (CastInfos.SpellId == 438)
            {
                if (CastInfos.Target.Team != CastInfos.Caster.Team)
                    return FightActionResultEnum.RESULT_NOTHING;
            }

            var TargetTeleport = new CastInfos(EffectEnum.Teleport, CastInfos.SpellId, CastInfos.Caster.Cell.Id, 0, 0, 0, 0, 0, CastInfos.Target, null);
            var CasterTeleport = new CastInfos(EffectEnum.Teleport, CastInfos.SpellId, CastInfos.Target.Cell.Id, 0, 0, 0, 0, 0, CastInfos.Caster, null);

            CastInfos.Caster.SetCell(null);
            CastInfos.Target.SetCell(null);

            if (TeleportEffect.ApplyTeleport(TargetTeleport) == FightActionResultEnum.RESULT_END)
                return FightActionResultEnum.RESULT_END;

            if (TeleportEffect.ApplyTeleport(CasterTeleport) == FightActionResultEnum.RESULT_END)
                return FightActionResultEnum.RESULT_END;

            return FightActionResultEnum.RESULT_NOTHING;
        }
    }
}
