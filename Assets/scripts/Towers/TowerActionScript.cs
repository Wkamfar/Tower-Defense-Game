using UnityEngine;

/// <summary>
/// TowerActionScript tracks and shoots an enemy
/// </summary>
public class TowerActionScript : MonoBehaviour
{

    private float shotTimer;
    
    // Update is called once per frame
    void Update()
    {
        GetComponent<TowerStats>().targetedEnemy = this.gameObject.GetComponent<TowerTargeting>().targeting(this.GetComponent<TowerStats>().targetingOptions[this.GetComponent<TowerStats>().targetingIndex]);
        if (HasEnemy())
        {
            TrackEnemy();
            if (CanShoot())
            {
                ProjectileShoot();
                //HitScanShoot();
            }
        }
    }
    bool HasEnemy()
    {
        if (this.gameObject.GetComponent<TowerTargeting>().GetComponent<TowerStats>().targets.Count > 0)
        {
            return true;
        }
        return false;
    }
    void TrackEnemy() //add aiming ahead and predicting movement later for projectile bullets
    {
        float xDifference = Mathf.Abs(GetComponent<TowerStats>().targetedEnemy.transform.position.x - this.gameObject.transform.position.x);
        float yDifference = Mathf.Abs(GetComponent<TowerStats>().targetedEnemy.transform.position.z - this.gameObject.transform.position.z);
        float angle = Mathf.Atan(yDifference / xDifference);
        angle = angle / Mathf.PI * 180 + 90;
        if (GetComponent<TowerStats>().targetedEnemy.transform.position.x < this.transform.position.x && GetComponent<TowerStats>().targetedEnemy.transform.position.z > this.transform.position.z)
        {
            angle = Mathf.Abs(angle - 180);
        }
        else if (GetComponent<TowerStats>().targetedEnemy.transform.position.x > this.transform.position.x && GetComponent<TowerStats>().targetedEnemy.transform.position.z < this.transform.position.z)
        {
            angle = Mathf.Abs(360 - angle);
        }
        else if (GetComponent<TowerStats>().targetedEnemy.transform.position.x > this.transform.position.x && GetComponent<TowerStats>().targetedEnemy.transform.position.z > this.transform.position.z)
        {
            angle += 180;
        }
        angle = Mathf.Abs(360 - angle);
        GetComponent<TowerUpgradeScript>().currentActiveTowerModel.transform.rotation = Quaternion.Euler(this.gameObject.transform.rotation.x, angle, this.gameObject.transform.rotation.z);

    }
    void HitScanShoot()
    {
        shotTimer = 60 / this.gameObject.GetComponent<TowerStats>().fireRate;
        GetComponent<TowerStats>().targetedEnemy.GetComponent<EnemyAI>().TakeDamage(this.gameObject.GetComponent<TowerStats>().damage, gameObject);
    }
    void ProjectileShoot()
    {
        shotTimer = 60 / this.gameObject.GetComponent<TowerStats>().fireRate;
        GameObject currentBullet = Instantiate(GetComponent<TowerStats>().bullet, GetComponent<TowerStats>().shootPoint.transform.position, Quaternion.identity);
        currentBullet.GetComponent<BulletStats>().damage = this.gameObject.GetComponent<TowerStats>().damage;
        currentBullet.GetComponent<BulletStats>().tower = this.gameObject;
        currentBullet.GetComponent<BulletStats>().despawnTimer = this.gameObject.GetComponent<TowerStats>().bulletLifespan;
        currentBullet.GetComponent<BulletStats>().maxDistance = this.gameObject.GetComponent<TowerStats>().maxTravelDistance;
        currentBullet.GetComponent<BulletStats>().pierce = this.gameObject.GetComponent<TowerStats>().pierce;
        Vector3 direction = (GetComponent<TowerStats>().targetedEnemy.transform.position - this.gameObject.transform.position).normalized;
        currentBullet.GetComponent<Rigidbody>().AddForce(direction * this.gameObject.GetComponent<TowerStats>().bulletSpeed, ForceMode.Impulse);
    }
    bool CanShoot()
    {
        if (shotTimer > 0)
        {
            shotTimer -= Time.deltaTime;
            return false;
        }
        return true;
    }
}
