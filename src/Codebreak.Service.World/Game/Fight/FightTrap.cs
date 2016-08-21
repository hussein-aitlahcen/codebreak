using Codebreak.Service.World.Game.Fight.Effect;
using Codebreak.Service.World.Network;
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
        public FightTrap(AbstractFight fight, AbstractFighter caster, CastInfos effect, int cell)
            : base(FightObstacleTypeEnum.TYPE_TRAP, ActiveType.ACTIVE_ENDMOVE, fight, caster, effect, cell, 0, 306, true, false, true)
        {
        }
        
        /// <summary>
        /// 
        /// </summary>
        public override void AppearForAll()
        {
            Hide = false;

            m_fight.CachedBuffer = true;
            m_fight.Dispatch(WorldMessage.GAME_DATA_ZONE(OperatorEnum.OPERATOR_ADD, Cell.Id, Length, Color));
            m_fight.Dispatch(WorldMessage.GAME_DATA_ZONE_CREATE(Cell.Id, ";Haaaaaaaaz3005;"));
            m_fight.CachedBuffer = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dispatcher"></param>
        public override void Appear(MessageDispatcher dispatcher)
        {
            dispatcher.CachedBuffer = true;
            dispatcher.Dispatch(WorldMessage.GAME_DATA_ZONE(OperatorEnum.OPERATOR_ADD, Cell.Id, Length, Color));
            dispatcher.Dispatch(WorldMessage.GAME_DATA_ZONE_CREATE(Cell.Id, ";Haaaaaaaaz3005;"));
            dispatcher.CachedBuffer = false;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void DisappearForAll()
        {
            if (Hide)
            {
                m_caster.Team.CachedBuffer = true;
                m_caster.Team.Dispatch(WorldMessage.GAME_DATA_ZONE(OperatorEnum.OPERATOR_REMOVE, Cell.Id, Length, Color));
                m_caster.Team.Dispatch(WorldMessage.GAME_DATA_ZONE_CREATE(Cell.Id));
                m_caster.Team.CachedBuffer = false;
            }
            else
            {
                m_fight.CachedBuffer = true;
                m_fight.Dispatch(WorldMessage.GAME_DATA_ZONE(OperatorEnum.OPERATOR_REMOVE, Cell.Id, Length, Color));
                m_fight.Dispatch(WorldMessage.GAME_DATA_ZONE_CREATE(Cell.Id));
                m_fight.CachedBuffer = false;
            }
        }
    }
}
