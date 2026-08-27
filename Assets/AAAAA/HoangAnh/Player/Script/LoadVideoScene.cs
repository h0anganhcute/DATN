using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement; // Thêm thư viện này để quản lý việc chuyển Scene

public class LoadVideoScene : MonoBehaviour
{
    [Header("Video Settings")]
    public VideoPlayer video;

    [Header("Scene Load Settings")]
    // Khai báo biến string để nhập tên Scene trên Inspector, mặc định là "Level-04"
    public string sceneToLoad = "Level-04";

    void Start()
    {
        if (video != null)
        {
            // Đăng ký sự kiện: Khi video chạy xong sẽ tự động gọi hàm OnVideoEnd
            video.loopPointReached += OnVideoEnd;
        }
    }

    // Hàm này sẽ được gọi khi video kết thúc
    void OnVideoEnd(VideoPlayer vp)
    {
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            Debug.Log("Video kết thúc! Đang load Scene: " + sceneToLoad);
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            Debug.LogWarning("Chưa nhập tên Scene để load!");
        }
    }

    // Hủy đăng ký sự kiện khi object bị hủy để dọn dẹp bộ nhớ
    void OnDestroy()
    {
        if (video != null)
        {
            video.loopPointReached -= OnVideoEnd;
        }
    }
}