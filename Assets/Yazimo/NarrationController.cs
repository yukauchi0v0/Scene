using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NarrationController : MonoBehaviour
{
    public AudioSource narrationAudioSource;
    public Text subtitleText;

    [TextArea(3, 10)]
    public string[] subtitles;
    public float[] subtitleTimings;

    public float startDelay = 2f;
    public float volumeMultiplier = 2f;

    void Start()
    {
        narrationAudioSource.playOnAwake = false; // 保險措施
        StartCoroutine(PlayWithDelay());
    }

    IEnumerator PlayWithDelay()
    {
        yield return new WaitForSeconds(startDelay);

        narrationAudioSource.volume = Mathf.Clamp01(narrationAudioSource.volume * volumeMultiplier);
        narrationAudioSource.Play();

        for (int i = 0; i < subtitles.Length && i < subtitleTimings.Length; i++)
        {
            subtitleText.text = subtitles[i];
            yield return new WaitForSeconds(subtitleTimings[i]);
        }

        subtitleText.text = "";
    }
}
