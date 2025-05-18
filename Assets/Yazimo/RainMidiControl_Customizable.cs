using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using MidiJack;

public class RainMidiControl_Customizable : MonoBehaviour
{
    [Header("MIDI 滑桿控制")]
    public int knobIndex = 0;

    [Header("雨粒子系統")]
    public ParticleSystem rainSystem;

    [Header("FMOD 聲音控制")]
    public StudioEventEmitter rainEmitter;

    [Header("滑桿對應的雨強度範圍設定")]
    [Range(0f, 1f)] public float rainIntensity = 0f;

    [Header("雨量（rateOverTime）")]
    public float minRate = 0f;
    public float midRate = 500f;
    public float maxRate = 1500f;

    [Header("粒子存活時間（lifetime）")]
    public float minLifetime = 0.5f;
    public float midLifetime = 2f;
    public float maxLifetime = 4f;

    [Header("下墜速度（startSpeed）")]
    public float minSpeed = 5f;
    public float midSpeed = 15f;
    public float maxSpeed = 25f;

    [Header("最大同時粒子數")]
    public int minMaxParticles = 100;
    public int midMaxParticles = 2000;
    public int maxMaxParticles = 5000;

    void Update()
    {
        float input = MidiMaster.GetKnob(knobIndex);

        // 插值分段（0~0.5 = 小雨~中雨，0.5~1 = 中雨~大雨）
        float rate = input < 0.5f
            ? Mathf.Lerp(minRate, midRate, input * 2f)
            : Mathf.Lerp(midRate, maxRate, (input - 0.5f) * 2f);

        float lifetime = input < 0.5f
            ? Mathf.Lerp(minLifetime, midLifetime, input * 2f)
            : Mathf.Lerp(midLifetime, maxLifetime, (input - 0.5f) * 2f);

        float speed = input < 0.5f
            ? Mathf.Lerp(minSpeed, midSpeed, input * 2f)
            : Mathf.Lerp(midSpeed, maxSpeed, (input - 0.5f) * 2f);

        int maxParticles = input < 0.5f
            ? Mathf.RoundToInt(Mathf.Lerp(minMaxParticles, midMaxParticles, input * 2f))
            : Mathf.RoundToInt(Mathf.Lerp(midMaxParticles, maxMaxParticles, (input - 0.5f) * 2f));

        // 粒子設定
        var emission = rainSystem.emission;
        var main = rainSystem.main;

        emission.rateOverTime = rate;
        main.startLifetime = lifetime;
        main.startSpeed = speed;
        main.maxParticles = maxParticles;

        // 聲音設定
        if (rainEmitter != null && rainEmitter.EventInstance.isValid())
        {
            rainEmitter.EventInstance.setParameterByName("Rain", input);
        }

        // 永遠維持播放狀態（不再停用粒子）
        if (!rainSystem.isPlaying)
            rainSystem.Play();
    }
}
