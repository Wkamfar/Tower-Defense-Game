using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerData
{
    public static float playerMaxHp = 10000;
    public static float playerCurrentHp = 10000;
    public static int playerMoney = 100000;
    public static void ChangeHealth(float amount)
    {
        playerCurrentHp += amount;
        if (amount < 0)
        {
            if (DataScript.damageTaken.Count < AIData.currentWave)
                DataScript.damageTaken.Add(0);
            else
                DataScript.damageTaken[AIData.currentWave - 1] += amount;
        }
    }
    public static void ChangeMoney(int amount)
    {
        playerMoney += amount;
    }
}
