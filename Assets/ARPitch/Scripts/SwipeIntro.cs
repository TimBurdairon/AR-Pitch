using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TouchScript.Gestures;
using UnityEngine.UI;
using TouchScript.Gestures.TransformGestures;

// Navigate through the different tutorial pages
public class SwipeIntro : MonoBehaviour 
{
    public Animator animator;
    private ScreenTransformGesture swipeTransformGesture;

    // Animations to play 
    private string[] animations = {"IntroAnimation", "IntroAnimation2", "IntroAnimation3"};

    public int currentAnimation = 0;

    //public Text debugText;
    private float swipeDirection;

    // Use this for initialization
    void Start()
    {
        swipeTransformGesture = GetComponent<ScreenTransformGesture>();
    }

	private void OnEnable()
	{
        Start();
        swipeTransformGesture.TransformCompleted += SwipeHandler;
        swipeTransformGesture.Transformed += DirectionSwipe;

	}

    private void OnDisable()
    {
        swipeTransformGesture.TransformCompleted -= SwipeHandler;
        swipeTransformGesture.Transformed -= DirectionSwipe;

    }

    private void DirectionSwipe(object sender, System.EventArgs e)
    {
        
        swipeDirection += swipeTransformGesture.DeltaPosition.x;
    }

	

    // Determine direction of the user swipes and set according tutorial page
    private void SwipeHandler(object sender, System.EventArgs e)
    {
        string animationName = "";

        // Determine direction of the user swipes 
        if (swipeDirection <= 0)
        {
            ++currentAnimation;
            if (currentAnimation < 3)
                animationName = animations[currentAnimation];
        }
        else 
        {
            --currentAnimation;
            if (currentAnimation < 0)
                currentAnimation = 0;
            animationName = animations[currentAnimation + 1] + "Inverse";
        }
       
        swipeDirection = 0;
        // Load the appropriate pitch scene
        if (currentAnimation == 3)
        {
            LoadScene.instance.LoadLevel();
            return;
        }

        animator.Play(animationName);
    }

  

}
