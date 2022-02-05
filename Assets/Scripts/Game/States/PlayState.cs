namespace Game.States
{
    public class PlayState : GameState
    {
        public PlayState(GameProcessing gameProcessing, GameStateMachine stateMachine) : base(gameProcessing, stateMachine)
        {
        }
        
        public override void Enter()
        {
            base.Enter();
            Process.EnableCertainCanvas(0);
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