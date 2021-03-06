using System;
using System.Collections;
using Game;
using UnityEngine;

public class PlayerStats : MonoBehaviour, IRestartable
{
    #region General

    private void Awake()
    {
        _health = maxHealth;
        TravelDistance = 0;
        _oldPlayerZPosition = (int) PlayerPosition.z;
        _playerStartingZPosition = _oldPlayerZPosition;
    }

    private void Update()
    {
        ScoreHandler();
    }
    
    public void Restart()
    {
        _isInvulnerable = false;
        Health = 3;
        Coins = 0;
        TravelDistance = 0;
        _oldPlayerZPosition = _playerStartingZPosition;
        StopAllCoroutines();
    }

    #endregion
    
    #region Score

    [SerializeField] private Transform player;
    public Vector3 PlayerPosition => player.position;
    public event Action<int> MultiplierChange;
    public event Action<int> DistanceChange;
    
    private int _oldPlayerZPosition;
    private int _playerStartingZPosition;
    private int _scoreMultiplier;
    private int _travelDistance;

    private readonly int[] _floorHeights = {6, 12};

    private int ScoreMultiplier
    {
        get => _scoreMultiplier;
        set
        {
            if (_scoreMultiplier == value) return;
            MultiplierChange?.Invoke(value);
            _scoreMultiplier = value;
        }
    }

    private int TravelDistance
    {
        get => _travelDistance;
        set
        {
            DistanceChange?.Invoke(value);
            _travelDistance = value;
        }
    }
    
    
    private void ScoreHandler()
    {
        if (PlayerPosition.y < _floorHeights[0])
        {
            ScoreMultiplier = 1;
        }
        else if (PlayerPosition.y < _floorHeights[1])
        {
            ScoreMultiplier = 2;
        }
        else
        {
            ScoreMultiplier = 3;
        }

        if (PlayerPosition.z > _oldPlayerZPosition)
        {
            _oldPlayerZPosition += 1;
            TravelDistance += 1;
        }
    }
    

    #endregion

    #region Health

    [SerializeField] private float regenerationTime = 5.0f;
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private float invulnerabilityTime = 0.1f;
    public event Action<int> HealthChange;
    
    private int _health;
    private bool _isInvulnerable = false;
    public int Health
    {
        get => _health;
        set
        {
            if (_isInvulnerable && value < _health) return;
            
            HealthChange?.Invoke(value);
            
            if (value < _health && !_isInvulnerable)
            {
                StartCoroutine(InvulnerabilityStart());
                
                StopCoroutine(HealthRegeneration());
                StartCoroutine(HealthRegeneration());
            }

            _health = value;
        }
    }
    
    private IEnumerator HealthRegeneration()
    {
        yield return new WaitForSeconds(regenerationTime);
        while ( Health < 3)
        {
            Health += 1;
            
            yield return new WaitForSeconds(regenerationTime);
        }
    }

    private IEnumerator InvulnerabilityStart()
    {
        _isInvulnerable = true;
        yield return new WaitForSeconds(invulnerabilityTime);
        _isInvulnerable = false;
    }

    #endregion

    #region Collectables

    private int _coins;
    public event Action<int> CoinsChange;
    public event Action<bool> DoubleCoins;
    public int Coins
    {
        get => _coins;
        set
        {
            _coins = value;
            CoinsChange?.Invoke(value);
        }
    }
    
    
    private void OnX2Get()
    {
        DoubleCoins?.Invoke(true);
    }

    #endregion
}