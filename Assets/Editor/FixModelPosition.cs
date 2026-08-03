using UnityEngine;
using UnityEditor;

public class FixModelPosition : EditorWindow
{
    [MenuItem("Tools/Fix RongBang Model Position")]
    static void FixPosition()
    {
        // Tìm object untitled1 trong scene
        GameObject model = GameObject.Find("untitled1");
        
        if (model == null)
        {
            // Thử tìm theo tên khác
            GameObject[] allObjects = GameObject.FindObjectsByType<GameObject>(FindObjectsSortMode.None);
            foreach (var obj in allObjects)
            {
                if (obj.name.Contains("untitled1") || obj.name.Contains("em124"))
                {
                    model = obj;
                    break;
                }
            }
        }

        if (model == null)
        {
            EditorUtility.DisplayDialog("Không tìm thấy", 
                "Không tìm thấy object 'untitled1' trong scene.\nHãy chọn object cần sửa trong Hierarchy rồi dùng Tools > Fix Selected Model Position.", 
                "OK");
            return;
        }

        FixModelTransform(model);
    }

    [MenuItem("Tools/Fix Selected Model Position")]
    static void FixSelectedPosition()
    {
        GameObject model = Selection.activeGameObject;
        
        if (model == null)
        {
            EditorUtility.DisplayDialog("Chưa chọn object", 
                "Hãy chọn model cần sửa trong Hierarchy trước!", 
                "OK");
            return;
        }

        FixModelTransform(model);
    }

    static void FixModelTransform(GameObject model)
    {
        Undo.RecordObject(model.transform, "Fix Model Position");

        // Lưu vị trí X, Z hiện tại
        float currentX = model.transform.position.x;
        float currentZ = model.transform.position.z;

        // Reset rotation về (0, 0, 0)
        model.transform.rotation = Quaternion.identity;

        // Tìm bounds của model để đặt chân xuống sàn
        Renderer[] renderers = model.GetComponentsInChildren<Renderer>();
        
        if (renderers.Length > 0)
        {
            // Tính bounding box tổng hợp
            Bounds combinedBounds = renderers[0].bounds;
            for (int i = 1; i < renderers.Length; i++)
            {
                combinedBounds.Encapsulate(renderers[i].bounds);
            }

            // Tính offset Y để đáy model nằm trên mặt sàn (Y=0)
            // hoặc trên vị trí terrain gần nhất
            float bottomY = combinedBounds.min.y;
            float groundY = 0f;

            // Thử raycast xuống để tìm mặt sàn/terrain
            RaycastHit hit;
            Vector3 rayOrigin = new Vector3(model.transform.position.x, combinedBounds.max.y + 10f, model.transform.position.z);
            if (Physics.Raycast(rayOrigin, Vector3.down, out hit, 1000f))
            {
                groundY = hit.point.y;
                Debug.Log($"[FixModel] Tìm thấy mặt sàn tại Y = {groundY} (object: {hit.collider.gameObject.name})");
            }
            else
            {
                Debug.Log("[FixModel] Không tìm thấy mặt sàn bằng raycast, sử dụng Y = 0");
            }

            // Dịch model lên để đáy chạm sàn
            float offsetY = groundY - bottomY;
            model.transform.position = new Vector3(currentX, model.transform.position.y + offsetY, currentZ);

            Debug.Log($"[FixModel] Đã sửa model '{model.name}':");
            Debug.Log($"  - Rotation: (0, 0, 0)");
            Debug.Log($"  - Position: {model.transform.position}");
            Debug.Log($"  - Model bounds: min={combinedBounds.min}, max={combinedBounds.max}");
            Debug.Log($"  - Offset Y applied: {offsetY}");

            EditorUtility.DisplayDialog("Đã sửa xong!", 
                $"Model '{model.name}' đã được căn chỉnh:\n" +
                $"- Rotation: (0, 0, 0)\n" +
                $"- Position: {model.transform.position}\n" +
                $"- Đáy model đặt tại Y = {groundY}\n\n" +
                $"Nếu vẫn chưa đúng, hãy chỉnh Position Y trong Inspector.",
                "OK");
        }
        else
        {
            // Không tìm thấy renderer, chỉ reset rotation
            model.transform.position = new Vector3(currentX, 0, currentZ);
            
            EditorUtility.DisplayDialog("Đã reset!", 
                $"Model '{model.name}' đã reset rotation về (0,0,0) và Y = 0.\n" +
                "Không tìm thấy Renderer để tính bounds.\n" +
                "Hãy chỉnh Position Y thủ công trong Inspector.",
                "OK");
        }

        // Đánh dấu scene đã thay đổi
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene());
    }
}
