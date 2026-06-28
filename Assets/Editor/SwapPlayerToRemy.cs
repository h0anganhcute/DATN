using UnityEngine;
using UnityEditor;

/// <summary>
/// Editor script tự động thay model Player thành Remy.
/// Sử dụng: Menu Unity → Tools → Thay Model Player thanh Remy
/// </summary>
public class SwapPlayerToRemy
{
    [MenuItem("Tools/Thay Model Player thanh Remy")]
    static void Execute()
    {
        string remyPath = "Assets/AAAAA/HoangAnh/Player/Remy@Punching.fbx";

        // ============================================================
        // BƯỚC 1: Đảm bảo Remy được import là Humanoid
        // ============================================================
        ModelImporter importer = AssetImporter.GetAtPath(remyPath) as ModelImporter;
        if (importer == null)
        {
            EditorUtility.DisplayDialog("Lỗi",
                "Không tìm thấy file:\n" + remyPath, "OK");
            return;
        }

        bool needsReimport = false;

        if (importer.animationType != ModelImporterAnimationType.Human)
        {
            importer.animationType = ModelImporterAnimationType.Human;
            importer.avatarSetup = ModelImporterAvatarSetup.CreateFromThisModel;
            needsReimport = true;
        }

        if (needsReimport)
        {
            importer.SaveAndReimport();
            Debug.Log("[SwapModel] Đã reimport Remy với Humanoid rig");
        }

        // ============================================================
        // BƯỚC 2: Tìm PlayerArmature trong scene
        // ============================================================
        GameObject player = GameObject.Find("PlayerArmature");
        if (player == null)
        {
            EditorUtility.DisplayDialog("Lỗi",
                "Không tìm thấy PlayerArmature trong scene!\n" +
                "Hãy mở scene có chứa PlayerArmature trước.", "OK");
            return;
        }

        Undo.RegisterFullObjectHierarchyUndo(player, "Swap Player Model to Remy");

        // ============================================================
        // BƯỚC 3: Load Remy assets (Prefab + Avatar)
        // ============================================================
        GameObject remyPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(remyPath);
        if (remyPrefab == null)
        {
            EditorUtility.DisplayDialog("Lỗi", "Không thể load Remy model!", "OK");
            return;
        }

        Avatar remyAvatar = null;
        foreach (Object obj in AssetDatabase.LoadAllAssetsAtPath(remyPath))
        {
            if (obj is Avatar a)
            {
                remyAvatar = a;
                break;
            }
        }

        if (remyAvatar == null)
        {
            EditorUtility.DisplayDialog("Lỗi",
                "Không tìm thấy Humanoid Avatar trong Remy FBX!\n" +
                "Có thể model chưa được import đúng cách.", "OK");
            return;
        }

        // ============================================================
        // BƯỚC 4: Đổi Avatar trong Animator
        // ============================================================
        Animator animator = player.GetComponent<Animator>();
        if (animator != null)
        {
            animator.avatar = remyAvatar;
            EditorUtility.SetDirty(animator);
            Debug.Log("[SwapModel] ✓ Đã swap Avatar → Remy");
        }

        // ============================================================
        // BƯỚC 5: Tắt Geometry cũ (mesh nhân vật gốc)
        // ============================================================
        Transform geometry = player.transform.Find("Geometry");
        if (geometry != null)
        {
            geometry.gameObject.SetActive(false);
            EditorUtility.SetDirty(geometry.gameObject);
            Debug.Log("[SwapModel] ✓ Đã tắt Geometry cũ");
        }

        // ============================================================
        // BƯỚC 6: Dọn dẹp model Remy cũ (nếu đã thêm thủ công trước đó)
        // ============================================================
        // Xóa các child thừa từ lần thử trước
        string[] oldPartNames = { "Body", "Bottoms", "Eyelashes", "Eyes", "Hair", "Tops" };
        foreach (string partName in oldPartNames)
        {
            Transform part = player.transform.Find(partName);
            if (part != null)
            {
                Undo.DestroyObjectImmediate(part.gameObject);
                Debug.Log("[SwapModel] Đã xóa child thừa: " + partName);
            }
        }

        // Xóa skeleton mixamorig cũ nếu bị kéo vào thủ công
        Transform oldMixamoHips = player.transform.Find("mixamorig:Hips");
        if (oldMixamoHips != null)
        {
            Undo.DestroyObjectImmediate(oldMixamoHips.gameObject);
            Debug.Log("[SwapModel] Đã xóa mixamorig:Hips thừa");
        }

        // Xóa RemyModel cũ nếu script đã chạy trước đó
        Transform existingRemy = player.transform.Find("RemyModel");
        if (existingRemy != null)
        {
            Undo.DestroyObjectImmediate(existingRemy.gameObject);
            Debug.Log("[SwapModel] Đã xóa RemyModel cũ");
        }

        // ============================================================
        // BƯỚC 7: Đảm bảo PlayerCameraRoot đang bật
        // ============================================================
        Transform cameraRoot = player.transform.Find("PlayerCameraRoot");
        if (cameraRoot != null && !cameraRoot.gameObject.activeSelf)
        {
            cameraRoot.gameObject.SetActive(true);
            EditorUtility.SetDirty(cameraRoot.gameObject);
            Debug.Log("[SwapModel] ✓ Đã bật lại PlayerCameraRoot");
        }

        // ============================================================
        // BƯỚC 8: Thêm Remy model vào PlayerArmature
        // ============================================================
        GameObject remy = PrefabUtility.InstantiatePrefab(remyPrefab, player.transform) as GameObject;
        if (remy == null)
        {
            // Fallback nếu InstantiatePrefab không hỗ trợ FBX
            remy = Object.Instantiate(remyPrefab, player.transform);
        }

        Undo.RegisterCreatedObjectUndo(remy, "Add Remy Model");
        remy.name = "RemyModel";
        remy.transform.localPosition = Vector3.zero;
        remy.transform.localRotation = Quaternion.identity;
        remy.transform.localScale = Vector3.one;
        Debug.Log("[SwapModel] ✓ Đã thêm RemyModel vào PlayerArmature");

        // ============================================================
        // BƯỚC 9: Xóa Animator thừa trên RemyModel (tránh xung đột)
        // ============================================================
        Animator remyAnimator = remy.GetComponent<Animator>();
        if (remyAnimator != null)
        {
            Object.DestroyImmediate(remyAnimator);
            Debug.Log("[SwapModel] ✓ Đã xóa Animator thừa trên RemyModel");
        }

        // ============================================================
        // BƯỚC 10: Fix materials cho URP (tránh hiện màu hồng)
        // ============================================================
        Shader urpLit = Shader.Find("Universal Render Pipeline/Lit");
        if (urpLit != null)
        {
            int fixedCount = 0;
            foreach (Renderer renderer in remy.GetComponentsInChildren<Renderer>(true))
            {
                Material[] mats = renderer.sharedMaterials;
                for (int i = 0; i < mats.Length; i++)
                {
                    if (mats[i] != null && mats[i].shader != null)
                    {
                        string shaderName = mats[i].shader.name;
                        if (shaderName.Contains("Standard") || shaderName.Contains("Autodesk"))
                        {
                            // Lưu texture trước khi đổi shader
                            Texture mainTex = mats[i].mainTexture;
                            Color color = mats[i].HasProperty("_Color")
                                ? mats[i].color
                                : Color.white;

                            mats[i].shader = urpLit;

                            // Khôi phục texture và màu
                            if (mainTex != null)
                                mats[i].SetTexture("_BaseMap", mainTex);
                            mats[i].SetColor("_BaseColor", color);

                            EditorUtility.SetDirty(mats[i]);
                            fixedCount++;
                        }
                    }
                }
            }
            if (fixedCount > 0)
                Debug.Log($"[SwapModel] ✓ Đã fix {fixedCount} materials cho URP");
        }

        // ============================================================
        // BƯỚC 11: Lưu thay đổi
        // ============================================================
        EditorUtility.SetDirty(player);
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
            UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene());

        Debug.Log("[SwapModel] ========================================");
        Debug.Log("[SwapModel] ✅ HOÀN TẤT! Model đã được thay thành Remy.");
        Debug.Log("[SwapModel] Nhấn Ctrl+S để lưu, rồi Play để kiểm tra.");
        Debug.Log("[SwapModel] ========================================");

        EditorUtility.DisplayDialog("✅ Thành công!",
            "Đã thay model Player thành Remy!\n\n" +
            "• Avatar: Remy Humanoid ✓\n" +
            "• Mesh cũ: Đã ẩn ✓\n" +
            "• Model Remy: Đã thêm ✓\n" +
            "• Materials URP: Đã fix ✓\n\n" +
            "→ Nhấn Ctrl+S để lưu scene\n" +
            "→ Nhấn Play để kiểm tra!", "OK");
    }
}
