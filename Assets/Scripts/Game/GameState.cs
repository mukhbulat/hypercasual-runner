namespace Game
{
    public abstract class GameState
    {
        protected UIProcessing Process;
        protected GameStateMachine StateMachine;

        protected GameState(UIProcessing uiProcessing, GameStateMachine stateMachine)
        {
            Process = uiProcessing;
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