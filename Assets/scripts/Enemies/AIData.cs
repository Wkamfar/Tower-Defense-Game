using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AIData 
{
    public static int currentMoney = 100;
    public static void ChangeMoney(int amount)
    {
        currentMoney += amount;
    }
}
