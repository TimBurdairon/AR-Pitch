using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleObject : MonoBehaviour 
{
    private float time = 0;


    // To modify
    private float timer = 0.35f;


    private Vector3 initScale;


    // To modify 
    private float percentageOfReduction = 5f;

    private Vector3 startScale = Vector3.zero;

    private Vector3 valueToIncrease;

   // public GameObject ObjectToScale;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) == true)
        {

            OnOpen();
        }

    }

    public  void Start()
    {
        initScale = transform.localScale;
        startScale = initScale / percentageOfReduction;
        valueToIncrease = startScale / 5f;
    }

    public void OnOpen()
    {
        time = 0;
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


}
