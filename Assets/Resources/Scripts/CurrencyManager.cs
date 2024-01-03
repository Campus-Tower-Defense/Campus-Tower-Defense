using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CurrencyManager : MonoBehaviour
{
    private static int playerCurrency;
    public Text text;

    void Start()
    {
        playerCurrency = 20;
        text.text = playerCurrency.ToString();
    }

    void Update()
    {
        text.text = playerCurrency.ToString();
    }

    public static int GetCurrentCurrency()
    {
        return playerCurrency;
    }

    public static void AddCurrency(int amount)
    {
        playerCurrency += amount;
    }

    public static bool DeductCurrency(int amount)
    {
        if (playerCurrency >= amount)
        {
            playerCurrency -= amount;
            return true;
        }
        else
        {
            // Not enough currency
            return false;
        }
    }
}
