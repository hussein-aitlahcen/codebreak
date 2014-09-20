namespace Codebreak.Service.World.Game.Fight.AI.Actions
{
    public abstract class AIAction
    {
        public AIFighter Fighter
        {
            get; 
            private set;
        }

        protected AIAction(AIFighter fighter)
        {
            Fighter = fighter;
        }

        public virtual AIActionResult Invoke()
        {
            return AIActionResult.Failure;
        }
    }
}
