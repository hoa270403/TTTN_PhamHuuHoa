using UnityEngine;
using UnityEngine.UI;

public class CoinCollector : MonoBehaviour
{
    public int coinValue = 50; // Số xu mỗi đồng tiền
    public Text coinText; // UI Text để hiển thị số xu

    public int totalCoins = 0; // Biến lưu tổng số xu

    void Start()
    {
        // Kiểm tra xem coinText có được gán chưa, nếu chưa thì tìm nó trong scene
        if (coinText == null)
        {
            coinText = GameObject.Find("CoinText").GetComponent<Text>();
        }
        UpdateCoinUI(); // Cập nhật UI lúc bắt đầu
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Kiểm tra xem đối tượng va chạm có tag "Coin"
        if (other.CompareTag("Coin"))
        {
            totalCoins += coinValue; // Cộng xu
            UpdateCoinUI(); // Cập nhật lại UI
            Destroy(other.gameObject); // Xóa đồng xu khỏi scene
        }
    }

    // Cập nhật UI text với số xu mới
    public void UpdateCoinUI()
    {
        coinText.text = "Coins:" + totalCoins.ToString();
    }
}
