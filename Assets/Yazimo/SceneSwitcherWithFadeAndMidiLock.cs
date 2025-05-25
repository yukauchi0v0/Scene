using UnityEngine;
using MidiJack;
using UnityEngine.UI;
using System.Collections;

public class SceneSwitcherStableOptimized : MonoBehaviour
{
    public Transform characterTarget;
    public Transform skyPosition;
    public Transform cityPosition;
    public Transform seaPosition;

    public Image fadeImage;
    public int knobIndex = 0;
    public float transitionDuration = 2f;

    private bool isTransitioning = false;

    private enum Region { Sea, City, Sky }
    private Region currentRegion = Region.City;

    [Header("區間判斷範圍")]
    public float seaMax = 0.3f;
    public float cityMin = 0.35f;
    public float cityMax = 0.65f;
    public float skyMin = 0.7f;

    [Header("MIDI 按鍵通道編號")]
    public int seaButtonIndex = 0;
    public int cityButtonIndex = 1;
    public int skyButtonIndex = 2;

    void Start()
    {
        if (fadeImage != null)
            fadeImage.color = new Color(0, 0, 0, 0);
    }

    void Update()
    {
        if (isTransitioning) return;

        // ✅ 按鈕觸發優先判斷
        if (MidiMaster.GetKeyDown(seaButtonIndex))
        {
            TrySwitchRegion(Region.Sea);
            return;
        }
        else if (MidiMaster.GetKeyDown(cityButtonIndex))
        {
            TrySwitchRegion(Region.City);
            return;
        }
        else if (MidiMaster.GetKeyDown(skyButtonIndex))
        {
            TrySwitchRegion(Region.Sky);
            return;
        }

        // ✅ 滑桿判斷（原本邏輯保留）
        float knobValue = MidiMaster.GetKnob(knobIndex, 0.5f);
        Region targetRegion = currentRegion;

        if (knobValue <= seaMax)
            targetRegion = Region.Sea;
        else if (knobValue >= skyMin)
            targetRegion = Region.Sky;
        else if (knobValue >= cityMin && knobValue <= cityMax)
            targetRegion = Region.City;

        TrySwitchRegion(targetRegion);
    }

    void TrySwitchRegion(Region targetRegion)
    {
        if (targetRegion != currentRegion)
        {
            currentRegion = targetRegion;
            StartCoroutine(SwitchTo(GetTargetForRegion(currentRegion)));
        }
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

        if (fadeImage != null)
        {
            float t = 0f;
            while (t < 1f)
            {
                t += Time.deltaTime / (transitionDuration / 2f);
                fadeImage.color = new Color(0, 0, 0, Mathf.Clamp01(t));
                yield return null;
            }
            fadeImage.color = new Color(0, 0, 0, 1f);
        }

        yield return new WaitForSeconds(0.05f);

        Vector3 startPos = characterTarget.position;
        Vector3 endPos = target.position;

        float moveT = 0f;
        while (moveT < transitionDuration)
        {
            moveT += Time.deltaTime;
            float lerpT = moveT / transitionDuration;
            characterTarget.position = Vector3.Lerp(startPos, endPos, lerpT);
            yield return null;
        }

        characterTarget.position = endPos;

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
