#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
using UnityEditor.SceneManagement;
using System.Collections.Generic;

public class SceneSizeCalculator : EditorWindow
{
    private const float BuildCompressionRatio = 0.35f; // VRChat scene builder compression ratio
    private const int TopAssetsToDisplay = 10; // Number of top heavy assets to display

    [MenuItem("Tools/Calculate Scene Size")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(SceneSizeCalculator), true, "Scene Size Calculator");
    }

    void OnGUI()
    {
        if (GUILayout.Button("Calculate Size of Current Scene"))
        {
            CalculateSceneSize();
        }
    }

    private static void CalculateSceneSize()
    {
        var scene = EditorSceneManager.GetActiveScene();
        string[] dependencies = AssetDatabase.GetDependencies(scene.path);
        long totalSizeBytes = 0;
        List<KeyValuePair<string, long>> assetSizes = new List<KeyValuePair<string, long>>();

        foreach (string dependencyPath in dependencies)
        {
            if (!string.IsNullOrEmpty(dependencyPath))
            {
                FileInfo fileInfo = new FileInfo(dependencyPath);
                if (fileInfo.Exists)
                {
                    long fileSize = fileInfo.Length;
                    totalSizeBytes += fileSize;
                    assetSizes.Add(new KeyValuePair<string, long>(dependencyPath, fileSize));
                }
            }
        }

        // Order the assets by size in descending order and take the top heaviest assets
        List<KeyValuePair<string, long>> heaviestAssets = assetSizes.OrderByDescending(pair => pair.Value)
                                                                     .Take(TopAssetsToDisplay)
                                                                     .ToList();

        float totalSizeMB = totalSizeBytes / (1024f * 1024f);
        float approximateBuildSizeMB = totalSizeMB * BuildCompressionRatio;

        // Log the total size and the approximate build size
        Debug.Log("Total size of scene " + scene.name + " is approximately: " + totalSizeMB.ToString("F2") + " MB");
        Debug.Log("Approximate build size after compression: " + approximateBuildSizeMB.ToString("F2") + " MB");

        // Log the heaviest assets
        Debug.Log("Heaviest assets in the scene:");
        foreach (KeyValuePair<string, long> asset in heaviestAssets)
        {
            Debug.Log(asset.Key + ": " + (asset.Value / (1024f * 1024f)).ToString("F2") + " MB");
        }

        // Log if the size is over the limit
        if (totalSizeMB > 100f)
        {
            float overSizeMB = totalSizeMB - 100f;
            Debug.LogWarning("Scene size is over 100MB by approximately: " + overSizeMB.ToString("F2") + " MB");
        }
        else
        {
            Debug.Log("Scene size is under 100MB limit by approximately: " + (100f - totalSizeMB).ToString("F2") + " MB");
        }
    }
}
#endif
