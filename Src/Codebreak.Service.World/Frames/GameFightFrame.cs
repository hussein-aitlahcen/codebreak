using System;
using Codebreak.Framework.Network;
using Codebreak.Service.World.Game;
using Codebreak.Service.World.Game.Entity;
using Codebreak.Service.World.Game.Fight;
using Codebreak.Service.World.Game.Action;

namespace Codebreak.Service.World.Frames
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class GameFightFrame : FrameBase<GameFightFrame, EntityBase, string>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public override Action<EntityBase, string> GetHandler(string message)
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
        private void FightOption(EntityBase entity, string message)
        {
            var fighter = (FighterBase)entity;

            fighter.AddMessage(() =>
                {
                    if (!fighter.IsLeader)
                    {
                        Logger.Debug("GameFight::Option non leader player wants to lock : " + entity.Name);
                        fighter.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                        return;
                    }

                    fighter.Team.OptionLock(FightOptionTypeEnum.TYPE_SPECTATOR);
                });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="message"></param>
        private void FightTurnReady(EntityBase entity, string message)
        {
            var fighter = (FighterBase)entity;

            fighter.AddMessage(() =>
            {
                if(!fighter.HasGameAction(GameActionTypeEnum.FIGHT))
                {
                    fighter.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                    return;
                }

                if (fighter.Spectating)
                {
                    Logger.Debug("GameFight::TurnReady spectator player cant be ready : " + entity.Name);
                    fighter.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                    return;
                }

                fighter.TurnReady = true;
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="message"></param>
        private void FightTurnPass(EntityBase entity, string message)
        {
            var fighter = (FighterBase)entity;

            fighter.AddMessage(() =>
            {
                if (!fighter.HasGameAction(GameActionTypeEnum.FIGHT))
                {
                    fighter.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                    return;
                }

                if (fighter.Spectating)
                {
                    Logger.Debug("GameFight::TurnPass spectator player cant pass turn : " + entity.Name);
                    fighter.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                    return;
                }

                if (fighter.Fight.CurrentFighter != fighter)
                {
                    Logger.Debug("GameFight::TurnPass not the turn of this player : " + entity.Name);
                    fighter.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                    return;
                }

                fighter.TurnPass = true;
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="message"></param>
        private void FightQuit(EntityBase entity, string message)
        {
            var fighter = (FighterBase)entity;

            fighter.AddMessage(() =>
            {
                if (!fighter.HasGameAction(GameActionTypeEnum.FIGHT))
                {
                    fighter.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                    return;
                }

                fighter.Fight.AddMessage(() => fighter.Fight.FightQuit(fighter));
            });
        }        
    }
}
