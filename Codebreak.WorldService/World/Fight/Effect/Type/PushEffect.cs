using Codebreak.WorldService.World.Action;
using Codebreak.WorldService.World.Map;
using Codebreak.WorldService.World.Spell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.WorldService.World.Fight.Effect.Type
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class PushEffect : EffectBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="castInfos"></param>
        /// <returns></returns>
        public override FightActionResultEnum ApplyEffect(CastInfos castInfos)
        {
            if (castInfos.Target == null)
                return FightActionResultEnum.RESULT_NOTHING;

            int direction = 0;

            switch (castInfos.EffectType)
            {
                case EffectEnum.PushBack:
                    if (Pathfinding.InLine(castInfos.Target.Fight.Map, castInfos.CellId, castInfos.Target.Cell.Id) && castInfos.CellId != castInfos.Target.Cell.Id)
                        direction = Pathfinding.GetDirection(castInfos.Target.Fight.Map, castInfos.CellId, castInfos.Target.Cell.Id);
                    else if (Pathfinding.InLine(castInfos.Target.Fight.Map, castInfos.Caster.Cell.Id, castInfos.Target.Cell.Id))
                        direction = Pathfinding.GetDirection(castInfos.Target.Fight.Map, castInfos.Caster.Cell.Id, castInfos.Target.Cell.Id);
                    else
                    {
                        return FightActionResultEnum.RESULT_NOTHING;
                    }
                    break;

                case EffectEnum.PushFront:
                    direction = Pathfinding.GetDirection(castInfos.Target.Fight.Map, castInfos.Target.Cell.Id, castInfos.Caster.Cell.Id);
                    break;
            }

            return ApplyPush(castInfos, castInfos.Target, direction, castInfos.Value1);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="castInfos"></param>
        /// <param name="target"></param>
        /// <param name="direction"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static FightActionResultEnum ApplyPush(CastInfos castInfos, FighterBase target, int direction, int length)
        {
            var currentCell = target.Cell;

            // TODO : Ajout de l'action sur le joueur !

            for (int i = 0; i < length; i++)
            {
                var nextCell = target.Fight.GetCell(Pathfinding.NextCell(target.Fight.Map, currentCell.Id, direction));

                if (nextCell != null && nextCell.CanWalk)
                {
                    if (nextCell.HasObject(FightObstacleTypeEnum.TYPE_TRAP))
                    {
                        target.Fight.Dispatch(WorldMessage.GAME_ACTION(GameActionTypeEnum.MAP_PUSHBACK, target.Id, target.Id + "," + nextCell.Id));

                        target.Fight.SetSubAction(() =>
                        {
                            return target.SetCell(nextCell);
                        }, 1 + length * 200);

                        return FightActionResultEnum.RESULT_NOTHING;
                    }
                }
                else
                {
                    if (i != 0)
                    {
                        target.Fight.Dispatch(WorldMessage.GAME_ACTION(GameActionTypeEnum.MAP_PUSHBACK, target.Id, target.Id + "," + currentCell.Id));
                    }

                    // Application des dommages
                        target.Fight.SetSubAction(() =>
                        {
                            if (castInfos.EffectType == EffectEnum.PushBack)
                            {
                                var pushResult = PushEffect.ApplyPushBackDamages(castInfos, target, length, i);
                                if (pushResult != FightActionResultEnum.RESULT_NOTHING)
                                    return pushResult;
                            }

                            return target.SetCell(currentCell);
                        }, 1 + length * 130);

                    return FightActionResultEnum.RESULT_NOTHING;
                }

                currentCell = nextCell;
            }

            target.Fight.Dispatch(WorldMessage.GAME_ACTION(GameActionTypeEnum.MAP_PUSHBACK, target.Id, target.Id + "," + currentCell.Id));

            target.Fight.SetSubAction(() =>
            {
                return target.SetCell(currentCell);
            }, 1 + length * 200);

            return FightActionResultEnum.RESULT_NOTHING;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="castInfos"></param>
        /// <param name="target"></param>
        /// <param name="Length"></param>
        /// <param name="CurrentLength"></param>
        /// <returns></returns>
        private static FightActionResultEnum ApplyPushBackDamages(CastInfos castInfos, FighterBase target, int Length, int CurrentLength)
        {
            var damageCoef = Util.Next(8, 17);
            double levelCoef = castInfos.Caster.Level / 50;
            if (levelCoef < 0.1)
                levelCoef = 0.1;
            int damageValue = (int)Math.Floor(damageCoef * levelCoef) * (Length - CurrentLength + 1);
            var subInfos = new CastInfos(EffectEnum.DamageBrut, castInfos.SpellId, castInfos.CellId, 0, 0, 0, 0, 0, castInfos.Caster, null);

            return DamageEffect.ApplyDamages(subInfos, target, ref damageValue);
        }
    }
}
