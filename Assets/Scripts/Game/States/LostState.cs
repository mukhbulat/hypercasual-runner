namespace Game.States
{
    public class LostState : GameState
    {
        public LostState(GameProcessing gameProcessing, GameStateMachine stateMachine) : base(gameProcessing, stateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();
            Process.EnableCertainCanvas(1);
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