using UnityEngine;

public class Boss : MonoBehaviour, IEntity, IEnemyMoveable
{
    public float MaxHealth { get; set; }
    public float CurrentHealth { get; set; }
    public Rigidbody2D RB { get; set; }

    void Start()
    {
        CurrentHealth = MaxHealth;
        RB = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (CurrentHealth <= 0)
        {
            death();
        }
    }
    public void death()
    {
        Destroy(gameObject);
    }

    public void TakeDamage(float dmg)
    {
        CurrentHealth -= dmg;
    }

    public void MoveEnemy(Vector2 velocity)
    {
        throw new System.NotImplementedException();
    }

    public void BasicAttack(float dmg)
    {
        throw new System.NotImplementedException();
    }
}
