using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TouchScript.Gestures.TransformGestures;

using TouchScript.Gestures;
using UnityEngine.XR.iOS;


// Main class for interactions with  AR objects
public class ARObject : MonoBehaviour
{

    private float time = 0;
    private float timer = 0.35f;
    private Vector3 initScale;
    private float percentageOfReduction = 5f;
    private Vector3 startScale = Vector3.zero;
    private Vector3 valueToIncrease;


    // Pick/Drop object
    private float DropPickTimer = 0;
    private float DropPickFinalValue = 0.15f;

    // Increment vector for courountine 0f -> 0.15f
    private Vector3 IncrementDropPick;


    // Initial value to keep consistency between vertical and horizontal surfaces
    private Vector3 initPos;

    // Initial value to keep consistency between vertical and horizontal surfaces
    private Vector3 initialValuePressLongObject = Vector3.zero;

    // Initial value to keep consistency between vertical and horizontal surfaces
    private Vector3 initialRotationPressLongObject = Vector3.zero;

    // Temporary value to keep consistency between vertical and horizontal surfaces
    private Vector3 tmpRotationPressLongObject = Vector3.zero;



    private UnityARGeneratePlane grid;


    private Vector3 parentRotation;

    //Are all the initial values set
    private bool rotationCalibrated = false;

    // Is the user pressing two fingers on the iPad
    private bool twoFingerPressed = false;


    private bool currentGestureLocked = false;

    // Interaction classes from Touch Script, Inherited by ARModel/ARVideo classes
    protected PinnedTransformGesture rotationGesture;
    protected ScreenTransformGesture scaleGestureTwoFingers;
    protected TransformGesture transformGestureTwoFingers;
    protected LongPressGesture longPressGestureTwoFingers;
    protected ReleaseGesture releaseGestureTwoFingers;


    public static bool isMoving = false;


    // Initial values

    private Vector3 initialScale = Vector3.one;

    // Use this for initialization
    protected void Start()
    {
        grid = GameObject.Find("GeneratePlanes").GetComponent<UnityARGeneratePlane>();
        rotationGesture = GetComponent<PinnedTransformGesture>();
        scaleGestureTwoFingers = GetComponent<ScreenTransformGesture>();
        transformGestureTwoFingers = GetComponent<TransformGesture>();
        longPressGestureTwoFingers = GetComponent<LongPressGesture>();
        releaseGestureTwoFingers = GetComponent<ReleaseGesture>();

    }

    // Make sure the object are initialised
	private void OnEnable()
	{
        this.transform.localPosition = new Vector3(0, 0, 0);
        this.transform.localEulerAngles = new Vector3(0, 0, 0);
        this.transform.GetChild(0).localEulerAngles = new Vector3(0, 0, 0);
        this.transform.localScale = initialScale;
        rotationCalibrated = false;
	}

    // Hide Objects
	private void OnDisable()
	{
        List<ARPlaneAnchorGameObject> listARPlaneAnchor = grid.unityARAnchorManager.planeAnchorMapList;
        foreach (ARPlaneAnchorGameObject list in listARPlaneAnchor)
        {
            list.gameObject.SetActive(false);
        }
        twoFingerPressed = false;
        scaleGestureTwoFingers.enabled = true;
        isMoving = false;
	}

	public virtual void SetInitialValues(Vector3 scale, ARReferenceImage ar)
    {
        initialScale = scale; 
    }


    // Reset all
	public void Reset()
	{
        this.transform.localPosition = new Vector3(0, 0, 0);
        this.transform.localEulerAngles = new Vector3(0, 0, 0);
        this.transform.GetChild(0).localEulerAngles = new Vector3(0, 0, 0);
        this.transform.localScale = initialScale;
        if (transform.parent.eulerAngles == Vector3.zero)
            transform.parent.eulerAngles = parentRotation;
        rotationCalibrated = false;

	}

	#region debug

	#endregion

