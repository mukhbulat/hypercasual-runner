using System;
using Game.States;
using UnityEngine;

namespace Game
{
    public class GameProcessing : MonoBehaviour
    {
        #region Fields And Properties

        private GameStateMachine _stateMachine;
        // States
        private LostState _lostState;
        private MenuState _menuState;
        private PauseState _pauseState;
        private PlayState _playState;
        

        #endregion

        #region Unity Functions

        private void Start()
        {
            _stateMachine = new GameStateMachine();

            _lostState = new LostState(this, _stateMachine);
            _menuState = new MenuState(this, _stateMachine);
            _pauseState = new PauseState(this, _stateMachine);
            _playState = new PlayState(this, _stateMachine);
            
            _stateMachine.Initialize(_playState);
        }

        #endregion
        public void DisableAllCanvases()
        {
            
        }
        
        
    }
}