using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorScript : MonoBehaviour
{
    [SerializeField] private GameObject localGameObject;
    private List<Vector3> beamEnds = new List<Vector3>();
    private List<GameObject> towers = new List<GameObject>();
    private List<GameObject> reflectedLasers = new List<GameObject>();
    private RaycastHit mirrorHitPoint;
    private bool hasReflectedBeam;
    public void FindReflectedBeamEndPoint(GameObject tower, Vector3 pointOne, Vector3 hitPoint, float maxDistance, float width, int pierce)
    {
        Vector3 beamEnd = new Vector3();
        GameObject localPointOne = Instantiate(localGameObject, pointOne, Quaternion.identity, transform);
        GameObject localHitPoint = Instantiate(localGameObject, hitPoint, Quaternion.identity, transform);
        localHitPoint.transform.localPosition = new Vector3(Mathf.Round(localHitPoint.transform.localPosition.x * 1000) / 1000, localHitPoint.transform.localPosition.x, Mathf.Round(localHitPoint.transform.localPosition.z * 1000) / 1000);
        float x = localPointOne.transform.localPosition.x - localHitPoint.transform.localPosition.x;
        float y = localPointOne.transform.localPosition.z - localHitPoint.transform.localPosition.z;
        float distance = Mathf.Sqrt(x * x + y * y);
        if (-GetComponent<SpecialItemStats>().hitbox < localHitPoint.transform.localPosition.x && localHitPoint.transform.localPosition.x < GetComponent<SpecialItemStats>().hitbox)
        {
            float multiplier = maxDistance / distance;
            beamEnd = new Vector3(-x * multiplier, hitPoint.y, y * multiplier);
        }
        else 
        {
            float multiplier = maxDistance / distance;
            beamEnd = new Vector3(x * multiplier, hitPoint.y, -y * multiplier);
        }
        beamEnd = FindLaserEndPoint(hitPoint, beamEnd, width, pierce, Vector3.Distance(beamEnd, hitPoint));
        Destroy(localPointOne);
        Destroy(localHitPoint);
        bool hasTower = false;
        for (int i = 0; i < towers.Count; ++i)
        {
            if (towers[i] == tower)
            {
                beamEnds[i] = beamEnd;
                hasTower = true;
                break;
            }
        }
        if (!hasTower)
        {
            towers.Add(tower);
            beamEnds.Add(beamEnd);
            reflectedLasers.Add(null);
        }
    }
    private Vector3 FindLaserEndPoint(Vector3 pointOne, Vector3 pointTwo, float width, int pierce, float maxDistance) // adjust for mirrors later
    {
        bool hitsMirror = false;
        RaycastHit hit;
        LayerMask layerMask = LayerMask.GetMask("Shield");
        float m = maxDistance / Vector3.Distance(pointOne, pointTwo);
        float x = (pointTwo.x - pointOne.x) * m;
        float z = (pointTwo.z - pointOne.z) * m;
        Vector3 furthestPoint = new Vector3(pointOne.x + x, transform.position.y, pointOne.z + z);
        if (Physics.SphereCast(pointOne, width / 2, pointTwo - pointOne, out hit, maxDistance, layerMask))
        {
            furthestPoint = hit.point;
            maxDistance = Vector3.Distance(furthestPoint, pointOne);
        }
        layerMask = LayerMask.GetMask("Mirror");
        if (Physics.SphereCast(pointOne, width / 2, pointTwo - pointOne, out hit, maxDistance, layerMask))
        {
            furthestPoint = hit.point;
            maxDistance = Vector3.Distance(furthestPoint, pointOne);
            mirrorHitPoint = hit;
            hitsMirror = true;
        }
        layerMask = LayerMask.GetMask("Enemy");
        if (Physics.SphereCast(pointOne, width / 2, pointTwo - pointOne, out hit, maxDistance, layerMask))
        {
            RaycastHit[] hits = Physics.SphereCastAll(pointOne, width /2, pointTwo - pointOne, maxDistance, layerMask);
            if (hits.Length > 0)
            {
                if (hits.Length < pierce)
                {
                    if (hitsMirror)
                    {
                        mirrorHitPoint.collider.gameObject.GetComponent<MirrorScript>().FindReflectedBeamEndPoint(gameObject, pointOne, mirrorHitPoint.point, maxDistance, width, pierce);
                        hasReflectedBeam = true;
                    }
                    return furthestPoint;
                }
                hasReflectedBeam = false;
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
        if (hitsMirror)
        {
            mirrorHitPoint.collider.gameObject.GetComponent<MirrorScript>().FindReflectedBeamEndPoint(gameObject, pointOne, mirrorHitPoint.point, maxDistance, width, pierce);
            hasReflectedBeam = true;
        }
        return furthestPoint;
    }
    public void ReflectBeamDisplay(GameObject  tower, Vector3 beamStart, GameObject beamMaterial, float visibleWidth)
    {
        Vector3 beamEnd = new Vector3();
        GameObject reflectedLaser = null;
        for (int i = 0; i < towers.Count; ++i)
        {
            if (towers[i] == tower)
            {
                beamEnd = beamEnds[i];
                if (reflectedLasers[i] != null)
                {
                    reflectedLaser = reflectedLasers[i];

                }
                else
                {
                    reflectedLasers[i] = Instantiate(beamMaterial);
                    reflectedLaser = reflectedLasers[i];
                }
            }
        }
        float length = Vector3.Distance(beamStart, beamEnd);
        Vector3 middlePoint = new Vector3((beamStart.x + beamEnd.x) / 2,
                                          (beamStart.y + beamEnd.y) / 2,
                                          (beamStart.z + beamEnd.z) / 2);
        reflectedLaser.transform.position = middlePoint;
        float x = beamStart.x - beamEnd.x;
        float y = beamStart.z - beamEnd.z;
        float d = Mathf.Sqrt(x * x + y * y);
        reflectedLaser.transform.localScale = new Vector3(length, visibleWidth, visibleWidth);
        float angle = 180 - Mathf.Abs(Mathf.Asin(x / d) * 180 / Mathf.PI - 90);
        if (y < 0)
        {
            angle = 360 - angle;
        }
        reflectedLaser.transform.rotation = Quaternion.Euler(0, angle, 0);
    }
    public void DeleteReflectedBeam(GameObject tower)
    {
        for (int i = 0; i < towers.Count; ++i)
        {
            if (towers[i] == tower)
            {
                towers.RemoveAt(i);
                beamEnds.RemoveAt(i);
                Destroy(reflectedLasers[i]);
                reflectedLasers.RemoveAt(i);
            }
        }
    }
    public void ReflectBeamDamage(GameObject tower)
    {

    }
}
