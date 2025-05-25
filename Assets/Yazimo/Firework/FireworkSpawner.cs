using UnityEngine;
using MidiJack;

public class FireworkByCC : MonoBehaviour
{
    public GameObject fireworkPrefab;
    public Transform spawnPoint;
    public int ccIndex = 20; // 預設為 CC 控制器編號 20

    private float lastValue = 0f;
    public float threshold = 0.8f; // 偵測值變成這麼高就觸發

    void Update()
    {
        float ccValue = MidiMaster.GetKnob(ccIndex);

        // 從低變到高（按下的瞬間）
        if (ccValue > threshold && lastValue <= threshold)
        {
            SpawnFirework();
        }

        lastValue = ccValue;
    }

    void SpawnFirework()
    {
        GameObject fx = Instantiate(fireworkPrefab, spawnPoint.position, Quaternion.identity);
        Destroy(fx, 5f);
    }
}
