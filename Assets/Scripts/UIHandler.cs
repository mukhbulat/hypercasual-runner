using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class UIHandler : MonoBehaviour
    {
        // General
        [SerializeField] private PlayerStats playerStats;
        
        // Health
        [SerializeField] private List<Sprite> heartSprites;
        [SerializeField] private Image healthUI;
        
        // Score
        [SerializeField] private List<Color> scoreColors;
        private int ScoreMultiplier => playerStats.ScoreMultiplier;
        private int Score => playerStats.Score;
        private void Awake()
        {
            playerStats.HealthChange += OnHealthChange;
        }

        private void OnHealthChange(int newHealth)
        {
            // TODO: UI things.
            if (newHealth <= 0)
            {
                DieUI();
            }
            else
            {
                healthUI.sprite = heartSprites[newHealth - 1];
            }
            
        }

        private void ScoreView()
        {
            
        }

        private void DieUI()
        {
            
        }
    }
}