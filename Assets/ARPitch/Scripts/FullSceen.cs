using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Video;
//using UnityEngine.


// Handle Video in Fullscreen
public class FullSceen : MonoBehaviour
{
    public Button play;
    public Button minimize;

    public Text currentTimerText;
    public Text TimeLeftText;

    public GameObject Background;


    public VideoPlayer videoPlayerFullScreen;
   // public EventTrigger trigger;

    public Slider slider;

    public bool getNewValue = false;


    public bool isPlay = false;

    private bool entered = false;

    private bool done = false;

    public Material pauseMaterial;
    public Material playMaterial;


    public static GameObject fullScreen;
    public static GameObject backGroundImage;

    //public GameObject scanGameObject;

    public Animator animatorButton;
    //public Animator animatorFullScreen;

    // Buttons Hide
    public bool buttonsShown = false;
    private float timer = 0f;

    public float valueSlider;

    public void UpdateTimer()
    {
        getNewValue = true;
        valueSlider = slider.value;
    }
    public void FinishUpdateTimer()
    {
        getNewValue = false;

    }

    // Use this for initialization
    void Start()
    {
        // minimize.;
    }

    private void OnEnable()
    {
        if (!animatorButton.isInitialized)
        {
            Debug.Log("good boy");
            animatorButton.Rebind();
        }

        //OnTouchScreen();
    }
    // Update is called once per frame
    void Update()
    {


        if (entered == true)
        {
            timer = 0;
            Debug.Log("Down2");
        }
        
    }


    public void PlayPause()
    {
        isPlay = !isPlay;
        if (isPlay == true)
        {
            videoPlayerFullScreen.Play();
            play.GetComponent<RawImage>().texture = pauseMaterial.mainTexture;
        }

        else
        {
            videoPlayerFullScreen.Pause();
            play.GetComponent<RawImage>().texture = playMaterial.mainTexture; 
        }
    }

	public void OnDisable()
	{
       // animatorButton.re
        animatorButton.Rebind();
        Debug.Log("qweqwe");
        //animatorButton.gameObject.GetComponent<RectTransform>().anchoredPosition3D  = new Vector3(0, -700, 0);
        //animatorButton.gameObject.SetActive(false);
	}

	public void Entered()
    {
        entered = true;
    }

    public void Exited()
    {
        entered = false;

    }



	public void OnTouchScreen()
    {
        Debug.Log("Down");
        timer = 0;
        if (buttonsShown == false)
        {
            buttonsShown = true;
            animatorButton.SetBool("Show", true);
            timer = 0;
            StartCoroutine(ButtonCountdown());
        }
       
    }

    private IEnumerator ButtonCountdown()
    {
        while (timer < 1.2f)
        {
            timer += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        buttonsShown = false;
        animatorButton.SetBool("Show", false);
    }

}
