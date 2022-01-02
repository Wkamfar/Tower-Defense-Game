using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaEffect : LiquidEffect
{
    public int minDamageRate; // 60 / damageRate is the true damage rate
    public int maxDamageRate;
    private float damageRate;
    public int minEffectDamage;
    public int maxEffectDamage;
    protected override void ApplyEffect(float intensity, GameObject tower)
    { //Add more code here later, for now this is good
        if (damageRate <= 0)
        {
            float effectDamage = Random.Range(minEffectDamage, maxEffectDamage + 1) * intensity;
            transform.GetComponentInParent<EnemyAI>().TakeDamage(effectDamage, tower);
        }
        damageRate = damageRate <= 0 ? 60 / Random.Range(minDamageRate, maxDamageRate + 1) : damageRate - Time.deltaTime;
    }
}
