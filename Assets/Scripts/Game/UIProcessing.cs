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
        
        // Score and coins
        [SerializeField] private List<Color> scoreColors;
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private TextMeshProUGUI coinsText;
        [SerializeField] private TextMeshProUGUI loseScoreText;
        [SerializeField] private TextMeshProUGUI loseCoinsText;
        
        private int _scoreMultiplier = 1;
        private int _travelDistance = 0;
        private int _score = 0;
    
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
            // Checking restartable scripts.
            _listOfIRestartables = new List<IRestartable>(listOfRestartableScripts.Count);
            foreach (var restartableScript in listOfRestartableScripts)
            {
                IRestartable temporary = restartableScript as IRestartable;
                _listOfIRestartables.Add(temporary);
            }
            // Health
            playerStats.HealthChange += OnHealthChange;
            // Score
            playerStats.DistanceChange += OnDistanceChange;
            playerStats.MultiplierChange += OnMultiplierChange;
            // Coins
            playerStats.CoinsChange += OnCoinsChange;

            // State Machine
            _stateMachine = new GameStateMachine();

            _lostState = new LostState(this, _stateMachine);
            _menuState = new MenuState(this, _stateMachine);
            _pauseState = new PauseState(this, _stateMachine);
            _playState = new PlayState(this, _stateMachine);

            _stateMachine.Initialize(_menuState);
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
        
        public void Restart()
        {
            _scoreMultiplier = 1;
            _travelDistance = 0;
            _score = 0;
            foreach (var script in _listOfIRestartables)
            {
                script.Restart();
            }
        }

        public void EnableMovement(bool isEnabled)
        {
            playerMovement.IsEnabled = isEnabled;
        }

        public void EnableCertainCanvas(int index)
        {
            listOfCanvases[index].enabled = true;
        }

        public void GamePause(bool isPaused)
        {
            Time.timeScale = isPaused ? 0 : 1;
        }

        public void OnLose()
        {
            loseCoinsText.text = $"Coins: {playerStats.Coins}";
            loseScoreText.text = $"Score: {_score}";
        }

        #region State Independent Event Listeners

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
            _scoreMultiplier = multiplier;
        }

        private void OnDistanceChange(int distance)
        {
            // Bad code. idk, how to make it better.
            // Parenthesized expression always 1. so technically, I don't need distance here. 
            _score += (distance - _travelDistance) * _scoreMultiplier;
            _travelDistance = distance;
            
            scoreText.text = Convert.ToString(_score);
        }

        private void OnCoinsChange(int coins)
        {
            coinsText.text = Convert.ToString(coins);
        }

        #endregion
        
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