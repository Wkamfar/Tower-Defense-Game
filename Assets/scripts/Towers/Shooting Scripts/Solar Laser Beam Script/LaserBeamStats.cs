using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeamStats : MonoBehaviour
{
    //Laser stats
    [SerializeField] bool isContinuous;
    [SerializeField] float beamDuration;
    [SerializeField] float laserDamageRate;
    [SerializeField] float chargeTime;
    [SerializeField] GameObject beamEndMaterial; //Add this later
    [SerializeField] float beamEndDamageMultiplier;
    [SerializeField] float beamWidth;
    [SerializeField] int radCount;
    [SerializeField] float laserFadeDuration;
    //Tower Stats
    [SerializeField] float damage;
    [SerializeField] float fireRate;
    [SerializeField] float maxTravelDistance;
    [SerializeField] int pierce;
    [SerializeField] bool seesCamo;
    public void SetStats(GameObject tower)
    {
        //Laser Stats
        tower.GetComponent<LaserShoot>().isContinuous = isContinuous;
        tower.GetComponent<LaserShoot>().beamDuration = beamDuration;
        tower.GetComponent<LaserShoot>().laserDamageRate = laserDamageRate;
        tower.GetComponent<LaserShoot>().chargeTime = chargeTime;
        tower.GetComponent<LaserShoot>().beamEndMaterial = beamEndMaterial;
        tower.GetComponent<LaserShoot>().beamEndDamageMultiplier = beamEndDamageMultiplier;
        tower.GetComponent<LaserShoot>().beamWidth = beamWidth;
        tower.GetComponent<LaserShoot>().radCount = radCount;
        tower.GetComponent<LaserShoot>().laserFadeDuration = laserFadeDuration;
        //Tower Stats
        tower.GetComponent<TowerStats>().damage = damage;
        tower.GetComponent<TowerStats>().fireRate = fireRate;
        tower.GetComponent<TowerStats>().maxTravelDistance = maxTravelDistance;
        tower.GetComponent<TowerStats>().pierce = pierce;
        tower.GetComponent<TowerStats>().seesCamo = seesCamo;

    }
}
