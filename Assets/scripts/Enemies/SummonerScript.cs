using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonerScript : EnemyAI
{
    [SerializeField] private GameObject spawnedEnemy;
    [SerializeField] private int spawningAmount;
    [SerializeField] private float spawningRate;
    private float spawningTimer;
    [SerializeField] private int spawningMax;
    [SerializeField] private List<GameObject> summonedEnemies;
    protected override void UseAbility()
    {
        if (spawningTimer <= 0 && summonedEnemies.Count < spawningMax)
        {
            for (int i = 0; i < spawningAmount; ++i)
            {
                GameObject currentEnemy = Instantiate(spawnedEnemy, this.gameObject.transform.position, Quaternion.identity);
                currentEnemy.GetComponent<EnemyAI>().currentWaypoint = currentWaypoint;
            summonedEnemies.Add(currentEnemy);
            }
            spawningTimer = spawningRate;
        }
        spawningTimer -= Time.deltaTime;
    }
}
