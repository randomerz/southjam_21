using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperTimer : MonoBehaviour
{
    public float timeLimit;
    private float currentTime;

    private bool isActive = false;

    // audio
    private int currentBeep;
    private float timeBetweenBeep = 0.75f;

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

            CheckBeep();

            if (paperSpotlight != null)
                paperSpotlight.transform.position = paper.transform.position;

            //Debug.Log(currentTime);
            if (currentTime >= timeLimit)
            {
                Debug.Log("Game over!");
                GameHandler.GameOver();
                SetActive(false);
            }
        }
    }

    private void CheckBeep()
    {
        float nextBeepTime = timeLimit - ((3 - currentBeep) * timeBetweenBeep);
        if (currentTime > nextBeepTime)
        {
            AudioManager.Play("Paper Beep " + (currentBeep + 1));
            currentBeep += 1;
        }
    }

    public void SetActive(bool value)
    {
        // set active
        if (!isActive && value)
        {
            isActive = true;
            currentTime = 0;
            currentBeep = 0;
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
