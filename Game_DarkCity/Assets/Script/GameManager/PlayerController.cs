using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Kiểm tra va chạm với đối tượng có tag "dichchuyen"
        if (other.CompareTag("dichchuyen"))
        {
            // Lưu vị trí hiện tại của nhân vật vào GameManager
            GameManager.Instance.player = transform; // Gán player từ transform
            Debug.Log("jdjfjdj");

            // Chuyển Scene
            string currentScene = SceneManager.GetActiveScene().name;
            string targetScene = (currentScene == "Scene2") ? "Scene3" : "Scene2";

            // Tải Scene mới
            SceneManager.LoadScene(targetScene);
        }
    }

    private void Start()
    {
        // Đặt nhân vật vào vị trí lưu trữ khi chuyển Scene
        if (GameManager.Instance != null && GameManager.Instance.player != null)
        {
            transform.position = GameManager.Instance.player.position;
        }
        else
        {
            Debug.LogWarning("Player position is not set in GameManager.");
        }
    }
}
