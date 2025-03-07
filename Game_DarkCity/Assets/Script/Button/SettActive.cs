using UnityEngine;

public class ActivateOnStart : MonoBehaviour
{
    public GameObject objectToActivate; // Tham chiếu đến đối tượng bạn muốn bật

    // Hàm Start được gọi khi game bắt đầu
    void Start()
    {
        if (objectToActivate != null)
        {
            objectToActivate.SetActive(true); // Bật đối tượng
        }
    }
}
