using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MidiJack;

public class OptimizedDayNightController : MonoBehaviour
{
    [Header("漸變控制")]
    [SerializeField] private float transitionSpeed = 1.5f;
    [SerializeField] private bool useSliderControl = true;
    [SerializeField] private float dayDurationInSeconds = 60f;

    [Header("滑桿偵測設定")]
    [SerializeField] private int midiKnobIndex = 0;
    [SerializeField] private float valueThreshold = 0.01f;

    [Header("快捷鍵 MIDI Note（S/M/R）")]
    [SerializeField] private int noteDay = 32;    // S 鍵
    [SerializeField] private int noteDusk = 48;   // M 鍵
    [SerializeField] private int noteNight = 64;  // R 鍵

    [Header("光線與顏色")]
    [SerializeField] private Light directionalLight;
    [SerializeField] private Material skyboxMaterial;
    [SerializeField] private Gradient fogGradient;
    [SerializeField] private Gradient ambientGradient;
    [SerializeField] private Gradient directionLightGradient;
    [SerializeField] private Gradient skyboxTintGradient;

    private float currentTime = 0f;
    private float targetTime = 0f;
    private float lastKnobValue = -1f;

    void Update()
    {
        // 快捷鍵控制（優先於滑桿）
        if (MidiMaster.GetKeyDown(noteDay))
        {
            targetTime = 0.0f;  // 白天
        }
        else if (MidiMaster.GetKeyDown(noteDusk))
        {
            targetTime = 0.45f; // 黃昏
        }
        else if (MidiMaster.GetKeyDown(noteNight))
        {
            targetTime = 0.85f; // 夜晚
        }
        else if (useSliderControl)
        {
            float knobValue = MidiMaster.GetKnob(midiKnobIndex);

            // 若滑桿變化幅度太小，就不更新
            if (Mathf.Abs(knobValue - lastKnobValue) > valueThreshold)
            {
                targetTime = knobValue;  // 0~1 直接代表一天的時間
                lastKnobValue = knobValue;
            }
        }
        else
        {
            // 自動時間流動模式
            targetTime += Time.deltaTime / dayDurationInSeconds;
            targetTime = Mathf.Repeat(targetTime, 1f);
        }

        // 平滑追趕 targetTime
        currentTime = Mathf.Lerp(currentTime, targetTime, Time.deltaTime * transitionSpeed);

        UpdateLighting(currentTime);
        RotateSkybox();
    }

    void UpdateLighting(float time)
    {
        float sunPosition = Mathf.Repeat(time + 0.25f, 1f);
        directionalLight.transform.rotation = Quaternion.Euler(sunPosition * 360f, 0f, 0f);

        RenderSettings.fogColor = fogGradient.Evaluate(time);
        RenderSettings.ambientLight = ambientGradient.Evaluate(time);
        directionalLight.color = directionLightGradient.Evaluate(time);
        skyboxMaterial.SetColor("_Tint", skyboxTintGradient.Evaluate(time));
    }

    void RotateSkybox()
    {
        float currentRotation = skyboxMaterial.GetFloat("_Rotation");
        float newRotation = currentRotation + 1f * Time.deltaTime;
        skyboxMaterial.SetFloat("_Rotation", newRotation);
    }

    private void OnApplicationQuit()
    {
        // 重設 Skybox 顏色，避免關閉後保持夜晚
        skyboxMaterial.SetColor("_Tint", new Color(0.5f, 0.5f, 0.5f));
    }
}
