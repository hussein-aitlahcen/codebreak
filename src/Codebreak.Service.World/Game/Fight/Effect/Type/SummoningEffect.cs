using Codebreak.Service.World.Game.Entity;
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
    public sealed class SummoningEffect : EffectBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="castInfos"></param>
        /// <returns></returns>
        public override FightActionResultEnum ApplyEffect(CastInfos castInfos)
        {
            var invocationCount = castInfos.Caster.Team.AliveFighters.Count(fighter => fighter.Invocator == castInfos.Caster);

            if (invocationCount >= castInfos.Caster.Statistics.GetTotal(Spell.EffectEnum.AddInvocationMax))
                return FightActionResultEnum.RESULT_NOTHING;

            var monsterTemplate = Database.Repository.MonsterRepository.Instance.GetById(castInfos.Value1);
            if (monsterTemplate == null)
                return FightActionResultEnum.RESULT_NOTHING;

            var monsterGrade = monsterTemplate.GetGrades().FirstOrDefault(grade => grade.Grade == castInfos.Value2);
            if (monsterGrade == null)
                return FightActionResultEnum.RESULT_NOTHING;

            var cell = castInfos.Fight.GetCell(castInfos.CellId);
            if (!cell.CanWalk)
                return FightActionResultEnum.RESULT_NOTHING;

            return castInfos.Fight.SummonFighter(new MonsterEntity(castInfos.Fight.NextFighterId, monsterGrade, castInfos.Caster), castInfos.Caster.Team, castInfos.CellId);
        }
    }
}
