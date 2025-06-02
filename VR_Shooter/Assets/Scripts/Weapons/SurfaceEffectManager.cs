using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class SurfaceEffect
{
    public string tag;
    public GameObject particlePrefab;
}

public class SurfaceEffectManager : MonoBehaviour
{
    public List<SurfaceEffect> surfaceEffects;

    private Dictionary<string, GameObject> effectDict;

    void Awake()
    {
        effectDict = new Dictionary<string, GameObject>();
        foreach (var effect in surfaceEffects)
        {
            if (!effectDict.ContainsKey(effect.tag))
                effectDict.Add(effect.tag, effect.particlePrefab);
        }
    }

    public GameObject GetEffectForTag(string tag)
    {
        if (effectDict != null && effectDict.TryGetValue(tag, out var prefab))
            return prefab;
        return null;
    }
} 