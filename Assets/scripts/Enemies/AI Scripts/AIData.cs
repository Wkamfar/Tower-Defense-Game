using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AIData 
{
    public static int currentMoney = 100;
    //public static int totalNumberOfAI = 0;
    public static List<GameObject> enemies = new List<GameObject>();
    public static int currentWave;
    public static void ChangeMoney(int amount)
    {
        currentMoney += amount;
    }
}
