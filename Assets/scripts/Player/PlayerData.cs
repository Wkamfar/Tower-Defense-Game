using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerData
{
    public static float playerMaxHp = 150;
    public static float playerCurrentHp = 150;
    public static int playerMoney = 100;
    public static void ChangeHealth(int amount)
    {
        playerCurrentHp += amount;
    }
    public static void ChangeMoney(int amount)
    {
        playerMoney += amount;
    }
}
