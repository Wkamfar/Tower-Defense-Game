using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private float maxHp;
    private float currentHp;
    private float movementSpeed;
    private int currentPath;
    private GameObject AI;

    private void GetRandomPath()
    {
        currentPath = Random.Range(0, PathData.possiblePaths.Count);
    }
    EnemyAI()
    {
        GetRandomPath();
        AI = this.gameObject;
    }
    
    private void Move()
    {
        //AI.transform.position  
    }

}
