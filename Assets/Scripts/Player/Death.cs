using UnityEngine;

public class Death : MonoBehaviour
{
    LevelManager levelManager;
    private void Start()
    {
        levelManager = FindObjectOfType<LevelManager>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Hazard"))
        {
            Die();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("FallDetector"))
        {
            Die();
        }
    }
    private void Die()
    {
        Instantiate(levelManager.deathParticles, transform.position, Quaternion.identity);
        Destroy(gameObject);
        levelManager.ProcessPlayerDeath();
    }
}
