using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    //--------PLAYER DATA---------
    [SerializeField] private PlayerData pd;

    //--------MOVEMENT---------
    private Vector2 moveInput;

    //--------GAMEOBJECT ---------
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    //--------GROUND CHECK---------
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    //--------ANIMATOR---------
    private Animator anim;


    void Start()
    {
        pd.IsRolling = false;

        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        moveCharacter();
    }

    public void moveValueRead(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            moveInput = ctx.ReadValue<Vector2>();
            if(moveInput.x < 0)
            {
                sr.flipX = true;
            }
            else if(moveInput.x > 0)
            {
                sr.flipX = false;
            }
        }
        else if (ctx.canceled)
        {
            moveInput = Vector2.zero;
        }
    }

    private void moveCharacter()
    {
        Vector2 movement = new Vector2(moveInput.x * pd.PlayerMovement, rb.linearVelocity.y);
        rb.linearVelocity = movement;
    }

    public void jump(InputAction.CallbackContext ctx)
    {
        if (isGround())
        {
            Vector2 jumping = new Vector2(rb.linearVelocity.x, 1f * pd.PlayerJumpForce);
            rb.linearVelocity = jumping;
        }
    }

    public void attackRight(InputAction.CallbackContext ctx)
    {
        sr.flipX = false;
        if (ctx.performed)
        {
            anim.SetTrigger("Attack");
            pd.attackRight = true;
            StartCoroutine(attackDuration());
        }
    }

    public void attackLeft(InputAction.CallbackContext ctx)
    {
        sr.flipX = true;
        if (ctx.performed)
        {
            anim.SetTrigger("Attack");
            pd.attackLeft = true;
            StartCoroutine(attackDuration());
        }
    }

    public void roll(InputAction.CallbackContext ctx)
    {
        if(!pd.IsRolling)
        {
            Debug.Log("rolling");
            StartCoroutine(RollCoroutine());
        }
    }

    private IEnumerator RollCoroutine()
    {
        pd.IsRolling = true;
        gameObject.layer = LayerMask.NameToLayer("PlayerRolling");

        Vector2 dir;
        if (moveInput != Vector2.zero)
        {
            dir = new Vector2(moveInput.x, 0);
        }
        else
        {
            dir = new Vector2(1, 0);
        }

        float startTime = Time.time;
        while (Time.time < startTime + pd.PlayerRollDuration)
        {
            
            rb.linearVelocity = dir * pd.PlayerRollSpeed;
            yield return null;
        }

        rb.linearVelocity = Vector3.zero;
        gameObject.layer = LayerMask.NameToLayer("PlayerDefault");

        yield return new WaitForSeconds(pd.PlayerRollCooldown);
        pd.IsRolling = false;
    }

    private IEnumerator attackDuration()
    {
        yield return new WaitForSeconds(1f);
        pd.attackRight = false;
        pd.attackLeft = false;
    }

    private bool isGround()
    {
        return Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer);
    }
}
