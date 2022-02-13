using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSpecialRequirement : MonoBehaviour
{
    public virtual bool HasMetSpecialRequirement(GameObject mouseFollower)
    {
        return true;
    }
}
