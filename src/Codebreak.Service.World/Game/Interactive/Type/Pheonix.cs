using Codebreak.Service.World.Game.Entity;
using Codebreak.Service.World.Game.Job;
using Codebreak.Service.World.Game.Map;
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
    public sealed class Pheonix : InteractiveObject
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="map"></param>
        /// <param name="cellId"></param>
        public Pheonix(MapInstance map, int cellId) 
            : base(map, cellId, false)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="skill"></param>
        public override void UseWithSkill(CharacterEntity character, SkillIdEnum skill)
        {
            switch(skill)
            {
                case SkillIdEnum.SKILL_USE_PHOENIX:
                    ReleasePlayer(character);
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void ReleasePlayer(CharacterEntity character)
        {
            if (!character.IsGhost)
                return;

            character.Reborn();
        }
    }
}
