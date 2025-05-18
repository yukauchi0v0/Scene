using UnityEngine;
using UnityEditor;

public class ReplaceWithPrefabTool : EditorWindow
{
    GameObject prefabToReplace;

    [MenuItem("Tools/Replace Selected with Prefab")]
    public static void ShowWindow()
    {
        GetWindow<ReplaceWithPrefabTool>("Replace With Prefab");
    }

    private void OnGUI()
    {
        GUILayout.Label("選擇要替換的 Prefab", EditorStyles.boldLabel);
        prefabToReplace = (GameObject)EditorGUILayout.ObjectField("Prefab", prefabToReplace, typeof(GameObject), false);

        if (GUILayout.Button("替換選中的物件"))
        {
            ReplaceSelectedObjects();
        }
    }

    private void ReplaceSelectedObjects()
    {
        if (prefabToReplace == null)
        {
            Debug.LogError("請指定一個 Prefab 來替換！");
            return;
        }

        foreach (GameObject obj in Selection.gameObjects)
        {
            Transform parent = obj.transform.parent;
            Vector3 position = obj.transform.position;
            Quaternion rotation = obj.transform.rotation;
            Vector3 scale = obj.transform.localScale;

            GameObject newObj = (GameObject)PrefabUtility.InstantiatePrefab(prefabToReplace, obj.scene);
            newObj.transform.SetParent(parent);
            newObj.transform.position = position;
            newObj.transform.rotation = rotation;
            newObj.transform.localScale = scale;

            Undo.RegisterCreatedObjectUndo(newObj, "Replace with Prefab");
            Undo.DestroyObjectImmediate(obj);
        }

        Debug.Log("已將選中的物件替換成 Prefab！");
    }
}
