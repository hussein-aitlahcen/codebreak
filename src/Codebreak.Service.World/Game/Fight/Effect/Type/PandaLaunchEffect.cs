using Codebreak.Service.World.Game.Map;
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
    public sealed class PandaLaunchEffect : AbstractSpellEffect
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="CastInfos"></param>
        /// <returns></returns>
        public override FightActionResultEnum ApplyEffect(CastInfos CastInfos)
        {
            var infos = CastInfos.Caster.StateManager.FindState(FighterStateEnum.STATE_CARRIER);

            if (infos != null)
            {
                var target = infos.Target;

                if (target.StateManager.HasState(FighterStateEnum.STATE_CARRIED))
                {
                    var cell = target.Fight.GetCell(CastInfos.CellId);
                    if (cell.CanWalk)
                    {
                        var sleepTime = 1 + (WorldConfig.FIGHT_PANDA_LAUNCH_CELL_TIME * Pathfinding.GoalDistance(target.Fight.Map, target.Cell.Id, CastInfos.CellId));

                        target.Fight.Dispatch(WorldMessage.GAME_ACTION(EffectEnum.PandaLaunch, CastInfos.Caster.Id, CastInfos.CellId.ToString()));

                        target.Fight.SetSubAction(() =>
                        {
                            return target.SetCell(cell);
                        }, sleepTime);
                    }
                }
            }

            return FightActionResultEnum.RESULT_NOTHING;
        }
    }
}
