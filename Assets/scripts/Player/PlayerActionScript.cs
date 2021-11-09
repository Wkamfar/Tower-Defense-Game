using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActionScript : MonoBehaviour
{
    public Vector3 mousePos;
    public int SpawningHeight;
    void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Debug.Log("The mouse position is: (" + mousePos.x + ", " + mousePos.z + ")");
    }
}
