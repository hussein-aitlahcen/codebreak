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
    public sealed class GameTaxCollectorDefenderAction : GameActionBase
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
        /// <param name="character"></param>
        public GameTaxCollectorDefenderAction(CharacterEntity character)
            : base(GameActionTypeEnum.TAXCOLLECTOR_AGGRESSION, character)
        {
            Character = character;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public override void Abort(params object[] args)
        {
            base.Abort(args);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public override void Stop(params object[] args)
        {           
            if (Character.CharacterGuild != null)
            {
                Character.SafeDispatch(WorldMessage.GUILD_TAXCOLLECTOR_DEFENDER_LEAVE(Character.CharacterGuild.TaxCollectorJoinedId, Character.Id));
                Character.CharacterGuild.TaxCollectorLeave();
            }
            base.Stop(args);   
        }
    }
}
