using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandSlow : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "ball")
        {
            Debug.Log("Entered Sand");
            PlayerManager.Instance.ApplySandDrag();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "ball")
        {
            PlayerManager.Instance.RemoveSandDrag();
        }
    }
}
