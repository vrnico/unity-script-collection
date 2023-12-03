#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System;

public class DeleteCollidersInView : EditorWindow
{
    private Type selectedColliderType = typeof(BoxCollider); // Default to BoxCollider

    [MenuItem("Tools/Delete Specific Colliders in Scene View")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(DeleteCollidersInView), true, "Delete Specific Colliders");
    }

    void OnGUI()
    {
        GUILayout.Label("Select Collider Type to Delete", EditorStyles.boldLabel);

        // Dropdown to select collider type
        string[] colliderTypes = { "BoxCollider", "SphereCollider", "CapsuleCollider", "MeshCollider" };
        int selectedIndex = Array.IndexOf(colliderTypes, selectedColliderType.Name);
        selectedIndex = EditorGUILayout.Popup("Collider Type", selectedIndex, colliderTypes);
        selectedColliderType = Type.GetType($"UnityEngine.{colliderTypes[selectedIndex]}, UnityEngine");

        if (GUILayout.Button("Delete Colliders in View"))
        {
            DeleteColliders();
        }
    }

    private void DeleteColliders()
    {
        Camera sceneCamera = SceneView.lastActiveSceneView.camera;
        if (sceneCamera == null)
        {
            Debug.LogError("No active scene view camera found.");
            return;
        }

        var colliders = GameObject.FindObjectsOfType(selectedColliderType);
        int deleteCount = 0;

        foreach (Collider collider in colliders)
        {
            if (IsColliderInView(collider, sceneCamera))
            {
                DestroyImmediate(collider.gameObject);
                deleteCount++;
            }
        }

        Debug.Log($"Deleted {deleteCount} {selectedColliderType.Name} colliders in the scene view.");
    }

    private static bool IsColliderInView(Collider collider, Camera camera)
    {
        Vector3 pointOnScreen = camera.WorldToViewportPoint(collider.transform.position);
        // Check if the point is in the camera's view frustum
        return pointOnScreen.z > 0 && pointOnScreen.x > 0 && pointOnScreen.x < 1 && pointOnScreen.y > 0 && pointOnScreen.y < 1;
    }
}
#endif
