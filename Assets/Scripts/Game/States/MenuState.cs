namespace Game.States
{
    public class MenuState : GameState
    {
        public MenuState(UIProcessing uiProcessing, GameStateMachine stateMachine) : base(uiProcessing, stateMachine)
        {
        }
        
        public override void Enter()
        {
            base.Enter();
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