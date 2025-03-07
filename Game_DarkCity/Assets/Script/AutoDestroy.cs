using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    public float lifetime = 0.01f; // Thời gian tồn tại của hiệu ứng

    void Start()
    {
        Destroy(gameObject, lifetime); // Hủy hiệu ứng sau thời gian quy định
    }
}
