using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public List<GameObject> activeEffects = new List<GameObject>();
    public void AddEffect(GameObject effect)
    {
        activeEffects.Add(effect);
        for (int i = 0; i < activeEffects.Count - 1; ++i)
        {
            if (activeEffects[i].GetComponent<LiquidEffect>().effectID == effect.GetComponent<LiquidEffect>().effectID)
            {
                if (activeEffects[i].GetComponent<LiquidEffect>().isPermanent)
                {
                    RemoveEffect(effect);
                }
                else if (activeEffects[i].GetComponent<LiquidEffect>().effectDuration >= effect.GetComponent<LiquidEffect>().effectDuration)
                {
                    RemoveEffect(effect);
                }
                else
                {
                    RemoveEffect(activeEffects[i]);
                    break;
                }
            }
        }
    }
    public void RemoveEffect(GameObject effect)
    {
        activeEffects.Remove(effect);
        Destroy(effect);
    }
}
