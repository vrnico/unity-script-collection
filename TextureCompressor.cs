#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class TextureCompressor : EditorWindow
{
    private const int MaxTextureSize = 512;

    [MenuItem("Tools/Compress PNG Textures")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(TextureCompressor), true, "Compress PNG Textures");
    }

    void OnGUI()
    {
        if (GUILayout.Button("Compress PNG Textures"))
        {
            CompressTextures();
        }
    }

    private static void CompressTextures()
    {
        string[] guids = AssetDatabase.FindAssets("t:Texture2D", new[] { "Assets" });
        foreach (string guid in guids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            TextureImporter importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;

            if (importer != null && assetPath.EndsWith(".png", System.StringComparison.OrdinalIgnoreCase))
            {
                Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(assetPath);
                int maxTextureSize = Mathf.Min(MaxTextureSize, Mathf.Max(texture.width, texture.height));
                
                importer.maxTextureSize = maxTextureSize;
                importer.SaveAndReimport();
            }
        }
        Debug.Log("Compression of PNG textures to a max size of " + MaxTextureSize + " is complete.");
    }
}
#endif
