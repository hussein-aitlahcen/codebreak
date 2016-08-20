using Codebreak.Service.World.Game.Dialog;
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
    public sealed class GameNpcDialogAction : AbstractGameAction
    {
        /// <summary>
        /// 
        /// </summary>
        public override bool CanAbort
        {
            get 
            { 
                return true; 
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public NpcDialog Dialog
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public NonPlayerCharacterEntity Npc
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="npc"></param>
        public GameNpcDialogAction(CharacterEntity character, NonPlayerCharacterEntity npc)
            : base(GameActionTypeEnum.NPC_DIALOG, character)
        {
            Npc = npc;
            Dialog = new NpcDialog(character, npc);
        }
        
        /// <summary>
        /// 
        /// </summary>
        public override void Start()
        {
            Entity.Dispatch(WorldMessage.DIALOG_CREATE(Npc.Id));
            Dialog.SendQuestion(Npc.InitialQuestion);
            base.Start();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public override void Abort(params object[] args)
        {
            Entity.Dispatch(WorldMessage.DIALOG_LEAVE());
            base.Abort(args);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public override void Stop(params object[] args)
        {
            Entity.Dispatch(WorldMessage.DIALOG_LEAVE());
            base.Stop(args);
        }
    }
}
