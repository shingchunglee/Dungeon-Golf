using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelUpUI : MonoBehaviour
{
    public TextMeshProUGUI levelUpText;
    public Color textColourBase;
    public Color textColourFlash;

    private RectTransform panelRectTransform;
    private Vector2 initialPosition;
    private Vector2 targetPosition;

    private bool isAnimationPlaying = false;

    private void Start()
    {
        levelUpText = GetComponentInChildren<TextMeshProUGUI>();
        panelRectTransform = GetComponent<RectTransform>();

    }

    public void LevelUpAnimation()
    {
        if (isAnimationPlaying == true) return;

        isAnimationPlaying = true;

        if (levelUpText == null)
        {
            Debug.LogError("Level UP UI text not found.");
            return;
        }

        StartCoroutine(MovePanel());
        StartCoroutine(FlashText());
    }

    private IEnumerator FlashText()
    {
        for (int i = 0; i < 40; i++)
        {
            levelUpText.color = textColourFlash;
            yield return new WaitForSeconds(0.1f);
            levelUpText.color = textColourBase;
            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator MovePanel()
    {
        initialPosition = panelRectTransform.anchoredPosition;
        targetPosition = new Vector2(panelRectTransform.anchoredPosition.x, 362f);

        // Ease-in move
        float elapsedTime = 0f;
        float moveDuration = 0.5f;
        while (elapsedTime < moveDuration)
        {
            float t = elapsedTime / moveDuration;
            float easedT = Mathf.Pow(t, 5); // Use SmoothStep for ease-in effect
            panelRectTransform.anchoredPosition = Vector2.LerpUnclamped(initialPosition, targetPosition, easedT);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        panelRectTransform.anchoredPosition = targetPosition;

        yield return new WaitForSeconds(1f);

        // Ease-out move
        elapsedTime = 0f;
        while (elapsedTime < moveDuration)
        {
            float t = elapsedTime / moveDuration;
            float easedT = Mathf.Pow(t, 5); // Use SmoothStep for ease-out effect
            panelRectTransform.anchoredPosition = Vector2.LerpUnclamped(targetPosition, initialPosition, easedT);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        isAnimationPlaying = false;
    }


}
