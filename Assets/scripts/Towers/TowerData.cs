using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Towers 
{
    turret = 0,
    liquid_shooter = 1,
    barracks = 2,
    solar_laser = 3,
    black_hole_gun = 4,
    support_beacon = 5,
    farm = 6
}
public static class TowerData
{
    public static GameObject selectedTower;
    public static bool hasSelectedTower = false;
}
