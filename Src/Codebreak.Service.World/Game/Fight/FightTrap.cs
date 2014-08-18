using Codebreak.Service.World.Game.Fight.Effect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Game.Fight
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class FightTrap : FightActivableObject
    {        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fight"></param>
        /// <param name="caster"></param>
        /// <param name="effect"></param>
        /// <param name="cell"></param>
        public FightTrap(FightBase fight, FighterBase caster, CastInfos effect, int cell)
            : base(FightObstacleTypeEnum.TYPE_TRAP, ActiveType.ACTIVE_ENDMOVE, fight, caster, effect, cell, 0, 306, true, false, true)
        {
        }
        
        /// <summary>
        /// 
        /// </summary>
        public override void AppearForAll()
        {
            Hide = false;

            _fight.Dispatch(WorldMessage.GAME_DATA_ZONE(OperatorEnum.OPERATOR_ADD, Cell.Id, Length, Color));
            _fight.Dispatch(WorldMessage.GAME_DATA_ZONE_CREATE(Cell.Id, ";Haaaaaaaaz3005;"));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dispatcher"></param>
        public override void Appear(MessageDispatcher dispatcher)
        {
            dispatcher.Dispatch(WorldMessage.GAME_DATA_ZONE(OperatorEnum.OPERATOR_ADD, Cell.Id, Length, Color));
            dispatcher.Dispatch(WorldMessage.GAME_DATA_ZONE_CREATE(Cell.Id, ";Haaaaaaaaz3005;"));
        }

        /// <summary>
        /// 
        /// </summary>
        public override void DisappearForAll()
        {
            if (Hide)
            {
                _caster.Team.Dispatch(WorldMessage.GAME_DATA_ZONE(OperatorEnum.OPERATOR_REMOVE, Cell.Id, Length, Color));
                _caster.Team.Dispatch(WorldMessage.GAME_DATA_ZONE_CREATE(Cell.Id));
            }
            else
            {
                _fight.Dispatch(WorldMessage.GAME_DATA_ZONE(OperatorEnum.OPERATOR_REMOVE, Cell.Id, Length, Color));
                _fight.Dispatch(WorldMessage.GAME_DATA_ZONE_CREATE(Cell.Id));
            }
        }
    }
}
