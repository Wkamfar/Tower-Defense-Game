using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DataScript 
{
    //Use
    //AIData.currentWave
    //AIData.currentMoney
    //AIData.enemies
    //PlayerData.playerMoney
    //PlayerData.playerCurrentHp
    //TowerData.towers 
    //Damage
    public static List<float> damageTaken = new List<float>();
    //Wave Length
    public static float currentWaveStart;
    public static float currentWaveEnd;
    public static float currentSpawningWaveEnd;
    public static List<float> spawningWaveLength = new List<float>();
    public static List<float> waveLength = new List<float>();
    //DPS // potential dps is a combination of pierce, damage / fire rate or dps
    public static float totalPotentialDPSWithPierce;
    public static float totalPotentialDPSWithoutPierce;
    //Ratings //base ratings off of track coverage, potential dps, and amount of each tower and then compare that to the total health output of each of the enemies that the AI can put out // rating is from 0 - 10
    public static float heavyProtection;
    public static float hordeProtection;
    public static float camoProtection;
    public static float defenseStrength;
}
