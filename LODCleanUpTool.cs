using UnityEngine;
using UnityEditor;
using System.Collections.Generic; // Added for List<>
using UnityEditor.SceneManagement; // For marking scene dirty

public class LODCleanupTool : EditorWindow
{
    [MenuItem("Tools/LOD Cleanup Tool")]
    public static void ShowWindow()
    {
        GetWindow(typeof(LODCleanupTool), false, "LOD Cleanup Tool");
    }

    void OnGUI()
    {
        if (GUILayout.Button("Cleanup LODs"))
        {
            CleanupLODs();
        }
    }

    private static void CleanupLODs()
    {
        int deleteCount = 0;

        foreach (GameObject parentObject in Selection.gameObjects)
        {
            // Skip if the GameObject has no children
            if (parentObject.transform.childCount == 0)
                continue;

            // Check if the GameObject is part of a Prefab instance
            if (PrefabUtility.IsPartOfPrefabInstance(parentObject))
            {
                // Unpack the prefab completely before making any changes
                PrefabUtility.UnpackPrefabInstance(parentObject, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
            }

            Transform highestLODTransform = null;
            int highestLODIndex = -1;

            // Find the child with the highest LOD index
            foreach (Transform child in parentObject.transform)
            {
                if (child.name.Contains("LOD"))
                {
                    string[] splitName = child.name.Split('_');
                    string lodPart = splitName[splitName.Length - 1]; // Get the last part after the underscore
                    if (int.TryParse(lodPart.Replace("LOD", ""), out int lodIndex))
                    {
                        // If this child has a higher LOD index, it's the new highest
                        if (lodIndex > highestLODIndex)
                        {
                            highestLODIndex = lodIndex;
                            highestLODTransform = child;
                        }
                    }
                }
            }

            // Create a list of children to iterate over to avoid modifying the collection while iterating
            List<Transform> children = new List<Transform>();
            foreach (Transform child in parentObject.transform)
            {
                children.Add(child);
            }

            // Delete all other LODs except the highest index
            foreach (Transform child in children)
            {
                if (child != highestLODTransform && child.name.Contains("LOD"))
                {
                    DestroyImmediate(child.gameObject);
                    deleteCount++;
                }
            }

            // Remove the LODGroup component from the parent object
            RemoveLODGroup(parentObject);
        }

        // Refresh the editor
        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());

        Debug.Log($"LOD cleanup complete. Deleted {deleteCount} objects.");
    }

    private static void RemoveLODGroup(GameObject gameObject)
    {
        LODGroup lodGroup = gameObject.GetComponent<LODGroup>();
        if (lodGroup != null)
        {
            DestroyImmediate(lodGroup);
            Debug.Log($"LODGroup component removed from {gameObject.name}.");
        }
    }
}
