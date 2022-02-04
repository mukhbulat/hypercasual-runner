using System;
using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float regenerationTime = 5.0f;
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private float invulnerabilityTime = 0.1f;

    private int _health;
    private bool _isInvulnerable = false;
    public int Health
    {
        get => _health;
        set
        {
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

    private void Awake()
    {
        _health = maxHealth;
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
}