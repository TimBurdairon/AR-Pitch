using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



// Determine which object is currently selected
public class ObjectSelectionManager : MonoBehaviour 
{
    private List<ARAnchor> listArAnchor;
    private readonly ARAnchor previousARAcnhor;
    private readonly bool done = false;

    public GameObject fullScreen;
    public GameObject backGround;
    public Button closeButton;

 
	void Start () 
    {
        listArAnchor = new List<ARAnchor>();
        for (int inc = 0; inc < transform.childCount; inc++ )
        {
            listArAnchor.Add(transform.GetChild(inc).GetComponent<ARAnchor>());

        }
        FullSceen.fullScreen = fullScreen;
        FullSceen.backGroundImage = backGround;
        closeButton.gameObject.SetActive(false);
	}

    // Close all of opened medias
    public void CloseAllMedia()
    {
        foreach (ARAnchor ArAnchor in listArAnchor)
        {
            ArAnchor.augmentedObjectActive.SetActive(false);
            if (ArAnchor.isActive == true)
                ArAnchor.augmentedObjectIdle.SetActive(true);
            ArAnchor.augmentedObjectActive.GetComponent<ARObject>().Reset();
        }
        closeButton.gameObject.SetActive(false);
    }
	
    // When Open UI button is pressed (the one over each target image), this function is called
    // Enable the selected object
    public void SelectObject(ARAnchor ARAnchorSender)
    {
        foreach(ARAnchor ArAnchor in listArAnchor)
        {
            ArAnchor.augmentedObjectActive.SetActive(false);
            ArAnchor.augmentedObjectIdle.SetActive(false);
            ArAnchor.augmentedObjectActive.GetComponent<ARObject>().Reset();
        }
     
        ARAnchorSender.augmentedObjectActive.SetActive(true);
        ARAnchorSender.augmentedObjectIdle.SetActive(false);
        closeButton.gameObject.SetActive(true);
    }


}
