using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiquidEffect : MonoBehaviour
{
    public string effectID;

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
        GetComponent<EffectManager>().RemoveEffect(gameObject);
    }
    private void Update()
    {
            ApplyEffect(effectIntensity, tower);
    }
    protected virtual void ApplyEffect(float intensity, GameObject tower)
    {

    }
    
}
