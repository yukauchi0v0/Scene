using UnityEngine;

public class RainFollowCameraOffset : MonoBehaviour
{
    public Transform cameraTransform;
    private Vector3 initialOffset;

    public float delayBeforeStart = 2f;  // ⏱️ 可調整延遲時間（秒）
    private bool initialized = false;

    void Update()
    {
        // 延遲一段時間才初始化 offset
        if (!initialized)
        {
            delayBeforeStart -= Time.deltaTime;
            if (delayBeforeStart <= 0f)
            {
                if (cameraTransform != null)
                {
                    initialOffset = transform.position - cameraTransform.position;
                    initialized = true;
                }
            }
        }
    }

    void LateUpdate()
    {
        if (initialized && cameraTransform != null)
        {
            transform.position = cameraTransform.position + initialOffset;
        }
    }
}
