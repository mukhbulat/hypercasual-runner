using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
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

    private int _scoreMultiplier = 1;
    private int _score = 0;

    
    public int ScoreMultiplier => -_scoreMultiplier;
    public int Score => _score;
    
    private void Awake()
    {
        _health = maxHealth;
        _score = 0;
    }

    private void Die()
    {
        Debug.Log("Dead!");
    }

    private IEnumerator HealthRegeneration()
    {
        while ( Health < 3)
        {
            yield return new WaitForSeconds(regenerationTime);

            Health += 1;
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
            _scoreMultiplier = 1;
        }
        else if (PlayerPosition.y < 40)
        {
            _scoreMultiplier = 2;
        }
        else
        {
            _scoreMultiplier = 3;
        }

        _score = (int) PlayerPosition.y * _scoreMultiplier;
        
        
    }
}