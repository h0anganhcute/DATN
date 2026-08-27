using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadSceneCuoi : MonoBehaviour
{

    public RectTransform MatTrenRectTransform;
    public RectTransform MatDuoiRectTransform;

    private float elapsedTime = 0f;
    private float duration = 5f;
    
    private float startHeightTren;
    private float startHeightDuoi;
    private float targetHeight = 300f;

    void OnEnable()
    {
        // Reset thời gian khi object được bật
        elapsedTime = 0f;
        
        // Lấy Height ban đầu
        if (MatTrenRectTransform != null)
            startHeightTren = MatTrenRectTransform.sizeDelta.y;
            
        if (MatDuoiRectTransform != null)
            startHeightDuoi = MatDuoiRectTransform.sizeDelta.y;
            

    }

    void Update()
    {
        if (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            
            // 1. Tăng Height lên 300
            if (MatTrenRectTransform != null)
                MatTrenRectTransform.sizeDelta = new Vector2(MatTrenRectTransform.sizeDelta.x, Mathf.Lerp(startHeightTren, targetHeight, t));
                
            if (MatDuoiRectTransform != null)
                MatDuoiRectTransform.sizeDelta = new Vector2(MatDuoiRectTransform.sizeDelta.x, Mathf.Lerp(startHeightDuoi, targetHeight, t));
                

            
            // 3. Chuyển scene khi hoàn thành
            if (elapsedTime >= duration)
            {
                SceneManager.LoadScene("VideoScene2");
            }
        }
    }
}
