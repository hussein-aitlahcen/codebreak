using Codebreak.Service.World.Frames;
using Codebreak.Service.World.Game.Action;
using Codebreak.Service.World.Game.Entity;
using Codebreak.Service.World.Game.Map;
using Codebreak.Service.World.Game.Spell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Codebreak.Service.World.Manager;
using Codebreak.Service.World.Network;

namespace Codebreak.Service.World.Game.Fight
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class FighterBase : EntityBase, IFightObstacle
    {

        #region IFightObstacle

        public FightObstacleTypeEnum ObstacleType
        {
            get
            {
                return FightObstacleTypeEnum.TYPE_FIGHTER;
            }
        }

        public bool CanGoThrough
        {
            get
            {
                return false;
            }
        }

        public bool CanStack
        {
            get
            {
                return false;
            }
        }

        #endregion

        #region EntityBase
               
        /// <summary>
        /// 
        /// </summary>
        public abstract override int MapId
        {
            get;
            set;
        }


        /// <summary>
        /// 
        /// </summary>
        public override abstract int BaseLife
        {
            get;
        }
               
        /// <summary>
        /// 
        /// </summary>
        public override abstract  int CellId
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public override abstract string Name
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        public override abstract int Level
        {
            get;
            set;
        }

        #endregion

        #region FighterBase

        /// <summary>
        /// 
        /// </summary>
        public abstract bool TurnReady
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public abstract bool TurnPass
        {
            get;
            set;
        }
        
        /// <summary>
        /// 
        /// </summary>
        public abstract int SkinBase
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        public abstract int SkinSizeBase
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        public int Skin
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int SkinSize
        {
            get;
            set;
        }

        #endregion

        #region Fighter
                
        /// <summary>
        /// 
        /// </summary>
        public bool Disconnected
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int DisconnectedTurnLeft
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Spectating
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public FightCell Cell
        {
            get;
            private set;
        }

        #endregion
        
        /// <summary>
        /// 
        /// </summary>
        public FightTeam Team
        {
            get;
            private set;
        }
        
        /// <summary>
        /// 
        /// </summary>
        public bool IsLeader
        {
            get
            {
                if (Team == null)
                    return false;
                return Team.LeaderId == Id;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsFighterDead
        {
            get
            {
                return Life <= 0;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool CanBeginTurn
        {
            get
            {
                return !IsFighterDead && Fight != null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int UsedAP
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int UsedMP
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int MaxAP
        {
            get
            {
                return Statistics.GetTotal(EffectEnum.AddAP);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int MaxMP
        {
            get
            {
                return Statistics.GetTotal(EffectEnum.AddMP);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int AP
        {
            get
            {
                return MaxAP - UsedAP;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int MP
        {
            get
            {
                return MaxMP - UsedMP;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int APDodge
        {
            get
            {
                return (int)Math.Floor((double)Statistics.GetTotal(EffectEnum.AddWisdom) / 4) + Statistics.GetTotal(EffectEnum.AddAPDodge);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int MPDodge
        {
            get
            {
                return (int)Math.Floor((double)Statistics.GetTotal(EffectEnum.AddWisdom) / 4) + Statistics.GetTotal(EffectEnum.AddAPDodge);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public FighterBase Invocator
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public BuffEffectManager BuffManager
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public FighterStateManager StateManager
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public SpellCastManager SpellManager
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public FighterBase(EntityTypEnum type, long id)
            : base(type, id)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        public new void Move(MovementPath path)
        {
            CurrentAction = new GameFightMovementAction(this, path);

            StartAction(GameActionTypeEnum.MAP_MOVEMENT);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cellId"></param>
        /// <param name="spellId"></param>
        /// <param name="spellLevel"></param>
        /// <param name="sprite"></param>
        /// <param name="spriteInfos"></param>
        /// <param name="duration"></param>
        /// <param name="callback"></param>
        public void LaunchSpell(int cellId, int spellId, int spellLevel, string sprite, string spriteInfos, long duration, System.Action callback)
        {
            CurrentAction = new GameFightSpellAction(this, cellId, spellId, spellLevel, sprite, spriteInfos, duration, callback);

            StartAction(GameActionTypeEnum.FIGHT_SPELL_LAUNCH);
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void JoinFight(FightBase fight, FightTeam team)
        {
            BuffManager = new BuffEffectManager(this);
            StateManager = new FighterStateManager(this);
            SpellManager = new SpellCastManager();

            Orientation = 1;
            Skin = SkinBase;
            SkinSize = SkinSizeBase;
            UsedAP = 0;
            UsedMP = 0;

            Fight = fight;
            Team = team;
            TurnReady = false;

            Team.AddFighter(this);

            SetCell(team.FreePlace);

            StartAction(GameActionTypeEnum.FIGHT);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fight"></param>
        public void JoinSpectator(FightBase fight)
        {
            Fight = fight;
            Spectating = true;

            Fight.SpectatorTeam.AddSpectator(this);
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void LeaveFight(bool kicked = false)
        {
            if (Spectating)
            {
                Fight.SpectatorTeam.RemoveSpectator(this);
            }
            else
            {
                Team.RemoveFighter(this);
                if (!kicked)
                {
                    Fight.Result.AddResult(this, false, true);
                }
            }

            EndFight();
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void EndFight(bool win = false)
        {
            StopAction(GameActionTypeEnum.FIGHT);

            Team.RemoveHandler(base.Dispatch);

            if (!Spectating)
            {
                switch (Fight.Type)
                {
                        // On rend la vie aux joueur en pvp
                    case FightTypeEnum.TYPE_CHALLENGE:
                        Life = MaxLife;
                        break;

                    case FightTypeEnum.TYPE_PVM:
                        if (Type == EntityTypEnum.TYPE_CHARACTER)
                            Life = MaxLife; // 
                        break;
                }

                Fight.TurnProcessor.RemoveFighter(this);
                Statistics.ClearBoosts();
            }

            if (Disconnected)
            {
                EntityManager.Instance.RemoveCharacter((CharacterEntity)this);
            }

            SetCell(null);
            Team = null;
            Fight = null;

            Spectating = false;
            Disconnected = false;
            Invocator = null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual FightActionResultEnum BeginTurn()
        {
            TurnPass = false;

            var buffResult = BuffManager.BeginTurn();
            if (buffResult != FightActionResultEnum.RESULT_NOTHING)
                return buffResult;

            return Cell.BeginTurn(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual FightActionResultEnum MiddleTurn()
        {
            UsedAP = 0;
            UsedMP = 0;

            return FightActionResultEnum.RESULT_NOTHING;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual FightActionResultEnum EndTurn()
        {
            if (Disconnected)
            {
                if (DisconnectedTurnLeft == 0)
                {
                    Fight.Dispatch(WorldMessage.INFORMATION_MESSAGE(InformationTypeEnum.ERROR, InformationEnum.ERROR_FIGHTER_KICKED_DUE_TO_DISCONNECTION, Name));

                    if (Fight.FightQuit(this) == FightActionResultEnum.RESULT_END)                    
                        return FightActionResultEnum.RESULT_END;                    
                }
                else
                {
                    Fight.Dispatch(WorldMessage.INFORMATION_MESSAGE(InformationTypeEnum.INFO, InformationEnum.INFO_FIGHT_DISCONNECT_TURN_REMAIN, Name, DisconnectedTurnLeft));
                }

                DisconnectedTurnLeft--;
            }

            SpellManager.EndTurn();

            return BuffManager.EndTurn();
        }

        /// <summary>
        /// Calcul damages
        /// </summary>
        /// <param name="effect"></param>
        /// <param name="jet"></param>
        public void CalculDamages(EffectEnum effect, ref int jet)
        {
            switch (effect)
            {
                case EffectEnum.DamageEarth:
                case EffectEnum.StealEarth:
                case EffectEnum.DamageNeutral:
                case EffectEnum.StealNeutral:
                    jet = (int)Math.Floor((double)jet * (100 + Statistics.GetTotal(EffectEnum.AddStrength) + Statistics.GetTotal(EffectEnum.AddDamagePercent)) / 100 +
                                                  Statistics.GetTotal(EffectEnum.AddDamagePhysic) + Statistics.GetTotal(EffectEnum.AddDamage));
                    break;

                case EffectEnum.DamageFire:
                case EffectEnum.StealFire:
                    jet = (int)Math.Floor((double)jet * (100 + Statistics.GetTotal(EffectEnum.AddIntelligence) + Statistics.GetTotal(EffectEnum.AddDamagePercent)) / 100 +
                                           Statistics.GetTotal(EffectEnum.AddDamageMagic) + Statistics.GetTotal(EffectEnum.AddDamage));
                    break;

                case EffectEnum.DamageAir:
                case EffectEnum.StealAir:
                    jet = (int)Math.Floor((double)jet * (100 + Statistics.GetTotal(EffectEnum.AddAgility) + Statistics.GetTotal(EffectEnum.AddDamagePercent)) / 100 +
                                           Statistics.GetTotal(EffectEnum.AddDamageMagic) + Statistics.GetTotal(EffectEnum.AddDamage));
                    break;

                case EffectEnum.DamageWater:
                case EffectEnum.StealWater:
                    jet = (int)Math.Floor((double)jet * (100 + Statistics.GetTotal(EffectEnum.AddChance) + Statistics.GetTotal(EffectEnum.AddDamagePercent)) / 100 +
                                           Statistics.GetTotal(EffectEnum.AddDamageMagic) + Statistics.GetTotal(EffectEnum.AddDamage));
                    break;
            }
        }

        /// <summary>
        /// Calcul reuced damages
        /// </summary>
        /// <param name="Effect"></param>
        /// <param name="Damages"></param>
        public void CalculReduceDamages(EffectEnum Effect, ref int Damages)
        {
            switch (Effect)
            {
                case EffectEnum.DamageNeutral:
                case EffectEnum.StealNeutral:
                    Damages = Damages * (100 - Statistics.GetTotal(EffectEnum.AddReduceDamagePercentNeutral) - Statistics.GetTotal(EffectEnum.AddReduceDamagePercentPvPNeutral)) / 100
                                             - Statistics.GetTotal(EffectEnum.AddReduceDamageNeutral) - Statistics.GetTotal(EffectEnum.AddReduceDamagePvPNeutral);
                    break;

                case EffectEnum.DamageEarth:
                case EffectEnum.StealEarth:
                    Damages = Damages * (100 - Statistics.GetTotal(EffectEnum.AddReduceDamagePercentEarth) - Statistics.GetTotal(EffectEnum.AddReduceDamagePercentPvPEarth)) / 100
                                             - Statistics.GetTotal(EffectEnum.AddReduceDamageEarth) - Statistics.GetTotal(EffectEnum.AddReduceDamagePvPEarth);
                    break;

                case EffectEnum.DamageFire:
                case EffectEnum.StealFire:
                    Damages = Damages * (100 - Statistics.GetTotal(EffectEnum.AddReduceDamagePercentFire) - Statistics.GetTotal(EffectEnum.AddReduceDamagePercentPvPFire)) / 100
                                             - Statistics.GetTotal(EffectEnum.AddReduceDamageFire) - Statistics.GetTotal(EffectEnum.AddReduceDamagePvPFire);
                    break;

                case EffectEnum.DamageAir:
                case EffectEnum.StealAir:
                    Damages = Damages * (100 - Statistics.GetTotal(EffectEnum.AddReduceDamagePercentAir) - Statistics.GetTotal(EffectEnum.AddReduceDamagePercentPvPAir)) / 100
                                             - Statistics.GetTotal(EffectEnum.AddReduceDamageAir) - Statistics.GetTotal(EffectEnum.AddReduceDamagePvPAir);
                    break;

                case EffectEnum.DamageWater:
                case EffectEnum.StealWater:
                    Damages = Damages * (100 - Statistics.GetTotal(EffectEnum.AddReduceDamagePercentWater) - Statistics.GetTotal(EffectEnum.AddReduceDamagePercentPvPWater)) / 100
                                             - Statistics.GetTotal(EffectEnum.AddReduceDamageWater) - Statistics.GetTotal(EffectEnum.AddReduceDamagePvPWater);
                    break;
            }
        }

        /// <summary>
        /// Calcul heal
        /// </summary>
        /// <param name="Heal"></param>
        public void CalculHeal(ref int Heal)
        {
            Heal = (int)Math.Floor((double)Heal * (100 + Statistics.GetTotal(EffectEnum.AddIntelligence)) / 100 + Statistics.GetTotal(EffectEnum.AddHealCare));
        }

        /// <summary>
        /// Calcul armor vs damage
        /// </summary>
        /// <param name="DamageEffect"></param>
        public int CalculArmor(EffectEnum DamageEffect)
        {
            switch (DamageEffect)
            {
                case EffectEnum.DamageEarth:
                case EffectEnum.StealEarth:
                case EffectEnum.DamageNeutral:
                case EffectEnum.StealNeutral:
                    return (Statistics.GetTotal(EffectEnum.AddArmorEarth) * Math.Max(1 + Statistics.GetTotal(EffectEnum.AddStrength) / 100, 1 + Statistics.GetTotal(EffectEnum.AddStrength) / 200 + Statistics.GetTotal(EffectEnum.AddIntelligence) / 200)) +
                           (Statistics.GetTotal(EffectEnum.AddArmor) * Math.Max(1 + Statistics.GetTotal(EffectEnum.AddStrength) / 100, 1 + Statistics.GetTotal(EffectEnum.AddStrength) / 200 + Statistics.GetTotal(EffectEnum.AddIntelligence) / 200));

                case EffectEnum.DamageFire:
                case EffectEnum.StealFire:
                    return (Statistics.GetTotal(EffectEnum.AddArmorFire) * Math.Max(1 + Statistics.GetTotal(EffectEnum.AddIntelligence) / 100, 1 + Statistics.GetTotal(EffectEnum.AddIntelligence) / 200 + Statistics.GetTotal(EffectEnum.AddIntelligence) / 200)) +
                           (Statistics.GetTotal(EffectEnum.AddArmor) * Math.Max(1 + Statistics.GetTotal(EffectEnum.AddIntelligence) / 100, 1 + Statistics.GetTotal(EffectEnum.AddIntelligence) / 200 + Statistics.GetTotal(EffectEnum.AddIntelligence) / 200));

                case EffectEnum.DamageAir:
                case EffectEnum.StealAir:
                    return (Statistics.GetTotal(EffectEnum.AddArmorAir) * Math.Max(1 + Statistics.GetTotal(EffectEnum.AddAgility) / 100, 1 + Statistics.GetTotal(EffectEnum.AddAgility) / 200 + Statistics.GetTotal(EffectEnum.AddIntelligence) / 200)) +
                           (Statistics.GetTotal(EffectEnum.AddArmor) * Math.Max(1 + Statistics.GetTotal(EffectEnum.AddAgility) / 100, 1 + Statistics.GetTotal(EffectEnum.AddAgility) / 200 + Statistics.GetTotal(EffectEnum.AddIntelligence) / 200));

                case EffectEnum.DamageWater:
                case EffectEnum.StealWater:
                    return (Statistics.GetTotal(EffectEnum.AddArmorWater) * Math.Max(1 + Statistics.GetTotal(EffectEnum.AddChance) / 100, 1 + Statistics.GetTotal(EffectEnum.AddChance) / 200 + Statistics.GetTotal(EffectEnum.AddIntelligence) / 200)) +
                           (Statistics.GetTotal(EffectEnum.AddArmor) * Math.Max(1 + Statistics.GetTotal(EffectEnum.AddChance) / 100, 1 + Statistics.GetTotal(EffectEnum.AddChance) / 200 + Statistics.GetTotal(EffectEnum.AddIntelligence) / 200));

            }

            return 0;
        }

        /// <summary>
        /// Calcul ap or mp dodged
        /// </summary>
        /// <param name="Caster"></param>
        /// <param name="lostPoint"></param>
        /// <param name="mp"></param>
        /// <returns></returns>
        public int CalculDodgeAPMP(FighterBase caster, int lostPoint, bool mp = false)
        {
            var RealLostPoint = 0;

            if (!mp)
            {
                var dodgeAPCaster = caster.APDodge + 1.1;
                var dodgeAPTarget = APDodge + 1.1;

                for (int i = 0; i < lostPoint; i++)
                {
                    var actualAP = AP - RealLostPoint;
                    var percentLastAP = actualAP / AP;
                    var chance = 0.5 * (dodgeAPCaster / dodgeAPTarget) * percentLastAP;
                    var percentChance = chance * 100;

                    if (percentChance > 100) percentChance = 90;
                    if (percentChance < 10) percentChance = 10;

                    if (Util.Next(0, 100) < percentChance) RealLostPoint++;
                }
            }
            else
            {
                var dodgeMPCaster = caster.MPDodge + 1.1;
                var dodgeMPTarget = MPDodge + 1.1;

                for (int i = 0; i < lostPoint; i++)
                {
                    var actualMP = MP - RealLostPoint;
                    var percentLastMP = actualMP / MP;
                    var chance = 0.5 * (dodgeMPCaster / dodgeMPTarget) * percentLastMP;
                    var percentChance = chance * 100;

                    if (percentChance > 100) percentChance = 90;
                    if (percentChance < 10) percentChance = 10;

                    if (Util.Next(0, 100) < percentChance) RealLostPoint++;
                }
            }

            return RealLostPoint;
        }

        /// <summary>
        /// 
        /// </summary>
        public void OnDeath()
        {
            if (Cell != null)
            {
                Cell.RemoveObject(this);

                Cell = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        public FightActionResultEnum SetCell(FightCell cell)
        {
            if (IsFighterDead)            
                return FightActionResultEnum.RESULT_DEATH;            

            if (Cell != null)
            {
                if (Cell == cell)
                    return FightActionResultEnum.RESULT_NOTHING;

                Cell.RemoveObject(this);
            }

            Cell = cell;

            if (Cell != null)
            {
                var moveResult = Cell.AddObject(this);
                if (moveResult != FightActionResultEnum.RESULT_NOTHING)
                    return moveResult;

                var buffResult = BuffManager.EndMove();
                if (buffResult != FightActionResultEnum.RESULT_NOTHING)
                    return buffResult;
            }

            if (Fight.State != FightStateEnum.STATE_FIGHTING)
                return FightActionResultEnum.RESULT_NOTHING;

            return Fight.TryKillFighter(this, Id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionType"></param>
        public override void StartAction(GameActionTypeEnum actionType)
        {
            switch(actionType)
            {
                case GameActionTypeEnum.FIGHT:
                    StopAction(GameActionTypeEnum.MAP);
                    Fight.AddUpdatable(this);
                    FrameManager.AddFrame(GameFightPlacementFrame.Instance);
                    break;

                case GameActionTypeEnum.FIGHT_SPELL_LAUNCH:
                    Fight.Dispatch(WorldMessage.GAME_ACTION(GameActionTypeEnum.FIGHT_SPELL_LAUNCH, Id, CurrentAction.SerializeAs_GameAction()));
                    break;

                case GameActionTypeEnum.MAP:
                    if (HasGameAction(GameActionTypeEnum.FIGHT))
                    {
                        if (StateManager.HasState(FighterStateEnum.STATE_STEALTH))
                        {
                            Team.Dispatch(WorldMessage.GAME_ACTION(actionType, Id, CurrentAction.SerializeAs_GameAction()));
                            return;
                        }
                    }
                    break;
            }

            base.StartAction(actionType);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionType"></param>
        /// <param name="args"></param>
        public override void StopAction(GameActionTypeEnum actionType, params object[] args)
        {
            switch(actionType)
            {
                case GameActionTypeEnum.FIGHT:
                    Fight.RemoveUpdatable(this);
                    WorldService.Instance.AddUpdatable(this);
                    FrameManager.AddFrame(GameCreationFrame.Instance);
                    FrameManager.RemoveFrame(GameFightPlacementFrame.Instance);
                    FrameManager.RemoveFrame(GameFightFrame.Instance);
                    break;
            }

            base.StopAction(actionType, args);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionType"></param>
        /// <param name="args"></param>
        public override void AbortAction(GameActionTypeEnum actionType, params object[] args)
        {
            switch(actionType)
            {
                case GameActionTypeEnum.FIGHT:
                    if(Fight != null)
                    {
                        Fight.FightDisconnect(this);
                    }
                    break;
            }

            base.AbortAction(actionType, args);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="message"></param>
        public override abstract void SerializeAs_GameMapInformations(OperatorEnum operation, StringBuilder message);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public override void SerializeAs_ShopItemsListInformations(StringBuilder message)
        {
            throw new NotImplementedException();
        }
    }
}
