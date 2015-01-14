using System;
using System.Linq;
using Codebreak.Framework.Network;
using Codebreak.Service.World.Game;
using Codebreak.Service.World.Game.Action;
using Codebreak.Service.World.Game.Entity;
using Codebreak.Service.World.Game.Fight;
using Codebreak.Service.World.Game.Map;
using Codebreak.Service.World.Network;
using Codebreak.Service.World.Game.Job;

namespace Codebreak.Service.World.Frame
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class GameActionFrame : FrameBase<GameActionFrame, CharacterEntity, string>
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

                case 'D':
                    switch(message[1])
                    {
                        case 'C':
                            return DialogCreate;
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
        private void DialogCreate(CharacterEntity character, string message)
        {
            long npcId = -1;
            if (!long.TryParse(message.Substring(2), out npcId))
            {
                character.SafeDispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            character.AddMessage(() =>
                {
                    var target = character.Map.GetEntity(npcId);
                    if(target == null || target.Type != EntityTypeEnum.TYPE_NPC)
                    {
                        character.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                        return;
                    }

                    var npc = (NonPlayerCharacterEntity)target;
                    if(npc.InitialQuestion == null)
                    {
                        character.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                        return;
                    }

                    if(!character.CanGameAction(GameActionTypeEnum.NPC_DIALOG))
                    {
                        character.Dispatch(WorldMessage.INFORMATION_MESSAGE(InformationTypeEnum.ERROR, InformationEnum.ERROR_YOU_ARE_AWAY));
                        return;
                    }

                    character.NpcDialogStart(npc);
                });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="message"></param>
        private void GameActionStart(CharacterEntity character, string message)
        {
            var actionId = -1;
            if (!int.TryParse(message.Substring(2, 3), out actionId))
            {
                character.SafeDispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            if(!Enum.IsDefined(typeof(GameActionTypeEnum), actionId))
            {
                character.SafeDispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            character.AddMessage(() =>
            {
                var actionType = (GameActionTypeEnum)actionId;
                if (!character.CanGameAction(actionType))
                {
                    Logger.Debug("GameActionFrame::Start entity cant game action : " + character.Name);
                    character.Dispatch(WorldMessage.INFORMATION_MESSAGE(InformationTypeEnum.ERROR, InformationEnum.ERROR_YOU_ARE_AWAY));
                    return;
                }

                switch (actionType)
                {
                    case GameActionTypeEnum.MAP_MOVEMENT:
                        GameMapMovement(character, message);
                        break;

                    case GameActionTypeEnum.CHALLENGE_REQUEST: 
                        GameChallengeRequest(character, message);
                        break;

                    case GameActionTypeEnum.CHALLENGE_ACCEPT:
                        GameChallengeAccept(character, message);
                        break;

                    case GameActionTypeEnum.CHALLENGE_DECLINE:
                        GameChallengeDeny(character, message);
                        break;

                    case GameActionTypeEnum.FIGHT_JOIN:
                        GameFightJoin(character, message);                        
                        break;

                    case GameActionTypeEnum.FIGHT_SPELL_LAUNCH:
                        GameFightSpellLaunch(character, message);
                        break;

                    case GameActionTypeEnum.FIGHT_WEAPON_USE:
                        GameWeaponUse(character, message);
                        break;

                    case GameActionTypeEnum.TAXCOLLECTOR_AGGRESSION:
                        GameTaxcollectorAggression(character, message);
                        break;

                    case GameActionTypeEnum.SKILL_USE:
                        GameSkillUse(character, message);
                        break;
                        
                    case GameActionTypeEnum.FIGHT_AGGRESSION:
                        GameAlignmentAggression(character, message);
                        break;
                }
            }); 
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="message"></param>
        private void GameSkillUse(CharacterEntity character, string message)
        {
            var skillData = message.Substring(5).Split(';');
            var cellId = int.Parse(skillData[0]);
            var skillId = int.Parse(skillData[1]);

            character.Map.AddMessage(() => 
                {
                    if(!character.CharacterJobs.HasSkill(skillId))
                    {
                        Logger.Debug("GameActionFrame::SkillUse character dont have the skill : " + (SkillIdEnum)skillId);
                        character.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                        return;
                    }

                    var action = character.CurrentAction as GameMapMovementAction;
                    if(action == null)
                    {
                        character.Map.InteractiveExecute(character, cellId, skillId);
                        return;
                    }
                    
                    action.SkillCellId = cellId;
                    action.SkillId = skillId;
                    action.SkillMapId = character.MapId;
                });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="message"></param>
        private void GameAlignmentAggression(CharacterEntity character, string message)
        {
            if (character.Map.FightTeam0Cells.Count == 0 || character.Map.FightTeam1Cells.Count == 0)
            {
                character.Dispatch(WorldMessage.SERVER_ERROR_MESSAGE("Cell pattern not found, unable to fight here"));
                return;
            }

            long victimId = -1;
            if (!long.TryParse(message.Substring(5), out victimId))
            {
                character.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            var distantEntity = character.Map.GetEntity(victimId);
            if (distantEntity == null)
            {
                Logger.Debug("GameActionFrame::AlignmentAggression unknow victimId " + character.Name);
                character.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            if (distantEntity.Type != EntityTypeEnum.TYPE_CHARACTER)
            {
                Logger.Debug("GameActionFrame::AlignmentAggression trying to aggro non taxcollector entity : " + character.Name);
                character.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            var victim = (CharacterEntity)distantEntity;
            if (!victim.CanGameAction(GameActionTypeEnum.FIGHT) && !victim.HasEntityRestriction(EntityRestrictionEnum.RESTRICTION_CANT_BE_ASSAULT))            
                victim.AbortAction(victim.CurrentAction.Type);

            // Active l'alignement de force s'il ne l'est pas.
            character.EnableAlignment();

            character.Map.FightManager.StartAggression(character, victim);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="message"></param>
        private void GameTaxcollectorAggression(CharacterEntity character, string message)
        {
            if (character.Map.FightTeam0Cells.Count == 0 || character.Map.FightTeam1Cells.Count == 0)
            {
                character.Dispatch(WorldMessage.INFORMATION_MESSAGE(InformationTypeEnum.INFO, InformationEnum.INFO_SERVER_MESSAGE, "Cell pattern not found, unable to fight here"));
                return;
            }

            long taxcollectorId = -1;
            if (!long.TryParse(message.Substring(5), out taxcollectorId))
            {
                character.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            var distantEntity = character.Map.GetEntity(taxcollectorId);
            if (distantEntity == null)
            {
                Logger.Debug("GameActionFrame::TaxcollectorAggression unknow taxcollectorId " + character.Name);
                character.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }
            
            if (distantEntity.Type != EntityTypeEnum.TYPE_TAX_COLLECTOR)
            {
                Logger.Debug("GameActionFrame::TaxCollectorAggression trying to aggro non taxcollector entity : " + character.Name);
                character.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            var taxCollector = distantEntity as TaxCollectorEntity;
            if(character.GuildMember != null && character.GuildMember.GuildId == taxCollector.Guild.Id)
            {
                character.Dispatch(WorldMessage.SERVER_ERROR_MESSAGE("You cannot aggro your own taxcollector."));
                character.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            if(!taxCollector.CanGameAction(GameActionTypeEnum.FIGHT))
            {
                character.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            character.Map.FightManager.StartTaxCollectorAggression(character, taxCollector);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="message"></param>
        private void GameWeaponUse(CharacterEntity character, string message)
        {
            var cellId = -1;
            if(!int.TryParse(message.Substring(5), out cellId))
            {
                character.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            character.Fight.TryUseWeapon(character, cellId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="message"></param>
        private void GameFightJoin(CharacterEntity character, string message)
        {
            var fightData = message.Substring(5).Split(';');
            var fightId = int.Parse(fightData[0]);
            var fight = character.Map.FightManager.GetFight(fightId);

            if(fight == null)
            {
                Logger.Debug("GameActionFrame::ChallengeJoin unknow fight : " + character.Name);
                character.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            if(fightData.Length == 1)
            {
                fight.TrySpectate(character);
                return;
            }
            
            long leaderId = -1;
            if(!long.TryParse(fightData[1], out leaderId))
            {                
                Logger.Debug("GameActionFrame::ChallengeJoin unknow leaderId : " + character.Name);
                character.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            fight.TryJoin(character, leaderId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="message"></param>
        private void GameFightSpellLaunch(CharacterEntity character, string message)
        {
            if(!message.Contains(';'))
            {
                character.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            var spellData = message.Substring(5).Split(';');
            if(spellData.Length < 2)
            {
                character.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            var spellId = -1;
            if(!int.TryParse(spellData[0], out spellId))
            {
                character.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            var cellId = -1;
            if(!int.TryParse(spellData[1], out cellId))
            {
                Logger.Debug("GameActionFrame::SpellLaunch wrong packet content : " + character.Name);
                character.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            character.Fight.TryLaunchSpell(character, spellId, cellId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="message"></param>
        private void GameChallengeDeny(CharacterEntity character, string message)
        {
            character.AbortAction(GameActionTypeEnum.CHALLENGE_REQUEST);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="message"></param>
        private void GameChallengeAccept(CharacterEntity character, string message)
        {
            character.StopAction(GameActionTypeEnum.CHALLENGE_REQUEST);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="message"></param>
        private void GameChallengeRequest(CharacterEntity character, string message)
        {
            if (character.Map.FightTeam0Cells.Count == 0 || character.Map.FightTeam1Cells.Count == 0)
            {
                character.Dispatch(WorldMessage.INFORMATION_MESSAGE(InformationTypeEnum.INFO, InformationEnum.INFO_SERVER_MESSAGE, "Cell pattern not found, unable to fight here"));
                return;
            }
            
            long distantEntityId = -1;
            if(!long.TryParse(message.Substring(5), out distantEntityId))
            {
                character.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            var distantEntity = character.Map.GetEntity(distantEntityId);
            if(distantEntity == null)
            {
                Logger.Debug("GameActionFrame::ChallengeRequest unknow distantEntityId " + character.Name);
                character.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            if(distantEntity.Type != EntityTypeEnum.TYPE_CHARACTER)
            {
                Logger.Debug("GameActionFrame::ChallengeRequest trying to challenge non player entity : " + character.Name);
                character.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }
            
            if(!distantEntity.CanGameAction(GameActionTypeEnum.CHALLENGE_REQUEST))
            {
                character.Dispatch(WorldMessage.INFORMATION_MESSAGE(InformationTypeEnum.ERROR, InformationEnum.ERROR_PLAYER_AWAY_NOT_INVITABLE));
                return;
            }

            character.ChallengePlayer((CharacterEntity)distantEntity);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="message"></param>
        private void GameMapMovement(CharacterEntity character, string message)
        {
            if(character.MovementHandler == null)
            {
                Logger.Debug("GameActionFrame::MapMovement entity is not on a map : " + character.Name);
                character.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }
                        
            switch(character.MovementHandler.FieldType)
            {
                case FieldTypeEnum.TYPE_MAP:
                    character.MovementHandler.Move(character, character.CellId, message.Substring(5));
                    break;
                case FieldTypeEnum.TYPE_FIGHT:
                    var fighter = (FighterBase)character;
                    fighter.Fight.Move(character, fighter.Cell.Id, message.Substring(5));
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="message"></param>
        private void GameActionAbort(CharacterEntity character, string message)
        {
            if(!message.Contains('|'))
            {
                character.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            var abortData = message.Split('|');
            if (abortData.Length < 2)
            {
                character.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            var actionId = -1;
            if (!int.TryParse(abortData[0].Substring(3), out actionId))
            {
                character.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            var actionArgs = abortData[1];

            character.AddMessage(() =>
                {
                    var action = character.CurrentAction;
                    if (action == null)
                    {
                        Logger.Debug("GameActionFrame::GameActionFinish entity has empty action : " + character.Name);
                        character.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                        return;
                    }                    
                    character.AbortAction(character.CurrentAction.Type, actionArgs);
                });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="message"></param>
        private void GameActionFinish(CharacterEntity character, string message)
        {
            var actionId = -1;
            if (!int.TryParse(message.Substring(3), out actionId))
            {
                character.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            character.AddMessage(() =>
                {
                    var action = character.CurrentAction;
                    if (action == null)
                    {
                        character.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                        return;
                    }
                    character.StopAction(action.Type);
                });
        }
    }
}
