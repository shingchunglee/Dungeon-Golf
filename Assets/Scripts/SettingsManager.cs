using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    private static SettingsManager instance;

    public static SettingsManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SettingsManager>();

                if (instance == null)
                {
                    GameObject container = new GameObject("SettingsManager");
                    DontDestroyOnLoad(container);
                    instance = container.AddComponent<SettingsManager>();
                }
            }

            return instance;
        }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public GolfAimType golfAimType = GolfAimType.Click;
}
