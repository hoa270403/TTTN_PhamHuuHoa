using UnityEngine;
using UnityEngine.UI;  // Để làm việc với UI

public class ShowCanvasOnButtonClick : MonoBehaviour
{
    public GameObject canvasToShow;  // Canvas bạn muốn hiển thị
    public Button buttonToClick;     // Nút để nhấn

    void Start()
    {
        // Kiểm tra xem nút có được gán chưa
        if (buttonToClick != null)
        {
            // Gắn sự kiện nhấn nút để gọi hàm ShowCanvas
            buttonToClick.onClick.AddListener(ShowCanvas);
        }
        else
        {
            Debug.LogError("Button is not assigned!");
        }
    }

    void ShowCanvas()
    {
        // Kiểm tra xem Canvas có bị null không và hiển thị Canvas
        if (canvasToShow != null)
        {
            canvasToShow.SetActive(true); // Hiển thị Canvas
        }
        else
        {
            Debug.LogError("Canvas is not assigned!");
        }
    }
}
