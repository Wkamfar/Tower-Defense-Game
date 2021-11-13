using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    // Make an explosion effect on the death of enemies
    private float maxHp;
    public float spawnRate = 1f;
    private float currentHp;
    private float movementSpeed = 10f;
    private int currentPath;
    private int currentWaypoint = 0;
    private float minDistance = 0.1f;
    private GameObject AI;

    private void GetRandomPath()
    {
        currentPath = Random.Range(0, PathData.realPossiblePaths.Count - 1);
    }
    private void Start()
    {
        GetRandomPath();
        AI = this.gameObject;
    }
    private void Update()
    {
        Move();
    }
    public float GetSpawnRate()
    {
        return spawnRate;
    }
    private void Move()
    {
        if (currentWaypoint >= PathData.realPossiblePaths[currentPath].Count)
        {
            //Add explosion particle
            Destroy(this.gameObject);
            return;
        }
        //Debug.Log("EnemyAI.Move: This happened");
        float distance = GetDistance();
        this.transform.position = Vector3.MoveTowards(this.transform.position, PathData.realPossiblePaths[currentPath][currentWaypoint], Time.deltaTime * movementSpeed);
        if (distance <= minDistance)
        {
            currentWaypoint++;
        }
        
    }
    private float GetDistance()
    {
        float x = Mathf.Abs(this.transform.position.x - PathData.realPossiblePaths[currentPath][currentWaypoint].x);
        float y = Mathf.Abs(this.transform.position.z - PathData.realPossiblePaths[currentPath][currentWaypoint].z);
        return Mathf.Sqrt(x * x + y * y);
    }
}
