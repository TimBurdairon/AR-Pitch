using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.iOS;

// ARkit basic Hit Plane Detection Script
public class HitPlaneDetection : MonoBehaviour 
{
        
        static private Transform m_HitTransform;
        public LayerMask collisionLayer = 1 << 10;  //ARKitPlane layer


	static Vector3 HitTestWithResultType (ARPoint point, ARHitTestResultType resultTypes)
        {
            List<ARHitTestResult> hitResults = UnityARSessionNativeInterface.GetARSessionNativeInterface ().HitTest (point, resultTypes);
            if (hitResults.Count > 0) 
            {
                foreach (var hitResult in hitResults) 
                {
                //Transform t = new Transform();
                   Vector3 position = UnityARMatrixOps.GetPosition (hitResult.worldTransform);
                  //  m_HitTransform.rotation = UnityARMatrixOps.GetRotation (hitResult.worldTransform);
                    return position;
                }
            }
            return Vector3.zero;
        }
        
       //Return the position of the object
       static public Vector3 PositionObjectOnPlane(Vector2 FingerscreenPosition) 
        {
          
                ARPoint point = new ARPoint {
                    x = FingerscreenPosition.x,
                    y = FingerscreenPosition.y
                   };

                    // prioritize reults types
                    ARHitTestResultType[] resultTypes = 
                {
                        //ARHitTestResultType.ARHitTestResultTypeExistingPlaneUsingGeometry,
                        ARHitTestResultType.ARHitTestResultTypeExistingPlaneUsingExtent, 
                        // if you want to use infinite planes use this:
                        //ARHitTestResultType.ARHitTestResultTypeExistingPlane,
                        //ARHitTestResultType.ARHitTestResultTypeEstimatedHorizontalPlane, 
                        //ARHitTestResultType.ARHitTestResultTypeEstimatedVerticalPlane, 
                        //ARHitTestResultType.ARHitTestResultTypeFeaturePoint
                    };

                foreach (ARHitTestResultType resultType in resultTypes)
                {
                    return HitTestWithResultType(point, resultType);
                }
                            
                return Vector3.zero;
         }


    // Determine rotation of the hit result
    static Vector3 HitTestWithResultTypeRotation(ARPoint point, ARHitTestResultType resultTypes)
    {
        List<ARHitTestResult> hitResults = UnityARSessionNativeInterface.GetARSessionNativeInterface().HitTest(point, resultTypes);
        if (hitResults.Count > 0)
        {
            foreach (var hitResult in hitResults)
            {
              
                Vector3 rotation = UnityARMatrixOps.GetRotation (hitResult.worldTransform).eulerAngles;
                return rotation;
            }
        }
        return Vector3.zero;
    }


    //Return the position of the object
    static public Vector3 PositionObjectOnPlaneRotation(Vector2 FingerscreenPosition)
    {

        ARPoint point = new ARPoint
        {
            x = FingerscreenPosition.x,
            y = FingerscreenPosition.y
        };

        // prioritize reults types
        ARHitTestResultType[] resultTypes =
    {
                        //ARHitTestResultType.ARHitTestResultTypeExistingPlaneUsingGeometry,
                        ARHitTestResultType.ARHitTestResultTypeExistingPlaneUsingExtent, 
                        // if you want to use infinite planes use this:
                        //ARHitTestResultType.ARHitTestResultTypeExistingPlane,
                        //ARHitTestResultType.ARHitTestResultTypeEstimatedHorizontalPlane, 
                        //ARHitTestResultType.ARHitTestResultTypeEstimatedVerticalPlane, 
                        //ARHitTestResultType.ARHitTestResultTypeFeaturePoint
                    };

        foreach (ARHitTestResultType resultType in resultTypes)
        {
            return HitTestWithResultTypeRotation(point, resultType);
        }

        return Vector3.zero;
    }
            

}
