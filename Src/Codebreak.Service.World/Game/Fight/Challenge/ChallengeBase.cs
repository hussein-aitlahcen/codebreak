using Codebreak.Service.World.Database.Structure;
using Codebreak.Service.World.Game.Fight.Effect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Game.Fight.Challenges
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class ChallengeBase : MessageDispatcher
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
        /// <param name="id"></param>
        /// <param name="fight"></param>
        public ChallengeBase(ChallengeTypeEnum type)
        {
            Id = (int)type;
            Success = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fighter"></param>
        public virtual void BeginTurn(FighterBase fighter)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fighter"></param>
        public virtual void EndTurn(FighterBase fighter)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fighter"></param>
        /// <param name="castInfos"></param>
        public virtual void CheckSpell(FighterBase fighter, CastInfos castInfos)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="beginCell"></param>
        /// <param name="endCell"></param>
        /// <param name="length"></param>
        public virtual void CheckMovement(int beginCell, int endCell, int length)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fighter"></param>
        /// <param name="weaponTemplate"></param>
        public virtual void CheckWeapon(FighterBase fighter, ItemTemplateDAO weaponTemplate)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fighter"></param>
        public virtual void CheckDeath(FighterBase fighter)
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
        public virtual void OnFailed()
        {
            if(!Success && !Failed)
            {
                Success = false;
                Failed = true;
                base.Dispatch(WorldMessage.FIGHT_CHALLENGE_FAILED(Id));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cellId"></param>
        public void FlagCell(int cellId)
        {
            base.Dispatch(WorldMessage.FIGHT_CELL_FLAG(cellId));
        }
    }
}
