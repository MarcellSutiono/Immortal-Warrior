using UnityEngine;

public class Boss : MonoBehaviour, IEntity, IEnemyMoveable
{
    private float moveTimer = 0;
    private float moveDuration = 0;
    private bool isMoving = false;
    private Vector2 moveVelocity;
    [field: SerializeField] public float MaxHealth { get; set; }
    [field: SerializeField] public float CurrentHealth { get; set; }
    [field: SerializeField] public float Speed { get; set; }
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

    private void moveDurationCount()
    {
        moveDuration += Time.deltaTime;
    }

    public void MoveEnemy()
    {
        moveTimerCount();

        if(moveTimer >= 3.5f)
        {
            moveTimer = 0;
            isMoving = true;
            int chance = Random.Range(0, 2);
            switch (chance)
            {
                case 0:
                    moveVelocity = Vector2.left * Speed;
                    break;
                case 1:
                    moveVelocity = Vector2.right * Speed;
                    break;
            }
        }

        if(isMoving)
        {
            moveDurationCount();

            float dur = Random.Range(0.7f, 3.5f);

            if (moveDuration >= dur)
            {
                isMoving = false;
                moveDuration = 0;
                moveVelocity = Vector2.zero;
            }
        }

        RB.linearVelocity = new Vector2(moveVelocity.x, RB.linearVelocity.y);
    }

    public void BasicAttack(float dmg)
    {
        
    }

    public void Idle()
    {
        
    }
}
