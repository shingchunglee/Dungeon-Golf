using System.Collections.Generic;
using UnityEngine;

public class ResourcesCache
{
    private static ResourcesCache instance;

    private ResourcesCache() { }

    public static ResourcesCache Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new ResourcesCache();
            }
            return instance;
        }
    }
    private Dictionary<string, GameObject> prefabCache = new Dictionary<string, GameObject>();
    private Dictionary<string, Sprite> spriteCache = new Dictionary<string, Sprite>();

    public GameObject GetPrefab(string location)
    {
        if (!prefabCache.ContainsKey(location))
        {
            prefabCache[location] = Resources.Load<GameObject>(location);
        }
        return prefabCache[location];
    }

    public Sprite GetSprite(string location)
    {
        if (!spriteCache.ContainsKey(location))
        {
            spriteCache[location] = Resources.Load<Sprite>(location);
        }
        return spriteCache[location];
    }
}