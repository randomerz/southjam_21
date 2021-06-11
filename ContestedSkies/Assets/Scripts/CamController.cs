using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamController : MonoBehaviour
{
    public static CamController Instance
    {
        get { return _instance; }
    }
    private static CamController _instance;

    public AnimationCurve shake_envelope;

    Cinemachine.CinemachineVirtualCamera cvc;

    private void OnEnable()
    {
        _instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        cvc = GetComponent<Cinemachine.CinemachineVirtualCamera>();
        cvc.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0;
    }

    /**
     * magnitude: magnitude of shake, there is no standard unit for this, but the bigger the more dramatic
     * duration: duration of shake in seconds
     */
    public void Shake(float magnitude, float duration)
    {
        StartCoroutine(ShakeCoroutine(magnitude, duration));
    }

    private IEnumerator ShakeCoroutine(float magnitude, float duration)
    {
        if (duration <= 0)
        {
            throw new System.ArgumentException("duration must be larger than 0");
        }
        float t = 0;
        Cinemachine.CinemachineVirtualCamera cvc = GetComponent<Cinemachine.CinemachineVirtualCamera>();
        if (cvc == null)
        {
            throw new System.NullReferenceException("cannot find cinemachine virtual camera in gameobject");
        }
        while (t < duration)
        {
            cvc.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain =
                magnitude * shake_envelope.Evaluate(t / duration);

            t += Time.deltaTime;
            yield return null;
        }
        cvc.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0;
        yield return null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
