namespace Game.States
{
    public class PlayState : GameState
    {
        public PlayState(UIProcessing uiProcessing, GameStateMachine stateMachine) : base(uiProcessing, stateMachine)
        {
        }
        
        public override void Enter()
        {
            base.Enter();
            Process.EnableCertainCanvas(0);
            Process.GamePause(false);
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