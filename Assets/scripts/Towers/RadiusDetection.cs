using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Adds enemies within radius
/// Is used in TowerTargeting
/// </summary>
public class RadiusDetection : MonoBehaviour
{
    public GameObject tower;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            tower.GetComponent<TowerStats>().targets.Add(other.gameObject);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            tower.GetComponent<TowerStats>().targets.Remove(other.gameObject);
        }
    }
}
