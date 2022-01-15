using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileShoot : TowerActionScript
{
    private float shotTimer;
    public List<GameObject> bullets = new List<GameObject>();
    // Update is called once per frame
    protected override void Update()
    {
        GetComponent<TowerStats>().targetedLocation = GetComponent<TowerTargeting>().targeting(GetComponent<TowerStats>().targetingOptions[GetComponent<TowerStats>().targetingIndex]);
        if (HasTarget())
        {
            TrackEnemy();
            if (CanShoot() && AIData.enemies.Count > 0)
            {
                Shoot();
            }
        }
    }
    protected override bool HasTarget()
    {
        if (GetComponent<TowerStats>().targets.Count > 0 || GetComponent<TowerTargeting>().hasMarker)
        {
            return true;
        }
        return false;
    }
    protected override void TrackEnemy() //add aiming ahead and predicting movement later for projectile bullets
    {
        float xDifference = Mathf.Abs(GetComponent<TowerStats>().targetedLocation.x - this.gameObject.transform.position.x);
        float yDifference = Mathf.Abs(GetComponent<TowerStats>().targetedLocation.z - this.gameObject.transform.position.z);
        float angle = Mathf.Atan(yDifference / xDifference);
        angle = angle / Mathf.PI * 180 + 90;
        if (GetComponent<TowerStats>().targetedLocation.x < this.transform.position.x && GetComponent<TowerStats>().targetedLocation.z > this.transform.position.z)
        {
            angle = Mathf.Abs(angle - 180);
        }
        else if (GetComponent<TowerStats>().targetedLocation.x > this.transform.position.x && GetComponent<TowerStats>().targetedLocation.z < this.transform.position.z)
        {
            angle = Mathf.Abs(360 - angle);
        }
        else if (GetComponent<TowerStats>().targetedLocation.x > this.transform.position.x && GetComponent<TowerStats>().targetedLocation.z > this.transform.position.z)
        {
            angle += 180;
        }
        angle = Mathf.Abs(360 - angle);
        GetComponent<TowerUpgradeScript>().currentActiveTowerModel.transform.rotation = Quaternion.Euler(this.gameObject.transform.rotation.x, angle, this.gameObject.transform.rotation.z);
    }
    protected override void Shoot()
    {
        shotTimer = 60 / GetComponent<TowerStats>().fireRate;
        GameObject currentBullet = Instantiate(GetComponent<TowerStats>().bullet, GetComponent<TowerStats>().shootPoint.transform.position, Quaternion.identity);
        bullets.Add(currentBullet);
        currentBullet.GetComponent<BulletStats>().damage = this.gameObject.GetComponent<TowerStats>().damage;
        currentBullet.GetComponent<BulletStats>().tower = this.gameObject;
        currentBullet.GetComponent<BulletStats>().despawnTimer = this.gameObject.GetComponent<TowerStats>().bulletLifespan;
        currentBullet.GetComponent<BulletStats>().maxDistance = this.gameObject.GetComponent<TowerStats>().maxTravelDistance;
        currentBullet.GetComponent<BulletStats>().pierce = this.gameObject.GetComponent<TowerStats>().pierce;
        Vector3 direction = (GetComponent<TowerStats>().targetedLocation - gameObject.transform.position).normalized;
        currentBullet.GetComponent<Rigidbody>().AddForce(direction * GetComponent<TowerStats>().bulletSpeed, ForceMode.Impulse);
    }
    protected override bool CanShoot()
    {
        if (shotTimer > 0)
        {
            shotTimer -= Time.deltaTime;
            return false;
        }
        return true;
    }
    public override void OnDestroyTower()
    {
        for (int i = 0; i < bullets.Count; ++i)
        {
            Destroy(bullets[i]);
        }
    }
}
