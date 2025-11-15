using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public enum BossState
{
    Moving,
    BasicAttack,
    Chasing,
    Dead
}

public class Boss : MonoBehaviour, IEntity, IEnemyMoveable, IAttack
{
    public BossState State { get; set; }

    private float moveTimer = 0;
    private float moveDuration = 0;
    private bool isMoving = false;
    private Vector2 moveVelocity;
    private GameObject player;
    [field: SerializeField] public PlayerData pd { get; set; }
    [field: SerializeField] public float MaxHealth { get; set; }
    [field: SerializeField] public float CurrentHealth { get; set; }
    [field: SerializeField] public float Speed { get; set; }
    [field: SerializeField] public float ChaseSpeed { get; set; }
    [field: SerializeField] public float Power { get; set; }
    [field: SerializeField] public float AttackDelay { get; set; }
    [field: SerializeField] public float AttackCooldown { get; set; }
    [field: SerializeField] public bool CanAttack { get; set; } = true;
    [field: SerializeField] public bool CanMove { get; set; } = true;
    [field: SerializeField] public bool Immunity { get; set; } = false;

    public Rigidbody2D RB { get; set; }
    [field: SerializeField] public Collider2D ChaseRadius { get; set; }
    [field: SerializeField] public Collider2D AttackTriggerRadius { get; set; }
    [field: SerializeField] public Collider2D AttackRadius { get; set; }

    protected void Start()
    {
        State = BossState.Moving;
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
        else if(IsPlayerInAttackTriggerRadius())
        {
            State = BossState.BasicAttack;
        }
        else if(IsPlayerInChaseRadius() && CanMove)
        {
            State = BossState.Chasing;
        }
        else if(CanMove)
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
        if(!Immunity)
        {
            CurrentHealth -= dmg;
            Immunity = true;
            StartCoroutine(ImmuneDelay(2));
        }
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
            int dir = Random.Range(-1, 2);
            switch (dir)
            {
                case -1:
                    moveVelocity = Vector2.left * Speed;
                    FlipSprite(dir);
                    break;
                case 1:
                    moveVelocity = Vector2.right * Speed;
                    FlipSprite(dir);
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

    public void FlipSprite(float dir)
    {
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * dir;
        transform.localScale = scale;
    }

    private bool IsPlayerInChaseRadius()
    {
        return ChaseRadius.OverlapPoint(player.transform.position);
    }

    private bool IsPlayerInAttackTriggerRadius()
    {
        return AttackTriggerRadius.OverlapPoint(player.transform.position);
    }

    private bool IsPlayerInAttackRadius()
    {
        return AttackRadius.OverlapPoint(player.transform.position);
    }

    public void Chase()
    {
        if (player == null || !CanAttack) return;

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

        if (dir != 0)
        {
            FlipSprite(dir);
        }

        RB.linearVelocity = new Vector2(dir * ChaseSpeed, RB.linearVelocity.y);
    }

    public void BasicAttack(float dmg)
    {
        if (CanAttack)
        {
            CanAttack = false;
            CanMove = false;
            StartCoroutine(AttackRoutine(dmg));
        }
    }

    private IEnumerator AttackRoutine(float dmg)
    {
        RB.linearVelocity = Vector2.zero;

        yield return new WaitForSeconds(AttackDelay);

        if(IsPlayerInAttackRadius())
        {
            pd.PlayerHealth -= dmg;
            Debug.Log("Player get damage: " + dmg);
        }

        CanMove = true;

        yield return new WaitForSeconds(AttackCooldown);

        CanAttack = true;
    }

    private IEnumerator ImmuneDelay(float sec)
    {
        yield return new WaitForSeconds(sec);

        Immunity = false;
    }
}
