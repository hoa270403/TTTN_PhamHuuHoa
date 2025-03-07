// Import các thư viện cần thiết cho Unity
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// Định nghĩa lớp ItemCollector, kế thừa từ MonoBehaviour
public class ItemCollector : MonoBehaviour
{
    // Biến lưu trữ số lượng chuối đã thu thập
    private int bananas = 0;

    // Biến lưu trữ tổng số chuối cần thu thập trong level hiện tại
    private int totalBananas;

    // Thành phần UI để hiển thị số lượng chuối đã thu thập
    [SerializeField] private Text BananasText;

    // Âm thanh phát ra khi thu thập chuối
    [SerializeField] private AudioSource collectsoundeffect;

    // Chỉ số của level hiện tại
    private int currentLevelIndex;

    // Hàm Start được gọi khi đối tượng khởi tạo
    void Start()
    {
        // Lấy chỉ số của level hiện tại
        currentLevelIndex = SceneManager.GetActiveScene().buildIndex;

        // Tìm và đếm số lượng đối tượng có tag "banana" trong level hiện tại
        totalBananas = GameObject.FindGameObjectsWithTag("key").Length;

        // Cập nhật văn bản hiển thị số lượng chuối đã thu thập
        BananasText.text = "Level " + currentLevelIndex + " - Key: " + bananas + "/" + totalBananas;
    }

    // Hàm OnTriggerEnter2D được gọi khi có va chạm với collider 2D khác
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Kiểm tra xem đối tượng va chạm có tag "banana" không
        if (collision.gameObject.CompareTag("key"))
        {
            // Phát âm thanh thu thập chuối
            collectsoundeffect.Play();

            // Hủy đối tượng chuối đã va chạm
            Destroy(collision.gameObject);

            // Tăng số lượng chuối đã thu thập lên 1
            bananas++;

            // Cập nhật văn bản hiển thị số lượng chuối đã thu thập
            BananasText.text = "Level " + currentLevelIndex + " - Key: " + bananas + "/" + totalBananas;
        }
    }

    // Hàm kiểm tra xem người chơi đã thu thập đủ chuối chưa
    public bool checkBananas()
    {
        // Trả về true nếu số lượng chuối đã thu thập bằng với tổng số chuối trong level
        return bananas == totalBananas;
    }
}
