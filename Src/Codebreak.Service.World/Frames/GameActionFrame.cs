using System;
using System.Linq;
using Codebreak.Framework.Network;
using Codebreak.Service.World.Game;
using Codebreak.Service.World.Game.Action;
using Codebreak.Service.World.Game.Entity;
using Codebreak.Service.World.Game.Fight;
using Codebreak.Service.World.Game.Map;

namespace Codebreak.Service.World.Frames
{
    public sealed class GameActionFrame : FrameBase<GameActionFrame, EntityBase, string>
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
                        case 'A':
                            return GameActionStart;

                        case 'K':
                            switch (message[2])
                            {
                                case 'K':
                                    return GameActionFinish;

                                case 'E':
                                    return GameActionAbort;

                                default:
                                    return null;
                            }

                        default:
                            return null;
                    }
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="message"></param>
        private void GameActionStart(EntityBase entity, string message)
        {
            var actionId = -1;
            if (!int.TryParse(message.Substring(2, 3), out actionId))
            {
                Logger.Debug("GameActionFrame::Start failed to parse actionId : " + entity.Name);
                entity.SafeDispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            if(!Enum.IsDefined(typeof(GameActionTypeEnum), actionId))
            {
                Logger.Debug("GameActionFrame::Start unknow action id  : " + actionId + " : " + entity.Name);
                entity.SafeDispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            entity.AddMessage(() =>
            {
                var actionType = (GameActionTypeEnum)actionId;
                if (!entity.CanGameAction(actionType))
                {
                    Logger.Debug("GameActionFrame::Start entity cant game action : " + entity.Name);
                    entity.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                    return;
                }

                switch (actionType)
                {
                    case GameActionTypeEnum.MAP_MOVEMENT:
                        GameMapMovement(entity, message);
                        break;

                    case GameActionTypeEnum.CHALLENGE_REQUEST: 
                        GameChallengeRequest(entity, message);
                        break;

                    case GameActionTypeEnum.CHALLENGE_ACCEPT:
                        GameChallengeAccept(entity, message);
                        break;

                    case GameActionTypeEnum.CHALLENGE_DECLINE:
                        GameChallengeDeny(entity, message);
                        break;

                    case GameActionTypeEnum.FIGHT_JOIN:
                        GameChallengeJoin(entity, message);                        
                        break;

                    case GameActionTypeEnum.FIGHT_SPELL_LAUNCH:
                        GameFightSpellLaunch(entity, message);
                        break;

                    case GameActionTypeEnum.FIGHT_WEAPON_USE:
                        GameWeaponUse(entity, message);
                        break;
                }
            }); 
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="message"></param>
        private void GameWeaponUse(EntityBase entity, string message)
        {
            var cellId = -1;

            if(!int.TryParse(message.Substring(5), out cellId))
            {
                entity.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            var fighter = (FighterBase)entity;

            fighter.Fight.TryUseWeapon(fighter, cellId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="message"></param>
        private void GameChallengeJoin(EntityBase entity, string message)
        {
            var fightData = message.Substring(5).Split(';');
            var fightId = int.Parse(fightData[0]);
            var fight = entity.Map.FightManager.GetFight(fightId);

            if(fight == null)
            {
                Logger.Debug("GameActionFrame::ChallengeJoin unknow fight : " + entity.Name);
                entity.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            if(fightData.Length == 1)
            {
                fight.TrySpectate((FighterBase)entity);
                return;
            }
            
            long leaderId = -1;
            if(!long.TryParse(fightData[1], out leaderId))
            {                
                Logger.Debug("GameActionFrame::ChallengeJoin unknow leaderId : " + entity.Name);
                entity.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            fight.TryJoin((FighterBase)entity, leaderId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="message"></param>
        private void GameFightSpellLaunch(EntityBase entity, string message)
        {
            if(!message.Contains(';'))
            {
                Logger.Debug("GameActionFrame::SpellLaunch wrong packet content : " + entity.Name);
                entity.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            var spellData = message.Substring(5).Split(';');
            if(spellData.Length < 2)
            {
                Logger.Debug("GameActionFrame::SpellLaunch wrong packet content : " + entity.Name);
                entity.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            var spellId = -1;
            if(!int.TryParse(spellData[0], out spellId))
            {
                Logger.Debug("GameActionFrame::SpellLaunch wrong packet content : " + entity.Name);
                entity.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            var cellId = -1;
            if(!int.TryParse(spellData[1], out cellId))
            {
                Logger.Debug("GameActionFrame::SpellLaunch wrong packet content : " + entity.Name);
                entity.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            var fighter = (FighterBase)entity;

            fighter.Fight.TryLaunchSpell(fighter, spellId, cellId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="message"></param>
        private void GameChallengeDeny(EntityBase entity, string message)
        {
            entity.AbortAction(GameActionTypeEnum.CHALLENGE_REQUEST, entity.Id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="message"></param>
        private void GameChallengeAccept(EntityBase entity, string message)
        {
            entity.StopAction(GameActionTypeEnum.CHALLENGE_REQUEST, entity.Id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="message"></param>
        private void GameChallengeRequest(EntityBase entity, string message)
        {
            if (entity.Map.FightTeam0Cells.Count == 0 || entity.Map.FightTeam1Cells.Count == 0)
            {
                entity.Dispatch(WorldMessage.INFORMATION_MESSAGE(InformationTypeEnum.INFO, InformationEnum.INFO_SERVER_MESSAGE, "Aucune cell de combat n'est disponible sur cette map."));
                return;
            }
            
            long distantEntityId = -1;
            if(!long.TryParse(message.Substring(5), out distantEntityId))
            {
                Logger.Debug("GameActionFrame::ChallengeRequest failed to parse distantEntityId : " + entity.Name);
                entity.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            var distantEntity = entity.Map.GetEntity(distantEntityId);
            if(distantEntity == null)
            {
                Logger.Debug("GameActionFrame::ChallengeRequest unknow distantEntityId " + entity.Name);
                entity.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            if(distantEntity.Type != EntityTypEnum.TYPE_CHARACTER)
            {
                Logger.Debug("GameActionFrame::ChallengeRequest trying to challenge non player entity : " + entity.Name);
                entity.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }
            
            if(!distantEntity.CanGameAction(GameActionTypeEnum.CHALLENGE_REQUEST))
            {
                Logger.Debug("GameActionFrame::ChallengeRequest distantEntity cannot start a request, probably in another action or restricted : " + distantEntity.Name);
                entity.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            ((CharacterEntity)entity).ChallengePlayer((CharacterEntity)distantEntity);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="message"></param>
        private void GameMapMovement(EntityBase entity, string message)
        {
            if(entity.MovementHandler == null)
            {
                Logger.Debug("GameActionFrame::MapMovement entity is not on a map : " + entity.Name);
                entity.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            if (entity.HasEntityRestriction(EntityRestrictionEnum.RESTRICTION_IS_TOMBESTONE))
            {
                Logger.Debug("GameActionFrame::MapMovement Tombestone entity trying to move : " + entity.Name);
                entity.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }
            
            switch(entity.MovementHandler.FieldType)
            {
                case FieldTypeEnum.TYPE_MAP:
                    entity.MovementHandler.Move(entity, entity.CellId, message.Substring(5));
                    break;
                case FieldTypeEnum.TYPE_FIGHT:
                    var fighter = (FighterBase)entity;
                    fighter.Fight.Move(entity, fighter.Cell.Id, message.Substring(5));
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="message"></param>
        private void GameActionAbort(EntityBase entity, string message)
        {
            if(!message.Contains('|'))
            {
                entity.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            var abortData = message.Split('|');
            if (abortData.Length < 2)
            {
                entity.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            var actionId = -1;
            if (!int.TryParse(abortData[0].Substring(3), out actionId))
            {
                Logger.Debug("GameActionFrame::Abort unable to finish action, unknow id : " + entity.Name);
                entity.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            var actionArgs = abortData[1];

            var action = entity.CurrentAction;
            if (action == null)
            {
                Logger.Debug("GameActionFrame::GameActionFinish entity has empty action : " + entity.Name);
                entity.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            entity.AbortAction(entity.CurrentAction.Type, actionArgs);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="message"></param>
        private void GameActionFinish(EntityBase entity, string message)
        {
            var actionId = -1;
            if (!int.TryParse(message.Substring(3), out actionId))
            {
                Logger.Debug("GameActionFrame::Finish unable to finish action, unknow id : " + entity.Name);
                entity.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            var action = entity.CurrentAction;
            if (action == null)
            {
                entity.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            entity.StopAction(action.Type);
        }
    }
}
