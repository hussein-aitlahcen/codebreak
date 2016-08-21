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
    public sealed class ActivableObjectEffect : AbstractSpellEffect
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="castInfos"></param>
        /// <returns></returns>
        public override FightActionResultEnum ApplyEffect(CastInfos castInfos)
        {
            FightActivableObject obj = null;

            switch(castInfos.EffectType)
            {
                case EffectEnum.UseGlyph:
                    if (castInfos.Caster.Fight.HasObjectOnCell(FightObstacleTypeEnum.TYPE_FIGHTER, castInfos.CellId))
                    {
                        return FightActionResultEnum.RESULT_NOTHING;
                    }

                    obj = new FightGlyph(castInfos.Caster.Fight, castInfos.Caster, castInfos, castInfos.CellId, castInfos.Duration);
                    break;

                case EffectEnum.UseTrap:
                    if (!castInfos.Caster.Fight.CanPutObject(castInfos.CellId))
                    {
                        return FightActionResultEnum.RESULT_NOTHING;
                    }

                    obj = new FightTrap(castInfos.Caster.Fight, castInfos.Caster, castInfos, castInfos.CellId);
                    break;
            }

            if(obj != null)
                castInfos.Caster.Fight.AddActivableObject(castInfos.Caster, obj);

            return FightActionResultEnum.RESULT_NOTHING;
        }
    }
}
