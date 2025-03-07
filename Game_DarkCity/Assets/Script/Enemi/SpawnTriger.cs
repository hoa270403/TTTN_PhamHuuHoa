using UnityEngine;

public class SpawnTrigger : MonoBehaviour
{
    public MonsterSpawner monsterSpawner; // Tham chiếu đến MonsterSpawner

    private void Start()
    {
        if (monsterSpawner == null)
        {
            Debug.LogError("MonsterSpawner chưa được gán vào SpawnTrigger!");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Chạm vào " + gameObject.name + " bởi " + other.name);

        if (other.CompareTag("Player")) // Kiểm tra nếu Player chạm vào trigger
        {
            Debug.Log("Kích hoạt MonsterSpawner!");

            if (monsterSpawner != null)
            {
                monsterSpawner.StartSpawning();
                gameObject.SetActive(false); // Vô hiệu hóa trigger sau khi kích hoạt
            }
        }
    }
}
