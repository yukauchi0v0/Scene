using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraWindShake : MonoBehaviour
{
    public float shakeMagnitude = 0.1f;  // 晃動幅度（建議 0.01 ~ 0.1）
    public float shakeSpeed = 0.5f;      // 晃動速度（越高變化越快）

    private Vector3 originalPos;
    private float noiseSeedX;
    private float noiseSeedY;

    void Start()
    {
        originalPos = transform.localPosition;
        noiseSeedX = Random.Range(0f, 100f);
        noiseSeedY = Random.Range(0f, 100f);
    }

    void Update()
    {
        float offsetX = (Mathf.PerlinNoise(Time.time * shakeSpeed + noiseSeedX, 0f) - 0.5f) * 2f * shakeMagnitude;
        float offsetY = (Mathf.PerlinNoise(Time.time * shakeSpeed + noiseSeedY, 1f) - 0.5f) * 2f * shakeMagnitude;

        transform.localPosition = originalPos + new Vector3(offsetX, offsetY, 0f);
    }
}
