using UnityEngine;
using System;

public class Monster : MonoBehaviour
{
    public event Action<GameObject> OnMonsterDestroyed; // Sự kiện khi quái bị tiêu diệt

    private void OnDestroy()
    {
        OnMonsterDestroyed?.Invoke(gameObject); // Gọi sự kiện khi quái bị hủy
    }
}
