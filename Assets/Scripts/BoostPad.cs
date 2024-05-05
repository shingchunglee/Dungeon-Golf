using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostPad : MonoBehaviour
{
    public float boostForce = 10f;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "ball")
        {

            float rotationZ = transform.rotation.eulerAngles.z;

            // Boost ball in direction of boost pad
            Vector2 boostDirection = new Vector2(Mathf.Cos(rotationZ * Mathf.Deg2Rad), Mathf.Sin(rotationZ * Mathf.Deg2Rad));

            PlayerManager.Instance.ballRB.AddForce(boostDirection * boostForce, ForceMode2D.Impulse);

        }
    }
}
