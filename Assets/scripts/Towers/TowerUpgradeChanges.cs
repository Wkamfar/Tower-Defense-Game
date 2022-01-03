using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerUpgradeChanges : MonoBehaviour
{
    //Check for upgrade combos

    //Number Stats
    [SerializeField] private float radiusChange;
    [SerializeField] private float hitboxChange;
    [SerializeField] private float damageChange;
    [SerializeField] private float fireRateChange;
    [SerializeField] private float bulletSpeedChange;
    [SerializeField] private float bulletLifespanChange;
    [SerializeField] private float maxTravelDistanceChange;
    [SerializeField] private float pierceChange;
    [SerializeField] private int sellPercentageChange;
    //Other stats
    [SerializeField] private bool seesCamo;
    [SerializeField] private bool infiniteRange;
    [SerializeField] private List<int> targetingOptions = new List<int>() { 0, 1 }; // 0 = first enemy, 1 = last enemy, 2 = strongest enemy, 3 = weakest enemy
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject shootPoint;

    public void upgradeTower(GameObject tower)
    {
        TowerStats towerStats = tower.GetComponent<TowerStats>();
        //Number StatChanges
        towerStats.radius = towerStats.radius + radiusChange >= 0 ? towerStats.radius + radiusChange : 0;
        towerStats.hitbox = towerStats.hitbox + hitboxChange >= 0 ? towerStats.hitbox + hitboxChange : 0;
        towerStats.damage = towerStats.damage + damageChange >= 0 ? towerStats.damage + damageChange : 0;
        towerStats.fireRate = towerStats.fireRate + fireRateChange >= 0 ? towerStats.fireRate + fireRateChange : 0;
        towerStats.bulletSpeed = towerStats.bulletSpeed + bulletSpeedChange >= 0 ? towerStats.bulletSpeed + bulletSpeedChange : 0;
        towerStats.bulletLifespan = towerStats.bulletLifespan + bulletLifespanChange >= 0 ? towerStats.bulletLifespan + bulletLifespanChange : 0;
        towerStats.maxTravelDistance = towerStats.maxTravelDistance + maxTravelDistanceChange >= 0 ? towerStats.maxTravelDistance + maxTravelDistanceChange : 0;
        towerStats.pierce = towerStats.pierce + pierceChange >= 0 ? towerStats.pierce + pierceChange : 0;
        towerStats.sellPercentage = towerStats.sellPercentage + sellPercentageChange >= 0 ? towerStats.sellPercentage + sellPercentageChange : 0;
        //Other Stats
        towerStats.seesCamo = towerStats.seesCamo = false ? seesCamo : towerStats.seesCamo;
        towerStats.infiniteRange = towerStats.infiniteRange = false ? infiniteRange : towerStats.infiniteRange;
        for (int i = 0; i < targetingOptions.Count; ++i)
        {
            bool alreadyUsed = false;
            for (int j = 0; j < towerStats.targetingOptions.Count; ++j)
            {
                if (targetingOptions[i] == towerStats.targetingOptions[j])
                {
                    alreadyUsed = true;
                    break;
                }
            }
            if (!alreadyUsed)
            {
                int targetingOption = targetingOptions[i];
                for (int j = 0; j < towerStats.targetingOptions.Count; ++j)
                {
                    if (targetingOption > towerStats.targetingOptions[j])
                    {
                        List<int> saveTargets = new List<int>();
                        for (int k = j; k < towerStats.targetingOptions.Count; ++k)
                        {
                            saveTargets.Add(towerStats.targetingOptions[k]);
                            towerStats.targetingOptions.RemoveAt(k);
                        }
                        towerStats.targetingOptions.Add(targetingOption);
                        for (int k = 0; k < saveTargets.Count; ++k)
                        {
                            towerStats.targetingOptions.Add(saveTargets[k]);
                        }
                    }
                }
            }
        }


    }
}
