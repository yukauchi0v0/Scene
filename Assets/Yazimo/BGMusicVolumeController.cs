using UnityEngine;
using MidiJack;

public class BGMusicVolumeController : MonoBehaviour
{
    [Header("控制的音樂來源（可留空，自動尋找）")]
    public AudioSource bgmSource;

    [Header("MIDI 旋鈕 Index")]
    public int knobIndex = 0; // 預設使用第 1 顆旋鈕

    [Header("音量倍率 (可選)")]
    [Range(0f, 2f)] public float volumeMultiplier = 1f;

    void Start()
{
    // 如果沒有手動指定音源，就自動從場景中找一個 Camera 上的 AudioSource
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
        float initKnobValue = MidiMaster.GetKnob(knobIndex);
        float initVolume = Mathf.Clamp01(initKnobValue * volumeMultiplier);
        bgmSource.volume = initVolume;
    }
}


    void Update()
    {
        if (bgmSource == null) return;

        float knobValue = MidiMaster.GetKnob(knobIndex);
        float volume = Mathf.Clamp01(knobValue * volumeMultiplier);
        bgmSource.volume = volume;
    }
}
