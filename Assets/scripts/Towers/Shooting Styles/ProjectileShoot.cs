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
        if (transform.position != GetComponent<TowerTargeting>().targeting(GetComponent<TowerStats>().targetingOptions[GetComponent<TowerStats>().targetingIndex]))
        {
            GetComponent<TowerStats>().targetedLocation = GetComponent<TowerTargeting>().targeting(GetComponent<TowerStats>().targetingOptions[GetComponent<TowerStats>().targetingIndex]);
            Ray minDistanceRay = new Ray(new Vector3(transform.position.x, GetComponent<TowerStats>().shootPoint.transform.position.y, transform.position.z), (new Vector3(GetComponent<TowerStats>().targetedLocation.x, GetComponent<TowerStats>().shootPoint.transform.position.y, GetComponent<TowerStats>().targetedLocation.z) - new Vector3(transform.position.x, GetComponent<TowerStats>().shootPoint.transform.position.y, transform.position.z)).normalized);
            float targetPosDistance = Vector3.Distance(new Vector3(GetComponent<TowerStats>().targetedLocation.x, GetComponent<TowerStats>().shootPoint.transform.position.y, GetComponent<TowerStats>().targetedLocation.z), new Vector3(transform.position.x, GetComponent<TowerStats>().shootPoint.transform.position.y, transform.position.z));
            float minDistance = Vector3.Distance(GetComponent<TowerStats>().shootPoint.transform.position, new Vector3(transform.position.x, GetComponent<TowerStats>().shootPoint.transform.position.y, transform.position.z)) + 1;
            targetPosDistance = targetPosDistance > minDistance ? targetPosDistance : minDistance;
            GetComponent<TowerStats>().targetedLocation = minDistanceRay.GetPoint(targetPosDistance);
        }
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
        float xDifference = Mathf.Abs(GetComponent<TowerStats>().targetedLocation.x - transform.position.x);
        float yDifference = Mathf.Abs(GetComponent<TowerStats>().targetedLocation.z - transform.position.z);
        float angle = Mathf.Atan(yDifference / xDifference);
        angle = angle / Mathf.PI * 180 + 90;
        if (GetComponent<TowerStats>().targetedLocation.x < transform.position.x && GetComponent<TowerStats>().targetedLocation.z > transform.position.z)
        {
            angle = Mathf.Abs(angle - 180);
        }
        else if (GetComponent<TowerStats>().targetedLocation.x > transform.position.x && GetComponent<TowerStats>().targetedLocation.z < transform.position.z)
        {
            angle = Mathf.Abs(360 - angle);
        }
        else if (GetComponent<TowerStats>().targetedLocation.x > transform.position.x && GetComponent<TowerStats>().targetedLocation.z > transform.position.z)
        {
            angle += 180;
        }
        angle = Mathf.Abs(360 - angle);
        GameObject towerModel = GetComponent<TowerUpgradeScript>().currentActiveTowerModel;
        float targetDistance = Vector3.Distance(new Vector3(GetComponent<TowerStats>().targetedLocation.x, GetComponent<TowerStats>().shootPoint.transform.position.y, GetComponent<TowerStats>().targetedLocation.z), new Vector3(transform.position.x, GetComponent<TowerStats>().shootPoint.transform.position.y, transform.position.z));
        Ray currentTargetRay = new Ray(new Vector3(transform.position.x, GetComponent<TowerStats>().shootPoint.transform.position.y, transform.position.z), (GetComponent<TowerStats>().shootPoint.transform.position - new Vector3(transform.position.x, GetComponent<TowerStats>().shootPoint.transform.position.y, transform.position.z)).normalized);
        if (Mathf.Round(towerModel.transform.rotation.eulerAngles.y * 100) / 100 != Mathf.Round(angle * 100) / 100)
        {
            float angleDistance = angle - towerModel.transform.rotation.eulerAngles.y;
            float rotationAmount = GetComponent<TowerStats>().towerTurnSpeed * Time.deltaTime;
            if (angleDistance <= rotationAmount)
            {
                towerModel.transform.rotation = Quaternion.Euler(transform.rotation.x, angle, transform.rotation.z);
            }
            else if (angleDistance <= 180)
            {
                if (angle > towerModel.transform.rotation.eulerAngles.y)
                {
                    towerModel.transform.rotation = Quaternion.Euler(transform.rotation.x, towerModel.transform.rotation.eulerAngles.y + rotationAmount, transform.rotation.z);
                }
                else
                {
                    towerModel.transform.rotation = Quaternion.Euler(transform.rotation.x, towerModel.transform.rotation.eulerAngles.y - rotationAmount, transform.rotation.z);
                }
            }
            else
            {
                if (angle > towerModel.transform.rotation.eulerAngles.y)
                {
                    towerModel.transform.rotation = Quaternion.Euler(transform.rotation.x, towerModel.transform.rotation.eulerAngles.y - rotationAmount, transform.rotation.z);
                }
                else
                {
                    towerModel.transform.rotation = Quaternion.Euler(transform.rotation.x, towerModel.transform.rotation.eulerAngles.y + rotationAmount, transform.rotation.z);
                }
            }

        }
        GetComponent<TowerStats>().currentAimLocation = currentTargetRay.GetPoint(targetDistance);
    }
    protected override void Shoot()
    {
        shotTimer = 60 / GetComponent<TowerStats>().fireRate;
        GameObject currentBullet = Instantiate(GetComponent<TowerStats>().bullet, GetComponent<TowerStats>().shootPoint.transform.position, Quaternion.identity);
        bullets.Add(currentBullet);
        currentBullet.GetComponent<BulletStats>().damage = GetComponent<TowerStats>().damage;
        currentBullet.GetComponent<BulletStats>().tower = gameObject;
        currentBullet.GetComponent<BulletStats>().despawnTimer = GetComponent<TowerStats>().bulletLifespan;
        currentBullet.GetComponent<BulletStats>().maxDistance = GetComponent<TowerStats>().maxTravelDistance;
        currentBullet.GetComponent<BulletStats>().pierce = GetComponent<TowerStats>().pierce;
        Vector3 direction = (GetComponent<TowerStats>().currentAimLocation - transform.position).normalized;
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
