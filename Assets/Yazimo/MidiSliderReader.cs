using UnityEngine;
using MidiJack;

public class MidiSliderReader : MonoBehaviour
{
    void Update()
    {
        // 這裡是逐個測試 KORG nanoKONTROL2 的控制器通道
        for (int knob = 0; knob < 128; knob++)
        {
            float value = MidiMaster.GetKnob(knob, 0f);
            if (value != 0f)
            {
                Debug.Log($"Knob {knob} value: {value}");
            }
        }
    }
}
