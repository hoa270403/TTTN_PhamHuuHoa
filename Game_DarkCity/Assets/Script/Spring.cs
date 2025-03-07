using UnityEngine;

public class Spring : MonoBehaviour
{
    [SerializeField] private float springForce = 15f; // Lực của lò xo

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D playerRB = collision.gameObject.GetComponent<Rigidbody2D>();
            if (playerRB != null)
            {
                Vector2 jumpDirection = Vector2.up * springForce;
                playerRB.velocity = jumpDirection;
            }
        }
    }
}
