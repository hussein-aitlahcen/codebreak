using Codebreak.Service.World.Game.Map;
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
    public sealed class PerceptionEffect : AbstractSpellEffect
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="castInfos"></param>
        /// <returns></returns>
        public override FightActionResultEnum ApplyEffect(CastInfos castInfos)
        {
            foreach (var cell in CellZone.GetCells(castInfos.Map, castInfos.CellId, castInfos.Target.Cell.Id, castInfos.RangeType))
            {
                var fightCell = castInfos.Fight.GetCell(cell);
                if(fightCell != null)
                {
                    foreach(var fightObject in fightCell.FightObjects)
                    {
                        if(fightObject.Cell.Id == cell)
                        {
                            if(fightObject.ObstacleType == FightObstacleTypeEnum.TYPE_TRAP)
                            {
                                var trap = (FightTrap)fightObject;
                                trap.Appear(castInfos.Caster.Team);
                            }
                            else if (fightObject.ObstacleType == FightObstacleTypeEnum.TYPE_FIGHTER)
                            {
                                var fighter = (AbstractFighter)fightObject;
                                if(fighter.Team != castInfos.Caster.Team)
                                {
                                    if(fighter.StateManager.HasState(FighterStateEnum.STATE_STEALTH))
                                    {
                                        fighter.BuffManager.RemoveStealth();
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return FightActionResultEnum.RESULT_NOTHING;
        }
    }
}
