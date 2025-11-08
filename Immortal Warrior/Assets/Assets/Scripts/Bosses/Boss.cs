using UnityEngine;

public class Boss : MonoBehaviour, IEntity, IEnemyMoveable
{
    private float moveTimer = 0;
    private Vector2 moveVelocity;
    public float MaxHealth { get; set; }
    public float CurrentHealth { get; set; }
    public float Speed { get; set; }
    public Rigidbody2D RB { get; set; }

    protected void Start()
    {
        CurrentHealth = MaxHealth;
        RB = GetComponent<Rigidbody2D>();
    }

    protected void Update()
    {
        if (CurrentHealth <= 0)
        {
            death();
        }

        MoveEnemy();
    }

    public void death()
    {
        Destroy(gameObject);
    }

    public void TakeDamage(float dmg)
    {
        CurrentHealth -= dmg;
    }

    private void moveTimerCount()
    {
        moveTimer += Time.deltaTime;
    }

    public void MoveEnemy()
    {
        moveTimerCount();
        if (moveTimer >= 3)
        {
            moveTimer = 0;
            int chance = Random.Range(0, 3);
            switch (chance)
            {
                case 0:
                    moveVelocity = Vector2.zero;
                    break;
                case 1:
                    moveVelocity = Vector2.left * Speed;
                    break;
                case 2:
                    moveVelocity = Vector2.right * Speed;
                    break;
            }
        }
        RB.linearVelocity = moveVelocity;
    }

    public void BasicAttack(float dmg)
    {
        
    }

    public void Idle()
    {
        
    }
}
