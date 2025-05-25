using UnityEngine;
using MidiJack;

public class FireworkManager : MonoBehaviour
{
    public GameObject fireworkPrefab; // 拖入煙火特效Prefab
    public Transform player;          // 角色物件
    public int knobIndex = 0;         // MIDI 控制按鈕 Index
    public float spawnRadius = 3f;    // 煙火生成半徑

    private float lastValue = 0f;

    void Update()
    {
        float currentValue = MidiMaster.GetKnob(knobIndex); // 或 GetKey(knobIndex)
        
        if (lastValue < 0.5f && currentValue >= 0.5f)
        {
            SpawnFirework();
        }

        lastValue = currentValue;
    }

    void SpawnFirework()
{
    Vector3 behindDirection = -player.forward;
    Vector3 randomOffset = new Vector3(
        Random.Range(-1f, 1f),
        Random.Range(0.5f, 2f),
        Random.Range(0.5f, 1.5f)
    );
    Vector3 spawnPos = player.position + behindDirection * spawnRadius + randomOffset;

    // 發射方向：以「大致往上」為主，加一點隨機偏移
    Vector3 fireDirection = Vector3.up + new Vector3(
        Random.Range(-0.3f, 0.3f),
        0,
        Random.Range(-0.3f, 0.3f)
    );

    Quaternion fireRotation = Quaternion.LookRotation(fireDirection.normalized);

    Instantiate(fireworkPrefab, spawnPos, fireRotation);
}
}
