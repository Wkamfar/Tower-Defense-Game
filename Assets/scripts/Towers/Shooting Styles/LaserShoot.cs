using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserShoot : TowerActionScript // Add radiation manager for the radiation that the nuclear death ray applies // Add after effects for the beam
{
    public bool firing;
    public bool isContinuous;
    public float beamDuration;
    public float laserDamageRate;
    public float chargeTime;
    public GameObject beamMaterial; // make the bullet the beam material or something like that, do this later
    public GameObject preBeamMaterial;
    public GameObject beamEndMaterial; //Add this later
    public GameObject beam;
    public GameObject preBeam;
    public GameObject beamEnd;
    public float beamEndDamageMultiplier;
    public float visibleBeamWidth;
    public float beamWidth;
    public float preBeamWidth;
    public int preBeamPierce = 1;
    public int radCount; //Gives a radiation count that will do large amounts of damage to enemies
    public float laserFadeDuration;
    private float shotTimer;
    private float currentChargeTime;
    private float beamTimer;
    private float damageTimer;
    private List<GameObject> fadingBeams = new List<GameObject>();
    // Update is called once per frame
    private void Start()
    {
        currentChargeTime = chargeTime;
        beamTimer = beamDuration;
    }
    protected override void Update()
    {
        GetComponent<TowerStats>().targetedLocation = GetComponent<TowerTargeting>().targeting(GetComponent<TowerStats>().targetingOptions[GetComponent<TowerStats>().targetingIndex]);
        if (HasTarget())
        {
            TrackEnemy();
            if (!firing)
            {
                if (CanShoot() && AIData.enemies.Count > 0)
                {
                    if (IsCharged())
                    {
                        Shoot();
                    }
                }
            }

        }
        if (firing)
        {
            if (isContinuous)
            {
                ContinuousFire();
            }
            else
            {
                OneShotFire();
            }
        }
        if (AIData.enemies.Count == 0)
        {
            ResetTower();
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
        currentChargeTime = chargeTime;
        Destroy(preBeam);
        beam = Instantiate(beamMaterial);
        beamEnd = Instantiate(beamEndMaterial);
        firing = true;
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
    bool IsCharged()
    {
        if (currentChargeTime > 0)
        {
            if (preBeam == null)
            {
                preBeam = Instantiate(preBeamMaterial); // have the prebeam do a little damage
            }
            UseLaser(GetComponent<TowerStats>().shootPoint.transform.position, GetComponent<TowerStats>().targetedLocation, preBeam, preBeamWidth, preBeamWidth, preBeamPierce);
            currentChargeTime -= Time.deltaTime;
            return false;
        }
        return true;
    }
    public void UseLaser(Vector3 pointOne, Vector3 pointTwo, GameObject laser, float visibleWidth, float width, int pierce)
    {
        Vector3 targetPos = FindLaserEndPoint(pointOne, pointTwo, width, pierce);
        float length = Vector3.Distance(pointOne, targetPos);
        Vector3 middlePoint = new Vector3((pointOne.x + targetPos.x) / 2,
                                          (pointOne.y + targetPos.y) / 2,
                                          (pointOne.z + targetPos.z) / 2);
        laser.transform.position = middlePoint;
        float x = pointOne.x - targetPos.x;
        float y = pointOne.z - targetPos.z;
        float d = Mathf.Sqrt(x * x + y * y);
        laser.transform.localScale = new Vector3(length, visibleWidth, visibleWidth);
        float angle = 180 - Mathf.Abs(Mathf.Asin(x / d) * 180 / Mathf.PI - 90);
        if (y < 0)
        {
            angle = 360 - angle;
        }
        laser.transform.rotation = Quaternion.Euler(0, angle, 0);
    }
    private Vector3 FindLaserEndPoint(Vector3 pointOne, Vector3 pointTwo, float width, int pierce) // adjust for mirrors later
    {

        RaycastHit hit;
        float maxDistance = GetComponent<TowerStats>().maxTravelDistance;
        LayerMask layerMask = LayerMask.GetMask("Enemy");
        float m = maxDistance / Vector3.Distance(pointOne, pointTwo);
        float x = (pointTwo.x - pointOne.x) * m;
        float z = (pointTwo.z - pointOne.z) * m;
        Vector3 furthestPoint = new Vector3(pointOne.x + x, transform.position.y, pointOne.z + z);
        if (Physics.SphereCast(pointOne, width, pointTwo - pointOne, out hit, maxDistance, layerMask))
        {
            RaycastHit[] hits = Physics.SphereCastAll(pointOne, width, pointTwo - pointOne, maxDistance, layerMask);
            if (hits.Length > 0)
            {
                if (hits.Length < pierce)
                {
                    return furthestPoint;
                }
                for (int i = 0; i < hits.Length - 1; ++i)
                {
                    for (int j = 0; j < hits.Length - i - 1; ++j)
                    {
                        if (Vector3.Distance(pointOne, hits[j].transform.position) > Vector3.Distance(pointOne, hits[j + 1].transform.position))
                        {
                            RaycastHit temp = hits[j];
                            hits[j] = hits[j + 1];
                            hits[j + 1] = temp;
                        }
                    }
                }
                hit = hits[pierce - 1];
                return hit.point;
            }
        }
        return furthestPoint;
    }
    void ContinuousFire()
    {
        beamTimer -= Time.deltaTime;
        if (beamTimer > 0)
        {
            UseLaser(GetComponent<TowerStats>().shootPoint.transform.position, GetComponent<TowerStats>().targetedLocation, beam, visibleBeamWidth, beamWidth, GetComponent<TowerStats>().pierce);
            DealDamageWithRaycasts(GetComponent<TowerStats>().damage, laserDamageRate, GetComponent<TowerStats>().shootPoint.transform.position, GetComponent<TowerStats>().targetedLocation, beamWidth, GetComponent<TowerStats>().pierce);
        }
        else
        {
            FadeLaser();
            beamTimer = beamDuration;
            firing = false;
        }
    }
    void OneShotFire() // work on this later
    {
        UseLaser(GetComponent<TowerStats>().shootPoint.transform.position, GetComponent<TowerStats>().targetedLocation, beam, visibleBeamWidth, beamWidth, GetComponent<TowerStats>().pierce); // Destroy beam and play an animation
        DealDamageWithRaycasts(GetComponent<TowerStats>().damage, GetComponent<TowerStats>().shootPoint.transform.position, GetComponent<TowerStats>().targetedLocation, beamWidth, GetComponent<TowerStats>().pierce);
        FadeLaser();
        firing = false;
    }
    void FadeLaser()
    {
        GameObject currentFadingBeam = Instantiate(beam, beam.transform.position, beam.transform.rotation);
        fadingBeams.Add(currentFadingBeam);
        Destroy(beam);
        Destroy(beamEnd);
        Invoke("RemoveLaser", laserFadeDuration); // laser animation for when laser gets destroyed
    }
    void RemoveLaser()
    {
        if (fadingBeams.Count > 0)
        {
            int n = fadingBeams.Count - 1;
            Destroy(fadingBeams[n]);
        }        
    }
    void DealDamageWithRaycasts(float damage, float damageRate, Vector3 pointOne, Vector3 pointTwo, float width, int pierce)
    {
        
        if (damageTimer <= 0)
        {
            float maxDistance = GetComponent<TowerStats>().maxTravelDistance;
            LayerMask layerMask = LayerMask.GetMask("Enemy");
            Vector3 targetPos = FindLaserEndPoint(pointOne, pointTwo, width, pierce);
            RaycastHit[] hits = Physics.SphereCastAll(pointOne, width, targetPos - pointOne, maxDistance, layerMask);
            List<GameObject> enemies = new List<GameObject>();
            if (hits.Length > 0)
            {
                for (int i = 0; i < hits.Length - 1; ++i)
                {
                    for (int j = 0; j < hits.Length - i - 1; ++j)
                    {
                        if (Vector3.Distance(pointOne, hits[j].transform.position) > Vector3.Distance(pointOne, hits[j + 1].transform.position))
                        {
                            RaycastHit temp = hits[j];
                            hits[j] = hits[j + 1];
                            hits[j + 1] = temp;
                        }
                    }
                }
            }
            for (int i = 0; i < pierce && i < hits.Length; ++i)
            {
                enemies.Add(hits[i].collider.gameObject);
            }
            foreach (GameObject e in enemies)
            {
                e.GetComponent<EnemyAI>().TakeDamage((damage + damage * e.GetComponent<RadiationManager>().radCount / 100), gameObject);
                e.GetComponent<RadiationManager>().IncreaseRadCount(gameObject);
            }
            damageTimer = 60 / damageRate;
        }
        damageTimer = damageTimer > 0 ? damageTimer - Time.deltaTime : 0;
    }
    void DealDamageWithRaycasts(float damage, Vector3 pointOne, Vector3 pointTwo, float width, int pierce)
    {
        float maxDistance = GetComponent<TowerStats>().maxTravelDistance;
        LayerMask layerMask = LayerMask.GetMask("Enemy");
        Vector3 targetPos = FindLaserEndPoint(pointOne, pointTwo, width, pierce);
        RaycastHit[] hits = Physics.SphereCastAll(pointOne, width, targetPos - pointOne, maxDistance, layerMask);
        List<GameObject> enemies = new List<GameObject>();
        if (hits.Length > 0)
        {
            for (int i = 0; i < hits.Length - 1; ++i)
            {
                for (int j = 0; j < hits.Length - i - 1; ++j)
                {
                    if (Vector3.Distance(pointOne, hits[j].transform.position) > Vector3.Distance(pointOne, hits[j + 1].transform.position))
                    {
                        RaycastHit temp = hits[j];
                        hits[j] = hits[j + 1];
                        hits[j + 1] = temp;
                    }
                }
            }
        }
        for (int i = 0; i < pierce && i < hits.Length; ++i)
        {
            enemies.Add(hits[i].collider.gameObject);
        }
        foreach (GameObject e in enemies)
        {
            e.GetComponent<EnemyAI>().TakeDamage((damage + damage * e.GetComponent<RadiationManager>().radCount / 100), gameObject);
            e.GetComponent<RadiationManager>().IncreaseRadCount(gameObject);
        }
    }
    void ResetTower()
    {
        CancelInvoke();
        if (beam != null)
            Destroy(beam);
        if (beamEnd != null)
            Destroy(beamEnd);
        if (preBeam != null)
            Destroy(preBeam);
        for (int i = 0; i < fadingBeams.Count; ++i)
        {
            Destroy(fadingBeams[i]);
        }
        shotTimer = 60 / GetComponent<TowerStats>().fireRate;
        currentChargeTime = chargeTime;
        beamTimer = beamDuration;
        firing = false;
    }
}
