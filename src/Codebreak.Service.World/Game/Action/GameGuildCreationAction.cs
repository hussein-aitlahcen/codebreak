using Codebreak.Service.World.Game.Entity;
using Codebreak.Service.World.Network;
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
    public sealed class GameGuildCreationAction : AbstractGameAction
    {
        /// <summary>
        /// 
        /// </summary>
        public override bool CanAbort => true;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        public GameGuildCreationAction(CharacterEntity character)
            : base(GameActionTypeEnum.GUILD_CREATE, character)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Start()
        {
            Entity.Dispatch(WorldMessage.GUILD_CREATION_OPEN());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public override void Abort(params object[] args)
        {
            Entity.Dispatch(WorldMessage.GUILD_CREATION_CLOSE());
            base.Abort(args);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public override void Stop(params object[] args)
        {
            Entity.Dispatch(WorldMessage.GUILD_CREATION_CLOSE());
            base.Stop(args);
        }
    }
}
