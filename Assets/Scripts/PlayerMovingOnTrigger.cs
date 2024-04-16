using UnityEngine;
using UnityEngine.Events;

public class PlayerMovingOnTrigger : MonoBehaviour
{
    [SerializeField] private UnityEvent PlayerMovingOnTriggerAction;

    public void OnTrigger()
    {
        PlayerMovingOnTriggerAction?.Invoke();
    }
}