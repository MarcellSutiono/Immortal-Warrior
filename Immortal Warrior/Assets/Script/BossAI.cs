using UnityEngine;

public class BossAI : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float attackRange = 1.5f;
    public float attackCooldown = 2f;

    private Transform player;
    private float lastAttackTime;
    private Animator animator;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance > attackRange)
        {
            WalkToPlayer();
        }
        else
        {
            TryAttack();
        }
    }

    void WalkToPlayer()
    {
        animator.SetBool("isWalking", true);
        Vector2 direction = (player.position - transform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
    }

    void TryAttack()
    {
        animator.SetBool("isWalking", false);

        if (Time.time - lastAttackTime >= attackCooldown)
        {
            animator.SetTrigger("Attack"); // trigger attack animation
            lastAttackTime = Time.time;
        }
    }
}
