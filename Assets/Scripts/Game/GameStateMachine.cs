namespace Game
{
    public class GameStateMachine
    {
        public GameState CurrentGameState { get; private set; }

        public void Initialize(GameState startingState)
        {
            CurrentGameState = startingState;
            startingState.Enter();
        }

        public void ChangeState(GameState newState)
        {
            CurrentGameState.Exit();

            CurrentGameState = newState;
            CurrentGameState.Enter();
        }
    }
}