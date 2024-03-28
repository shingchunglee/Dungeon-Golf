using UnityEngine;

public class AimingIndicatorController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        HideIndicator();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            ShowIndicator();
        }
        
        if (Input.GetMouseButtonUp(0))
        {
            HideIndicator();
        }
    }

    public void ShowIndicator()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = true;
        }
    }

    public void HideIndicator()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = false;
        }
    }
}
