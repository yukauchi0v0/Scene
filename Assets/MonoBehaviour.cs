using UnityEngine;

public class CameraSpriteFollower : MonoBehaviour 
{
    public Transform targetCamera; // 绑定主相机
    public Vector3 offset = new Vector3(0, -1, 2); // 偏移量

    void Update() 
    {
        // 只同步位置（不继承旋转）
        transform.position = targetCamera.transform.position + 
                           targetCamera.transform.forward * offset.z +
                           targetCamera.transform.up * offset.y;

        // 保持Sprite始终面向相机（Billboard效果）
        transform.rotation = Quaternion.Euler(0, targetCamera.transform.eulerAngles.y, 0);
    }
}