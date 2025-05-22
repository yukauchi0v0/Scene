using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class IntroController : MonoBehaviour
{
    public Transform introStartPos;
    public Transform introEndPos;
    public float introDuration = 5f;
    public AnimationCurve introSpeedCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    public Image fadePanel;
    public Text introText;
    public GameObject introLogo;

    public GameObject character;
    public CanvasGroup characterCanvasGroup;
    public float characterFadeDuration = 2f;

    public CameraFollow cameraFollowScript;

    void Start()
    {
        character.SetActive(false);
        if (introLogo != null) introLogo.SetActive(true);
        StartCoroutine(PlayIntro());
    }

    IEnumerator PlayIntro()
    {
        float t = 0f;
        transform.position = introStartPos.position;

        if (fadePanel != null) fadePanel.color = Color.black;
        if (introText != null) introText.color = Color.white;

        while (t < introDuration)
        {
            t += Time.deltaTime;
            float lerpT = introSpeedCurve.Evaluate(t / introDuration);
            transform.position = Vector3.Lerp(introStartPos.position, introEndPos.position, lerpT);
            yield return null;
        }

        yield return StartCoroutine(FadeOutIntroElements());
        yield return StartCoroutine(FadeInCharacter());

        // ✅ 設定 offset，避免畫面跳動
        if (cameraFollowScript != null && character != null)
        {
            cameraFollowScript.offset = cameraFollowScript.transform.position - character.transform.position;
            cameraFollowScript.EnableFollow();
        }
    }

    IEnumerator FadeOutIntroElements()
    {
        float t = 0f;
        float fadeDuration = 2f;

        Color panelColor = fadePanel != null ? fadePanel.color : Color.black;
        Color textColor = introText != null ? introText.color : Color.white;
        Color logoColor = Color.white;
        Image logoImg = introLogo?.GetComponent<Image>();

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, t / fadeDuration);

            if (fadePanel != null)
            {
                panelColor.a = alpha;
                fadePanel.color = panelColor;
            }

            if (introText != null)
            {
                textColor.a = alpha;
                introText.color = textColor;
            }

            if (logoImg != null)
            {
                logoColor.a = alpha;
                logoImg.color = logoColor;
            }

            yield return null;
        }

        if (fadePanel != null) fadePanel.gameObject.SetActive(false);
        if (introText != null) introText.gameObject.SetActive(false);
        if (introLogo != null) introLogo.SetActive(false);
    }

    IEnumerator FadeInCharacter()
    {
        character.SetActive(true);
        float t = 0f;

        SpriteRenderer sr = character.GetComponent<SpriteRenderer>();
        Color baseColor = sr != null ? sr.color : Color.white;

        while (t < characterFadeDuration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, t / characterFadeDuration);

            if (sr != null)
            {
                Color c = baseColor;
                c.a = alpha;
                sr.color = c;
            }

            if (characterCanvasGroup != null)
                characterCanvasGroup.alpha = alpha;

            yield return null;
        }
    }
}
