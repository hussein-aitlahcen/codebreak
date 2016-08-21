using Codebreak.Service.World.Game.Map;
using log4net;
namespace Codebreak.Service.World.Game.Fight.AI.Action
{
    public enum AIActionResult
    {
        SUCCESS,
        FAILURE,
        RUNNING
    }

    public enum AIActionState
    {
        INITIALIZE,
        EXECUTE,
        FINISH,
    }

    public abstract class AIAction
    {
        protected static ILog Logger = LogManager.GetLogger(typeof(AIAction));

        public bool Timedout => m_timeout <= Fighter.Fight.UpdateTime;

        private long m_timeout;
        public long Timeout
        {
            set
            {
                m_timeout = Fighter.Fight.UpdateTime + value;
            }
        }

        public MapInstance Map
        {
            get;
            private set;
        }

        public AbstractFight Fight
        {
            get;
            private set;
        }

        public AIFighter Fighter
        {
            get; 
            private set;
        }

        public AIActionState State
        {
            get;
            protected set;
        }

        public AIAction NextAction
        {
            get;
            private set;
        }

        protected AIAction(AIFighter fighter)
        {
            Fighter = fighter;
            Fight = Fighter.Fight;
            Map = Fight.Map;
            State = AIActionState.INITIALIZE;
        }

        public virtual AIActionResult Initialize()
        {
            return AIActionResult.FAILURE;
        }

        public virtual AIActionResult Execute()
        {
            return AIActionResult.FAILURE;
        }

        public virtual AIActionResult Finish()
        {
            return AIActionResult.FAILURE;
        }

        public virtual void Update()
        {
            switch(State)
            {
                case AIActionState.INITIALIZE: State = Initialize() != AIActionResult.RUNNING ? AIActionState.FINISH : AIActionState.EXECUTE ; break;
                case AIActionState.EXECUTE: State = Execute() != AIActionResult.RUNNING ? State = AIActionState.FINISH : State; break;
                case AIActionState.FINISH: Finish(); break;
            }
        }

        public AIAction LinkWith(AIAction action)
        {
            NextAction = action;
            return action;
        }
    }
}
