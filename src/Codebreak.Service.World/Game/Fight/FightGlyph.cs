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
    public sealed class FightGlyph : AbstractActivableObject
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fight"></param>
        /// <param name="caster"></param>
        /// <param name="effect"></param>
        /// <param name="cell"></param>
        /// <param name="duration"></param>
        public FightGlyph(AbstractFight fight, AbstractFighter caster, CastInfos effect, int cell, int duration)
            : base(FightObstacleTypeEnum.TYPE_GLYPH, ActiveType.ACTIVE_BEGINTURN, fight, caster, effect, cell, duration, 307, true, true)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public override void AppearForAll()
        {
            m_fight.CachedBuffer = true;
            m_fight.Dispatch(WorldMessage.GAME_DATA_ZONE(OperatorEnum.OPERATOR_ADD, Cell.Id, Length, Color));
            m_fight.Dispatch(WorldMessage.GAME_DATA_ZONE_CREATE(Cell.Id));
            m_fight.CachedBuffer = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dispatcher"></param>
        public override void Appear(MessageDispatcher dispatcher)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public override void DisappearForAll()
        {
            m_fight.CachedBuffer = true;
            m_fight.Dispatch(WorldMessage.GAME_DATA_ZONE(OperatorEnum.OPERATOR_REMOVE, Cell.Id, Length, Color));
            m_fight.Dispatch(WorldMessage.GAME_DATA_ZONE_CREATE(Cell.Id));
            m_fight.CachedBuffer = false;
        }
    }
}
