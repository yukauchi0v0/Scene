using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using MidiJack;

public class WindMidiControl : MonoBehaviour
{
    public StudioEventEmitter windEmitter;
    public int knobIndex = 1;  // 指定控制風聲的 MIDI 編號

    private float lastValue = -1f;

    void Update()
    {
        float current = MidiMaster.GetKnob(knobIndex);

        if (Mathf.Abs(current - lastValue) > 0.001f && windEmitter.EventInstance.isValid())
        {
            windEmitter.EventInstance.setParameterByName("Wind", current);
            lastValue = current;
        }
    }
}
