using UnityEngine;

/// <summary>
/// TowerActionScript tracks and shoots an enemy
/// </summary>
public class TowerActionScript : MonoBehaviour
{
    private float shotTimer;
    // Update is called once per frame
    protected virtual void Update()
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
    bool HasTarget()
    {
        if (GetComponent<TowerStats>().targets.Count > 0 || GetComponent<TowerTargeting>().hasMarker)
        {
            return true;
        }
        return false;
    }
    protected virtual void TrackEnemy() //add aiming ahead and predicting movement later for projectile bullets
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
    protected virtual void Shoot()
    {
        shotTimer = 60 / GetComponent<TowerStats>().fireRate;
        GameObject currentBullet = Instantiate(GetComponent<TowerStats>().bullet, GetComponent<TowerStats>().shootPoint.transform.position, Quaternion.identity);
        currentBullet.GetComponent<BulletStats>().damage = this.gameObject.GetComponent<TowerStats>().damage;
        currentBullet.GetComponent<BulletStats>().tower = this.gameObject;
        currentBullet.GetComponent<BulletStats>().despawnTimer = this.gameObject.GetComponent<TowerStats>().bulletLifespan;
        currentBullet.GetComponent<BulletStats>().maxDistance = this.gameObject.GetComponent<TowerStats>().maxTravelDistance;
        currentBullet.GetComponent<BulletStats>().pierce = this.gameObject.GetComponent<TowerStats>().pierce;
        Vector3 direction = (GetComponent<TowerStats>().targetedLocation - this.gameObject.transform.position).normalized;
        currentBullet.GetComponent<Rigidbody>().AddForce(direction * this.gameObject.GetComponent<TowerStats>().bulletSpeed, ForceMode.Impulse);
    }
    protected virtual bool CanShoot()
    {
        if (shotTimer > 0)
        {
            shotTimer -= Time.deltaTime;
            return false;
        }
        return true;
    }
}
