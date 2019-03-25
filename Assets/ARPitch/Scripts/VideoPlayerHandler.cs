using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.iOS;
using UnityEngine.Video;

using UnityEngine.UI;

public class VideoPlayerHandler : MonoBehaviour 
{



    //public GameObject backGround;
    public bool isPortrait = false; 
    public VideoPlayer videoPlayer;

    // Show current play time of the video
    private float currentTimer;

    // Current time left until the video is finished
    private float timeLeft;


    private GameObject fullScreenGameObject = null;

    // UI Material Play
    public Material playMaterial;

    // UI Material Pause
    public Material pauseMaterial;

    public Animator animatorButton;

    // Full Screen
    private bool fullScreen = false;

    // Scale Video to Fullscreen
    private Vector3 scaleBeforeFullScreen;

    //Get initial value of video posiiton
    private Vector3 positionBeforeFullScreen;

    //Get initial value of video rotation
    private Vector3 rotationBeforeFullScreen;



    // Keep track of button visibility
    private bool buttonsShown = false;

    // Timer for utility canvas to disapear
    private float timerCanvas = 0f;

    private bool playVideo = false;     
    private Vector2 imageSize;
    private float ratioVideo;
    private float ratioImage;

	

    public void PlayPauseVideoFullScreen()
    {
        playVideo = !playVideo;
    }

	private void OnEnable()
	{
        // Set video FullScreen
        Camera.main.clearFlags = CameraClearFlags.Color;
        fullScreenGameObject = FullSceen.fullScreen;

        //Assign current AR touchpoint videoPlayer to full screen
        fullScreenGameObject.GetComponent<FullSceen>().videoPlayerFullScreen = videoPlayer;


        fullScreenGameObject.SetActive(true);

        // Set Pause/Play button to the pause material UI
        fullScreenGameObject.GetComponent<FullSceen>().play.GetComponent<RawImage>().texture = pauseMaterial.mainTexture;

        Camera.main.gameObject.transform.GetChild(0).gameObject.SetActive(true);

        playVideo = false;
        fullScreen = true;
      
        //Enable the user to change the current time of the video.
        SetUpTimeSlider();

        FullSceen fullScreenScript = fullScreenGameObject.GetComponent<FullSceen>();
        fullScreenScript.buttonsShown = false;
        fullScreenScript.OnTouchScreen();
        fullScreenScript.isPlay = false;
        fullScreenScript.PlayPause();

	}

    private void OnDisable()
    {       
        fullScreen = false;
        if (Camera.main.gameObject.transform.GetChild(0).gameObject)
            Camera.main.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        fullScreenGameObject.GetComponent<FullSceen>().play.GetComponent<RawImage>().texture = pauseMaterial.mainTexture;
        fullScreenGameObject.SetActive(false);
     
        if (videoPlayer)
        {
            videoPlayer.Stop();
        }

    }

	/*public void OnTouchScreen()
    {
        timer = 0;
        Debug.Log("ChangedTimer");
        if (buttonsShown == false)
        {
            buttonsShown = true;
            animatorButton.SetBool("Show", true);
            timer = 0;
            StartCoroutine(ButtonCountdown());
        }
       
    }*/

    private string ConvertFloatToStringTimer(float value)
    {
        string minSec = string.Format("{0}:{1:00}", (int)value / 60, (int)value % 60);
        return minSec;
    }

  
    // On time slider value change change the video 
    public void ModifyCurrentTime(Slider slider)
    {
        currentTimer = slider.value;
    }

    public void SetUpTimeSlider()
    {
        timeLeft = (float)videoPlayer.clip.length;
        fullScreenGameObject.GetComponent<FullSceen>().slider.minValue = 0;
        fullScreenGameObject.GetComponent<FullSceen>().currentTimerText.text = ConvertFloatToStringTimer(0);
        fullScreenGameObject.GetComponent<FullSceen>().slider.maxValue = timeLeft;
        fullScreenGameObject.GetComponent<FullSceen>().TimeLeftText.text = ConvertFloatToStringTimer(timeLeft);
        fullScreenGameObject.GetComponent<FullSceen>().slider.value = 0;

    }



    //Update video and UI when time silder value is modified
    void Update()
    {

        currentTimer = (float)videoPlayer.time;
        if (fullScreenGameObject.GetComponent<FullSceen>().getNewValue == true)
        {
            currentTimer = fullScreenGameObject.GetComponent<FullSceen>().slider.value;
            videoPlayer.time = fullScreenGameObject.GetComponent<FullSceen>().slider.value;

            //Time Left
            timeLeft = (float)videoPlayer.clip.length - (float)videoPlayer.time;   
            fullScreenGameObject.GetComponent<FullSceen>().TimeLeftText.text = ConvertFloatToStringTimer(timeLeft);

            //Current Time;
            fullScreenGameObject.GetComponent<FullSceen>().currentTimerText.text = ConvertFloatToStringTimer(currentTimer);
        }
        else
        {
            // Value
            fullScreenGameObject.GetComponent<FullSceen>().slider.value = currentTimer;

            //Time Left
            timeLeft = (float)videoPlayer.clip.length - (float)videoPlayer.time;
            fullScreenGameObject.GetComponent<FullSceen>().TimeLeftText.text = ConvertFloatToStringTimer(timeLeft);

            //Current Time;
            fullScreenGameObject.GetComponent<FullSceen>().currentTimerText.text = ConvertFloatToStringTimer(currentTimer);
        }

       
       
    }

    // Hide silder after 5 second if there were no interaction with the screen
    private IEnumerator ButtonCountdown()
    {
        while (timerCanvas < 5f)
        {
            timerCanvas += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        buttonsShown = false;
        animatorButton.SetBool("Show", false);
    }

    public void KeepRatioSet(ARReferenceImage referenceImage)
    {
        
       
    }

	
	
}
