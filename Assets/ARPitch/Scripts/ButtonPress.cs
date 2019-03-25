using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Feedback when you press the button,
public class ButtonPress : MonoBehaviour
{
    public float amplitudeUp = 0.3f;
    public float timerValue = 2f;
    private float  timer = 0;

    public float initialValueUp;
	// Use this for initialization
	void Start () 
    {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnButtonPress(Transform transformButton)
    {
        timer = 0;
        initialValueUp = transformButton.position.y;
        StartCoroutine(PressEffect(transformButton));

    }


    private IEnumerator PressEffect(Transform transformButton)
    {
        while (timer < timerValue)
        {
            timer += Time.deltaTime;
            if (timer < (timerValue / 2))
            {
                transformButton.position.Set(transformButton.position.x, transformButton.position.y - amplitudeUp ,transformButton.position.z);
            }
            else
            {
                transformButton.position.Set(transformButton.position.x, transformButton.position.y + amplitudeUp, transformButton.position.z);
            }
          
            yield return null;
        }
        transformButton.position.Set(transformButton.position.x, initialValueUp, transformButton.position.z);
    }
}
