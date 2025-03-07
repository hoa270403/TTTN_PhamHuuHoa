using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSText : MonoBehaviour
{
    public Text fpsText; // Tham chiếu đến Text UI để hiển thị FPS
    private float deltaTime = 0.0f; // Lưu trữ thời gian giữa các frame

    void Update()
    {
        // Cập nhật thời gian giữa các frame
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;

        // Tính FPS từ thời gian giữa các frame
        float fps = 1.0f / deltaTime;

        // Cập nhật Text UI với FPS
        fpsText.text = string.Format("FPS: {0:0}", fps);
    }
}
