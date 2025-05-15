using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DNAAnimator : MonoBehaviour
{
    [Header("基本設定")]
    public GameObject blockPrefab;
    public int rows = 20;
    public float spacingY = 1.1f;
    public float rowWidth = 10f;
    public float rowDepth = 2f;

    [Header("螺旋參數")]
    public float spiralRadius = 3f;
    public float angleStep = 18f;

    [Header("段落間隙")]
    public float gap = 0.1f;

    private List<GameObject> rowGroups = new List<GameObject>();

    void Start()
    {
        StartCoroutine(Sequence());
    }

    IEnumerator Sequence()
    {
        // === 階段 1：生成 rows 列方塊（直線堆疊）===
        for (int i = 0; i < rows; i++)
        {
            GameObject rowGroup = new GameObject("Row_" + i);
            rowGroup.transform.parent = this.transform;

            // 初始直立堆疊
            rowGroup.transform.localPosition = new Vector3(0, i * spacingY, 0);
            rowGroup.transform.localRotation = Quaternion.identity;

            GameObject block = Instantiate(blockPrefab, rowGroup.transform);
            block.transform.localScale = new Vector3(rowWidth, 1, rowDepth);
            block.transform.localPosition = Vector3.zero;

            rowGroups.Add(rowGroup);
        }

        yield return new WaitForSeconds(5f);

        // === 階段 2：每列原地旋轉 ===
        foreach (var row in rowGroups)
        {
            row.AddComponent<Spin>().rotationSpeed = new Vector3(0, 60f, 0);
        }

        yield return new WaitForSeconds(5f);

        // === 階段 3：偶數層進行三段分割 ===
        for (int i = 0; i < rowGroups.Count; i++)
        {
            var row = rowGroups[i];
            if (i % 2 != 0) continue;

            Transform oldBlock = row.transform.GetChild(0);
            Destroy(oldBlock.gameObject);

            float[] ratios = { 1f, 2f, 1f };
            float totalRatio = 4f;
            float effectiveWidth = rowWidth - 2 * gap;

            float[] widths = new float[3];
            for (int j = 0; j < 3; j++)
                widths[j] = (ratios[j] / totalRatio) * effectiveWidth;

            float offset = -rowWidth / 2;
            for (int j = 0; j < 3; j++)
            {
                GameObject segment = Instantiate(blockPrefab, row.transform);
                segment.transform.localScale = new Vector3(widths[j], 1, rowDepth);

                offset += widths[j] / 2;
                segment.transform.localPosition = new Vector3(offset, 0, 0);
                offset += widths[j] / 2 + gap;
            }
        }

        yield return new WaitForSeconds(3f); // 延遲後再螺旋化

        // === 階段 4：排列成螺旋樓梯 ===
        for (int i = 0; i < rowGroups.Count; i++)
        {
            GameObject row = rowGroups[i];

            float angle = i * angleStep;
            float rad = angle * Mathf.Deg2Rad;

            float x = Mathf.Cos(rad) * spiralRadius;
            float z = Mathf.Sin(rad) * spiralRadius;
            float y = i * spacingY;

            row.transform.localPosition = new Vector3(x, y, z);
            row.transform.localRotation = Quaternion.Euler(0, angle, 0); // 面向外圈
        }
    }
}

// === 原地旋轉組件 ===
public class Spin : MonoBehaviour
{
    public Vector3 rotationSpeed = new Vector3(0, 60f, 0);
    void Update() => transform.Rotate(rotationSpeed * Time.deltaTime);
}
