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
    private List<Vector3> bulletLocations = new List<Vector3>();

    private void Start()
    {
        startingPosition = new Vector2(this.transform.position.x, this.transform.position.z);
        bulletLocations.Add(transform.position);
        Invoke("DestroyBullet", despawnTimer);
    }
    private void Update()
    {
        //Work on this later
        bulletLocations.Add(transform.position);
        RaycastHit hit;
        LayerMask layerMask = LayerMask.GetMask("Enemy");
        float d = Vector3.Distance(bulletLocations[bulletLocations.Count - 2], bulletLocations[bulletLocations.Count - 1]);
        if (Physics.SphereCast(bulletLocations[bulletLocations.Count - 2], transform.localScale.x / 2, (bulletLocations[bulletLocations.Count - 1] - bulletLocations[bulletLocations.Count - 2]), out hit, d + transform.localScale.x / 2, layerMask))
        {
            //Debug.Log("BulletStats.Update: The raycast hit an enemy");
            DamageEnemy(hit.collider.gameObject);
        }
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
    private void DamageEnemy(GameObject enemy)
    {
        if (!enemy.GetComponent<EnemyAI>().isHit)
        {
            enemy.GetComponent<EnemyAI>().TakeDamage(damage, tower);
            if (tower.GetComponent<TowerStats>().hasEffect)
            {
                GameObject currentEffect = Instantiate(tower.GetComponent<TowerStats>().bulletEffect, enemy.transform);
                enemy.GetComponent<EffectManager>().AddEffect(currentEffect);
                currentEffect.GetComponent<LiquidEffect>().tower = tower;
                currentEffect.GetComponent<LiquidEffect>().effectIntensity = tower.GetComponent<TowerStats>().effectIntensity;
                currentEffect.GetComponent<LiquidEffect>().effectDuration = tower.GetComponent<TowerStats>().effectDuration;
                currentEffect.GetComponent<LiquidEffect>().isPermanent = tower.GetComponent<TowerStats>().isPermanent;
            }
            pierce--;
            if (pierce <= 0)
            {
                DestroyBullet();
            }
        }
    }
}
