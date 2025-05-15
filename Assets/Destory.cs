// 引入 Unity 的核心函式庫
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 定義一個繼承自 MonoBehaviour 的類別 Destory（名字打錯了，應為 Destroy）
public class Destory : MonoBehaviour
{
    // 公開整數變數，用來紀錄目前已經觸發幾次碰撞
    public int count;

    // 公開整數變數，設定最大碰撞次數上限（超過這個值後自我銷毀）
    public int MexCount;

    // Unity 的 Start 方法，遊戲開始時呼叫一次。這裡沒做初始化，但可擴充
    void Start()
    {
        
    }

    // Unity 的 Update 方法，每幀呼叫一次。這裡沒使用但可擴充
    void Update()
    {
        
    }

    // 當有碰撞發生時會自動呼叫此方法（碰撞器必須勾選 `Is Trigger = false` 並且有 Rigidbody）
    void OnCollisionEnter(Collision collision)
    {
        // 印出訊息到 Console，幫助除錯：有物體進入碰撞
        Debug.Log("enter");

        // 銷毀碰撞到的物件，延遲 0 秒（立即銷毀）
        Destroy(collision.gameObject, 0);

        // 碰撞計數器加一
        count++; // 相當於 count = count + 1;

        // 如果碰撞次數超過設定值，且遊戲時間已超過 10 秒，就銷毀自身物件
        if (count > MexCount && Time.time > 10f){
            Destroy(gameObject);
        }
    }
}
