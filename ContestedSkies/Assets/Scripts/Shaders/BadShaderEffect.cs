using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Creates the material when needed
//   works when you put on camera obj
[ExecuteInEditMode]
public class BadShaderEffect : MonoBehaviour
{
    [Range(0f, 1f)]
    public float intensity;
    public Color tintColor;
    public bool doLerp;

    public float spotlightRadius;
    public Vector3 playerPosition;
    public Vector3 targetPosition;

    private Material material;

    void Awake()
    {
        material = new Material(Shader.Find("Hidden/PaperShadeEffect"));
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (intensity == 0)
        {
            Graphics.Blit(source, destination);
            return;
        }

        material.SetFloat("_Intensity", intensity);
        material.SetColor("_TintColor", tintColor);
        material.SetFloat("_BoolDoLerp", doLerp ? 1 : 0);
        material.SetFloat("_SpotlightRadius", spotlightRadius);
        material.SetVector("_PlayerPosition", playerPosition);
        material.SetVector("_TargetPosition", targetPosition);
        Graphics.Blit(source, destination, material);
    }
}
