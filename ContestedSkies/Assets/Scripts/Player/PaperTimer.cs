using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperTimer : MonoBehaviour
{
    public float timeLimit;
    private float currentTime;

    private bool isActive = false;

    [Header("References")]
    public GameObject player;
    private GameObject paper;
    private GameObject paperSpotlight;

    void Start()
    {
        
    }
    
    void FixedUpdate()
    {
        if (isActive)
        {
            currentTime += Time.deltaTime;

            if (paperSpotlight != null)
                paperSpotlight.transform.position = paper.transform.position;

            Debug.Log(currentTime);
            if (currentTime >= timeLimit)
            {
                Debug.Log("Game over!");
            }
        }
    }

    public void SetActive(bool value)
    {
        // set active
        if (!isActive && value)
        {
            isActive = true;
            currentTime = 0;
        }
        else if (!value)
        {
            isActive = false;
            currentTime = 0;
        }
    }

    public void SetPaper(GameObject paperObj, GameObject paperSpotlightObj=null)
    {
        paper = paperObj;
        paperSpotlight = paperSpotlightObj;
    }
}
