using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class LogoFade : MonoBehaviour
{
    [Header("淡入／淡出時間 (秒)")]
    [Tooltip("遊戲開始時淡入所需時間")]
    public float fadeInDuration = 2f;
    [Tooltip("顯示完畢後淡出所需時間")]
    public float fadeOutDuration = 2f;
    [Tooltip("Logo 顯示後，等待多少秒才開始淡出")]
    public float delayBeforeFadeOut = 1f;

    private CanvasGroup canvasGroup;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        // 一開始完全透明，並禁止交互
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    void Start()
    {
        // 開始時先淡入，淡入結束後自動等待一段時間，再淡出
        StartCoroutine(FadeInOutSequence());
    }

    private IEnumerator FadeInOutSequence()
    {
        // ◆ 淡入
        float t = 0f;
        while (t < fadeInDuration)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, t / fadeInDuration);
            yield return null;
        }
        canvasGroup.alpha = 1f;

        // 淡入完成後才允許交互（若有按鈕需求可設 true）
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        // ◆ 等待
        yield return new WaitForSeconds(delayBeforeFadeOut);

        // ◆ 淡出
        t = 0f;
        while (t < fadeOutDuration)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, t / fadeOutDuration);
            yield return null;
        }
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        // 淡出完成後可選擇銷毀物件或隱藏
        gameObject.SetActive(false);
    }

    /// <summary>
    /// 你也可以在別處呼叫這個方法，手動觸發再淡出一次
    /// </summary>
    public void StartFadeOut()
    {
        StartCoroutine(FadeOutOnly());
    }

    private IEnumerator FadeOutOnly()
    {
        float t = 0f;
        while (t < fadeOutDuration)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, t / fadeOutDuration);
            yield return null;
        }
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
}
