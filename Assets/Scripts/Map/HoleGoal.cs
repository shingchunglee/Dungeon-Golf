using UnityEngine;
using UnityEngine.SceneManagement;

public class HoleGoal : MonoBehaviour
{
    public string nextSceneName;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag != "ball") return;

        if (nextSceneName == "")
        {
            GameManager.Instance.AdvanceLevel();
        }
        else
        {   
            
            SceneManager.LoadScene(nextSceneName);
        }
    }

    public void GoalSpawnInit()
    {
        Vector2 goalSpawn = GameManager.Instance.proceduralGenerationPresets[GameManager.Instance.procGenLevelIndex].GoalSpawn;
        transform.position = new Vector3(goalSpawn.x, goalSpawn.y, transform.position.z);
    }
}
