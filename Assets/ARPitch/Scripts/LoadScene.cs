using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


// After introduction tutorial, chose which AR pitch to show.
public class LoadScene : MonoBehaviour
{

    public enum EnumScene { MAIN, PACO_RABANNE, CH ,KitKat};
    public EnumScene scene;
    private string[] sceneString = {"Main","Paco Rabanne", "CH", "KitKat"};
    public static LoadScene instance;


    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }



    public void LoadLevel()
	{
        SceneManager.LoadScene(sceneString[(int)scene]);
	}
}
