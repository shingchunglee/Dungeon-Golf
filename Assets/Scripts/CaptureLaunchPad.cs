using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptureLaunchPad : MonoBehaviour
{
    public float boostForce = 10f;
    public float waitTime = 0.75f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "ball")
        {
            float rotationZ = transform.rotation.eulerAngles.z;

            PlayerManager.Instance.KeepMovementTurnGoing(2);

            PlayerManager.Instance.ballRB.velocity = Vector2.zero;
            PlayerManager.Instance.ballRB.angularVelocity = 0;
            PlayerManager.Instance.varianceLevelController.selectedVariance = null;

            PlayerManager.Instance.ballRB.MovePosition(this.transform.position);

            StartCoroutine(HoldAndLaunch());

        }
    }

    private IEnumerator HoldAndLaunch()
    {
        float timeWaited = 0f;
        PlayerManager.Instance.ballRB.Sleep();

        while (timeWaited < waitTime)
        {
            yield return new WaitForFixedUpdate();
            timeWaited += Time.deltaTime;
        }
        PlayerManager.Instance.ballRB.WakeUp();

        PlayerManager.Instance.ballRB.AddForce(Vector2.left * boostForce, ForceMode2D.Impulse);
    }
}

