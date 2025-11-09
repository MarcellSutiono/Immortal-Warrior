using UnityEngine;

public enum BossState
{
    Moving,
    BasicAttack,
    Chasing,
    Dead
}

public class Boss : MonoBehaviour, IEntity, IEnemyMoveable
{
    public BossState State { get; set; }

    private float moveTimer = 0;
    private float moveDuration = 0;
    private bool isMoving = false;
    private Vector2 moveVelocity;
    private GameObject player;

    [field: SerializeField] public float MaxHealth { get; set; }
    [field: SerializeField] public float CurrentHealth { get; set; }
    [field: SerializeField] public float Speed { get; set; }
    [field: SerializeField] public float ChaseSpeed { get; set; }
    [field: SerializeField] public float Power { get; set; }

    public Rigidbody2D RB { get; set; }
    [field: SerializeField] public Collider2D ChaseRadius { get; set; }

    protected void Start()
    {
        State = BossState.Chasing;
        player = GameObject.FindGameObjectWithTag("Player");
        RB = GetComponent<Rigidbody2D>();
        CurrentHealth = MaxHealth;
    }

    protected void Update()
    {
        if (CurrentHealth <= 0)
        {
            death();
        }

        if(IsPlayerInChaseRadius())
        {
            State = BossState.Chasing;
        }
        else
        {
            State = BossState.Moving;
        }

        switch (State)
        { 
            case BossState.Moving:
                MoveEnemy();
                break;
            case BossState.Chasing:
                Chase();
                break;
            case BossState.BasicAttack:
                BasicAttack(Power);
                break;
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
    private bool IsPlayerInChaseRadius()
    {
        return ChaseRadius.OverlapPoint(player.transform.position);
    }

    public void Chase()
    {
        if (player == null) return;
        Vector2 diff = (player.transform.position - transform.position).normalized;
        float dir = 0;
        if(diff.x > 0)
        {
            dir = 1;
        }
        else if(diff.x < 0)
        {
            dir = -1;
        }

        RB.linearVelocity = new Vector2(dir * ChaseSpeed, RB.linearVelocity.y);
    }

    public void BasicAttack(float dmg)
    {
        
    }
}
