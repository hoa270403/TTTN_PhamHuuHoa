using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public GameObject monsterPrefab; // Prefab của quái vật
    public Transform player;         // Transform của người chơi
    public float spawnRadius = 10f;  // Bán kính spawn xung quanh người chơi
    public int maxMonsters = 10;     // Số lượng quái tối đa
    public float spawnInterval = 2f; // Thời gian giữa các lần spawn
    public GameObject spawnTrigger;  // GameObject để kích hoạt việc spawn

    private List<GameObject> activeMonsters = new List<GameObject>(); // Danh sách quái hiện tại
    private bool canSpawn = false; // Biến kiểm tra xem có thể spawn quái hay không

    private void Start()
    {
        // Không tự động spawn quái ngay lập tức
        if (spawnTrigger != null)
        {
            spawnTrigger.SetActive(true); // Đảm bảo vùng kích hoạt đang hoạt động
        }
    }

    private void Update()
    {
        // Đồng bộ danh sách quái, loại bỏ quái null
        activeMonsters.RemoveAll(monster => monster == null);
    }

    private void SpawnMonster()
    {
        if (!canSpawn || activeMonsters.Count >= maxMonsters) return; // Chỉ spawn khi có thể

        Vector3 randomPosition = GenerateSpawnPosition();
        GameObject monster = Instantiate(monsterPrefab, randomPosition, Quaternion.identity);

        var enemyScript = monster.GetComponent<EnemyFollowPlayerWithFlip>();
        if (enemyScript != null)
        {
            enemyScript.player = player;
        }

        activeMonsters.Add(monster);

        var monsterScript = monster.GetComponent<Monster>();
        if (monsterScript != null)
        {
            monsterScript.OnMonsterDestroyed += RemoveMonsterFromList;
        }
    }

    private Vector3 GenerateSpawnPosition()
    {
        Vector3 randomPosition;
        int attempts = 0;
        do
        {
            randomPosition = player.position + Random.insideUnitSphere * spawnRadius;
            randomPosition.y = player.position.y;
            randomPosition.z = 0;
            attempts++;
        } while (attempts < 10 && !IsPositionValid(randomPosition));

        return randomPosition;
    }

    private bool IsPositionValid(Vector3 position)
    {
        return !Physics.CheckSphere(position, 0.5f);
    }

    private void RemoveMonsterFromList(GameObject monster)
    {
        activeMonsters.Remove(monster);
    }

    // Khi người chơi chạm vào vùng kích hoạt
    public void StartSpawning()
    {
        if (!canSpawn)
        {
            Debug.Log("Bắt đầu spawn quái vật!");
            canSpawn = true;
            InvokeRepeating(nameof(SpawnMonster), 0f, spawnInterval);
        }
    }
}
