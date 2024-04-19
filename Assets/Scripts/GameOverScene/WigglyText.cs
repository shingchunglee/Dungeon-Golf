using UnityEngine;
using TMPro;
using System.Collections;

public class WigglyText : MonoBehaviour
{
    public float wiggleAmount = 0.5f;
    public float wiggleSpeed = 5f;
    public float rotationMultiplier = 3f;
    public float flickerMinTime = 0.05f;
    public float flickerMaxTime = 0.2f;

    private TMP_Text textMeshPro;
    private float timeOffset;

    void Start()
    {
        textMeshPro = GetComponent<TMP_Text>();
        timeOffset = Random.Range(0f, 100f);
        StartCoroutine(FlickerText());
    }

    void Update()
    {
        WobbleText();
    }

    void WobbleText()
    {
        textMeshPro.ForceMeshUpdate();
        var textInfo = textMeshPro.textInfo;

        for (int i = 0; i < textInfo.characterCount; ++i)
        {
            var charInfo = textInfo.characterInfo[i];
            if (!charInfo.isVisible)
                continue;

            var verts = textInfo.meshInfo[charInfo.materialReferenceIndex].vertices;
            for (int j = 0; j < 4; ++j)
            {
                var orig = verts[charInfo.vertexIndex + j];
                verts[charInfo.vertexIndex + j] = orig + new Vector3(
                    Mathf.Sin(Time.time * wiggleSpeed + orig.x * 0.01f + timeOffset) * wiggleAmount,
                    Mathf.Cos(Time.time * wiggleSpeed + orig.y * 0.01f + timeOffset) * wiggleAmount,
                    0f);
            }
            
            textMeshPro.UpdateVertexData(TMP_VertexDataUpdateFlags.Vertices);
        }
    }

    IEnumerator FlickerText()
    {
        while (true)
        {
            textMeshPro.alpha = Random.Range(0.4f, 0.6f); 
            yield return new WaitForSeconds(Random.Range(flickerMinTime, flickerMaxTime));
            textMeshPro.alpha = 1f;
            yield return new WaitForSeconds(Random.Range(flickerMinTime, flickerMaxTime));
        }
    }
}


