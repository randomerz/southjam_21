using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ColorStrobe : MonoBehaviour
{
    public Material whiteFlashMat;
    private const float strobeDelay = 0.125f;
    private SpriteRenderer spriteRenderer;

    private bool strobing;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }



    public void StrobeWhite(int strobeCount)
    {
        if (strobing)
            return;

        Material oldMat = spriteRenderer.material;

        StartCoroutine(StrobeWhiteHelper((strobeCount * 2) - 1, oldMat, whiteFlashMat));
    }

    private IEnumerator StrobeWhiteHelper(int stopAt, Material oldMat, Material toStrobe)
    {
        strobing = true;
        int i = 0;
        
        while (i <= stopAt)
        {
            if (i % 2 == 0)
                spriteRenderer.material = toStrobe;
            else
                spriteRenderer.material = oldMat;

            yield return new WaitForSeconds(strobeDelay);
            i++;
        }

        strobing = false;
    }



    public void StrobeColor(int strobeCount, Color toStrobe)
    {
        if (strobing)
            return;

        StartCoroutine(StrobeColorHelper((strobeCount * 2) - 1, spriteRenderer.color, toStrobe));
    }

    public void StrobeAlpha(int _strobeCount, float a)
    {
        if (strobing)
            return;

        Color oldColor = spriteRenderer.color;
        Color toStrobe = oldColor;
        toStrobe.a = a;

        StartCoroutine(StrobeColorHelper((_strobeCount * 2) - 1, oldColor, toStrobe));
    }

    private IEnumerator StrobeColorHelper(int stopAt, Color color, Color toStrobe)
    {
        strobing = true;

        int i = 0;

        while (i <= stopAt)
        {
            if (i % 2 == 0)
                spriteRenderer.color = toStrobe;
            else
                spriteRenderer.color = color;

            yield return new WaitForSeconds(strobeDelay);
            i++;
            //StartCoroutine(StrobeColorHelper((i + 1), stopAt, color, toStrobe));
        }

        strobing = false;
    }
}
