using Codebreak.Service.World.Game.Entity;
using Codebreak.Service.World.Game.Interactive.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Game.Action
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class GameHarvestAction : AbstractGameAction
    {
        /// <summary>
        /// 
        /// </summary>
        public override bool CanAbort
        {
            get { return true; }
        }

        /// <summary>
        /// 
        /// </summary>
        public CharacterEntity Character
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public HarvestableResource HarvestableResource
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="harvestableObject"></param>
        public GameHarvestAction(CharacterEntity character, HarvestableResource harvestableResource, int duration)
            : base(GameActionTypeEnum.SKILL_HARVEST, character, duration)
        {
            HarvestableResource = harvestableResource;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Start()
        {
            base.Start();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public override void Abort(params object[] args)
        {
            HarvestableResource.AbortHarvest();

            base.Abort(args);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string SerializeAs_GameAction()
        {
            return HarvestableResource.CellId + "," + Duration;
        }
    }
}
