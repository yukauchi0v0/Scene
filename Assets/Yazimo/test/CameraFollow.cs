using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 5, -8); // 預設距離（可被覆蓋）
    public float followSpeed = 5f;

    private bool canFollow = false;
    private bool justEnabledFollow = false;

    /// <summary>
    /// 開啟攝影機跟隨模式（動畫結束後呼叫）
    /// </summary>
    public void EnableFollow()
    {
        Debug.Log("Camera Follow 啟用，Offset: " + offset);
        canFollow = true;
        justEnabledFollow = true; // 下一幀立即對位
    }

    void LateUpdate()
    {
        if (!canFollow || target == null) return;

        Vector3 desiredPos = target.position + offset;

        if (justEnabledFollow)
        {
            // 第一幀直接定位，避免跳動
            transform.position = desiredPos;
            justEnabledFollow = false;
        }
        else
        {
            // 後續平滑跟隨
            transform.position = Vector3.Lerp(transform.position, desiredPos, Time.deltaTime * followSpeed);
        }

        
    }
}
