using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MidiJack;

public class DayNightController : MonoBehaviour
{
    [Header("Gradients")]
    [SerializeField] private Gradient fogGradient;
    [SerializeField] private Gradient ambientGradient;
    [SerializeField] private Gradient directionLightGradient;
    [SerializeField] private Gradient skyboxTintGradient;

    [Header("Enviromental Assets")]
    [SerializeField] private Light directionalLight;
    [SerializeField] private Material skyboxMaterial;

    [Header("Variables")]
    [SerializeField] private float dayDurationInSeconds = 60f;
    [SerializeField] private float rotationSpeed = 1f;
    [SerializeField] private bool useSliderControl = true;
    [SerializeField] private float transitionSpeed = 1.5f; // 漸變速度

    private float currentTime = 0f;
    private float targetTime = 0f;

    void Update()
    {
        UpdateTime();
        SmoothUpdate();
        UpdateDayNightCycle();
        RotateSkybox();
    }

    private void UpdateTime()
    {
        if (useSliderControl)
        {
            float raw = MidiMaster.GetKnob(0);

            if (raw < 0.3f)
                targetTime = 0f; // 白天
            else if (raw < 0.5f)
                targetTime = Mathf.InverseLerp(0.3f, 0.5f, raw) * 0.25f + 0.25f; // 黃昏 (0.25~0.5)
            else if (raw < 0.7f)
                targetTime = 0.5f; // 夜晚
            else if (raw < 0.9f)
                targetTime = Mathf.InverseLerp(0.7f, 0.9f, raw) * 0.25f + 0.75f; // 清晨 (0.75~1)
            else
                targetTime = 1f; // 白天
        }
        else
        {
            targetTime += Time.deltaTime / dayDurationInSeconds;
            targetTime = Mathf.Repeat(targetTime, 1f);
        }
    }

    private void SmoothUpdate()
    {
        // 漸變過渡
        currentTime = Mathf.Lerp(currentTime, targetTime, Time.deltaTime * transitionSpeed);
    }

    private void UpdateDayNightCycle()
    {
        float sunPosition = Mathf.Repeat(currentTime + 0.25f, 1f);
        directionalLight.transform.rotation = Quaternion.Euler(sunPosition * 360f, 0f, 0f);

        RenderSettings.fogColor = fogGradient.Evaluate(currentTime);
        RenderSettings.ambientLight = ambientGradient.Evaluate(currentTime);
        directionalLight.color = directionLightGradient.Evaluate(currentTime);
        skyboxMaterial.SetColor("_Tint", skyboxTintGradient.Evaluate(currentTime));
    }

    private void RotateSkybox()
    {
        float currentRotation = skyboxMaterial.GetFloat("_Rotation");
        float newRotation = currentRotation + rotationSpeed * Time.deltaTime;
        skyboxMaterial.SetFloat("_Rotation", newRotation);
    }

    private void OnApplicationQuit()
    {
        skyboxMaterial.SetColor("_Tint", new Color(0.5f, 0.5f, 0.5f));
    }
}
