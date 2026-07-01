using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManagerr : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject dialogPanel;      // Khung nền chứa đoạn thoại
    public Text dialogText;             // Text (Legacy) để hiển thị chữ
    
    [Header("Dialog Settings")]
    [TextArea(3, 10)]                   // Giúp ô nhập liệu trên Inspector to hơn, dễ nhìn hơn
    public string[] dialogLines;        // Danh sách các câu thoại
    public float typingSpeed = 0.05f;   // Tốc độ hiển thị từng chữ

    private int currentLineIndex = 0;
    private bool isTyping = false;
    private Coroutine typingCoroutine;

    void Start()
    {
        StartDialogue();
    }

    void Update()
    {
        // Kiểm tra nếu người chơi nhấn phím F và cửa sổ thoại đang được hiển thị
        if (Input.GetKeyDown(KeyCode.F) && dialogPanel.activeInHierarchy)
        {
            // Tái sử dụng lại logic của nút Continue
            OnContinueButtonClicked();
        }
    }

    public void StartDialogue()
    {
        // 1. Hiện UI thoại lên
        dialogPanel.SetActive(true);
        // 2. Bắt đầu từ câu đầu tiên (index = 0)
        currentLineIndex = 0;
        // 3. Chạy hiệu ứng gõ chữ
        StartTypewriter();
    }

    // Gắn hàm này vào sự kiện OnClick() của nút Continue trên giao diện
    public void OnContinueButtonClicked()
    {
        // Tình huống 1: Chữ vẫn đang chạy lạch cạch
        if (isTyping)
        {
            // Dừng việc chạy chữ
            StopCoroutine(typingCoroutine);
            // Hiển thị ngay lập tức TOÀN BỘ câu thoại hiện tại
            dialogText.text = dialogLines[currentLineIndex];
            isTyping = false;
        }
        // Tình huống 2: Chữ đã hiện ra đầy đủ rồi
        else
        {
            // Chuyển sang câu tiếp theo
            NextLine();
        }
    }

    private void NextLine()
    {
        currentLineIndex++;

        // Kiểm tra xem còn câu thoại nào không
        if (currentLineIndex < dialogLines.Length)
        {
            StartTypewriter();
        }
        else
        {
            EndDialogue();
        }
    }

    private void StartTypewriter()
    {
        // Xóa nội dung cũ trước khi hiển thị nội dung mới
        dialogText.text = "";
        typingCoroutine = StartCoroutine(TypeLine(dialogLines[currentLineIndex]));
    }

    private IEnumerator TypeLine(string line)
    {
        isTyping = true;
        
        // Tách câu thành từng chữ cái và hiển thị dần
        foreach (char c in line.ToCharArray())
        {
            dialogText.text += c;
            // Chờ một khoảng thời gian (typingSpeed) rồi mới chạy vòng lặp tiếp theo
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    private void EndDialogue()
    {
        // Tắt cửa sổ thoại đi
        dialogPanel.SetActive(false);
    }
}
