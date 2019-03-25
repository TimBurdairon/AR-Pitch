using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TouchScript.Gestures.TransformGestures;


// Inherit interactions from ARObject class
public class ARModel : ARObject
{
      
     void Start()
    {
        base.Start();
       // OnEnable();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnEnable()
    {
        Start();
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
