# Unity Script Collection

This collection contains various Unity Editor scripts. Explanations for specific tools will be added at my convenience.

## Delete Specific Colliders Tool

- **Open the Tool**: In Unity, navigate to `Tools > Delete Specific Colliders in Scene View`.
- **Select Collider Type**: Choose the type of collider you want to delete (e.g., BoxCollider, SphereCollider) from the dropdown menu.
- **Delete Colliders**: Click the "Delete Colliders in View" button to remove all colliders of the selected type within the current scene view.

## LOD Cleanup Tool

- **Open the Tool**: In Unity, navigate to `Tools > LOD Cleanup Tool`.
- **Functionality**: This tool cleans up Level of Detail (LOD) objects within a selected GameObject. When you run it, it will:
  - Unpack any prefab instances completely.
  - Identify and keep the child GameObject with the highest LOD index (e.g., LOD2 in a set of LOD0, LOD1, LOD2).
  - Delete all other LOD levels (e.g., LOD0, LOD1) except for the highest one.
  - Remove the `LODGroup` component from the parent GameObject.
- **Usage**: Select the GameObject(s) with LODs in the scene, then click "Cleanup LODs" in the tool window.
- **Note**: The script marks the scene as dirty after changes, prompting to save the scene if necessary.

