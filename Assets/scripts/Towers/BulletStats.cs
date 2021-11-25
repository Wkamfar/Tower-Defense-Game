using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletStats : MonoBehaviour
{
    public float damage;
    [SerializeField] private int maxDistance;
    [SerializeField] private float despawnTimer;
    private Vector2 startingPosition;
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            col.gameObject.GetComponent<EnemyAI>().TakeDamage(damage);
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
