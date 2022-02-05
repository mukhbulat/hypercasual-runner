namespace Game.States
{
    public class LostState : GameState
    {
        public LostState(UIProcessing uiProcessing, GameStateMachine stateMachine) : base(uiProcessing, stateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();
            Process.EnableCertainCanvas(1);
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