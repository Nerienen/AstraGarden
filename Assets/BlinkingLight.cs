using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkingLight : MonoBehaviour
{
    public float blinkInterval = 0.5f; // Adjust this value to control the speed of blinking
    public float minIntensity = 0f;    // The minimum light intensity when it's off
    public float maxIntensity = 1f;    // The maximum light intensity when it's on

    private Light pointLight;

    void Start()
    {
        pointLight = GetComponent<Light>();
        StartCoroutine(Blink());
    }

    private System.Collections.IEnumerator Blink()
    {
        while (true)
        {
            pointLight.intensity = Mathf.PingPong(Time.time / blinkInterval, 1f) * (maxIntensity - minIntensity) + minIntensity;
            yield return null;
        }
    }
}
