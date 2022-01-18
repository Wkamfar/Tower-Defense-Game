using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CreateDataRatings 
{
    public static void SetTotalPotentialDPS(bool includePierce)
    {
        float totalPotentialDPS = 0;
        if (includePierce)
        {
            for (int i = 0; i < TowerData.towers.Count; ++i)
            {
                totalPotentialDPS += GetPotentialDPS(TowerData.towers[i], includePierce);
            }
            DataScript.totalPotentialDPSWithPierce = totalPotentialDPS;
            //Debug.Log("CreateDataRatings.SetTotalPotentialDPS: totalPotentialDPSWithPierce is: " + DataScript.totalPotentialDPSWithPierce);
        }
        else
        {
            for (int i = 0; i < TowerData.towers.Count; ++i)
            {
                totalPotentialDPS += GetPotentialDPS(TowerData.towers[i], includePierce);
            }
            DataScript.totalPotentialDPSWithoutPierce = totalPotentialDPS;
            //Debug.Log("CreateDataRatings.SetTotalPotentialDPS: totalPotentialDPSWithoutPierce is: " + DataScript.totalPotentialDPSWithoutPierce);
        }
    }
    public static float GetPotentialDPS(GameObject tower, bool includePierce) // ignore liquid effects and radCount for now because I haven't completed them
    {
        float potentialDPS = 0;
        int pierce = tower.GetComponent<TowerStats>().pierce;
        if (!includePierce)
            pierce = 1;
        float damage = tower.GetComponent<TowerStats>().damage;
        float fireRate = tower.GetComponent<TowerStats>().fireRate;
        if (tower.GetComponent<ProjectileShoot>() != null)
        {
            potentialDPS = pierce * damage / (60 / fireRate);
        }
        else if (tower.GetComponent<LaserShoot>() != null)
        {
            float chargeTime = tower.GetComponent<LaserShoot>().chargeTime;
            bool isContinuous = tower.GetComponent<LaserShoot>().isContinuous;
            float beamDuration = tower.GetComponent<LaserShoot>().beamDuration;
            float laserDamageRate = tower.GetComponent<LaserShoot>().laserDamageRate;
            //int radCount = tower.GetComponent<LaserShoot>().radCount;
            if (isContinuous)
            {
                potentialDPS = pierce * (damage * beamDuration / (60 / laserDamageRate) / (beamDuration + chargeTime + 60 / fireRate));
            }
            else
            {
                potentialDPS = pierce * damage / (60 / fireRate + chargeTime);
            }
        }
        return potentialDPS;
    }
}
