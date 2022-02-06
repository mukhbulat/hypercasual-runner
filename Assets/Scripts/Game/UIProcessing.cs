using System;
using System.Collections.Generic;
using Game.States;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class UIProcessing : MonoBehaviour
    {
        
        #region Fields and Properties

        // General
        [SerializeField] private PlayerStats playerStats;
        [SerializeField] private List<Canvas> listOfCanvases;
        [SerializeField] private List<MonoBehaviour> listOfRestartableScripts;
        [SerializeField] private PlayerMovement playerMovement;

        private List<IRestartable> _listOfIRestartables;

        // Health
        [SerializeField] private List<Sprite> heartSprites;
        [SerializeField] private Image healthUI;
        
        // Score
        [SerializeField] private List<Color> scoreColors;
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private TextMeshProUGUI coinsText;
    
        // Buttons
        [SerializeField] private List<Sprite> soundButtonSprites;
        [SerializeField] private Image soundButtonImage;
        private bool _soundEnabled = true;
        
        // State Machine
        private GameStateMachine _stateMachine;
        // States
        private LostState _lostState;
        private MenuState _menuState;
        private PauseState _pauseState;
        private PlayState _playState;
        
        #endregion

        private void Awake()
        {
            // Health
            playerStats.HealthChange += OnHealthChange;
            // Score
            playerStats.ScoreChange += OnScoreChange;
            playerStats.MultiplierChange += OnMultiplierChange;
            // Coins
            playerStats.CoinsChange += OnCoinsChange;

            // State Machine
            _stateMachine = new GameStateMachine();

            _lostState = new LostState(this, _stateMachine);
            _menuState = new MenuState(this, _stateMachine);
            _pauseState = new PauseState(this, _stateMachine);
            _playState = new PlayState(this, _stateMachine);

            _stateMachine.Initialize(_playState);

            // Checking restartable scripts.
            _listOfIRestartables = new List<IRestartable>(listOfRestartableScripts.Count);
            foreach (var script in listOfRestartableScripts)
            {
                if (script is IRestartable restartable)
                {
                    _listOfIRestartables.Add(restartable);
                }
            }
        }


    
        private void Update()
        {
            _stateMachine.CurrentGameState.LogicUpdate();
        }

        public void DisableAllCanvases()
        {
            foreach (var canvas in listOfCanvases)
            {
                canvas.enabled = false;
            }
        }

        public void EnableCertainCanvas(int index)
        {
            listOfCanvases[index].enabled = true;
        }

        public void GamePause(bool isPaused)
        {
            Time.timeScale = isPaused ? 0 : 1;
        }
    
        private void OnHealthChange(int newHealth)
        {
            if (newHealth <= 0)
            {
                _stateMachine.ChangeState(_lostState);
            }
            else
            {
                healthUI.sprite = heartSprites[newHealth - 1];
            }
        }

        private void OnMultiplierChange(int multiplier)
        {
            scoreText.color = scoreColors[multiplier - 1];
        }

        private void OnScoreChange(int score)
        {
            scoreText.text = Convert.ToString(score);
        }

        private void OnCoinsChange(int coins)
        {
            coinsText.text = Convert.ToString(coins);
        }

        public void Restart()
        {
            foreach (var restartable in _listOfIRestartables)
            {
                restartable.Restart();
            }
        }

        public void EnableMovement(bool isEnabled)
        {
            playerMovement.enabled = isEnabled;
        }

        #region Buttons
        
        public void OnHomeButtonClick()
        {
            _stateMachine.ChangeState(_menuState);
        }

        public void OnPlayButtonClick()
        {
            Restart();
            _stateMachine.ChangeState(_playState);
        }

        public void OnPauseButtonClick()
        {
            _stateMachine.ChangeState(_pauseState);
        }

        public void OnResumeButtonClick()
        {
            _stateMachine.ChangeState(_playState);
        }

        public void OnSoundButtonClick()
        {
            if (_soundEnabled)
            {
                _soundEnabled = false;
                soundButtonImage.sprite = soundButtonSprites[0];
            }
            else
            {
                _soundEnabled = true;
                soundButtonImage.sprite = soundButtonSprites[1];
            }
            // TODO Sound Manager.
        }
        #endregion
        
    }
}