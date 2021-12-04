using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerStats : MonoBehaviour
{
    //Add overheating, reloading, and everything else later
    public float radius;
    public float hitbox;
    public float damage;
    public float fireRate;
    public float bulletSpeed;
    public bool seesCamo;
    public int cost;
    public List<GameObject> allowedBlocks;
    public int value;
    public int sellPercentage;
    public List<int> targetingOptions = new List<int>() { 0, 1 };
    public int targetingIndex = 0;
    public List<int> upgradePathLevel = new List<int>() { 0, 0 };
    public List<int> upgradePathMaxLevel = new List<int>() { 2, 2 };
    public List<List<int>> upgradeCost = new List<List<int>>() {
                                                                new List<int>() { 100, 200 }, //upgrade path one cost
                                                                new List<int>() { 100, 200 }  //upgrade path two cost
                                                               };
    public List<List<string>> upgradeNames = new List<List<string>>() { 
                                                                       new List<string>() { "", "" }, //upgrade path one names
                                                                       new List<string>() { "", "" }  //upgrade path two names
                                                                      };
    private void Start()
    {
        Debug.Log("TowerStats.Start: The cost of the first upgrade on path one is: " + upgradeCost[0][0]);
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
}
