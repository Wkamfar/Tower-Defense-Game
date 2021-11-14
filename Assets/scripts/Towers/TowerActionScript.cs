using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerActionScript : MonoBehaviour
{
    public GameObject targetedEnemy;
    private float shotTimer;
    // Update is called once per frame
    void Update()
    {
        targetedEnemy = this.gameObject.GetComponent<TowerTargeting>().FirstEnemy();
        if (HasEnemy())
        {
            TrackEnemy();
            if (CanShoot())
            {
                HitScanShoot();
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
    }
    void ProjectileShoot()
    {

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
