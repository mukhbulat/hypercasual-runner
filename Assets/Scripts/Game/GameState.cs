namespace Game
{
    public abstract class GameState
    {
        protected GameProcessing Process;
        protected GameStateMachine StateMachine;

        protected GameState(GameProcessing gameProcessing, GameStateMachine stateMachine)
        {
            Process = gameProcessing;
            StateMachine = stateMachine;
        }
        
        public virtual void Enter()
        {
            Process.DisableAllCanvases();
        }

        public virtual void LogicUpdate()
        {
            
        }

        public virtual void Exit()
        {
            
        }
    }
}