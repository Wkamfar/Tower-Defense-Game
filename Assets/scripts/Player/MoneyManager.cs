using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum EnemyCosts
{
    grunt = 100,
    jetpacker = 150,
    shielder = 200,
    summoner = 250,
    transport = 300,
    enhancer = 350
}
public enum TowerCosts 
{
    turret = 100,
    liquid_shooter = 150,
    barracks = 200,
    solar_laser = 250,
    black_hole_gun = 300,
    support_beacon = 350
}
public enum FarmCosts
{
    farm = 400
}
public static class MoneyManager
{
    public static int playerMoneyCount;
    public static int AIMoneyCount;
}
