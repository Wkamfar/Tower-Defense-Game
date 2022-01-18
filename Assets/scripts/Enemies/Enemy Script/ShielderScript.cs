using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShielderScript : EnemyAI // make energy shield, physical shield, and support shield / cloak that makes all towers under it camo and nullifies all tower effects
{
    public GameObject shield;
    public float maxShieldHealth;
    public float shieldHealth;
    public float shieldSize;
    public float restorationPercentage;
    public float shieldRechargeTime; 
    private float currentRechargeTime;
    public float shieldRepairCooldown;
    private float cooldownTimer;
    public float regenerationPercentage;
    public float regenerationInterval;
    private float currentIntervalTime;

    protected override void UseAbility()
    {
        if (shieldHealth > 0)
        {
            shield.SetActive(true);
            shield.transform.localScale = new Vector3(shieldSize, shieldSize, shieldSize);
            if (shieldHealth < maxShieldHealth)
            {
                cooldownTimer = cooldownTimer > 0 ? cooldownTimer -= Time.deltaTime : 0;
                if (cooldownTimer == 0)
                {
                    currentIntervalTime = currentIntervalTime > 0 ? currentIntervalTime -= Time.deltaTime : regenerationInterval;
                    if (currentIntervalTime! > 0)
                        shieldHealth = shieldHealth + shieldHealth * regenerationPercentage / 100 < maxShieldHealth ? shieldHealth + shieldHealth * regenerationPercentage / 100 : maxShieldHealth; //Debug.Log("ShielderScript.UseAbility: Healing Health by: " + shieldHealth * regenerationPercentage / 100);
                }
            }
        }
        else
        {
            shield.SetActive(false);
            currentRechargeTime = currentRechargeTime > 0 ? currentRechargeTime - Time.deltaTime : shieldRechargeTime;
            if (currentRechargeTime <= 0)
            {
                //Debug.Log("ShielderScript.UseAbility: This happened and The currentRechargeTime is: " + currentRechargeTime);
                shieldHealth = maxShieldHealth * restorationPercentage / 100;
            }
        }
    }
    public void TakeShieldDamage(float damage, GameObject tower)
    {
        float dealtDamage = shieldHealth >= damage ? damage : shieldHealth;
        shieldHealth -= dealtDamage;
        tower.GetComponent<TowerStats>().IncreaseDamageDealt(dealtDamage);
        cooldownTimer = shieldRepairCooldown;
        //Debug.Log("ShielderScript.TakeShieldDamage: The remaining health is: " + shieldHealth);
    }
}
