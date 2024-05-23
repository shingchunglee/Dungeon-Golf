using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string header;
    public string body;

    IEnumerator ShowTooltip()
    {
        yield return new WaitForSeconds(0.3f);

        TooltipSystem.Show(body, header);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        // StartCoroutine(ShowTooltip());
        TooltipSystem.Show(body, header);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipSystem.Hide();
    }

    private void OnMouseEnter()
    {
        // StartCoroutine(ShowTooltip());
        TooltipSystem.Show(body, header);
    }

    private void OnMouseExit()
    {
        TooltipSystem.Hide();
    }
}