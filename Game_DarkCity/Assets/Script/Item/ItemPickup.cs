using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public ItemClass itemToPickUp;  // Item bạn muốn nhặt (thường là một loại ItemClass)
    /*public float pickupRange = 3f;  // Khoảng cách nhặt item

    private void Update()
    {
        // Kiểm tra nếu người chơi nhấn phím E và có item để nhặt
        if (Input.GetKeyDown(KeyCode.E))
        {
            PickupItem();
        }
    }*/
    // Item bạn muốn nhặt (thường là một loại ItemClass)

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Kiểm tra nếu đối tượng va chạm là người chơi
        if (other.CompareTag("Player"))
        {
            PickupItem();
        }
    }

    private void PickupItem()
    {
        // Kiểm tra xem có item để nhặt không
        if (itemToPickUp != null)
        {
            // Thêm item vào inventory của người chơi
            InventoryManager inventoryManager = FindObjectOfType<InventoryManager>();
            inventoryManager.AddItem(itemToPickUp, 1);  // Thêm item vào túi (1 món)
            // Hủy item khỏi cảnh (chỉ hủy vật phẩm, không phải người chơi)
            Destroy(gameObject);  // Hủy đối tượng item
        }
        else
        {
            Debug.LogWarning("Item is not assigned to this object!");
        }
    }
}
