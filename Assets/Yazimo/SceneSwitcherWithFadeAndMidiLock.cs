using UnityEngine;
using MidiJack;
using UnityEngine.UI;
using System.Collections;

public class SceneSwitcherStableOptimized : MonoBehaviour
{
    public Transform cameraTarget;
    public Transform skyPosition;
    public Transform cityPosition;
    public Transform seaPosition;

    public Image fadeImage; // 全螢幕黑色 UI Image
    public int knobIndex = 0;
    public float transitionDuration = 2f;
    public float triggerThreshold = 0.15f;

    private bool isTransitioning = false;
    private float lastValue = 0.5f;

    private enum Region { Sea, City, Sky }
    private Region currentRegion = Region.City;

    
    void Start()
    {
        if (fadeImage != null)
            fadeImage.color = new Color(0, 0, 0, 0);
    }

    void Update()
    {
        if (isTransitioning) return;

        float knobValue = MidiMaster.GetKnob(knobIndex, 0.5f);

        if (Mathf.Abs(knobValue - lastValue) < 0.01f) return;

        Region targetRegion = currentRegion;

        if (knobValue <= triggerThreshold)
            targetRegion = Region.Sea;
        else if (knobValue >= 1f - triggerThreshold)
            targetRegion = Region.Sky;
        else
            targetRegion = Region.City;

        if (targetRegion != currentRegion)
        {
            currentRegion = targetRegion;
            StartCoroutine(SwitchTo(GetTargetForRegion(currentRegion)));
        }

        lastValue = knobValue;
    }

    Transform GetTargetForRegion(Region region)
    {
        switch (region)
        {
            case Region.Sea: return seaPosition;
            case Region.Sky: return skyPosition;
            default: return cityPosition;
        }
    }

    IEnumerator SwitchTo(Transform target)
    {
        isTransitioning = true;

        // 🎬 淡出黑幕（確保完全黑）
        if (fadeImage != null)
        {
            float t = 0f;
            while (t < 1f)
            {
                t += Time.deltaTime / (transitionDuration / 2f);
                fadeImage.color = new Color(0, 0, 0, Mathf.Clamp01(t));
                yield return null;
            }
            fadeImage.color = new Color(0, 0, 0, 1f); // 強制補齊
        }

        // ✨ 可選：黑完後稍微停一下（讓觀眾感覺更自然）
        yield return new WaitForSeconds(0.05f);

        // 📸 相機移動
        Vector3 startPos = cameraTarget.position;
        Quaternion startRot = cameraTarget.rotation;
        Vector3 endPos = target.position;
        Quaternion endRot = target.rotation;

        float moveT = 0f;
        while (moveT < transitionDuration)
        {
            moveT += Time.deltaTime;
            float lerpT = moveT / transitionDuration;
            cameraTarget.position = Vector3.Lerp(startPos, endPos, lerpT);
            cameraTarget.rotation = Quaternion.Slerp(startRot, endRot, lerpT);
            yield return null;
        }

        cameraTarget.position = endPos;
        cameraTarget.rotation = endRot;

        // 🎬 淡入還原畫面
        if (fadeImage != null)
        {
            float t = 1f;
            while (t > 0f)
            {
                t -= Time.deltaTime / (transitionDuration / 2f);
                fadeImage.color = new Color(0, 0, 0, Mathf.Clamp01(t));
                yield return null;
            }
            fadeImage.color = new Color(0, 0, 0, 0f);
        }

        isTransitioning = false;
    }
}
