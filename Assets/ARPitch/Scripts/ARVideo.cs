using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;



public class ARVideo : ARObject
{
    // Script Manager for playing video full screen
    public VideoPlayerHandler videoPlayerHandler;


    void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Set Ration of the video Player
    public override void SetInitialValues(Vector3 scale, ARReferenceImage referenceImage)
    {
        videoPlayerHandler.KeepRatioSet(referenceImage);
    }

    private void OnEnable()
    {
        Start();
        //base.Start();
        rotationGesture.Transformed += RotationHandler;
        scaleGestureTwoFingers.Transformed += ScaleHandler;
        transformGestureTwoFingers.Transformed += TransformHandler;
        longPressGestureTwoFingers.LongPressed += LongPressHandler;
        releaseGestureTwoFingers.Released += ReleaseHandler;
    }

    private void OnDisable()
    {
        rotationGesture.Transformed -= RotationHandler;
        scaleGestureTwoFingers.Transformed -= ScaleHandler;
        transformGestureTwoFingers.Transformed -= TransformHandler;
        longPressGestureTwoFingers.LongPressed -= LongPressHandler;
        releaseGestureTwoFingers.Released -= ReleaseHandler;
    }


}
