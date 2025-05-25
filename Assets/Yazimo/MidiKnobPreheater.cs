using UnityEngine;
using MidiJack;

/// <summary>
/// ✅ 專門用來預熱 MIDI 通道，避免第一次滑桿卡頓。
/// 只要掛在任意場景物件上就會生效，不需改動原本的 MIDI 讀取邏輯。
/// </summary>
public class MidiKnobPreheater : MonoBehaviour
{
    [Header("要預熱的旋鈕通道 Index 範圍")]
    public int startIndex = 0;
    public int endIndex = 15;

    [Header("預設值（通常填 0.5）")]
    public float defaultKnobValue = 0.5f;

    void Start()
    {
        for (int i = startIndex; i <= endIndex; i++)
        {
            // 預熱：強制先讀取一次，避免初始卡頓
            float _ = MidiMaster.GetKnob(i, defaultKnobValue);
        }

        Debug.Log($"[MidiKnobPreheater] 預熱完成：Knob {startIndex} ~ {endIndex}");
    }
}
