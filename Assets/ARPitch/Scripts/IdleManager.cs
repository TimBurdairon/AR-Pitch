using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.iOS;



// Handle inactive objects, place a open button on top of target images 
public class IdleManager : MonoBehaviour 
{
    public Transform background;

    private ARReferenceImage referenceImage;

   
    private Vector2 imageSize;

    private Vector3 openOriginalButtonSize;
    private Vector3 ratioOpenButtonSize;

    private GameObject buttonOpen;

    public Vector2 offset;
    public Vector2 sizeDecreaseMultiplicater = new Vector2(1,1);


    // Resize the 3D ui button placed on top of the taget image, keep ration of button image
    public void Resize()
    {
        buttonOpen = background.GetChild(0).gameObject;

       
        // Determine if width or height is more dominent
        referenceImage = transform.parent.GetComponent<ARAnchor>().referenceImage;
        imageSize = new Vector2(0, 0);
        imageSize.x = referenceImage.physicalSize;
        imageSize.y = referenceImage.physicalSize * referenceImage.imageTexture.height / referenceImage.imageTexture.width;

        if (imageSize.x < imageSize.y)
            background.transform.localScale = new Vector3(imageSize.x, imageSize.x, 1) * 0.8f;
        else
            background.transform.localScale = new Vector3(imageSize.y, imageSize.y, 1) * 0.8f;


        buttonOpen.transform.localPosition = new Vector3(offset.x, offset.y, 0);
        buttonOpen.transform.localScale = new Vector3(buttonOpen.transform.localScale.x, buttonOpen.transform.localScale.y, buttonOpen.transform.localScale.z);
     
    }
	
	
}
