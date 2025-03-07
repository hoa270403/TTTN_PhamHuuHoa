using Spine.Unity.Examples;
using System.Collections;
using UnityEngine;
using UnityEngine.UI; // Import UI để sử dụng Image

public class HealthBarSystem : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f; // Máu tối đa
    private float currentHealth;   // Máu hiện tại
    [Header("UI Components")]
    public Image healthBarFill;    // Thanh máu (phần Fill)
    [Header("Healing Settings")]
    public float healInterval = 2f; // Thời gian giữa mỗi lần hồi máu
    public int healAmount = 20;     // Lượng máu hồi mỗi lần
    private bool isHealing = false; // Trạng thái đang hồi máu
    public AudioSource healSound;
    public SpineboyBeginnerModel charac;
    
    void Start()
    {
        // Khởi tạo máu và cập nhật thanh máu
        currentHealth = maxHealth;
        UpdateHealthBar();
    }

    public void TakeDamage(float damage)
    {
        // Giảm máu và đảm bảo không âm
        currentHealth -= damage;
        charac.TryHit();
        if(currentHealth<0) currentHealth= 0;
        UpdateHealthBar();

        // Kiểm tra nếu nhân vật chết
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        if (currentHealth >maxHealth)currentHealth= maxHealth;
        // Hồi máu và đảm bảo không vượt quá giới hạn tối đa
      
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        // Cập nhật thanh máu dựa trên tỉ lệ máu hiện tại
        if (healthBarFill != null)
        {
            
            healthBarFill.fillAmount = currentHealth / maxHealth;
        }
    }

    private void Die()
    {
        
        // Nếu đã chết, không thực hiện gì thê
        Debug.Log($"{gameObject.name} has died!");
        // Thực hiện logic khi nhân vật chết
        GetComponent<SpineboyBeginnerModel>()?.Die();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("health"))
        {
            healSound.Play();
            Debug.Log("Started healing from: " + other.name);
            if (!isHealing)
            {
                StartCoroutine(HealOverTime());
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("health"))
        {
            healSound.Pause();
            Debug.Log("Stopped healing from: " + other.name);
            StopCoroutine(HealOverTime());
            isHealing = false;
        }
    }

    private IEnumerator HealOverTime()
    {
        isHealing = true;
        while (isHealing && currentHealth < maxHealth)
        {
            Heal(healAmount); // Hồi máu
            Debug.Log("Healing... Current Health: " + currentHealth);
            yield return new WaitForSeconds(healInterval); // Đợi thời gian hồi máu tiếp theo
        }
        isHealing = false;
    }

    void Update()
    {
        // Kiểm tra phím H để hồi máu thủ công
        if (Input.GetKeyDown(KeyCode.H))
        {
            Heal(20f);
        }
    }
}
