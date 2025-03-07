using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Transform player; // Tham chiếu đến player

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Không phá hủy đối tượng này khi chuyển Scene
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
