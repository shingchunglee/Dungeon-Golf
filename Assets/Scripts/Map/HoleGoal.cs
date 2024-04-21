using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleGoal : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "ball")
        {
            GameManager.Instance.AdvanceLevel();
        }
    }
}
