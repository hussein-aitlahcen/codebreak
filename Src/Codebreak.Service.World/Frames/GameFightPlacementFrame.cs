using System;
using Codebreak.Framework.Network;
using Codebreak.Service.World.Game;
using Codebreak.Service.World.Game.Entity;
using Codebreak.Service.World.Game.Fight;

namespace Codebreak.Service.World.Frames
{
    public sealed class GameFightPlacementFrame : FrameBase<GameFightPlacementFrame, EntityBase, string>
    {
        public override Action<EntityBase, string> GetHandler(string message)
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
                            return GameQuit;
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
        private void FightOption(EntityBase entity, string message)
        {
            var fighter = (FighterBase)entity;
            var optionType = (FightOptionTypeEnum)message[1];

            if (!fighter.IsLeader)
            {
                Logger.Debug("GameFightPlacement::Option non leader player wants to lock : " + entity.Name);
                fighter.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            fighter.Team.OptionLock(optionType);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="message"></param>
        private void FightReady(EntityBase entity, string message)
        {
            var fighter = (FighterBase)entity;
            
            fighter.Fight.FighterReady(fighter);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="message"></param>
        private void FightPlacement(EntityBase entity, string message)
        {
            var fighter = (FighterBase)entity;

            if(fighter.TurnReady)
            {
                Logger.Debug("GameFightPlacement::Placement turn ready, unable to move anymore : " + entity.Name);
                fighter.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            int cellId = -1;
            if(!int.TryParse(message.Substring(2), out cellId))
            {
                Logger.Debug("GameFightPlacement::Placement unable to parse cell id : " + entity.Name);
                fighter.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }
                    
            fighter.Fight.FighterPlacementChange(fighter, cellId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="message"></param>
        private void GameQuit(EntityBase entity, string message)
        {
            var fighter = (FighterBase)entity;
                       
            if(message == "GQ")
            {
                fighter.Fight.AddMessage(() => fighter.Fight.FightQuit(fighter));
                return;
            }

            if (!fighter.IsLeader)
            {
                Logger.Debug("FightPlacement::Quit non leader player trying to kick : " + entity.Name);
                fighter.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            long fighterId = -1;
            if (!long.TryParse(message.Substring(2), out fighterId))
            {
                Logger.Debug("FightPlacement::Quit unable to parse fighterId : " + entity.Name);
                fighter.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            var selectedFighter = fighter.Team.GetFighter(fighterId);

            if (selectedFighter == null)
            {
                Logger.Debug("FightPlacement::Quit unknow fighter : " + entity.Name);
                fighter.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            if(selectedFighter.IsLeader)
            {
                Logger.Debug("FightPlacement::Quit unable to kick leader : " + entity.Name);
                fighter.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            fighter.Fight.AddMessage(() => fighter.Fight.FightQuit(selectedFighter, true));
        }
    }
}
