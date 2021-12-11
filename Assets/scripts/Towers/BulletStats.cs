using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletStats : MonoBehaviour
{
    public float damage;
    public float maxDistance;
    public float despawnTimer;
    public float pierce;
    public GameObject tower;
    private Vector2 startingPosition;
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            if (!col.gameObject.GetComponent<EnemyAI>().isHit)
            {
                col.gameObject.GetComponent<EnemyAI>().TakeDamage(damage);
                tower.GetComponent<TowerStats>().IncreaseDamageDealt(damage);
                pierce--;
                if (pierce <= 0)
                {
                    Destroy(this.gameObject);
                }
            } 
        }
    }
    private void Start()
    {
        startingPosition = new Vector2(this.transform.position.x, this.transform.position.z);
        Invoke("DestroyBullet", despawnTimer);
    }
    private void Update()
    {
        float distance = Mathf.Sqrt(Mathf.Abs(startingPosition.x - this.transform.position.x) + Mathf.Abs(startingPosition.y - this.transform.position.z));
        if (distance > maxDistance)
        {
            DestroyBullet();
        }
    }
    private void DestroyBullet()
    {
        Destroy(this.gameObject);
    }
}
