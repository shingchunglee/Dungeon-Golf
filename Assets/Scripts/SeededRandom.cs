
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SeededRandom : MonoBehaviour
{
    [SerializeField] private int seed = -1;
    private static SeededRandom instance;
    public static SeededRandom Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SeededRandom>();
                if (instance == null)
                {
                    GameObject obj = new();
                    obj.name = "SeededRandom";
                    instance = obj.AddComponent<SeededRandom>();
                }
            }
            return instance;
        }
    }

    private SeededRandom() { }

    public System.Random MapRandom;
    public System.Random ItemRandom;

    private void Awake()
    {
        System.Random rand = new(seed >= 0 ? seed : Environment.TickCount);

        MapRandom = new System.Random(rand.Next());

        ItemRandom = new System.Random(rand.Next());
    }

    public void save()
    {
        BinaryFormatter formatter = new();
        MemoryStream mapStream = new();
        formatter.Serialize(mapStream, MapRandom);

        MemoryStream itemStream = new();
        formatter.Serialize(itemStream, MapRandom);
    }

    public void restore(MemoryStream mapStream, MemoryStream itemStream)
    {
        BinaryFormatter formatter = new();

        MapRandom = (System.Random)formatter.Deserialize(mapStream);
        ItemRandom = (System.Random)formatter.Deserialize(itemStream);
    }

    public static int Range(System.Random rand, int min, int max)
    {
        return rand.Next(min, max);
    }

    public static float Range(System.Random rand, float min, float max)
    {
        return ((float)rand.NextDouble() * (max - min)) + min;
    }
}
