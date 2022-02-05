namespace Game.States
{
    public class PauseState : GameState
    {
        public PauseState(UIProcessing uiProcessing, GameStateMachine stateMachine) : base(uiProcessing, stateMachine)
        {
        }
        
        public override void Enter()
        {
            base.Enter();
            Process.EnableCertainCanvas(2);
            Process.GamePause(true);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}