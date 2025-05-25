using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("基本移動速度")]
    public float normalSpeed = 5f;
    public float sprintSpeed = 10f;
    public float acceleration = 5f;

    [Header("初始化位置")]
    public Vector3 resetPosition = new Vector3(-107.81f, 2.23f, 165.8f);

    private float currentSpeed;

    void Start()
    {
        currentSpeed = normalSpeed;
    }

    void Update()
    {
        // 判斷是否按下 Shift（加速）
        bool isSprinting = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

        float targetSpeed = isSprinting ? sprintSpeed : normalSpeed;

        // 平滑加速 / 減速
        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, Time.deltaTime * acceleration);

        // 基本方向（左右前後）
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // 上下移動（R/F）
        float y = 0f;
        if (Input.GetKey(KeyCode.R)) y = 1f;
        if (Input.GetKey(KeyCode.F)) y = -1f;

        Vector3 moveDir = new Vector3(h, y, v);
        transform.Translate(moveDir.normalized * currentSpeed * Time.deltaTime, Space.World);

        // 回到指定初始位置（X鍵）
        if (Input.GetKeyDown(KeyCode.X))
        {
            transform.position = resetPosition;
        }
    }
}
