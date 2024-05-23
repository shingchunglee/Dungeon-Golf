using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipSystem : MonoBehaviour
{
    private static TooltipSystem instance;

    public static TooltipSystem Instance { get { return instance; } }

    public Tooltip tooltip;

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

    public static void Show(string body, string header = "")
    {
        if (Instance == null || Instance.tooltip == null) return;
        Instance.tooltip.SetText(body, header);
        Instance.tooltip.gameObject.SetActive(true);
    }

    public static void Hide()
    {
        if (Instance == null || Instance.tooltip == null) return;
        Instance.tooltip.gameObject.SetActive(false);
    }
}
