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
    private void Update()
    {
        if (tower.GetComponent<TowerStats>().infiniteRange)
        {
            List<GameObject> _targets = new List<GameObject>();
            for (int i = 0; i < AIData.enemies.Count; ++i)
            {
                if (CanAttackEnemy(AIData.enemies[i]))
                {
                    _targets.Add(AIData.enemies[i]);
                }
            }
            tower.GetComponent<TowerStats>().targets = _targets;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy" && !tower.GetComponent<TowerStats>().infiniteRange)
        {
            if (CanAttackEnemy(other.gameObject))
            {
                tower.GetComponent<TowerStats>().targets.Add(other.gameObject);
            }  
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Enemy" && !tower.GetComponent<TowerStats>().infiniteRange)
        {
            tower.GetComponent<TowerStats>().targets.Remove(other.gameObject);
        }
    }
    bool CanAttackEnemy(GameObject enemy)
    {
        if (enemy.GetComponent<EnemyAI>().isCamo && !tower.GetComponent<TowerStats>().seesCamo)
            return false;
        return true;
    }
}
