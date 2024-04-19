
using Common;
using UnityEngine;

public class GolfAimDrag : MonoBehaviour
{
    // public Transform aimingIndicator;
    public GameObject interpolateIndicator;
    public float variance = 0;
    public Vector3 variancePosition;
    public float time = 0;
    private float interpolateAmount;
    public Vector3? aimDirection;
    public float maxDistance = 3f;
    public Vector3 dragStartPos;
    public Vector3 dragEndPos;
    public bool isDragging = false;

    void OnEnable()
    {
        // aimingIndicator.gameObject.SetActive(true);
        aimDirection = null;
    }

    void OnDisable()
    {
        // aimingIndicator.gameObject.SetActive(false);
    }

    void Update()
    {
        if (isDragging)
        {
            UpdateDirection();
            UpdatePower();
            UpdateVariance();
            UpdateAimIndicator();
        }
    }

    private void UpdatePower()
    {
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        float mouseDistance = Mathf.Clamp(Mathf.Abs(Vector3.Distance(mousePos, dragStartPos)), 0, maxDistance);
        float normalizedDistance = mouseDistance / maxDistance;
        float power = (PlayerManager.Instance.inventoryController.GetSelectedClub().maxPower - PlayerManager.Instance.inventoryController.GetSelectedClub().minPower) * normalizedDistance + PlayerManager.Instance.inventoryController.GetSelectedClub().minPower;
        PlayerManager.Instance.powerLevelController.SetPower(power);
    }

    private void UpdateVariance(float duration = 1f)
    {
        time += Time.deltaTime;
        float delta = Mathf.Clamp01(time / duration);
        float t = Easing.Factory(EasingFunction.LinearInverse)(delta);

        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        float dragAmount = Mathf.Clamp(Mathf.Abs(Vector3.Distance(mousePos, dragStartPos)), 0, maxDistance);

        float value = Mathf.Lerp(-dragAmount, dragAmount, t);
        variance = value;

        if (delta >= 0.99f)
        {
            time = 0;
        }
    }

    private void UpdateDirection()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        aimDirection = dragStartPos - mousePos;
        aimDirection = aimDirection?.normalized;
    }

    private void UpdateDirectionByVariance()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        aimDirection = variancePosition - transform.position;
        aimDirection = aimDirection?.normalized;
    }

    private void UpdateAimIndicator()
    {
        GameObject[] gos = GameObject.FindGameObjectsWithTag("aimingIndicator");
        foreach (GameObject go in gos)
            Destroy(go);
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        float dragAmount = Mathf.Clamp(Mathf.Abs(Vector3.Distance(mousePos, dragStartPos)), 0, maxDistance);
        Vector3 aimingPosition = transform.position - (Vector3)(-aimDirection * dragAmount);
        Vector3 perpendicular = Vector3.Cross(aimingPosition - transform.position, Vector3.forward).normalized;
        variancePosition = aimingPosition + (perpendicular * variance);

        for (float i = 0f; i < 1f; i += 0.2f)
        {
            Vector3 ballAim = Vector3.Lerp(transform.position, aimingPosition, i);
            Instantiate(interpolateIndicator).transform.position = Vector3.Lerp(ballAim, variancePosition, i);
        }
    }

    public void OnMouseDown()
    {
        dragStartPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        dragStartPos.z = 0;
        // aimingIndicator.position = dragStartPos;
        isDragging = true;
    }

    public void OnMouseUp()
    {
        UpdateDirection();
        UpdateDirectionByVariance();
        GameObject[] gos = GameObject.FindGameObjectsWithTag("aimingIndicator");
        foreach (GameObject go in gos)
            Destroy(go);
        dragEndPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        dragEndPos.z = 0;
        isDragging = false;
    }
}