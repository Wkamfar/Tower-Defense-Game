using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
/// <summary>
/// TowerStats stores the stats of the tower
/// We don't need to change this
/// </summary>
public class TowerStats : MonoBehaviour
{
    //Add overheating, reloading, and everything else later
    //Add stat caps later
    public bool infiniteRange;
    public float radius;
    public float hitbox;
    public float damage;
    public float fireRate;
    public float bulletSpeed;
    public float bulletLifespan;
    public float maxTravelDistance;
    public int pierce;
    public bool seesCamo;
    public int cost;
    public List<GameObject> allowedBlocks;
    public int value;
    public int sellPercentage;
    public float damageDealt = 0;
    public List<int> targetingOptions = new List<int>() { 0, 1 }; // 0 = first enemy, 1 = last enemy, 2 = strongest enemy, 3 = weakest enemy, 4 = Target Marker, 5 = Follow Mouse
    public List<string> targetingOptionNames = new List<string>() { "First Enemy", "Last Enemy", "Strongest Enemy", "Weakest Enemy", "Choose Target", "Follow Mouse" };
    public int targetingIndex = 0;
    public GameObject bullet;
    public GameObject shootPoint;
    public Vector3 targetedLocation;
    public List<GameObject> targets;
    public bool hasEffect;
    public GameObject bulletEffect;
    public float effectDuration;
    public bool isPermanent;
    public float effectIntensity;
    public List<GameObject> specialItems;
    public bool hasCapsuleCollider;
    public bool hasBoxCollider;

    
    public string towerName;

    private void Start()
    {
        value = cost;
    }
    public void AddValue(int _value)
    {
        value += _value;
    }
    public int SellValue()
    {
        return value * sellPercentage / 100;
    }
    public void IncreaseDamageDealt(float amount)
    {
        damageDealt += amount;
        this.gameObject.GetComponent<TowerMenuScript>().damageDealtDisplay.GetComponent<TextMeshProUGUI>().text = Mathf.Round(damageDealt).ToString();
    }
    public virtual string GetTowerName()
    {
        return towerName;
    }
}
