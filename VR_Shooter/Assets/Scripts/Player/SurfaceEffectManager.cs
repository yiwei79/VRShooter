using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

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

    public GameObject Canvas;

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
        if(tag== "Final")
        {
            Canvas.SetActive(true);

            // Load the Game scene when the tag is "Final" after 5 seconds with a coroutine
            Invoke(nameof(LoadGameScene), 5f);
        }
        if (effectDict != null && effectDict.TryGetValue(tag, out var prefab))
            return prefab;
        return null;
    }
    private void LoadGameScene()
    {
        SceneManager.LoadScene("Game");
    }
} 