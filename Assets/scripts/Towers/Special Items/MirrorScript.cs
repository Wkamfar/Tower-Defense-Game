using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorScript : MonoBehaviour
{
    [SerializeField] private GameObject localGameObject;
    private List<Vector3> beamEnds = new List<Vector3>();
    private List<GameObject> towers = new List<GameObject>();
    public void FindReflectedBeamEndPoint(GameObject tower, Vector3 pointOne, Vector3 hitPoint, float maxDistance, float width, int pierce)
    {
        Vector3 beamEnd = new Vector3();
        GameObject localPointOne = Instantiate(localGameObject, pointOne, Quaternion.identity, transform);
        GameObject localHitPoint = Instantiate(localGameObject, hitPoint, Quaternion.identity, transform);
        float x = Mathf.Abs(localPointOne.transform.localPosition.x - localHitPoint.transform.localPosition.x);
        float y = Mathf.Abs(localPointOne.transform.localPosition.z - localHitPoint.transform.localPosition.z);
        //if (< localHitPoint.transform.localPosition.x < )
        float distance = Mathf.Sqrt(x * x + y * y);
        //float radiant = Mathf.atan()
        Debug.Log("Distance = " + distance);
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
        }
    }
    public void ReflectBeamDisplay(GameObject  tower, Vector3 beamStart)
    {
        
    }
    public void ReflectBeamDamage(GameObject tower)
    {

    }
}
