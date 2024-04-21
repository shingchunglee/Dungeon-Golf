using UnityEngine;

public class DragAimIndicator : MonoBehaviour
{
    private float amplitude = 0.01f;
    private float period = 1f;
    private float time = 0f;

    private void Start()
    {
    }

    private void FixedUpdate()
    {
        time += Time.fixedDeltaTime;
        transform.position = transform.position + Vector3.up * Mathf.Sin(time * Mathf.PI * 2 / period) * amplitude;
    }
}