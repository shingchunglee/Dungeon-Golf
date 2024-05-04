using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ManagerInitializer : MonoBehaviour
{
    // Start is called before the first frame update
    private void Awake()
    {
        GameManager.Instance.InitializeThisFromOtherScript();
    }

}
