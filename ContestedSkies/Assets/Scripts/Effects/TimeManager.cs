using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public float slowdownFactor = 0.5f;
    public float slowdownLength = 1;

    private static TimeManager instance;

    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public static void StartSlowMotion()
    {
        instance.StartSlowMotionHelper();
    }

    private void StartSlowMotionHelper()
    {
        StopAllCoroutines();
        Time.timeScale = instance.slowdownFactor;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;

        AudioManager.SetPitch(slowdownFactor);
    }

    public static void ResumeTimeSmooth()
    {
        instance.StartCoroutine(instance.ResumeTimeSmoothHelper());
    }

    private IEnumerator ResumeTimeSmoothHelper()
    {
        while (Time.timeScale < 1)
        {
            Time.timeScale += (1 / slowdownLength) * Time.unscaledDeltaTime;
            yield return null;
        }
        Time.timeScale = 1;

        AudioManager.SetPitch(1);
    }

    public static void Restart()
    {
        instance.RestartHelper();
    }

    private void RestartHelper()
    {
        Time.timeScale = 1;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }
}
