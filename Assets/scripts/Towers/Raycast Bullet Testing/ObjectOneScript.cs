using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectOneScript : MonoBehaviour
{
    [SerializeField] GameObject objectOne;
    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        objectOne.transform.position = new Vector3(mousePos.x, transform.position.y, mousePos.z);
    }
}
