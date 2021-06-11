using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PrologueManager : MonoBehaviour
{
    public TextMeshProUGUI[] textLines;
    public float timeBetweenReveals = 2;
    public float fadeDuration = 0.5f;
    
    private bool finishedReveal = false;
    public Image whitePanel;
    public float transitionOutDuration = 2f;

    void Start()
    {
        SetTextAlpha(0);
        StartCoroutine(PassageRevealer());
    }

    private void Update()
    {
        // anyKeyDown
        if (Input.anyKeyDown)
        {
            if (!finishedReveal)
            {
                StopAllCoroutines();
                SetTextAlpha(1);
                finishedReveal = true;
            }
            else
            {
                StartCoroutine(TransitionSceneOut());
            }
        }
    }

    private IEnumerator PassageRevealer()
    {
        int i = 0;
        
        while (i < textLines.Length)
        {
            StartCoroutine(TextRevealer(textLines[i]));
            i++;
            
            if (i + 1 == textLines.Length) // delay extra for last [ press any key ]
            {
                yield return new WaitForSeconds(timeBetweenReveals);
            }
            if (i < textLines.Length) // don't delay after finished end
            {
                yield return new WaitForSeconds(timeBetweenReveals);
            }
        }

        finishedReveal = true;
    }

    private IEnumerator TextRevealer(TextMeshProUGUI text)
    {
        float time = 0;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;

            Color c = text.color;
            c.a = time / fadeDuration;
            text.color = c;

            yield return null;
        }

        Color col = text.color;
        col.a = 1;
        text.color = col;
    }

    private void SetTextAlpha(float alpha)
    {
        for (int i = 0; i < textLines.Length; i++)
        {
            Color c = textLines[i].color;
            c.a = alpha;
            textLines[i].color = c;
        }
    }

    private IEnumerator TransitionSceneOut()
    {
        float time = 0;

        while (time < transitionOutDuration)
        {
            time += Time.deltaTime;

            Color c = whitePanel.color;
            c.a = time / transitionOutDuration;
            whitePanel.color = c;

            yield return null;
        }

        // change scene
        GameHandler.SetTotalStartingTime();
        SceneManager.LoadScene("Game");
    }
}
