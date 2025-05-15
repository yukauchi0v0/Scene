using System.Collections;
using System.Collections.Generic;
using System.Threading; // ⚠️ 在 Unity 中通常不需要用到 Threading，請小心使用
using UnityEditor.Experimental.GraphView; // ⚠️ 這是編輯器專用命名空間，不建議在執行時腳本中使用
using UnityEngine;

public class ObjectGenerator : MonoBehaviour
{
    // 預製物件（預設生成用）
    public GameObject prefab;

    // 放置物體的基準點位置
    public Transform placement;

    // 浮動距離，用於生成位置偏移
    public float drift = 2.2f;

    // 額外的單獨預製物件
    public GameObject prefabA;
    public GameObject prefabB;
    public GameObject prefabC;

    // 用來存放多個預製物件的陣列
    public GameObject[] prefabs;

    // 時間控制變數
    public float Timer;
    public float interval;     // 每次生成的間隔時間
    public float spawnTime;    // 下一次可生成的時間

    void Start()
    {
        // 在一開始建立 5 個預製物件，往上排列
        for (int num = 0; num < 5; num++)
        {
            Instantiate(prefab, placement.position + new Vector3(0, num * 2.2f, 0), Quaternion.identity);
        }
    }

    void Update()
    {
        // 更新目前遊戲運行時間
        Timer = Time.time;

        // 每過 interval 秒，自動生成一個 prefabB
        if (Timer > spawnTime)
        {
            Instantiate(prefabB, placement.position, Quaternion.identity);
            spawnTime = Timer + interval;
        }

        // 當按下 J 鍵時：產生 5x5 排列的物件，並有隨機偏移
        if (Input.GetKeyDown(KeyCode.J))
        {
            for (int X = 0; X < 5; X++)
            {
                for (int Z = 0; Z < 5; Z++)
                {
                    drift = Random.Range(1, 6); // 每次產生都重新設定 drift
                    Instantiate(prefabs[0], placement.position + new Vector3(X * drift, 10, Z * drift), Quaternion.identity);
                }
            }
        }

        // ⚠️ 錯誤：這段邏輯會在持續按住 J 鍵時，每幀都產生大量物件
        if (Input.GetKey(KeyCode.J))
        {
            for (int n = 0; n <= prefabs.Length - 1; n++)
            {
                Instantiate(prefabs[n], placement.position, Quaternion.identity);
                Debug.Log("陣列長度" + prefabs.Length);
            }
            spawnTime = Timer + interval;
        }

        // ⚠️ 重複條件！這三個條件都判斷 KeyUp J，因此後兩個會被忽略（只能執行一次）
        if (Input.GetKeyUp(KeyCode.J))
        {
            Instantiate(prefabs[2], placement.position, Quaternion.identity);
        }

        if (Input.GetKeyUp(KeyCode.J)) // ⚠️ 重複 KeyCode.J，這一段永遠不會執行
        {
            Instantiate(prefabs[3], placement.position, Quaternion.identity);
        }

        // 當放開 K 鍵時，如果時間條件允許，就生成所有陣列中的物件（從索引 1 開始）
        if (Input.GetKeyUp(KeyCode.K))
        {
            if (Timer > spawnTime)
            {
                for (int y = 1; y <= prefabs.Length - 1; y++)
                {
                    Instantiate(prefabs[y], placement.position, Quaternion.identity);
                    Debug.Log("陣列長度" + prefabs.Length);
                }
            }
        }
    }
}
