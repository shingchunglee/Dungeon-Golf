using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public float alphaOnHover = 0.5f; 

    private Image buttonImage;
    private Color originalColor;
    
    void Start()
    {
        buttonImage = GetComponent<Image>();
        if (buttonImage != null)
        {
            originalColor = buttonImage.color; 
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (buttonImage != null)
        {
            buttonImage.color = new Color(originalColor.r, originalColor.g, originalColor.b, alphaOnHover);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (buttonImage != null)
        {
            buttonImage.color = originalColor;
        }
    }
}

