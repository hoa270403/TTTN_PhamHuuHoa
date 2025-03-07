using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    public ItemClass itemData; // Dữ liệu của item
    public float throwForce = 5f; // Lực ném item ra ngoài

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (itemData != null)
        {
            spriteRenderer.sprite = itemData.itemIcon; // Gán icon của item vào SpriteRenderer
        }
        else
        {
            Debug.LogError("Item data is not assigned.");
        }

        // Thêm lực rơi (nếu có)
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.AddForce(Vector2.up * throwForce, ForceMode2D.Impulse);
        }
    }

    // Phương thức để nhặt item
    public void PickupItem(InventoryManager inventoryManager)
    {
        inventoryManager.AddItem(itemData, 1); // Thêm item vào inventory
        Destroy(gameObject); // Xóa item khỏi mặt đất
    }
}
