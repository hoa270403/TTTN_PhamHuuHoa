using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointSpawnEnemi : MonoBehaviour
{
    public GameObject monsterPrefab; // Prefab quái vật
    public Transform player;         // Transform của người chơi
    public List<Transform> spawnPoints; // Danh sách các điểm spawn
    public int maxMonsters = 10;     // Tổng số quái tối đa
    public float spawnInterval = 2f; // Thời gian giữa các lần spawn
    public float spawnRange = 10f;   // Khoảng cách để kích hoạt spawn

    private Dictionary<Transform, int> spawnTracker = new Dictionary<Transform, int>(); // Số quái mỗi điểm spawn
    private int maxPerSpawnPoint; // Số quái tối đa mỗi điểm spawn
    private List<GameObject> activeMonsters = new List<GameObject>(); // Danh sách quái hiện tại

    private void Start()
    {
        if (spawnPoints.Count == 0)
        {
            Debug.LogError("No spawn points available for spawning monsters!");
            return;
        }

        // Xác định số quái tối đa mỗi điểm spawn
        maxPerSpawnPoint = maxMonsters / spawnPoints.Count;

        // Khởi tạo bộ đếm số quái mỗi điểm spawn
        foreach (var point in spawnPoints)
        {
            spawnTracker[point] = 0;
        }

        // Ẩn quái gốc trong Hierarchy nếu có
        if (monsterPrefab != null)
        {
            monsterPrefab.SetActive(false);
        }

        // Lặp việc kiểm tra spawn theo định kỳ
        InvokeRepeating(nameof(SpawnMonsters), 0f, spawnInterval);
    }

    private void SpawnMonsters()
    {
        foreach (var spawnPoint in spawnPoints)
        {
            float distanceToPlayer = Vector3.Distance(player.position, spawnPoint.position);

            // Kiểm tra nếu người chơi ở trong phạm vi spawn và chưa đạt giới hạn
            if (distanceToPlayer <= spawnRange && spawnTracker[spawnPoint] < maxPerSpawnPoint && activeMonsters.Count < maxMonsters)
            {
                SpawnMonsterAtPoint(spawnPoint);
            }
        }
    }

    private void SpawnMonsterAtPoint(Transform spawnPoint)
    {
        // Spawn quái từ prefab
        GameObject monster = Instantiate(monsterPrefab, spawnPoint.position, Quaternion.identity);
        monster.SetActive(true); // Bật quái mới lên

        // Gán player cho script EnemyFollowPlayerWithFlip (nếu có)
        var enemyScript = monster.GetComponent<EnemyFollowPlayerWithFlip>();
        if (enemyScript != null)
        {
            enemyScript.player = player;
        }

        // Thêm vào danh sách quái hiện tại
        activeMonsters.Add(monster);
        spawnTracker[spawnPoint]++; // Tăng số lượng quái của điểm spawn

        // Gắn sự kiện khi quái bị hủy
        var monsterScript = monster.GetComponent<Monster>();
        if (monsterScript != null)
        {
            monsterScript.OnMonsterDestroyed += (destroyedMonster) => RemoveMonsterFromList(destroyedMonster, spawnPoint);
        }
    }

    private void RemoveMonsterFromList(GameObject monster, Transform spawnPoint)
    {
        activeMonsters.Remove(monster);
        if (spawnTracker.ContainsKey(spawnPoint))
        {
            spawnTracker[spawnPoint]--; // Giảm số quái tại điểm spawn này
        }
    }
}
