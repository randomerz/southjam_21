using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelComplete : MonoBehaviour
{
    public string dest;
    public Image whitePanel;
    public float transitionOutDuration = 2f;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            StartCoroutine(TransitionSceneOut());
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
        SceneManager.LoadScene(dest);
    }
}
