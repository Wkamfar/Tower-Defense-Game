using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemy;
    private Queue<GameObject> enemiesToSpawn; // Queue<(GameObject, float)>
    private float timer = 0;
    // Start is called before the first frame update
    void Start()
    {
        enemiesToSpawn = new Queue<GameObject>();
        for (int i = 0; i < 50; i++)
        {
            enemiesToSpawn.Enqueue(enemy);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (MapData.isMapSpawned && enemiesToSpawn.Count > 0 && timer <= 0)
        {
            GameObject unit = enemiesToSpawn.Dequeue();
            Spawn(unit);
            timer = unit.GetComponent<EnemyAI>().GetSpawnRate();
        }
        else if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
    }
    
    public void Spawn(GameObject _enemy)
    {
        GameObject currentEnemy = Instantiate(_enemy, MapData.PointToRealWorld(MapData.startingPoint), Quaternion.identity);
    }
}
