using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemy;
    private Queue<GameObject> enemiesToSpawn;
    // Start is called before the first frame update
    void Start()
    {
        enemiesToSpawn = new Queue<GameObject>();
        enemiesToSpawn.Enqueue(enemy);
    }

    // Update is called once per frame
    void Update()
    {
        if (MapData.isMapSpawned && enemiesToSpawn.Count > 0)
        {
            Spawn(enemiesToSpawn.Dequeue());
        }
    }
    public void Spawn(GameObject _enemy)
    {
        GameObject currentEnemy = Instantiate(_enemy, MapData.PointToRealWorld(MapData.startingPoint), Quaternion.identity);
        Debug.Log("This function happened");
    }
}
