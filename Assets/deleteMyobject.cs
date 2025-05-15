using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deleteMyobject : MonoBehaviour
{
    // 公開的變數 timer，可以在 Unity 編輯器中設定此物件延遲銷毀的時間（秒）
    public float timer;

    // Start 是 Unity 的內建方法，當物件啟用時會執行一次
    void Start()
    {
        // 使用 Unity 的 Destroy 函數，延遲 timer 秒後銷毀此物件本身
        Destroy(gameObject, timer);
    }

    // Update 每一幀執行一次，但這裡目前沒有需要處理每幀的邏輯
    void Update()
    {
        
    }
}
