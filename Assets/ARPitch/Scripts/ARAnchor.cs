using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.iOS;
using UnityEngine.UI;

public class ARAnchor : MonoBehaviour
{
    [SerializeField]
    public ARReferenceImage referenceImage;
    // Use this for initialization


    // Model, Sound or Video Objects
    public GameObject augmentedObjectActive;

    // Idle GameObject
    public GameObject augmentedObjectIdle;


    // Detect Image Canvas
    public static bool CanvasDeactivated = false;

    private GameObject imageAnchorGO;
   
    [HideInInspector]
    public bool isActive = false;

    // TO DELETE
    //public Text debugText;

    #region ARkit

    protected void Start()
    {
        
        UnityARSessionNativeInterface.ARImageAnchorAddedEvent += AddImageAnchor;
        UnityARSessionNativeInterface.ARImageAnchorUpdatedEvent += UpdateImageAnchor;
        UnityARSessionNativeInterface.ARImageAnchorRemovedEvent += RemoveImageAnchor;
       
        augmentedObjectActive.SetActive(false);
        augmentedObjectIdle.SetActive(false);
    }


    void AddImageAnchor(ARImageAnchor arImageAnchor)
    {
        Debug.Log("image anchor added");

        if (arImageAnchor.referenceImageName == referenceImage.imageName)
        {
            Vector3 position = UnityARMatrixOps.GetPosition(arImageAnchor.transform);
            Quaternion rotation = UnityARMatrixOps.GetRotation(arImageAnchor.transform);
            isActive = true;

            if (CanvasDeactivated == false)
            {
                GameObject.Find("ScanArea").SetActive(false);
                CanvasDeactivated = true;
            }
             augmentedObjectIdle.SetActive(true);

            // Set initial values for Anchor
            string imageAnchorName = augmentedObjectActive.name + "Anchor";
            imageAnchorGO = new GameObject(imageAnchorName);
            imageAnchorGO.transform.localScale = new Vector3(1, 1, 1);
            imageAnchorGO.transform.position = position;
            imageAnchorGO.transform.rotation = rotation;
           

            // Reset the "active" object to initial position
            augmentedObjectActive.transform.SetParent(imageAnchorGO.transform);
            augmentedObjectActive.transform.localPosition = new Vector3(0, 0, 0);
            augmentedObjectActive.transform.localEulerAngles = new Vector3(0, 0, 0);
            augmentedObjectActive.GetComponent<ARObject>().SetInitialValues(augmentedObjectActive.transform.localScale, referenceImage);

            //Reset "Idle" object to initial position
            augmentedObjectIdle.GetComponent<IdleManager>().Resize();
            augmentedObjectIdle.transform.SetParent(imageAnchorGO.transform);
            augmentedObjectIdle.transform.localPosition = new Vector3(0, 0, 0);
            augmentedObjectIdle.transform.localEulerAngles = new Vector3(0, 0, 0);

        }
    }

    void UpdateImageAnchor(ARImageAnchor arImageAnchor)
    {

        if (arImageAnchor.referenceImageName == referenceImage.imageName)
        {
            if (augmentedObjectActive.activeInHierarchy == false)
            {
                imageAnchorGO.transform.position = UnityARMatrixOps.GetPosition(arImageAnchor.transform);
                imageAnchorGO.transform.rotation = UnityARMatrixOps.GetRotation(arImageAnchor.transform);
            }
        }

    }

    void RemoveImageAnchor(ARImageAnchor arImageAnchor)
    {
       
        if (imageAnchorGO)
        {
            GameObject.Destroy(imageAnchorGO);
        }

    }

    void OnDestroy()
    {
        UnityARSessionNativeInterface.ARImageAnchorAddedEvent -= AddImageAnchor;
        UnityARSessionNativeInterface.ARImageAnchorUpdatedEvent -= UpdateImageAnchor;
        UnityARSessionNativeInterface.ARImageAnchorRemovedEvent -= RemoveImageAnchor;
    }

    #endregion
}
