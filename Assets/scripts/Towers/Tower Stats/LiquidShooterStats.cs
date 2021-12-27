using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiquidShooterStats : TowerStats
{
    //Make it so that the bullets store the changes and when they are applied, all of the stats are added
    public GameObject connectedLiquid;
    public override string GetTowerName()
    {
        return connectedLiquid.GetComponent<BlockStats>().blockName + " Shooter";
    }
}