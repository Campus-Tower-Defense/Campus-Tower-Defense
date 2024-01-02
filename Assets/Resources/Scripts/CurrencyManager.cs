using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CurrencyManager
{
    private static int playerCurrency = 20;

    public static int GetCurrentCurrency()
    {
        return playerCurrency;
    }

    public static void AddCurrency(int amount)
    {
        playerCurrency += amount;
        // update UI element logic here
    }

    public static bool DeductCurrency(int amount)
    {
        if (playerCurrency >= amount)
        {
            playerCurrency -= amount;
             // update UI element logic here
            return true;
        }
        else
        {
            // Not enough currency
            return false;
        }
    }
}
