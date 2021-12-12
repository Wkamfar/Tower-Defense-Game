using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerTargeting : MonoBehaviour
{
    //add anything that comes into the radius and can be shot into a list, and then from there choose the priority, either strongest target, first target, or last target, and then remove them when they leave the radius
    private List<GameObject> targets = new List<GameObject>();
    private void Update()
    {
        targets = GetComponent<TowerStats>().targets;
    }
    public GameObject targeting(int mode)
    {
        if (mode == 0)
        {
            return FirstEnemy();
        }
        else if (mode == 1)
        {
            return LastEnemy();
        }
        else if (mode == 2)
        {
            return StrongestEnemy();
        }
        else if (mode == 3)
        {
            return WeakestEnemy();
        }
        //Add more firing modes later, and maybe some specialty ones
        return null;
    }
    public GameObject StrongestEnemy()
    {
        if (targets.Count > 0)
        {

        }
        return null;
    }
    public GameObject WeakestEnemy()
    {
        if (targets.Count > 0)
        {

        }
        return null;
    }//Do these two later
    public GameObject FirstEnemy()
    {
        while (targets.Count > 0 && (!targets[0] || targets[0].GetComponent<EnemyAI>().IsDead))
        {
            DeleteDeadObject(0);
        }
        if (targets.Count > 0)
        {
            return targets[0];
        }
        return null;
    }
    public GameObject LastEnemy()
    {
        if (targets.Count > 0)
        {
            return targets[targets.Count - 1];
        }
        return null;
    }
    private void DeleteDeadObject(int i)
    {
        targets.Remove(targets[i]);
    }
}
