using UnityEngine;

public class SwordHitbox : MonoBehaviour
{
    public int damage = 10;
    private bool canDamage = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (canDamage && other.CompareTag("Player"))
        {
            PlayerHealth health = other.GetComponent<PlayerHealth>();
            if (health != null)
            {
                health.TakeDamage(damage);
            }
        }
    }

    // These will be called from animation events
    public void EnableDamage()
    {
        canDamage = true;
    }

    public void DisableDamage()
    {
        canDamage = false;
    }
}
