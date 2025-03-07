using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class IntroManager : MonoBehaviour
{
    public GameObject startPanel; // Panel chứa nút Play
    public Button playButton; // Nút Play
    public GameObject introPanel;
    public Image introImage; // Hình ảnh
    public TextMeshProUGUI introText; // Chữ
    public Button nextButton; // Nút bấm
    public GameObject levelPanel;
    public Sprite[] images; // Danh sách hình ảnh
    public string[] texts; // Danh sách chữ

    private int currentIndex = 0; // Ảnh hiện tại
    private Coroutine typingCoroutine; // Lưu coroutine hiện tại
    private bool isTyping = false; // Kiểm tra xem chữ đang chạy hay đã xong

    void Start()
    {
        playButton.onClick.AddListener(StartIntro);
        nextButton.onClick.AddListener(NextSlide);

        // Ẩn phần intro khi chưa bấm "Play"
        introPanel.gameObject.SetActive(false);
        introImage.gameObject.SetActive(false);
        introText.gameObject.SetActive(false);
        nextButton.gameObject.SetActive(false);
    }

    void StartIntro()
    {
        startPanel.SetActive(false); // Ẩn màn hình Start
        introPanel.gameObject.SetActive(true);
        introImage.gameObject.SetActive(true);
        introText.gameObject.SetActive(true);
        nextButton.gameObject.SetActive(true);

        ShowSlide();
    }

    void ShowSlide()
    {
        if (currentIndex < images.Length)
        {
            introImage.sprite = images[currentIndex];
            introText.text = ""; // Xóa chữ cũ
            typingCoroutine = StartCoroutine(TypeText(texts[currentIndex]));
        }
        else
        {
            LoadNextScene(); // Khi hết intro, vào màn chơi
        }
    }

    IEnumerator TypeText(string text)
    {
        isTyping = true; // Đánh dấu chữ đang chạy
        introText.text = "";

        foreach (char letter in text)
        {
            introText.text += letter;
            yield return new WaitForSeconds(0.05f); // Tốc độ chạy chữ
        }

        isTyping = false; // Đánh dấu chữ đã hoàn thành
    }

    void NextSlide()
    {
        if (isTyping)
        {
            // Nếu chữ đang chạy, dừng coroutine và hiển thị toàn bộ ngay lập tức
            StopCoroutine(typingCoroutine);
            introText.text = texts[currentIndex]; // Hiện đầy đủ nội dung
            isTyping = false; // Đánh dấu đã hoàn thành
        }
        else
        {
            // Nếu chữ đã hiện đầy đủ, chuyển sang slide tiếp theo
            currentIndex++;
            if (currentIndex < images.Length)
            {
                ShowSlide();
            }
            else
            {
                LoadNextScene();
            }
        }
    }

    void LoadNextScene()
    {
        //SceneManager.LoadScene("GameScene"); // Thay bằng tên scene của bạn
        levelPanel.gameObject.SetActive(true);
        introPanel.gameObject.SetActive(false);
    }
}
