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

    public void GoalSpawnInit()
    {
        Vector2 goalSpawn = GameManager.Instance.proceduralGenerationPresets[GameManager.Instance.procGenLevelIndex].GoalSpawn;
        transform.position = new Vector3(goalSpawn.x, goalSpawn.y, transform.position.z);
    }
}
