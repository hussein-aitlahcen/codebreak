using Codebreak.Service.World.Game.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Codebreak.Service.World.Game.Entity;
using Codebreak.Service.World.Game.Job;
using Codebreak.Service.World.Database.Structure;
using Codebreak.Service.World.Game.Mount;
using Codebreak.Service.World.Manager;
using Codebreak.Service.World.Network;

namespace Codebreak.Service.World.Game.Interactive.Type
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class PaddockDoor : InteractiveObject
    {
        /// <summary>
        /// 
        /// </summary>
        private Paddock m_paddock;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="map"></param>
        /// <param name="cellId"></param>
        public PaddockDoor(MapInstance map, int cellId)
            :base(map, cellId)
        {
            m_paddock = map.Paddock;
            if (m_paddock == null)
                Logger.Info("null paddock on map " + map.Id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="skill"></param>
        public override void UseWithSkill(CharacterEntity character, JobSkill skill)
        {
            if (m_paddock == null)
            {
                base.UseWithSkill(character, skill);
            }
            else
            {
                switch (skill.Id)
                {
                    case SkillIdEnum.SKILL_ACCEDER:
                        Access(character);
                        break;

                    case SkillIdEnum.SKILL_ACHETER_ENCLOS:
                        Buy(character);
                        break;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        public void Access(CharacterEntity character)
        {
            if (m_paddock.Public)
            {
                character.ExchangePaddock(m_paddock);
            }
            else if (!m_paddock.OnSale)
            {
                // TODO : if in the same guild and has enough rights
            }
            else
            {
                // Trying to access to an in sale paddock, almost cheating no ?
                character.Dispatch(WorldMessage.BASIC_NO_OPERATION());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        public void Buy(CharacterEntity character)
        {
            if (m_paddock.OnSale)
            {

            }
            else
            {
                // Trying to buy a public or already owned paddock, cheating for sure :)))
                character.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                Logger.Info("PaddockDoor::Buy() trying to buy a free or already owned paddock " + character.Name);
            }
        }
    }
}
