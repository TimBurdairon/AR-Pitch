using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroManager : MonoBehaviour {

    public Button m_YourButton;

    // Use this for initialization
    void Start () 
    {
        Button btn = m_YourButton.GetComponent<Button>();
        btn.onClick.AddListener(NextOnClick);
    }

    void NextOnClick()
    {
        Debug.Log("You have clicked the button!");
    }
}
