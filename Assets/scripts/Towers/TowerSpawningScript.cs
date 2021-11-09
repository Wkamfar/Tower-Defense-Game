using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public void SpawnTower(Vector3 spawnLocation, GameObject Tower)
    {
        Instantiate(Tower, spawnLocation, Quaternion.identity);
    }
}
