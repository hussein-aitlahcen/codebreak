using Codebreak.Service.World.Game.Action;
using Codebreak.Service.World.Game.Map;
using Codebreak.Service.World.Game.Spell;
using Codebreak.Service.World.Network;
using Codebreak.WorldService;
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
    public sealed class PushFearEffect : EffectBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="castInfos"></param>
        /// <returns></returns>
        public override FightActionResultEnum ApplyEffect(CastInfos castInfos)
        {
            int direction = Pathfinding.GetDirection(castInfos.Map, castInfos.Caster.Cell.Id, castInfos.CellId);
            var targetFighterCell = Pathfinding.NextCell(castInfos.Map, castInfos.Caster.Cell.Id, direction);

            var target = castInfos.Fight.GetFighterOnCell(targetFighterCell);
            if (target == null)
                return FightActionResultEnum.RESULT_NOTHING;

            var distance = Pathfinding.GoalDistance(castInfos.Map, target.Cell.Id, castInfos.CellId);
            var currentCell = target.Cell;

            for (int i = 0; i < distance; i++)
            {
                var nextCell = castInfos.Fight.GetCell(Pathfinding.NextCell(castInfos.Map, currentCell.Id, direction));

                if (nextCell != null && nextCell.CanWalk)
                {
                    if (nextCell.HasObject(FightObstacleTypeEnum.TYPE_TRAP))
                    {
                        castInfos.Fight.Dispatch(WorldMessage.GAME_ACTION(GameActionTypeEnum.MAP_PUSHBACK, target.Id, target.Id + "," + nextCell.Id));

                        castInfos.Fight.SetSubAction(() =>
                        {
                            return target.SetCell(nextCell);
                        }, 1 + ++i * WorldConfig.FIGHT_PUSH_CELL_TIME);

                        return FightActionResultEnum.RESULT_NOTHING;
                    }
                }
                else
                {
                    if (i != 0)
                    {
                        castInfos.Fight.Dispatch(WorldMessage.GAME_ACTION(GameActionTypeEnum.MAP_PUSHBACK, target.Id, target.Id + "," + currentCell.Id));
                    }

                    castInfos.Fight.SetSubAction(() =>
                    {
                        return target.SetCell(currentCell);
                    }, 1 + (i * WorldConfig.FIGHT_PUSH_CELL_TIME));

                    return FightActionResultEnum.RESULT_NOTHING;
                }

                currentCell = nextCell;
            }

            castInfos.Fight.Dispatch(WorldMessage.GAME_ACTION(GameActionTypeEnum.MAP_PUSHBACK, target.Id, target.Id + "," + currentCell.Id));

            castInfos.Fight.SetSubAction(() =>
            {
                return target.SetCell(currentCell);
            }, 1 + distance * WorldConfig.FIGHT_PUSH_CELL_TIME);

            return FightActionResultEnum.RESULT_NOTHING;
        }
    }
}
