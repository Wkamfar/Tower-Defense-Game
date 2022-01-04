using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolarLaserStats : TowerStats
{
    public GameObject powerSource;
    public override string GetTowerName()
    {
        return powerSource.GetComponent<powerSourceStats>().towerName;
    }
}
