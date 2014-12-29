using System;
using Codebreak.Framework.Network;
using Codebreak.Service.World.Game;
using Codebreak.Service.World.Game.Entity;
using Codebreak.Service.World.Game.Fight;
using Codebreak.Service.World.Game.Action;
using Codebreak.Service.World.Network;

namespace Codebreak.Service.World.Frames
{
    public sealed class GameFightPlacementFrame : FrameBase<GameFightPlacementFrame, CharacterEntity, string>
    {
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
        private void FightOption(CharacterEntity entity, string message)
        {
            var optionType = (FightOptionTypeEnum)message[1];

            entity.AddMessage(() =>
                {
                    if (!entity.HasGameAction(GameActionTypeEnum.FIGHT))
                    {
                        entity.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                        return;
                    }

                    if (!entity.IsLeader)
                    {
                        Logger.Debug("GameFightPlacement::Option non leader player wants to lock : " + entity.Name);
                        entity.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                        return;
                    }

                    entity.Team.OptionLock(optionType);
                });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="message"></param>
        private void FightReady(CharacterEntity entity, string message)
        {
            entity.AddMessage(() =>
                {
                    if (!entity.HasGameAction(GameActionTypeEnum.FIGHT))
                    {
                        entity.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                        return;
                    }

                    entity.Fight.FighterReady(entity);
                });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="message"></param>
        private void FightPlacement(CharacterEntity entity, string message)
        {
            entity.AddMessage(() =>
                {
                    if (!entity.HasGameAction(GameActionTypeEnum.FIGHT))
                    {
                        entity.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                        return;
                    }

                    if (entity.TurnReady)
                    {
                        Logger.Debug("GameFightPlacement::Placement turn ready, unable to move anymore : " + entity.Name);
                        entity.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                        return;
                    }

                    int cellId = -1;
                    if (!int.TryParse(message.Substring(2), out cellId))
                    {
                        Logger.Debug("GameFightPlacement::Placement unable to parse cell id : " + entity.Name);
                        entity.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                        return;
                    }

                    entity.Fight.FighterPlacementChange(entity, cellId);
                });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="message"></param>
        private void FightQuit(CharacterEntity entity, string message)
        {
            entity.AddMessage(() =>
            {
                if (!entity.HasGameAction(GameActionTypeEnum.FIGHT))
                {
                    entity.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                    return;
                }

                if (message == "GQ")
                {
                    entity.Fight.AddMessage(() => entity.Fight.FightQuit(entity));
                    return;
                }

                if (!entity.IsLeader)
                {
                    Logger.Debug("FightPlacement::Quit non leader player trying to kick : " + entity.Name);
                    entity.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                    return;
                }

                long fighterId = -1;
                if (!long.TryParse(message.Substring(2), out fighterId))
                {
                    Logger.Debug("FightPlacement::Quit unable to parse fighterId : " + entity.Name);
                    entity.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                    return;
                }

                var selectedFighter = entity.Team.GetFighter(fighterId);
                if (selectedFighter == null)
                {
                    entity.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                    return;
                }

                if (selectedFighter.IsLeader)
                {
                    Logger.Debug("FightPlacement::Quit unable to kick leader : " + entity.Name);
                    entity.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                    return;
                }

                entity.Fight.AddMessage(() => entity.Fight.FightQuit(selectedFighter, true));
            });
        }
    }
}
