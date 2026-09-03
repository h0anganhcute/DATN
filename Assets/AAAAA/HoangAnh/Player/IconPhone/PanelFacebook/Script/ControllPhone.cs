using UnityEngine;
using System.Collections;

public class ControllPhone : MonoBehaviour
{
    public RectTransform Phone;
    public float moveDuration = 1.5f;
    private bool isMoving = false;
    private bool isOpen = false;
    private float initialPosY;
    public GameObject Facebook;
    public RectTransform faceBookPanel;

    private bool isFbPanelMoving = false;
    private bool isFbPanelOpen = false;
    private float initialFbTop;
    private float initialFbBottom;
    public GameObject Iphone;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (Phone != null)
        {
            // Lưu lại vị trí Y ban đầu để khi tắt sẽ quay về đây
            initialPosY = Phone.anchoredPosition.y;
        }

        if (faceBookPanel != null)
        {
            // Top là -offsetMax.y, Bottom là offsetMin.y
            initialFbTop = -faceBookPanel.offsetMax.y;
            initialFbBottom = faceBookPanel.offsetMin.y;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && !isMoving)
        {
            if (!isOpen)
            {
                // Trạng thái đang tắt -> Bật lên và di chuyển tới Y = -175
                StartCoroutine(TogglePhone(true, -175f, moveDuration));
            }
            else
            {
                // Trạng thái đang bật -> Di chuyển về vị trí ban đầu rồi mới tắt
                StartCoroutine(TogglePhone(false, initialPosY, moveDuration));
            }
        }

        // Bật / tắt GameObject Facebook bằng phím 1
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (Facebook != null)
            {
                Facebook.SetActive(!Facebook.activeSelf);
            }
        }

        // Điều khiển faceBookPanel bằng phím 2 khi Facebook đang được bật
        if (Input.GetKeyDown(KeyCode.Alpha2) && !isFbPanelMoving)
        {
            if (Facebook != null && Facebook.activeSelf)
            {
                if (!isFbPanelOpen)
                {
                    StartCoroutine(ToggleFacebookPanel(true, 102.581f, -0.0009994507f, 1.5f));
                }
                else
                {
                    StartCoroutine(ToggleFacebookPanel(false, initialFbTop, initialFbBottom, 1.5f));
                }
            }
        }
    }

    private IEnumerator TogglePhone(bool open, float targetY, float duration)
    {
        isMoving = true;
        if (Phone != null)
        {
            // Nếu là mở thì bật gameObject lên trước khi di chuyển
            if (open)
            {
                Iphone.SetActive(true);
            }

            Vector2 startPos = Phone.anchoredPosition;
            Vector2 targetPos = new Vector2(startPos.x, targetY);
            float timeElapsed = 0f;

            while (timeElapsed < duration)
            {
                Phone.anchoredPosition = Vector2.Lerp(startPos, targetPos, timeElapsed / duration);
                timeElapsed += Time.deltaTime;
                yield return null;
            }

            Phone.anchoredPosition = targetPos;

            // Nếu là tắt thì chờ di chuyển xong mới tắt gameObject
            if (!open)
            {
               Iphone.SetActive(false);
            }
        }
        isOpen = open;
        isMoving = false;
    }

    private IEnumerator ToggleFacebookPanel(bool open, float targetTop, float targetBottom, float duration)
    {
        isFbPanelMoving = true;
        if (faceBookPanel != null)
        {
            Vector2 startOffsetMin = faceBookPanel.offsetMin;
            Vector2 startOffsetMax = faceBookPanel.offsetMax;
            
            // target offsetMin: x giữ nguyên, y là targetBottom
            Vector2 targetOffsetMin = new Vector2(startOffsetMin.x, targetBottom);
            // target offsetMax: x giữ nguyên, y là -targetTop
            Vector2 targetOffsetMax = new Vector2(startOffsetMax.x, -targetTop);

            float timeElapsed = 0f;

            while (timeElapsed < duration)
            {
                faceBookPanel.offsetMin = Vector2.Lerp(startOffsetMin, targetOffsetMin, timeElapsed / duration);
                faceBookPanel.offsetMax = Vector2.Lerp(startOffsetMax, targetOffsetMax, timeElapsed / duration);
                timeElapsed += Time.deltaTime;
                yield return null;
            }

            faceBookPanel.offsetMin = targetOffsetMin;
            faceBookPanel.offsetMax = targetOffsetMax;
        }
        isFbPanelOpen = open;
        isFbPanelMoving = false;
    }
}
