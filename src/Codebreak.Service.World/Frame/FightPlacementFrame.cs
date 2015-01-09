using System;
using Codebreak.Framework.Network;
using Codebreak.Service.World.Game;
using Codebreak.Service.World.Game.Entity;
using Codebreak.Service.World.Game.Fight;
using Codebreak.Service.World.Game.Action;
using Codebreak.Service.World.Network;

namespace Codebreak.Service.World.Frame
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class FightPlacementFrame : FrameBase<FightPlacementFrame, CharacterEntity, string>
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
                case 'G':
                    switch (message[1])
                    {
                        case 'R':
                            return FightReady;

                        case 'p':
                            return FightPlacement;

                        case 'Q':
                            return FightQuit;
                    }
                    break;

                case 'f':
                    switch (message[1])
                    {
                        case 'N':
                        case 'S':
                        case 'P':
                        case 'H':
                            return FightOption;
                    }
                    break;
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="message"></param>
        private void FightOption(CharacterEntity character, string message)
        {
            var optionType = (FightOptionTypeEnum)message[1];

            character.AddMessage(() =>
                {
                    if (!character.HasGameAction(GameActionTypeEnum.FIGHT))
                    {
                        character.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                        return;
                    }

                    if (!character.IsLeader)
                    {
                        Logger.Debug("GameFightPlacement::Option non leader player wants to lock : " + character.Name);
                        character.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                        return;
                    }

                    character.Team.OptionLock(optionType);
                });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="message"></param>
        private void FightReady(CharacterEntity character, string message)
        {
            character.AddMessage(() =>
                {
                    if (!character.HasGameAction(GameActionTypeEnum.FIGHT))
                    {
                        character.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                        return;
                    }

                    character.Fight.FighterReady(character);
                });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="message"></param>
        private void FightPlacement(CharacterEntity character, string message)
        {
            character.AddMessage(() =>
                {
                    if (!character.HasGameAction(GameActionTypeEnum.FIGHT))
                    {
                        character.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                        return;
                    }

                    if (character.TurnReady)
                    {
                        Logger.Debug("GameFightPlacement::Placement turn ready, unable to move anymore : " + character.Name);
                        character.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                        return;
                    }

                    int cellId = -1;
                    if (!int.TryParse(message.Substring(2), out cellId))
                    {
                        Logger.Debug("GameFightPlacement::Placement unable to parse cell id : " + character.Name);
                        character.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                        return;
                    }

                    character.Fight.FighterPlacementChange(character, cellId);
                });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="message"></param>
        private void FightQuit(CharacterEntity character, string message)
        {
            character.AddMessage(() =>
            {
                if (!character.HasGameAction(GameActionTypeEnum.FIGHT))
                {
                    character.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                    return;
                }

                if (message == "GQ")
                {
                    character.Fight.AddMessage(() => character.Fight.FightQuit(character));
                    return;
                }

                if (!character.IsLeader)
                {
                    Logger.Debug("FightPlacement::Quit non leader player trying to kick : " + character.Name);
                    character.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                    return;
                }

                long fighterId = -1;
                if (!long.TryParse(message.Substring(2), out fighterId))
                {
                    Logger.Debug("FightPlacement::Quit unable to parse fighterId : " + character.Name);
                    character.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                    return;
                }

                var selectedFighter = character.Team.GetFighter(fighterId);
                if (selectedFighter == null)
                {
                    character.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                    return;
                }

                if (selectedFighter.IsLeader)
                {
                    Logger.Debug("FightPlacement::Quit unable to kick leader : " + character.Name);
                    character.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                    return;
                }

                character.Fight.AddMessage(() => character.Fight.FightQuit(selectedFighter, true));
            });
        }
    }
}
