using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using MidiJack;

public class RainMidiControl : MonoBehaviour
{
    public StudioEventEmitter rainEmitter;
    public int knobIndex = 0;

    private float lastValue = -1f;

    void Update()
    {
        float current = MidiMaster.GetKnob(knobIndex);

        if (Mathf.Abs(current - lastValue) > 0.001f && rainEmitter.EventInstance.isValid())
        {
            rainEmitter.EventInstance.setParameterByName("Rain", current);
            lastValue = current;
        }
    }
}
