// 引入 Unity 所需的命名空間
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 定義一個繼承自 MonoBehaviour 的類別 myboxman，這樣才能掛在 Unity 的 GameObject 上使用
public class myboxman : MonoBehaviour
{
    // 公開變數，可在 Inspector 中調整，表示物件的旋轉速度
    public float Speed;

    // 每次增加速度的量
    public float AddSpeed = 0.01f;

    // 最大速度限制
    public float MexSpeed = 10;

    // 預設的增加速度值，用來在某些情況下重設 AddSpeed
    public float AddSpeedDefault = 0.01f;

    // 整數變數，用來控制旋轉方向（正 1、負 -1、或 0）
    public int Reveres;

    // 旋轉方向向量，可自訂方向（例如 Vector3.up 代表繞 Y 軸旋轉）
    public Vector3 rotateDirection;

    // 開關布林變數，用來作為控制方向判斷的邏輯條件
    public bool flag;

    // 另一個布林變數，也可以作為方向控制的條件
    public bool otherDir;

    // Start 是 Unity 生命週期方法之一，遊戲啟動時呼叫一次，常用來初始化變數
    void Start()
    {
        // 此處目前未做初始化設定
    }

    // Update 是 Unity 每幀會呼叫一次的方法，常用來處理持續性的邏輯，例如輸入控制、移動等
    void Update()
    {
        /*
        // 這段註解掉的程式碼原本是根據 flag 和 otherDir 的值來改變旋轉方向
        if(flag){
            Reveres = 0; // 當 flag 為 true，Reveres 設為 0（停止旋轉）
        }
        else if (otherDir){
            Reveres = 1; // 當 otherDir 為 true，Reveres 設為 1（正向旋轉）
        }
        else {
            Reveres = -1; // 否則反向旋轉
        }
        */

        // 每一幀都將速度加上 AddSpeed
        Speed = Speed + AddSpeed;

        // 限制速度不能超過最大值
        if(Speed > MexSpeed){
            Speed = MexSpeed;
        }

        // 當按下空白鍵（space）時，反轉旋轉方向
        if (Input.GetKeyDown("space")){
            Reveres = Reveres * -1;
        }

        // 當持續按著 O 鍵時，啟用爆速模式：
        // 把 AddSpeed 設為最大速度，Speed 強制設為 1（或啟動瞬間旋轉）
        if (Input.GetKey(KeyCode.O)){
            AddSpeed = MexSpeed;
            Speed = 1;
        }
        else {
            // 若沒按 O 鍵，則將加速值重設為預設值
            AddSpeed = AddSpeedDefault;
        }

        // 當按著 P 鍵時，速度變慢（用來手動控制速度為 0.1）
        if (Input.GetKey(KeyCode.P)){
            Speed = 0.1f;
        }

        // 最終根據旋轉方向、速度與方向倍率 (Reveres) 進行旋轉
        transform.Rotate(rotateDirection * Speed * Reveres /* * Time.deltaTime */);
        // 提示：如果你希望旋轉效果與幀率無關，建議取消 Time.deltaTime 的註解
    }
}
