using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPlacerGroundCollision : MonoBehaviour
{
    public bool canPlace = true;
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Block")
        {
            canPlace = false;
        }
    }
    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Block")
        {
            canPlace = true;
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            canPlace = true;
        }
    }
    public bool CanPlace()
    {
        return canPlace;
    }
}
