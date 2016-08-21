using Codebreak.Service.World.Game.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Codebreak.Service.World.Manager;
using Codebreak.Service.World.Network;

namespace Codebreak.Service.World.Game.Action
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class GameChallengeRequestAction : AbstractGameAction
    {
        /// <summary>
        /// 
        /// </summary>
        public override bool CanAbort => true;

        /// <summary>
        /// 
        /// </summary>
        public CharacterEntity Attacker
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public CharacterEntity Defender
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="defender"></param>
        public GameChallengeRequestAction(CharacterEntity attacker, CharacterEntity defender)
            : base(GameActionTypeEnum.CHALLENGE_REQUEST, attacker)
        {
            Attacker = attacker;
            Defender = defender;
            Attacker.Map.Dispatch(WorldMessage.GAME_ACTION(GameActionTypeEnum.CHALLENGE_REQUEST, Attacker.Id, SerializeAs_GameAction()));
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Stop(params object[] args)        
        {
            Finish(GameActionTypeEnum.CHALLENGE_ACCEPT);
            Attacker.Map.FightManager.StartChallenge(Attacker, Defender);
            base.Stop(args);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public override void Abort(params object[] args)
        {
            Finish(GameActionTypeEnum.CHALLENGE_DECLINE);
            base.Abort(args);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="result"></param>
        private void Finish(GameActionTypeEnum result)
        {
            var message = WorldMessage.GAME_ACTION(result, Attacker.Id, Defender.Id.ToString());
            Attacker.Dispatch(message);
            Defender.Dispatch(message);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string SerializeAs_GameAction()
        {
            return Defender.Id.ToString();
        }
    }
}
