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
    List<GameObject> hitEnemies = new List<GameObject>();

    private void Start()
    {
        startingPosition = new Vector2(transform.position.x, transform.position.z);
        bulletLocations.Add(transform.position);
        Invoke("DestroyBullet", despawnTimer);
    }
    private void Update()
    {
        //Work on this later
        bulletLocations.Add(transform.position);
        Ray ray = new Ray(bulletLocations[bulletLocations.Count - 2], bulletLocations[bulletLocations.Count - 1] - bulletLocations[bulletLocations.Count - 2]);
        RaycastHit hit;
        LayerMask layerMask = LayerMask.GetMask("Shield");
        float d = Vector3.Distance(bulletLocations[bulletLocations.Count - 2], bulletLocations[bulletLocations.Count - 1]);
        if (Physics.SphereCast(ray, transform.localScale.x / 2, out hit, d + transform.localScale.x / 2, layerMask))
        {
            DamageShield(hit.collider.gameObject);
        }
        layerMask = LayerMask.GetMask("Enemy");
        if (Physics.SphereCast(ray, transform.localScale.x / 2, out hit, d + transform.localScale.x / 2, layerMask))
        {
            //Debug.Log("BulletStats.Update: The raycast hit an enemy");
            DamageEnemy(hit.collider.gameObject);
        }
        float distance = Mathf.Sqrt(Mathf.Abs(startingPosition.x - transform.position.x) + Mathf.Abs(startingPosition.y - transform.position.z));
        if (distance > maxDistance)
        {
            DestroyBullet();
        }
        if (hitEnemies.Count > 0)
        {
            List<GameObject> hitEnemiesRemoval = new List<GameObject>();
            for (int i = 0; i < hitEnemies.Count; ++i)
            {
                RaycastHit[] hits = Physics.SphereCastAll(ray, transform.localScale.x / 2, d + transform.localScale.x / 2, layerMask);
                if (!WFunctions.AlreadyInList(hits, hitEnemies[i]))
                {
                    hitEnemies.Remove(hitEnemies[i]);
                    --i;
                }
            }
        }
    }
    private void DestroyBullet()
    {
        tower.GetComponent<ProjectileShoot>().bullets.Remove(gameObject);
        Destroy(gameObject);
    }
    private void DamageEnemy(GameObject enemy)
    {
        if (!enemy.GetComponent<EnemyAI>().IsDead && !WFunctions.AlreadyInList(hitEnemies, enemy))
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
            hitEnemies.Add(enemy);
            if (pierce <= 0)
            {
                DestroyBullet();
            }
        }
    }
    private void DamageShield(GameObject shield)
    {
        shield.GetComponentInParent<ShielderScript>().TakeShieldDamage(damage, tower);
        DestroyBullet();
    }
}