using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;

/// <summary>
/// Editor utility to upgrade URP Pipeline Assets to the latest version.
/// Fixes BuildFailedException: "UniversalRenderPipelineAsset is not at last version"
/// Usage: Edit > Rendering > Upgrade URP Pipeline Assets
/// </summary>
public static class UpgradeURPAssets
{
    [MenuItem("Edit/Rendering/Upgrade URP Pipeline Assets")]
    public static void UpgradeAllURPAssets()
    {
        string[] guids = AssetDatabase.FindAssets("t:UniversalRenderPipelineAsset");

        if (guids.Length == 0)
        {
            Debug.LogWarning("[URP Upgrade] No UniversalRenderPipelineAsset found in project.");
            return;
        }

        int upgradedCount = 0;

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            var asset = AssetDatabase.LoadAssetAtPath<UniversalRenderPipelineAsset>(path);

            if (asset != null)
            {
                // Access the SerializedObject to trigger internal upgrade logic
                var serializedObject = new SerializedObject(asset);
                serializedObject.Update();

                // Force the asset to mark itself as dirty, triggering OnValidate/upgrade
                EditorUtility.SetDirty(asset);
                upgradedCount++;
                Debug.Log($"[URP Upgrade] Processed: {path}");
            }
        }

        // Also upgrade all UniversalRendererData assets
        string[] rendererGuids = AssetDatabase.FindAssets("t:ScriptableRendererData");
        foreach (string guid in rendererGuids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            var asset = AssetDatabase.LoadAssetAtPath<ScriptableRendererData>(path);

            if (asset != null)
            {
                var serializedObject = new SerializedObject(asset);
                serializedObject.Update();
                EditorUtility.SetDirty(asset);
                Debug.Log($"[URP Upgrade] Processed Renderer: {path}");
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log($"[URP Upgrade] Done! Processed {upgradedCount} URP Pipeline Asset(s). Please try building again.");
        EditorUtility.DisplayDialog(
            "URP Upgrade Complete",
            $"Successfully processed {upgradedCount} URP Pipeline Asset(s).\n\nPlease try Build & Run again.",
            "OK"
        );
    }
}
