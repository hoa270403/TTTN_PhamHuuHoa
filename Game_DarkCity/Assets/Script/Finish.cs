using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Thêm thư viện UI

public class Finish : MonoBehaviour
{
    private AudioSource finishsound;
    private bool levelcomplete = false;

    public GameObject winCanvas; // Canvas chiến thắng
    public Button nextLevelButton; // Nút Next Level

    private void Start()
    {
        finishsound = GetComponent<AudioSource>();
        winCanvas.SetActive(false); // Ẩn canvas chiến thắng ban đầu
        nextLevelButton.onClick.AddListener(Completelevel); // Gán sự kiện cho nút
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "PLAYER Spineboy" && !levelcomplete)
        {
            
            bool bananasCollected = FindObjectOfType<ItemCollector>().checkBananas();
            if (bananasCollected)
            {
                Debug.Log("da va cham");
                //finishsound.Play();
                levelcomplete = true;
                winCanvas.SetActive(true); // Hiện canvas chiến thắng
                Time.timeScale = 0f; // Dừng game
                // Lưu trạng thái hoàn thành level hiện tại
                int currentLevel = SceneManager.GetActiveScene().buildIndex;
                PlayerPrefs.SetInt("Level" + SceneManager.GetActiveScene().buildIndex, 1);
                PlayerPrefs.Save();
            }
            /*levelcomplete = true;
            winCanvas.SetActive(true); // Hiện canvas chiến thắng
            Time.timeScale = 0f; // Dừng game
            PlayerPrefs.SetInt("Level" + SceneManager.GetActiveScene().buildIndex, 1);
            PlayerPrefs.Save();*/
        }
    }

    private void Completelevel()
    {
        Time.timeScale = 1f; // Tiếp tục game
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
