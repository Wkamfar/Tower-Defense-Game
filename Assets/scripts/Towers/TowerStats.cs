using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TowerStats : MonoBehaviour
{
    //Add overheating, reloading, and everything else later
    public float radius;
    public float hitbox;
    public float damage;
    public float fireRate;
    public float bulletSpeed;
    public float bulletLifespan;
    public float maxTravelDistance;
    public float pierce;
    public bool seesCamo;
    public int cost;
    public List<GameObject> allowedBlocks;
    public int value;
    public int sellPercentage;
    public float damageDealt = 0;
    public List<int> targetingOptions = new List<int>() { 0, 1 }; // 0 = first enemy, 1 = last enemy, 2 = strongest enemy, 3 = weakest enemy
    public int targetingIndex = 0;
    public List<int> upgradePathLevel = new List<int>() { 0, 0 };
    public List<int> upgradePathMaxLevel = new List<int>() { 2, 2 };
    public List<List<int>> upgradeCost = new List<List<int>>() {
                                                                new List<int>() { 100, 200 }, //upgrade path one cost
                                                                new List<int>() { 100, 200 }  //upgrade path two cost
                                                               };
    public List<List<string>> upgradeNames = new List<List<string>>() { 
                                                                       new List<string>() { "Higher Fire Rate", "Bullet Rain" }, //upgrade path one names
                                                                       new List<string>() { "Depleted Plutonium", "Armor Piercing" }  //upgrade path two names
                                                                      };
    public List<List<GameObject>> upgradeIndicators = new List<List<GameObject>>() { 
                                                                                    new List<GameObject>() { null, null },
                                                                                    new List<GameObject>() { null, null }
                                                                                   };

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
        this.gameObject.GetComponent<TowerMenuScript>().damageDealtDisplay.GetComponent<TextMeshProUGUI>().text = damageDealt.ToString();
    }
}
