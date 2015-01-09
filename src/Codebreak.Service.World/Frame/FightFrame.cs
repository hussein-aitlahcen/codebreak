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
    public sealed class FightFrame : FrameBase<FightFrame, CharacterEntity, string>
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
                        case 't':
                            return FightTurnPass;

                        case 'T':
                            return FightTurnReady;

                        case 'Q':
                            return FightQuit;
                    }
                    break;
                case 'f':
                    switch (message[1])
                    {
                        case 'S':
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
            character.AddMessage(() =>
                {
                    if (!character.IsLeader)
                    {
                        Logger.Debug("GameFight::Option non leader player wants to lock : " + character.Name);
                        character.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                        return;
                    }

                    character.Team.OptionLock(FightOptionTypeEnum.TYPE_SPECTATOR);
                });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="message"></param>
        private void FightTurnReady(CharacterEntity character, string message)
        {
            character.AddMessage(() =>
            {
                if (!character.HasGameAction(GameActionTypeEnum.FIGHT))
                {
                    character.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                    return;
                }

                if (character.IsSpectating)
                {
                    Logger.Debug("GameFight::TurnReady spectator player cant be ready : " + character.Name);
                    character.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                    return;
                }

                character.TurnReady = true;
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="message"></param>
        private void FightTurnPass(CharacterEntity character, string message)
        {
            character.AddMessage(() =>
            {
                if (!character.HasGameAction(GameActionTypeEnum.FIGHT))
                {
                    character.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                    return;
                }

                if (character.IsSpectating)
                {
                    Logger.Debug("GameFight::TurnPass spectator player cant pass turn : " + character.Name);
                    character.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                    return;
                }

                if (character.Fight.CurrentFighter != character)
                {
                    Logger.Debug("GameFight::TurnPass not the turn of this player : " + character.Name);
                    character.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                    return;
                }

                character.TurnPass = true;
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

                if (!character.Fight.CancelButton && character.Fight.State != FightStateEnum.STATE_FIGHTING)
                {
                    character.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                    return;
                }

                character.Fight.AddMessage(() => character.Fight.FightQuit(character));                
            });
        }        
    }
}
