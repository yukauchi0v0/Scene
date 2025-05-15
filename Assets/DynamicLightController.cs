using UnityEngine;

public class SimplePingPongMover : MonoBehaviour
{
    [Header("X 軸移動設定")]
    public float minX = 2f;    // X 軸的最小位置
    public float maxX = 40f;   // X 軸的最大位置
    public float speedX = 2f;  // 移動速度

    [Header("起始位置")]
    public float startX = 2f;  // 物件一開始要出現在的 X 位置

    private float range;
    private float pingPongTime;

    void Start()
    {
        // 確保物件一開始就在 startX 位置
        transform.position = new Vector3(startX, transform.position.y, transform.position.z);

        // 計算範圍
        range = maxX - minX;
        if (range <= 0)
        {
            Debug.LogError("minX 必須小於 maxX");
        }
    }

    void Update()
    {
        if (range <= 0)
            return;

        // 根據時間來計算 PingPong
        pingPongTime = Mathf.PingPong(Time.time * speedX, range);
        float currentX = minX + pingPongTime;

        // 只改變 X 軸，保持 Y 和 Z 軸不變
        transform.position = new Vector3(currentX, transform.position.y, transform.position.z);
    }
}
