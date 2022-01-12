using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerData
{
    public static float playerMaxHp = 10000;
    public static float playerCurrentHp = 10000;
    public static int playerMoney = 100000;
    public static void ChangeHealth(int amount)
    {
        playerCurrentHp += amount;
    }
    public static void ChangeMoney(int amount)
    {
        playerMoney += amount;
    }
}
