using System;
using Codebreak.Framework.Network;
using Codebreak.Service.World.Game;
using Codebreak.Service.World.Game.Entity;
using Codebreak.Service.World.Game.Fight;
using Codebreak.Service.World.Game.Action;
using Codebreak.Service.World.Network;

namespace Codebreak.Service.World.Frames
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class GameFightFrame : FrameBase<GameFightFrame, CharacterEntity, string>
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
        private void FightOption(CharacterEntity entity, string message)
        {
            entity.AddMessage(() =>
                {
                    if (!entity.IsLeader)
                    {
                        Logger.Debug("GameFight::Option non leader player wants to lock : " + entity.Name);
                        entity.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                        return;
                    }

                    entity.Team.OptionLock(FightOptionTypeEnum.TYPE_SPECTATOR);
                });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="message"></param>
        private void FightTurnReady(CharacterEntity entity, string message)
        {
            entity.AddMessage(() =>
            {
                if (!entity.HasGameAction(GameActionTypeEnum.FIGHT))
                {
                    entity.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                    return;
                }

                if (entity.IsSpectating)
                {
                    Logger.Debug("GameFight::TurnReady spectator player cant be ready : " + entity.Name);
                    entity.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                    return;
                }

                entity.TurnReady = true;
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="message"></param>
        private void FightTurnPass(CharacterEntity entity, string message)
        {
            entity.AddMessage(() =>
            {
                if (!entity.HasGameAction(GameActionTypeEnum.FIGHT))
                {
                    entity.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                    return;
                }

                if (entity.IsSpectating)
                {
                    Logger.Debug("GameFight::TurnPass spectator player cant pass turn : " + entity.Name);
                    entity.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                    return;
                }

                if (entity.Fight.CurrentFighter != entity)
                {
                    Logger.Debug("GameFight::TurnPass not the turn of this player : " + entity.Name);
                    entity.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                    return;
                }

                entity.TurnPass = true;
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

                if (!entity.Fight.CancelButton && entity.Fight.State != FightStateEnum.STATE_FIGHTING)
                {
                    entity.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                    return;
                }

                entity.Fight.AddMessage(() => entity.Fight.FightQuit(entity));                
            });
        }        
    }
}
