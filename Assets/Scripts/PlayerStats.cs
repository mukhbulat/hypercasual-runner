using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    #region FieldsAndProperties

    // Health
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
            if (_isInvulnerable) return;
            
            HealthChange?.Invoke(value);
            
            if (value < _health && !_isInvulnerable)
            {
                StartCoroutine(InvulnerabilityStart());
                
                StopCoroutine(HealthRegeneration());
                StartCoroutine(HealthRegeneration());
            }

            _health = value;
            if (_health <= 0)
            {
                Die();
            }
        }
    }

    // Score
    public Vector3 PlayerPosition => transform.position;
    public event Action<int> MultiplierChange;
    public event Action<int> ScoreChange;
    
    private int _oldPlayerYPosition;
    private int _scoreMultiplier;
    private int _score;

    private int[] _floorHeights = new[] {6, 12};

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

    private int Score
    {
        get => _score;
        set
        {
            ScoreChange?.Invoke(value);
            _score = value;
        }
    }
    
    // Coins

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

    #endregion

    private void Awake()
    {
        _health = maxHealth;
        Score = 0;
        _oldPlayerYPosition = (int) PlayerPosition.z;
    }

    private void Update()
    {
        ScoreHandler();
    }

    #region Health
    
    private void Die()
    {
        Debug.Log("Dead!");
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

    #region Score

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

        if (PlayerPosition.z > _oldPlayerYPosition)
        {
            _oldPlayerYPosition += 1;
            Score += ScoreMultiplier;
        }
    }
    
    #endregion

    #region Coins
    
    private void OnX2Get()
    {
        DoubleCoins?.Invoke(true);
    }

    #endregion
    
    
}