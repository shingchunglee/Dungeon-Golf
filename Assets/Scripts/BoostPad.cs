using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostPad : MonoBehaviour
{
    public float boostForce = 10f;
    public bool FixDirectionForTutorial = false;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "ball")
        {

            if (!FixDirectionForTutorial)
            {
                float rotationZ = transform.rotation.eulerAngles.z;

                // Boost ball in direction of boost pad
                Vector2 boostDirection = new Vector2(Mathf.Cos(rotationZ * Mathf.Deg2Rad), Mathf.Sin(rotationZ * Mathf.Deg2Rad));

                PlayerManager.Instance.ballRB.AddForce(boostDirection * boostForce, ForceMode2D.Impulse);
            }
            else
            {
                float rotationZ = transform.rotation.eulerAngles.z;

                //Reset the velocity before boosting
                PlayerManager.Instance.ballRB.velocity = Vector2.zero;
                PlayerManager.Instance.ballRB.angularVelocity = 0;
                PlayerManager.Instance.varianceLevelController.selectedVariance = 0;

                // Boost ball in direction of boost pad
                Vector2 boostDirection = Vector2.left;
                // PlayerManager.Instance.ballRB.Sleep();

                PlayerManager.Instance.ballRB.AddForce(boostDirection * boostForce, ForceMode2D.Impulse);
            }



        }


    }
}
