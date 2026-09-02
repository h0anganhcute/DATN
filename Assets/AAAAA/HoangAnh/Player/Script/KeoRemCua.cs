using System.Collections;
using UnityEngine;

public class KeoRemCua : MonoBehaviour
{
    public GameObject RemCua;

    void Start()
    {
        // Kiểm tra xem đã gán RemCua chưa để tránh lỗi NullReferenceException
        if (RemCua != null)
        {
            // Gọi Coroutine chạy trong 5 giây
            StartCoroutine(AnimateRemCua(3f));
        }
        else
        {
            Debug.LogWarning("Bạn chưa gán GameObject RemCua trong Inspector!");
        }
    }

    void Update()
    {

    }

    // Coroutine xử lý thay đổi thông số theo thời gian
    IEnumerator AnimateRemCua(float duration)
    {
        // Lưu lại vị trí và tỷ lệ ban đầu
        Vector3 startPosition = RemCua.transform.position;
        Vector3 startScale = RemCua.transform.localScale;

        // Tạo vị trí và tỷ lệ mục tiêu theo yêu cầu
        Vector3 targetPosition = new Vector3(startPosition.x, 3.0129f, startPosition.z);
        Vector3 targetScale = new Vector3(startScale.x, 0.2256f, startScale.z);

        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            // Tính toán tỷ lệ thời gian đã trôi qua (từ 0 đến 1)
            float t = timeElapsed / duration;

            // Dùng Vector3.Lerp để chuyển đổi mượt mà từ giá trị ban đầu đến mục tiêu
            RemCua.transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            RemCua.transform.localScale = Vector3.Lerp(startScale, targetScale, t);

            timeElapsed += Time.deltaTime;

            // Chờ đến frame tiếp theo rồi mới chạy tiếp vòng lặp
            yield return null;
        }

        // Đảm bảo sau 5 giây thì gán chính xác giá trị cuối cùng
        RemCua.transform.position = targetPosition;
        RemCua.transform.localScale = targetScale;
    }
}