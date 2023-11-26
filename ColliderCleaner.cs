#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

public class ColliderCleaner : MonoBehaviour
{
    [MenuItem("Custom/Remove Specific Mesh Colliders")]
    static void RemoveSpecificMeshColliders()
    {
        // Counter for the number of colliders removed
        int collidersRemovedCount = 0;

        // Find all MeshCollider components in the scene
        MeshCollider[] allColliders = GameObject.FindObjectsOfType<MeshCollider>();

        foreach (MeshCollider collider in allColliders)
        {
            if (collider.sharedMesh != null)
            {
                // Check if the name of the mesh contains "SM" or "ConvexHull"
                if (collider.sharedMesh.name.Contains("SM") || collider.sharedMesh.name.Contains("ConvexHull"))
                {
                    // Log the name and GameObject
                    Debug.Log($"Removing MeshCollider with mesh '{collider.sharedMesh.name}' from GameObject '{collider.gameObject.name}'");

                    // Remove the MeshCollider component from the GameObject
                    DestroyImmediate(collider);
                    collidersRemovedCount++;
                }
            }
        }

        // Log how many colliders were removed
        Debug.Log($"Finished removing Mesh Colliders. Total removed: {collidersRemovedCount}.");
    }
}
#endif
