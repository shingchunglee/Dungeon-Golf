using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class NewBehaviourScript : MonoBehaviour
{
    public int Par;
    public TextMeshProUGUI parText;

    void Update()
    {
         parText.text = Par.ToString()+"/6";
    }

    public void oneStroke()
    {
        Par--;
        updateParText();
    }

    void updateParText()
    {
         parText.text = Par.ToString()+"/6";
    }
}
