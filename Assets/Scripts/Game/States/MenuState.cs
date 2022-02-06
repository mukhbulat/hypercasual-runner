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
            Process.EnableCertainCanvas(3);
            Process.GamePause(false);
            Process.Restart();
            Process.EnableMovement(false);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
        }

        public override void Exit()
        {
            base.Exit();
            Process.EnableMovement(true);
        }
    }
}