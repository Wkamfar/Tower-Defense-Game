using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonerScript : EnemyAI // make a separate enemy for the summonr
{
    [SerializeField] private GameObject spawnedEnemy;
    [SerializeField] private int spawningAmount;
    [SerializeField] private float SPAWNING_TIMER;
    [SerializeField] private float spawnDelay;
    private float spawningTimer;
    [SerializeField] private int spawningMax;
    public List<GameObject> summonedEnemies = new List<GameObject>();
    protected override void UseAbility()
    {
        if (spawningTimer <= 0 && summonedEnemies.Count < spawningMax)
        {
            for (int i = 0; i < spawningAmount; ++i)
            {
                Invoke("Spawn", i * spawnDelay); 
            }
            spawningTimer = SPAWNING_TIMER;
        }
        spawningTimer -= Time.deltaTime;
    }
    private void Spawn()
    {
        if (summonedEnemies.Count >= spawningMax)
        {
            return;
        }
        GameObject currentEnemy = Instantiate(spawnedEnemy, transform.position, Quaternion.identity);
        currentEnemy.GetComponent<EnemyAI>().currentPath = currentPath;
        currentEnemy.GetComponent<EnemyAI>().currentWaypoint = currentWaypoint;
        summonedEnemies.Add(currentEnemy);
    }
}
