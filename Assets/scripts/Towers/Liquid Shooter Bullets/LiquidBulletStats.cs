using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiquidBulletStats : MonoBehaviour
{
    public float damage;
    public float fireRate;
    public float bulletSpeed;
    public float bulletLifespan;
    public float maxTravelDistance;
    public int pierce;
    public bool seesCamo;
    public bool hasEffect;
    public bool isPermanent;
    public float effectDuration;
    public float effectIntensity;
    public GameObject bulletEffect;
    public void SetStats(GameObject tower)
    {
        tower.GetComponent<TowerStats>().damage = damage;
        tower.GetComponent<TowerStats>().fireRate = fireRate;
        tower.GetComponent<TowerStats>().bulletSpeed = bulletSpeed;
        tower.GetComponent<TowerStats>().bulletLifespan = bulletLifespan;
        tower.GetComponent<TowerStats>().maxTravelDistance = maxTravelDistance;
        tower.GetComponent<TowerStats>().pierce = pierce;
        tower.GetComponent<TowerStats>().seesCamo = seesCamo;
        tower.GetComponent<TowerStats>().hasEffect = hasEffect;
        tower.GetComponent<TowerStats>().isPermanent = isPermanent;
        tower.GetComponent<TowerStats>().effectDuration = effectDuration;
        tower.GetComponent<TowerStats>().effectIntensity = effectIntensity;
        tower.GetComponent<TowerStats>().bulletEffect = bulletEffect;
    }
}