	#region Touch

	// Rotation of object
	protected void RotationHandler(object sender, System.EventArgs e)
    {
        this.transform.GetChild(0).localRotation *= Quaternion.AngleAxis(rotationGesture.DeltaRotation, rotationGesture.RotationAxis);
    }

    // Scale of object
    protected void ScaleHandler(object sender, System.EventArgs e)
    {
        if (twoFingerPressed == false)
        {
            // Max scale
            if (this.transform.localScale.x >= 8.00f && this.transform.localScale.y >= 8.00f && this.transform.localScale.z >= 8.00f) 
            {
                this.transform.localScale = new Vector3(8.0f, 8.0f, 8.0f); 
            }

            //Min scale
            if (this.transform.localScale.x <= 0.10f && this.transform.localScale.y <= 0.10f && this.transform.localScale.z <= 0.10f)
            {
                this.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            }
            this.transform.localScale *= scaleGestureTwoFingers.DeltaScale;
            currentGestureLocked = true;
        }
    }

    //Move object on ARkit plane
    protected void TransformHandler(object sender, System.EventArgs e)
    {
        if (twoFingerPressed == true)
        {
            Vector2 screenPositionTouch = new Vector2(transformGestureTwoFingers.ScreenPosition.x / Screen.width, transformGestureTwoFingers.ScreenPosition.y / Screen.height);
            Vector3 positionObject = HitPlaneDetection.PositionObjectOnPlane(screenPositionTouch);
            Vector3 rotationObject = HitPlaneDetection.PositionObjectOnPlaneRotation(screenPositionTouch);

            // If touch is on a 
            if (positionObject != Vector3.zero)
            {
                tmpRotationPressLongObject = rotationObject;
                rotationCalibrated = true;
               
                // Rotate object according to surface and initial rotation
                this.transform.localEulerAngles = initialRotationPressLongObject - rotationObject;
                this.transform.position = positionObject;
            }
        }
    }


    // Enable Move on the object
    protected  void LongPressHandler(object sender, System.EventArgs e)
    {
        if (currentGestureLocked == false)
        {
            twoFingerPressed = true;
            OnPick();
            parentRotation = transform.parent.eulerAngles;
            transform.parent.eulerAngles = Vector3.zero;
            List<ARPlaneAnchorGameObject> listARPlaneAnchor = grid.unityARAnchorManager.planeAnchorMapList;
            foreach (ARPlaneAnchorGameObject list in listARPlaneAnchor)
            {
                list.gameObject.SetActive(true);
            }

            Vector2 screenPositionOne = new Vector2(longPressGestureTwoFingers.ScreenPosition.x / Screen.width, longPressGestureTwoFingers.ScreenPosition.y / Screen.height);
            Vector3 rotationObject = HitPlaneDetection.PositionObjectOnPlaneRotation(screenPositionOne);


            if (rotationCalibrated == false)
            {
                initialRotationPressLongObject = rotationObject;
                rotationCalibrated = true;
            }

            if (initialRotationPressLongObject != rotationObject)
                initialRotationPressLongObject = tmpRotationPressLongObject;

            scaleGestureTwoFingers.enabled = false;
            isMoving = true;
        }
    }

    // Drop object when it was moving
    protected void ReleaseHandler(object sender, System.EventArgs e)
    {
        if (twoFingerPressed == true)
        {
            OnRelease();
            Vector2 screenPositionOne = new Vector2(longPressGestureTwoFingers.ScreenPosition.x / Screen.width, longPressGestureTwoFingers.ScreenPosition.y / Screen.height);
            Vector3 rotationObject = HitPlaneDetection.PositionObjectOnPlaneRotation(screenPositionOne);
            tmpRotationPressLongObject = initialRotationPressLongObject;
            List<ARPlaneAnchorGameObject> listARPlaneAnchor = grid.unityARAnchorManager.planeAnchorMapList;
            foreach (ARPlaneAnchorGameObject list in listARPlaneAnchor)
            {
                list.gameObject.SetActive(false);
            }
            scaleGestureTwoFingers.enabled = true;
            isMoving = false;
        }
        twoFingerPressed = false;
        currentGestureLocked = false;
    }


