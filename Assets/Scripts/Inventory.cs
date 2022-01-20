using UnityEngine;

public class Inventory : MonoBehaviour
{
    private int _coins;

    public int Coins
    {
        get => _coins;
        set
        {
            _coins = value;
            Debug.Log($"Number of coins is {_coins}");
        }
    }
    
    
}