using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class XuatHienTrum : MonoBehaviour
{
    [Tooltip("Thời gian chờ trước khi bắt đầu mờ (giây)")]
    public float delayBeforeFade = 5f;

    [Tooltip("Thời gian mờ dần (giây)")]
    public float fadeDuration = 2f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(FadeAlpha());
    }

    IEnumerator FadeAlpha()
    {
        Image image = GetComponent<Image>();
        if (image == null)
        {
            Debug.LogWarning("Không tìm thấy component Image!");
            yield break;
        }

        // Chờ một khoảng thời gian trước khi hiệu ứng mờ dần bắt đầu
        yield return new WaitForSeconds(delayBeforeFade);

        Color currentColor = image.color;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            // Trong code Unity, Alpha đi từ 1.0 (tương đương 255) về 0.0 (tương đương 0)
            float newAlpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            image.color = new Color(currentColor.r, currentColor.g, currentColor.b, newAlpha);
            yield return null; // Đợi đến frame tiếp theo
        }

        // Đảm bảo alpha bằng 0 ở cuối
        image.color = new Color(currentColor.r, currentColor.g, currentColor.b, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
