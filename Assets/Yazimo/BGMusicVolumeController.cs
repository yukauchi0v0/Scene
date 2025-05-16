using UnityEngine;
using MidiJack;

public class BGMusicVolumeController : MonoBehaviour
{
    [Header("控制的音樂來源（可留空，自動尋找）")]
    public AudioSource bgmSource;

    [Header("MIDI 旋鈕 Index")]
    public int knobIndex = 0;

    [Header("音量倍率 (可選)")]
    [Range(0f, 2f)] public float volumeMultiplier = 1f;

    [Header("更新頻率控制")]
    public float updateInterval = 0.05f; // 每 0.05 秒更新一次

    [Header("數值變動閾值")]
    public float threshold = 0.01f; // 變動小於 1% 不更新

    [Header("平滑過渡速度")]
    public float lerpSpeed = 5f; // 數值越大，越快跟上旋鈕變動

    private float targetVolume = 0f;
    private float currentVolume = 0f;
    private float updateTimer = 0f;

    void Start()
    {
        if (bgmSource == null)
        {
            Camera cam = GameObject.FindObjectOfType<Camera>();
            if (cam != null)
            {
                bgmSource = cam.GetComponent<AudioSource>();
                if (bgmSource != null)
                    Debug.Log("找到了 AudioSource，並已連接！");
                else
                    Debug.LogWarning("找到了 Camera，但沒有 AudioSource！");
            }
            else
            {
                Debug.LogWarning("場景中找不到任何 Camera！");
            }
        }

        if (bgmSource != null)
        {
            // 一開始就讀取旋鈕數值並同步
            float initKnobValue = MidiMaster.GetKnob(knobIndex);
            targetVolume = Mathf.Clamp01(initKnobValue * volumeMultiplier);
            currentVolume = targetVolume;
            bgmSource.volume = currentVolume;

            Debug.Log($"初始化音量：{currentVolume}");
        }
    }

    void Update()
    {
        if (bgmSource == null) return;

        updateTimer += Time.deltaTime;

        if (updateTimer >= updateInterval)
        {
            updateTimer = 0f;

            float knobValue = MidiMaster.GetKnob(knobIndex);
            float newTargetVolume = Mathf.Clamp01(knobValue * volumeMultiplier);

            if (Mathf.Abs(newTargetVolume - targetVolume) > threshold)
            {
                targetVolume = newTargetVolume;
            }
        }

        // 平滑過渡音量
        currentVolume = Mathf.Lerp(currentVolume, targetVolume, lerpSpeed * Time.deltaTime);
        bgmSource.volume = currentVolume;
    }
}
