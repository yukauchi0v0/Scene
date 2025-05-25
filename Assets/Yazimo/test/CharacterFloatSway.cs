using UnityEngine;

public class CharacterFloatSway : MonoBehaviour
{
    public float swayAmount = 0.2f;
    public float swaySpeed = 1f;

    private Vector3 initialLocalPos;
    private float swayTimer = 0f;

    void Start()
    {
        initialLocalPos = transform.localPosition;
    }

    void Update()
    {
        swayTimer += Time.deltaTime * swaySpeed;

        float offsetY = Mathf.PerlinNoise(swayTimer, 0f) - 0.5f;
        float offsetX = Mathf.PerlinNoise(0f, swayTimer) - 0.5f;

        Vector3 swayOffset = new Vector3(offsetX, offsetY, 0) * swayAmount;

        transform.localPosition = initialLocalPos + swayOffset;
    }
}
