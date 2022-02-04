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
            Debug.Log($"Health is {_health}");
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

    private int ScoreMultiplier
    {
        get => _scoreMultiplier;
        set
        {
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
    
    #endregion

    private void Awake()
    {
        _health = maxHealth;
        Score = 0;
        _oldPlayerYPosition = (int) PlayerPosition.y;
    }

    private void Update()
    {
        ScoreHandler();
    }

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

    private void ScoreHandler()
    {
        if (PlayerPosition.y < 20)
        {
            ScoreMultiplier = 1;
        }
        else if (PlayerPosition.y < 40)
        {
            ScoreMultiplier = 2;
        }
        else
        {
            ScoreMultiplier = 3;
        }

        if (PlayerPosition.y > _oldPlayerYPosition)
        {
            _oldPlayerYPosition += 1;
            Score += ScoreMultiplier;
        }
    }
}