using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserScript : MonoBehaviour
{
    [SerializeField] GameObject laser;

    public void ActivateLaser(GameObject objectOne, GameObject objectTwo)
    {
        laser.SetActive(true);
        Vector3 middlePoint = new Vector3((objectOne.transform.position.x + objectTwo.transform.position.x) / 2, 
                                          (objectOne.transform.position.y + objectTwo.transform.position.y) / 2,    
                                          (objectOne.transform.position.z + objectTwo.transform.position.z) / 2);
        laser.transform.position = middlePoint;
        float x = objectOne.transform.position.x - objectTwo.transform.position.x;
        float y = objectOne.transform.position.z - objectTwo.transform.position.z;
        float d = Mathf.Sqrt(x * x + y * y);
        laser.transform.localScale = new Vector3(d + objectTwo.transform.localScale.x, objectTwo.transform.localScale.y, objectTwo.transform.localScale.z);
        float angle = 180 - Mathf.Abs(Mathf.Asin(x / d) * 180 / Mathf.PI - 90);
        if (y < 0)
        {
            angle = 360 - angle;
        }
        laser.transform.rotation = Quaternion.Euler(0, angle, 0);
        //Debug.Log("LaserScript.ActivateLaser: The laser angle is: " + angle);
    }
    public void DeactivateLaser()
    {
        laser.SetActive(false);
    }
}
