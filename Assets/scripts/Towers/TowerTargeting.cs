using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerTargeting : MonoBehaviour
{
    //add anything that comes into the radius and can be shot into a list, and then from there choose the priority, either strongest target, first target, or last target, and then remove them when they leave the radius
    public List<GameObject> targets;
    private GameObject chosenTarget;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
