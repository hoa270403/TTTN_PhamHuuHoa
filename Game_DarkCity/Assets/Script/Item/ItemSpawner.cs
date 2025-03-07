using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] private GameObject itemPrefab; // Prefab của item
    [SerializeField] private Transform spawnPoint;  // Vị trí spawn item
    [SerializeField] private ItemClass itemData;   // ItemClass để gán dữ liệu

    public void SpawnItem()
    {
        // Tạo GameObject mới từ Prefab
        GameObject itemObject = Instantiate(itemPrefab, spawnPoint.position, Quaternion.identity);

        // Gán dữ liệu item cho GameObject mới
        ItemDrop itemDrop = itemObject.GetComponent<ItemDrop>();
        if (itemDrop != null)
        {
            itemDrop.itemData = itemData; // Gán ItemClass vào item mới
        }
    }
}
