using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemClass item;  // Lưu thông tin của ItemClass

    // Hàm khởi tạo để thiết lập item mặc định (optional)
    private void Start()
    {
        if (item == null)
        {
            Debug.LogError("Item not assigned on: " + gameObject.name);
        }
    }
}
