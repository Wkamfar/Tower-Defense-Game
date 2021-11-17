using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Profiling;
public class EnemyAI : MonoBehaviour
{
    // Make an explosion effect on the death of enemies
    [SerializeField] private float maxHp;
    [SerializeField] private int bounty;
    [SerializeField] private int damage;
    [SerializeField] private float movementSpeed = 10f;
    public int cost;
    private float currentHp;
    public int currentPath = -1;
    public int currentWaypoint = 0;
    private float minDistance = 0.1f;
    private GameObject AI;
    public GameObject deathEffect;
    public bool IsDead;
    private void GetRandomPath()
    {
        if (currentPath == -1)
        {
            currentPath = Random.Range(0, PathData.realPossiblePaths.Count - 1); 
        }
    }
    private void Start()
    {
        currentHp = maxHp;
        GetRandomPath();
        AI = this.gameObject;
    }
    private void Update()
    {
        Move();
        if (currentHp <= 0 && !IsDead)
        {
            KillAI();
        }
        UseAbility();
    }
    private void Move()
    {
        if (currentWaypoint >= PathData.realPossiblePaths[currentPath].Count && !IsDead)
        {
            bounty = 0;
            PlayerData.ChangeHealth(-damage);
            KillAI();
        }
        if (IsDead)
        {
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
    private void KillAI()
    {
        //Instantiate(deathEffect, this.gameObject.transform.position, Quaternion.identity, this.gameObject.transform);
        Invoke("DespawnBody", 1f);
        IsDead = true;
        PlayerData.ChangeMoney(bounty);
    }
    private void DespawnBody()
    {
        Destroy(this.gameObject);
    }
    private float GetDistance()
    {
        float x = Mathf.Abs(this.transform.position.x - PathData.realPossiblePaths[currentPath][currentWaypoint].x);
        float y = Mathf.Abs(this.transform.position.z - PathData.realPossiblePaths[currentPath][currentWaypoint].z);
        return Mathf.Sqrt(x * x + y * y);
    }
    public void TakeDamage(float damage)
    {
        currentHp -= damage;
        DamageIndicator();
    }
    private void DamageIndicator()
    {
        this.gameObject.GetComponent<Renderer>().material.color = Color.red;
    }
    protected virtual void UseAbility()
    {

    }
}
