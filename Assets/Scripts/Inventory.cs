using System;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private int _coins;

    public event Action<bool> DoubleCoins;
    public int Coins
    {
        get => _coins;
        set
        {
            _coins = value;
            Debug.Log($"Number of coins is {_coins}");
        }
    }

    private void OnX2Get()
    {
        DoubleCoins?.Invoke(true);
    }
}