using UnityEngine;

public class TowerActionScript : MonoBehaviour
{
    public GameObject targetedEnemy;
    private float shotTimer;
    public GameObject bullet;
    public GameObject shootPoint;
    // Update is called once per frame
    void Update()
    {
        targetedEnemy = this.gameObject.GetComponent<TowerTargeting>().targeting(this.GetComponent<TowerStats>().targetingOptions[this.GetComponent<TowerStats>().targetingIndex]);
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
        if (this.gameObject.GetComponent<TowerTargeting>().targets.Count > 0)
        {
            return true;
        }
        return false;
    }
    void TrackEnemy() //add aiming ahead and predicting movement later for projectile bullets
    {
        float xDifference = Mathf.Abs(targetedEnemy.transform.position.x - this.gameObject.transform.position.x);
        float yDifference = Mathf.Abs(targetedEnemy.transform.position.z - this.gameObject.transform.position.z);
        float angle = Mathf.Atan(yDifference / xDifference);
        angle = angle / Mathf.PI * 180 + 90;
        if (targetedEnemy.transform.position.x < this.transform.position.x && targetedEnemy.transform.position.z > this.transform.position.z)
        {
            angle = Mathf.Abs(angle - 180);
        }
        else if (targetedEnemy.transform.position.x > this.transform.position.x && targetedEnemy.transform.position.z < this.transform.position.z)
        {
            angle = Mathf.Abs(360 - angle);
        }
        else if (targetedEnemy.transform.position.x > this.transform.position.x && targetedEnemy.transform.position.z > this.transform.position.z)
        {
            angle += 180;
        }
        angle = Mathf.Abs(360 - angle);
        this.gameObject.transform.rotation = Quaternion.Euler(this.gameObject.transform.rotation.x, angle, this.gameObject.transform.rotation.z);

    }
    void HitScanShoot()
    {
        shotTimer = 60 / this.gameObject.GetComponent<TowerStats>().fireRate;
        targetedEnemy.GetComponent<EnemyAI>().TakeDamage(this.gameObject.GetComponent<TowerStats>().damage);
        this.gameObject.GetComponent<TowerStats>().IncreaseDamageDealt(this.gameObject.GetComponent<TowerStats>().damage);
    }
    void ProjectileShoot()
    {
        shotTimer = 60 / this.gameObject.GetComponent<TowerStats>().fireRate;
        GameObject currentBullet = Instantiate(bullet, shootPoint.transform.position, Quaternion.identity);
        currentBullet.GetComponent<BulletStats>().damage = this.gameObject.GetComponent<TowerStats>().damage;
        currentBullet.GetComponent<BulletStats>().tower = this.gameObject;
        currentBullet.GetComponent<BulletStats>().despawnTimer = this.gameObject.GetComponent<TowerStats>().bulletLifespan;
        currentBullet.GetComponent<BulletStats>().maxDistance = this.gameObject.GetComponent<TowerStats>().maxTravelDistance;
        currentBullet.GetComponent<BulletStats>().pierce = this.gameObject.GetComponent<TowerStats>().pierce;
        Vector3 direction = (targetedEnemy.transform.position - this.gameObject.transform.position).normalized;
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
