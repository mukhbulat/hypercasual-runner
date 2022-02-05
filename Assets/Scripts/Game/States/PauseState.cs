namespace Game.States
{
    public class PauseState : GameState
    {
        public PauseState(GameProcessing gameProcessing, GameStateMachine stateMachine) : base(gameProcessing, stateMachine)
        {
        }
        
        public override void Enter()
        {
            base.Enter();
            Process.EnableCertainCanvas(2);
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