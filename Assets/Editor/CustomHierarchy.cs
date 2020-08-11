using System.Linq;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class CustomHierarchy : MonoBehaviour
{
    private static Vector2 offset = new Vector2(0, 2);

    static CustomHierarchy()
    {
        EditorApplication.hierarchyWindowItemOnGUI += HandleHierarchyWindowItemOnGUI;
    }

    private static void HandleHierarchyWindowItemOnGUI(int instanceID, Rect selectionRect)
    {
        var gameObject = EditorUtility.InstanceIDToObject(instanceID) as GameObject;

        // Sceneはreturn
        if (gameObject == null)
            return;

        if (gameObject.name.Contains("---"))
        {
            Color fontColor = Color.white;
            Color backgroundColor = new Color(0.3f, 0.3f, 0.3f); //默认背景色

            var prefabType = PrefabUtility.GetPrefabType(gameObject);
            if (prefabType == PrefabType.None)
            {
                if (Selection.instanceIDs.Contains(instanceID))
                {
                    fontColor = new Color(1, 1, 1, 0);
                    backgroundColor = new Color(0, 0, 0, 0); //选中颜色
                }
                Rect offsetRect = new Rect(selectionRect.position + offset, selectionRect.size);
                EditorGUI.DrawRect(selectionRect, backgroundColor);
                string showName = gameObject.name.Substring(3, gameObject.name.Length - 3);
                EditorGUI.LabelField(offsetRect, showName, new GUIStyle()
                {
                    normal = new GUIStyleState() { textColor = fontColor },
                    fontStyle = FontStyle.Bold
                });
            }
        }
    }
}
