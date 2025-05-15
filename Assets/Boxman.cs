// 引入 Unity 的系統集合命名空間，提供像是 IEnumerable 等介面
using System.Collections;
// 引入泛型集合命名空間，提供像是 List<T> 等泛型容器
using System.Collections.Generic;
// 引入 UnityEngine 命名空間，這是 Unity 的核心功能庫
using UnityEngine;

// 宣告一個公開的類別 Boxman，繼承自 MonoBehaviour（所有 Unity 腳本的基礎類別）
public class Boxman : MonoBehaviour
{
    // 宣告一個公開的浮點數變數 addValue，可在 Unity Inspector 面板上調整
    public float addValue;
    
    // 宣告旋轉速度變數
    public float roateSpeed;
    
    // 宣告移動速度變數
    public float moveSpeed;
    
    // 宣告旋轉方向，為一個三維向量 (Vector3)
    public Vector3 roateDirection;
    
    // 宣告移動方向，為一個三維向量
    public Vector3 moveDirection;

    // Start 是 Unity 的生命週期方法，在場景開始執行的第一幀呼叫一次
    void Start()
    {
        // 輸出訊息到 Console，確認 Start 被呼叫
        Debug.Log("start Hi");

        // 以下是被註解掉的測試程式碼，可用來設定 addValue 初始值
        // addValue = 0;
        // addValue = 100;
        // Debug.Log(addValue);
    }
 
    // Update 是 Unity 的生命週期方法，每一幀都會呼叫一次（大約每秒呼叫數十次，取決於 FPS）
    void Update()
    {
        // 以下是註解掉的測試程式碼，用來讓 addValue 隨時間增加
        // addValue = addValue + 0.05f;

        // 讓物件以 roateDirection 為方向進行旋轉，旋轉速率是 roateSpeed 加上 addValue
        transform.Rotate(roateDirection * (roateSpeed + addValue));

        // 讓物件以 moveDirection 為方向進行移動，移動速率是 moveSpeed
        transform.Translate(moveDirection * moveSpeed);
    }
}
