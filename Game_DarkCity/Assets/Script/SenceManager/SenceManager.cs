using UnityEngine;

public class ToggleGameObjects : MonoBehaviour
{
    public GameObject GameObject1; // Đối tượng cần bật/tắt
    public GameObject GameObject2; // Đối tượng cần bật/tắt

    private bool isGameObject1Active; // Trạng thái hiện tại của GameObject1

    private void Start()
    {
        // Lấy trạng thái ban đầu của GameObject1
        if (GameObject1 != null)
        {
            isGameObject1Active = GameObject1.activeSelf;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Kiểm tra nếu đối tượng va chạm có tag "dichuyen"
        if (other.CompareTag("dichuyen1"))
        {
            Debug.Log("kdkfk");
            // Đổi trạng thái của GameObject1 và GameObject2
            isGameObject1Active = !isGameObject1Active;

            if (GameObject1 != null)
                GameObject1.SetActive(isGameObject1Active);

            if (GameObject2 != null)
                GameObject2.SetActive(!isGameObject1Active);
        }
    }
}