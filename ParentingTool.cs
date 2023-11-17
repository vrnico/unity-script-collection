#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

public class ParentingTool : EditorWindow
{
    private string parentName = "NewParent";

    // Add menu named "Parenting Tool" to the Window menu
    [MenuItem("Window/Parenting Tool")]
    public static void ShowWindow()
    {
        //Show existing window instance. If one doesn't exist, make one.
        EditorWindow.GetWindow(typeof(ParentingTool));
    }

    void OnGUI()
    {
        GUILayout.Label("Parenting Options", EditorStyles.boldLabel);
        parentName = EditorGUILayout.TextField("Name of new Parent", parentName);

        if (GUILayout.Button("Parent Selected"))
        {
            ParentSelectedObjects();
        }
    }

    void ParentSelectedObjects()
    {
        GameObject parentObject = GameObject.Find(parentName);

        // If a parent with the name doesn't exist, create it
        if (parentObject == null)
        {
            parentObject = new GameObject(parentName);
            Undo.RegisterCreatedObjectUndo(parentObject, "Create Parent Object");
        }

        // If there are objects selected in the editor
        if (Selection.objects.Length > 0)
        {
            foreach (GameObject go in Selection.gameObjects)
            {
                // Record the undo for the reparenting of the GameObjects
                Undo.SetTransformParent(go.transform, parentObject.transform, "Parent " + go.name);
            }

            // Select the new parent in the editor
            Selection.activeGameObject = parentObject;
        }
        else
        {
            Debug.LogWarning("No objects selected in the editor");
        }
    }
}
#endif
