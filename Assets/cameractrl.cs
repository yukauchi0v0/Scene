using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class cameractrl : MonoBehaviour
{
    public Transform target;
    public Transform target1;
    public Transform target2;
    public Transform target3;

    public float Speed = 1000;
    public float Speed_rotate = 10;
    public float AddSpeed = 10;
    public float slowSpeed = 1;
    public float AddSpeed_rotate = 20;
    public float slowSpeed_rotate = 10;

    public Transform Camer_Pos1;
    public Transform Camer_Pos2;
    public Transform Camer_Pos3;
    public bool LookAt;

    // Camera sway
    public float swayAmount = 0.5f;
    public float swaySpeed = 0.2f;
    private Vector3 initialPosition;
    private float swayTimer = 0f;

    // Intro animation
    public bool playIntro = true;
    public Transform introStartPos;
    public Transform introEndPos;
    public float introDuration = 5f;
    public AnimationCurve introSpeedCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f); // <== 新增速度曲線
    public Image fadePanel;
    public Text introText;
    public GameObject introLogo;
    public float fadeDuration = 2f;

    // Character
    public GameObject character;
    public CanvasGroup characterCanvasGroup;
    public float characterFadeDuration = 2f;

    private bool introPlayed = false;

    void Start()
    {
        initialPosition = transform.localPosition;

        if (playIntro)
        {
            character.SetActive(false);
            if (introLogo != null) introLogo.SetActive(true);
            StartCoroutine(PlayIntro());
        }
    }

    void Update()
    {
        if (playIntro && !introPlayed) return;

        bool isMoving =
            Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) ||
            Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) ||
            Input.GetKey(KeyCode.R) || Input.GetKey(KeyCode.F) ||
            Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.E);

        if (!isMoving)
        {
            swayTimer += Time.deltaTime * swaySpeed;
            float swayX = (Mathf.PerlinNoise(swayTimer, 0f) - 0.5f) * 2f;
            float swayY = (Mathf.PerlinNoise(0f, swayTimer) - 0.5f) * 2f;
            float angleX = swayX * swayAmount * 10f;
            float angleY = swayY * swayAmount * 10f;
            transform.localRotation = Quaternion.Euler(angleY, 0f, angleX);
        }
        else
        {
            transform.localRotation = Quaternion.identity;
        }

        if (Input.GetKey(KeyCode.W)) transform.Translate(Vector3.forward * Speed * Time.deltaTime);
        if (Input.GetKey(KeyCode.S)) transform.Translate(Vector3.back * Speed * Time.deltaTime);
        if (Input.GetKey(KeyCode.A)) transform.Translate(Vector3.left * Speed * Time.deltaTime);
        if (Input.GetKey(KeyCode.D)) transform.Translate(Vector3.right * Speed * Time.deltaTime);
        if (Input.GetKey(KeyCode.R)) transform.Translate(Vector3.up * Speed * Time.deltaTime);
        if (Input.GetKey(KeyCode.F)) transform.Translate(Vector3.down * Speed * Time.deltaTime);

        if (Input.GetKey(KeyCode.Q)) transform.Rotate(Vector3.up * Speed_rotate * Time.deltaTime);
        if (Input.GetKey(KeyCode.E)) transform.Rotate(Vector3.up * -Speed_rotate * Time.deltaTime);
        if (Input.GetKey(KeyCode.Z)) transform.Rotate(Vector3.forward * Speed_rotate * Time.deltaTime);
        if (Input.GetKey(KeyCode.C)) transform.Rotate(Vector3.forward * -Speed_rotate * Time.deltaTime);

        if (Input.GetKey(KeyCode.X))
        {
            transform.position = new Vector3(-107.81f, 2.23f, 165.8f);
            transform.rotation = Quaternion.identity;
            LookAt = false;
        }

        if (Input.GetKey(KeyCode.V))
        {
            Speed = AddSpeed;
            Speed_rotate = AddSpeed_rotate;
        }
        else
        {
            Speed = slowSpeed;
            Speed_rotate = slowSpeed_rotate;
        }

        if (Input.GetKey(KeyCode.Alpha1))
        {
            transform.position = Camer_Pos1.position;
            target = target1;
            LookAt = true;
        }
        if (Input.GetKey(KeyCode.Alpha2))
        {
            transform.position = Camer_Pos2.position;
            target = target2;
            LookAt = true;
        }
        if (Input.GetKey(KeyCode.Alpha3))
        {
            transform.position = Camer_Pos3.position;
            target = target3;
            LookAt = true;
        }

        if (LookAt && target != null)
        {
            transform.LookAt(target);
        }
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
            float normalizedTime = Mathf.Clamp01(t / introDuration);
            float curvedT = introSpeedCurve.Evaluate(normalizedTime); // 套用速度曲線
            transform.position = Vector3.Lerp(introStartPos.position, introEndPos.position, curvedT);
            yield return null;
        }

        StartCoroutine(FadeOutIntroElements());
        StartCoroutine(FadeInCharacter());

        introPlayed = true;
    }

    IEnumerator FadeOutIntroElements()
{
    float t = 0f;
    Color fadeColor = fadePanel != null ? fadePanel.color : Color.black;
    Color textColor = introText != null ? introText.color : Color.white;
    Color logoColor = Color.white;
    UnityEngine.UI.Image logoImage = null;

    if (introLogo != null)
    {
        logoImage = introLogo.GetComponent<UnityEngine.UI.Image>();
        if (logoImage != null)
        {
            logoColor = logoImage.color;
        }
    }

    while (t < fadeDuration)
    {
        t += Time.deltaTime;
        float alpha = Mathf.Lerp(1f, 0f, t / fadeDuration);

        if (fadePanel != null)
        {
            fadeColor.a = alpha;
            fadePanel.color = fadeColor;
        }

        if (introText != null)
        {
            textColor.a = alpha;
            introText.color = textColor;
        }

        if (logoImage != null)
        {
            Color c = logoColor;
            c.a = alpha;
            logoImage.color = c;
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
        Color original = sr != null ? sr.color : Color.white;

        while (t < characterFadeDuration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, t / characterFadeDuration);

            if (sr != null)
            {
                Color c = original;
                c.a = alpha;
                sr.color = c;
            }

            if (characterCanvasGroup != null)
            {
                characterCanvasGroup.alpha = alpha;
            }

            yield return null;
        }
    }
}
