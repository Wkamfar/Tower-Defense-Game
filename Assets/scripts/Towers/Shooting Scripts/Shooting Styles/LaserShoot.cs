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
    public bool hasReflectedBeam;
    private Vector3 mirrorHitPoint; // make this a game object and a Vector3
    private GameObject currentMirror;
    public float beamEndDamageMultiplier;
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
    public GameObject localPositionChecker;
    private void Start()
    {
        currentChargeTime = chargeTime;
        beamTimer = beamDuration;
    }
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
        currentChargeTime = chargeTime;
        Destroy(preBeam);
        if (hasReflectedBeam)
        {
            currentMirror.GetComponent<MirrorScript>().DeleteReflectedBeam(gameObject);
            hasReflectedBeam = false;
        }
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
            UseLaser(GetComponent<TowerStats>().shootPoint.transform.position, GetComponent<TowerStats>().currentAimLocation, preBeam, preBeamWidth, preBeamPierce);
            currentChargeTime -= Time.deltaTime;
            return false;
        }
        return true;
    }
    public void UseLaser(Vector3 pointOne, Vector3 pointTwo, GameObject laser, float width, int pierce)
    {
        Vector3 targetPos = FindLaserEndPoint(pointOne, pointTwo, width, pierce);
        float length = Vector3.Distance(pointOne, targetPos);
        Vector3 middlePoint = new Vector3((pointOne.x + targetPos.x) / 2,
                                          pointOne.y,
                                          (pointOne.z + targetPos.z) / 2);
        laser.transform.position = middlePoint;
        float x = pointOne.x - targetPos.x;
        float y = pointOne.z - targetPos.z;
        float d = Mathf.Sqrt(x * x + y * y);
        laser.transform.localScale = new Vector3(length, width, width);
        float angle = 180 - Mathf.Abs(Mathf.Asin(x / d) * 180 / Mathf.PI - 90);
        if (y < 0)
        {
            angle = 360 - angle;
        }
        laser.transform.rotation = Quaternion.Euler(0, angle, 0);
        if (hasReflectedBeam)
        {
            currentMirror.GetComponent<MirrorScript>().ReflectBeamDisplay(gameObject, mirrorHitPoint, laser, width);
        }
    }
    private Vector3 FindLaserEndPoint(Vector3 pointOne, Vector3 pointTwo, float width, int pierce) // adjust for mirrors later
    {
        Ray ray = new Ray(pointOne, (new Vector3(pointTwo.x, pointOne.y, pointTwo.z) - pointOne).normalized);
        bool hitsMirror = false;
        RaycastHit hit;
        float maxDistance = GetComponent<TowerStats>().maxTravelDistance;
        LayerMask layerMask = LayerMask.GetMask("Shield");
        //float a = pointTwo.x - pointOne.x;
        //float b = pointTwo.z - pointOne.z;
        //float m = maxDistance - Vector3.Distance(pointTwo, pointOne);
        //Vector3 furthestPoint = new Vector3(pointTwo.x + a * (m - 1), pointTwo.y, pointTwo.z + b * (m - 1));
        Vector3 furthestPoint = ray.GetPoint(maxDistance);
        if (Physics.SphereCast(ray, width / 2, out hit, maxDistance, layerMask))
        {
            furthestPoint = new Vector3(hit.point.x, GetComponent<TowerStats>().shootPoint.transform.position.y, hit.point.z);
            maxDistance = Vector3.Distance(furthestPoint, pointOne);
            /*RaycastHit raycastHit;
            if (Physics.Raycast(ray, out raycastHit, Vector3.Distance(hit.point, pointOne) + width, layerMask))
            {
                furthestPoint = raycastHit.point;
                maxDistance = Vector3.Distance(furthestPoint, pointOne);
            }
            else
            {
                float x = pointOne.x - pointTwo.x;
                float y = pointOne.z - pointTwo.z;
                float d = Mathf.Sqrt(x * x + y * y);
                float angle = 180 - Mathf.Abs(Mathf.Asin(x / d) * 180 / Mathf.PI - 90);
                if (y < 0)
                {
                    angle = 360 - angle;
                }
                GameObject localPointOne = Instantiate(localPositionChecker, pointOne, Quaternion.Euler(0, angle, 0));
                GameObject localHitPoint = Instantiate(localPositionChecker, hit.point, Quaternion.Euler(0, angle, 0), localPointOne.transform);
                float localZ = localHitPoint.transform.localPosition.z - localPointOne.transform.localPosition.z;
                maxDistance = Vector3.Distance(hit.point, new Vector3(localPointOne.transform.localPosition.x, localPointOne.transform.localPosition.y, localPointOne.transform.localPosition.z + localZ));
                furthestPoint = ray.GetPoint(maxDistance);
                Destroy(localPointOne);
                Destroy(localHitPoint);
                //furthestPoint = hit.point;
                //maxDistance = Vector3.Distance(furthestPoint, pointOne);
                //furthestPoint = ray.GetPoint(Vector3.Distance(pointOne, hit.point) + 5);
                //maxDistance = Vector3.Distance(furthestPoint, pointOne);
                //Debug.Log("Furthest point is located at: " + furthestPoint);
                //Debug.Log("hit.point is located at: " + hit.point);
            }*/

        }
        layerMask = LayerMask.GetMask("Mirror");
        if (Physics.SphereCast(ray, width / 2, out hit, maxDistance, layerMask))
        {
            furthestPoint = new Vector3(hit.point.x, GetComponent<TowerStats>().shootPoint.transform.position.y, hit.point.z);
            maxDistance = Vector3.Distance(furthestPoint, pointOne);
            if (hasReflectedBeam && currentMirror != hit.collider.gameObject)
                currentMirror.GetComponent<MirrorScript>().DeleteReflectedBeam(gameObject);
            currentMirror = hit.collider.gameObject;
            mirrorHitPoint = new Vector3(hit.point.x, GetComponent<TowerStats>().shootPoint.transform.position.y, hit.point.z);
            hitsMirror = true;
        }
        layerMask = LayerMask.GetMask("Enemy");
        if (Physics.SphereCast(ray, width / 2, out hit, maxDistance, layerMask))
        {
            RaycastHit[] hits = Physics.SphereCastAll(ray, width / 2, maxDistance, layerMask);
            for (int i = 0; i < hits.Length; ++i)
            {
                hits[i].point = new Vector3(hits[i].point.x, GetComponent<TowerStats>().shootPoint.transform.position.y, hits[i].point.z);
            }
            if (hits.Length > 0)
            {
                if (hits.Length < pierce)
                {
                    if (hitsMirror)
                    {
                        currentMirror.GetComponent<MirrorScript>().FindReflectedBeamEndPoint(gameObject, pointOne, mirrorHitPoint, GetComponent<TowerStats>().maxTravelDistance, width, pierce - hits.Length);
                        hasReflectedBeam = true;
                    }
                    return furthestPoint;
                }
                if (hasReflectedBeam && !hitsMirror)
                {
                    currentMirror.GetComponent<MirrorScript>().DeleteReflectedBeam(gameObject);
                    hasReflectedBeam = false;
                }
                for (int i = 0; i < hits.Length - 1; ++i)
                {
                    for (int j = 0; j < hits.Length - i - 1; ++j)
                    {
                        if (Vector3.Distance(pointOne, new Vector3(hits[j].transform.position.x, GetComponent<TowerStats>().shootPoint.transform.position.y, hits[j].transform.position.z)) > Vector3.Distance(pointOne, new Vector3(hits[j + 1].transform.position.x, GetComponent<TowerStats>().shootPoint.transform.position.y, hits[j + 1].transform.position.z)))
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
        if (hitsMirror)
        {
            currentMirror.GetComponent<MirrorScript>().FindReflectedBeamEndPoint(gameObject, pointOne, mirrorHitPoint, GetComponent<TowerStats>().maxTravelDistance, width, pierce);
            hasReflectedBeam = true;
        }
        else if (hasReflectedBeam && !hitsMirror)
        {
            currentMirror.GetComponent<MirrorScript>().DeleteReflectedBeam(gameObject);
            hasReflectedBeam = false;
        }
        return furthestPoint;
    }
    void ContinuousFire()
    {
        beamTimer -= Time.deltaTime;
        if (beamTimer > 0)
        {
            UseLaser(GetComponent<TowerStats>().shootPoint.transform.position, GetComponent<TowerStats>().currentAimLocation, beam, beamWidth, GetComponent<TowerStats>().pierce);
            DealDamageWithRaycasts(GetComponent<TowerStats>().damage, laserDamageRate, GetComponent<TowerStats>().shootPoint.transform.position, GetComponent<TowerStats>().currentAimLocation, beamWidth, GetComponent<TowerStats>().pierce);
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
        UseLaser(GetComponent<TowerStats>().shootPoint.transform.position, GetComponent<TowerStats>().currentAimLocation, beam, beamWidth, GetComponent<TowerStats>().pierce); // Destroy beam and play an animation
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
        if (hasReflectedBeam)
        {
            currentMirror.GetComponent<MirrorScript>().DeleteReflectedBeam(gameObject);
            hasReflectedBeam = false;
        }
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
            LayerMask layerMask = LayerMask.GetMask("Enemy");
            Vector3 targetPos = FindLaserEndPoint(pointOne, pointTwo, width, pierce);
            float maxDistance = Vector3.Distance(targetPos, pointOne);
            RaycastHit[] hits = Physics.SphereCastAll(pointOne, width / 2, targetPos - pointOne, maxDistance, layerMask);
            for (int i = 0; i < hits.Length; ++i)
            {
                hits[i].point = new Vector3(hits[i].point.x, GetComponent<TowerStats>().shootPoint.transform.position.y, hits[i].point.z);
            }
            List<GameObject> enemies = new List<GameObject>();
            if (hits.Length > 0)
            {
                for (int i = 0; i < hits.Length - 1; ++i)
                {
                    for (int j = 0; j < hits.Length - i - 1; ++j)
                    {
                        if (Vector3.Distance(pointOne, new Vector3(hits[j].transform.position.x, pointOne.y, hits[j].transform.position.z)) > Vector3.Distance(pointOne, new Vector3(hits[j + 1].transform.position.x, pointOne.y, hits[j + 1].transform.position.z)))
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
            if (enemies.Count < pierce)
            {
                string hitLayer = "";
                float closestDistance = -1;
                RaycastHit closestHit = new RaycastHit();
                layerMask = LayerMask.GetMask("Shield");
                RaycastHit hit;
                if (Physics.SphereCast(pointOne, width / 2, targetPos - pointOne, out hit, maxDistance, layerMask))
                {
                    if (closestDistance == -1)
                    {
                        hitLayer = LayerMask.LayerToName(hit.collider.gameObject.layer);
                        closestDistance = Vector3.Distance(pointOne, new Vector3(hit.point.x, pointOne.y, hit.point.z));
                        closestHit = hit;
                    }
                }
                layerMask = LayerMask.GetMask("Mirror");
                if (Physics.SphereCast(pointOne, width / 2, targetPos - pointOne, out hit, closestDistance, layerMask))
                {
                    if (closestDistance == -1)
                    {
                        hitLayer = LayerMask.LayerToName(hit.collider.gameObject.layer);
                        closestDistance = Vector3.Distance(pointOne, new Vector3(hit.point.x, pointOne.y, hit.point.z));
                        closestHit = hit;
                    }
                    else if (closestDistance >= Vector3.Distance(pointOne, new Vector3(hit.point.x, pointOne.y, hit.point.z)))
                    {
                        hitLayer = LayerMask.LayerToName(hit.collider.gameObject.layer);
                        closestDistance = Vector3.Distance(pointOne, new Vector3(hit.point.x, pointOne.y, hit.point.z));
                        closestHit = hit;
                    }
                }
                if (hitLayer == "Shield")
                {
                    closestHit.collider.GetComponentInParent<ShielderScript>().TakeShieldDamage(damage, gameObject);
                }
                else if (hitLayer == "Mirror")
                {

                }

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
        LayerMask layerMask = LayerMask.GetMask("Enemy");
        Vector3 targetPos = FindLaserEndPoint(pointOne, pointTwo, width, pierce);
        float maxDistance = Vector3.Distance(targetPos, pointOne);
        RaycastHit[] hits = Physics.SphereCastAll(pointOne, width / 2, targetPos - pointOne, maxDistance, layerMask);
        for (int i = 0; i < hits.Length; ++i)
        {
            hits[i].point = new Vector3(hits[i].point.x, GetComponent<TowerStats>().shootPoint.transform.position.y, hits[i].point.z);
        }
        List<GameObject> enemies = new List<GameObject>();
        if (hits.Length > 0)
        {
            for (int i = 0; i < hits.Length - 1; ++i)
            {
                for (int j = 0; j < hits.Length - i - 1; ++j)
                {
                    if (Vector3.Distance(pointOne, new Vector3(hits[j].transform.position.x, pointOne.y, hits[j].transform.position.z)) > Vector3.Distance(pointOne, new Vector3(hits[j + 1].transform.position.x, pointOne.y, hits[j + 1].transform.position.z)))
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
        if (enemies.Count < pierce)
        {
            string hitLayer = "";
            float closestDistance = -1;
            RaycastHit closestHit = new RaycastHit();
            layerMask = LayerMask.GetMask("Shield");
            RaycastHit hit;
            if (Physics.SphereCast(pointOne, width / 2, targetPos - pointOne, out hit, maxDistance, layerMask))
            {
                if (closestDistance == -1)
                {
                    hitLayer = LayerMask.LayerToName(hit.collider.gameObject.layer);
                    closestDistance = Vector3.Distance(pointOne, new Vector3(hit.point.x, pointOne.y, hit.point.z));
                    closestHit = hit;
                }
            }
            layerMask = LayerMask.GetMask("Mirror");
            if (Physics.SphereCast(pointOne, width / 2, targetPos - pointOne, out hit, closestDistance, layerMask))
            {
                if (closestDistance == -1)
                {
                    hitLayer = LayerMask.LayerToName(hit.collider.gameObject.layer);
                    closestDistance = Vector3.Distance(pointOne, new Vector3(hit.point.x, pointOne.y, hit.point.z));
                    closestHit = hit;
                }
                else if (closestDistance >= Vector3.Distance(pointOne, new Vector3(hit.point.x, pointOne.y, hit.point.z)))
                {
                    hitLayer = LayerMask.LayerToName(hit.collider.gameObject.layer);
                    closestDistance = Vector3.Distance(pointOne, new Vector3(hit.point.x, pointOne.y, hit.point.z));
                    closestHit = hit;
                }
            }
            if (hitLayer == "Shield")
            {
                closestHit.collider.GetComponentInParent<ShielderScript>().TakeShieldDamage(damage, gameObject);
            }
            else if (hitLayer == "Mirror")
            {

            }

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
        if (hasReflectedBeam)
        {
            currentMirror.GetComponent<MirrorScript>().DeleteReflectedBeam(gameObject);
            hasReflectedBeam = false;
        }
        for (int i = 0; i < fadingBeams.Count; ++i)
        {
            Destroy(fadingBeams[i]);
        }
        shotTimer = 60 / GetComponent<TowerStats>().fireRate;
        currentChargeTime = chargeTime;
        beamTimer = beamDuration;
        firing = false;
    }
    public override void OnDestroyTower()
    {
        ResetTower();
    }
}
