using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerData
{
    public static float playerMaxHp = 150;
    public static float playerCurrentHp = 150;
    public static int playerMoney;
    public static void TakeDamage(int damage)
    {
        playerCurrentHp -= damage;
    }
}
