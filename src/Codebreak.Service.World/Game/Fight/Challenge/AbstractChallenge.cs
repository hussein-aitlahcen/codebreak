using Codebreak.Service.World.Database.Structure;
using Codebreak.Service.World.Game.Fight.Effect;
using Codebreak.Service.World.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Game.Fight.Challenge
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class AbstractChallenge : MessageDispatcher
    {
        /// <summary>
        /// 
        /// </summary>
        public int Id
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Success
        {
            get;
            protected set;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Failed
        {
            get;
            protected set;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool ShowTarget
        {
            get;
            protected set;
        }

        /// <summary>
        /// 
        /// </summary>
        public long TargetId
        {
            get;
            protected set;
        }

        /// <summary>
        /// 
        /// </summary>
        public long BasicXpBonus
        {
            get;
            protected set;
        }

        /// <summary>
        /// 
        /// </summary>
        public long TeamXpBonus
        {
            get;
            protected set;
        }

        /// <summary>
        /// 
        /// </summary>
        public long BasicDropBonus
        {
            get;
            protected set;
        }

        /// <summary>
        /// 
        /// </summary>
        public long TeamDropBonus
        {
            get;
            protected set;
        }

        /// <summary>
        /// 
        /// </summary>
        public AbstractFighter Target
        {
            get;
            protected set;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="fight"></param>
        public AbstractChallenge(ChallengeTypeEnum type)
        {
            Id = (int)type;
            Success = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="team"></param>
        public virtual void StartFight(FightTeam team)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fighter"></param>
        public virtual void BeginTurn(AbstractFighter fighter)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fighter"></param>
        public virtual void EndTurn(AbstractFighter fighter)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fighter"></param>
        /// <param name="castInfos"></param>
        public virtual void CheckSpell(AbstractFighter fighter, CastInfos castInfos)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="beginCell"></param>
        /// <param name="endCell"></param>
        /// <param name="length"></param>
        public virtual void CheckMovement(AbstractFighter fighter, int beginCell, int endCell, int length)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fighter"></param>
        /// <param name="weaponTemplate"></param>
        public virtual void CheckWeapon(AbstractFighter fighter, ItemTemplateDAO weaponTemplate)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fighter"></param>
        public virtual void CheckDeath(AbstractFighter fighter)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void OnSuccess()
        {
            if (!Success && !Failed)
            {
                Success = true;
                Failed = false;
                base.Dispatch(WorldMessage.FIGHT_CHALLENGE_SUCCESS(Id));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void OnFailed(string name = "")
        {
            if(!Success && !Failed)
            {
                Success = false;
                Failed = true;
                base.CachedBuffer = true;
                base.Dispatch(WorldMessage.FIGHT_CHALLENGE_FAILED(Id));
                if(name != "")                
                    base.Dispatch(WorldMessage.INFORMATION_MESSAGE(InformationTypeEnum.INFO, InformationEnum.INFO_FIGHT_CHALLENGE_FAILED_DUE_TO, name));                
                base.CachedBuffer = false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cellId"></param>
        public void FlagCell(int cellId, long fighterId = 0)
        {
            base.Dispatch(WorldMessage.FIGHT_CELL_FLAG(cellId, fighterId));
        }
    }
}
