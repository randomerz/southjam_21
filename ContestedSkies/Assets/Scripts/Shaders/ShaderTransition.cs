using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BadShaderEffect))]
public class ShaderTransition : MonoBehaviour
{
    public BadShaderEffect shader;
    public float transitionLength = 1f;

    void Awake()
    {
        shader.intensity = 1;
        StartCoroutine(StartTransition());
    }
    
    private IEnumerator StartTransition()
    {
        float time = 0;

        while (time < transitionLength)
        {
            time += Time.deltaTime;

            shader.intensity = 1 - (time / transitionLength);

            yield return null;
        }

        shader.intensity = 0;
    }
}
