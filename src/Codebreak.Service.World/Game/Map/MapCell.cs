using Codebreak.Service.World.Database.Structure;
using Codebreak.Service.World.Game.Entity;
using Codebreak.Service.World.Game.Interactive;
using Codebreak.Service.World.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Game.Map
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class MapCell
    {
        public int Id;

        public bool Walkable => m_walkable && (InteractiveObjectId == 0 || (InteractiveObject != null && InteractiveObject.CanWalkThrough));

        public bool LineOfSight;
        public int InteractiveObjectId;

        private readonly bool m_walkable;

        /// <summary>
        /// 
        /// </summary>
        public InteractiveObject InteractiveObject
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        public MapTriggerDAO Trigger
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="map"></param>
        /// <param name="id"></param>
        /// <param name="data"></param>
        /// <param name="trigger"></param>
        public MapCell(MapInstance map, int id, byte[] data, MapTriggerDAO trigger = null)
        {
            Id = id;
            Trigger = trigger;
            m_walkable = ((data[2] & 56) >> 3) > 0;
            if (!m_walkable && ((data[2] & 56) >> 3) != 0)
                return;
            LineOfSight = (data[0] & 1) == 1;
            if ((data[7] & 2) >> 1 == 1)
            {
                InteractiveObjectId = ((data[0] & 2) << 12) + ((data[1] & 1) << 12) + (data[8] << 6) + data[9];
                if (InteractiveObjectManager.Instance.Exists(InteractiveObjectId))
                    InteractiveObject = InteractiveObjectManager.Instance.Generate(InteractiveObjectId, map, Id);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <returns></returns>
        public bool SatisfyConditions(CharacterEntity character)
        {
            return Trigger.SatisfyConditions(character);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        public void ApplyActions(CharacterEntity character)
        {
            foreach (var action in Trigger.ActionsList)
                ActionEffectManager.Instance.ApplyEffect(character, action.Effect, action.Parameters);
        }
    }
}
