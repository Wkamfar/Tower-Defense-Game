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
}
