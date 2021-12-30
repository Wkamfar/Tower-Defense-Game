using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectTwoScript : MonoBehaviour
{
    [SerializeField] GameObject objectOne;
    [SerializeField] GameObject objectTwo;

    void Update()
    {
        RaycastHit hit;
        LayerMask layerMask = LayerMask.GetMask("Enemy");
        float x = Mathf.Abs(objectOne.transform.position.x - objectTwo.transform.position.x);
        float y = Mathf.Abs(objectOne.transform.position.y - objectTwo.transform.position.y);
        float d = Mathf.Sqrt(x * x + y * y);
        if (Physics.SphereCast(objectTwo.transform.position, objectTwo.transform.localScale.x / 2, objectOne.transform.position - objectTwo.transform.position, out hit, d, layerMask))
        {
            GetComponent<LaserScript>().ActivateLaser(objectOne, objectTwo);
            //Debug.Log("Wall has been hit!");
        }
        else
        {
            GetComponent<LaserScript>().DeactivateLaser();
        }

    }
}