    #endregion


    protected void GenerateMeshCollider(Transform childTransform)
    {
        for (int counter = 0; counter < childTransform.transform.childCount; counter++)
        {
            if (childTransform.GetChild(counter).GetComponent<MeshRenderer>() != null &&
                childTransform.transform.GetChild(counter).GetComponent<MeshFilter>() != null &&
                childTransform.transform.GetChild(counter).GetComponent<MeshCollider>() == null)

            {
                MeshCollider childCollider = childTransform.transform.GetChild(counter).gameObject.AddComponent<MeshCollider>();
                childCollider.sharedMesh = childTransform.transform.GetChild(counter).GetComponent<MeshFilter>().mesh;
                childCollider.convex = true;
            }
            if (childTransform.transform.GetChild(counter).transform.childCount > 0)
            {
                GenerateMeshCollider(childTransform.transform.GetChild(counter).transform);
                return;
            }
        }
    }

    protected void GenerateRigidBody(Transform childTransform)
    {
        for (int counter = 0; counter < childTransform.transform.childCount; counter++)
        {
            if (childTransform.GetChild(counter).GetComponent<MeshRenderer>() != null &&
                childTransform.transform.GetChild(counter).GetComponent<MeshFilter>() != null &&
                childTransform.transform.GetChild(counter).GetComponent<MeshCollider>() != null &&
                childTransform.transform.GetChild(counter).GetComponent<Rigidbody>() == null)
            {
                Rigidbody childRigidbody = childTransform.transform.GetChild(counter).gameObject.AddComponent<Rigidbody>();
                childRigidbody.isKinematic = true;
                childRigidbody.useGravity = false;
                childRigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            }
            if (childTransform.transform.GetChild(counter).transform.childCount > 0)
            {
                GenerateRigidBody(childTransform.transform.GetChild(counter).transform);
                return;
            }
        }
    }

    public void OnOpen()
    {
        time = 0;
        initScale = transform.localScale;
        startScale = initScale / percentageOfReduction;
        valueToIncrease = startScale / 5f;
        transform.localScale = startScale;
        StartCoroutine(ScaleTimer());
    }

    private IEnumerator ScaleTimer()
    {
        while (time < timer)
        {
            time += Time.deltaTime;
            transform.localScale += valueToIncrease;
            Debug.Log(transform.localScale);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        transform.localScale = initScale;
    }


   

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) == true)
        {
            OnPick();
        }

        if (Input.GetMouseButtonUp(0) == true)
        {
            OnRelease();
        }
    }


    // Lift object when two fingers are pressed on the iPad
    public void OnPick()
    {
        DropPickTimer = 0;
        initPos = transform.GetChild(0).localPosition;
        IncrementDropPick = new Vector3(0, .01f, 0);
        StartCoroutine(PosTimer());
    }

    // Drop object when two fingers are released
    public void OnRelease()
    {
        DropPickTimer = 0;
        IncrementDropPick = new Vector3(0, .013f, 0);
        StartCoroutine(PosTimerMinus());
    }

    private IEnumerator PosTimer()
    {
        while (DropPickTimer < 0.15f)
        {
            DropPickTimer += Time.deltaTime;
            transform.GetChild(0).localPosition += IncrementDropPick;
            Debug.Log(transform.localPosition);
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    private IEnumerator PosTimerMinus()
    {
        while (DropPickTimer < 0.1f)
        {
            DropPickTimer += Time.deltaTime;
            transform.GetChild(0).localPosition -= IncrementDropPick;
            Debug.Log(transform.localPosition);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        transform.GetChild(0).localPosition = new Vector3(0,0,0);
    }

}
