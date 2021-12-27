using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public List<GameObject> activeEffects = new List<GameObject>();
    public void AddEffect(GameObject effect)
    {
        activeEffects.Add(effect);
        for (int i = 0; i < activeEffects.Count; ++i)
        {
            for (int j = i + 1; j < activeEffects.Count; ++j)
            {
                if (activeEffects[i].GetComponent<LiquidEffect>().effectID == activeEffects[j].GetComponent<LiquidEffect>().effectID)
                {
                    if (activeEffects[i].GetComponent<LiquidEffect>().isPermanent)
                    {
                        RemoveEffect(activeEffects[j]);
                    }
                    else if (activeEffects[i].GetComponent<LiquidEffect>().effectDuration >= activeEffects[j].GetComponent<LiquidEffect>().effectDuration)
                    {
                        RemoveEffect(activeEffects[j]);
                    }
                    else
                    {
                        RemoveEffect(activeEffects[i]);
                        break;
                    }
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
