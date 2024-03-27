using UnityEngine;

public class AimingIndicatorController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        // Cache the SpriteRenderer component for efficiency
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        // Optional: Hide the aiming indicator at the start
        HideIndicator();
    }

    void Update()
    {
        // Check if the left mouse button is pressed
        if (Input.GetMouseButtonDown(0)) // 0 is the left mouse button
        {
            ShowIndicator();
        }

        // Check if the left mouse button is released
        if (Input.GetMouseButtonUp(0))
        {
            HideIndicator();
        }
    }

    public void ShowIndicator()
    {
        // Make the aiming indicator visible
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = true;
        }
    }

    public void HideIndicator()
    {
        // Hide the aiming indicator
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = false;
        }
    }
}
