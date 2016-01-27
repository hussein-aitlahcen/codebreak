using Codebreak.Service.World.Game.Action;
using Codebreak.Service.World.Game.Entity;
using Codebreak.Service.World.Game.Job;
using Codebreak.Service.World.Game.Map;
using Codebreak.Service.World.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Game.Interactive.Type
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class TrashCan : InteractiveObject
    {
        /// <summary>
        /// 
        /// </summary>
        private StorageInventory m_storage;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="map"></param>
        /// <param name="cellId"></param>
        public TrashCan(MapInstance map, int cellId) 
            : base(map, cellId)
        {
            m_storage = new StorageInventory();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="skill"></param>
        public override void UseWithSkill(CharacterEntity character, JobSkill skill)
        {
            switch(skill.Id)
            {
                case SkillIdEnum.SKILL_FOUILLER:
                    StartUse(character);
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        public void StartUse(CharacterEntity character)
        {
            if(!character.CanGameAction(GameActionTypeEnum.EXCHANGE))
            {
                character.Dispatch(WorldMessage.INFORMATION_MESSAGE(InformationTypeEnum.ERROR, InformationEnum.ERROR_YOU_ARE_AWAY));
                return;
            }

            character.ExchangeStorage(m_storage);
        }
    }
}
