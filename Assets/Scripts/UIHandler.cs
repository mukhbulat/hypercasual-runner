using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    // General
    [SerializeField] private PlayerStats playerStats;
        
    // Health
    [SerializeField] private List<Sprite> heartSprites;
    [SerializeField] private Image healthUI;
        
    // Score
    [SerializeField] private List<Color> scoreColors;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI coinsText;
    private void Awake()
    {
        // Health
        playerStats.HealthChange += OnHealthChange;
        // Score
        playerStats.ScoreChange += OnScoreChange;
        playerStats.MultiplierChange += OnMultiplierChange;
        // Coins
        playerStats.CoinsChange += OnCoinsChange;
    }

    private void OnHealthChange(int newHealth)
    {
        // TODO: UI things.
        if (newHealth <= 0)
        {
            LoseUI();
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

    private void LoseUI()
    {
        
    }

    private void OnCoinsChange(int coins)
    {
        coinsText.text = Convert.ToString(coins);
    }
}