using log4net;
namespace Codebreak.Service.World.Game.Fight.AI.Action
{
    public enum AIActionResult
    {
        Success,
        Failure,
        Running
    }

    public enum AIActionState
    {
        Initialize,
        Execute,
        Finish,
    }

    public abstract class AIAction
    {
        protected static ILog Logger = LogManager.GetLogger(typeof(AIAction));

        public bool Timedout
        {
            get
            {
                return WorldService.Instance.LastUpdate > m_timeout;
            }
        }

        private long m_timeout;
        public long Timeout
        {
            set
            {
                m_timeout = WorldService.Instance.LastUpdate + value;
            }
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
            State = AIActionState.Initialize;
        }

        public virtual AIActionResult Initialize()
        {
            return AIActionResult.Failure;
        }

        public virtual AIActionResult Execute()
        {
            return AIActionResult.Failure;
        }

        public virtual AIActionResult Finish()
        {
            return AIActionResult.Failure;
        }

        public virtual void Update()
        {
            switch(State)
            {
                case AIActionState.Initialize: State = Initialize() != AIActionResult.Running ? AIActionState.Finish : AIActionState.Execute ; break;
                case AIActionState.Execute: State = Execute() != AIActionResult.Running ? State = AIActionState.Finish : State; break;
                case AIActionState.Finish: Finish(); break;
            }
        }

        public AIAction LinkWith(AIAction action)
        {
            NextAction = action;
            return action;
        }
    }
}
