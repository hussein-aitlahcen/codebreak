using System;
using Codebreak.Framework.Network;
using Codebreak.Service.World.Game.Entity;
using Codebreak.Service.World.Game;
using Codebreak.Service.World.Network;

namespace Codebreak.Service.World.Frame
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class MapFrame : FrameBase<MapFrame, CharacterEntity, string>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public override Action<CharacterEntity, string> GetHandler(string message)
        {
            if (message.Length < 2)
                return null;

            switch (message[0])
            {
                case 'f':
                    switch (message[1])
                    {

                        case 'L':
                            return FightList;

                        case 'D':
                            return FightDetails;
                    }
                    break;

                case 'G':
                    switch(message[1])
                    {
                        case 'P': // alignment
                            if (message.Length < 3)
                                return null;

                            switch (message[2])
                            {
                                case '+':
                                    return AlignmentEnable;
                                    
                                case '-':
                                    return AlignmentDisable;

                                case '*':
                                    return AlignmentDisableCost;
                            }
                            break;
                    }
                    break;
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="message"></param>
        private void AlignmentDisableCost(CharacterEntity character, string message)
        {
            character.SafeDispatch(WorldMessage.ALIGNMENT_DISABLE_COST((character.CharacterAlignment.Honour / 100) * 5));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="message"></param>
        private void AlignmentDisable(CharacterEntity character, string message)
        {
            character.AddMessage(() => character.DisableAlignment());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="message"></param>
        private void AlignmentEnable(CharacterEntity character, string message)
        {
            character.AddMessage(() => character.EnableAlignment());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="message"></param>
        private void FightList(CharacterEntity entity, string message)
        {
            entity.AddMessage(() =>
            {
                if (entity.Map.FightManager.FightCount > 0)
                {
                    entity.Dispatch(WorldMessage.FIGHT_LIST(entity.Map.FightManager.Fights));
                }
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="message"></param>
        private void FightDetails(CharacterEntity entity, string message)
        {
            if (message.Length < 3)
                return;

            var fightId = int.Parse(message.Substring(2));

            entity.AddMessage(() =>
                {
                    var fight = entity.Map.FightManager.GetFight(fightId);

                    if (fight == null)
                        return;

                    entity.Dispatch(WorldMessage.FIGHT_DETAILS(fight));
                });
        }
    }
}
