using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public enum BossState
{
    Moving,
    BasicAttack,
    Chasing,
    Jump,
    Dead
}

public class Boss : MonoBehaviour, IEntity, IEnemyMoveable, IAttack
{
    public GameObject door;
    public bool lastBoss = false;
    
    //JUMP
    public bool canJump = false;
    public float jumpPrepareTime;
    private float jumpPrepareTimeCounter;
    private bool isJumping = false;
    public float jumpPower;
    private bool jumpCooldown = false;
    public float jumpCooldownTime;

    public BossState State { get; set; }

    //UI
    public GameObject winUI;
    public GameObject textDamage;

    // MOVE
    private float moveTimer = 0;
    private float moveDuration = 0;
    private bool isMoving = false;
    public Vector2 moveVelocity;
    private GameObject player;
    public GameObject audioObject;
    public AudioManager audioManager;
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
    [field: SerializeField] public Collider2D JumpAttackRadius { get; set; }

    public Animator anim { get; set; }

    protected void Start()
    {
        audioManager = audioObject.GetComponent<AudioManager>();
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
        else if(!isJumping && IsPlayerInAttackTriggerRadius())
        {
            State = BossState.BasicAttack;
        }
        else if(!isJumping && IsPlayerInChaseRadius() && CanMove)
        {
            int chance = Random.Range(1, 101);
            if(chance <= 10 && canJump && !jumpCooldown)
            {
                State = BossState.Jump;
            }
            else if(!isJumping)
            {
                State = BossState.Chasing;
            }
        }
        else if(!isJumping && CanMove)
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
            case BossState.Jump:
                BossJumpPrepareCounter();
                BossJump();
                break;
        }
    }

    private void BossJump()
    {
        if(!isJumping)
        {
            isJumping = true;
            anim.SetTrigger("PrepareJump");
        }

        if(jumpPrepareTimeCounter >= jumpPrepareTime && !jumpCooldown)
        {
            jumpPrepareTimeCounter = 0;
            Vector2 diff = (player.transform.position - transform.position).normalized;
            RB.linearVelocity = new Vector2(diff.x * jumpPower, 5f);
            FlipSprite(diff.x > 0 ? 1 : -1);

            audioManager.playSFX(audioManager.bossJump);

            jumpCooldown = true;
            isJumping = false;

            StartCoroutine(AirDelay());
            StartCoroutine(JumpDelay(jumpCooldownTime));
        }
    }

    private void BossJumpPrepareCounter()
    {
        jumpPrepareTimeCounter += Time.deltaTime;
        Debug.Log(jumpPrepareTimeCounter);
    }

    public virtual void death(){}

    public void TakeDamage(float dmg)
    {
        if(!Immunity)
        {
            CurrentHealth -= dmg;
            Immunity = true;

            Vector2 playerOffset = new Vector2(transform.position.x + Random.Range(-1, 3), transform.position.y);
            GameObject dmgText = Instantiate(textDamage, playerOffset, Quaternion.identity);
            dmgText.transform.rotation = Quaternion.identity;
            dmgText.transform.GetChild(0).GetComponent<TextMeshPro>().text = dmg.ToString();

            StartCoroutine(ImmuneDelay(1.5f));
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
            int dir = Random.Range(1, 101);

            if(dir <= 25)
            {
                moveVelocity = Vector2.right * Speed;
                FlipSprite(1);
            }
            else
            {
                moveVelocity = Vector2.left * Speed;
                FlipSprite(-1);
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
        scale.x = Mathf.Abs(scale.x) * (dir * -1);
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

    private bool IsPlayerInJumpAttackRadius()
    {
        return JumpAttackRadius.OverlapPoint(player.transform.position);
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
            anim.SetTrigger("Attack");
            StartCoroutine(AttackRoutine(dmg));
        }
    }

    private void DamagingPlayer(float dmg)
    {
        if (pd.IsParrying)
        {
            audioManager.playSFX(audioManager.parry);
            if(pd.chargeTime < 3)
            {
                pd.chargeTime += 1.2f;
                pd.PlayerHealth += 5f;

                if(pd.chargeTime >= 3f)
                {
                    pd.chargeTime = 3f;
                }
            }
        }
        else
        {
            pd.PlayerHealth -= dmg;
            Debug.Log("Player get damage: " + dmg);
        }
    }

    private IEnumerator AttackRoutine(float dmg)
    {
        RB.linearVelocity = Vector2.zero;

        yield return new WaitForSeconds(AttackDelay);
        audioManager.playSFX(audioManager.attackBoss);

        if(IsPlayerInAttackRadius())
        {
            DamagingPlayer(dmg);
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

    private IEnumerator JumpDelay(float sec)
    {
        yield return new WaitForSeconds(sec);
        jumpCooldown = false;
    }

    private IEnumerator AirDelay()
    {
        yield return new WaitForSeconds(1.2f);
        audioManager.playSFX(audioManager.jumpAttack);
        if(IsPlayerInJumpAttackRadius())
        {
            DamagingPlayer(Power);
        }
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
