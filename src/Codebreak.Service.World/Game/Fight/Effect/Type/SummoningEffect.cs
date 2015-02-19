using Codebreak.Service.World.Game.Entity;
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
    public sealed class SummoningEffect : EffectBase
    {
        /// <summary>
        /// 
        /// </summary>
        private bool m_static;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="staticInvoc"></param>
        public SummoningEffect(bool staticInvoc = false)
        {
            m_static = staticInvoc;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="castInfos"></param>
        /// <returns></returns>
        public override FightActionResultEnum ApplyEffect(CastInfos castInfos)
        {
            var monsterTemplate = Database.Repository.MonsterRepository.Instance.GetById(castInfos.Value1);
            if (monsterTemplate == null)
                return FightActionResultEnum.RESULT_NOTHING;

            var monsterGrade = monsterTemplate.Grades.FirstOrDefault(grade => grade.Grade == castInfos.Value2);
            if (monsterGrade == null)
                return FightActionResultEnum.RESULT_NOTHING;

            var cell = castInfos.Fight.GetCell(castInfos.CellId);
            if (!cell.CanWalk)
                return FightActionResultEnum.RESULT_NOTHING;

            return castInfos.Fight.SummonFighter(new MonsterEntity(castInfos.Fight.NextFighterId, monsterGrade, castInfos.Caster, m_static), castInfos.Caster.Team, castInfos.CellId);
        }
    }
}
