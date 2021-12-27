using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiquidEffect : MonoBehaviour
{
    public GameObject enemy;
    public GameObject tower;
    public float effectIntensity;
    public float effectDuration;
    public bool isPermanent;
    private void Start()
    {
        if (!isPermanent)
        {
            Invoke("DisableEffect", effectDuration);
        }
    }
    private void DisableEffect()
    {
        Destroy(this.gameObject);
    }
    private void Update()
    {
        ApplyEffect(effectIntensity, enemy, tower);
    }
    protected virtual void ApplyEffect(float intensity, GameObject enemy, GameObject tower)
    {

    }
    
}
