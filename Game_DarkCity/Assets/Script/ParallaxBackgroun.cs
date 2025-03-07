using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    public Transform player;  // Tham chiếu đến đối tượng người chơi
    public float parallaxEffectMultiplier;  // Hệ số parallax

    private Vector3 lastPlayerPosition;

    void Start()
    {
        // Lưu lại vị trí ban đầu của người chơi
        lastPlayerPosition = player.position;
    }

    void Update()
    {
        // Tính toán sự thay đổi vị trí chỉ theo trục X
        float deltaX = player.position.x - lastPlayerPosition.x;

        // Di chuyển background theo trục X với hệ số parallax
        transform.position += new Vector3(deltaX * parallaxEffectMultiplier, 0, 0);

        // Cập nhật lại vị trí của người chơi
        lastPlayerPosition = player.position;
    }
}
